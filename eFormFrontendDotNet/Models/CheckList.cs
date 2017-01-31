namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CheckList : DbContext
    {
        public CheckList(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<check_lists> check_lists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<check_lists>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.label)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.custom)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.case_type)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.folder_name)
                .IsUnicode(false);
        }
    }
}
