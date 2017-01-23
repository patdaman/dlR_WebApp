namespace RapidInventory2015.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RAPID_IM_SER
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ITEM_NO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string SER_NO { get; set; }

        [StringLength(10)]
        public string LOC_ID { get; set; }

        [Required]
        [StringLength(1)]
        public string STAT { get; set; }

        [Required]
        [StringLength(1)]
        public string PREV_STAT { get; set; }

        public decimal? SER_COST { get; set; }

        public DateTime? WARR_DAT_1 { get; set; }

        public DateTime? WARR_DAT_2 { get; set; }

        [Required]
        [StringLength(1)]
        public string MAN_ENTD { get; set; }

        public DateTime? RECV_DAT { get; set; }

        [StringLength(15)]
        public string VEND_NO { get; set; }

        [StringLength(10)]
        public string PROMPT_COD_1 { get; set; }

        [StringLength(30)]
        public string PROMPT_ALPHA_1 { get; set; }

        public DateTime? PROMPT_DAT_1 { get; set; }

        public decimal? PROMPT_NO_1 { get; set; }

        [StringLength(30)]
        public string PROMPT_1_STR { get; set; }

        [StringLength(10)]
        public string PROMPT_COD_2 { get; set; }

        [StringLength(30)]
        public string PROMPT_ALPHA_2 { get; set; }

        public DateTime? PROMPT_DAT_2 { get; set; }

        public decimal? PROMPT_NO_2 { get; set; }

        [StringLength(30)]
        public string PROMPT_2_STR { get; set; }

        [StringLength(10)]
        public string PROMPT_COD_3 { get; set; }

        [StringLength(30)]
        public string PROMPT_ALPHA_3 { get; set; }

        public DateTime? PROMPT_DAT_3 { get; set; }

        public decimal? PROMPT_NO_3 { get; set; }

        [StringLength(30)]
        public string PROMPT_3_STR { get; set; }

        public DateTime? LST_MAINT_DT { get; set; }

        [StringLength(10)]
        public string LST_MAINT_USR_ID { get; set; }

        public DateTime? LST_LCK_DT { get; set; }
    }
}
