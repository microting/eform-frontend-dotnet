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

            Core core = getCore();

            ViewBag.sites = core.Advanced_SiteItemReadAll();

            return View();
        }

        public ActionResult Edit(int id)
        {
            Core core = getCore();

            ViewBag.site = core.Advanced_SiteItemRead(id);
            return View();
        }

        public ActionResult Update(int id)
        {
            try
            {
                Core core = getCore();
                var site = core.Advanced_SiteItemRead(id);
                core.Advanced_SiteItemUpdate((int)site.SiteUId, Request.Form.Get("site['name']"));

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
                var site = core.Advanced_SiteItemRead(id);
                if (core.Advanced_SiteItemDelete(id))
                {
                    JObject response = JObject.FromObject(new
                    {
                        data = new
                        {
                            status = "success",
                            message = $"Site \"{site.SiteName}\" deleted successfully",
                            id = site.SiteUId,
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
                            message = $"Site \"{site.SiteName}\" could not be deleted!",
                            id = site.SiteUId,
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