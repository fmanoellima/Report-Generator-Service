using System;
using System.Net;
using WebServiceRGS.Core.ReportExecution2005;
using WebServiceRGS.Core.ReportService2005;
using WebServiceRGS.Core.Models;
using System.Collections.Generic;
using Common.Tools;
using System.Configuration;
using System.Data;
using Common.Helpers;


namespace WebServiceRGS.Core.Services
{
    public class ReportManager
    {
        #region Atributos Privados

        private ReportingService2005 _rsService;
        private ReportExecutionService _rsExecution;
        private NetworkCredential _credential;

        //argumentos de renderização
        private string _historyID;
        private string _deviceInfo;
        private string _encoding;
        private string _mimeType;
        private string _extension;
        private ReportExecution2005.Warning[] _warnings;
        private string[] _streamIDs;

        //Variáveis necessárias pelo GetParameters
        private string _reportName;
        private bool _forRendering;

        //variáveis SQL
        private readonly string _connectionString;
        private readonly string _fsConnectionString;
        private SqlDataAccess _sqlDAc;
        private Dictionary<string, object> _sqlValues;
        private string _sqlCommand;

        //Classe para armazenagem do arquivo no banco
        private BDFileStream.BDFileStream _dbFileStream;

        #endregion

        public ReportManager()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["RGSServer"].ConnectionString;

            _fsConnectionString = ConfigurationManager.ConnectionStrings["fileStreamServer"].ConnectionString;
        }

        #region Métodos Publicos

        public ReportsContainer RenderReports(ReportsContainer reportsContainer)
         {
            if (string.IsNullOrEmpty(reportsContainer.ReportServiceUrl) || string.IsNullOrEmpty(reportsContainer.ReportExecutionUrl))
            {
                throw new Exception("Atributos ReportServicesUrl e ReportExecutionUrl devem ser definidos");
            }

            //Instancia as classes do ReportingService
            _rsService = new ReportingService2005();
            _rsExecution = new ReportExecutionService();

            if (reportsContainer.CredentialType == CredentialType.DefaultCredential )
            {
                _rsService.Credentials = CredentialCache.DefaultCredentials;
                _rsExecution.Credentials = CredentialCache.DefaultCredentials;
            }
            else if(reportsContainer.CredentialType == CredentialType.NetworkCredential)
            {
                _rsService.Credentials = CredentialCache.DefaultNetworkCredentials;
                _rsExecution.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                _credential = new NetworkCredential
                                    {
                                        UserName = reportsContainer.ReportUserLogin,
                                        Password = reportsContainer.ReportUserPassword,
                                        Domain = reportsContainer.NetworkDomain
                                    };
                _rsService.Credentials = _credential;
                _rsExecution.Credentials = _credential;
            }

            //Define a URL do Servidor que vai gerar o relatório
            _rsService.Url = reportsContainer.ReportServiceUrl;
            _rsExecution.Url = reportsContainer.ReportExecutionUrl;

            if (reportsContainer.LogReports)
            {
                SaveReportsContainerInformation(reportsContainer);
                reportsContainer.Reports.ForEach(r => r.ReportsContainerGuid = reportsContainer.ReportsContainerGuid);
            }

            for (int i = 0; i < reportsContainer.Reports.Count; i++ )
            {
                Report report = reportsContainer.Reports[i];

                try
                {
                    report = RenderReportItem(reportsContainer, report);
                }
                catch (Exception ex)
                {
                    report.ReportGenerationStatus = false;
                    report.ReportErrorDescription = ex.ToString();
                    UpdateReportStatus(report);
                }
            }
             return reportsContainer;
         }

        public Report GetReportFileByGUID(Guid reportGUID, bool deleteFile)
        {
            Report report = GetReportByGUID(reportGUID);

            if (report != null)
            {
                if (report.ReportGenerationStatus && report.StoreReportInDB)
                {
                    _dbFileStream = new BDFileStream.BDFileStream(_fsConnectionString);

                    report.ReportFile = _dbFileStream.GetFileByGuid(report.ReportGuid);

                    if(deleteFile)
                        _dbFileStream.DeleteFileByGuid(report.ReportGuid);
                }
            }
            return report;
        }

        public Report RenderReportByGUID(Guid reportGUID)
        {
            Report report = GetReportByGUID(reportGUID);
            ReportsContainer reportsContainer= GetReportsContainerByGUID(report.ReportsContainerGuid);

            report.ReportFile = RenderReport(reportsContainer, report);
            
            return report;
        }

        #endregion

        #region Métodos Privados

        private Report RenderReportItem(ReportsContainer reportsContainer, Report reportItem)
        {
            try
            {
                byte[] reportFile = null;
                string ReportDbFilestreamPathName = string.Empty;

                //Verificando se o arquivo será armazenado no BD para gerar seu GUID e seu DBPathName (necessário para o filestream)
                if (reportItem.StoreReportInDB)
                {
                    //Instanciando a classe BDFileStream
                    _dbFileStream = new BDFileStream.BDFileStream(_fsConnectionString);

                    Dictionary<string, string> placeholderInfo = _dbFileStream.GenerateFilePlaceholder();
                    reportItem.ReportGuid = new Guid(placeholderInfo["GUIDFile"]);
                    ReportDbFilestreamPathName = placeholderInfo["bdPathName"];
                }

                if (reportsContainer.LogReports)
                {
                    //Salva as informações sobre o relatório no Banco de dados
                    SaveReportInformation(reportItem, reportsContainer);
                }

                //Renderiza o relatório
                reportFile = RenderReport(reportItem);

                //Define a variável do status de renderização do relatório
                reportItem.ReportGenerationStatus = true;

                if (reportItem.StoreReportInDB)
                {
                    //Utilizando o método que armazena o relatório no SQL
                    _dbFileStream.StoreFileByDbPathName(reportFile, ReportDbFilestreamPathName);
                }

                if (reportsContainer.LogReports)
                {
                    //Atualiza o status do relatório
                    UpdateReportStatus(reportItem);
                }

                //Armazena o arquivo no objeto Report
                reportItem.ReportFile = reportFile;
            }
            catch(Exception ex)
            {
                if (reportItem.StoreReportInDB && reportItem.ReportGuid != new Guid())
                    _dbFileStream.DeleteFileByGuid(reportItem.ReportGuid);

                throw ex;
            }

            return reportItem;
        }


        private void SaveReportsContainerInformation(ReportsContainer reportsContainer)
        {
            _sqlDAc = new SqlDataAccess(_connectionString);
            _sqlValues = new Dictionary<string, object>();

            reportsContainer.ReportsContainerGuid = Guid.NewGuid();

            _sqlValues.Add("@ReportsContainerGUID", reportsContainer.ReportsContainerGuid);
            _sqlValues.Add("@jsonReportsContainer", JsonHelper.ConvertToJSON(reportsContainer));
            _sqlValues.Add("@NetworkDomain", reportsContainer.NetworkDomain);
            _sqlValues.Add("@ReportUserLogin", reportsContainer.ReportUserLogin);
            _sqlValues.Add("@ReportUserPassword", reportsContainer.ReportUserPassword);
            _sqlValues.Add("@ReportServiceUrl", reportsContainer.ReportServiceUrl);
            _sqlValues.Add("@ReportExecutionUrl", reportsContainer.ReportExecutionUrl);

            _sqlCommand = "SaveReportContainerInformation";

            _sqlDAc.ExecuteByStoredProc(SqlDataAccess.ReturnType.NonQuery, _sqlCommand, ref _sqlValues);
        }

        private ReportsContainer GetReportsContainerByGUID(Guid reportsContainerGUID)
        {
            ReportsContainer reportsContainer = null;
            _sqlDAc = new SqlDataAccess(_connectionString);
            _sqlValues = new Dictionary<string, object>();
            _sqlValues.Add("@reportsContainerGUID", reportsContainerGUID);
            _sqlCommand = "GetReportsContainerInformation";

            Object returnObject = _sqlDAc.ExecuteByStoredProc(SqlDataAccess.ReturnType.DataTable, _sqlCommand, ref _sqlValues);

            if (returnObject != null)
            {
                DataTable dataTable = (DataTable)returnObject;

                if (dataTable.Rows.Count > 0)
                {
                    DataRow[] dr = dataTable.Select();
                    reportsContainer = JsonHelper.ConvertToObject<ReportsContainer>(dr[0]["jsonReportsContainer"].ToString());
                }
            }
            return reportsContainer;
        }


        private void SaveReportInformation(Report report, ReportsContainer reportsContainer)
         {
            _sqlDAc = new SqlDataAccess(_connectionString);
            _sqlValues = new Dictionary<string, object>();
            
            _sqlValues.Add("@jsonReport", JsonHelper.ConvertToJSON(report));
            _sqlValues.Add("@ReportName", report.ReportName);
            _sqlValues.Add("@reportsContainerGUID", reportsContainer.ReportsContainerGuid);

            if (report.ReportGuid != new Guid())
                _sqlValues.Add("@reportGUID", report.ReportGuid);

            _sqlCommand = "SaveReportInformation";

            var result = _sqlDAc.ExecuteByStoredProc(SqlDataAccess.ReturnType.Scalar, _sqlCommand, ref _sqlValues);

            report.ReportGuid = new Guid(result.ToString());
         }

        private Report GetReportByGUID(Guid reportGUID)
        {
            Report report = null;
            _sqlDAc = new SqlDataAccess(_connectionString);
            _sqlValues = new Dictionary<string, object>();
            _sqlValues.Add("@reportGUID", reportGUID);
            _sqlCommand = "GetReportInformation";

            Object returnObject = _sqlDAc.ExecuteByStoredProc(SqlDataAccess.ReturnType.DataTable, _sqlCommand, ref _sqlValues);

            if (returnObject != null)
            {
                DataTable dataTable = (DataTable)returnObject;

                if (dataTable.Rows.Count > 0)
                {
                    DataRow[] dr = dataTable.Select();
                    report = JsonHelper.ConvertToObject<Report>(dr[0]["jsonReportResult"].ToString());
                }
            }
            return report;
        }

        private void UpdateReportStatus(Report report)
         {
             try
            {
                 _sqlDAc = new SqlDataAccess(_connectionString);
                 _sqlValues = new Dictionary<string, object>();

                 _sqlValues.Add("@reportGUID", report.ReportGuid);
                 _sqlValues.Add("@jsonReportResult", JsonHelper.ConvertToJSON(report));
                 if (!report.ReportGenerationStatus)
                     _sqlValues.Add("@errorDescription", report.ReportErrorDescription);

                 _sqlCommand = "UpdateReportStatus";

                 _sqlDAc.ExecuteByStoredProc(SqlDataAccess.ReturnType.NonQuery, _sqlCommand, ref _sqlValues);
            }
             catch (Exception ex)
             {
                 Exception newException = new Exception("Erro na geracao do historico do relatorio", ex);
                 throw newException;
             }
         }


        private byte[] RenderReport(Report report)
        {
            byte[] reportFile = null;
            string ReportDbFilestreamPathName = string.Empty;

            _reportName = @"/" + report.ReportName;
            _historyID = null;
            _forRendering = false;

            //busca no reportService os parametros exigidos pelo relatório (caso queira utilizar para alguma verificação)
            //ReportService2005.ParameterValue[] _parameterValues = null;
            //ReportService2005.DataSourceCredentials[] _dsCredentials = null;
            //ReportService2005.ReportParameter[] _reportParameters = null;
            //_reportParameters = _rsService.GetReportParameters(_reportName, _historyID, _forRendering, _parameterValues, _dsCredentials);

            //Carrega a session para o relatorio selecionado
            ReportExecution2005.ExecutionInfo _executionInfo = _rsExecution.LoadReport(_reportName, _historyID);

            //Prepara os parametros do relatório.
            ReportExecution2005.ParameterValue[] reportExecutionParameters = new ReportExecution2005.ParameterValue[report.Parameters.Count];

            //lista todos os parametros do objeto Report e carrega nos parametros do relatório
            int contador = 0;
            foreach (KeyValuePair<string, object> parametro in report.Parameters)
            {
                reportExecutionParameters[contador++] = new ReportExecution2005.ParameterValue
                {
                    Name = parametro.Key,
                    Value = parametro.Value.ToString()
                };
            }

            //definindo os valores dos parametros do relatório
            _rsExecution.SetExecutionParameters(reportExecutionParameters, "en-us");

            //o relatorio é armazenado como um array de bytes
            if (report.ReportFormat == ReportFormat.HTML4)
                reportFile = _rsExecution.Render("HTML4.0", _deviceInfo, out _extension, out _encoding, out _mimeType, out _warnings, out _streamIDs);
            else
                reportFile = _rsExecution.Render(report.ReportFormat.ToString(), _deviceInfo, out _extension, out _encoding, out _mimeType, out _warnings, out _streamIDs);

            return reportFile;
        }

        private byte[] RenderReport(ReportsContainer reportsContainer, Report report)
        {
            byte[] reportFile = null;
            string ReportDbFilestreamPathName = string.Empty;

            if (string.IsNullOrEmpty(reportsContainer.ReportServiceUrl) || string.IsNullOrEmpty(reportsContainer.ReportExecutionUrl))
            {
                throw new Exception("Atributos ReportServicesUrl e ReportExecutionUrl devem ser definidos");
            }

            //Instancia as classes do ReportingService
            _rsService = new ReportingService2005();
            _rsExecution = new ReportExecutionService();

            if (reportsContainer.CredentialType == CredentialType.DefaultCredential)
            {
                _rsService.Credentials = CredentialCache.DefaultCredentials;
                _rsExecution.Credentials = CredentialCache.DefaultCredentials;
            }
            else if (reportsContainer.CredentialType == CredentialType.NetworkCredential)
            {
                _rsService.Credentials = CredentialCache.DefaultNetworkCredentials;
                _rsExecution.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                _credential = new NetworkCredential
                {
                    UserName = reportsContainer.ReportUserLogin,
                    Password = reportsContainer.ReportUserPassword,
                    Domain = reportsContainer.NetworkDomain
                };
                _rsService.Credentials = _credential;
                _rsExecution.Credentials = _credential;
            }

            //Define a URL do Servidor que vai gerar o relatório
            _rsService.Url = reportsContainer.ReportServiceUrl;
            _rsExecution.Url = reportsContainer.ReportExecutionUrl;


            _reportName = @"/" + report.ReportName;
            _historyID = null;
            _forRendering = false;

            //Carrega a session para o relatorio selecionado
            ReportExecution2005.ExecutionInfo _executionInfo = _rsExecution.LoadReport(_reportName, _historyID);

            //Prepara os parametros do relatório.
            ReportExecution2005.ParameterValue[] reportExecutionParameters = new ReportExecution2005.ParameterValue[report.Parameters.Count];

            //lista todos os parametros do objeto Report e carrega nos parametros do relatório
            int contador = 0;
            foreach (KeyValuePair<string, object> parametro in report.Parameters)
            {
                reportExecutionParameters[contador++] = new ReportExecution2005.ParameterValue
                {
                    Name = parametro.Key,
                    Value = parametro.Value.ToString()
                };
            }

            //definindo os valores dos parametros do relatório
            _rsExecution.SetExecutionParameters(reportExecutionParameters, "en-us");

            //o relatorio é armazenado como um array de bytes
            if (report.ReportFormat == ReportFormat.HTML4)
                reportFile = _rsExecution.Render("HTML4.0", _deviceInfo, out _extension, out _encoding, out _mimeType, out _warnings, out _streamIDs);
            else
                reportFile = _rsExecution.Render(report.ReportFormat.ToString(), _deviceInfo, out _extension, out _encoding, out _mimeType, out _warnings, out _streamIDs);

            return reportFile;
        }

        #endregion

    }
}

