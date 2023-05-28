
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/28/2023 16:31:25
-- Generated from EDMX file: C:\Users\79872\Desktop\dit_07_227\TemplateLab2\Template_4332\Galyamshin4332.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ISRPO33];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[galyamshinSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[galyamshinSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'galyamshinSet'
CREATE TABLE [dbo].[galyamshinSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CodeOrder] nvarchar(max)  NULL,
    [CreateDate] nvarchar(max)  NULL,
    [CodeClient] nvarchar(max)  NULL,
    [Services] nvarchar(max)  NULL,
    [ProkatTime] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'galyamshinSet'
ALTER TABLE [dbo].[galyamshinSet]
ADD CONSTRAINT [PK_galyamshinSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------