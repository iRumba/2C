using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Attributes
{
    public sealed class IdFieldNameAttribute : Attribute
    {
        public string Name { get; }

        public IdFieldNameAttribute(string name)
        {
            Name = name;
        }
    }
}
