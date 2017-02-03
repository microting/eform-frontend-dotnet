using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eFormFrontendDotNet.Controllers
{
    public class WorkersController : Controller
    {
        // GET: Site
        public ActionResult Index()
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            using (var db = new Models.Worker(connectionStr))
            {
                try
                {
                    ViewBag.workers = db.workers.ToList();
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

            using (var db = new Models.Worker(connectionStr))
            {
                try
                {
                    ViewBag.worker = db.workers.Single(x => x.id == id);
                    ViewBag.worker_id = id;
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

            using (var db = new Models.Worker(connectionStr))
            {
                try
                {
                    Models.workers worker = db.workers.Single(x => x.id == id);

                    worker.first_name = Request.Form.Get("worker['first_name']");
                    worker.last_name = Request.Form.Get("worker['last_name']");                    
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