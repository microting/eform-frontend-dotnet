using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microting;
using eFormRequest;

namespace eFormFrontendDotNet.Controllers
{
    public class CaseController : Controller
    {
        object _lockLogFil = new object();
        public ActionResult Index(String check_list_id)
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(string id)
        {
            var keys = Request.Form.AllKeys;
            List<string> checkListValueList = new List<string>();
            List<string> fieldValueList = new List<string>();
            int i = 0;

            foreach (string key in keys)
            {
                if (key.Contains("cl_"))
                {
                    
                    checkListValueList.Add(key.Replace("cl_v[", "").Replace("]", "") + "|" + Request.Form.Get(keys[i]));
                } else
                {
                    fieldValueList.Add(key.Replace("f_[", "").Replace("]", "") + "|" + Request.Form.Get(keys[i]));
                }
                i += 1;
            }
            return View();
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
               
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string serverConnectionString = lines.First();

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
            core.StartSqlOnly(serverConnectionString);
            try
            {
                var theCase = core.CaseRead(id, null);

                ViewBag.theCase = theCase;
                return View();
            } catch
            {
                return RedirectToAction("Index");
            }
            
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