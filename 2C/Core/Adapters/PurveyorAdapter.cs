using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Adapters
{
    internal class PurveyorAdapter : BaseAdapter<Purveyor>
    {
        protected override Purveyor GetModelFromDataRow(DataRow row)
        {
            var res = new Purveyor();
            res.Id = row.Field<int>(ModelHelper.GetIdFieldName<Purveyor>());
            res.Name = row.Field<string>("Name");
            return null;
        }
    }
}
