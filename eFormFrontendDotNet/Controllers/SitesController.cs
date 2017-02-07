﻿// The MIT License(MIT)
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

using eFormCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eFormFrontendDotNet.Controllers
{
    public class SitesController : ApplicationController
    {
        // GET: Site
        public ActionResult Index()
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            using (var db = new Models.Site(connectionStr))
            {
                try
                {
                    ViewBag.sites = db.sites.Where(x => x.workflow_state != "removed").ToList();
                    return View();
                }
                catch (Exception ex)
                {
                }
            }

            return View();
        }

        public ActionResult Edit(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            using (var db = new Models.Site(connectionStr))
            {
                try
                {
                    ViewBag.site = db.sites.Single(x => x.microting_uid == id);
                    ViewBag.site_id = id;
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
                Core core = getCore();
                var site = core.SiteRead(id);
                core.SiteUpdate((int)site.MicrotingUid, Request.Form.Get("site['name']"));

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
                var site = core.SiteRead(id);
                if (core.SiteDelete(id))
                {
                    JObject response = JObject.FromObject(new
                    {
                        data = new
                        {
                            status = "success",
                            message = $"Site \"{site.Name}\" deleted successfully",
                            id = site.MicrotingUid,
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
                            message = $"Site \"{site.Name}\" could not be deleted!",
                            id = site.MicrotingUid,
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
                        message = $"Site with id \"{id}\" could not be deleted!",
                        id = id,
                        value = ""
                    }
                });
                return Json(response.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}