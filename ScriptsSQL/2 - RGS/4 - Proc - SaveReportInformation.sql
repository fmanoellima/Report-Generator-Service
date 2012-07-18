USE RGS
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF OBJECT_ID(N'dbo.SaveReportInformation') IS NULL 
    EXECUTE('CREATE PROCEDURE dbo.SaveReportInformation AS RETURN') ;
GO


ALTER PROCEDURE SaveReportInformation
    @jsonReport VARCHAR(MAX) ,
    @ReportName VARCHAR(256) ,
    @reportsContainerGUID UNIQUEIDENTIFIER ,
    @reportGUID UNIQUEIDENTIFIER = NULL
AS 
    SET NOCOUNT ON

    DECLARE @error INT

    IF @reportGUID IS NULL 
        BEGIN
            SET @reportGUID = NEWID()
        END
		
    BEGIN TRANSACTION
    INSERT  INTO RenderedReports
            ( ReportGUID ,
              ReportsContainerGUID ,
              ReportName ,
              jsonReport ,
              cdStatus 
            )
    VALUES  ( @reportGUID ,
              @reportsContainerGUID ,
              @ReportName ,
              @jsonReport ,
              'PND'
            )
              
    SELECT  @error = @@ERROR 
    IF @error <> 0 
        BEGIN 
            IF @@TRANCOUNT > 0 
                ROLLBACK TRANSACTION 
        END
	
    COMMIT TRANSACTION
    
    SELECT  @reportGUID AS ReportGUID

    SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
