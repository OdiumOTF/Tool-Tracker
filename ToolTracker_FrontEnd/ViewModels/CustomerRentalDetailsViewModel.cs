using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToolTracker_FrontEnd.Models;
using ToolTracker_FrontEnd.ViewModels;

namespace ToolTracker_FrontEnd.ViewModels
{
    public class CustomerRentalDetailsViewModel
    {
        public Rental Rental { get; set; }
        public string CustomerName { get; set; }
        public List<CustomerToolViewModel> RentedTools { get; set; }

    }
}