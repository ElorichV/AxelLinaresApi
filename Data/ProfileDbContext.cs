using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AxelLinaresApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxelLinaresApi.Data
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var stringListComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                c => c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v != null ? v.GetHashCode() : 0)) : 0,
                c => c != null ? c.ToList() : new List<string>());

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.HardSkills)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(stringListComparer);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.SoftSkills)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(stringListComparer);

            modelBuilder.Entity<Experience>()
                .Property(e => e.Responsibilities)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(stringListComparer);

            modelBuilder.Entity<Project>()
                .Property(p => p.TechStack)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(stringListComparer);

            modelBuilder.Entity<Project>()
                .Property(p => p.ArchitectureNotes)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(stringListComparer);
        }
    }
}
