﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1022
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Efika.Crm.AccesoServicios.Properties {
    
    
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
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.16.4.252:5555/mscrmservices/2007/MetaDataService.asmx")]
        public string Efika_Crm_AccesoServicios_crmmetadatasdk_MetadataService {
            get {
                return ((string)(this["Efika_Crm_AccesoServicios_crmmetadatasdk_MetadataService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://serviciostest.bmsc.com.bo:443/soa-infra/services/crm/pjtObtenerTasaSCA/Ob" +
            "tenerTasaService")]
        public string Efika_Crm_AccesoServicios_BMSCTASAS_ObtenerTasaService {
            get {
                return ((string)(this["Efika_Crm_AccesoServicios_BMSCTASAS_ObtenerTasaService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://srvdesacrm01:5555/MSCrmServices/2007/CrmService.asmx")]
        public string Efika_Crm_AccesoServicios_CRMSDK_CrmService {
            get {
                return ((string)(this["Efika_Crm_AccesoServicios_CRMSDK_CrmService"]));
            }
        }
    }
}