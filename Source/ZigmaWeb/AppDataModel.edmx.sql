
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/23/2014 17:51:48
-- Generated from EDMX file: E:\Development\TFS\Source\ZWeb\ZigmaWeb\ZigmaWeb\AppDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ZigmaWebDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_GadgetGadgetInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GadgetInstances] DROP CONSTRAINT [FK_GadgetGadgetInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_GadgetInstanceGadgetInstanceUserSetting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GadgetInstanceSettings] DROP CONSTRAINT [FK_GadgetInstanceGadgetInstanceUserSetting];
GO
IF OBJECT_ID(N'[dbo].[FK_GadgetGadgetSetting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GadgetSettings] DROP CONSTRAINT [FK_GadgetGadgetSetting];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Gadgets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Gadgets];
GO
IF OBJECT_ID(N'[dbo].[GadgetInstances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GadgetInstances];
GO
IF OBJECT_ID(N'[dbo].[GadgetInstanceSettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GadgetInstanceSettings];
GO
IF OBJECT_ID(N'[dbo].[UserSettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSettings];
GO
IF OBJECT_ID(N'[dbo].[UsernameBlackLists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsernameBlackLists];
GO
IF OBJECT_ID(N'[dbo].[GadgetSettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GadgetSettings];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Gadgets'
CREATE TABLE [dbo].[Gadgets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GadgetType] tinyint  NOT NULL,
    [SystemName] nvarchar(256)  NOT NULL,
    [PublicName] nvarchar(256)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Version] nvarchar(20)  NOT NULL,
    [FolderName] nvarchar(256)  NOT NULL,
    [CreateDate] datetime  NULL,
    [LastUpdate] datetime  NULL,
    [HomePageUrl] nvarchar(256)  NULL,
    [Enabled] bit  NOT NULL,
    [Data] nvarchar(max)  NULL
);
GO

-- Creating table 'GadgetInstances'
CREATE TABLE [dbo].[GadgetInstances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [Row] tinyint  NOT NULL,
    [Column] tinyint  NOT NULL,
    [RowSpan] tinyint  NOT NULL,
    [ColumnSpan] tinyint  NOT NULL,
    [Gadget_Id] int  NOT NULL
);
GO

-- Creating table 'GadgetInstanceSettings'
CREATE TABLE [dbo].[GadgetInstanceSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(256)  NOT NULL,
    [Value] nvarchar(4000)  NOT NULL,
    [Public] bit  NOT NULL,
    [GadgetInstance_Id] int  NOT NULL
);
GO

-- Creating table 'UserSettings'
CREATE TABLE [dbo].[UserSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [Key] nvarchar(256)  NOT NULL,
    [Value] nvarchar(4000)  NOT NULL,
    [Public] bit  NOT NULL
);
GO

-- Creating table 'UsernameBlackLists'
CREATE TABLE [dbo].[UsernameBlackLists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'GadgetSettings'
CREATE TABLE [dbo].[GadgetSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(256)  NOT NULL,
    [Value] nvarchar(4000)  NOT NULL,
    [Public] bit  NOT NULL,
    [Gadget_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Gadgets'
ALTER TABLE [dbo].[Gadgets]
ADD CONSTRAINT [PK_Gadgets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GadgetInstances'
ALTER TABLE [dbo].[GadgetInstances]
ADD CONSTRAINT [PK_GadgetInstances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GadgetInstanceSettings'
ALTER TABLE [dbo].[GadgetInstanceSettings]
ADD CONSTRAINT [PK_GadgetInstanceSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSettings'
ALTER TABLE [dbo].[UserSettings]
ADD CONSTRAINT [PK_UserSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UsernameBlackLists'
ALTER TABLE [dbo].[UsernameBlackLists]
ADD CONSTRAINT [PK_UsernameBlackLists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GadgetSettings'
ALTER TABLE [dbo].[GadgetSettings]
ADD CONSTRAINT [PK_GadgetSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Gadget_Id] in table 'GadgetInstances'
ALTER TABLE [dbo].[GadgetInstances]
ADD CONSTRAINT [FK_GadgetGadgetInstance]
    FOREIGN KEY ([Gadget_Id])
    REFERENCES [dbo].[Gadgets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GadgetGadgetInstance'
CREATE INDEX [IX_FK_GadgetGadgetInstance]
ON [dbo].[GadgetInstances]
    ([Gadget_Id]);
GO

-- Creating foreign key on [GadgetInstance_Id] in table 'GadgetInstanceSettings'
ALTER TABLE [dbo].[GadgetInstanceSettings]
ADD CONSTRAINT [FK_GadgetInstanceGadgetInstanceUserSetting]
    FOREIGN KEY ([GadgetInstance_Id])
    REFERENCES [dbo].[GadgetInstances]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GadgetInstanceGadgetInstanceUserSetting'
CREATE INDEX [IX_FK_GadgetInstanceGadgetInstanceUserSetting]
ON [dbo].[GadgetInstanceSettings]
    ([GadgetInstance_Id]);
GO

-- Creating foreign key on [Gadget_Id] in table 'GadgetSettings'
ALTER TABLE [dbo].[GadgetSettings]
ADD CONSTRAINT [FK_GadgetGadgetSetting]
    FOREIGN KEY ([Gadget_Id])
    REFERENCES [dbo].[Gadgets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GadgetGadgetSetting'
CREATE INDEX [IX_FK_GadgetGadgetSetting]
ON [dbo].[GadgetSettings]
    ([Gadget_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------