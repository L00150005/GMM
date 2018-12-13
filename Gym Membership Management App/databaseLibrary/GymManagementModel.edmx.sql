
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/13/2018 13:40:31
-- Generated from EDMX file: C:\Users\home\Source\Repos\GMM\Gym Membership Management App\databaseLibrary\GymManagementModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [theGymDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Tables'
CREATE TABLE [dbo].[Tables] (
    [clientID] int IDENTITY(1,1) NOT NULL,
    [firstName] varchar(50)  NOT NULL,
    [lastName] varchar(50)  NOT NULL,
    [DOB] datetime  NOT NULL,
    [address1] varchar(50)  NOT NULL,
    [address2] varchar(50)  NULL,
    [phoneNo] varchar(20)  NULL
);
GO

-- Creating table 'tblClients'
CREATE TABLE [dbo].[tblClients] (
    [clientID] int IDENTITY(1,1) NOT NULL,
    [firstName] varchar(50)  NOT NULL,
    [lastName] varchar(50)  NOT NULL,
    [DOB] datetime  NOT NULL,
    [address1] varchar(50)  NOT NULL,
    [address2] varchar(50)  NULL,
    [phoneNo] varchar(20)  NULL
);
GO

-- Creating table 'tblClientMemberships'
CREATE TABLE [dbo].[tblClientMemberships] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [clientID] int  NOT NULL,
    [planID] int  NOT NULL,
    [startDate] datetime  NOT NULL,
    [endDate] datetime  NOT NULL
);
GO

-- Creating table 'tblPlans'
CREATE TABLE [dbo].[tblPlans] (
    [planID] int IDENTITY(1,1) NOT NULL,
    [planName] varchar(50)  NOT NULL,
    [planDescription] varchar(50)  NOT NULL,
    [planTerm] varchar(20)  NOT NULL,
    [planPrice] int  NOT NULL
);
GO

-- Creating table 'tblSecurityLevels'
CREATE TABLE [dbo].[tblSecurityLevels] (
    [levelID] int IDENTITY(1,1) NOT NULL,
    [levelDescription] varchar(50)  NOT NULL
);
GO

-- Creating table 'tblUsers'
CREATE TABLE [dbo].[tblUsers] (
    [userID] int IDENTITY(1,1) NOT NULL,
    [userName] nvarchar(30)  NOT NULL,
    [userPassword] nvarchar(15)  NOT NULL,
    [userSecurityLevel] int  NOT NULL,
    [userFullName] nvarchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [clientID] in table 'Tables'
ALTER TABLE [dbo].[Tables]
ADD CONSTRAINT [PK_Tables]
    PRIMARY KEY CLUSTERED ([clientID] ASC);
GO

-- Creating primary key on [clientID] in table 'tblClients'
ALTER TABLE [dbo].[tblClients]
ADD CONSTRAINT [PK_tblClients]
    PRIMARY KEY CLUSTERED ([clientID] ASC);
GO

-- Creating primary key on [ID] in table 'tblClientMemberships'
ALTER TABLE [dbo].[tblClientMemberships]
ADD CONSTRAINT [PK_tblClientMemberships]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [planID] in table 'tblPlans'
ALTER TABLE [dbo].[tblPlans]
ADD CONSTRAINT [PK_tblPlans]
    PRIMARY KEY CLUSTERED ([planID] ASC);
GO

-- Creating primary key on [levelID] in table 'tblSecurityLevels'
ALTER TABLE [dbo].[tblSecurityLevels]
ADD CONSTRAINT [PK_tblSecurityLevels]
    PRIMARY KEY CLUSTERED ([levelID] ASC);
GO

-- Creating primary key on [userID] in table 'tblUsers'
ALTER TABLE [dbo].[tblUsers]
ADD CONSTRAINT [PK_tblUsers]
    PRIMARY KEY CLUSTERED ([userID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [clientID] in table 'tblClientMemberships'
ALTER TABLE [dbo].[tblClientMemberships]
ADD CONSTRAINT [FK_tblClientMembership_TO_tblClient]
    FOREIGN KEY ([clientID])
    REFERENCES [dbo].[tblClients]
        ([clientID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblClientMembership_TO_tblClient'
CREATE INDEX [IX_FK_tblClientMembership_TO_tblClient]
ON [dbo].[tblClientMemberships]
    ([clientID]);
GO

-- Creating foreign key on [planID] in table 'tblClientMemberships'
ALTER TABLE [dbo].[tblClientMemberships]
ADD CONSTRAINT [FK_tblClientMembership_TO_tblPlan]
    FOREIGN KEY ([planID])
    REFERENCES [dbo].[tblPlans]
        ([planID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblClientMembership_TO_tblPlan'
CREATE INDEX [IX_FK_tblClientMembership_TO_tblPlan]
ON [dbo].[tblClientMemberships]
    ([planID]);
GO

-- Creating foreign key on [userSecurityLevel] in table 'tblUsers'
ALTER TABLE [dbo].[tblUsers]
ADD CONSTRAINT [FK_tblUser_TO_tblSecurityLevel]
    FOREIGN KEY ([userSecurityLevel])
    REFERENCES [dbo].[tblSecurityLevels]
        ([levelID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblUser_TO_tblSecurityLevel'
CREATE INDEX [IX_FK_tblUser_TO_tblSecurityLevel]
ON [dbo].[tblUsers]
    ([userSecurityLevel]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------