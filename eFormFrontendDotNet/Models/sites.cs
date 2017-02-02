namespace eFormFrontendDotNet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sites
    {

        public sites()
        {
            this.cases = new HashSet<cases>();
            this.check_list_sites = new HashSet<check_list_sites>();
        }

        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        [Key]
        public int? microting_uid { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(50)]
        public string customer_no { get; set; }

        [StringLength(10)]
        public string otp_code { get; set; }

        public virtual ICollection<cases> cases { get; set; }

        public virtual ICollection<check_list_sites> check_list_sites { get; set; }
    }
}
