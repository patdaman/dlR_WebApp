namespace RapidInventory2015.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RAPID_IM_ITEM
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
        [StringLength(20)]
        public string ITEM_NO { get; set; }

        [Required]
        [StringLength(30)]
        public string DESCR { get; set; }

        [Required]
        [StringLength(1)]
        public string ITEM_TYP { get; set; }

        [Required]
        [StringLength(1)]
        public string TRK_METH { get; set; }

        public int QTY_DECS { get; set; }

        [Required]
        [StringLength(15)]
        public string STK_UNIT { get; set; }

        [StringLength(15)]
        public string ALT_1_UNIT { get; set; }

        public decimal? ALT_1_NUMER { get; set; }

        public decimal? ALT_1_DENOM { get; set; }

        [StringLength(15)]
        public string ALT_2_UNIT { get; set; }

        public decimal? ALT_2_NUMER { get; set; }

        public decimal? ALT_2_DENOM { get; set; }

        [StringLength(15)]
        public string ALT_3_UNIT { get; set; }

        public decimal? ALT_3_NUMER { get; set; }

        public decimal? ALT_3_DENOM { get; set; }

        [StringLength(15)]
        public string ALT_4_UNIT { get; set; }

        public decimal? ALT_4_NUMER { get; set; }

        public decimal? ALT_4_DENOM { get; set; }

        [StringLength(15)]
        public string ALT_5_UNIT { get; set; }

        public decimal? ALT_5_NUMER { get; set; }

        public decimal? ALT_5_DENOM { get; set; }

        [StringLength(10)]
        public string GRID_DIM_1_TAG { get; set; }

        [StringLength(10)]
        public string GRID_DIM_2_TAG { get; set; }

        [StringLength(10)]
        public string GRID_DIM_3_TAG { get; set; }

        public decimal PRC_1 { get; set; }
    }
}
