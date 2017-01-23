namespace RapidInventory2015.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RAPID_IM_GRID_DIMS
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

        [Key]
        [Column(Order = 3)]
        [StringLength(15)]
        public string DIM_1_UPR { get; set; }

        public int DIM_1_SEQ_NO { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(15)]
        public string DIM_2_UPR { get; set; }

        public int DIM_2_SEQ_NO { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(15)]
        public string DIM_3_UPR { get; set; }

        public int DIM_3_SEQ_NO { get; set; }
    }
}
