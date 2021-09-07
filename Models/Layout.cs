using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Models
{
    public class Layout
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<PageElement> PageElements { get; set; }
    }
}
