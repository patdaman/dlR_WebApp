namespace RapidInventory2015.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RapidInventory : DbContext
    {
        public RapidInventory()
            : base("name=RapidInventory")
        {
        }

        public virtual DbSet<RAPID_HANDHELD> RAPID_HANDHELD { get; set; }
        public virtual DbSet<RAPID_IM_BARCOD> RAPID_IM_BARCOD { get; set; }
        public virtual DbSet<RAPID_IM_CNT_TRX> RAPID_IM_CNT_TRX { get; set; }
        public virtual DbSet<RAPID_IM_CNT_TRX_CELL> RAPID_IM_CNT_TRX_CELL { get; set; }
        public virtual DbSet<RAPID_IM_GRID_DIMS> RAPID_IM_GRID_DIMS { get; set; }
        public virtual DbSet<RAPID_IM_INV> RAPID_IM_INV { get; set; }
        public virtual DbSet<RAPID_IM_INV_CELL> RAPID_IM_INV_CELL { get; set; }
        public virtual DbSet<RAPID_IM_ITEM> RAPID_IM_ITEM { get; set; }
        public virtual DbSet<RAPID_IM_SER> RAPID_IM_SER { get; set; }
        public virtual DbSet<RAPID_INV_CONFIG> RAPID_INV_CONFIG { get; set; }
        public virtual DbSet<RAPID_RAW_SCAN_DATA> RAPID_RAW_SCAN_DATA { get; set; }
        public virtual DbSet<RAPID_SCAN_SECTIONS> RAPID_SCAN_SECTIONS { get; set; }
        public virtual DbSet<RAPID_USR_ACCT> RAPID_USR_ACCT { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RAPID_HANDHELD>()
                .Property(e => e.DEVICE_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_HANDHELD>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_BARCOD>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_BARCOD>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_BARCOD>()
                .Property(e => e.ITEM_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_BARCOD>()
                .Property(e => e.DIM_1_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_BARCOD>()
                .Property(e => e.DIM_2_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_BARCOD>()
                .Property(e => e.DIM_3_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_BARCOD>()
                .Property(e => e.BARCOD)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_BARCOD>()
                .Property(e => e.UNIT)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.ITEM_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_QTY_1)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_UNIT_1)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_1_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_1_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_1)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_QTY_2)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_UNIT_2)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_2_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_2_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_2)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_QTY_3)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_UNIT_3)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_3_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_3_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_3)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_QTY_4)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_UNIT_4)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_4_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_4_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_4)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_QTY_5)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_UNIT_5)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_5_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_5_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_5)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_QTY_6)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.CNT_UNIT_6)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_6_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_6_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_6)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.FRZ_QTY_ON_HND)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.AVG_COST)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_CNT_TRX>()
                .Property(e => e.UNIT_RETL_VAL)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_CNT_TRX_CELL>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX_CELL>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX_CELL>()
                .Property(e => e.ITEM_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX_CELL>()
                .Property(e => e.DIM_1_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX_CELL>()
                .Property(e => e.DIM_2_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX_CELL>()
                .Property(e => e.DIM_3_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_CNT_TRX_CELL>()
                .Property(e => e.FRZ_QTY_ON_HND)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_GRID_DIMS>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_GRID_DIMS>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_GRID_DIMS>()
                .Property(e => e.ITEM_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_GRID_DIMS>()
                .Property(e => e.DIM_1_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_GRID_DIMS>()
                .Property(e => e.DIM_2_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_GRID_DIMS>()
                .Property(e => e.DIM_3_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV>()
                .Property(e => e.ITEM_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV>()
                .Property(e => e.BIN_1)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV>()
                .Property(e => e.BIN_2)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV>()
                .Property(e => e.BIN_3)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV>()
                .Property(e => e.BIN_4)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV_CELL>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV_CELL>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV_CELL>()
                .Property(e => e.ITEM_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV_CELL>()
                .Property(e => e.DIM_1_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV_CELL>()
                .Property(e => e.DIM_2_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV_CELL>()
                .Property(e => e.DIM_3_UPR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_INV_CELL>()
                .Property(e => e.CELL_STAT)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ITEM_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.DESCR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ITEM_TYP)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.TRK_METH)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.STK_UNIT)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_1_UNIT)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_1_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_1_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_2_UNIT)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_2_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_2_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_3_UNIT)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_3_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_3_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_4_UNIT)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_4_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_4_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_5_UNIT)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_5_NUMER)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.ALT_5_DENOM)
                .HasPrecision(11, 0);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.GRID_DIM_1_TAG)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.GRID_DIM_2_TAG)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.GRID_DIM_3_TAG)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_ITEM>()
                .Property(e => e.PRC_1)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.ITEM_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.SER_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.STAT)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PREV_STAT)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.SER_COST)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.MAN_ENTD)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.VEND_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_COD_1)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_ALPHA_1)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_NO_1)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_1_STR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_COD_2)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_ALPHA_2)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_NO_2)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_2_STR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_COD_3)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_ALPHA_3)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_NO_3)
                .HasPrecision(15, 4);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.PROMPT_3_STR)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_IM_SER>()
                .Property(e => e.LST_MAINT_USR_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_INV_CONFIG>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_INV_CONFIG>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_RAW_SCAN_DATA>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_RAW_SCAN_DATA>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_RAW_SCAN_DATA>()
                .Property(e => e.SECTION_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_RAW_SCAN_DATA>()
                .Property(e => e.BARCOD)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_RAW_SCAN_DATA>()
                .Property(e => e.ITEM_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_RAW_SCAN_DATA>()
                .Property(e => e.HANDHELD_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_RAW_SCAN_DATA>()
                .Property(e => e.ISFORCED)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_SCAN_SECTIONS>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_SCAN_SECTIONS>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_SCAN_SECTIONS>()
                .Property(e => e.SECTION_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_USR_ACCT>()
                .Property(e => e.COMPANY_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_USR_ACCT>()
                .Property(e => e.LOC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_USR_ACCT>()
                .Property(e => e.USR_NAM)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_USR_ACCT>()
                .Property(e => e.PWD)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_USR_ACCT>()
                .Property(e => e.ALLOW_FORCE)
                .IsUnicode(false);

            modelBuilder.Entity<RAPID_USR_ACCT>()
                .Property(e => e.IS_MANAGER)
                .IsUnicode(false);
        }

        public System.Data.Entity.DbSet<RapidInventory2015.Models.VarianceCRUD> VarianceCRUDs { get; set; }
    }
}
