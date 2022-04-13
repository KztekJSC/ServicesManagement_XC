﻿
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Group')
BEGIN

CREATE TABLE [dbo].[Group](
	[Id] [VARCHAR](128) NOT NULL,
	[Code] [nvarchar](200) NULL,
	[Name] [nvarchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Service')
BEGIN

CREATE TABLE [dbo].[Service](
	[Id] [VARCHAR](128) NOT NULL,
	[Code] [nvarchar](200) NULL,
	[Name] [nvarchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'Cost')
BEGIN
	ALTER TABLE [tbl_Event] ADD Cost decimal not null DEFAULT 0
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'PaymentStatus')
BEGIN
	ALTER TABLE [tbl_Event] ADD PaymentStatus varchar(10) default('')
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'ServiceCode')
BEGIN
	ALTER TABLE [tbl_Event] ADD ServiceCode varchar(250) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'PackageNumber')
BEGIN
	ALTER TABLE [tbl_Event] ADD PackageNumber int not null default(0)
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'Quantity')
BEGIN
	ALTER TABLE [tbl_Event] ADD Quantity int not null default(0)
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'DivisionDate')
BEGIN
	ALTER TABLE [tbl_Event] ADD DivisionDate Datetime not null default('1001-01-01')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'ParkingPosition')
BEGIN
	ALTER TABLE [tbl_Event] ADD ParkingPosition varchar not null default('')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'VehicleStatusVN')
BEGIN
	ALTER TABLE [tbl_Event] ADD VehicleStatusVN int not null default 0
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'VehicleStatusCN')
BEGIN
	ALTER TABLE [tbl_Event] ADD VehicleStatusCN int not null default 0
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'TimeInVN')
BEGIN
	ALTER TABLE [tbl_Event] ADD TimeInVN Datetime not null  default('2021-01-01 00:00:00.000')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'TimeOutVN')
BEGIN
	ALTER TABLE [tbl_Event] ADD TimeOutVN Datetime not null  default('2021-01-01 00:00:00.000')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'TimeInCN')
BEGIN
	ALTER TABLE [tbl_Event] ADD TimeInCN Datetime not null  default('2021-01-01 00:00:00.000')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'TimeOutCN')
BEGIN
	ALTER TABLE [tbl_Event] ADD TimeOutCN Datetime not null  default('2021-01-01 00:00:00.000')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'ConfirmDate')
BEGIN
	ALTER TABLE [tbl_Event] ADD ConfirmDate Datetime not null  default('2021-01-01 00:00:00.000')
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'BB_Table')
BEGIN
	ALTER TABLE [tbl_Event] ADD BB_Table varchar(250) null
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'BB_Id')
BEGIN
	ALTER TABLE [tbl_Event] ADD BB_Id varchar(250) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'User' AND COLUMN_NAME = 'GroupIds')
BEGIN
	ALTER TABLE [User] ADD GroupIds varchar(500) null
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'User' AND COLUMN_NAME = 'TypeNotifi')
BEGIN
	ALTER TABLE [User] ADD TypeNotifi varchar(50) null 
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLog' AND COLUMN_NAME = 'OldInfo')
BEGIN
	ALTER TABLE tblLog ADD OldInfo nvarchar(max) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLog' AND COLUMN_NAME = 'NewInfo')
BEGIN
	ALTER TABLE tblLog ADD NewInfo nvarchar(max) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ColumTable')
BEGIN

CREATE TABLE [dbo].[ColumTable](
	[Id] [VARCHAR](128) NOT NULL,
	[Controller] [nvarchar](200) NULL,
	[Action] [nvarchar](500) NULL,
	[Columns] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[ColumShows] [varchar](max)  NULL,
 CONSTRAINT [PK_ColumTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'EventIn')
BEGIN

CREATE TABLE [dbo].[EventIn](
	[Id] [VARCHAR](128) NOT NULL,
	[Plate] [varchar](200) NULL,
	[PlateUnsign] [varchar](200) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[TimeIn] [datetime] NOT NULL,
 CONSTRAINT [PK_EventIn] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_LED' AND COLUMN_NAME = 'row')
BEGIN
	ALTER TABLE tbl_LED ADD row int not null default(32)  
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_LED' AND COLUMN_NAME = 'column_Led')
BEGIN
	ALTER TABLE tbl_LED ADD column_Led int  not null default(48)  
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_LED' AND COLUMN_NAME = 'fontSize')
BEGIN
	ALTER TABLE tbl_LED ADD fontSize int not null default(7)  
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_LED' AND COLUMN_NAME = 'color')
BEGIN
	ALTER TABLE tbl_LED ADD color int  not null default(1)  
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Event' AND COLUMN_NAME = 'TimeIntend')
BEGIN
	ALTER TABLE tbl_Event ADD TimeIntend datetime  not null  default('2021-01-01 00:00:00.000')
END