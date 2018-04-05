using Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Adapters
{
    public class GoodsAdapter : BaseAdapter<Goods>
    {
        protected override Goods GetModelFromDataRow(DataRow row)
        {
            var res = new Goods
            {
                Id = row.Field<int>("Id"),
                Markup = row.Field<float>("Markup"),
                Name = row.Field<string>("Name")
            };

            var filePath = row.Field<string>("Image");
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                res.Image = File.ReadAllBytes(filePath);

            return res;
        }
    }
}
