--<<FileName:dbo_GadgetSettings.sql>>--
--<< TABLE DEFINITION >>--

If Object_ID('dbo.GadgetSettings') Is Null
CREATE TABLE [dbo].[GadgetSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](256) NOT NULL,
	[Value] [nvarchar](4000) NOT NULL,
	[Public] [bit] NOT NULL,
	[Gadget_Id] [int] NOT NULL
) ON [PRIMARY]

--TEXTIMAGE_ON [SGBlob_Data]
--When a table has text, ntext, image, varchar(max), nvarchar(max), varbinary(max), xml or large user defined type columns uncomment above code
GO
--<< ADD CLOLUMNS >>--

--<<Sample>>--
/*if not exists (select 1 from sys.columns where object_id=object_id('dbo.GadgetSettings') and
				[name] = 'ColumnName')
begin
    Alter table dbo.GadgetSettings Add ColumnName DataType Nullable
end
GO*/

--<< ALTER COLUMNS >>--

--<< PRIMARYKEY DEFINITION >>--

If not Exists (select 1 from sys.objects where name = 'PK_GadgetSettings')
ALTER TABLE [dbo].[GadgetSettings] ADD  CONSTRAINT [PK_GadgetSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) ON [PRIMARY]
GO

--<< DEFAULTS CHECKS DEFINITION >>--

--<< RULES DEFINITION >>--

--<< INDEXES DEFINITION >>--

If not Exists (select 1 from sys.indexes where name = 'IX_FK_GadgetGadgetSetting')
CREATE NONCLUSTERED INDEX [IX_FK_GadgetGadgetSetting] ON [dbo].[GadgetSettings] 
(
	[Gadget_Id] ASC
) ON [PRIMARY]

GO
--<< FOREIGNKEYS DEFINITION >>--

If not Exists (select 1 from sys.objects where name = 'FK_GadgetGadgetSetting')
ALTER TABLE [dbo].[GadgetSettings]  ADD  CONSTRAINT [FK_GadgetGadgetSetting] FOREIGN KEY([Gadget_Id])
REFERENCES [dbo].[Gadgets] ([Id])
ON DELETE CASCADE

GO

--<< DROP OBJECTS >>--
