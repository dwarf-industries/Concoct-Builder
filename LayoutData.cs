﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Concoct_Builder
{
    public partial class LayoutData
    {
        public long Id { get; set; }
        public long? LayoutId { get; set; }
        public string ElementName { get; set; }
        public string Base64 { get; set; }
        public string Translate { get; set; }
        public long? RefereenceScreen { get; set; }
        public string ComponentName { get; set; }
        public string OffsetX { get; set; }
        public string OffsetY { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }

        public virtual Layouts Layout { get; set; }
        public virtual Layouts RefereenceScreenNavigation { get; set; }
    }
}