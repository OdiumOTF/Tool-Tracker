using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToolTracker_FrontEnd.ViewModels
{
    public class CustomerToolViewModel
    {
        public int RentalId { get; set; }
        public int RentalItemId { get; set; }
        public string ToolName { get; set; }
        public string ToolBrand { get; set; }
        public string ToolNotes { get; set; }
        public int Available { get; set; }
        public string AssetNum { get; set; }
        public string Comment { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }
}