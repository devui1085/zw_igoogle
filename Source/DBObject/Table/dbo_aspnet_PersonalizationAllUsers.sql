--<<FileName:dbo_aspnet_PersonalizationAllUsers.sql>>--
--<< TABLE DEFINITION >>--

If Object_ID('dbo.aspnet_PersonalizationAllUsers') Is Null
CREATE TABLE [dbo].[aspnet_PersonalizationAllUsers](
	[PathId] [uniqueidentifier] NOT NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--TEXTIMAGE_ON [SGBlob_Data]
--When a table has text, ntext, image, varchar(max), nvarchar(max), varbinary(max), xml or large user defined type columns uncomment above code
GO
--<< ADD CLOLUMNS >>--

--<<Sample>>--
/*if not exists (select 1 from sys.columns where object_id=object_id('dbo.aspnet_PersonalizationAllUsers') and
				[name] = 'ColumnName')
begin
    Alter table dbo.aspnet_PersonalizationAllUsers Add ColumnName DataType Nullable
end
GO*/

--<< ALTER COLUMNS >>--

--<< PRIMARYKEY DEFINITION >>--

If not Exists (select 1 from sys.objects where name = 'PK__aspnet_P__CD67DC5960A75C0F')
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers] ADD PRIMARY KEY CLUSTERED 
(
	[PathId] ASC
) ON [PRIMARY]
GO

--<< DEFAULTS CHECKS DEFINITION >>--

--<< RULES DEFINITION >>--

--<< INDEXES DEFINITION >>--

--<< FOREIGNKEYS DEFINITION >>--

If not Exists (select 1 from sys.objects where name = 'FK__aspnet_Pe__PathI__628FA481')

GO

--<< DROP OBJECTS >>--
