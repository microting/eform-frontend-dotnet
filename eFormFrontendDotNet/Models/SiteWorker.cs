namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SiteWorker : DbContext
    {
        public SiteWorker()
            : base("name=SiteWorker")
        {
        }

        public virtual DbSet<site_workers> site_workers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
