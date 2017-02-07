using eFormCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eFormFrontendDotNet.Controllers
{
    public class ApplicationController : Controller
    {
        object _lockLogFil = new object();
        Core core = null;

        #region core
        #region coreFunctions
        public Core getCore()
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
        #endregion
    }
}