USE RGS
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF OBJECT_ID (N'dbo.UpdateReportStatus') IS NULL
    EXECUTE('CREATE PROCEDURE dbo.UpdateReportStatus AS RETURN');
GO


ALTER PROCEDURE UpdateReportStatus
	@reportGUID	UNIQUEIDENTIFIER,
	@jsonReportResult VARCHAR(max),
	@errorDescription	VARCHAR(2000) = null

as
SET NOCOUNT ON

DECLARE @error		INT

		

IF @errorDescription IS NULL
	BEGIN
		BEGIN TRANSACTION
			UPDATE RenderedReports 
			SET EndDate = getdate()
				,cdStatus = 'PCD'
				,jsonReportResult = @jsonReportResult
			WHERE ReportGUID = @reportGUID
			
			SELECT @error = @@ERROR 
			IF @error <> 0 
				BEGIN 
					IF @@TRANCOUNT > 0 
						ROLLBACK TRANSACTION 
				END
			
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		BEGIN TRANSACTION
			UPDATE RenderedReports SET EndDate = getdate(),cdStatus = 'PCF',ErrorDescription = @errorDescription
			WHERE ReportGUID = @reportGUID
			
			SELECT @error = @@ERROR 
			IF @error <> 0 
				BEGIN 
					IF @@TRANCOUNT > 0 
						ROLLBACK TRANSACTION 
				END
			
		COMMIT TRANSACTION
	END





SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
