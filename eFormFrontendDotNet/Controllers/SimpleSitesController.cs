// The MIT License(MIT)
//
// Copyright(c) 2007-2017 microting
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microting;
using System.Web.Mvc;

namespace eFormFrontendDotNet.Controllers
{
    public class SimpleSitesController : ApplicationController
    {

        public ActionResult Index()
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            var db = new Models.Site(connectionStr);            
            try
            {
                if (db.sites.Count() < 1)
                {
                    Core core = getCore();
                    core.SiteLoadAllFromRemote();
                    core.WorkerLoadAllFromRemote();
                    core.SiteWorkerLoadAllFromRemote();
                    core.UnitLoadAllFromRemote();
                }
                ViewBag.sites = db.sites.ToList();
                return View();
            }
            catch (Exception ex)
            {
            }
            

            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create()
        {
            Core core = getCore();
            Models.DataResponse response = new Models.DataResponse();

            string userFirstName  = Request.Form.Get("first_name");
            string userLastName = Request.Form.Get("last_name");
            string siteName = userFirstName + " " + userLastName;

            eFormShared.Simple_Site_Dto site = core.SiteCreateSimple(siteName, userFirstName, userLastName, null);

            if (site != null)
            {
                response.data = new Models.DataResponse.Data($"Worker \"{site.Name}\" created successfully", "success", "");
            } else
            {
                response.data = new Models.DataResponse.Data($"Worker could not be created!", "error", "");
            }

            return Json(response);
        }
    }
}