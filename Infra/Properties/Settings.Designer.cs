﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace IMOD.Infra.Properties {
    
    
    [CompilerGenerated()]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [ApplicationScopedSetting()]
        [DebuggerNonUserCode()]
        [SpecialSetting(SpecialSetting.ConnectionString)]
        [DefaultSettingValue("Data Source=172.16.190.108\\SQLEXPRESS;Initial Catalog=D_iModCredenciamento;User I" +
            "D=imod;Password=imod;Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Conn" +
            "ection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True")]
        public string Credenciamento {
            get {
                return ((string)(this["Credenciamento"]));
            }
        }
    }
}
