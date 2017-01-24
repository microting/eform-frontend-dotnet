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
        // GET: Case
        public ActionResult Index(String check_list_id)
        //public String Index()
        {
            return View();
            //return "hej";
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(String id)
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

        public ActionResult Edit(String id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string serverConnectionString = lines[0];

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
            ////DOSOMETHING: changed to fit your wishes and needs 
            //Case_Dto temp = (Case_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs 
            //Case_Dto temp = (Case_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
        }

        public void EventCaseCompleted(object sender, EventArgs args)
        {
            //lock (_lockLogic)
            //{
            //    try
            //    {
            //        Case_Dto trigger = (Case_Dto)sender;
            //        int siteId = trigger.SiteId;
            //        string caseType = trigger.CaseType;
            //        string caseUid = trigger.CaseUId;
            //        string mUId = trigger.MicrotingUId;
            //        string checkUId = trigger.CheckUId;
            //    }
            //    catch
            //    {

            //    }
            //}
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            //Case_Dto temp = (Case_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs 
            //File_Dto temp = (File_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
            //string fileLocation = temp.FileLocation;
        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs 
            //int siteId = int.Parse(sender.ToString());
        }

        public void EventLog(object sender, EventArgs args)
        {
            //lock (_lockLogFil)
            //{
            //    try
            //    {
            //        //DOSOMETHING: changed to fit your wishes and needs 
            //        File.AppendAllText(@"log.txt", sender.ToString() + Environment.NewLine);
            //    }
            //    catch (Exception ex)
            //    {
            //        EventException(ex, EventArgs.Empty);
            //    }
            //}
        }

        public void EventMessage(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Console.WriteLine(sender.ToString());
        }

        public void EventWarning(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Console.WriteLine("## WARNING ## " + sender.ToString() + " ## WARNING ##");
        }

        public void EventException(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Exception ex = (Exception)sender;
        }
        #endregion
    }


}