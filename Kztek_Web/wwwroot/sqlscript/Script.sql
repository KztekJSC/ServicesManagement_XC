
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

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Vehicle')
BEGIN

CREATE TABLE [dbo].[Vehicle](
	[Id] [VARCHAR](128) NOT NULL,
	[Phone] [varchar](200) NULL,
	[Name] [nvarchar](500) NULL,
	[Plate] [varchar](200) NULL,
	[GroupId] [varchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VehicleEvent')
BEGIN

CREATE TABLE [dbo].[VehicleEvent](
	[Id] [VARCHAR](128) NOT NULL,
	[EventCode] [varchar](100) NULL,
	[PlateIn] [varchar](150) NULL,
	[PicVehicleIn] [varchar](max) NULL,
	[PicAllIn] [varchar](max) NULL,
	[GateIn] [varchar](150) NULL,
	[DateTimeIn] [datetime] NOT NULL,
	[PlateOut] [varchar](150) NULL,
	[PicVehicleOut] [varchar](max) NULL,
	[PicAllOut] [varchar](max) NULL,
	[GateOut] [varchar](150) NULL,
	[DateTimeOut] [datetime] NOT NULL,
	[GroupId] [varchar](150) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDelete] bit NOT NULL default (0),
 CONSTRAINT [PK_VehicleEvent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCamera' AND COLUMN_NAME = 'MotionZone')
BEGIN
	ALTER TABLE [tblCamera] ADD MotionZone nvarchar(128) NULL DEFAULT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCamera' AND COLUMN_NAME = 'Config')
BEGIN
	ALTER TABLE [tblCamera] ADD Config nvarchar(256) NULL DEFAULT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Vehicle' AND COLUMN_NAME = 'VehicleType')
BEGIN
	ALTER TABLE [Vehicle] ADD VehicleType int NOT NULL DEFAULT 0
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Vehicle' AND COLUMN_NAME = 'Weight')
BEGIN
	ALTER TABLE [Vehicle] ADD Weight int NOT NULL DEFAULT 0
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = 'adf03611-dae6-42c6-8a8f-0aebbc35b7b5')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('adf03611-dae6-42c6-8a8f-0aebbc35b7b5', N'Sự kiện xe kín','Report','1','ReportPrivateOut','','fa fa-caret-right','81478553','1','0','4','','','12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '718266a9-21a3-433c-8e8b-c24973bec9be')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('718266a9-21a3-433c-8e8b-c24973bec9be', N'Sự kiện xe hở','Report','1','ReportOpenOut','','fa fa-caret-right','81478553','1','0','5','','','12878956','0')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblGate' AND COLUMN_NAME = 'IsXi')
BEGIN
	ALTER TABLE [tblGate] ADD IsXi bit NOT NULL DEFAULT 0
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = 'f617b240-5352-4d03-82a0-deb7ab49dbb4')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('f617b240-5352-4d03-82a0-deb7ab49dbb4', N'Lượt xe chở xỉ','Report','1','ReportEventXi','','fa fa-caret-right','81478553','1','0','3','','','12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '24497936-479b-4a63-b9a5-697d827b2515')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('24497936-479b-4a63-b9a5-697d827b2515', N'Xe chưa đăng ký','Report','1','ReportUnRegistered','','fa fa-caret-right','81478553','1','0','3','','','12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = 'e07e12cd-3e4a-4129-970b-b95e106aa833')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('e07e12cd-3e4a-4129-970b-b95e106aa833', N'Sự kiện vào/ra','Report','1','ReportVehicleComeInOut','','fa fa-caret-right','81478553','1','0','3','','','12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = 'ff51ce87-d302-4e39-9e2c-0307f5c40276')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('ff51ce87-d302-4e39-9e2c-0307f5c40276', N'Máy tinh /Làn','tbl_Lane_PC','1','Index','','fa fa-caret-right','81507875','1','0','5','','','12878956','0')
END
IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = 'a9f2487a-d4a4-4d7d-8faf-daf67ea1033c')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('ff51ce87-d302-4e39-9e2c-0307f5c40276', N'LED','tblLED','1','Index','','fa fa-caret-right','81507875','1','0','4','','','12878956','0')
END
Update [dbo].[MenuFunction]
set MenuName = N'Lượt xe kín'
where Id = 'adf03611-dae6-42c6-8a8f-0aebbc35b7b5'

Update [dbo].[MenuFunction]
set MenuName = N'Lượt xe hở'
where Id = '718266a9-21a3-433c-8e8b-c24973bec9be'

Update [dbo].[MenuFunction]
set MenuName = N'Sự kiện nhiễu'
where Id = 'b099b19f-f5dc-412f-8a97-95558017be44'

Update [dbo].[MenuFunction]
set MenuName = N'Sự kiện vào/ra'
where Id = 'e07e12cd-3e4a-4129-970b-b95e106aa833'