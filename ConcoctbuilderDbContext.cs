﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Concoct_Builder
{
    public partial class ConcoctbuilderDbContext : DbContext
    {
        public ConcoctbuilderDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=ConcoctBuilder.db");
            }
        }


        public ConcoctbuilderDbContext(DbContextOptions<ConcoctbuilderDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AssociatedTags> AssociatedTags { get; set; }
        public virtual DbSet<LayoutData> LayoutData { get; set; }
        public virtual DbSet<Layouts> Layouts { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<UserSettings> UserSettings { get; set; }
        public virtual DbSet<WorkItems> WorkItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssociatedTags>(entity =>
            {
                entity.HasOne(d => d.Layout)
                    .WithMany(p => p.AssociatedTags)
                    .HasForeignKey(d => d.LayoutId);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.AssociatedTags)
                    .HasForeignKey(d => d.ProjectId);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.AssociatedTags)
                    .HasForeignKey(d => d.TagId);
            });

            modelBuilder.Entity<LayoutData>(entity =>
            {
                entity.HasOne(d => d.Layout)
                    .WithMany(p => p.LayoutDataLayout)
                    .HasForeignKey(d => d.LayoutId);

                entity.HasOne(d => d.RefereenceScreenNavigation)
                    .WithMany(p => p.LayoutDataRefereenceScreenNavigation)
                    .HasForeignKey(d => d.RefereenceScreen);
            });

            modelBuilder.Entity<Layouts>(entity =>
            {
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Layouts)
                    .HasForeignKey(d => d.ProjectId);

                entity.HasOne(d => d.UserSettingNavigation)
                    .WithMany(p => p.Layouts)
                    .HasForeignKey(d => d.UserSetting);

                entity.HasOne(d => d.WorkItem)
                    .WithMany(p => p.Layouts)
                    .HasForeignKey(d => d.WorkItemId);
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.Property(e => e.IsNew).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<UserSettings>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.OrganizationName).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}