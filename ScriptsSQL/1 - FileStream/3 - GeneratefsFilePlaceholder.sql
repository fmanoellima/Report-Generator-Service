USE RGSfs
GO

IF OBJECT_ID (N'dbo.GeneratefsFilePlaceholder') IS NULL
    EXECUTE('CREATE PROCEDURE dbo.GeneratefsFilePlaceholder AS RETURN');
GO

ALTER PROCEDURE GeneratefsFilePlaceholder
AS 
    SET NOCOUNT ON

	DECLARE @guid UNIQUEIDENTIFIER

	SET @guid = NEWID()
	
	INSERT  INTO fsFiles ( GUIDFile ,GeneratedDate, fsFile )
	VALUES ( @guid ,GETDATE(),CAST('' AS VARBINARY(MAX)))

	SELECT  GUIDFile,
			fsFile.PathName() AS bdPathName
	FROM    fsFiles
	WHERE   GUIDFile = @guid
	    

SET NOCOUNT OFF
GO
