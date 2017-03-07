using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using eFormFrontendDotNet.Models;
using eFormCore;

namespace eFormFrontendDotNet.Controllers
{
    public class SettingsController : ApplicationController
    {
        //private 

        // GET: settings
        public ActionResult Index()
        {
            return null;
        }

        // GET: settings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            //string connectionStr = lines.First();
            //Setting db = new Setting(connectionStr);
            //settings settings = await db.settings.FindAsync(id);
            //if (settings == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // GET: settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

        //        //string connectionStr = lines.First();
        //        //Setting db = new Setting(connectionStr);
        //        //db.settings.Add(settings);
        //        //await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View();
        //}

        // GET: settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            //string connectionStr = lines.First();
            //Setting db = new Setting(connectionStr);
            //settings settings = await db.settings.FindAsync(id);
            //if (settings == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(settings);
            return View();
        }

        // POST: settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            if (ModelState.IsValid)
            {
                //string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

                //string connectionStr = lines.First();
                //Setting db = new Setting(connectionStr);
                //db.Entry(settings).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: settings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            //string connectionStr = lines.First();
            //Setting db = new Setting(connectionStr);
            //settings settings = await db.settings.FindAsync(id);
            //if (settings == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // POST: settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            //string connectionStr = lines.First();
            //Setting db = new Setting(connectionStr);
            //settings settings = await db.settings.FindAsync(id);
            //db.settings.Remove(settings);
            //await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConnectionString()
        {
            string source = Request.Form.Get("connection['source']");
            string catalog = Request.Form.Get("connection['catalog']");
            string auth = Request.Form.Get("connection['auth']");
            string token = Request.Form.Get("connection['token']");

            string result = "Data Source=" + source + ";Initial Catalog=" + catalog + ";" + auth;
            System.IO.File.AppendAllText(Server.MapPath("~/bin/Input.txt"), result);
            AdminTools adminTools = null;
            try
            {
                 adminTools = new AdminTools(result);
            } catch
            {
                adminTools = new AdminTools(result);
            }
            
            adminTools.DbSetup(token);

            return Redirect("/");
        }

        public ActionResult ConnectionMissing()
        {
            return View();
        }

    }
}
