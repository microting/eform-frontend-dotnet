namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class check_lists
    {
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        [StringLength(255)]
        public string label { get; set; }

        [StringLength(255)]
        public string description { get; set; }

        public string custom { get; set; }

        public int? parent_id { get; set; }

        public int? repeated { get; set; }

        public int? display_index { get; set; }

        [StringLength(255)]
        public string case_type { get; set; }

        [StringLength(255)]
        public string folder_name { get; set; }

        public short? review_enabled { get; set; }

        public short? manual_sync { get; set; }

        public short? extra_fields_enabled { get; set; }

        public short? done_button_enabled { get; set; }

        public short? approval_enabled { get; set; }

        public short? multi_approval { get; set; }

        public short? fast_navigation { get; set; }

        public short? download_entities { get; set; }
    }
}
