namespace RapidInventory2015.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RAPID_USR_ACCT
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(40)]
        public string COMPANY_NAM { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string LOC_ID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(40)]
        public string USR_NAM { get; set; }

        [Required]
        [StringLength(32)]
        public string PWD { get; set; }

        public int? FLD_LGN_CNT { get; set; }

        [Required]
        [StringLength(1)]
        public string ALLOW_FORCE { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_MANAGER { get; set; }

        public DateTime? LST_MAINT_DT { get; set; }
    }
}
