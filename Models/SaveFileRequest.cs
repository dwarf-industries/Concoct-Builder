using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Models
{
    public class SaveFileRequest
    {
        public IncomingFileRequest File { get; set; }
        public List<PageElement> PageElements { get; set; }
    }
}
