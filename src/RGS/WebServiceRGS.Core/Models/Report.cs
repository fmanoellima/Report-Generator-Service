using System;
using System.Collections.Generic;

namespace WebServiceRGS.Core.Models
{
    public class Report
    {
        public string ReportName { get; set; }

        public ReportFormat ReportFormat { get; set; }

        public Dictionary<string, object> Parameters { get; set; }

        public bool ReportGenerationStatus { get; set; }

        public string ReportErrorDescription { get; set; }

        public bool StoreReportInDB { get; set; }

        public Guid ReportGuid { get; set; }

        public Guid ReportsContainerGuid { get; set; }

        public byte[] ReportFile { get; set; }

        public Report()
        {
            Parameters = new Dictionary<string, object>();
        }
    }

    public enum ReportFormat
    {
        CSV, 
        EXCEL,
        HTML4,
        IMAGE,
        MHTML,
        NULL,
        PDF, 
        WORD,
        XML
    }
}
