using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    internal class MappingInfo
    {
        internal PropertyInfo Property { get; }
        internal string FieldName { get; }
        internal MappingType MappingType { get; }
        internal MappingInfo(PropertyInfo propertyInfo, string fieldName, MappingType mappingType)
        {
            Property = propertyInfo;
            FieldName = fieldName;
            MappingType = mappingType;
        }
    }

    internal enum MappingType
    {
        Direct,
        ForeignKey
    }
}
