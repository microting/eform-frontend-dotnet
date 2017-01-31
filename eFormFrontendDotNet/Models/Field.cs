namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Field : DbContext
    {
        public Field(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<fields> fields { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<fields>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<fields>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<fields>()
                .Property(e => e.label)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.color)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.default_value)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.unit_name)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.min_value)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.max_value)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.barcode_type)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.query_type)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.key_value_pair_list)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.custom)
                .IsUnicode(false);
        }
    }
}
