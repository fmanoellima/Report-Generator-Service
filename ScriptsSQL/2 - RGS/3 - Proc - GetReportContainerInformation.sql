USE RGS
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF OBJECT_ID(N'dbo.GetReportsContainerInformation') IS NULL 
    EXECUTE('CREATE PROCEDURE dbo.GetReportsContainerInformation AS RETURN') ;
GO


ALTER PROCEDURE GetReportsContainerInformation
    @reportsContainerGUID UNIQUEIDENTIFIER
AS 
    SET NOCOUNT ON

    SELECT  NetworkDomain
            ,ReportUserLogin
            ,ReportUserPassword
            ,ReportServiceUrl
            ,ReportExecutionUrl
            ,jsonReportsContainer
    FROM    dbo.ReportsContainers
    WHERE   ReportsContainerGUID = @reportsContainerGUID
		

    SET NOCOUNT OFF
GO
