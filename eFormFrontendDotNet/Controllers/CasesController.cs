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
using System.Web.Mvc;
using eFormCore;

namespace eFormFrontendDotNet.Controllers
{
    public class CasesController : ApplicationController
    {
        public ActionResult Index(int id)
        {

            try
            {
                Core core = getCore();
                ViewBag.cases = core.CaseReadAll(id, null, null);
                return View();
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(int id)
        {
            var keys = Request.Form.AllKeys;
            List<string> checkListValueList = new List<string>();
            List<string> fieldValueList = new List<string>();
            int i = 0;
            Core core = getCore();
                try
                {
                    foreach (string key in keys)
                    {
                        if (key.Contains("cl_"))
                        {

                            checkListValueList.Add(key.Replace("cl_v[", "").Replace("]", "") + "|" + Request.Form.Get(keys[i]));
                        }
                        else
                        {
                            int _field_id = int.Parse(key.Replace("f_[", "").Replace("]", ""));
                            eFormData.Field field = core.Advanced_FieldRead(_field_id);
                            if (field.FieldType == "MultiSelect")
                            {
                                fieldValueList.Add(key.Replace("f_[", "").Replace("]", "") + "|" + Request.Form.Get(keys[i]).Replace(",", "|"));
                            } else
                            {
                                fieldValueList.Add(key.Replace("f_[", "").Replace("]", "") + "|" + Request.Form.Get(keys[i]));
                            }                            
                        }
                        i += 1;
                    }
                }
                catch (Exception ex)
                {
                }

            try
            {

                core.CaseUpdate(id, fieldValueList, checkListValueList);

                eFormShared.Case_Dto caseDto = core.CaseReadByCaseId(id);
                return Redirect($"/Cases/Index/{caseDto.CheckListId}");
            }
            catch (Exception ex)
            {
                return RedirectToRoute("Templates");
            }
        }

        public ActionResult Edit(int id)
        {
               
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            Core core = getCore();

            try
            {
                eFormShared.Case_Dto caseDto = core.CaseReadByCaseId(id);
                string microting_uuid = caseDto.MicrotingUId;
                string microting_check_uid = caseDto.CheckUId;
                var theCase = core.CaseRead(microting_uuid, microting_check_uid);

                ViewBag.theCase = theCase;
                ViewBag.case_id = id;
                ViewBag.filePath = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + "output/dataFolder/";
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            
        }
    }


}