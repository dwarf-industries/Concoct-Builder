﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Concoct_Builder
{
    public partial class Tags
    {
        public Tags()
        {
            AssociatedTags = new HashSet<AssociatedTags>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long? IsNew { get; set; }

        public virtual ICollection<AssociatedTags> AssociatedTags { get; set; }
    }
}