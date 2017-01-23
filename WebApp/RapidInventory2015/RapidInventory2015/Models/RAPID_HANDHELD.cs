namespace RapidInventory2015.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RAPID_HANDHELD
    {
        [Key]
        [StringLength(200)]
        public string DEVICE_ID { get; set; }

        [Required]
        [StringLength(20)]
        public string NAME { get; set; }
    }
}
