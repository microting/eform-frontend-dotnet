namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Site : DbContext
    {
        public Site(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<sites> sites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<sites>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<sites>()
                .Property(e => e.updated_at)
                .HasPrecision(0);
        }
    }
}
