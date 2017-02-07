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

using eFormCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eFormFrontendDotNet.Controllers
{
    public class UnitsController : ApplicationController
    {
        public ActionResult Index()
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();

            using (var db = new Models.Unit(connectionStr))
            {
                try
                {
                    ViewBag.units = db.units.Where(x => x.workflow_state != "removed").ToList();
                    return View();
                }
                catch (Exception ex)
                {
                }
            }
            return View();
        }

        public JsonResult RequestOtp(int id)
        {
            Models.DataResponse response = new Models.DataResponse();

            try
            {
                Core core = getCore();
                core.UnitRequestOtp(id);
                response.data = new Models.DataResponse.Data($"Unit \"{id}\" OTP requested successfully", "success");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.data = new Models.DataResponse.Data($"Unit \"{id}\" OTP request could not be completed!", "error");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}