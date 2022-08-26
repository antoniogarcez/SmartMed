using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartMed.Database.Entities;
using System;

namespace SmartMed.Database
{
    public partial class SmartMedContext : DbContext
    {
        public SmartMedContext()
        {
        }

        public SmartMedContext(DbContextOptions<SmartMedContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Medication> Medication { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medication>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 10)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
