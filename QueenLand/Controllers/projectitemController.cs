using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueenLand.Models;
using Newtonsoft.Json;
namespace QueenLand.Controllers
{
    public class projectitemController : Controller
    {
        private queenlandEntities db = new queenlandEntities();

        //
        // GET: /projectitem/

        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            return View(db.projectitems.OrderBy(o=>o.projectid).ThenBy(o=>o.id).ToList());
        }
        public ActionResult List(int id)
        {
            //Du an moi
            try
            {
                ViewBag.menuleft = Config.getProjectMenu();
                var p = (from q1 in db.projectitems
                         where q1.id == id
                         join q in db.projectcontents on q1.id equals q.itemid
                         select new
                         {
                             id = q.id,
                             title = q.title,
                             image = q.image,                            
                             project_name = q1.itemname,
                             project_id = q1.id,
                             project_pr_id=q.projectid,
                         }
                         ).OrderByDescending(o => o.id);
                var prs = p.ToList();
                string des = "";
                string projects = "";
                string products = "";// "<div class=\"item\" style=\"width:100%;display:block;position:relative;float:left;background-color:#FFCA08;\"><table width=\"100%\" align=center><tr><td align=center>";//<table width=\"100%\"><tr>
                for (int j = 0; j < prs.Count; j++)
                {
                    if (prs[j].project_name != projects)
                    {
                        products += "<div style=\"background:#74b709;color:#ffffff;height:40px;float:left;position:relative;width:100%;text-align:left;padding-top:10px;padding-left:10px;\" style=\"color:white;font-weight:bold;\">" + prs[j].project_name.ToUpperInvariant() + "</div>";
                        projects = prs[j].project_name;
                    }
                    string link = Config.domain + Config.unicodeToNoMark(prs[j].title) + "-" + prs[j].id;
                    products += "<div class=\"col-sm-3\" style=\"height: 290px;\"><p style=\"height: 47px;\"><a href=\"" + link + "\">" + prs[j].title + "</a></p><p><a href=\"" + link + "\"><img src=\"" + prs[j].image + "\" style=\"width:173px;height:225px;\"></a></p></div>";
                    des += prs[j].title + ", ";
                }

                ViewBag.products = products;
                ViewBag.des = des;
                ViewBag.image = Config.domain + prs[0].image;
                var project_parent_name = db.projects.Find(prs[0].project_pr_id).name;
                ViewBag.url = Config.domain + "danh-sach-san-pham-con/" + Config.unicodeToNoMark(project_parent_name) + "/" + Config.unicodeToNoMark(prs[0].project_name) + "-" + id;
                ViewBag.title = prs[0].project_name;
                ViewBag.keywords = des;
                var fullcontent = db.projectitems.Find(id).fullcontent;
                ViewBag.fullcontent = fullcontent;
            }
            catch (Exception ex2)
            {
            }
            return View();
        }
        //
        // GET: /projectitem/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectitem projectitem = db.projectitems.Find(id);
            if (projectitem == null)
            {
                return HttpNotFound();
            }
            return View(projectitem);
        }

        //
        // GET: /projectitem/Create

        public ActionResult Create()
        {
            //var p = (from q in db.projects select q).OrderByDescending(o => o.id).Take(1000);
            //string projects="<select class=\"form-control\" name=\"provin\" id=\"provin\">";
            //var rs = p.ToList();
            //for (int i = 0; i < rs.Count; i++) { 
            //}
            //projects += "</select>";
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            return View();
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string UploadImageProcessContent(HttpPostedFileBase file, string filename)
        {
            string physicalPath = HttpContext.Server.MapPath("../" + Config.ProductCatImagePath + "\\");
            string nameFile = String.Format("{0}.jpg", filename + "-" + Config.genCode());
            int countFile = Request.Files.Count;
            string fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
            string content = "";
            for (int i = 0; i < countFile; i++)
            {
                nameFile = String.Format("{0}.jpg", filename + "-" + Guid.NewGuid().ToString());
                fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
                content += "<img src=\"" + Config.ProductCatImagePath + "/" + nameFile + "\" width=200 height=126>";
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
        public string getProjectName(int id) {
            var p = db.projects.Find(id);
            return p.name;
        }
        public string getProjectItemName(int id)
        {
            var p = db.projectitems.Find(id);
            return p.itemname;
        }
        public string getListProject() { 
            var p = (from q in db.projects select q).OrderBy(o=>o.no).ThenByDescending(o => o.id).Take(100);
            return JsonConvert.SerializeObject( p.ToList());
        }
        public string getListProjectItem(int projectid) {
            var p = (from q in db.projectitems where q.projectid == projectid select q).OrderBy(o => o.id).Take(100);
            return JsonConvert.SerializeObject(p.ToList());
        }
        
        //
        // POST: /projectitem/Create

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(projectitem projectitem)
        {
            if (ModelState.IsValid)
            {
                db.projectitems.Add(projectitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectitem);
        }

        //
        // GET: /projectitem/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectitem projectitem = db.projectitems.Find(id);
            if (projectitem == null)
            {
                return HttpNotFound();
            }
            return View(projectitem);
        }

        //
        // POST: /projectitem/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(projectitem projectitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectitem);
        }

        //
        // GET: /projectitem/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectitem projectitem = db.projectitems.Find(id);
            if (projectitem == null)
            {
                return HttpNotFound();
            }
            return View(projectitem);
        }

        //
        // POST: /projectitem/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            projectitem projectitem = db.projectitems.Find(id);
            db.projectitems.Remove(projectitem);
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