﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Concoct_Builder
{
    public partial class Layouts
    {
        public Layouts()
        {
            LayoutDataLayout = new HashSet<LayoutData>();
            LayoutDataRefereenceScreenNavigation = new HashSet<LayoutData>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Owner { get; set; }
        public long? UserSetting { get; set; }
        public string LayoutThumbnail { get; set; }

        public virtual UserSettings UserSettingNavigation { get; set; }
        public virtual ICollection<LayoutData> LayoutDataLayout { get; set; }
        public virtual ICollection<LayoutData> LayoutDataRefereenceScreenNavigation { get; set; }
    }
}