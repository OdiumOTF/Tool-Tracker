using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ToolTracker_FrontEnd.DAL;
using ToolTracker_FrontEnd.Models;

namespace ToolTracker_FrontEnd.Controllers
{
    public class ToolsController : Controller
    {
        private ToolContext db = new ToolContext();

        // GET: Tools
        public ActionResult Index()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Tools").Result;
            // we are using IEnumerable because we only want to enumerate the collection and we are not going to add or delete elements
            IEnumerable<Tool> tools = response.Content.ReadAsAsync<IEnumerable<Tool>>().Result;

            return View(tools);
        }

        public ActionResult Rent(int? id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Tools/{id}").Result;
            var tool = response.Content.ReadAsAsync<Tool>().Result;
            return View(tool);
        }

        [HttpPost]
        public ActionResult Rent([Bind(Include = "ToolId,Name,Brand,Notes,Available,AssetNum,Comment")]int id, Tool tool)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Tools/{id}", tool).Result;

                //we will refer to this in the Index.cshtml of the tool so alertify can display the message.
                TempData["SuccessMessage"] = "Saved successfully.";

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View(tool);
            }
            catch
            {
                return View();
            }
        }


        // GET: Tools/Details/5
        public ActionResult Details(int? id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Tools/{id}").Result;
            var tool = response.Content.ReadAsAsync<Tool>().Result;
            return View(tool);
        }

        // GET: Tools/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ToolId,Name,Brand,Notes,Available,AssetNum,Comment")] Tool tool)
        {
            try
            {
                //    db.Tools.Add(tool);
                //    db.SaveChanges();
                //    return RedirectToAction("Index");
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("Tools", tool).Result;
                //we will refer to this in the Index.cshtml of the tool so alertify can display the message.
                TempData["SuccessMessage"] = "Tool added successfully.";

                return RedirectToAction("Index");

            }
            catch {
                return View();
            }
        }

        // GET: Tools/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Tools/{id}").Result;
            var tool = response.Content.ReadAsAsync<Tool>().Result;
            if (tool == null)
            {
                return HttpNotFound();
            }

            
            return View(tool);
        }

        // POST: Tools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ToolId,Name,Brand,Notes,Available,AssetNum,Comment")]int id, Tool tool)
        {
            try {
                //db.Entry(tool).State = EntityState.Modified;
                //db.SaveChanges();

                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Tools/{id}", tool).Result;

                //we will refer to this in the Index.cshtml of the tool so alertify can display the message.
                TempData["SuccessMessage"] = "Saved successfully.";

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View(tool);
            }
            catch {
                return View();
            }
        }

        // GET: Tools/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Tools/{id}").Result;
            var tool = response.Content.ReadAsAsync<Tool>().Result;
            if (tool == null)
            {
                return HttpNotFound();
            }

            
            return View(tool);
        }

        // POST: Tools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Tool tool = db.Tools.Find(id);
            //db.Tools.Remove(tool);
            //db.SaveChanges();
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"Tools/{id}").Result;
                //we will refer to this in the Index.cshtml of the tool so alertify can display the message.
                TempData["SuccessMessage"] = "Tool deleted successfully.";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
