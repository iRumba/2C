using Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public class DbManager
    {
        Configuration _configuration;

        public DbManager(Configuration configuration)
        {
            _configuration = configuration;
        }

        internal SqlConnection GetConnection()
        {
            var res = new SqlConnection(_configuration.ConnectionString);
            return res;
        }

        public async Task<bool> IsBaseExists()
        {
            var csb = new SqlConnectionStringBuilder(_configuration.ConnectionString);
            var dbName = csb.InitialCatalog;
            csb.InitialCatalog = null;
            using (var con = new SqlConnection(csb.ToString()))
            {
                await con.OpenAsync();
                using(var com = con.CreateCommand())
                {
                    com.CommandText = "SELECT count(*) FROM sysdatabases WHERE name=@name";
                    com.Parameters.Add(new SqlParameter("@name", dbName));
                    using(var reader = await com.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        return reader.GetInt32(0) == 1;
                    }
                }
            }
        }

        public async Task CreateDatabase()
        {
            var csb = new SqlConnectionStringBuilder(_configuration.ConnectionString);
            var dbName = csb.InitialCatalog;
            csb.InitialCatalog = null;
            using (var con = new SqlConnection(csb.ToString()))
            {
                await con.OpenAsync();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = $"CREATE DATABASE [{dbName}]";
                    await com.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DropDatabase()
        {
            var csb = new SqlConnectionStringBuilder(_configuration.ConnectionString);
            var dbName = csb.InitialCatalog;
            csb.InitialCatalog = null;
            using (var con = new SqlConnection(csb.ToString()))
            {
                await con.OpenAsync();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = $"DROP DATABASE [{dbName}]";
                    await com.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<CheckConnectionResult> CheckConnection()
        {
            var res = CheckConnectionResult.ServerConnectionError;
            try
            {
                var csb = new SqlConnectionStringBuilder(_configuration.ConnectionString);
                csb.InitialCatalog = null;
                using (var con = new SqlConnection(csb.ToString()))
                {
                    await con.OpenAsync();
                    con.Close();
                }
                res = CheckConnectionResult.DatabaseConnectionError;
                using (var con = GetConnection())
                {
                    await con.OpenAsync();
                    con.Close();
                }
                res = CheckConnectionResult.Success;
            }
            catch { }
            return res;
        }

        public async Task SetupDatabase()
        {
            using (var con = GetConnection())
            {
                await con.OpenAsync();
                var com = con.CreateCommand();
                var transaction = con.BeginTransaction();
                com.Transaction = transaction;
                try
                {
                    foreach(var query in GetCreateScripts())
                    {
                        com.CommandText = query;
                        await com.ExecuteNonQueryAsync();
                    }
                    
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        string[] GetCreateScripts()
        {
            return new string[]
            {
                GetCreateGoodsTableScript(),
                GetCreatePurveyorTableScript(),
                GetCreateWorkerTableScript(),
                GetCreatePurchaserTableScript(),
                GetCreateArrivalTableScript(),
                GetCreateArrivalDetailsTableScript(),
                GetCreateOrderTableScript(),
                GetCreateOrderDetailsTableScript(),
                GetCreateGoodsDetailsViewScript()
            };
        }

        string GetCreateHeader()
        {
            return "";
        }

        string GetCreateGoodsTableScript()
        {
            return $@"{GetCreateHeader()}
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Goods>()}](
	[{ModelHelper.GetIdFieldName<Goods>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Goods>(nameof(Goods.Name))}] [nvarchar](100) NOT NULL,
	[{ModelHelper.GetColumnName<Goods>(nameof(Goods.Markup))}] [float] NOT NULL,
	[{ModelHelper.GetColumnName<Goods>(nameof(Goods.Image))}] [nvarchar](max) NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Goods>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Goods>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
        }

        string GetCreatePurveyorTableScript()
        {
            return $@"{GetCreateHeader()}
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Purveyor>()}](
	[{ModelHelper.GetIdFieldName<Purveyor>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Purveyor>(nameof(Purveyor.Name))}] [nvarchar](100) NOT NULL,
	[{ModelHelper.GetColumnName<Purveyor>(nameof(Purveyor.TelephoneNumber))}] [varchar](20) NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Purveyor>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Purveyor>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]";
        }

        string GetCreateWorkerTableScript()
        {
            return $@"{GetCreateHeader()}
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Worker>()}](
	[{ModelHelper.GetIdFieldName<Worker>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Worker>(nameof(Worker.Name))}] [nvarchar](100) NOT NULL,
	[{ModelHelper.GetColumnName<Worker>(nameof(Worker.Post))}] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Worker>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Worker>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]";
        }

        string GetCreatePurchaserTableScript()
        {
            return $@"{GetCreateHeader()}
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Purchaser>()}](
	[{ModelHelper.GetIdFieldName<Purchaser>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Purchaser>(nameof(Purchaser.Name))}] [nvarchar](100) NOT NULL,
	[{ModelHelper.GetColumnName<Purchaser>(nameof(Purchaser.TelephoneNumber))}] [varchar](20) NULL,
	[{ModelHelper.GetColumnName<Purchaser>(nameof(Purchaser.Address))}] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Purchaser>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Purchaser>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
        }

        string GetCreateArrivalTableScript()
        {
            return $@"{GetCreateHeader()}
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Arrival>()}](
	[{ModelHelper.GetIdFieldName<Arrival>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Purveyor))}] [int] NOT NULL,
	[{ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Date))}] [datetime] NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Arrival>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Arrival>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Arrival>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<Arrival>()}_{ModelHelper.GetModelTableName<Purveyor>()}] FOREIGN KEY([{ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Purveyor))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Purveyor>()}] ([{ModelHelper.GetIdFieldName<Purveyor>()}])

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Arrival>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<Arrival>()}_{ModelHelper.GetModelTableName<Purveyor>()}]";
        }

        string GetCreateArrivalDetailsTableScript()
        {
            return $@"{GetCreateHeader()}
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<ArrivalDetails>()}](
	[{ModelHelper.GetIdFieldName<ArrivalDetails>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Arrival))}] [int] NOT NULL,
	[{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Goods))}] [int] NOT NULL,
	[{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Amount))}] [int] NOT NULL,
	[{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Price))}] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<ArrivalDetails>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<ArrivalDetails>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<ArrivalDetails>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<ArrivalDetails>()}_{ModelHelper.GetModelTableName<Arrival>()}] FOREIGN KEY([{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Arrival))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Arrival>()}] ([{ModelHelper.GetIdFieldName<Arrival>()}])

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<ArrivalDetails>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<ArrivalDetails>()}_{ModelHelper.GetModelTableName<Arrival>()}]

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<ArrivalDetails>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<ArrivalDetails>()}_{ModelHelper.GetModelTableName<Goods>()}] FOREIGN KEY([{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Goods))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Goods>()}] ([{ModelHelper.GetIdFieldName<Goods>()}])

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<ArrivalDetails>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<ArrivalDetails>()}_{ModelHelper.GetModelTableName<Goods>()}]";
        }

        string GetCreateOrderTableScript()
        {
            return $@"{GetCreateHeader()}
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Order>()}](
	[{ModelHelper.GetIdFieldName<Order>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Order>(nameof(Order.Purchaser))}] [int] NOT NULL,
	[{ModelHelper.GetColumnName<Order>(nameof(Order.Worker))}] [int] NOT NULL,
	[{ModelHelper.GetColumnName<Order>(nameof(Order.OrderDate))}] [datetime] NOT NULL,
	[{ModelHelper.GetColumnName<Order>(nameof(Order.DepartureDate))}] [datetime] NULL,
	[{ModelHelper.GetColumnName<Order>(nameof(Order.ArrivalDate))}] [datetime] NULL,
	[{ModelHelper.GetColumnName<Order>(nameof(Order.DeliveryMethod))}] [varchar](20) NOT NULL,
	[{ModelHelper.GetColumnName<Order>(nameof(Order.PaymentMethod))}] [varchar](20) NOT NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Order>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Order>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Order>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<Order>()}_{ModelHelper.GetModelTableName<Purchaser>()}] FOREIGN KEY([{ModelHelper.GetColumnName<Order>(nameof(Order.Purchaser))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Purchaser>()}] ([{ModelHelper.GetIdFieldName<Purchaser>()}])

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Order>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<Order>()}_{ModelHelper.GetModelTableName<Purchaser>()}]

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Order>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<Order>()}_{ModelHelper.GetModelTableName<Worker>()}] FOREIGN KEY([{ModelHelper.GetColumnName<Order>(nameof(Order.Worker))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Worker>()}] ([{ModelHelper.GetIdFieldName<Worker>()}])

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Order>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<Order>()}_{ModelHelper.GetModelTableName<Worker>()}]";
        }

        string GetCreateOrderDetailsTableScript()
        {
            return $@"{GetCreateHeader()}
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<OrderDetails>()}](
	[{ModelHelper.GetIdFieldName<OrderDetails>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Goods))}] [int] NOT NULL,
	[{ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Order))}] [int] NOT NULL,
	[{ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Amount))}] [int] NOT NULL,
	[{ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Price))}] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<OrderDetails>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<OrderDetails>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<OrderDetails>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<OrderDetails>()}_{ModelHelper.GetModelTableName<Goods>()}] FOREIGN KEY([{ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Goods))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Goods>()}] ([{ModelHelper.GetIdFieldName<Goods>()}])

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<OrderDetails>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<OrderDetails>()}_{ModelHelper.GetModelTableName<Goods>()}]

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<OrderDetails>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<OrderDetails>()}_{ModelHelper.GetModelTableName<Order>()}] FOREIGN KEY([{ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Order))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Order>()}] ([{ModelHelper.GetIdFieldName<Order>()}])

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<OrderDetails>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<OrderDetails>()}_{ModelHelper.GetModelTableName<Order>()}]";
        }

        string GetCreateGoodsDetailsViewScript()
        {
            return $@"{GetCreateHeader()}
CREATE VIEW [dbo].[GoodsDetails]
WITH SCHEMABINDING 
AS
SELECT g1.{ModelHelper.GetIdFieldName<Goods>()} Id, g2.currentPrice Price, ad4.arrivals - od1.orders Balance FROM dbo.{ModelHelper.GetModelTableName<Goods>()} g1
OUTER APPLY(
	SELECT ISNULL(sum({ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Amount))}), 0) arrivals FROM dbo.{ModelHelper.GetModelTableName<ArrivalDetails>()} WHERE {ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Goods))} = g1.{ModelHelper.GetIdFieldName<Goods>()}
) ad4
OUTER APPLY(
	SELECT ISNULL(sum({ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Amount))}), 0) orders FROM dbo.{ModelHelper.GetModelTableName<OrderDetails>()} WHERE {ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Goods))} = g1.{ModelHelper.GetIdFieldName<Goods>()}
) od1
OUTER APPLY
(SELECT MAX(ad1.{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Price))}) * (g1.{ModelHelper.GetColumnName<Goods>(nameof(Goods.Markup))} + 1) currentPrice FROM dbo.{ModelHelper.GetModelTableName<ArrivalDetails>()} ad1
JOIN dbo.{ModelHelper.GetModelTableName<Arrival>()} a1 ON a1.{ModelHelper.GetIdFieldName<Arrival>()} = ad1.{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Arrival))}
OUTER APPLY(
	SELECT Sum(Amount) sum FROM dbo.{ModelHelper.GetModelTableName<ArrivalDetails>()} ad2
		JOIN dbo.{ModelHelper.GetModelTableName<Arrival>()} a2 ON a2.{ModelHelper.GetIdFieldName<Arrival>()} = ad2.{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Arrival))}
	WHERE a1.{ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Date))} >= a2.{ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Date))} AND ad2.{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Goods))} = g1.{ModelHelper.GetIdFieldName<Goods>()}
) ad2sum
WHERE ad1.{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Goods))} = g1.{ModelHelper.GetIdFieldName<Goods>()} AND ad2sum.sum > od1.orders
) g2
";
        }

        //List<Type> SortModelTypesByDependencies(Type[] modelTypes)
        //{
        //    var res = new List<Type>();
        //    foreach(var type in modelTypes)
        //    {
        //        var fks = ModelHelper.GetMappingInfo(type).Where(mi => mi.MappingType == MappingType.ForeignKey).Select(mi => mi.Property.PropertyType);

        //        Type insertAfter = null;

        //        foreach (var model in res)
        //        {
        //            if (fks.Contains(model))
        //                insertAfter = model;
        //        }

        //        if (insertAfter == null)
        //            res.Insert(0, type);
        //        else
        //        {
        //            res.Insert(res.IndexOf(insertAfter))
        //        }
        //    }
        //}
    }

    public enum CheckConnectionResult
    {
        Success,
        ServerConnectionError,
        DatabaseConnectionError
    }
}
