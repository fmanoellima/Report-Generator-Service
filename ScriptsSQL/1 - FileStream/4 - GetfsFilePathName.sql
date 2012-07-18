USE RGSfs
GO


IF OBJECT_ID (N'dbo.GetfsFilePathName') IS NULL
    EXECUTE('CREATE PROCEDURE dbo.GetfsFilePathName AS RETURN');
GO

ALTER PROCEDURE GetfsFilePathName
	@fileGUID	UNIQUEIDENTIFIER
AS 
    SET NOCOUNT ON

	SELECT  fsFile.PathName() AS bdPathName
	FROM    fsFiles
	WHERE   guidFile = @fileGUID
	    

SET NOCOUNT OFF
GO
