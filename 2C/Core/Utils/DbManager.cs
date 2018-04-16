using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    internal class DbManager
    {
        string _connectionString;

        const string CREATE_GOODS_TABLE_SCRIPT = @"";

        const string CREATE_PURVEYOR_TABLE_SCRIPT = @"";

        const string CREATE_WORKER_TABLE_SCRIPT = @"";

        const string CREATE_PURCHASER_TABLE_SCRIPT = @"";

        const string CREATE_ARRIVAL_TABLE_SCRIPT = @"";

        const string CREATE_ARRIVALDETAILS_TABLE_SCRIPT = @"";

        const string CREATE_ORDER_TABLE_SCRIPT = @"";

        const string CREATE_ORDERDETAILS_TABLE_SCRIPT = @"";

        const string CREATE_GOODSDETAILS_VIEW_SCRIPT = @"SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[GoodsDetails]
WITH SCHEMABINDING 
AS
SELECT g1.Id Id, g2.currentPrice Price, ad4.arrivals - od1.orders Balance FROM dbo.Goods g1
OUTER APPLY(
	SELECT ISNULL(sum(Amount), 0) arrivals FROM dbo.ArrivalDetails WHERE GoodsId = g1.Id
) ad4
OUTER APPLY(
	SELECT ISNULL(sum(Amount), 0) orders FROM dbo.OrderDetails WHERE GoodsId = g1.Id
) od1
OUTER APPLY
(SELECT MAX(ad1.Price) * (g1.Markup + 1) currentPrice FROM dbo.ArrivalDetails ad1
JOIN dbo.Arrivals a1 ON a1.Id = ad1.ArrivalId
OUTER APPLY(
	SELECT Sum(Amount) sum FROM dbo.ArrivalDetails ad2
		JOIN dbo.Arrivals a2 ON a2.Id = ad2.ArrivalId
	WHERE a1.Date >= a2.Date AND ad2.GoodsId = g1.Id
) ad2sum
WHERE ad1.GoodsId = g1.Id AND ad2sum.sum > od1.orders
) g2

GO";

        public DbManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ClearDatabase()
        {

        }

        public void SetupDatabase()
        {

        }

        string GetCreateTablesScript(Type[] modelTypes)
        {
            var types = SortModelTypesByDependencies(modelTypes);
            var res = new StringBuilder(@"SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
");

            return null;
        }

        string GetCreateGoodsTableScript()
        {
            return $@"
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Goods>()}](
	[{ModelHelper.GetIdFieldName<Goods>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Goods>(nameof(Goods.Name))}] [nvarchar](100) NOT NULL,
	[{ModelHelper.GetColumnName<Goods>(nameof(Goods.Markup))}] [float] NOT NULL,
	[{ModelHelper.GetColumnName<Goods>(nameof(Goods.Image))}] [nvarchar](max) NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Goods>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Goods>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO";
        }

        string GetCreatePurveyorTableScript()
        {
            return $@"
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Purveyor>()}](
	[{ModelHelper.GetIdFieldName<Purveyor>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Purveyor>(nameof(Purveyor.Name))}] [nvarchar](100) NOT NULL,
	[{ModelHelper.GetColumnName<Purveyor>(nameof(Purveyor.TelephoneNumber))}] [varchar](20) NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Purveyor>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Purveyor>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO";
        }

        string GetCreateWorkerTableScript()
        {
            return $@"
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Worker>()}](
	[{ModelHelper.GetIdFieldName<Worker>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Worker>(nameof(Worker.Name))}] [nvarchar](100) NOT NULL,
	[{ModelHelper.GetColumnName<Worker>(nameof(Worker.Post))}] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Worker>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Worker>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO";
        }

        string GetCreatePurchaserTableScript()
        {
            return $@"
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Purchaser>()}](
	[{ModelHelper.GetIdFieldName<Purchaser>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Purchaser>(nameof(Purchaser.Name))}] [nvarchar](100) NOT NULL,
	[{ModelHelper.GetColumnName<Purchaser>(nameof(Purchaser.TelephoneNumber))}] [varchar](20) NULL,
	[{ModelHelper.GetColumnName<Purchaser>(nameof(Purchaser.Address))}] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Purchaser>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Purchaser>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO";
        }

        string GetCreateArrivalTableScript()
        {
            return $@"
CREATE TABLE [dbo].[{ModelHelper.GetModelTableName<Arrival>()}](
	[{ModelHelper.GetIdFieldName<Arrival>()}] [int] IDENTITY(1,1) NOT NULL,
	[{ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Purveyor))}] [int] NOT NULL,
	[{ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Date))}] [datetime] NULL,
 CONSTRAINT [PK_{ModelHelper.GetModelTableName<Purchaser>()}] PRIMARY KEY CLUSTERED 
(
	[{ModelHelper.GetIdFieldName<Arrival>()}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Arrival>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<Arrival>()}_{ModelHelper.GetModelTableName<Purveyor>()}] FOREIGN KEY([{ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Purveyor))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Purveyor>()}] ([{ModelHelper.GetIdFieldName<Purveyor>()}])
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Arrival>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<Arrival>()}_{ModelHelper.GetModelTableName<Purveyor>()}]
GO";
        }

        string GetCreateArrivalDetailsTableScript()
        {
            return $@"
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
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<ArrivalDetails>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<ArrivalDetails>()}_{ModelHelper.GetModelTableName<Arrival>()}] FOREIGN KEY([{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Arrival))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Arrival>()}] ([{ModelHelper.GetIdFieldName<Arrival>()}])
GO

ALTER TABLE [dbo].{ModelHelper.GetModelTableName<ArrivalDetails>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<ArrivalDetails>()}_{ModelHelper.GetModelTableName<Arrival>()}]
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<ArrivalDetails>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<ArrivalDetails>()}_{ModelHelper.GetModelTableName<Goods>()}] FOREIGN KEY([{ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Goods))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Goods>()}] ([{ModelHelper.GetIdFieldName<Goods>()}])
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<ArrivalDetails>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<ArrivalDetails>()}_{ModelHelper.GetModelTableName<Goods>()}]
GO";
        }

        string GetCreateOrderTableScript()
        {
            return $@"
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
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Order>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<Order>()}_{ModelHelper.GetModelTableName<Purchaser>()}] FOREIGN KEY([{ModelHelper.GetColumnName<Order>(nameof(Order.Purchaser))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Purchaser>()}] ([{ModelHelper.GetIdFieldName<Purchaser>()}])
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Order>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<Order>()}_{ModelHelper.GetModelTableName<Purchaser>()}]
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Order>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<Order>()}_{ModelHelper.GetModelTableName<Worker>()}] FOREIGN KEY([{ModelHelper.GetColumnName<Order>(nameof(Order.Worker))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Worker>()}] ([{ModelHelper.GetIdFieldName<Worker>()}])
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<Order>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<Order>()}_{ModelHelper.GetModelTableName<Worker>()}]
GO";
        }

        string GetCreateOrderDetailsTableScript()
        {
            return $@"
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
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<OrderDetails>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<OrderDetails>()}_{ModelHelper.GetModelTableName<Goods>()}] FOREIGN KEY([{ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Goods))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Goods>()}] ([{ModelHelper.GetIdFieldName<Goods>()}])
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<OrderDetails>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<OrderDetails>()}_{ModelHelper.GetModelTableName<Goods>()}]
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<OrderDetails>()}]  WITH CHECK ADD  CONSTRAINT [FK_{ModelHelper.GetModelTableName<OrderDetails>()}_{ModelHelper.GetModelTableName<Order>()}] FOREIGN KEY([{ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Order))}])
REFERENCES [dbo].[{ModelHelper.GetModelTableName<Order>()}] ([{ModelHelper.GetIdFieldName<Order>()}])
GO

ALTER TABLE [dbo].[{ModelHelper.GetModelTableName<OrderDetails>()}] CHECK CONSTRAINT [FK_{ModelHelper.GetModelTableName<OrderDetails>()}_{ModelHelper.GetModelTableName<Order>()}]
GO";
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
}
