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

namespace Efika.Crm.Web.BMSCOnBase {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="CaratulaOnbase_pttBinding", Namespace="/bmsc/crm/CaratulaOnbaseService")]
    public partial class CaratulaOnbaseService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CaratulaOnbaseOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CaratulaOnbaseService() {
            this.Url = global::Efika.Crm.Web.Properties.Settings.Default.Efika_Crm_Web_BMSCOnBase_CaratulaOnbaseService;
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
        public event CaratulaOnbaseCompletedEventHandler CaratulaOnbaseCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("CaratulaOnbase", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("caratulaOnBase_response", Namespace="/bmsc/crm")]
        public caratulaOnBase_response CaratulaOnbase([System.Xml.Serialization.XmlElementAttribute(Namespace="/bmsc/crm")] caratulaOnBase_request caratulaOnBase_request) {
            object[] results = this.Invoke("CaratulaOnbase", new object[] {
                        caratulaOnBase_request});
            return ((caratulaOnBase_response)(results[0]));
        }
        
        /// <remarks/>
        public void CaratulaOnbaseAsync(caratulaOnBase_request caratulaOnBase_request) {
            this.CaratulaOnbaseAsync(caratulaOnBase_request, null);
        }
        
        /// <remarks/>
        public void CaratulaOnbaseAsync(caratulaOnBase_request caratulaOnBase_request, object userState) {
            if ((this.CaratulaOnbaseOperationCompleted == null)) {
                this.CaratulaOnbaseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCaratulaOnbaseOperationCompleted);
            }
            this.InvokeAsync("CaratulaOnbase", new object[] {
                        caratulaOnBase_request}, this.CaratulaOnbaseOperationCompleted, userState);
        }
        
        private void OnCaratulaOnbaseOperationCompleted(object arg) {
            if ((this.CaratulaOnbaseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CaratulaOnbaseCompleted(this, new CaratulaOnbaseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public partial class caratulaOnBase_request {
        
        private string pTipoClienteField;
        
        private decimal pNroSolicitudField;
        
        private string pTipoCreditoField;
        
        private decimal pMontoFinanciadoField;
        
        private string pTipoIdentificacionField;
        
        private string pNroIdentificacionField;
        
        private string pNombresField;
        
        private string pApellidosField;
        
        private string pEmailField;
        
        private string pTipoTrabajoField;
        
        private string pRazonSocialField;
        
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
        public decimal pNroSolicitud {
            get {
                return this.pNroSolicitudField;
            }
            set {
                this.pNroSolicitudField = value;
            }
        }
        
        /// <remarks/>
        public string pTipoCredito {
            get {
                return this.pTipoCreditoField;
            }
            set {
                this.pTipoCreditoField = value;
            }
        }
        
        /// <remarks/>
        public decimal pMontoFinanciado {
            get {
                return this.pMontoFinanciadoField;
            }
            set {
                this.pMontoFinanciadoField = value;
            }
        }
        
        /// <remarks/>
        public string pTipoIdentificacion {
            get {
                return this.pTipoIdentificacionField;
            }
            set {
                this.pTipoIdentificacionField = value;
            }
        }
        
        /// <remarks/>
        public string pNroIdentificacion {
            get {
                return this.pNroIdentificacionField;
            }
            set {
                this.pNroIdentificacionField = value;
            }
        }
        
        /// <remarks/>
        public string pNombres {
            get {
                return this.pNombresField;
            }
            set {
                this.pNombresField = value;
            }
        }
        
        /// <remarks/>
        public string pApellidos {
            get {
                return this.pApellidosField;
            }
            set {
                this.pApellidosField = value;
            }
        }
        
        /// <remarks/>
        public string pEmail {
            get {
                return this.pEmailField;
            }
            set {
                this.pEmailField = value;
            }
        }
        
        /// <remarks/>
        public string pTipoTrabajo {
            get {
                return this.pTipoTrabajoField;
            }
            set {
                this.pTipoTrabajoField = value;
            }
        }
        
        /// <remarks/>
        public string pRazonSocial {
            get {
                return this.pRazonSocialField;
            }
            set {
                this.pRazonSocialField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="/bmsc/crm")]
    public partial class caratulaOnBase_response {
        
        private string pMensajeField;
        
        private string pRespuestaField;
        
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
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void CaratulaOnbaseCompletedEventHandler(object sender, CaratulaOnbaseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CaratulaOnbaseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CaratulaOnbaseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public caratulaOnBase_response Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((caratulaOnBase_response)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591