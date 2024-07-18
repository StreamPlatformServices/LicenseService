using LicenseService.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LicenseService.Persistance
{
    public class LicenseDatabaseContext : DbContext
    {
        public LicenseDatabaseContext(DbContextOptions<LicenseDatabaseContext> options)
            : base(options)
        {
        }
        public DbSet<LicenseData> Licenses { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<LicenseData>()
        //        .HasIndex(l => new { l.FileId, l.UserId })
        //        .IsUnique();
        //}
    }

}
