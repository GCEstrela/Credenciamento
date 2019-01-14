﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 12 - 2018
// ***********************************************************************

#region

using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Genetec.Sdk.Workspace;
using IMOD.Application.Service;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace iModSCCredenciamento.Modulo
{
    /// <summary>
    ///     Interaction logic for MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipalView
    {
        #region  Propriedades

        public static Workspace Workspace { get; private set; } 

        #endregion

        public MenuPrincipalView()
        {
            InitializeComponent(); 
            txtVersao.Text = VersaoSoftware; 
        }
        /// <summary>
        ///     Versao do Sistema
        /// </summary>
        /// <returns></returns>
        private static string VersaoSoftware
        {
            get
            {
                return ConfiguracaoService.ObterVersaoSoftware(Assembly.GetExecutingAssembly());
            }
        }
        #region  Metodos

        public void Initialize(Workspace wrk)
        {
            if (wrk == null)
                throw new ArgumentNullException (nameof (wrk));
            Workspace = wrk;

            DataContext = null; //Iniciar sem conteudo na tela do frame
        }

        /// <summary>
        ///     Obter view Empresa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEmpresaView_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

            this.ColaboradoresBt.Background = Brushes.Transparent;
            this.VeiculosBt.Background = Brushes.Transparent;
            this.ConfiguracoesBt.Background = Brushes.Transparent;
            this.RelatoriosBt.Background = Brushes.Transparent;
            this.TermosBt.Background = Brushes.Transparent;

            DataContext = new ViewSingleton().EmpresaView;
        }

        /// <summary>
        ///     Obter view Colaborador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnColaboradoresView_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

            this.EmpresasBt.Background = Brushes.Transparent;
            this.VeiculosBt.Background = Brushes.Transparent;
            this.ConfiguracoesBt.Background = Brushes.Transparent;
            this.RelatoriosBt.Background = Brushes.Transparent;
            this.TermosBt.Background = Brushes.Transparent;

            DataContext = new ViewSingleton().ColaboradorView;
        }

        /// <summary>
        ///     Obter view Veiculos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVeiculo_Click(object sender, RoutedEventArgs e)
        {

            Button btn = e.Source as Button;
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

            this.EmpresasBt.Background = Brushes.Transparent;
            this.ColaboradoresBt.Background = Brushes.Transparent;
            this.ConfiguracoesBt.Background = Brushes.Transparent;
            this.RelatoriosBt.Background = Brushes.Transparent;
            this.TermosBt.Background = Brushes.Transparent;

            DataContext = new ViewSingleton().VeiculoView;
        }

        /// <summary>
        ///     Obter view Configurações
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfiguracao_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

            this.EmpresasBt.Background = Brushes.Transparent;
            this.ColaboradoresBt.Background = Brushes.Transparent;
            this.VeiculosBt.Background = Brushes.Transparent;
            this.RelatoriosBt.Background = Brushes.Transparent;
            this.TermosBt.Background = Brushes.Transparent;

            DataContext = new ViewSingleton().ConfiguracoesView;
        }

        /// <summary>
        ///     Obter view Relatorios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelatorio_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

            this.EmpresasBt.Background = Brushes.Transparent;
            this.ColaboradoresBt.Background = Brushes.Transparent;
            this.VeiculosBt.Background = Brushes.Transparent;
            this.ConfiguracoesBt.Background = Brushes.Transparent;
            this.TermosBt.Background = Brushes.Transparent;

            DataContext = new ViewSingleton().RelatoriosView;
        }

        /// <summary>
        ///     Obter view Termo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTermo_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

            this.EmpresasBt.Background = Brushes.Transparent;
            this.ColaboradoresBt.Background = Brushes.Transparent;
            this.VeiculosBt.Background = Brushes.Transparent;
            this.ConfiguracoesBt.Background = Brushes.Transparent;
            this.RelatoriosBt.Background = Brushes.Transparent;

            DataContext = new ViewSingleton().TermosView;
        }

        #endregion
    }
}