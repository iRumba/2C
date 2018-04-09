using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Adapters
{
    internal class GoodsAdapter : BaseAdapter<Goods>
    {
        protected override Goods GetModelFromDataRow(DataRow row)
        {
            var res = new Goods();
            res.Id = row.Field<int>(ModelHelper.GetIdFieldName<Goods>());
            res.Markup = row.Field<double>("Markup");
            res.Name = row.Field<string>("Name");

            res.Image = row.Field<string>("Image");

            return res;
        }
    }
}
