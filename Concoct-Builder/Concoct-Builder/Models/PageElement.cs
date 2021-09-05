using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Models
{
    public class PageElement
    {
        public string ElementName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ClientX { get; set; }
        public int ClientY { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
    }
}
