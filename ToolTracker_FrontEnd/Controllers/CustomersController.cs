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
    public class CustomersController : Controller
    {
        private ToolContext db = new ToolContext();

        // GET: Customers
        public ActionResult Index()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Customers").Result;
            IEnumerable<Customer> customers = response.Content.ReadAsAsync<IEnumerable<Customer>>().Result;
            return View(customers);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Customers/{id}").Result;
            var customer = response.Content.ReadAsAsync<Customer>().Result;
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("Customers", customer).Result;
                //old system
                //db.Customers.Add(customer);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
            return View();
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Customers/{id}").Result;
            var customer = response.Content.ReadAsAsync<Customer>().Result;
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Customer customer)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Customers/{id}", customer).Result;
                // old system
                //db.Entry(customer).State = EntityState.Modified;
                //db.SaveChanges();
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View(customer);
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Customers/{id}").Result;
            var customer = response.Content.ReadAsAsync<Customer>().Result;

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, Customer customer)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"Customers/{id}").Result;

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
