using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueenLand.Models;
using PagedList;
using System.Drawing;
using System.Drawing.Drawing2D;
using ImageProcessor.Web;
using ImageProcessor.Processors;
using ImageProcessor.Imaging;
namespace QueenLand.Controllers
{
    public class projectcontentController : Controller
    {
        private queenlandEntities db = new queenlandEntities();

        //
        // GET: /projectcontent/

        public ActionResult Index(string name, int? page)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            if (name == null) name = "";
            ViewBag.name = name;
            var p = (from q in db.projectcontents where q.title.Contains(name) select q).OrderByDescending(o=>o.id).Take(100);
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(p.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /projectcontent/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectcontent projectcontent = db.projectcontents.Find(id);
            if (projectcontent == null)
            {
                return HttpNotFound();
            }
            return View(projectcontent);
        }
        public ActionResult SinglePage(int id) {
           
            try
            {
                //Tìm ra item menu đầu tiên của Project ấy
                int minItemId = id;//(int)db.projectitems.Where(o => o.projectid == id).Min(o => o.id);
                //Lấy ra content của nó để hiển thị
                var content = db.projectcontents.Where(o => o.id == minItemId).FirstOrDefault();
                ViewBag.content = content.fullcontent;
                ViewBag.des = content.des;
                //Tìm ra menuitem này thuộc project nào;
                var parentPr = db.projects.Where(o => o.id == content.projectid).FirstOrDefault();
                ViewBag.image = Config.domain + content.image;
                ViewBag.url = Config.domain + Config.unicodeToNoMark(content.title) + "-" + id;
                ViewBag.title = content.title;
                ViewBag.keywords = content.title + ", " + Config.unicodeToNoMark(content.title).Replace("-"," ") + ", " + parentPr.name;
                ViewBag.catname = parentPr.name;
                ViewBag.date_time = content.date_time;

                var p = (from q1 in db.projects
                         where q1.id == parentPr.id 
                         join q in db.projectcontents on q1.id equals q.projectid 
                         select new
                         {
                             id = q.id,
                             title = q.title,
                             image = q.image,
                             project_name = q1.name,
                             project_id = q1.id,
                         }
                        ).OrderByDescending(o => o.id);
                var prs = p.ToList();
                //string des = "";
                string projects = "";
                string products = "";// "<div class=\"item\" style=\"width:100%;display:block;position:relative;float:left;background-color:#FFCA08;\"><table width=\"100%\" align=center><tr><td align=center>";//<table width=\"100%\"><tr>
                for (int j = 0; j < prs.Count; j++)
                {
                    if (prs[j].id == id) continue;
                    if (prs[j].project_name != projects)
                    {
                        products += "<div style=\"background:#74b709;color:#ffffff;height:40px;float:left;position:relative;width:100%;text-align:left;padding-top:10px;padding-left:10px;\" style=\"color:white;font-weight:bold;\">SẢN PHẨM CÙNG LOẠI</div>";
                        projects = prs[j].project_name;
                    }
                    string link = Config.domain + Config.unicodeToNoMark(prs[j].title) + "-" + prs[j].id;
                    products += "<div class=\"col-sm-3\" style=\"height: 290px;\"><p style=\"height: 47px;\"><a href=\"" + link + "\">" + prs[j].title + "</a></p><p><a href=\"" + link + "\"><img src=\"" + prs[j].image + "\" style=\"width:173px;height:225px;\"></a></p></div>";
                    //des += prs[j].title + ", ";
                }
                ViewBag.products = products;
            }
            catch (Exception ex)
            {
            }
            try
            {
                ViewBag.menuleft = Config.getProjectMenu();
            }
            catch (Exception ex2) { 
            }
            return View();
        }
        //
        // GET: /projectcontent/Create

        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            return View();
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string UploadImageProcess(HttpPostedFileBase file, string filename)
        {
            string physicalPath = HttpContext.Server.MapPath("../" + Config.ProductImagePath + "\\");
            string nameFile = String.Format("{0}.jpg", filename + "-" + Config.genCode());
            int countFile = Request.Files.Count;
            string fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
            for (int i = 0; i < countFile; i++)
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                Request.Files[i].SaveAs(fullPath);
                break;
            }
            string ok = resizeImage(Config.imgWidthNews, Config.imgHeightNews, physicalPath, nameFile);
            return Config.ProductImagePath + "/" + nameFile;
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string UploadImageProcessContent(HttpPostedFileBase file, string filename)
        {
            string physicalPath = HttpContext.Server.MapPath("../" + Config.ProductImagePath + "\\");
            string nameFile = String.Format("{0}.jpg", filename + "-" + Config.genCode());
            int countFile = Request.Files.Count;
            string fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
            string content = "";
            for (int i = 0; i < countFile; i++)
            {
                nameFile = String.Format("{0}.jpg", filename + "-" + Guid.NewGuid().ToString());
                fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
                content += "<img src=\"" + Config.ProductImagePath + "/" + nameFile + "\" width=200 height=126>";
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                Request.Files[i].SaveAs(fullPath);
                //break;
            }
            //string ok = resizeImage(Config.imgWidthNews, Config.imgHeightNews, physicalPath, nameFile);
            //return Config.NewsImagePath + "/" + nameFile;
            return content;
        }
        public string test()
        {
            return "ok";
            var p = (from q in db.projectcontents select q).ToList();
            for (int i = 0; i < p.Count;i++)
            {
                string physicalPath = HttpContext.Server.MapPath("../" + Config.ProductImagePath + "\\");
                string nameFile = p[i].image.Replace("/Images/Products/", "");
                //return resizeImage(Config.imgWidthProduct, Config.imgHeightProduct, physicalPath + nameFile, Config.ProductImagePath + "/" + nameFile);
                ImageProcessor.ImageFactory iFF = new ImageProcessor.ImageFactory();
                ////Tạo ra file thumbail không có watermark
                Size size1 = new Size(Config.imgWidthProduct, Config.imgHeightProduct);
                iFF.Load(physicalPath + nameFile).Resize(size1).BackgroundColor(Color.WhiteSmoke).Save(physicalPath + nameFile);
                //iFF.Load(physicalPath + nameFile).co(Color.White).Resize(size1).Save(physicalPath + nameFile);
            }
            return "ok";
        }
        public string resizeImage(int maxWidth, int maxHeight, string fullPath, string path)
        {
            string physicalPath = fullPath;
            string nameFile = path;
            //return resizeImage(Config.imgWidthProduct, Config.imgHeightProduct, physicalPath + nameFile, Config.ProductImagePath + "/" + nameFile);
            ImageProcessor.ImageFactory iFF = new ImageProcessor.ImageFactory();
            ////Tạo ra file thumbail không có watermark
            Size size1 = new Size(Config.imgWidthProduct, Config.imgHeightProduct);
            iFF.Load(physicalPath + nameFile).BackgroundColor(Color.WhiteSmoke).Resize(size1).Save(physicalPath + nameFile);
            return "ok";
            //var image = System.Drawing.Image.FromFile(fullPath);
            //var ratioX = (double)maxWidth / image.Width;
            //var ratioY = (double)maxHeight / image.Height;
            //var ratio = Math.Min(ratioX, ratioY);
            //var newWidth = (int)(image.Width * ratioX);
            //var newHeight = (int)(image.Height * ratioY);
            //var newImage = new Bitmap(newWidth, newHeight);
            //Graphics thumbGraph = Graphics.FromImage(newImage);

            //thumbGraph.CompositingQuality = CompositingQuality.HighSpeed;
            //thumbGraph.SmoothingMode = SmoothingMode.HighSpeed;
           
            ////thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);
            //image.Dispose();

            //string fileRelativePath = path;// "newsizeimages/" + maxWidth + Path.GetFileName(path);
            //newImage.Save(HttpContext.Server.MapPath(fileRelativePath), newImage.RawFormat);
            //return fileRelativePath;
        }
        //
        // POST: /projectcontent/Create

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(projectcontent projectcontent)
        {
            if (ModelState.IsValid)
            {
                projectcontent.date_time = DateTime.Now;
                db.projectcontents.Add(projectcontent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectcontent);
        }

        //
        // GET: /projectcontent/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectcontent projectcontent = db.projectcontents.Find(id);
            if (projectcontent == null)
            {
                return HttpNotFound();
            }
            return View(projectcontent);
        }

        //
        // POST: /projectcontent/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(projectcontent projectcontent)
        {
            if (ModelState.IsValid)
            {
                projectcontent.date_time = DateTime.Now;
                db.Entry(projectcontent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectcontent);
        }

        //
        // GET: /projectcontent/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectcontent projectcontent = db.projectcontents.Find(id);
            if (projectcontent == null)
            {
                return HttpNotFound();
            }
            return View(projectcontent);
        }

        //
        // POST: /projectcontent/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            projectcontent projectcontent = db.projectcontents.Find(id);
            db.projectcontents.Remove(projectcontent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}