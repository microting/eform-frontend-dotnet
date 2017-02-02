namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class site_workers
    {
        public int id { get; set; }

        public int? site_id { get; set; }

        public int? worker_id { get; set; }
    }
}
