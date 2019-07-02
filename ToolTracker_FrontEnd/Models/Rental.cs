using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolTracker_FrontEnd.ViewModels;

namespace ToolTracker_FrontEnd.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int CustomerId { get; set; }
        public int ToolId { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime? DateReturned { get; set; }
        public virtual ICollection<RentalItem> RentalItems { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
        public IEnumerable<CustomerToolViewModel> RentedTools { get; set; }
    }
}