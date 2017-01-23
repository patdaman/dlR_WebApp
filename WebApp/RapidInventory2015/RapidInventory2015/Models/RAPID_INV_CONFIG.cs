namespace RapidInventory2015.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RAPID_INV_CONFIG
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(40)]
        public string COMPANY_NAM { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string LOC_ID { get; set; }

        public int TOTAL_SECTIONS { get; set; }
    }
}
