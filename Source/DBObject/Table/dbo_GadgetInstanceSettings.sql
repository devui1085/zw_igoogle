--<<FileName:dbo_GadgetInstanceSettings.sql>>--
--<< TABLE DEFINITION >>--

If Object_ID('dbo.GadgetInstanceSettings') Is Null
CREATE TABLE [dbo].[GadgetInstanceSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](256) NOT NULL,
	[Value] [nvarchar](4000) NOT NULL,
	[Public] [bit] NOT NULL,
	[GadgetInstance_Id] [int] NOT NULL
) ON [PRIMARY]

--TEXTIMAGE_ON [SGBlob_Data]
--When a table has text, ntext, image, varchar(max), nvarchar(max), varbinary(max), xml or large user defined type columns uncomment above code
GO
--<< ADD CLOLUMNS >>--

--<<Sample>>--
/*if not exists (select 1 from sys.columns where object_id=object_id('dbo.GadgetInstanceSettings') and
				[name] = 'ColumnName')
begin
    Alter table dbo.GadgetInstanceSettings Add ColumnName DataType Nullable
end
GO*/

--<< ALTER COLUMNS >>--

--<< PRIMARYKEY DEFINITION >>--

If not Exists (select 1 from sys.objects where name = 'PK_GadgetInstanceSettings')
ALTER TABLE [dbo].[GadgetInstanceSettings] ADD  CONSTRAINT [PK_GadgetInstanceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) ON [PRIMARY]
GO

--<< DEFAULTS CHECKS DEFINITION >>--

--<< RULES DEFINITION >>--

--<< INDEXES DEFINITION >>--

If not Exists (select 1 from sys.indexes where name = 'IX_FK_GadgetInstanceGadgetInstanceUserSetting')
CREATE NONCLUSTERED INDEX [IX_FK_GadgetInstanceGadgetInstanceUserSetting] ON [dbo].[GadgetInstanceSettings] 
(
	[GadgetInstance_Id] ASC
) ON [PRIMARY]

GO
--<< FOREIGNKEYS DEFINITION >>--

If not Exists (select 1 from sys.objects where name = 'FK_GadgetInstanceGadgetInstanceUserSetting')
ALTER TABLE [dbo].[GadgetInstanceSettings]  ADD  CONSTRAINT [FK_GadgetInstanceGadgetInstanceUserSetting] FOREIGN KEY([GadgetInstance_Id])
REFERENCES [dbo].[GadgetInstances] ([Id])
ON DELETE CASCADE

GO

--<< DROP OBJECTS >>--
