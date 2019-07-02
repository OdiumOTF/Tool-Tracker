using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToolTracker_FrontEnd.Models
{
    public class Tool
    {
        public int ToolId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Notes { get; set; }
        public int Available { get; set; }
        public string AssetNum { get; set; }
        public string Comment { get; set; }
    }
}