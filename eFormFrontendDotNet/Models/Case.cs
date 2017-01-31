namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Case : DbContext
    {
        public Case(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<cases> cases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<cases>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<cases>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<cases>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<cases>()
                .Property(e => e.done_at)
                .HasPrecision(0);

            modelBuilder.Entity<cases>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<cases>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<cases>()
                .Property(e => e.microting_check_uid)
                .IsUnicode(false);

            modelBuilder.Entity<cases>()
                .Property(e => e.case_uid)
                .IsUnicode(false);

            modelBuilder.Entity<cases>()
                .Property(e => e.custom)
                .IsUnicode(false);
        }
    }
}
