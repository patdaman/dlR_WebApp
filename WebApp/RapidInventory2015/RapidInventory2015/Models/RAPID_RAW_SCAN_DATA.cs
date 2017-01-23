namespace RapidInventory2015.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RAPID_RAW_SCAN_DATA
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
        [StringLength(10)]
        public string SECTION_ID { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime SCAN_DAT { get; set; }

        [Required]
        [StringLength(52)]
        public string BARCOD { get; set; }

        [Required]
        [StringLength(30)]
        public string ITEM_NO { get; set; }

        public int? CNT_QTY { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(800)]
        public string HANDHELD_ID { get; set; }

        [Required]
        [StringLength(1)]
        public string ISFORCED { get; set; }
    }
}
