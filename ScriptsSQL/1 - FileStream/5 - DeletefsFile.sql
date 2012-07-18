USE RGSfs
GO


IF OBJECT_ID (N'dbo.DeletefsFile') IS NULL
    EXECUTE('CREATE PROCEDURE dbo.DeletefsFile AS RETURN');
GO

ALTER PROCEDURE DeletefsFile
	@fileGUID	UNIQUEIDENTIFIER
AS 
    SET NOCOUNT ON

	DELETE FROM fsFiles
	WHERE   guidFile = @fileGUID
	    
SET NOCOUNT OFF
GO
