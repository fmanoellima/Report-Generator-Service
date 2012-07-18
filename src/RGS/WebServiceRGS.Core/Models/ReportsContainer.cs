using System.Collections.Generic;
using System;

namespace WebServiceRGS.Core.Models
{
    public class ReportsContainer
    {
        public Guid ReportsContainerGuid { get; set; }

        public CredentialType CredentialType { get; set; }

        public string NetworkDomain { get; set; }

        public string ReportUserLogin { get; set; }

        public string ReportUserPassword { get; set; }

        public string ReportServiceUrl { get; set; }

        public string ReportExecutionUrl { get; set; }

        public bool LogReports { get; set; }

        public List<Report> Reports { get; set; }

        public ReportsContainer()
        {
            Reports = new List<Report>();
        }
    }

    public enum CredentialType
    {
        DefaultCredential,
        NetworkCredential,
        CustomCredential
    }
}