﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebServiceRGS.Tester.RGSws {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RGSws.RGSWsSoap")]
    public interface RGSWsSoap {
        
        // CODEGEN: Generating message contract since element name serializedReportContainer from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RenderReports", ReplyAction="*")]
        WebServiceRGS.Tester.RGSws.RenderReportsResponse RenderReports(WebServiceRGS.Tester.RGSws.RenderReportsRequest request);
        
        // CODEGEN: Generating message contract since element name reportGuid from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetReportFileByGUID", ReplyAction="*")]
        WebServiceRGS.Tester.RGSws.GetReportFileByGUIDResponse GetReportFileByGUID(WebServiceRGS.Tester.RGSws.GetReportFileByGUIDRequest request);
        
        // CODEGEN: Generating message contract since element name reportGuid from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RenderReportByGUID", ReplyAction="*")]
        WebServiceRGS.Tester.RGSws.RenderReportByGUIDResponse RenderReportByGUID(WebServiceRGS.Tester.RGSws.RenderReportByGUIDRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RenderReportsRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RenderReports", Namespace="http://tempuri.org/", Order=0)]
        public WebServiceRGS.Tester.RGSws.RenderReportsRequestBody Body;
        
        public RenderReportsRequest() {
        }
        
        public RenderReportsRequest(WebServiceRGS.Tester.RGSws.RenderReportsRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class RenderReportsRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string serializedReportContainer;
        
        public RenderReportsRequestBody() {
        }
        
        public RenderReportsRequestBody(string serializedReportContainer) {
            this.serializedReportContainer = serializedReportContainer;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RenderReportsResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RenderReportsResponse", Namespace="http://tempuri.org/", Order=0)]
        public WebServiceRGS.Tester.RGSws.RenderReportsResponseBody Body;
        
        public RenderReportsResponse() {
        }
        
        public RenderReportsResponse(WebServiceRGS.Tester.RGSws.RenderReportsResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class RenderReportsResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string RenderReportsResult;
        
        public RenderReportsResponseBody() {
        }
        
        public RenderReportsResponseBody(string RenderReportsResult) {
            this.RenderReportsResult = RenderReportsResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetReportFileByGUIDRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetReportFileByGUID", Namespace="http://tempuri.org/", Order=0)]
        public WebServiceRGS.Tester.RGSws.GetReportFileByGUIDRequestBody Body;
        
        public GetReportFileByGUIDRequest() {
        }
        
        public GetReportFileByGUIDRequest(WebServiceRGS.Tester.RGSws.GetReportFileByGUIDRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetReportFileByGUIDRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string reportGuid;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public bool deleteFile;
        
        public GetReportFileByGUIDRequestBody() {
        }
        
        public GetReportFileByGUIDRequestBody(string reportGuid, bool deleteFile) {
            this.reportGuid = reportGuid;
            this.deleteFile = deleteFile;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetReportFileByGUIDResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetReportFileByGUIDResponse", Namespace="http://tempuri.org/", Order=0)]
        public WebServiceRGS.Tester.RGSws.GetReportFileByGUIDResponseBody Body;
        
        public GetReportFileByGUIDResponse() {
        }
        
        public GetReportFileByGUIDResponse(WebServiceRGS.Tester.RGSws.GetReportFileByGUIDResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetReportFileByGUIDResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetReportFileByGUIDResult;
        
        public GetReportFileByGUIDResponseBody() {
        }
        
        public GetReportFileByGUIDResponseBody(string GetReportFileByGUIDResult) {
            this.GetReportFileByGUIDResult = GetReportFileByGUIDResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RenderReportByGUIDRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RenderReportByGUID", Namespace="http://tempuri.org/", Order=0)]
        public WebServiceRGS.Tester.RGSws.RenderReportByGUIDRequestBody Body;
        
        public RenderReportByGUIDRequest() {
        }
        
        public RenderReportByGUIDRequest(WebServiceRGS.Tester.RGSws.RenderReportByGUIDRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class RenderReportByGUIDRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string reportGuid;
        
        public RenderReportByGUIDRequestBody() {
        }
        
        public RenderReportByGUIDRequestBody(string reportGuid) {
            this.reportGuid = reportGuid;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RenderReportByGUIDResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RenderReportByGUIDResponse", Namespace="http://tempuri.org/", Order=0)]
        public WebServiceRGS.Tester.RGSws.RenderReportByGUIDResponseBody Body;
        
        public RenderReportByGUIDResponse() {
        }
        
        public RenderReportByGUIDResponse(WebServiceRGS.Tester.RGSws.RenderReportByGUIDResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class RenderReportByGUIDResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string RenderReportByGUIDResult;
        
        public RenderReportByGUIDResponseBody() {
        }
        
        public RenderReportByGUIDResponseBody(string RenderReportByGUIDResult) {
            this.RenderReportByGUIDResult = RenderReportByGUIDResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface RGSWsSoapChannel : WebServiceRGS.Tester.RGSws.RGSWsSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RGSWsSoapClient : System.ServiceModel.ClientBase<WebServiceRGS.Tester.RGSws.RGSWsSoap>, WebServiceRGS.Tester.RGSws.RGSWsSoap {
        
        public RGSWsSoapClient() {
        }
        
        public RGSWsSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RGSWsSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RGSWsSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RGSWsSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WebServiceRGS.Tester.RGSws.RenderReportsResponse WebServiceRGS.Tester.RGSws.RGSWsSoap.RenderReports(WebServiceRGS.Tester.RGSws.RenderReportsRequest request) {
            return base.Channel.RenderReports(request);
        }
        
        public string RenderReports(string serializedReportContainer) {
            WebServiceRGS.Tester.RGSws.RenderReportsRequest inValue = new WebServiceRGS.Tester.RGSws.RenderReportsRequest();
            inValue.Body = new WebServiceRGS.Tester.RGSws.RenderReportsRequestBody();
            inValue.Body.serializedReportContainer = serializedReportContainer;
            WebServiceRGS.Tester.RGSws.RenderReportsResponse retVal = ((WebServiceRGS.Tester.RGSws.RGSWsSoap)(this)).RenderReports(inValue);
            return retVal.Body.RenderReportsResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WebServiceRGS.Tester.RGSws.GetReportFileByGUIDResponse WebServiceRGS.Tester.RGSws.RGSWsSoap.GetReportFileByGUID(WebServiceRGS.Tester.RGSws.GetReportFileByGUIDRequest request) {
            return base.Channel.GetReportFileByGUID(request);
        }
        
        public string GetReportFileByGUID(string reportGuid, bool deleteFile) {
            WebServiceRGS.Tester.RGSws.GetReportFileByGUIDRequest inValue = new WebServiceRGS.Tester.RGSws.GetReportFileByGUIDRequest();
            inValue.Body = new WebServiceRGS.Tester.RGSws.GetReportFileByGUIDRequestBody();
            inValue.Body.reportGuid = reportGuid;
            inValue.Body.deleteFile = deleteFile;
            WebServiceRGS.Tester.RGSws.GetReportFileByGUIDResponse retVal = ((WebServiceRGS.Tester.RGSws.RGSWsSoap)(this)).GetReportFileByGUID(inValue);
            return retVal.Body.GetReportFileByGUIDResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WebServiceRGS.Tester.RGSws.RenderReportByGUIDResponse WebServiceRGS.Tester.RGSws.RGSWsSoap.RenderReportByGUID(WebServiceRGS.Tester.RGSws.RenderReportByGUIDRequest request) {
            return base.Channel.RenderReportByGUID(request);
        }
        
        public string RenderReportByGUID(string reportGuid) {
            WebServiceRGS.Tester.RGSws.RenderReportByGUIDRequest inValue = new WebServiceRGS.Tester.RGSws.RenderReportByGUIDRequest();
            inValue.Body = new WebServiceRGS.Tester.RGSws.RenderReportByGUIDRequestBody();
            inValue.Body.reportGuid = reportGuid;
            WebServiceRGS.Tester.RGSws.RenderReportByGUIDResponse retVal = ((WebServiceRGS.Tester.RGSws.RGSWsSoap)(this)).RenderReportByGUID(inValue);
            return retVal.Body.RenderReportByGUIDResult;
        }
    }
}
