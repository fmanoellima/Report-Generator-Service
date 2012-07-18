using System;
using Common.Components;
using Common.Helpers;
using WebServiceRGS.Core.Models;

namespace WebServiceRGS.Tester
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
         {
           RenderizarRelatorios();
           //RecuperarArquivoRelatorioPorGUID("D17EAA9A-EB58-43FF-910D-685C418027FF");
           //RenderizarRelatorioPorGUID("D17EAA9A-EB58-43FF-910D-685C418027FF");
         }

        private void RenderizarRelatorios()
        {
            string serializedObject;
            string strDecompressedString;
            RGSws.RGSWsSoapClient _wsRGS = new RGSws.RGSWsSoapClient();
            ReportsContainer reportContainer = new ReportsContainer();
            
            //reportContainer.CredentialType = CredentialType.NetworkCredential;
            //reportContainer.ReportServiceUrl = "http://wibr001505/ReportServer/ReportService2005.asmx";
            //reportContainer.ReportExecutionUrl = "http://wibr001505/ReportServer/ReportExecution2005.asmx";
            
            //ReportContainer///////////////////////////////////////////////////////////////////////////////
            reportContainer.CredentialType = CredentialType.CustomCredential;
            reportContainer.ReportUserLogin = "Srv_reporting";
            reportContainer.ReportUserPassword = "B53k9js&";
            reportContainer.NetworkDomain = "PHARMA";
            reportContainer.ReportServiceUrl = "http://192.168.0.155/ReportServer/ReportService2005.asmx";
            reportContainer.ReportExecutionUrl = "http://192.168.0.155/ReportServer/ReportExecution2005.asmx";
            reportContainer.LogReports = true;
            ////////////////////////////////////////////////////////////////////////////////////////////////
            
            //Report////////////////////////////////////////////////////////////////////////////////////////
            Report report = new Report();
            report.ReportName = "Homologacao/Vitrine/AuditoriaAcordoPromocionalBandeira";
            report.ReportFormat = ReportFormat.EXCEL;

            report.Parameters.Add("DtInicio", "2012-01-01");
            report.Parameters.Add("DtFim", "2012-04-30");

            report.StoreReportInDB = true;
            ////////////////////////////////////////////////////////////////////////////////////////////////


            reportContainer.Reports.Add(report);
            
            serializedObject = JsonHelper.ConvertToJSON(reportContainer);

            strDecompressedString = GZipHelper.Decompress(_wsRGS.RenderReports(serializedObject));

            Feedback _feed = JsonHelper.ConvertToObject<Feedback>(strDecompressedString);

            if (_feed.Status == Feedback.TypeFeedback.Success)
            {
                txtResultado.Text = strDecompressedString;
                reportContainer = JsonHelper.ConvertToObject<ReportsContainer>(_feed.Output.ToString());

                if (reportContainer.Reports[0].ReportGenerationStatus)
                    GeraArquivo(reportContainer.Reports[0].ReportFile, true);
                else
                    txtResultado.Text = reportContainer.Reports[0].ReportErrorDescription;
            }
            else
                txtResultado.Text = _feed.Message[0];
        }

        private void RecuperarArquivoRelatorioPorGUID(string guidRelatorio)
        {
            string strDecompressedString;
            RGSws.RGSWsSoapClient _wsRGS = new RGSws.RGSWsSoapClient();

            Report report;

            strDecompressedString = GZipHelper.Decompress(_wsRGS.GetReportFileByGUID(guidRelatorio, false));

            Feedback _feed = JsonHelper.ConvertToObject<Feedback>(strDecompressedString);

            if (_feed.Status == Feedback.TypeFeedback.Success)
            {
                txtResultado.Text = strDecompressedString;

                if (_feed.Output != null)
                {
                    report = JsonHelper.ConvertToObject<Report>(_feed.Output.ToString());

                    if (report.ReportGenerationStatus)
                        GeraArquivo(report.ReportFile, true);
                    else
                        txtResultado.Text = report.ReportErrorDescription;
                }
            }
            else
                txtResultado.Text = _feed.Message[0];
        }

        private void RenderizarRelatorioPorGUID(string guidRelatorio)
        {
            string strDecompressedString;
            RGSws.RGSWsSoapClient _wsRGS = new RGSws.RGSWsSoapClient();

            Report report;

            strDecompressedString = GZipHelper.Decompress(_wsRGS.RenderReportByGUID(guidRelatorio));

            Feedback _feed = JsonHelper.ConvertToObject<Feedback>(strDecompressedString);

            if (_feed.Status == Feedback.TypeFeedback.Success)
            {
                txtResultado.Text = strDecompressedString;

                if (_feed.Output != null)
                {
                    report = JsonHelper.ConvertToObject<Report>(_feed.Output.ToString());

                    if (report.ReportGenerationStatus)
                        GeraArquivo(report.ReportFile, true);
                    else
                        txtResultado.Text = report.ReportErrorDescription;
                }
            }
            else
                txtResultado.Text = _feed.Message[0];
        }

        private void GeraArquivo(byte[] byteViewer, bool isFile)
        {
            try
            {
                //Armazenando o tamanho do array de bytes do relatório para verificação
                int byteLength = Convert.ToInt32(byteViewer.Length.ToString());

                //se a variável snAnexo for zero, o PDF será aberto no Browser
                //Caso contrário,abrirá o prompt para salvar o arquivo
                if (isFile)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Length", byteLength.ToString());
                    Response.AddHeader("Content-Disposition", "attachment; filename=Relatorio.xls");
                    Response.BinaryWrite(byteViewer);
                    Response.End();
                }
                else
                {
                    Response.Clear();
                    Response.AddHeader("Content-Length", byteLength.ToString());
                    /*
                     * Valores possíveis para Response.ContentType
                     	text/html
	                    text/xml
	                    image/tiff
	                    application/pdf
	                    application/xml
                        application/vnd.ms-excel	
                        application/msword
                     */
                    Response.ContentType = "application/xls";
                    Response.AddHeader("Content-Disposition", "inline; filename=Relatorio.xls");
                    Response.BinaryWrite(byteViewer);
                    Response.End();
                }
            }
            catch (FormatException)
            {
            }
            
        }
    }
}
