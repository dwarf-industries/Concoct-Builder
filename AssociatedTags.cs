// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Concoct_Builder
{
    public partial class AssociatedTags
    {
        public long Id { get; set; }
        public long? TagId { get; set; }
        public long? LayoutId { get; set; }
        public long? ProjectId { get; set; }
        public string Organization { get; set; }

        public virtual Layouts Layout { get; set; }
        public virtual Projects Project { get; set; }
        public virtual Tags Tag { get; set; }
    }
}