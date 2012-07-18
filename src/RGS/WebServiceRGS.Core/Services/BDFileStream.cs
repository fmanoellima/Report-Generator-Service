using System;
using System.Configuration;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Data;
using Common.Tools;
using System.Collections.Generic;


namespace BDFileStream
{
    public class BDFileStream
    {
        const UInt32 DESIRED_ACCESS_READ = 0x00000000;
        const UInt32 DESIRED_ACCESS_WRITE = 0x00000001;
        const UInt32 DESIRED_ACCESS_READWRITE = 0x00000002;
        const UInt32 SQL_FILESTREAM_OPEN_NO_FLAGS = 0x00000000;
        private readonly string _connectionString;
        private SqlDataAccess _sqlDAc;
        private Dictionary<string, object> _sqlValues;
        private string _sqlCommand;
        private Object _obj;
       
        private byte[] _cxCtx;

        [DllImport("sqlncli10.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern SafeFileHandle OpenSqlFilestream(
                    string FilestreamPath,
                    UInt32 DesiredAccess,
                    UInt32 OpenOptions,
                    byte[] FilestreamTransactionContext,
                    UInt32 FilestreamTransactionContextLength,
                    Int64 AllocationSize);

        public BDFileStream()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["fileStreamServer"].ConnectionString;
        }

        public BDFileStream(string strConnection)
        {
            _connectionString = strConnection;
        }


        public void StoreFileByDbPathName(byte[] file, string dbPathName)
        {
            try
            {
                _sqlDAc = new SqlDataAccess(_connectionString);
                _sqlValues = new Dictionary<string, object>();

                _sqlDAc.BeginTransaction();
                _sqlCommand = "SELECT GET_FILESTREAM_TRANSACTION_CONTEXT()";

                _obj = _sqlDAc.ExecuteBySqlText(SqlDataAccess.ReturnType.Scalar, _sqlCommand, _sqlValues);

                _cxCtx = (byte[])_obj;

                // open the filestream to the blob
                SafeFileHandle handle = OpenSqlFilestream(dbPathName, DESIRED_ACCESS_WRITE, SQL_FILESTREAM_OPEN_NO_FLAGS, _cxCtx, (UInt32)_cxCtx.Length, 0);

                // open a Filestream to write
                FileStream filestream = new FileStream(handle, FileAccess.Write, file.Length, false);
                filestream.Write(file, 0, file.Length);
                filestream.Close();

                if (handle != null && !handle.IsClosed)
                    handle.Close();

                _sqlDAc.Commit();
            }
            catch (Exception)
            {
                _sqlDAc.Rollback();
                throw;
            }
        }

        public void StoreFileByGuid(byte[] file, Guid fileGuid)
        {
            string dbPathName = GetFileDbPathNameByGuid(fileGuid);

            StoreFileByDbPathName(file, dbPathName);
        }

        
        public void SaveFileInPathByDbPathName(string dbPathName, string filePath)
        {
            //Recupera o arquivo do servidor
            byte[] buffer = GetFileByDbPathName(dbPathName);
            //Salva o arquivo
            FileStream filestream = new FileStream(filePath, FileMode.CreateNew);
            filestream.Write(buffer, 0, buffer.Length);
            filestream.Close();
        
        }

        public void SaveFileInPathByDbGuid(Guid fileGuid, string filePath)
        {
            //Recupera o arquivo do servidor
            byte[] buffer = GetFileByGuid(fileGuid);
            //Salva o arquivo
            FileStream filestream = new FileStream(filePath, FileMode.CreateNew);
            filestream.Write(buffer, 0, buffer.Length);
            filestream.Close();

        }


        public byte[] GetFileByDbPathName(string dbPathName)
        {
            try
            {
                _sqlDAc = new SqlDataAccess(_connectionString);
                _sqlValues = new Dictionary<string, object>();

                _sqlDAc.BeginTransaction();
                _sqlCommand = "SELECT GET_FILESTREAM_TRANSACTION_CONTEXT()";

                _obj = _sqlDAc.ExecuteBySqlText(SqlDataAccess.ReturnType.Scalar, _sqlCommand, _sqlValues);

                _cxCtx = (byte[])_obj;

                // open the filestream to the blob
                SafeFileHandle handle = OpenSqlFilestream(dbPathName, DESIRED_ACCESS_READ, SQL_FILESTREAM_OPEN_NO_FLAGS, _cxCtx, (UInt32)_cxCtx.Length, 0);

                // open a Filestream to read the blob
                FileStream filestream = new FileStream(handle, FileAccess.Read);

                byte[] buffer = new byte[(int)filestream.Length];
                filestream.Read(buffer, 0, buffer.Length);
                filestream.Close();

                if (handle != null && !handle.IsClosed)
                    handle.Close();

               _sqlDAc.Commit();
                
                return buffer;
            }
            catch (Exception)
            {
                 _sqlDAc.Rollback();
                throw;
            }
        }

        public byte[] GetFileByGuid(Guid fileGuid)
        {

            string dbPathName = GetFileDbPathNameByGuid(fileGuid);

            if (!string.IsNullOrEmpty(dbPathName))
                return GetFileByDbPathName(dbPathName);
            else
                return new byte[0];
           
        }


        public Dictionary<string,string> GenerateFilePlaceholder()
        {
            _sqlDAc = new SqlDataAccess(_connectionString);
            _sqlValues = new Dictionary<string, object>();
            _sqlCommand = "GeneratefsFilePlaceholder";
            Dictionary<string, string> filePlaceholderInformations = new Dictionary<string, string>();

            Object returnObject = _sqlDAc.ExecuteByStoredProc(SqlDataAccess.ReturnType.DataTable, _sqlCommand, ref _sqlValues);

            if (returnObject != null)
            {
                DataTable dataTable = (DataTable)returnObject;

                if (dataTable.Rows.Count > 0)
                {
                    DataRow[] dr = dataTable.Select();
                    filePlaceholderInformations.Add("GUIDFile", dr[0]["GUIDFile"].ToString());
                    filePlaceholderInformations.Add("bdPathName", dr[0]["bdPathName"].ToString());
                }
            }
            return filePlaceholderInformations;
        }


        public void DeleteFileByGuid(Guid fileGuid)
        {
            _sqlDAc = new SqlDataAccess(_connectionString);
            _sqlValues = new Dictionary<string, object>();
            _sqlValues.Add("@fileGUID", fileGuid);
            _sqlCommand = "DeletefsFile";

            _sqlDAc.ExecuteByStoredProc(SqlDataAccess.ReturnType.NonQuery, _sqlCommand, ref _sqlValues);

        }


        private string GetFileDbPathNameByGuid(Guid fileGuid)
        {
            string bdpathName;
            _sqlDAc = new SqlDataAccess(_connectionString);
            _sqlValues = new Dictionary<string, object>();
            _sqlValues.Add("@fileGUID", fileGuid);
            _sqlCommand = "GetfsFilePathName";

            Object returnObject = _sqlDAc.ExecuteByStoredProc(SqlDataAccess.ReturnType.DataTable, _sqlCommand, ref _sqlValues);

            if (returnObject != null)
            {
                DataTable dataTable = (DataTable)returnObject;

                //se for encontrado algum relatório no banco com a guid recebida
                if (dataTable.Rows.Count > 0)
                {
                    DataRow[] dr = dataTable.Select();
                    //armazenando o guid e o pathName no array de strings
                    bdpathName = dr[0]["bdPathName"].ToString();
                }
                //Caso contrário retorna um array de bytes vazio
                else
                    return string.Empty;

                return bdpathName;
            }
            else
                return string.Empty;
        }
    }
}