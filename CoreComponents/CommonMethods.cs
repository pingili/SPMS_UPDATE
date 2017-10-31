using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CoreComponents
{
    public static class CommonMethods
    {
        /// <summary>
        /// Used to Serialize the List<DTO> object into xml. and returns xmlstring.
        /// /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SerializeListDto<T>(T t)
        {
            string XMLString = string.Empty;

            if (t == null)
                return XMLString;

            XmlSerializer x = null;

            try
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    x = new XmlSerializer(typeof(T));

                    x.Serialize(stringwriter, t);

                    XMLString = stringwriter.ToString();
                }
            }
            catch
            {
                XMLString = string.Empty;
            }
            finally
            {
                x = null;
            }

            return XMLString;
        }

       
    }
}
