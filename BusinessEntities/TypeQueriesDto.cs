using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class MasterDto
    {
    }

    public class TypeQueryDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Value1 { get; set; }

        public string Value2 { get; set; }

        public string Value3 { get; set; }

        public string Value4 { get; set; }

        public string Value5 { get; set; }
    }

    public class TypeQueryResult : List<TypeQueryDto>
    {

    }
}
