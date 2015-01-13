﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18052.
// 
#pragma warning disable 1591

namespace Efika.Crm.WebService.BMSCSegmentacion {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ObtenerSegmentoMServiceSoap", Namespace="ObtenerSegmento")]
    public partial class ObtenerSegmentoMService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ObtenerSegmentoMOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ObtenerSegmentoMService() {
            this.Url = global::Efika.Crm.WebService.Properties.Settings.Default.Efika_Crm_WebService_BMSCSegmentacion_ObtenerSegmentoService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ObtenerSegmentoMCompletedEventHandler ObtenerSegmentoMCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("ObtenerSegmento/ObtenerSegmentoM", RequestNamespace="ObtenerSegmento", ResponseNamespace="ObtenerSegmento", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public OutputParametersM ObtenerSegmentoM(InputParametersM InputParameters) {
            object[] results = this.Invoke("ObtenerSegmentoM", new object[] {
                        InputParameters});
            return ((OutputParametersM)(results[0]));
        }
        
        /// <remarks/>
        public void ObtenerSegmentoMAsync(InputParametersM InputParameters) {
            this.ObtenerSegmentoMAsync(InputParameters, null);
        }
        
        /// <remarks/>
        public void ObtenerSegmentoMAsync(InputParametersM InputParameters, object userState) {
            if ((this.ObtenerSegmentoMOperationCompleted == null)) {
                this.ObtenerSegmentoMOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerSegmentoMOperationCompleted);
            }
            this.InvokeAsync("ObtenerSegmentoM", new object[] {
                        InputParameters}, this.ObtenerSegmentoMOperationCompleted, userState);
        }
        
        private void OnObtenerSegmentoMOperationCompleted(object arg) {
            if ((this.ObtenerSegmentoMCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ObtenerSegmentoMCompleted(this, new ObtenerSegmentoMCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="ObtenerSegmento")]
    public partial class InputParametersM {
        
        private int pMonedaField;
        
        private System.Nullable<decimal> pTipoIngresoField;
        
        private bool pTipoIngresoFieldSpecified;
        
        private System.Nullable<decimal> pIngresoField;
        
        private bool pIngresoFieldSpecified;
        
        private string pUsuarioField;
        
        private string pContrasenaField;
        
        private string pDominioField;
        
        /// <remarks/>
        public int pMoneda {
            get {
                return this.pMonedaField;
            }
            set {
                this.pMonedaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> pTipoIngreso {
            get {
                return this.pTipoIngresoField;
            }
            set {
                this.pTipoIngresoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pTipoIngresoSpecified {
            get {
                return this.pTipoIngresoFieldSpecified;
            }
            set {
                this.pTipoIngresoFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> pIngreso {
            get {
                return this.pIngresoField;
            }
            set {
                this.pIngresoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pIngresoSpecified {
            get {
                return this.pIngresoFieldSpecified;
            }
            set {
                this.pIngresoFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string pUsuario {
            get {
                return this.pUsuarioField;
            }
            set {
                this.pUsuarioField = value;
            }
        }
        
        /// <remarks/>
        public string pContrasena {
            get {
                return this.pContrasenaField;
            }
            set {
                this.pContrasenaField = value;
            }
        }
        
        /// <remarks/>
        public string pDominio {
            get {
                return this.pDominioField;
            }
            set {
                this.pDominioField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="ObtenerSegmento")]
    public partial class OutputParametersM {
        
        private decimal pSegmentoField;
        
        private decimal pResultadoField;
        
        private string pMensajeField;
        
        /// <remarks/>
        public decimal pSegmento {
            get {
                return this.pSegmentoField;
            }
            set {
                this.pSegmentoField = value;
            }
        }
        
        /// <remarks/>
        public decimal pResultado {
            get {
                return this.pResultadoField;
            }
            set {
                this.pResultadoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string pMensaje {
            get {
                return this.pMensajeField;
            }
            set {
                this.pMensajeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void ObtenerSegmentoMCompletedEventHandler(object sender, ObtenerSegmentoMCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ObtenerSegmentoMCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ObtenerSegmentoMCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public OutputParametersM Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((OutputParametersM)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591