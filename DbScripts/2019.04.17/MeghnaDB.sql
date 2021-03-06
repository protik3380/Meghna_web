
USE [MeghnaDB]
GO
/****** Object:  User [USER\shira_000]    Script Date: 17-Apr-19 5:43:37 PM ******/
CREATE USER [USER\shira_000] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [NC-PC\tayab]    Script Date: 17-Apr-19 5:43:37 PM ******/
CREATE USER [NC-PC\tayab] FOR LOGIN [NC-PC\tayab] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [USER\shira_000]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 17-Apr-19 5:43:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Distributor]    Script Date: 17-Apr-19 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Distributor](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[ContactPerson] [nvarchar](100) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [varchar](50) NULL,
	[MasterDepotId] [bigint] NULL,
	[LocationId] [bigint] NULL,
 CONSTRAINT [PK_Distributor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DistributorProductLine]    Script Date: 17-Apr-19 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DistributorProductLine](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DistributorId] [bigint] NULL,
	[ProductLineId] [bigint] NULL,
 CONSTRAINT [PK_DistributorProductLine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 17-Apr-19 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterDepot]    Script Date: 17-Apr-19 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterDepot](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[ContactPerson] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [varchar](50) NULL,
	[LocationId] [bigint] NULL,
 CONSTRAINT [PK_MasterDepot] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterDepotProductLine]    Script Date: 17-Apr-19 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterDepotProductLine](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MasterDepotId] [bigint] NULL,
	[ProductLineId] [bigint] NULL,
 CONSTRAINT [PK_MasterDepotProductLine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 17-Apr-19 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderNo] [varchar](50) NULL,
	[CustomerName] [nvarchar](50) NULL,
	[MobileNo] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[DeliveryAddress] [nvarchar](500) NULL,
	[LocationId] [bigint] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 17-Apr-19 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderId] [bigint] NULL,
	[ProductId] [bigint] NULL,
	[Quantity] [int] NULL,
	[Price] [decimal](18, 2) NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 17-Apr-19 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Unit] [nvarchar](50) NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[Description] [nvarchar](500) NULL,
	[CategoryId] [bigint] NULL,
	[Image] [varchar](500) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductLine]    Script Date: 17-Apr-19 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductLine](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_ProductLine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductLineDetails]    Script Date: 17-Apr-19 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductLineDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductLineId] [bigint] NULL,
	[ProductId] [bigint] NULL,
 CONSTRAINT [PK_ProductLineDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Distributor]  WITH CHECK ADD  CONSTRAINT [FK_Distributor_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Distributor] CHECK CONSTRAINT [FK_Distributor_Location]
GO
ALTER TABLE [dbo].[Distributor]  WITH CHECK ADD  CONSTRAINT [FK_Distributor_MasterDepot] FOREIGN KEY([MasterDepotId])
REFERENCES [dbo].[MasterDepot] ([Id])
GO
ALTER TABLE [dbo].[Distributor] CHECK CONSTRAINT [FK_Distributor_MasterDepot]
GO
ALTER TABLE [dbo].[DistributorProductLine]  WITH CHECK ADD  CONSTRAINT [FK_DistributorProductLine_Distributor] FOREIGN KEY([DistributorId])
REFERENCES [dbo].[Distributor] ([Id])
GO
ALTER TABLE [dbo].[DistributorProductLine] CHECK CONSTRAINT [FK_DistributorProductLine_Distributor]
GO
ALTER TABLE [dbo].[DistributorProductLine]  WITH CHECK ADD  CONSTRAINT [FK_DistributorProductLine_ProductLine] FOREIGN KEY([ProductLineId])
REFERENCES [dbo].[ProductLine] ([Id])
GO
ALTER TABLE [dbo].[DistributorProductLine] CHECK CONSTRAINT [FK_DistributorProductLine_ProductLine]
GO
ALTER TABLE [dbo].[MasterDepot]  WITH CHECK ADD  CONSTRAINT [FK_MasterDepot_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[MasterDepot] CHECK CONSTRAINT [FK_MasterDepot_Location]
GO
ALTER TABLE [dbo].[MasterDepotProductLine]  WITH CHECK ADD  CONSTRAINT [FK_MasterDepotProductLine_MasterDepot] FOREIGN KEY([MasterDepotId])
REFERENCES [dbo].[MasterDepot] ([Id])
GO
ALTER TABLE [dbo].[MasterDepotProductLine] CHECK CONSTRAINT [FK_MasterDepotProductLine_MasterDepot]
GO
ALTER TABLE [dbo].[MasterDepotProductLine]  WITH CHECK ADD  CONSTRAINT [FK_MasterDepotProductLine_ProductLine] FOREIGN KEY([ProductLineId])
REFERENCES [dbo].[ProductLine] ([Id])
GO
ALTER TABLE [dbo].[MasterDepotProductLine] CHECK CONSTRAINT [FK_MasterDepotProductLine_ProductLine]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Location]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Order]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category]
GO
ALTER TABLE [dbo].[ProductLineDetails]  WITH CHECK ADD  CONSTRAINT [FK_ProductLineDetails_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[ProductLineDetails] CHECK CONSTRAINT [FK_ProductLineDetails_Product]
GO
ALTER TABLE [dbo].[ProductLineDetails]  WITH CHECK ADD  CONSTRAINT [FK_ProductLineDetails_ProductLine] FOREIGN KEY([ProductLineId])
REFERENCES [dbo].[ProductLine] ([Id])
GO
ALTER TABLE [dbo].[ProductLineDetails] CHECK CONSTRAINT [FK_ProductLineDetails_ProductLine]
GO
USE [master]
GO
ALTER DATABASE [MeghnaDB] SET  READ_WRITE 
GO
