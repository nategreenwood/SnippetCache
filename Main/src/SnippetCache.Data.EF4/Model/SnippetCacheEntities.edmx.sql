
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/03/2012 02:25:39
-- Generated from EDMX file: C:\Development\Web\SnippetCacheSolution\SnippetCache.Data.EF4\Model\SnippetCacheEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SnippetCache];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_LanguageSnippet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Snippets] DROP CONSTRAINT [FK_LanguageSnippet];
GO
IF OBJECT_ID(N'[dbo].[FK_NotificationTypeNotification]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Notifications] DROP CONSTRAINT [FK_NotificationTypeNotification];
GO
IF OBJECT_ID(N'[dbo].[FK_SnippetComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_SnippetComment];
GO
IF OBJECT_ID(N'[dbo].[FK_SnippetFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_SnippetFile];
GO
IF OBJECT_ID(N'[dbo].[FK_SnippetHyperlink]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Hyperlinks] DROP CONSTRAINT [FK_SnippetHyperlink];
GO
IF OBJECT_ID(N'[dbo].[FK_UserComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_UserComment];
GO
IF OBJECT_ID(N'[dbo].[FK_UserNotification]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Notifications] DROP CONSTRAINT [FK_UserNotification];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSnippet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Snippets] DROP CONSTRAINT [FK_UserSnippet];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[Files]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Files];
GO
IF OBJECT_ID(N'[dbo].[Hyperlinks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Hyperlinks];
GO
IF OBJECT_ID(N'[dbo].[Languages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Languages];
GO
IF OBJECT_ID(N'[dbo].[Notifications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Notifications];
GO
IF OBJECT_ID(N'[dbo].[NotificationTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NotificationTypes];
GO
IF OBJECT_ID(N'[dbo].[Snippets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Snippets];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [LastModified] datetime  NOT NULL,
    [Snippet_Id] int  NOT NULL,
    [User_Id] int  NOT NULL,
    [User_FormsAuthId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Files'
CREATE TABLE [dbo].[Files] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Data] varbinary(max)  NOT NULL,
    [LastModified] datetime  NOT NULL,
    [Snippet_Id] int  NOT NULL
);
GO

-- Creating table 'Hyperlinks'
CREATE TABLE [dbo].[Hyperlinks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Uri] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [LastModified] datetime  NOT NULL,
    [Snippet_Id] int  NOT NULL
);
GO

-- Creating table 'Languages'
CREATE TABLE [dbo].[Languages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Notifications'
CREATE TABLE [dbo].[Notifications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [NotificationType_Id] int  NOT NULL,
    [User_Id] int  NOT NULL,
    [User_FormsAuthId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'NotificationTypes'
CREATE TABLE [dbo].[NotificationTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Snippets'
CREATE TABLE [dbo].[Snippets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Guid] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [PreviewData] varbinary(max)  NOT NULL,
    [Data] varbinary(max)  NOT NULL,
    [LastModified] datetime  NOT NULL,
    [IsPublic] bit  NOT NULL,
    [Language_Id] int  NOT NULL,
    [User_Id] int  NOT NULL,
    [User_FormsAuthId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FormsAuthId] uniqueidentifier  NOT NULL,
    [LoginName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [AvatarImage] varbinary(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [PK_Files]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Hyperlinks'
ALTER TABLE [dbo].[Hyperlinks]
ADD CONSTRAINT [PK_Hyperlinks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Languages'
ALTER TABLE [dbo].[Languages]
ADD CONSTRAINT [PK_Languages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Notifications'
ALTER TABLE [dbo].[Notifications]
ADD CONSTRAINT [PK_Notifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NotificationTypes'
ALTER TABLE [dbo].[NotificationTypes]
ADD CONSTRAINT [PK_NotificationTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Snippets'
ALTER TABLE [dbo].[Snippets]
ADD CONSTRAINT [PK_Snippets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [FormsAuthId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id], [FormsAuthId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Snippet_Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_SnippetComment]
    FOREIGN KEY ([Snippet_Id])
    REFERENCES [dbo].[Snippets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SnippetComment'
CREATE INDEX [IX_FK_SnippetComment]
ON [dbo].[Comments]
    ([Snippet_Id]);
GO

-- Creating foreign key on [User_Id], [User_FormsAuthId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_UserComment]
    FOREIGN KEY ([User_Id], [User_FormsAuthId])
    REFERENCES [dbo].[Users]
        ([Id], [FormsAuthId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserComment'
CREATE INDEX [IX_FK_UserComment]
ON [dbo].[Comments]
    ([User_Id], [User_FormsAuthId]);
GO

-- Creating foreign key on [Snippet_Id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_SnippetFile]
    FOREIGN KEY ([Snippet_Id])
    REFERENCES [dbo].[Snippets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SnippetFile'
CREATE INDEX [IX_FK_SnippetFile]
ON [dbo].[Files]
    ([Snippet_Id]);
GO

-- Creating foreign key on [Snippet_Id] in table 'Hyperlinks'
ALTER TABLE [dbo].[Hyperlinks]
ADD CONSTRAINT [FK_SnippetHyperlink]
    FOREIGN KEY ([Snippet_Id])
    REFERENCES [dbo].[Snippets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SnippetHyperlink'
CREATE INDEX [IX_FK_SnippetHyperlink]
ON [dbo].[Hyperlinks]
    ([Snippet_Id]);
GO

-- Creating foreign key on [Language_Id] in table 'Snippets'
ALTER TABLE [dbo].[Snippets]
ADD CONSTRAINT [FK_LanguageSnippet]
    FOREIGN KEY ([Language_Id])
    REFERENCES [dbo].[Languages]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LanguageSnippet'
CREATE INDEX [IX_FK_LanguageSnippet]
ON [dbo].[Snippets]
    ([Language_Id]);
GO

-- Creating foreign key on [NotificationType_Id] in table 'Notifications'
ALTER TABLE [dbo].[Notifications]
ADD CONSTRAINT [FK_NotificationTypeNotification]
    FOREIGN KEY ([NotificationType_Id])
    REFERENCES [dbo].[NotificationTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_NotificationTypeNotification'
CREATE INDEX [IX_FK_NotificationTypeNotification]
ON [dbo].[Notifications]
    ([NotificationType_Id]);
GO

-- Creating foreign key on [User_Id], [User_FormsAuthId] in table 'Notifications'
ALTER TABLE [dbo].[Notifications]
ADD CONSTRAINT [FK_UserNotification]
    FOREIGN KEY ([User_Id], [User_FormsAuthId])
    REFERENCES [dbo].[Users]
        ([Id], [FormsAuthId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserNotification'
CREATE INDEX [IX_FK_UserNotification]
ON [dbo].[Notifications]
    ([User_Id], [User_FormsAuthId]);
GO

-- Creating foreign key on [User_Id], [User_FormsAuthId] in table 'Snippets'
ALTER TABLE [dbo].[Snippets]
ADD CONSTRAINT [FK_UserSnippet]
    FOREIGN KEY ([User_Id], [User_FormsAuthId])
    REFERENCES [dbo].[Users]
        ([Id], [FormsAuthId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSnippet'
CREATE INDEX [IX_FK_UserSnippet]
ON [dbo].[Snippets]
    ([User_Id], [User_FormsAuthId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------