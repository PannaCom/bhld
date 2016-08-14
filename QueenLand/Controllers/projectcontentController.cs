using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueenLand.Models;
using PagedList;
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
            //string ok = resizeImage(Config.imgWidthNews, Config.imgHeightNews, fullPath, Config.NewsImagePath + "/" + nameFile);
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
                content += "<img src=\"" + Config.ProjectImagePath + "/" + nameFile + "\" width=200 height=126>";
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                Request.Files[i].SaveAs(fullPath);
                //break;
            }
            //string ok = resizeImage(Config.imgWidthNews, Config.imgHeightNews, fullPath, Config.NewsImagePath + "/" + nameFile);
            //return Config.NewsImagePath + "/" + nameFile;
            return content;
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