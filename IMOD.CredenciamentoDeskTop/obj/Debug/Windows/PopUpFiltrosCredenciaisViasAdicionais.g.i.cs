﻿#pragma checksum "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "56D865ED6A1B00BF2032779DE4BE37BF71EDC135"
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


namespace IMOD.CredenciamentoDeskTop.Windows {
    
    
    /// <summary>
    /// PopUpFiltrosCredenciaisViasAdicionais
    /// </summary>
    public partial class PopUpFiltrosCredenciaisViasAdicionais : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 42 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox Informacoes_gb;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Informacoes_sp;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton todas_rb;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton segundaemissao_rb;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton terceiraemissao_rb;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox data_gb;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dp_dataInicial;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dp_dataFinal;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button;
        
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
            System.Uri resourceLocater = new System.Uri("/IMOD.CredenciamentoDeskTop;component/windows/popupfiltroscredenciaisviasadiciona" +
                    "is.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
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
            
            #line 28 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Informacoes_gb = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 3:
            this.Informacoes_sp = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.todas_rb = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.segundaemissao_rb = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 6:
            this.terceiraemissao_rb = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 7:
            this.data_gb = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 8:
            this.dp_dataInicial = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 9:
            this.dp_dataFinal = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 10:
            this.button = ((System.Windows.Controls.Button)(target));
            
            #line 61 "..\..\..\Windows\PopUpFiltrosCredenciaisViasAdicionais.xaml"
            this.button.Click += new System.Windows.RoutedEventHandler(this.button_ClickFiltrar);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

