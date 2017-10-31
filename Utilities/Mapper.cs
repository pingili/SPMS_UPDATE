using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Utilities
{
    public class Mapper1
    {
        public static void Map(Object sourceObj, Object destObj)
        {
            foreach (var src in sourceObj.GetType().GetProperties())
            {
                foreach (var dst in destObj.GetType().GetProperties())
                {
                    if (dst.Name.ToUpper() == src.Name.ToUpper())
                    {
                        if (!dst.PropertyType.FullName.Contains("LaJitLLBgen."))
                            dst.SetValue(destObj, src.GetValue(sourceObj));
                    }
                }
                    
            }
        }

        
    }
}