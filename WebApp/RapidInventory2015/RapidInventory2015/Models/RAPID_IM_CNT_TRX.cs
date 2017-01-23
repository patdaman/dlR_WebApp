namespace RapidInventory2015.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RAPID_IM_CNT_TRX
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

        public decimal? CNT_QTY_1 { get; set; }

        [Required]
        [StringLength(1)]
        public string CNT_UNIT_1 { get; set; }

        public decimal? UNIT_1_NUMER { get; set; }

        public decimal? UNIT_1_DENOM { get; set; }

        [StringLength(15)]
        public string UNIT_1 { get; set; }

        public decimal? CNT_QTY_2 { get; set; }

        [Required]
        [StringLength(1)]
        public string CNT_UNIT_2 { get; set; }

        public decimal? UNIT_2_NUMER { get; set; }

        public decimal? UNIT_2_DENOM { get; set; }

        [StringLength(15)]
        public string UNIT_2 { get; set; }

        public decimal? CNT_QTY_3 { get; set; }

        [Required]
        [StringLength(1)]
        public string CNT_UNIT_3 { get; set; }

        public decimal? UNIT_3_NUMER { get; set; }

        public decimal? UNIT_3_DENOM { get; set; }

        [StringLength(15)]
        public string UNIT_3 { get; set; }

        public decimal? CNT_QTY_4 { get; set; }

        [Required]
        [StringLength(1)]
        public string CNT_UNIT_4 { get; set; }

        public decimal? UNIT_4_NUMER { get; set; }

        public decimal? UNIT_4_DENOM { get; set; }

        [StringLength(15)]
        public string UNIT_4 { get; set; }

        public decimal? CNT_QTY_5 { get; set; }

        [Required]
        [StringLength(1)]
        public string CNT_UNIT_5 { get; set; }

        public decimal? UNIT_5_NUMER { get; set; }

        public decimal? UNIT_5_DENOM { get; set; }

        [StringLength(15)]
        public string UNIT_5 { get; set; }

        public decimal? CNT_QTY_6 { get; set; }

        [Required]
        [StringLength(1)]
        public string CNT_UNIT_6 { get; set; }

        public decimal? UNIT_6_NUMER { get; set; }

        public decimal? UNIT_6_DENOM { get; set; }

        [StringLength(15)]
        public string UNIT_6 { get; set; }

        public DateTime? FRZ_DAT { get; set; }

        public decimal FRZ_QTY_ON_HND { get; set; }

        public decimal? AVG_COST { get; set; }

        public decimal? UNIT_RETL_VAL { get; set; }
    }
}
