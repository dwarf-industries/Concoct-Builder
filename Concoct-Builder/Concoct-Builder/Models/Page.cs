using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Models
{
    public class Page
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<PageElement> PageElements { get; set; }
        public Layout Layout { get; set; }
    }
}
