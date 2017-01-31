using System;
using System.Linq;
using System.Web.Mvc;
using Microting;
using System.Collections.Generic;

namespace eFormFrontendDotNet.Controllers
{
    public class TemplateController : Controller
    {
        object _lockLogFil = new object();

        // GET: Template
        public ActionResult Index()
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            List<Models.check_lists> check_lists = null;

            using (var db = new Models.CheckList(connectionStr))
            {
                try
                {
                    check_lists = db.check_lists.Where(x => x.parent_id == 0).ToList();
                    ViewBag.check_lists = check_lists;
                    return View();
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), ex.ToString() + Environment.NewLine);
                }
            }
            return View();
        }       
    }
}