﻿#pragma checksum "..\..\..\Views\UsColaboradoresCursosView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "71A65D8D3040E55CF21EF1681943911CF7ACFA75"
//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
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
    /// UsColaboradoresCursosView
    /// </summary>
    public partial class UsColaboradoresCursosView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox InformacoesCursos_gb;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel InformacoesAnexo_sp;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox cmbEmpresa;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbCurso;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DataValidade_dp;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NomeArquivo_tb;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BuscarCursoArquivo_bt;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AbrirCursoArquivo_bt;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox Impeditivo_cb;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListaColaboradoresCursos_lv;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAdicionar;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEditar;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExcluir;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\..\Views\UsColaboradoresCursosView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancelar;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\Views\UsColaboradoresCursosView.xaml"
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
            System.Uri resourceLocater = new System.Uri("/IMOD.CredenciamentoDeskTop;component/views/uscolaboradorescursosview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\UsColaboradoresCursosView.xaml"
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
            
            #line 20 "..\..\..\Views\UsColaboradoresCursosView.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Grid_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.InformacoesCursos_gb = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 3:
            this.InformacoesAnexo_sp = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.cmbEmpresa = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.cmbCurso = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.DataValidade_dp = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 7:
            this.NomeArquivo_tb = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.BuscarCursoArquivo_bt = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\Views\UsColaboradoresCursosView.xaml"
            this.BuscarCursoArquivo_bt.Click += new System.Windows.RoutedEventHandler(this.OnUpLoad_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.AbrirCursoArquivo_bt = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\..\Views\UsColaboradoresCursosView.xaml"
            this.AbrirCursoArquivo_bt.Click += new System.Windows.RoutedEventHandler(this.OnAbrirArquivo_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.Impeditivo_cb = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 11:
            this.ListaColaboradoresCursos_lv = ((System.Windows.Controls.ListView)(target));
            return;
            case 12:
            this.btnAdicionar = ((System.Windows.Controls.Button)(target));
            return;
            case 13:
            this.btnEditar = ((System.Windows.Controls.Button)(target));
            return;
            case 14:
            this.btnExcluir = ((System.Windows.Controls.Button)(target));
            return;
            case 15:
            this.btnCancelar = ((System.Windows.Controls.Button)(target));
            return;
            case 16:
            this.btnSalvar = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

