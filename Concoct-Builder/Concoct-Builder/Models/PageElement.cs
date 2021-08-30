using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Models
{
    public class PageElement
    {
        public int Id { get; set; }
        public string Element { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
