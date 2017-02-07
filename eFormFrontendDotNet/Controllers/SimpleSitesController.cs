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
using eFormCore;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

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
                    //core.SiteLoadAllFromRemote();
                    //core.WorkerLoadAllFromRemote();
                    //core.SiteWorkerLoadAllFromRemote();
                    //core.UnitLoadAllFromRemote();
                }
                ViewBag.sites = db.sites.Where(x => x.workflow_state != "removed").ToList();
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

            string userFirstName = Request.Form.Get("worker['first_name']");
            string userLastName = Request.Form.Get("worker['last_name']");
            string siteName = userFirstName + " " + userLastName;

            eFormShared.Simple_Site_Dto site = core.SiteCreateSimple(siteName, userFirstName, userLastName, null);

            if (site != null)
            {
                JObject response = JObject.FromObject(new
                {
                    data = new
                    {
                        status = "error",
                        message = $"Worker \"{site.Name}\" created successfully",
                        id = site.MicrotingUid,
                        value = ""
                    }
                });
                return Json(response.ToString(), JsonRequestBehavior.AllowGet);
            } else
            {
                JObject response = JObject.FromObject(new
                {
                    data = new
                    {
                        status = "error",
                        message = $"Worker could not be created!",
                        id = 0,
                        value = ""
                    }
                });
                return Json(response.ToString(), JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult Edit(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            using (var db = new Models.Worker(connectionStr))
            {
                try
                {
                    ViewBag.worker = db.workers.Single(x => x.microting_uid == id);
                    ViewBag.worker_id = id;
                    return View();
                }
                catch (Exception ex)
                {
                }
            }
            return View();
        }

        public ActionResult Update(int id)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));
                string connectionStr = lines.First();
                var db = new Models.Worker(connectionStr);
                Models.workers worker = db.workers.Single(x => x.microting_uid == id);
                Models.sites site = worker.site_workers.First().site;

                Core core = getCore();
                
                string userFirstName = Request.Form.Get("worker['first_name']");
                string userLastName = Request.Form.Get("worker['last_name']");
                bool result = core.WorkerUpdate(id, userFirstName, userLastName, worker.email);
                if (result)
                {
                    result = core.SiteUpdate((int)site.microting_uid, userFirstName + " " + userLastName);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public JsonResult Delete(int id)
        {
            //Models.DataResponse response = new Models.DataResponse();
            //response.model_id = id.ToString();

            try
            {
                string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

                string connectionStr = lines.First();


                var db = new Models.Site(connectionStr);
                Models.sites site = db.sites.Single(x => x.microting_uid == id);               
                Core core = getCore();
                Models.workers worker = null;
                foreach (Models.site_workers site_worker in site.site_workers.Where(x => x.workflow_state != "removed").ToList())
                {
                    core.SiteWorkerDelete((int)site_worker.microting_uid);
                    worker = site_worker.worker;
                    core.WorkerDelete((int)worker.microting_uid);
                }
                if (core.SiteDelete(id))
                {
                    JObject response = JObject.FromObject(new
                    {
                        data = new
                        {
                            status = "success",
                            message = $"Worker \"{worker.full_name()}\" deleted successfully",
                            id = id,
                            value = ""
                        }
                    });
                    return Json(response.ToString(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    JObject response = JObject.FromObject(new
                    {
                        data = new
                        {
                            status = "error",
                            message = $"Worker \"{worker.full_name()}\" could not be deleted!",
                            id = id,
                            value = ""
                        }
                    });
                    return Json(response.ToString(), JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                JObject response = JObject.FromObject(new
                {
                    data = new
                    {
                        status = "error",
                        message = $"Worker with id \"{id}\" could not be deleted!",
                        id = id,
                        value = ""
                    }
                });
                return Json(response.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult OtpCode(int id)
        {
            ViewBag.unit_id = id;
            return View();
        }

    }
}