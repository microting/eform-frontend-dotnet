namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CheckListSite : DbContext
    {
        public CheckListSite(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<check_list_sites> check_list_sites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<check_list_sites>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<check_list_sites>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_sites>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_sites>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<check_list_sites>()
                .Property(e => e.last_check_id)
                .IsUnicode(false);
        }
    }
}
