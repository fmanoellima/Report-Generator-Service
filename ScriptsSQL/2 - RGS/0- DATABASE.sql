sp_configure 'show advanced options',1
GO
RECONFIGURE
GO
--Permite o uso da função cmdshell
sp_configure xp_cmdshell,1
GO
RECONFIGURE
GO


declare @rc int,
@dbName NVARCHAR(128),
@rootDir nvarchar(4000), 
@dbDataLocation  nvarchar(4000),
@dbLogLocation  nvarchar(4000),
@fileStreamDir nvarchar(4000), 
@cmdShell  nvarchar(4000)

SET @dbName = 'RGS'

IF EXISTS (SELECT * FROM sys.databases WHERE name = @dbName)
  EXEC ('DROP DATABASE ' +  @dbName)

/*
BUSCANDO A PASTA DE INSTALAÇÃO DO SQL PARA A CRIAÇÃO DO DATABASE
*/
exec @rc = master.dbo.xp_instance_regread
      N'HKEY_LOCAL_MACHINE',
      N'Software\Microsoft\MSSQLServer\Setup',
      N'SQLPath', 
      @rootDir output, 'no_output'

--Definindo a pasta dos arquivos do DataBase baseando-se na pasta de instalção do SQL
SET @dbDataLocation = @rootDir + '\DATA\' + @dbName+ '_data.mdf'
SET @dbLogLocation  = @rootDir + '\DATA\'+ @dbName + '_log.ldf'

EXEC ('
CREATE DATABASE ' + @dbName + ' ON PRIMARY
  ( NAME = '''+@dbName+'_data'', 
    FILENAME = N''' + @dbDataLocation +''', 
    SIZE = 10MB,
    MAXSIZE = 50MB, 
    FILEGROWTH = 15%)
LOG ON 
  ( NAME = ''' + @dbName + '_log'', 
    FILENAME = N''' + @dbLogLocation + ''',
    SIZE = 5MB, 
    MAXSIZE = 25MB, 
    FILEGROWTH = 5MB);
')
GO
