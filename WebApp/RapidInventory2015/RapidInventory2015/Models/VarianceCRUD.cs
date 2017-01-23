using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace RapidInventory2015.Models
{
    public class VarianceCRUD
    {
        [Key]
        [Column(Order = 0)]
        public string COMPANY_NAM { get; set; }
        [Key]
        [Column(Order = 1)]
        public string LOC_ID { get; set; }
        [Key]
        [Column(Order = 2)]
        public string SECTION_ID { get; set; }
        public string ITEM_NO { get; set; }
        [Key]
        [Column(Order = 3)]
        public string BARCOD { get; set; }
        public string DESCR { get; set; }
        public string UNIT { get; set; }
        public decimal? CNT_QTY { get; set; }
        public decimal? FRZ_QTY_ON_HND { get; set; }

        public VarianceCRUD()
        { }
        public VarianceCRUD(VarianceCRUD obj)
        {
            COMPANY_NAM = obj.COMPANY_NAM;
            LOC_ID = obj.LOC_ID;
            SECTION_ID = obj.SECTION_ID;
            ITEM_NO = obj.ITEM_NO;
            BARCOD = obj.BARCOD;
            DESCR = obj.DESCR;
            UNIT = obj.UNIT;
            CNT_QTY = obj.CNT_QTY;
            FRZ_QTY_ON_HND = obj.FRZ_QTY_ON_HND;
        }
    }
}