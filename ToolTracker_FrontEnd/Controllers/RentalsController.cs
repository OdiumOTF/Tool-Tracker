using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using ToolTracker_FrontEnd;
using ToolTracker_FrontEnd.DAL;
using ToolTracker_FrontEnd.Models;
using ToolTracker_FrontEnd.ViewModels;

namespace VideoRental.Controllers
{
    public class RentalsController : Controller
    {
        private ToolContext db = new ToolContext();

        // GET: Rentals
        public ActionResult Index()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Rentals").Result;
            IEnumerable<Rental> rentals = response.Content.ReadAsAsync<IEnumerable<Rental>>().Result;
            response = WebClient.ApiClient.GetAsync("Customers").Result;
            IList<Customer> customers = response.Content.ReadAsAsync<IList<Customer>>().Result;

            var customerRentalsViewModel = rentals.Select(
                r => new CustomerRentalsViewModel
                {
                    RentalId = r.RentalId,
                    DateRented = r.DateRented,
                    CustomerName = customers.Where(c => c.CustomerId == r.CustomerId).Select(u => u.CustomerName).FirstOrDefault()
                }).OrderByDescending(o => o.DateRented).ToList();

            return View(customerRentalsViewModel);
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

        public ActionResult Edit(int Id)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
                var rental = response.Content.ReadAsAsync<Rental>().Result;
                response = WebClient.ApiClient.GetAsync($"RentalItemsById/{Id}").Result;
                IList<RentalItem> rentalItems = response.Content.ReadAsAsync<IList<RentalItem>>().Result;
                response = WebClient.ApiClient.GetAsync("Tools").Result;
                IList<Tool> dbTools = response.Content.ReadAsAsync<IList<Tool>>().Result;

                var customers = GetCustomers();
                var rentedTools = rentalItems.Select(
                        m => new CustomerToolViewModel
                        {
                            RentalItemId = m.RentalItemId,
                            RentalId = m.RentalId,
                            ToolName = dbTools.Where(c => c.ToolId == m.ToolId).Select(f => f.Name).FirstOrDefault()
                        }).ToList();

                rental.Customers = customers;
                rental.RentedTools = rentedTools;

                return View(rental);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(int Id, Rental rental)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Rentals/{Id}", rental).Result;

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View(rental);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
            var rental = response.Content.ReadAsAsync<Rental>().Result;
            response = WebClient.ApiClient.GetAsync("Customers").Result;
            IList<Customer> customers = response.Content.ReadAsAsync<IList<Customer>>().Result;
            response = WebClient.ApiClient.GetAsync("Tools").Result;
            IList<Tool> dbTools = response.Content.ReadAsAsync<IList<Tool>>().Result;

            var customerRentalDetails = new CustomerRentalDetailsViewModel
            {
                Rental = rental,
                CustomerName = customers.Select(cu => cu.CustomerName).FirstOrDefault(),
                RentedTools = rental.RentalItems.Select(
                        ri => new CustomerToolViewModel
                        {
                            RentalId = ri.RentalId,
                            ToolName = dbTools.Where(c2 => c2.ToolId == ri.ToolId).Select(m => m.Name).FirstOrDefault()
                        }).ToList()
            };

            return View(customerRentalDetails);
        }

        public ActionResult Create()
        {
            var rental = new Rental();
            var rentalItem = new RentalItem();
            rentalItem.RentalId = -999;
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("GetRentalMaxId").Result;
            // Setting the primary key value to a negative value will make SQL server to find the next available PKID when you save it.
            rental.RentalId = -999;         
            rental.DateRented = DateTime.Now;
            var customers = GetCustomers();
            rental.Customers = customers;
            
            rental.RentedTools = new List<CustomerToolViewModel>();

            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rental rental)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("Rentals", rental).Result;
                rental = response.Content.ReadAsAsync<Rental>().Result;
                //rental.Customers = GetCustomers();
                //response = WebClient.ApiClient.GetAsync($"RentalItemsById/{rental.RentalId}").Result;
                //IList<RentalItem> rentalItems = response.Content.ReadAsAsync<IList<RentalItem>>().Result;

                //if (rentalItems.Count == 0)
                    //return RedirectToAction("Edit", new { Id = rental.RentalId });
                //else
                    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
            var rental = response.Content.ReadAsAsync<Rental>().Result;

            return View(rental);
        }

        [HttpPost]
        public ActionResult Delete(int Id, Rental rental)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"Rentals/{Id}").Result;

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AddTools(int RentalId)
        {
            var rentalItem = new RentalItem();
            var tools = GetTools();
            rentalItem.RentalId = RentalId;
            rentalItem.Tools = tools;

            return View(rentalItem);
        }

        [HttpPost]
        public ActionResult AddTools(RentalItem rentalItem)
        {
            int Id = 0;
            try
            {
                Id = rentalItem.RentalId;
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("RentalItems", rentalItem).Result;

                return RedirectToAction("Edit", new { Id });
            }
            catch (Exception)
            {
                return View("No record of the associated rental can be found.  Make sure to submit the rental details before adding tools.");
            }
        }

        public ActionResult EditRentedTool(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"RentalItems/{Id}").Result;
            var rentalItem = response.Content.ReadAsAsync<RentalItem>().Result;
            var tools = GetTools();
            rentalItem.Tools = tools;

            return View(rentalItem);
        }

        [HttpPost]
        public ActionResult EditRentedTool(int Id, RentalItem rentalItem)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"RentalItems/{Id}", rentalItem).Result;
                Id = rentalItem.RentalId;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Edit", new { Id });

                return View(rentalItem);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteRentedTool(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"RentalItems/{Id}").Result;
            var rentalItem = response.Content.ReadAsAsync<RentalItem>().Result;
            var tools = GetTools();
            rentalItem.Tools = tools;

            return View(rentalItem);
        }

        [HttpPost]
        public ActionResult DeleteRentedTool(int Id, FormCollection collection)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"RentalItems/{Id}").Result;
                var rentalItem = response.Content.ReadAsAsync<RentalItem>().Result;
                Id = rentalItem.RentalId;
                return RedirectToAction("Edit", new { Id });
            }
            catch
            {
                return View();
            }
        }

        public IEnumerable<SelectListItem> GetTools()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Tools").Result;
            IList<Tool> dbTools = response.Content.ReadAsAsync<IList<Tool>>().Result;

            List<SelectListItem> tools = dbTools
                                            .OrderBy(o => o.Name)
                                            .Select(m => new SelectListItem
                                            {
                                                Value = m.ToolId.ToString(),
                                                Text = m.Name
                                            }).ToList();

            return new SelectList(tools, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetCustomers()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Customers").Result;
            IList<Customer> dbCustomers = response.Content.ReadAsAsync<IList<Customer>>().Result;
            List<SelectListItem> customers = dbCustomers
                .OrderBy(o => o.CustomerName)
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.CustomerName
                }).ToList();

            return new SelectList(customers, "Value", "Text");
        }
    }
}