﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Concoct_Builder
{
    public partial class WorkItems
    {
        public WorkItems()
        {
            Layouts = new HashSet<Layouts>();
        }

        public long Id { get; set; }
        public long? ProjectId { get; set; }
        public long? InternalId { get; set; }
        public string Title { get; set; }
        public long? SprintId { get; set; }
        public long? WorkItemType { get; set; }

        public virtual ICollection<Layouts> Layouts { get; set; }
    }
}