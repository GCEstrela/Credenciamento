﻿#pragma checksum "..\..\..\Views\ColaboradoresCredenciaisView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1BA8E61F9B2FCA159BFF97B2BF279B136DC9FA7B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Genetec.Sdk.Controls;
using Genetec.Sdk.Controls.Themes.Styles;
using Genetec.Sdk.Maps;
using IMOD.CredenciamentoDeskTop.Funcoes;
using IMOD.CredenciamentoDeskTop.ViewModels;
using RootLibrary.WPF.Localization;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace IMOD.CredenciamentoDeskTop.Views {
    
    
    /// <summary>
    /// ColaboradoresCredenciaisView
    /// </summary>
    public partial class ColaboradoresCredenciaisView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox Responsavel_gb;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox EmpresaVinculo_cb;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox StatusCredencial_cb;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox MotivoCredencial_cb;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox TipoCredencial_cb;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ModeloCredencial_cb;
        
        #line default
        #line hidden
        
        
        #line 137 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NumeroColete_tb;
        
        #line default
        #line hidden
        
        
        #line 143 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox TecnologiaCredencial_cb;
        
        #line default
        #line hidden
        
        
        #line 156 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FC_tb;
        
        #line default
        #line hidden
        
        
        #line 172 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Privilegio1_tb;
        
        #line default
        #line hidden
        
        
        #line 183 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Privilegio2_tb;
        
        #line default
        #line hidden
        
        
        #line 198 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox FormatoCredencial_cb;
        
        #line default
        #line hidden
        
        
        #line 215 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NumeroCredencial_tb;
        
        #line default
        #line hidden
        
        
        #line 230 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button brnImprimirCredencial;
        
        #line default
        #line hidden
        
        
        #line 235 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListaColaboradoresCredenciais_lv;
        
        #line default
        #line hidden
        
        
        #line 319 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAdicionar;
        
        #line default
        #line hidden
        
        
        #line 322 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEditar;
        
        #line default
        #line hidden
        
        
        #line 325 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExcluir;
        
        #line default
        #line hidden
        
        
        #line 328 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancelar;
        
        #line default
        #line hidden
        
        
        #line 331 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSalvar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/IMOD.CredenciamentoDeskTop;component/views/colaboradorescredenciaisview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
            ((IMOD.CredenciamentoDeskTop.Views.ColaboradoresCredenciaisView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Responsavel_gb = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 3:
            this.EmpresaVinculo_cb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.StatusCredencial_cb = ((System.Windows.Controls.ComboBox)(target));
            
            #line 75 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
            this.StatusCredencial_cb.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.StatusCredencial_cb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.MotivoCredencial_cb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.TipoCredencial_cb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.ModeloCredencial_cb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.NumeroColete_tb = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.TecnologiaCredencial_cb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 10:
            this.FC_tb = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.Privilegio1_tb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 12:
            this.Privilegio2_tb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 13:
            this.FormatoCredencial_cb = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 14:
            this.NumeroCredencial_tb = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            this.brnImprimirCredencial = ((System.Windows.Controls.Button)(target));
            
            #line 231 "..\..\..\Views\ColaboradoresCredenciaisView.xaml"
            this.brnImprimirCredencial.Click += new System.Windows.RoutedEventHandler(this.ImprimirCredencial_bt_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.ListaColaboradoresCredenciais_lv = ((System.Windows.Controls.ListView)(target));
            return;
            case 17:
            this.btnAdicionar = ((System.Windows.Controls.Button)(target));
            return;
            case 18:
            this.btnEditar = ((System.Windows.Controls.Button)(target));
            return;
            case 19:
            this.btnExcluir = ((System.Windows.Controls.Button)(target));
            return;
            case 20:
            this.btnCancelar = ((System.Windows.Controls.Button)(target));
            return;
            case 21:
            this.btnSalvar = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

