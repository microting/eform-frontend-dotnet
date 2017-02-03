using System;
using System.Linq;
using System.Web.Mvc;
using Microting;
using System.Collections.Generic;

namespace eFormFrontendDotNet.Controllers
{
    public class TemplatesController : Controller
    {
        object _lockLogFil = new object();
        Core core = null;

        // GET: Template
        public ActionResult Index()
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            List<Models.check_lists> check_lists = null;

            var db = new Models.CheckList(connectionStr);
            try
            {
                check_lists = db.check_lists.Where(x => x.parent_id == 0 && x.workflow_state != "removed").ToList();
                ViewBag.check_lists = check_lists;
                return View();
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), ex.ToString() + Environment.NewLine);
                return RedirectToAction("Index");
            }
        }       

        public FileResult Csv(int id)
        {
            Core core = getCore();

            string file_name = $"{id}_{DateTime.Now.Ticks}.csv";
            System.IO.Directory.CreateDirectory(Server.MapPath("~/bin/output/"));
            string file_path = Server.MapPath($"~/bin/output/{file_name}");
            core.CasesToCsv(id, null, null, file_path);

            return File(file_path, "text/csv", file_name);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create()
        {
            string tamplate_xml = Request.Form.Get("eFormXML");

            Core core = getCore();
            eFormRequest.MainElement new_template =  core.TemplatFromXml(tamplate_xml);
            if (new_template != null)
            {
                core.TemplatCreate(new_template);
            }
            Models.DataResponse response = new Models.DataResponse();
            response.data = new Models.DataResponse.Data($"eForm \"{new_template.Label}\" created successfully", "success");

            return Json(response);
        }

        public JsonResult Delete(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            Models.check_lists check_list = null;
            Models.DataResponse response = new Models.DataResponse();
            response.model_id = id.ToString();

            var db = new Models.CheckList(connectionStr);
            try
            {
                check_list = db.check_lists.Single(x => x.id == id);
                check_list.Remove();
                db.SaveChanges();
                response.data = new Models.DataResponse.Data($"eForm \"{check_list.label}\" deleted successfully", "success");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), ex.ToString() + Environment.NewLine);
                response.data = new Models.DataResponse.Data($"eForm \"{check_list.label}\" could not be deleted!", "error");
            }                               

            return Json(response);
        }

        #region coreFunctions
        private Core getCore()
        {

            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            this.core = new Core();

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

            return core;
        }
        #endregion

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