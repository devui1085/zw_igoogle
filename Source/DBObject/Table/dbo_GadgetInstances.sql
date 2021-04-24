--<<FileName:dbo_GadgetInstances.sql>>--
--<< TABLE DEFINITION >>--

If Object_ID('dbo.GadgetInstances') Is Null
CREATE TABLE [dbo].[GadgetInstances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Row] [tinyint] NOT NULL,
	[Column] [tinyint] NOT NULL,
	[RowSpan] [tinyint] NOT NULL,
	[ColumnSpan] [tinyint] NOT NULL,
	[Gadget_Id] [int] NOT NULL
) ON [PRIMARY]

--TEXTIMAGE_ON [SGBlob_Data]
--When a table has text, ntext, image, varchar(max), nvarchar(max), varbinary(max), xml or large user defined type columns uncomment above code
GO
--<< ADD CLOLUMNS >>--

--<<Sample>>--
/*if not exists (select 1 from sys.columns where object_id=object_id('dbo.GadgetInstances') and
				[name] = 'ColumnName')
begin
    Alter table dbo.GadgetInstances Add ColumnName DataType Nullable
end
GO*/

--<< ALTER COLUMNS >>--

--<< PRIMARYKEY DEFINITION >>--

If not Exists (select 1 from sys.objects where name = 'PK_GadgetInstances')
ALTER TABLE [dbo].[GadgetInstances] ADD  CONSTRAINT [PK_GadgetInstances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) ON [PRIMARY]
GO

--<< DEFAULTS CHECKS DEFINITION >>--

--<< RULES DEFINITION >>--

--<< INDEXES DEFINITION >>--

If not Exists (select 1 from sys.indexes where name = 'IX_FK_GadgetGadgetInstance')
CREATE NONCLUSTERED INDEX [IX_FK_GadgetGadgetInstance] ON [dbo].[GadgetInstances] 
(
	[Gadget_Id] ASC
) ON [PRIMARY]

GO
--<< FOREIGNKEYS DEFINITION >>--

If not Exists (select 1 from sys.objects where name = 'FK_GadgetGadgetInstance')
ALTER TABLE [dbo].[GadgetInstances]  ADD  CONSTRAINT [FK_GadgetGadgetInstance] FOREIGN KEY([Gadget_Id])
REFERENCES [dbo].[Gadgets] ([Id])
ON DELETE CASCADE

GO

--<< DROP OBJECTS >>--
