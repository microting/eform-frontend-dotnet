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

using System.Linq;
using System.Web.Mvc;
using eFormCore;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;

namespace eFormFrontendDotNet.Controllers
{
    public class TemplatesController : ApplicationController
    {
        public ActionResult Index()
        {
            try
            {
                try
                {
                    Core core = getCore();
                    ViewBag.templates = core.TemplateItemReadAll(false);
                    return View();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("PrimeDb"))
                    {
                        string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

                        string connectionStr = lines.First();
                        AdminTools adminTool = new AdminTools(connectionStr, false);
                        adminTool.DbSettingsReloadRemote();
                        return Redirect("Templates");
                    }
                    else
                    {
                        if (ex.InnerException.Message.Contains("Cannot open database"))
                        {
                            try
                            {
                                Core core = getCore();
                            }
                            catch (Exception ex2)
                            {

                            }
                            return Redirect("Settings");
                        }
                    }                    
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return Redirect("Settings/ConnectionMissing");
            }

        }

        public FileResult Csv(int id)
        {
            Core core = getCore();

            string file_name = $"{id}_{DateTime.Now.Ticks}.csv";
            System.IO.Directory.CreateDirectory(Server.MapPath("~/output/"));
            string file_path = Server.MapPath($"~/bin/output/{file_name}");
            core.CasesToCsv(id, null, null, file_path, string.Format("{0}://{1}{2}",Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + "output/dataFolder/");

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

            eFormData.MainElement newTemplate = core.TemplateFromXml(tamplate_xml);
            newTemplate = core.TemplateUploadData(newTemplate);
            List<string> errors = core.TemplateValidation(newTemplate);
            if (errors.Count() == 0)
            {
                if (newTemplate != null)
                {
                    core.TemplateCreate(newTemplate);
                    JObject response = JObject.FromObject(new
                    {
                        data = new
                        {
                            status = "success",
                            message = $"eForm \"{newTemplate.Label}\" created successfully",
                            id = newTemplate.Id,
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
                            message = $"eForm could not be created!",
                            id = newTemplate.Id,
                            value = ""
                        }
                    });
                    return Json(response.ToString(), JsonRequestBehavior.AllowGet);
                }
            } else
            {
                string message = "";
                foreach (string str in errors)
                {
                    message += "<br>" + str;
                }
                JObject response = JObject.FromObject(new
                {
                    data = new
                    {
                        status = "error",
                        message = $"eForm could not be created!" + message,
                        id = newTemplate.Id,
                        value = ""
                    }
                });
                return Json(response.ToString(), JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult Delete(int id)
        {

            Core core = getCore();
            eFormShared.Template_Dto templateDto = core.TemplateItemRead(id);
            try
            {
                if (core.TemplateDelete(id))
                {
                    JObject response = JObject.FromObject(new
                    {
                        data = new
                        {
                            status = "success",
                            message = $"eForm \"{templateDto.Label}\" deleted successfully",
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
                            message = $"eForm \"{templateDto.Label}\" could not be deleted!",
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
                        message = $"eForm \"{templateDto.Label}\" could not be deleted!",
                        id = id,
                        value = ""
                    }
                });
                return Json(response.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeployTo(int id)
        {

            Core core = getCore();
            eFormShared.Template_Dto templateDto = core.TemplateItemRead(id);

            ViewBag.template = templateDto;

            ViewBag.sites = core.Advanced_SiteItemReadAll();

            return View();
        }

        public JsonResult Deploy(int id)
        {
            List<int> deployedSiteIds = new List<int>();
            List<int> requestedSiteIds = new List<int>();

            List<int> sitesToBeRetractedFrom = new List<int>();
            List<int> sitesToBeDeployedTo = new List<int>();

            Core core = getCore();
            eFormShared.Template_Dto templateDto = core.TemplateItemRead(id);

            foreach (eFormShared.SiteName_Dto site in templateDto.DeployedSites)
            {
                deployedSiteIds.Add((int)site.SiteUId);
            }

            int i = 0;
            var keys = Request.Form.AllKeys;
            foreach (string key in keys)
            {
                int site_id = int.Parse(Request.Form.Get(keys[i]));
                requestedSiteIds.Add(site_id);
                i++;
            }

            if (requestedSiteIds.Count == 0)
            {
                foreach (eFormShared.SiteName_Dto site in templateDto.DeployedSites)
                {
                    sitesToBeRetractedFrom.Add(site.SiteUId);
                }
            }
            else
            {
                foreach (int siteId in requestedSiteIds)
                {
                    if (!deployedSiteIds.Contains(siteId))
                    {
                        sitesToBeDeployedTo.Add(siteId);
                    }
                }
            }
            if (deployedSiteIds.Count != 0)
            {
                foreach (eFormShared.SiteName_Dto site in templateDto.DeployedSites)
                {
                    if (!requestedSiteIds.Contains((int)site.SiteUId))
                    {
                        if (!sitesToBeRetractedFrom.Contains(site.SiteUId))
                        {
                            sitesToBeRetractedFrom.Add(site.SiteUId);
                        }                        
                    }
                }
            }
            
            if (sitesToBeDeployedTo.Count() > 0)
            {
                eFormData.MainElement mainElement = core.TemplateRead(id);
                mainElement.Repeated = 0; // We set this right now hardcoded, this will let the eForm be deployed until end date or we actively retract it.
                mainElement.EndDate = DateTime.Now.AddYears(10);
                mainElement.StartDate = DateTime.Now;
                core.CaseCreate(mainElement, "", sitesToBeDeployedTo, "", true);
            }

            foreach (int siteUId in sitesToBeRetractedFrom)
            {
                core.CaseDelete(id, siteUId);
            }
            
            JObject response = JObject.FromObject(new
            {
                data = new
                {
                    status = "success",
                    message = $"eForm \"{templateDto.Label}\" deployed successfully!",
                    id = id,
                    value = ""
                }
            });
            return Json(response.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}