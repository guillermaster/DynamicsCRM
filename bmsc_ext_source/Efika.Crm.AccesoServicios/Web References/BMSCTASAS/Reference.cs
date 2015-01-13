﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.296.
// 
#pragma warning disable 1591

namespace Efika.Crm.AccesoServicios.BMSCTASAS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ObtenerTasa_pttBinding", Namespace="/bmsc/crm/ObtenerTasaService")]
    public partial class ObtenerTasaService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ObtenerTasaOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ObtenerTasaService() {
            this.Url = global::Efika.Crm.AccesoServicios.Properties.Settings.Default.Efika_Crm_AccesoServicios_BMSCTASAS_ObtenerTasaService;
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
        public event ObtenerTasaCompletedEventHandler ObtenerTasaCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("ObtenerTasa", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("obtenerTasa_response", Namespace="/bmsc/crm")]
        public obtenerTasa_response ObtenerTasa([System.Xml.Serialization.XmlElementAttribute(Namespace="/bmsc/crm")] obtenerTasa_request obtenerTasa_request) {
            object[] results = this.Invoke("ObtenerTasa", new object[] {
                        obtenerTasa_request});
            return ((obtenerTasa_response)(results[0]));
        }
        
        /// <remarks/>
        public void ObtenerTasaAsync(obtenerTasa_request obtenerTasa_request) {
            this.ObtenerTasaAsync(obtenerTasa_request, null);
        }
        
        /// <remarks/>
        public void ObtenerTasaAsync(obtenerTasa_request obtenerTasa_request, object userState) {
            if ((this.ObtenerTasaOperationCompleted == null)) {
                this.ObtenerTasaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerTasaOperationCompleted);
            }
            this.InvokeAsync("ObtenerTasa", new object[] {
                        obtenerTasa_request}, this.ObtenerTasaOperationCompleted, userState);
        }
        
        private void OnObtenerTasaOperationCompleted(object arg) {
            if ((this.ObtenerTasaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ObtenerTasaCompleted(this, new ObtenerTasaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="/bmsc/crm")]
    public partial class obtenerTasa_request {
        
        private string pTipoClienteField;
        
        private string pSegmentoClienteField;
        
        private string pMonedaOperacionField;
        
        private string pProductoField;
        
        private string pUsuarioField;
        
        private string pContrasenaField;
        
        private string pDominioField;
        
        /// <remarks/>
        public string pTipoCliente {
            get {
                return this.pTipoClienteField;
            }
            set {
                this.pTipoClienteField = value;
            }
        }
        
        /// <remarks/>
        public string pSegmentoCliente {
            get {
                return this.pSegmentoClienteField;
            }
            set {
                this.pSegmentoClienteField = value;
            }
        }
        
        /// <remarks/>
        public string pMonedaOperacion {
            get {
                return this.pMonedaOperacionField;
            }
            set {
                this.pMonedaOperacionField = value;
            }
        }
        
        /// <remarks/>
        public string pProducto {
            get {
                return this.pProductoField;
            }
            set {
                this.pProductoField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="/bmsc/crm")]
    public partial class Result {
        
        private string campoField;
        
        private string valorField;
        
        /// <remarks/>
        public string Campo {
            get {
                return this.campoField;
            }
            set {
                this.campoField = value;
            }
        }
        
        /// <remarks/>
        public string Valor {
            get {
                return this.valorField;
            }
            set {
                this.valorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="/bmsc/crm")]
    public partial class obtenerTasa_response {
        
        private string pMensajeField;
        
        private string pRespuestaField;
        
        private Result[] pResultadoField;
        
        /// <remarks/>
        public string pMensaje {
            get {
                return this.pMensajeField;
            }
            set {
                this.pMensajeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string pRespuesta {
            get {
                return this.pRespuestaField;
            }
            set {
                this.pRespuestaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("pResultado")]
        public Result[] pResultado {
            get {
                return this.pResultadoField;
            }
            set {
                this.pResultadoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ObtenerTasaCompletedEventHandler(object sender, ObtenerTasaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ObtenerTasaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ObtenerTasaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public obtenerTasa_response Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((obtenerTasa_response)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591