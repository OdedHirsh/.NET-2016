USE [shook]
GO

/****** Object: Table [dbo].[Nadlan] Script Date: 5/22/2017 9:30:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Nadlan] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [CityId]              INT            NOT NULL,
    [PropertyTypeId]      INT            NOT NULL,
    [Rooms]               INT            NOT NULL,
    [Price]               INT            NOT NULL,
    [Size]                INT            NOT NULL,
    [FloorNumber]         INT            NOT NULL,
    [PropertyDescription] NVARCHAR (100) NULL,
    [UserName]            VARCHAR (20)   NOT NULL
);


