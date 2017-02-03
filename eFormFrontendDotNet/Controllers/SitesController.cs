using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eFormFrontendDotNet.Controllers
{
    public class SitesController : Controller
    {
        // GET: Site
        public ActionResult Index()
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            using (var db = new Models.Site(connectionStr))
            {
                try
                {
                    ViewBag.sites = db.sites.ToList();
                    return View();
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), ex.ToString() + Environment.NewLine);
                }
            }

            return View();
        }

        public ActionResult Edit(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            using (var db = new Models.Site(connectionStr))
            {
                try
                {
                    ViewBag.site = db.sites.Single(x => x.id == id);
                    ViewBag.site_id = id;
                    return View();
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), ex.ToString() + Environment.NewLine);
                }
            }
            return View();
        }

        public ActionResult Update(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            using (var db = new Models.Site(connectionStr))
            {
                try
                {
                    Models.sites site = db.sites.Single(x => x.id == id);

                    site.name = Request.Form.Get("site['name']");
                   
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), ex.ToString() + Environment.NewLine);
                }
            }
            return RedirectToAction("Index");
        }
    }
}