
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/20/2016 02:12:20
-- Generated from EDMX file: C:\devsource\CMMI\CMMI.Data\CMMI.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO

IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ReviewRestaurant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reviews] DROP CONSTRAINT [FK_ReviewRestaurant];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Restaurants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Restaurants];
GO
IF OBJECT_ID(N'[dbo].[Reviews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reviews];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Restaurants'
CREATE TABLE [dbo].[Restaurants] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [City] nvarchar(100)  NOT NULL,
    [Rating] smallint  NOT NULL,
    [ReviewCount] smallint  NOT NULL,
    [UserGuid] uniqueidentifier  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [Approved] bit  NOT NULL
);
GO

-- Creating table 'Reviews'
CREATE TABLE [dbo].[Reviews] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [RestaurantId] bigint  NOT NULL,
    [Rating] smallint  NOT NULL,
    [Comment] nvarchar(2000)  NOT NULL,
    [Approved] bit  NOT NULL,
    [UserGuid] uniqueidentifier  NOT NULL,
    [CreateDate] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Restaurants'
ALTER TABLE [dbo].[Restaurants]
ADD CONSTRAINT [PK_Restaurants]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [PK_Reviews]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RestaurantId] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [FK_RestaurantReview]
    FOREIGN KEY ([RestaurantId])
    REFERENCES [dbo].[Restaurants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RestaurantReview'
CREATE INDEX [IX_FK_RestaurantReview]
ON [dbo].[Reviews]
    ([RestaurantId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------