﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Efika.Crm.Web.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://serviciosdesa.bmsc.com.bo:443/soa-infra/services/crm/pjtEvaluarScoreSCA/E" +
            "valuarScoreService")]
        public string Efika_Crm_Web_BMSCEvaluarScoreService_EvaluarScoreService {
            get {
                return ((string)(this["Efika_Crm_Web_BMSCEvaluarScoreService_EvaluarScoreService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.16.19.248:8001/soa-infra/services/crm/pjtEnviarCaratulaOnBaseSCA/Carat" +
            "ulaOnbaseService")]
        public string Efika_Crm_Web_BMSCOnBase_CaratulaOnbaseService {
            get {
                return ((string)(this["Efika_Crm_Web_BMSCOnBase_CaratulaOnbaseService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://serviciosdesa.bmsc.com.bo:443/soa-infra/services/clientes/pjtTipoCambioSC" +
            "A/TipoCambioService")]
        public string Efika_Crm_Web_BMSCTipoCambio_TipoCambioService {
            get {
                return ((string)(this["Efika_Crm_Web_BMSCTipoCambio_TipoCambioService"]));
            }
        }
    }
}
