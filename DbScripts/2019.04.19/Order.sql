/*
   Friday, April 19, 201912:03:14 PM
   User: 
   Server: NC-PC
   Database: MeghnaDB
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MasterDepot SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.[Order] ADD
	MasterDepotId bigint NULL
GO
ALTER TABLE dbo.[Order] ADD CONSTRAINT
	FK_Order_MasterDepot FOREIGN KEY
	(
	MasterDepotId
	) REFERENCES dbo.MasterDepot
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.[Order] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
