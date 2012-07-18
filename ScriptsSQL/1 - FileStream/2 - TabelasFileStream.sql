USE RGSfs
GO

if exists (select * from sys.tables WHERE name = N'fsFiles' )
	DROP TABLE fsFiles
GO


CREATE TABLE fsFiles(
	guidFile				[uniqueidentifier] ROWGUIDCOL NOT NULL UNIQUE,
	GeneratedDate			DATETIME,
	fsFile 					VARBINARY(max) FILESTREAM
)
