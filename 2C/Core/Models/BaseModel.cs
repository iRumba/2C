using Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Core.Models
{
    public abstract class BaseModel: ICloneable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        internal BaseModel()
        {
            ForeignKeys = new Dictionary<string, int>();
        }

        internal Dictionary<string, int> ForeignKeys { get; }

        public int GetForeignKey(string propertyName)
        {
            var type = GetType();
            var prop = type.GetProperty(propertyName);
            var fkFieldName = ModelHelper.GetFKFieldName(prop);
            return ForeignKeys[fkFieldName];
        }

        public void ResetForeignKey(string propertyName)
        {
            var type = GetType();
            var prop = type.GetProperty(propertyName);
            var fkFieldName = ModelHelper.GetFKFieldName(prop);
            ForeignKeys[fkFieldName] = -1;
            prop.SetValue(this, null);
        }

        public virtual object Clone()
        {
            var res = (BaseModel)MemberwiseClone();

            return MemberwiseClone();
        }
    }
}
