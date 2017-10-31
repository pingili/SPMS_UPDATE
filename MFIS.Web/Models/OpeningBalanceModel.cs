using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Models
{
    public class OpeningBalanceModel
    {
        public List<OpeningBalanceModelLiabilities> openingBalanceModelLiabilities = new List<OpeningBalanceModelLiabilities>();
        public List<OpeningBalanceModelAssets> openingBalanceModelAssets = new List<OpeningBalanceModelAssets>();
        public double AssetTotal { get; set; }
        public double LiabilitiesTotal { get; set; }
    }
    public class OpeningBalanceModelAssets
    {
        public int AssetAHID { get; set; }
        public string AssetAHCode { get; set; }
        public string AssetAHName { get; set; }
        public double AssetOpeningBalance { get; set; }
       
    }
    public class OpeningBalanceModelLiabilities
    {
        public int LiabilitiesAHID { get; set; }
        public string LiabilitiesAHCode { get; set; }
        public string LiabilitiesAHName { get; set; }
        public double LiabilitiesOpeningBalance { get; set; }
       
    }
}