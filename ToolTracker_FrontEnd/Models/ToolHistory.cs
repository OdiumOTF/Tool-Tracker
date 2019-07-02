using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToolTracker_FrontEnd.Models
{
    public class ToolHistory
    {
        public int ToolHistoryId { get; set; }
        public string CustomerName { get; set; }
        public DateTime Borrowed { get; set; }
        public DateTime? Returned { get; set; }
        public string AssetNum { get; set; }
    }
}