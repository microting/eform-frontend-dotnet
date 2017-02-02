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

            var db = new Models.CheckList(connectionStr);
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
            return View();
        }       

        public FileResult Csv(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            Core core = new Core();

            core.HandleCaseCreated += EventCaseCreated;
            core.HandleCaseRetrived += EventCaseRetrived;
            core.HandleCaseCompleted += EventCaseCompleted;
            core.HandleCaseDeleted += EventCaseDeleted;
            core.HandleFileDownloaded += EventFileDownloaded;
            core.HandleSiteActivated += EventSiteActivated;
            core.HandleEventLog += EventLog;
            core.HandleEventMessage += EventMessage;
            core.HandleEventWarning += EventWarning;
            core.HandleEventException += EventException;
            core.StartSqlOnly(connectionStr);

            string file_name = $"{id}_{DateTime.Now.Ticks}.csv";
            System.IO.Directory.CreateDirectory(Server.MapPath("~/bin/output/"));
            string file_path = Server.MapPath($"~/bin/output/{file_name}");
            core.CasesToCsv(id, null, null, file_path);

            return File(file_path, "text/csv", file_name);
        }

        #region events
        public void EventCaseCreated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseCompleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventLog(object sender, EventArgs args)
        {
            lock (_lockLogFil)
            {
                try
                {
                    System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), sender.ToString() + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    EventException(ex, EventArgs.Empty);
                }
            }
        }

        public void EventMessage(object sender, EventArgs args)
        {
            lock (_lockLogFil)
            {
                try
                {
                    System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), sender.ToString() + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    EventException(ex, EventArgs.Empty);
                }
            }
        }

        public void EventWarning(object sender, EventArgs args)
        {
            lock (_lockLogFil)
            {
                try
                {
                    System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), "## WARNING ## " + sender.ToString() + " ## WARNING ##" + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    EventException(ex, EventArgs.Empty);
                }
            }
        }

        public void EventException(object sender, EventArgs args)
        {
            lock (_lockLogFil)
            {
                try
                {
                    System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), sender.ToString() + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    EventException(ex, EventArgs.Empty);
                }
            }
        }
        #endregion
    }
}