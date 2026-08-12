USE [master]
GO
/****** Object:  Database [CarRentalDB]    Script Date: 8/12/2026 9:42:36 AM ******/
CREATE DATABASE [CarRentalDB]
 CONTAINMENT = NONE
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CarRentalDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CarRentalDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CarRentalDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CarRentalDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CarRentalDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CarRentalDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CarRentalDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CarRentalDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CarRentalDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CarRentalDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CarRentalDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CarRentalDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CarRentalDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CarRentalDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CarRentalDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CarRentalDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CarRentalDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CarRentalDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CarRentalDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CarRentalDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CarRentalDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CarRentalDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CarRentalDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CarRentalDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CarRentalDB] SET RECOVERY SIMPLE
GO
ALTER DATABASE [CarRentalDB] SET  MULTI_USER 
GO
ALTER DATABASE [CarRentalDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CarRentalDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CarRentalDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CarRentalDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CarRentalDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CarRentalDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CarRentalDB', N'ON'
GO
ALTER DATABASE [CarRentalDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [CarRentalDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200)
GO
USE [CarRentalDB]
GO
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs](
	[AuditLogID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Action] [nvarchar](50) NOT NULL,
	[EntityName] [nvarchar](50) NOT NULL,
	[EntityID] [int] NOT NULL,
	[IPAddress] [varchar](45) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AuditLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookingStatus]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingStatus](
	[BookingStatusID] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BookingStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ContactInformation] [nvarchar](100) NOT NULL,
	[DriverLicenseNumber] [nvarchar](20) NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FuelTypes]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FuelTypes](
	[FuelTypeID] [int] IDENTITY(1,1) NOT NULL,
	[FuelType] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FuelTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Maintenance]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Maintenance](
	[MaintenanceID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleID] [int] NOT NULL,
	[Description] [nvarchar](300) NULL,
	[MaintenanceDate] [datetime] NOT NULL,
	[ExpectedFinishDate] [date] NULL,
	[Cost] [decimal](10, 2) NOT NULL,
	[MaintenanceStatusID] [int] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
	[UpdatedByUserID] [int] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaintenanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaintenanceCompletion]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaintenanceCompletion](
	[CompletionID] [int] IDENTITY(1,1) NOT NULL,
	[MaintenanceID] [int] NOT NULL,
	[CompletedDate] [date] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
	[FinalCost] [decimal](10, 2) NOT NULL,
	[Notes] [nvarchar](500) NULL,
	[VehicleMileage] [int] NOT NULL,
	[IsPassedInspection] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedByUserID] [int] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CompletionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaintenanceStatus]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaintenanceStatus](
	[MaintenanceStatusID] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaintenanceStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RentalBooking]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentalBooking](
	[BookingID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[VehicleID] [int] NOT NULL,
	[RentalStartDate] [date] NOT NULL,
	[RentalEndDate] [date] NOT NULL,
	[PickupLocation] [nvarchar](100) NULL,
	[DropoffLocation] [nvarchar](100) NULL,
	[InitialRentalDays] [tinyint] NOT NULL,
	[RentalPricePerDay] [smallmoney] NOT NULL,
	[InitialTotalDueAmount] [smallmoney] NOT NULL,
	[InitialCheckNotes] [nvarchar](500) NULL,
	[BookingStatusID] [int] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BookingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RentalTransactions]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentalTransactions](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[BookingID] [int] NOT NULL,
	[ReturnID] [int] NULL,
	[PaymentMethod] [tinyint] NULL,
	[PaidInitialTotalDueAmount] [smallmoney] NOT NULL,
	[ActualTotalDueAmount] [smallmoney] NULL,
	[TotalRemaining] [smallmoney] NULL,
	[TotalRefundedAmount] [smallmoney] NULL,
	[TransactionDate] [datetime] NOT NULL,
	[UpdatedTransactionDate] [datetime] NULL,
	[CreatedByUserID] [int] NOT NULL,
	[UpdatedByUserID] [int] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SecurityLogs]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityLogs](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[EventType] [nvarchar](50) NOT NULL,
	[UserID] [int] NULL,
	[IPAddress] [varchar](45) NOT NULL,
	[Endpoint] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](500) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](30) NULL,
	[RoleID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[RefreshTokenHash] [nvarchar](255) NULL,
	[RefreshTokenExpiresAt] [datetime] NULL,
	[RefreshTokenRevokedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleCategories]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleCategories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleReturns]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleReturns](
	[ReturnID] [int] IDENTITY(1,1) NOT NULL,
	[BookingID] [int] NOT NULL,
	[ActualReturnDate] [datetime] NOT NULL,
	[ActualRentalDays] [tinyint] NOT NULL,
	[Mileage] [int] NOT NULL,
	[ConsumedMileage] [int] NOT NULL,
	[FinalCheckNotes] [nvarchar](500) NULL,
	[AdditionalCharges] [smallmoney] NOT NULL,
	[ActualTotalDueAmount] [smallmoney] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ReturnID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehicles]    Script Date: 8/12/2026 9:42:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicles](
	[VehicleID] [int] IDENTITY(1,1) NOT NULL,
	[Make] [nvarchar](50) NOT NULL,
	[Model] [nvarchar](50) NOT NULL,
	[VIN] [nvarchar](50) NOT NULL,
	[Color] [nvarchar](30) NOT NULL,
	[EngineNumber] [nvarchar](50) NOT NULL,
	[Year] [int] NOT NULL,
	[Mileage] [int] NOT NULL,
	[FuelTypeID] [int] NOT NULL,
	[PlateNumber] [nvarchar](20) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[RentalPricePerDay] [decimal](10, 2) NOT NULL,
	[IsAvailableForRent] [bit] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[VehicleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AuditLogs] ON 

INSERT [dbo].[AuditLogs] ([AuditLogID], [UserID], [Action], [EntityName], [EntityID], [IPAddress], [CreatedAt]) VALUES (1, 1, N'Update', N'Customer', 1, N'::1', CAST(N'2026-08-09T15:06:55.5588592' AS DateTime2))
INSERT [dbo].[AuditLogs] ([AuditLogID], [UserID], [Action], [EntityName], [EntityID], [IPAddress], [CreatedAt]) VALUES (2, 1, N'Delete', N'Customer', 2, N'::1', CAST(N'2026-08-09T15:12:34.7698356' AS DateTime2))
INSERT [dbo].[AuditLogs] ([AuditLogID], [UserID], [Action], [EntityName], [EntityID], [IPAddress], [CreatedAt]) VALUES (3, 1, N'Update', N'Customer', 3, N'::1', CAST(N'2026-08-09T15:27:36.3213655' AS DateTime2))
INSERT [dbo].[AuditLogs] ([AuditLogID], [UserID], [Action], [EntityName], [EntityID], [IPAddress], [CreatedAt]) VALUES (4, 1, N'Delete', N'Customer', 3, N'::1', CAST(N'2026-08-09T15:28:02.8714938' AS DateTime2))
SET IDENTITY_INSERT [dbo].[AuditLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[BookingStatus] ON 

INSERT [dbo].[BookingStatus] ([BookingStatusID], [StatusName]) VALUES (3, N'Cancelled')
INSERT [dbo].[BookingStatus] ([BookingStatusID], [StatusName]) VALUES (1, N'Reserved')
INSERT [dbo].[BookingStatus] ([BookingStatusID], [StatusName]) VALUES (2, N'Returned')
SET IDENTITY_INSERT [dbo].[BookingStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 

INSERT [dbo].[Customers] ([CustomerID], [Name], [ContactInformation], [DriverLicenseNumber], [CreatedByUserID], [CreatedDate]) VALUES (1, N'Mohamed Ismat Bakri', N'091023367', N'bm01440z', 1, CAST(N'2026-07-08T13:55:18.537' AS DateTime))
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[FuelTypes] ON 

INSERT [dbo].[FuelTypes] ([FuelTypeID], [FuelType]) VALUES (2, N'Diesel')
INSERT [dbo].[FuelTypes] ([FuelTypeID], [FuelType]) VALUES (4, N'Electric')
INSERT [dbo].[FuelTypes] ([FuelTypeID], [FuelType]) VALUES (3, N'Hybrid')
INSERT [dbo].[FuelTypes] ([FuelTypeID], [FuelType]) VALUES (1, N'Petrol')
SET IDENTITY_INSERT [dbo].[FuelTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[MaintenanceStatus] ON 

INSERT [dbo].[MaintenanceStatus] ([MaintenanceStatusID], [StatusName]) VALUES (3, N'Cancelled')
INSERT [dbo].[MaintenanceStatus] ([MaintenanceStatusID], [StatusName]) VALUES (2, N'Completed')
INSERT [dbo].[MaintenanceStatus] ([MaintenanceStatusID], [StatusName]) VALUES (1, N'InProgress')
SET IDENTITY_INSERT [dbo].[MaintenanceStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description]) VALUES (1, N'Administrator', N'Full system access')
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description]) VALUES (2, N'Manager', N'Manages bookings and vehicles')
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description]) VALUES (3, N'Employee', N'Handles daily rental operations')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[SecurityLogs] ON 

INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (1, N'LoginSucceeded', 1, N'::1', N'/api/Auth/Login', CAST(N'2026-08-09T11:04:53.8677536' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (2, N'LoginFailed', 1, N'::1', N'/api/Auth/Login', CAST(N'2026-08-09T11:07:08.3738661' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (3, N'LoginFailed', NULL, N'::1', N'/api/Auth/Login', CAST(N'2026-08-09T11:08:10.5792239' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (4, N'LoginSucceeded', 1, N'::1', N'/api/Auth/Login', CAST(N'2026-08-09T12:02:02.4557576' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (5, N'RefreshTokenSucceeded', 1, N'::1', N'/api/Auth/Refresh', CAST(N'2026-08-09T12:05:55.9754375' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (6, N'RefreshTokenFailed', 1, N'::1', N'/api/Auth/Refresh', CAST(N'2026-08-09T12:06:57.6097543' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (7, N'RefreshTokenFailed', 1, N'::1', N'/api/Auth/Refresh', CAST(N'2026-08-09T12:08:19.1228529' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (8, N'LoginSucceeded', 1, N'::1', N'/api/Auth/Login', CAST(N'2026-08-09T12:18:22.3342897' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (9, N'LogoutSucceeded', 1, N'::1', N'/api/Auth/Logout', CAST(N'2026-08-09T12:18:56.9680037' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (10, N'LogoutFailed', 1, N'::1', N'/api/Auth/Logout', CAST(N'2026-08-09T12:19:50.1289587' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (11, N'LogoutSucceeded', 1, N'::1', N'/api/Auth/Logout', CAST(N'2026-08-09T12:21:45.9079951' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (12, N'RefreshTokenRevoked', 1, N'::1', N'/api/Auth/Refresh', CAST(N'2026-08-09T12:22:25.7731196' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (13, N'LoginSucceeded', 1, N'::1', N'/api/Auth/Login', CAST(N'2026-08-09T15:04:40.2378362' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (14, N'LoginSucceeded', 1, N'::1', N'/api/Auth/Login', CAST(N'2026-08-09T15:10:38.5043581' AS DateTime2))
INSERT [dbo].[SecurityLogs] ([LogID], [EventType], [UserID], [IPAddress], [Endpoint], [CreatedAt]) VALUES (15, N'LoginSucceeded', 1, N'::1', N'/api/Auth/Login', CAST(N'2026-08-09T15:24:57.3180757' AS DateTime2))
SET IDENTITY_INSERT [dbo].[SecurityLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [FullName], [Username], [PasswordHash], [Email], [Phone], [RoleID], [IsActive], [CreatedDate], [RefreshTokenHash], [RefreshTokenExpiresAt], [RefreshTokenRevokedAt]) VALUES (1, N'System Administrator', N'admin', N'$2a$11$2z8VKYkk0maHAbAII3Ne7OONIbcNd2F4.d1M4Eq3h026o1OVeP8Rq', N'admin@carrental.com', N'01000000000', 1, 1, CAST(N'2026-07-06T15:37:32.160' AS DateTime), N'$2a$11$BrnF8n1yDBuWeyZ4Lkf.3eeZw9wmnxsB7WFWFEPCHt40mOvGlJxvW', CAST(N'2026-08-16T22:24:56.987' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Username], [PasswordHash], [Email], [Phone], [RoleID], [IsActive], [CreatedDate], [RefreshTokenHash], [RefreshTokenExpiresAt], [RefreshTokenRevokedAt]) VALUES (2, N'Samer Ahmed Ali', N'samer', N'$2a$11$hyIXytKLJeLyF0CVrA19IOiQjS3RfnuOdd.FAeRSrQUARzFJrAN8W', N'samer@gmail.com', N'12355569', 1, 1, CAST(N'2026-07-22T10:29:25.840' AS DateTime), N'$2a$11$9S2AiDuYeb37We4Q61pgSOBLpyK7lj6eQ7LHluLN2XR4msXUPQu8e', CAST(N'2026-08-04T12:35:02.833' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[VehicleCategories] ON 

INSERT [dbo].[VehicleCategories] ([CategoryID], [CategoryName]) VALUES (6, N'Convertible')
INSERT [dbo].[VehicleCategories] ([CategoryID], [CategoryName]) VALUES (5, N'Coupe')
INSERT [dbo].[VehicleCategories] ([CategoryID], [CategoryName]) VALUES (1, N'Economy')
INSERT [dbo].[VehicleCategories] ([CategoryID], [CategoryName]) VALUES (4, N'Hatchback')
INSERT [dbo].[VehicleCategories] ([CategoryID], [CategoryName]) VALUES (9, N'Luxury')
INSERT [dbo].[VehicleCategories] ([CategoryID], [CategoryName]) VALUES (7, N'Pickup')
INSERT [dbo].[VehicleCategories] ([CategoryID], [CategoryName]) VALUES (2, N'Sedan')
INSERT [dbo].[VehicleCategories] ([CategoryID], [CategoryName]) VALUES (10, N'Sports')
INSERT [dbo].[VehicleCategories] ([CategoryID], [CategoryName]) VALUES (3, N'SUV')
INSERT [dbo].[VehicleCategories] ([CategoryID], [CategoryName]) VALUES (8, N'Van')
SET IDENTITY_INSERT [dbo].[VehicleCategories] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__BookingS__05E7698AAFC2A2BE]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[BookingStatus] ADD UNIQUE NONCLUSTERED 
(
	[StatusName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Customer__C32FF260A6B5B7C0]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[Customers] ADD UNIQUE NONCLUSTERED 
(
	[DriverLicenseNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__FuelType__2F4FDCEC745FC45A]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[FuelTypes] ADD UNIQUE NONCLUSTERED 
(
	[FuelType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Maintena__E60542B41CE536EC]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[MaintenanceCompletion] ADD UNIQUE NONCLUSTERED 
(
	[MaintenanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Maintena__05E7698AFF1BCEA5]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[MaintenanceStatus] ADD UNIQUE NONCLUSTERED 
(
	[StatusName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Roles__8A2B6160BB935E66]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E42016EBEC]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__VehicleC__8517B2E02A91B4DA]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[VehicleCategories] ADD UNIQUE NONCLUSTERED 
(
	[CategoryName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__VehicleR__73951ACC600522F3]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[VehicleReturns] ADD UNIQUE NONCLUSTERED 
(
	[BookingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Vehicles__0369262436397076]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[Vehicles] ADD UNIQUE NONCLUSTERED 
(
	[PlateNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Vehicles__C5DF234C0479B8BE]    Script Date: 8/12/2026 9:42:37 AM ******/
ALTER TABLE [dbo].[Vehicles] ADD UNIQUE NONCLUSTERED 
(
	[VIN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditLogs] ADD  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Maintenance] ADD  DEFAULT ((1)) FOR [MaintenanceStatusID]
GO
ALTER TABLE [dbo].[MaintenanceCompletion] ADD  DEFAULT ((1)) FOR [IsPassedInspection]
GO
ALTER TABLE [dbo].[MaintenanceCompletion] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RentalBooking] ADD  DEFAULT ((1)) FOR [BookingStatusID]
GO
ALTER TABLE [dbo].[RentalBooking] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RentalTransactions] ADD  DEFAULT ((1)) FOR [PaymentMethod]
GO
ALTER TABLE [dbo].[RentalTransactions] ADD  DEFAULT (getdate()) FOR [TransactionDate]
GO
ALTER TABLE [dbo].[SecurityLogs] ADD  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[VehicleReturns] ADD  DEFAULT ((0)) FOR [AdditionalCharges]
GO
ALTER TABLE [dbo].[VehicleReturns] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Vehicles] ADD  DEFAULT ((1)) FOR [IsAvailableForRent]
GO
ALTER TABLE [dbo].[Vehicles] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[AuditLogs]  WITH CHECK ADD  CONSTRAINT [FK_AuditLogs_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[AuditLogs] CHECK CONSTRAINT [FK_AuditLogs_Users]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_Users]
GO
ALTER TABLE [dbo].[Maintenance]  WITH CHECK ADD  CONSTRAINT [FK_Maintenance_CreatedByUse] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Maintenance] CHECK CONSTRAINT [FK_Maintenance_CreatedByUse]
GO
ALTER TABLE [dbo].[Maintenance]  WITH CHECK ADD  CONSTRAINT [FK_Maintenance_MaintenanceStatus] FOREIGN KEY([MaintenanceStatusID])
REFERENCES [dbo].[MaintenanceStatus] ([MaintenanceStatusID])
GO
ALTER TABLE [dbo].[Maintenance] CHECK CONSTRAINT [FK_Maintenance_MaintenanceStatus]
GO
ALTER TABLE [dbo].[Maintenance]  WITH CHECK ADD  CONSTRAINT [FK_Maintenance_UpdatedByUser] FOREIGN KEY([UpdatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Maintenance] CHECK CONSTRAINT [FK_Maintenance_UpdatedByUser]
GO
ALTER TABLE [dbo].[Maintenance]  WITH CHECK ADD  CONSTRAINT [FK_Maintenance_Vehicle] FOREIGN KEY([VehicleID])
REFERENCES [dbo].[Vehicles] ([VehicleID])
GO
ALTER TABLE [dbo].[Maintenance] CHECK CONSTRAINT [FK_Maintenance_Vehicle]
GO
ALTER TABLE [dbo].[MaintenanceCompletion]  WITH CHECK ADD  CONSTRAINT [FK_MaintenanceCompletion_CreatedByUser] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[MaintenanceCompletion] CHECK CONSTRAINT [FK_MaintenanceCompletion_CreatedByUser]
GO
ALTER TABLE [dbo].[MaintenanceCompletion]  WITH CHECK ADD  CONSTRAINT [FK_MaintenanceCompletion_Maintenance] FOREIGN KEY([MaintenanceID])
REFERENCES [dbo].[Maintenance] ([MaintenanceID])
GO
ALTER TABLE [dbo].[MaintenanceCompletion] CHECK CONSTRAINT [FK_MaintenanceCompletion_Maintenance]
GO
ALTER TABLE [dbo].[MaintenanceCompletion]  WITH CHECK ADD  CONSTRAINT [FK_MaintenanceCompletion_UpdatedByUser] FOREIGN KEY([UpdatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[MaintenanceCompletion] CHECK CONSTRAINT [FK_MaintenanceCompletion_UpdatedByUser]
GO
ALTER TABLE [dbo].[RentalBooking]  WITH CHECK ADD  CONSTRAINT [FK_RentalBooking_BookingStatus] FOREIGN KEY([BookingStatusID])
REFERENCES [dbo].[BookingStatus] ([BookingStatusID])
GO
ALTER TABLE [dbo].[RentalBooking] CHECK CONSTRAINT [FK_RentalBooking_BookingStatus]
GO
ALTER TABLE [dbo].[RentalBooking]  WITH CHECK ADD  CONSTRAINT [FK_RentalBooking_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[RentalBooking] CHECK CONSTRAINT [FK_RentalBooking_Customer]
GO
ALTER TABLE [dbo].[RentalBooking]  WITH CHECK ADD  CONSTRAINT [FK_RentalBooking_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[RentalBooking] CHECK CONSTRAINT [FK_RentalBooking_Users]
GO
ALTER TABLE [dbo].[RentalBooking]  WITH CHECK ADD  CONSTRAINT [FK_RentalBooking_Vehicle] FOREIGN KEY([VehicleID])
REFERENCES [dbo].[Vehicles] ([VehicleID])
GO
ALTER TABLE [dbo].[RentalBooking] CHECK CONSTRAINT [FK_RentalBooking_Vehicle]
GO
ALTER TABLE [dbo].[RentalTransactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Booking] FOREIGN KEY([BookingID])
REFERENCES [dbo].[RentalBooking] ([BookingID])
GO
ALTER TABLE [dbo].[RentalTransactions] CHECK CONSTRAINT [FK_Transactions_Booking]
GO
ALTER TABLE [dbo].[RentalTransactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_CreatedByUser] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[RentalTransactions] CHECK CONSTRAINT [FK_Transactions_CreatedByUser]
GO
ALTER TABLE [dbo].[RentalTransactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Return] FOREIGN KEY([ReturnID])
REFERENCES [dbo].[VehicleReturns] ([ReturnID])
GO
ALTER TABLE [dbo].[RentalTransactions] CHECK CONSTRAINT [FK_Transactions_Return]
GO
ALTER TABLE [dbo].[RentalTransactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_UpdatedByUser] FOREIGN KEY([UpdatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[RentalTransactions] CHECK CONSTRAINT [FK_Transactions_UpdatedByUser]
GO
ALTER TABLE [dbo].[SecurityLogs]  WITH CHECK ADD  CONSTRAINT [FK_SecurityLogs_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[SecurityLogs] CHECK CONSTRAINT [FK_SecurityLogs_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[VehicleReturns]  WITH CHECK ADD  CONSTRAINT [FK_VehicleReturns_Booking] FOREIGN KEY([BookingID])
REFERENCES [dbo].[RentalBooking] ([BookingID])
GO
ALTER TABLE [dbo].[VehicleReturns] CHECK CONSTRAINT [FK_VehicleReturns_Booking]
GO
ALTER TABLE [dbo].[VehicleReturns]  WITH CHECK ADD  CONSTRAINT [FK_VehicleReturns_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[VehicleReturns] CHECK CONSTRAINT [FK_VehicleReturns_Users]
GO
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_Vehicles_Categories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[VehicleCategories] ([CategoryID])
GO
ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_Vehicles_Categories]
GO
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_Vehicles_FuelTypes] FOREIGN KEY([FuelTypeID])
REFERENCES [dbo].[FuelTypes] ([FuelTypeID])
GO
ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_Vehicles_FuelTypes]
GO
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_Vehicles_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_Vehicles_Users]
GO
ALTER TABLE [dbo].[RentalBooking]  WITH CHECK ADD  CONSTRAINT [CK_RentalBooking_Dates] CHECK  (([RentalEndDate]>=[RentalStartDate]))
GO
ALTER TABLE [dbo].[RentalBooking] CHECK CONSTRAINT [CK_RentalBooking_Dates]
GO
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [CK_Vehicles_Mileage] CHECK  (([Mileage]>=(0)))
GO
ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [CK_Vehicles_Mileage]
GO
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [CK_Vehicles_RentalPrice] CHECK  (([RentalPricePerDay]>(0)))
GO
ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [CK_Vehicles_RentalPrice]
GO
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [CK_Vehicles_Year] CHECK  (([Year]>=(2015)))
GO
ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [CK_Vehicles_Year]
GO
USE [master]
GO
ALTER DATABASE [CarRentalDB] SET  READ_WRITE 
GO
