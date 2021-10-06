using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Models
{
    public class IncomingServerResponse
    {
        public bool item1 { get; set; }
        public List<InternalProject> item2 { get; set; }
        public List<string> item3 { get; set; }

    }

    public class InternalProject
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<InternalWorkItem> WorkItems { get; set; }
        public string OrganizationName { get; set; }
    }

    public class InternalWorkItem
    {
        public int id { get; set; }
        public int workItemType { get; set; }
        public string title { get; set; }
        public int iteration { get; set; }
    }
}
