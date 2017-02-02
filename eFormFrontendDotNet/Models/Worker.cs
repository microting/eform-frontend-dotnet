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
                .Property(e => e.first_name)
                .IsUnicode(false);

            modelBuilder.Entity<workers>()
                .Property(e => e.last_name)
                .IsUnicode(false);

            modelBuilder.Entity<workers>()
                .Property(e => e.microting_uuid)
                .IsUnicode(false);

            modelBuilder.Entity<workers>()
                .Property(e => e.email)
                .IsUnicode(false);
        }
    }
}
