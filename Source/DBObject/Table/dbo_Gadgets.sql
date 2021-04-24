--<<FileName:dbo_Gadgets.sql>>--
--<< TABLE DEFINITION >>--

If Object_ID('dbo.Gadgets') Is Null
CREATE TABLE [dbo].[Gadgets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GadgetType] [tinyint] NOT NULL,
	[SystemName] [nvarchar](256) NOT NULL,
	[PublicName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Version] [nvarchar](20) NOT NULL,
	[FolderName] [nvarchar](256) NOT NULL,
	[CreateDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
	[HomePageUrl] [nvarchar](256) NULL,
	[Enabled] [bit] NOT NULL,
	[Data] [nvarchar](max) NULL
) ON [PRIMARY]

--TEXTIMAGE_ON [SGBlob_Data]
--When a table has text, ntext, image, varchar(max), nvarchar(max), varbinary(max), xml or large user defined type columns uncomment above code
GO
--<< ADD CLOLUMNS >>--

--<<Sample>>--
/*if not exists (select 1 from sys.columns where object_id=object_id('dbo.Gadgets') and
				[name] = 'ColumnName')
begin
    Alter table dbo.Gadgets Add ColumnName DataType Nullable
end
GO*/

--<< ALTER COLUMNS >>--

--<< PRIMARYKEY DEFINITION >>--

If not Exists (select 1 from sys.objects where name = 'PK_Gadgets')
ALTER TABLE [dbo].[Gadgets] ADD  CONSTRAINT [PK_Gadgets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) ON [PRIMARY]
GO

--<< DEFAULTS CHECKS DEFINITION >>--

--<< RULES DEFINITION >>--

--<< INDEXES DEFINITION >>--

--<< FOREIGNKEYS DEFINITION >>--


--<< DROP OBJECTS >>--
