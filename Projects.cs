﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Concoct_Builder
{
    public partial class Projects
    {
        public Projects()
        {
            AssociatedTags = new HashSet<AssociatedTags>();
            Layouts = new HashSet<Layouts>();
        }

        public long Id { get; set; }
        public string Organization { get; set; }
        public string ProjectName { get; set; }
        public long InternalId { get; set; }

        public virtual ICollection<AssociatedTags> AssociatedTags { get; set; }
        public virtual ICollection<Layouts> Layouts { get; set; }
    }
}