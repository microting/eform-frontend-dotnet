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

            Core core = getCore();

            ViewBag.sites = core.SiteReadAll(false);            

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

            eFormShared.Site_Dto site = core.SiteCreate(siteName, userFirstName, userLastName, null);

            if (site != null)
            {
                JObject response = JObject.FromObject(new
                {
                    data = new
                    {
                        status = "success",
                        message = $"Worker \"{site.SiteName}\" created successfully",
                        id = site.SiteId,
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
            Core core = getCore();
            ViewBag.site_id = id;
            ViewBag.simpleSite = core.SiteRead(id);               

            return View();
        }

        public ActionResult Update(int id)
        {
            try
            {
                Core core = getCore();
                eFormShared.Site_Dto siteDto = core.SiteRead(id);
                eFormShared.Worker_Dto workerDto = core.Advanced_WorkerRead((int)siteDto.WorkerUid);
                string userFirstName = Request.Form.Get("worker['first_name']");
                string userLastName = Request.Form.Get("worker['last_name']");

                string fullName = userFirstName + " " + userLastName;
                core.SiteUpdate(id, fullName, userFirstName, userLastName, workerDto.Email);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public JsonResult Delete(int id)
        {

            try
            {
             
                Core core = getCore();
                eFormShared.SiteName_Dto siteNameDto = core.Advanced_SiteItemRead(id);

                bool result3 = core.SiteDelete(siteNameDto.SiteUId);
                if (result3)
                {
                    JObject response = JObject.FromObject(new
                    {
                        data = new
                        {
                            status = "success",
                            message = $"Worker \"{siteNameDto.SiteName}\" deleted successfully",
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
                            message = $"Worker \"{siteNameDto.SiteName}\" could not be deleted!",
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