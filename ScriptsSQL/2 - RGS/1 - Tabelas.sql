USE RGS
GO

if exists (select * from sys.tables WHERE name = N'RenderedReports' )
	DROP TABLE RenderedReports
GO

if exists (select * from sys.tables WHERE name = N'ReportsContainers' )
	DROP TABLE ReportsContainers
GO

if exists (select * from sys.tables WHERE name = N'ReportStatus' )
	DROP TABLE ReportStatus
GO

CREATE TABLE ReportStatus
(
	cdStatus	CHAR(3) NOT NULL,
	nmStatus	VARCHAR(256),
	constraint PK_ReportStatus PRIMARY KEY  CLUSTERED (cdStatus)  ON [PRIMARY]
)
INSERT INTO ReportStatus
VALUES ('PND','Processando')

INSERT INTO ReportStatus
VALUES ('PCD','Processado com Sucesso')

INSERT INTO ReportStatus
VALUES ('PCF','Processado com Falha ou não processado')

INSERT INTO ReportStatus
VALUES ('NPD','Nao Processado')

INSERT INTO ReportStatus (cdStatus,nmStatus)
 VALUES ('ENV','Enviado')
INSERT INTO ReportStatus (cdStatus,nmStatus) 
VALUES ('NEV','Nao Enviado')


CREATE TABLE ReportsContainers(
	ReportsContainerGUID	[uniqueidentifier] ROWGUIDCOL NOT NULL UNIQUE,
	ReportsContainerDate	DATETIME CONSTRAINT DF_ReportContainers_StartDate DEFAULT GETDATE(),
	NetworkDomain			VARCHAR(256),
	ReportUserLogin			VARCHAR(256),
	ReportUserPassword		VARCHAR(256),
	ReportServiceUrl		VARCHAR(512),
	ReportExecutionUrl		VARCHAR(512),
	jsonReportsContainer	VARCHAR(MAX)
)


CREATE TABLE RenderedReports(
ReportGUID			[uniqueidentifier] ROWGUIDCOL NOT NULL UNIQUE,
ReportsContainerGUID	[uniqueidentifier],
StartDate			DATETIME CONSTRAINT DF_RenderedReports_StartDate DEFAULT GETDATE(),
EndDate				DATETIME,
ReportName			VARCHAR(128),
jsonReport	 		VARCHAR(MAX),
jsonReportResult	VARCHAR(MAX),
ErrorDescription	VARCHAR(4000),
cdStatus			CHAR(3),
constraint PK_RenderedReports PRIMARY KEY  CLUSTERED (ReportGUID)  ON [PRIMARY],
constraint FK_ReportsContainers_RenderedReports  FOREIGN KEY (ReportsContainerGUID) REFERENCES ReportsContainers (ReportsContainerGUID),
constraint FK_ReportStatus_RenderedReports  FOREIGN KEY (cdStatus) REFERENCES ReportStatus (cdStatus)
)


