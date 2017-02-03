namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Worker : DbContext
    {
        public Worker(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<workers> workers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<workers>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<workers>()
                .Property(e => e.updated_at)
                .HasPrecision(0);
        }
    }
}
