using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class DepreciationModel
    {

        public int DepreciationID            { get; set; }
        public string AHName                 { get; set; }
        public int AHID                      { get; set; }
        public byte? Rate                     { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        public int StatusID                  { get; set; }
    }
}