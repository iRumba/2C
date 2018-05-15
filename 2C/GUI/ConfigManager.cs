using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    static class ConfigManager
    {
        public static Configuration GetDefault()
        {
            return new Configuration()
            {
                ConnectionString = @"Server=IRUMBA-PC\MSSQL;Database=2C_DB;Trusted_Connection=True;"
            };
        }

        public static Configuration LoadFromFile(string fileName)
        {
            return new Configuration
            {
                ConnectionString = File.ReadAllText(fileName).Trim()
            };
        }
    }
}
