USE [master]
GO

/****** Object:  Database [RAPID_e4473]    Script Date: 04/20/2015 15:00:07 ******/
CREATE DATABASE [RAPID_e4473] ON  PRIMARY 
( NAME = N'RAPID_e4473', FILENAME = N'D:\SqlServer\MSSQL13.MSSQLSERVER\MSSQL\DATA\RAPID_e4473.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'RAPID_e4473_log', FILENAME = N'D:\SqlServer\MSSQL13.MSSQLSERVER\MSSQL\DATA\RAPID_e4473_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [RAPID_e4473] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RAPID_e4473].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [RAPID_e4473] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [RAPID_e4473] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [RAPID_e4473] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [RAPID_e4473] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [RAPID_e4473] SET ARITHABORT OFF 
GO

ALTER DATABASE [RAPID_e4473] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [RAPID_e4473] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [RAPID_e4473] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [RAPID_e4473] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [RAPID_e4473] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [RAPID_e4473] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [RAPID_e4473] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [RAPID_e4473] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [RAPID_e4473] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [RAPID_e4473] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [RAPID_e4473] SET  DISABLE_BROKER 
GO

ALTER DATABASE [RAPID_e4473] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [RAPID_e4473] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [RAPID_e4473] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [RAPID_e4473] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [RAPID_e4473] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [RAPID_e4473] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [RAPID_e4473] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [RAPID_e4473] SET  READ_WRITE 
GO

ALTER DATABASE [RAPID_e4473] SET RECOVERY FULL 
GO

ALTER DATABASE [RAPID_e4473] SET  MULTI_USER 
GO

ALTER DATABASE [RAPID_e4473] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [RAPID_e4473] SET DB_CHAINING OFF 
GO


USE [RAPID_e4473]
GO

/****** Object:  Table [dbo].[CUSTOMERS]    Script Date: 04/20/2015 15:00:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CUSTOMERS](
	[CUST_ID] [int] IDENTITY(1,1) NOT NULL,
	[BARCODE] [varchar](100) NULL,
	[PASSWORD] [varchar](100) NULL,
	[EMAIL] [nvarchar](150) NULL,
	[TIMESTAMP] [datetime] NULL,
	[FIRST_NAME] [varchar](50) NULL,
	[LAST_NAME] [varchar](50) NULL,
	[MIDDLE_NAME] [varchar](50) NULL,
	[ADDRS_1] [varchar](150) NULL,
	[ADDRS_2] [varchar](150) NULL,
	[CITY] [varchar](100) NULL,
	[COUNTY] [varchar](100) NULL,
	[STATE] [varchar](50) NULL,
	[ZIP_COD] [varchar](50) NULL,
	[PLACE_OF_BIRTH_STATE] [varchar](100) NULL,
	[PLACE_OF_BIRTH_CITY] [varchar](100) NULL,
	[PLACE_OF_BIRTH_FOREIGN] [varchar](100) NULL,
	[HEIGHT_FT] [varchar](50) NULL,
	[HEIGHT_IN] [varchar](50) NULL,
	[WEIGHT] [varchar](50) NULL,
	[GENDER] [varchar](50) NULL,
	[BIRTHDATE] [varchar](100) NULL,
	[SOC_SEC_NUM] [varchar](50) NULL,
	[UPIN] [varchar](50) NULL,
	[ETHNICITY] [varchar](50) NULL,
	[RACE] [varchar](50) NULL,
	[C11A] [varchar](50) NULL,
	[C11B] [varchar](50) NULL,
	[C11C] [varchar](50) NULL,
	[C11D] [varchar](50) NULL,
	[C11E] [varchar](50) NULL,
	[C11F] [varchar](50) NULL,
	[C11G] [varchar](50) NULL,
	[C11H] [varchar](50) NULL,
	[C11I] [varchar](50) NULL,
	[C11J] [varchar](50) NULL,
	[C11K] [varchar](50) NULL,
	[C11L] [varchar](50) NULL,
	[ALIEN] [varchar](50) NULL,
	[RESIDENCE_STATE] [varchar](50) NULL,
	[CITIZENSHIP] [varchar](100) NULL,
	[ALIEN_NUM] [varchar](100) NULL,
 CONSTRAINT [PK_CUSTOMERS] PRIMARY KEY CLUSTERED 
(
	[CUST_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

