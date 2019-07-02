using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToolTracker_FrontEnd.ViewModels
{
    public class CustomerRentalsViewModel
    {
        [Key]
        public int RentalId { get; set; }
        public DateTime DateRented { get; set; }
        public string CustomerName { get; set; }
    }
}