USE RGS
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF OBJECT_ID (N'dbo.GetReportInformation') IS NULL
    EXECUTE('CREATE PROCEDURE dbo.GetReportInformation AS RETURN');
GO


ALTER PROCEDURE GetReportInformation
    @reportGUID	UNIQUEIDENTIFIER
AS 
    SET NOCOUNT ON

	SELECT ReportsContainerGUID,StartDate,EndDate,ReportName,jsonReport ,jsonReportResult,ErrorDescription,cdStatus
	FROM dbo.RenderedReports
	WHERE ReportGUID = @reportGUID
		

SET NOCOUNT OFF
GO
