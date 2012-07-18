
sp_configure 'show advanced options',1
GO
RECONFIGURE
GO
--Permite o uso da função cmdshell
sp_configure xp_cmdshell,1
GO
RECONFIGURE
GO

-- 1) Configure SQL Server to have filestream access (1 = T-SQL, 2 = T-SQL + Win32)
exec sp_configure 'filestream access level', 2
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

SET @dbName = 'RGSfs'

IF EXISTS (SELECT * FROM sys.databases WHERE name = @dbName)
  EXEC ('DROP DATABASE ' +  @dbName)

/*
BUSCANDO A PASTA DE INSTALAÇÃO DO SQL PARA A CRIAÇÃO DO DATABASE E A CRIAÇÃO DA PASTA DO FILESTREAM
*/
exec @rc = master.dbo.xp_instance_regread
      N'HKEY_LOCAL_MACHINE',
      N'Software\Microsoft\MSSQLServer\Setup',
      N'SQLPath', 
      @rootDir output, 'no_output'


create table #xp_fileexist_output (
[FILE_EXISTS]			int	not null,
[FILE_IS_DIRECTORY]		int	not null,
[PARENT_DIRECTORY_EXISTS]	int	not null)

--Define a pasta raiz do filestream baseado na pasta de instalação do SQL
SET @fileStreamDir = @rootDir + '\SQLSvrFileStreamDoc'

--Verificando se a pasta ja existe
insert into #xp_fileexist_output
exec master.dbo.xp_fileexist @fileStreamDir

--Caso não exista, executa o comando para criar a pasta
if NOT exists ( select * from #xp_fileexist_output where FILE_IS_DIRECTORY = 1 )
	BEGIN
		SET @cmdShell = 'MD "' + @fileStreamDir + '"'
		EXEC master.dbo.xp_cmdshell @cmdShell
	end

drop table #xp_fileexist_output

--Definindo a pasta dos arquivos do DataBase baseando-se na pasta de instalção do SQL
SET @dbDataLocation = @rootDir + '\DATA\' + @dbName+ '_data.mdf'
SET @dbLogLocation  = @rootDir + '\DATA\'+ @dbName + '_log.ldf'
SET @fileStreamDir = @rootDir + '\SQLSvrFileStreamDoc\' + @dbName


EXEC ('
CREATE DATABASE ' + @dbName + ' ON PRIMARY
  ( NAME = '''+@dbName+'_data'', 
    FILENAME = N''' + @dbDataLocation +''', 
    SIZE = 10MB,
    MAXSIZE = 50MB, 
    FILEGROWTH = 15%),
FILEGROUP '+@dbName+' CONTAINS FILESTREAM
  ( NAME = ''fg'+@dbName+''', 
    FILENAME = N''' + @fileStreamDir + ''')
LOG ON 
  ( NAME = ''' + @dbName + '_log'', 
    FILENAME = N''' + @dbLogLocation + ''',
    SIZE = 5MB, 
    MAXSIZE = 25MB, 
    FILEGROWTH = 5MB);
')
GO
