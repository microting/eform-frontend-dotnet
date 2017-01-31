using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microting;

namespace eFormFrontendDotNet.Controllers
{
    public class CaseController : Controller
    {
        object _lockLogFil = new object();
        public ActionResult Index(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            List<Models.cases> cases = null;

            using (var db = new Models.Case(connectionStr))
            {
                try
                {
                    cases = db.cases.Where(x => x.check_list_id == id).ToList();
                    ViewBag.cases = cases;
                    return View();
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), ex.ToString() + Environment.NewLine);
                }
            }
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

            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            using (var db = new Models.Field(connectionStr))
            {
                try
                {
                    foreach (string key in keys)
                    {
                        if (key.Contains("cl_"))
                        {

                            checkListValueList.Add(key.Replace("cl_v[", "").Replace("]", "") + "|" + Request.Form.Get(keys[i]));
                        }
                        else
                        {
                            int _field_id = int.Parse(key.Replace("f_[", "").Replace("]", ""));
                            Models.fields field = db.fields.SingleOrDefault(x => x.id == _field_id);
                            if (field.field_type_id == 10)
                            {
                                fieldValueList.Add(key.Replace("f_[", "").Replace("]", "") + "|" + Request.Form.Get(keys[i]).Replace(",", "|"));
                            } else
                            {
                                fieldValueList.Add(key.Replace("f_[", "").Replace("]", "") + "|" + Request.Form.Get(keys[i]));
                            }                            
                        }
                        i += 1;
                    }
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), ex.ToString() + Environment.NewLine);
                }
            }

            try
            {
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
                core.CaseUpdate(int.Parse(id), fieldValueList);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), ex.ToString() + Environment.NewLine);
            }
            return View();
        }

        public ActionResult Edit(int id)
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
            try
            {
                string microting_uuid = null;
                string microting_check_uid = null;
                using (var db = new Models.Case(connectionStr))
                {
                    Models.cases my_case = db.cases.SingleOrDefault(x => x.id == id);
                    microting_uuid = my_case.microting_uid;
                    microting_check_uid = my_case.microting_check_uid;
                }
                var theCase = core.CaseRead(microting_uuid, microting_check_uid);

                ViewBag.theCase = theCase;
                ViewBag.case_id = id;
                return View();
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(Server.MapPath("~/bin/log/log.txt"), ex.ToString() + Environment.NewLine);
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