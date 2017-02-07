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
using Microting;
using System.Collections.Generic;
using System;

namespace eFormFrontendDotNet.Controllers
{
    public class TemplatesController : ApplicationController
    {
        public ActionResult Index()
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
                return RedirectToAction("Index");
            }
        }       

        public FileResult Csv(int id)
        {
            Core core = getCore();

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
            Models.DataResponse response = new Models.DataResponse();
            Core core = getCore();
            eFormRequest.MainElement new_template =  core.TemplatFromXml(tamplate_xml);
            if (new_template != null)
            {
                core.TemplatCreate(new_template);
                response.data = new Models.DataResponse.Data($"eForm \"{new_template.Label}\" created successfully", "success");
            }
            response.data = new Models.DataResponse.Data($"eForm could not be created!", "error");


            return Json(response);
        }

        public JsonResult Delete(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            Models.check_lists check_list = null;
            Models.DataResponse response = new Models.DataResponse();
            response.model_id = id.ToString();

            var db = new Models.CheckList(connectionStr);
            try
            {
                check_list = db.check_lists.Single(x => x.id == id);
                Core core = getCore();
                if (core.TemplateDelete(id))
                {
                    response.data = new Models.DataResponse.Data($"eForm \"{check_list.label}\" deleted successfully", "success");
                } else
                {
                    response.data = new Models.DataResponse.Data($"eForm \"{check_list.label}\" could not be deleted!", "error");
                }
                
            }
            catch (Exception ex)
            {
                response.data = new Models.DataResponse.Data($"eForm \"{check_list.label}\" could not be deleted!", "error");
            }                               

            return Json(response);
        }
    }
}