using System;
using System.Web.Services;
using Common.Components;
using Common.Helpers;
using WebServiceRGS.Core.Models;
using WebServiceRGS.Core.Services;

namespace WebServiceRGS
{
    /// <summary>
    /// Summary description for RGSWs
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RGSWs : System.Web.Services.WebService
    {
        /// <summary>
        /// Renderiza todos os relatórios de uma lista de relatórios contida dentro do Objeto ReportsContainer
        /// </summary>
        /// <param name="serializedReportContainer">String JSON do objeto ReportsContainer</param>
        /// <returns>String JSON comprimida do objeto ReportsContainer com todos os relatórios renderizados</returns>
        [WebMethod]
        public string RenderReports(string serializedReportContainer)
        {
            Feedback feed;
            try
            {
                ReportManager reportManager = new ReportManager();
                ReportsContainer reportContainer = JsonHelper.ConvertToObject<ReportsContainer>(serializedReportContainer);

                reportContainer = reportManager.RenderReports(reportContainer);

                feed = new Feedback(Feedback.TypeFeedback.Success, String.Empty, reportContainer);
            }
            catch (Exception ex)
            {
                feed = new Feedback(Feedback.TypeFeedback.Error, ex.Message, null);
            }
            string xmlObject = JsonHelper.ConvertToJSON(feed);
            return GZipHelper.Compress(xmlObject);
        }

        /// <summary>
        /// Recupera o arquivo armazenado em servidor FileStream de um relatório através do seu GUID
        /// </summary>
        /// <param name="reportGuid">GUID do relatório que será recuperado</param>
        /// <param name="deleteFile">Flag que determina se o arquivo deverá ser deletado do servidor no final do processo</param>
        /// <returns>String JSON do objeto Report com o atributo ReportFile carregado</returns>
        [WebMethod]
        public string GetReportFileByGUID(string reportGuid, bool deleteFile)
        {
            Feedback feed;
            try
            {
                ReportManager reportManager = new ReportManager();

                Report reportItem = reportManager.GetReportFileByGUID(new Guid(reportGuid), deleteFile);

                feed = new Feedback(Feedback.TypeFeedback.Success, String.Empty, reportItem);
            }
            catch (Exception ex)
            {
                feed = new Feedback(Feedback.TypeFeedback.Error, ex.Message, null);
            }
            string xmlObject = JsonHelper.ConvertToJSON(feed);
            return GZipHelper.Compress(xmlObject);
        }

        /// <summary>
        /// Renderiza novamente um relatório anteriormente gerado que possui log no RGS.(Não armazena o arquivo no servidor FileStream)
        /// </summary>
        /// <param name="reportGuid">GUID do relatório que será renderizado</param>
        /// <returns>String JSON do objeto Report com o atributo ReportFile carregado</returns>
        [WebMethod]
        public string RenderReportByGUID(string reportGuid)
        {
            Feedback feed;
            try
            {
                ReportManager reportManager = new ReportManager();

                Report reportItem = reportManager.RenderReportByGUID(new Guid(reportGuid));

                feed = new Feedback(Feedback.TypeFeedback.Success, String.Empty, reportItem);
            }
            catch (Exception ex)
            {
                feed = new Feedback(Feedback.TypeFeedback.Error, ex.Message, null);
            }
            string xmlObject = JsonHelper.ConvertToJSON(feed);
            return GZipHelper.Compress(xmlObject);
        }

    }
}
