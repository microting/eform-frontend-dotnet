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
                string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

                string connectionStr = lines.First();
                List<Models.check_lists> check_lists = null;

                var db = new Models.CheckList(connectionStr);
                try
                {
                    check_lists = db.check_lists.Where(x => x.parent_id == 0 && x.workflow_state != "removed").ToList();
                    ViewBag.check_lists = check_lists;
                    return View();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("Cannot open database"))
                    {
                        try
                        {
                            Core core = getCore(false);
                        }
                        catch (Exception ex2)
                        {

                        }
                        return Redirect("Settings");
                    }
                    return RedirectToAction("Index");
                }
            } catch (Exception ex)
            {
                return Redirect("Settings/ConnectionMissing");
            }
            
        }

        public FileResult Csv(int id)
        {
            Core core = getCore(false);

            string file_name = $"{id}_{DateTime.Now.Ticks}.csv";
            System.IO.Directory.CreateDirectory(Server.MapPath("~/bin/output/"));
            string file_path = Server.MapPath($"~/bin/output/{file_name}");
            core.CasesToCsv(id, null, null, file_path);

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
            //Models.DataResponse response = new Models.DataResponse();
            Core core = getCore(false);
            eFormRequest.MainElement new_template = core.TemplatFromXml(tamplate_xml);
            if (new_template != null)
            {
                core.TemplatCreate(new_template);
                JObject response = JObject.FromObject(new
                {
                    data = new
                    {
                        status = "success",
                        message = $"eForm \"{new_template.Label}\" created successfully",
                        id = new_template.Id,
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
                        id = new_template.Id,
                        value = ""
                    }
                });
                return Json(response.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            Models.check_lists check_list = null;
            //Models.DataResponse response = new Models.DataResponse();
            //response.model_id = id.ToString();

            var db = new Models.CheckList(connectionStr);
            try
            {
                check_list = db.check_lists.Single(x => x.id == id);
                Core core = getCore(false);
                if (core.TemplateDelete(id))
                {
                    JObject response = JObject.FromObject(new
                    {
                        data = new
                        {
                            status = "error",
                            message = $"eForm \"{check_list.label}\" deleted successfully",
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
                            message = $"eForm \"{check_list.label}\" could not be deleted!",
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
                        message = $"eForm \"{check_list.label}\" could not be deleted!",
                        id = id,
                        value = ""
                    }
                });
                return Json(response.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeployTo(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));
            string connectionStr = lines.First();

            var cl_db = new Models.CheckList(connectionStr);

            Models.check_lists check_list = cl_db.check_lists.Single(x => x.id == id);

            ViewBag.check_list = check_list;
            ViewBag.check_list_sites = check_list.check_list_sites.ToList();

            var db = new Models.Site(connectionStr);

            ViewBag.sites = db.sites.ToList();

            return View();
        }

        public JsonResult Deploy(int id)
        {
            List<int> deployedSiteIds = new List<int>();
            List<int> requestedSiteIds = new List<int>();

            List<int> sitesToBeRetractedFrom = new List<int>();
            List<int> sitesToBeDeployedTo = new List<int>();

            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));
            string connectionStr = lines.First();

            var cl_db = new Models.CheckList(connectionStr);
            Models.check_lists check_list = cl_db.check_lists.Single(x => x.id == id);
            List<Models.check_list_sites> checkListSites = check_list.check_list_sites.ToList();
            foreach (Models.check_list_sites cls in checkListSites)
            {
                deployedSiteIds.Add((int)cls.site_id);
            }

            var site_db = new Models.Site(connectionStr);
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
                foreach (Models.check_list_sites cls in checkListSites)
                {
                    //int _siteId = (int)site_db.sites.Single(x => x.id == siteId).microting_uid;
                    int mUid = int.Parse(cls.microting_uid);
                    sitesToBeRetractedFrom.Add(mUid);
                }
            } else
            {
                foreach (int siteId in requestedSiteIds)
                {
                    if (!deployedSiteIds.Contains(siteId))
                    {
                        int _siteId = (int)site_db.sites.Single(x => x.id == siteId).microting_uid;
                        sitesToBeDeployedTo.Add(_siteId);
                    }
                }
            }
            if (deployedSiteIds.Count != 0)
            {
                //foreach (int siteId in deployedSiteIds)
                foreach (Models.check_list_sites cls in checkListSites)
                {
                    if (!requestedSiteIds.Contains((int)cls.site_id))
                    {
                        //int _siteId = (int)site_db.sites.Single(x => x.id == siteId).microting_uid;
                        int mUid = int.Parse(cls.microting_uid);
                        sitesToBeRetractedFrom.Add(mUid);
                    }
                }
            }

            Core core = getCore(false);

            eFormRequest.MainElement mainElement = core.TemplatRead(id);
            mainElement.Repeated = 0; // We set this right now hardcoded, this will let the eForm be deployed until end date or we actively retract it.
            mainElement.EndDate = DateTime.Now.AddYears(10);
            mainElement.StartDate = DateTime.Now;
            core.CaseCreate(mainElement, "", sitesToBeDeployedTo, "", true);

            foreach (int mUid in sitesToBeRetractedFrom)
            {
                core.CaseDelete(mUid.ToString());
            }
            

            JObject response = JObject.FromObject(new
            {
                data = new
                {
                    status = "error",
                    message = $"eForm \"{check_list.label}\" deployed successfully!",
                    id = id,
                    value = ""
                }
            });
            return Json(response.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}