using QueenLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace QueenLand.Controllers
{
    public class HomeController : Controller
    {
        private queenlandEntities db = new queenlandEntities();
        public ActionResult Index()
        {
            try
            {
                var hp = (from sl in db.slides select sl).OrderBy(o => o.no).ThenByDescending(o => o.id).Take(20);
                var rshp = hp.ToList();
               
                string slide = "";
                for (int h = 0; h < rshp.Count; h++)
                {
                    slide += "<li data-transition=\"fade\" data-slotamount=\"7\" data-masterspeed=\"1500\">";
                    slide += "<img src=\"" + Config.domain + "/" + rshp[h].image + "\" style=\"opacity:0;\" alt=\"slidebg1\"  data-bgfit=\"cover\" data-bgposition=\"left bottom\" data-bgrepeat=\"no-repeat\">";
                    //slide += " <div class=\"textlink\"><span style=\"text-shadow: 0 0 5px #0026ff, 0 0 7px #0026ff;margin: 10 10 10 10;color:#fff;display: block;font-size:18px;line-height: 1;-webkit-margin-before: 0.67em;-webkit-margin-after: 0.67em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;font-weight: bold;\">" + rshp[h].caption + "</span><b><a href=\"" + rshp[h].link + "\" style=\"color:#fff;font-weight:bold;font-size:12px;\">" + rshp[h].linktext + "</a></b></div>";
                    slide += " <div class=\"caption sft revolution-starhotel smalltext\" data-x=\"35\" data-y=\"30\" data-speed=\"800\" data-start=\"1700\" data-easing=\"easeOutBack\">";
                    slide += " <span style=\"text-shadow: 0 0 25px #000, 0 0 27px #000;margin: 10px 10px;color:#fff;display: block;font-size:28px;line-height: 1;-webkit-margin-before: 0.67em;-webkit-margin-after: 0.67em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;font-weight:bold;background: rgb(200, 15, 15) none repeat scroll 0% 0%;border-radius: 25px;padding:10px 10px 10px 10px;\">" + rshp[h].slogan + "</span><br>";
                    slide += " </div></li>";
                }
                ViewBag.slide = slide;
                ViewBag.news = (from q in db.news select q).OrderByDescending(o => o.id).Take(3).ToList();
            }
            catch (Exception ex)
            {
            }
            //Du an moi
            try
            {
                var p = (from q in db.projectcontents
                         join q1 in db.projects on q.projectid equals q1.id
                         select new
                         {
                             id = q.id,
                             title = q.title,
                             image = q.image,
                             project_name = q1.name,
                             project_id=q1.id,
                         }
                         ).OrderBy(o => o.project_name).ThenByDescending(o => o.id);
                var prs = p.ToList();
                string projects = "";
                string link = "";
                string products = "";// "<div class=\"item\" style=\"width:100%;display:block;position:relative;float:left;background-color:#FFCA08;\"><table width=\"100%\" align=center><tr><td align=center>";//<table width=\"100%\"><tr>
                for (int j = 0; j < prs.Count; j++)
                {
                    if (prs[j].project_name != projects) {
                        products += "<div style=\"background:#74b709;color:#ffffff;height:40px;float:left;position:relative;width:100%;text-align:left;padding-top:10px;padding-left:10px;\"><a href=\"/danh-sach-san-pham/" + Config.unicodeToNoMark(prs[j].project_name) + "-" + prs[j].project_id + "\" style=\"color:white;\">" + prs[j].project_name + "</a></div>";
                        projects = prs[j].project_name;
                    }
                    link = Config.domain +Config.unicodeToNoMark(prs[j].title) + "-" + prs[j].id;
                    products += "<div class=\"col-sm-3\"><p><a href=\"" + link + "\">" + prs[j].title + "</a></p><p><a href=\"" + link + "\"><img src=\"" + prs[j].image + "\" style=\"width:173px;height:255px;\"></a></p></div>";
                }
                
                ViewBag.products = products;
            }
            catch (Exception ex2)
            {
            }
            ////Tin tuc
            //try
            //{
            //    var p2 = (from q in db.news select q).OrderByDescending(o => o.id).Take(6);
            //    var prs2 = p2.ToList();
            //    string news = "";
            //    string link="";
            //    for (int j = 0; j < prs2.Count; j++)
            //    {
            //        link = "/news/details/"+Config.unicodeToNoMark(prs2[j].title)+"-"+prs2[j].id;
            //        ///hotel/" + Config.unicodeToNoMark(prs[j].name) + "-" + ViewBag.fromdate + "-" + ViewBag.todate + "-" + prs[j].id + "
            //        //news += "<div class=\"col-sm-4 single\" style=\"height:345px;\">";
            //        //news +=" <div ><a href=\""+link+"\"><img src=\""+Config.domain+prs2[j].image+"\" alt=\""+prs2[j].title+"\" class=\"img-responsive\" /></a>";
            //        //news +="  <div class=\"mask\">";			
            //        //news +="   <div class=\"main\">";
            //        //news += "      <a href=\"" + link + "\"><b>" + prs2[j].title + "</b></a>";
            //        //news += "      <p>" + prs2[j].des + "</p>";
            //        //news +="    </div>";
            //        //news +=" </div>";
            //        //news +=" </div>";
            //        //news +=" </div>";
            //        news +="<div style=\"margin-bottom:25px;display:block;position:relative;float:left;\">"; 
            //        news +="<a href=\""+link+"\" class=\"mask\" style=\"display:block;position:relative;float:left;\"><img src=\""+Config.domain+prs2[j].image+"\" alt=\""+prs2[j].title+"\" class=\"imagenewslist\" style=\"border:1px solid #808080\"></a>";
            //        news += "<div class=\"row rowimagenewslist\">";
            //        news +="<div class=\"col-sm-11 col-xs-10 meta\">";
            //        news +="<h2 style=\"font-size:22px;margin-top:0px;\"><a href=\""+link+"\">"+ prs2[j].title+"</a></h2>";
            //        news +="<p style=\"color: #999;\">&nbsp;Gửi ngày "+prs2[j].datetime+"</p>";
            //        news +="<p class=\"intro\">"+prs2[j].des+"</p>";
            //        news +="</div>";
            //        news +="</div>";
            //        news +="</div>";

            //    }
            //    ViewBag.news = news;
            //}
            //catch (Exception ex2)
            //{
            //}
            return View();
        }

        public ActionResult About()
        {
            
            try
            {
                ViewBag.menuleft = Config.getProjectMenu();
                var p = db.abouts.FirstOrDefault().fullcontent;
                ViewBag.content = p;
            }
            catch (Exception ex) { 
                
            }
            return View();
        }

        public ActionResult Contact()
        {
             try
            {
                ViewBag.menuleft = Config.getProjectMenu();
                var p = db.contacts.FirstOrDefault().fullcontent;
                ViewBag.content = p;
            }
            catch (Exception ex) { 
                
            }
             return View();

        }
        public string generateSiteMap()
        {

            try
            {

                XmlWriterSettings settings = null;
                string xmlDoc = null;
                
                settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                xmlDoc = HttpRuntime.AppDomainAppPath + "sitemap.xml";//HttpContext.Server.MapPath("../") + 
                float percent = 0.85f;

                string urllink = "";
                using (XmlTextWriter writer = new XmlTextWriter(xmlDoc, Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("urlset");
                    writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://baoholaodongdonganh.com");
                    writer.WriteElementString("changefreq", "always");
                    writer.WriteElementString("priority", "1");
                    writer.WriteEndElement();
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://baoholaodongdonganh.com/tin");
                    writer.WriteElementString("changefreq", "always");
                    writer.WriteElementString("priority", "0.98");
                    writer.WriteEndElement();

                    var p0 = (from q in db.projectcontents select q).OrderByDescending(o=>o.id).ToList();
                    for (int i = 0; i < p0.Count; i++)
                    {
                        try
                        {
                          
                            writer.WriteStartElement("url");
                            urllink = "http://baoholaodongdonganh.com/" + Config.unicodeToNoMark(p0[i].title) + "-" + p0[i].id;
                            writer.WriteElementString("loc", urllink);
                            writer.WriteElementString("changefreq", "hourly");
                            percent = 0.95f;
                            writer.WriteElementString("priority", percent.ToString("0.00"));
                            writer.WriteEndElement();
                        }
                        catch (Exception ex2)
                        {
                        }
                    }
                    var p = (from q in db.projects select q).ToList();
                    for (int i = 0; i < p.Count; i++)
                    {
                        try
                        {
                            writer.WriteStartElement("url");
                            urllink = "http://baoholaodongdonganh.com/danh-sach-san-pham/" + Config.unicodeToNoMark(p[i].name) + "-" + p[i].id;
                            writer.WriteElementString("loc", urllink);
                            writer.WriteElementString("changefreq", "hourly");
                            percent = 0.85f;
                            writer.WriteElementString("priority", percent.ToString("0.00"));
                            writer.WriteEndElement();
                        }
                        catch (Exception ex2)
                        {
                        }
                    }
                    var p2 = (from q in db.projectitems select q).ToList();
                    for (int i = 0; i < p2.Count; i++)
                    {
                        try
                        {
                            var project_parent_name = db.projects.Find(p2[i].projectid).name;
                            writer.WriteStartElement("url");
                            urllink = "http://baoholaodongdonganh.com/danh-sach-san-pham-con/" + Config.unicodeToNoMark(project_parent_name) + "/" + Config.unicodeToNoMark(p2[i].itemname) + "-" + p2[i].id;
                            writer.WriteElementString("loc", urllink);
                            writer.WriteElementString("changefreq", "hourly");
                            percent = 0.75f;
                            writer.WriteElementString("priority", percent.ToString("0.00"));
                            writer.WriteEndElement();
                        }
                        catch (Exception ex2)
                        {
                        }
                    }
                    var p3 = (from q in db.news select q).OrderByDescending(o=>o.id).ToList();
                    for (int i = 0; i < p3.Count; i++)
                    {
                        try
                        {
                           
                            writer.WriteStartElement("url");
                            urllink = "http://baoholaodongdonganh.com/tin/" + Config.unicodeToNoMark(p3[i].title) +"-"+ p3[i].id;
                            writer.WriteElementString("loc", urllink);
                            writer.WriteElementString("changefreq", "hourly");
                            percent = 0.70f;
                            writer.WriteElementString("priority", percent.ToString("0.00"));
                            writer.WriteEndElement();
                        }
                        catch (Exception ex2)
                        {
                        }
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

            }
            catch (Exception extry)
            {
                //StreamWriter sw = new StreamWriter();
            }
            return "ok";
        }
    }
}
