using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eFormFrontendDotNet.Models;

namespace eFormFrontendDotNet.Controllers
{
    public class SettingsController : Controller
    {
        //private 

        // GET: settings
        public async Task<ActionResult> Index()
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            Setting db = new Setting(connectionStr);
            if (db.settings.Count() < 11)
            {
                SettingAdd(1, "firstRunDone", "false", connectionStr);
                SettingAdd(2, "knownSitesDone", "false", connectionStr);
                SettingAdd(3, "logLevel", "true", connectionStr);
                SettingAdd(4, "comToken", "", connectionStr);
                SettingAdd(5, "comAddress", "", connectionStr);
                SettingAdd(6, "comAddressBasic", "", connectionStr);
                SettingAdd(7, "organizationId", "", connectionStr);
                SettingAdd(8, "subscriberToken", "", connectionStr);
                SettingAdd(9, "subscriberAddress", "", connectionStr);
                SettingAdd(10, "subscriberName", "", connectionStr);
                SettingAdd(11, "fileLocation", "datafolder/", connectionStr);

            }
            return View(await db.settings.ToListAsync());
        }

        // GET: settings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            Setting db = new Setting(connectionStr);
            settings settings = await db.settings.FindAsync(id);
            if (settings == null)
            {
                return HttpNotFound();
            }
            return View(settings);
        }

        // GET: settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,name,value")] settings settings)
        {
            if (ModelState.IsValid)
            {
                string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

                string connectionStr = lines.First();
                Setting db = new Setting(connectionStr);
                db.settings.Add(settings);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(settings);
        }

        // GET: settings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            Setting db = new Setting(connectionStr);
            settings settings = await db.settings.FindAsync(id);
            if (settings == null)
            {
                return HttpNotFound();
            }
            return View(settings);
        }

        // POST: settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,name,value")] settings settings)
        {
            if (ModelState.IsValid)
            {
                string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

                string connectionStr = lines.First();
                Setting db = new Setting(connectionStr);
                db.Entry(settings).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(settings);
        }

        // GET: settings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            Setting db = new Setting(connectionStr);
            settings settings = await db.settings.FindAsync(id);
            if (settings == null)
            {
                return HttpNotFound();
            }
            return View(settings);
        }

        // POST: settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/bin/Input.txt"));

            string connectionStr = lines.First();
            Setting db = new Setting(connectionStr);
            settings settings = await db.settings.FindAsync(id);
            db.settings.Remove(settings);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private void SettingAdd(int id, string name, string value, string connectionStr)
        {
            using (var db = new Setting(connectionStr))
            {
                settings set = new settings();
                set.id = id;
                set.name = name;
                set.value = value;

                db.settings.Add(set);
                db.SaveChanges();
            }
        }

    }
}
