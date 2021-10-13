using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Models
{
    public class PageElement
    {
        public string ElementName { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string ClientX { get; set; }
        public string ClientY { get; set; }
        public string OffsetX { get; set; }
        public string OffsetY { get; set; }
        public string Translate { get; set; }
        public string Base64 { get; set; }
        public List<Event> Events{ get; set; }
        public string ComponentName { get; set; }
        public bool IsTrigger { get; set; }
        public string hoPercent { get; set; }
        public string woPercent { get; set; }
        public string wPercent { get; set; }

        public string hPercent { get; set; }
    }
}
