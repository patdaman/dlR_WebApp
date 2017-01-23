using System;
using System.Collections.Generic;
using System.Web;
using System.Web.DynamicData;

namespace Rapid_e4473.Models
{
    public class barcode
    {
        public string BARCODE { get; set; }
        public string IMAGEURL { get; set; }
        public byte[] BARCODEIMG { get; set; }
    }
}