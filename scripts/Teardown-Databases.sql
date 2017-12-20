USE [master]
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Questions')
DROP DATABASE [Questions]
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Answers')
DROP DATABASE [Answers]
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Rules')
DROP DATABASE [Rules]
GO