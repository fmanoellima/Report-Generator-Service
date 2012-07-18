USE RGS
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF OBJECT_ID (N'dbo.SaveReportContainerInformation') IS NULL
    EXECUTE('CREATE PROCEDURE dbo.SaveReportContainerInformation AS RETURN');
GO


ALTER PROCEDURE SaveReportContainerInformation
	@ReportsContainerGUID	UNIQUEIDENTIFIER,
	@NetworkDomain			VARCHAR(256),
	@ReportUserLogin		VARCHAR(256),
	@ReportUserPassword		VARCHAR(256),
	@ReportServiceUrl		VARCHAR(512),
	@ReportExecutionUrl		VARCHAR(512),
	@jsonReportsContainer 	VARCHAR(MAX)
	
AS 
    SET NOCOUNT ON

    DECLARE @error INT

    BEGIN TRANSACTION
    INSERT  INTO ReportsContainers
            ( 
				ReportsContainerGUID
				,NetworkDomain		
				,ReportUserLogin	
				,ReportUserPassword	
				,ReportServiceUrl	
				,ReportExecutionUrl	
				,jsonReportsContainer)
    VALUES  (
				@reportsContainerGUID
				,@NetworkDomain			
				,@ReportUserLogin		
				,@ReportUserPassword		
				,@ReportServiceUrl		
				,@ReportExecutionUrl		
				,@jsonReportsContainer)
              
    SELECT  @error = @@ERROR 
    IF @error <> 0 
        BEGIN 
            IF @@TRANCOUNT > 0 
                ROLLBACK TRANSACTION 
        END
	
    COMMIT TRANSACTION
    
SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
