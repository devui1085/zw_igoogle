--<<FileName:dbo_UsernameBlackLists.sql>>--
--<< TABLE DEFINITION >>--

If Object_ID('dbo.UsernameBlackLists') Is Null
CREATE TABLE [dbo].[UsernameBlackLists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](256) NOT NULL
) ON [PRIMARY]

--TEXTIMAGE_ON [SGBlob_Data]
--When a table has text, ntext, image, varchar(max), nvarchar(max), varbinary(max), xml or large user defined type columns uncomment above code
GO
--<< ADD CLOLUMNS >>--

--<<Sample>>--
/*if not exists (select 1 from sys.columns where object_id=object_id('dbo.UsernameBlackLists') and
				[name] = 'ColumnName')
begin
    Alter table dbo.UsernameBlackLists Add ColumnName DataType Nullable
end
GO*/

--<< ALTER COLUMNS >>--

--<< PRIMARYKEY DEFINITION >>--

If not Exists (select 1 from sys.objects where name = 'PK_UsernameBlackLists')
ALTER TABLE [dbo].[UsernameBlackLists] ADD  CONSTRAINT [PK_UsernameBlackLists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) ON [PRIMARY]
GO

--<< DEFAULTS CHECKS DEFINITION >>--

--<< RULES DEFINITION >>--

--<< INDEXES DEFINITION >>--

--<< FOREIGNKEYS DEFINITION >>--


--<< DROP OBJECTS >>--
