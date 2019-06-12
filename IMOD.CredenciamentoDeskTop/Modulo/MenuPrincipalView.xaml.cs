// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 12 - 2018
// ***********************************************************************

#region

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Genetec.Sdk.Workspace;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;

#endregion

namespace IMOD.CredenciamentoDeskTop.Modulo
{
    /// <summary>
    ///     Interaction logic for MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipalView
    {
        #region  Propriedades

        public static Workspace Workspace { get; private set; }
        private ViewSingleton _viewSingleton;

        #endregion

        public MenuPrincipalView()
        {
            try
            {
                InitializeComponent();
                txtVersao.Text = VersaoSoftware;
                _viewSingleton = new ViewSingleton();
            }
            catch (Exception ex)
            {
                WpfHelp.MboxError(ex);
            }
        }
        /// <summary>
        ///     Versao do Sistema
        /// </summary>
        /// <returns></returns>
        private static string VersaoSoftware
        {
            get
            {
                try
                {
                    return ConfiguracaoService.ObterVersaoSoftware(Assembly.GetExecutingAssembly());
                }
                catch (Exception ex)
                {
                    WpfHelp.MboxError(ex);
                    return null;
                }
            }
        }

        private static string ObterNomeDataBase
        {
            get
            {
                try
                {
                    var config = new ConfiguracaoService();
                    var nomeDataBase = config.ObterInformacaoBancoDeDados.BaseDados;
                    return nomeDataBase;
                }
                catch (Exception ex)
                {
                    WpfHelp.MboxError(ex);
                    return null;
                }

            }
        }
        #region  Metodos 

        public void Initialize(Workspace wrk)
        {
            try
            {
                if (wrk == null)
                    throw new ArgumentNullException(nameof(wrk));
                Workspace = wrk;

                DataContext = null; //Iniciar sem conteudo na tela do frame
            }
            catch (Exception ex)
            {
                WpfHelp.MboxError(ex);
            }
        }

        /// <summary>
        ///     Obter view Empresa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEmpresaView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = e.Source as Button;
                btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

                this.ColaboradoresBt.Background = Brushes.Transparent; 
                this.VeiculosBt.Background = Brushes.Transparent; 
                this.EquipamentosBt.Background = Brushes.Transparent;
                this.ConfiguracoesBt.Background = Brushes.Transparent;
                this.RelatoriosBt.Background = Brushes.Transparent;
                this.TermosBt.Background = Brushes.Transparent;

                DataContext = _viewSingleton.EmpresaView; //new ViewSingleton().EmpresaView;
            }
            catch (Exception ex)
            {
                WpfHelp.MboxError(ex);
            }
           
        }
         
        /// <summary>
        ///     Obter view Colaborador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnColaboradoresView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = e.Source as Button;
                btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

                this.EmpresasBt.Background = Brushes.Transparent;
                this.VeiculosBt.Background = Brushes.Transparent;
                this.EquipamentosBt.Background = Brushes.Transparent;
                this.ConfiguracoesBt.Background = Brushes.Transparent;
                this.RelatoriosBt.Background = Brushes.Transparent;
                this.TermosBt.Background = Brushes.Transparent;

                DataContext = _viewSingleton.ColaboradorView; //new ViewSingleton().ColaboradorView;
            }
            catch (Exception ex)
            {
                WpfHelp.MboxError(ex);
            }
           
        }

        /// <summary>
        ///     Obter view Veiculos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVeiculo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = e.Source as Button;
                btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

                this.EmpresasBt.Background = Brushes.Transparent;
                this.ColaboradoresBt.Background = Brushes.Transparent;
                this.EquipamentosBt.Background = Brushes.Transparent;
                this.ConfiguracoesBt.Background = Brushes.Transparent;
                this.RelatoriosBt.Background = Brushes.Transparent;
                this.TermosBt.Background = Brushes.Transparent;

                DataContext = _viewSingleton.VeiculoView; //new ViewSingleton().VeiculoView;
            }
            catch (Exception ex)
            {
                WpfHelp.MboxError(ex);
            }
           
        }

        /// <summary>
        ///     Obter view Configurações
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfiguracao_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Button btn = e.Source as Button;
                btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

                this.EmpresasBt.Background = Brushes.Transparent;
                this.ColaboradoresBt.Background = Brushes.Transparent;
                this.VeiculosBt.Background = Brushes.Transparent;
                this.EquipamentosBt.Background = Brushes.Transparent;
                this.RelatoriosBt.Background = Brushes.Transparent;
                this.TermosBt.Background = Brushes.Transparent;

                DataContext = _viewSingleton.ConfiguracoesView; //new ViewSingleton().ConfiguracoesView;
            }
            catch (Exception ex)
            {
                WpfHelp.MboxError(ex);
            }
            
        }

        /// <summary>
        ///     Obter view Relatorios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelatorio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = e.Source as Button;
                btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

                this.EmpresasBt.Background = Brushes.Transparent;
                this.ColaboradoresBt.Background = Brushes.Transparent;
                this.VeiculosBt.Background = Brushes.Transparent;
                this.EquipamentosBt.Background = Brushes.Transparent;
                this.ConfiguracoesBt.Background = Brushes.Transparent;
                this.TermosBt.Background = Brushes.Transparent;

                DataContext = _viewSingleton.RelatoriosView; //new ViewSingleton().RelatoriosView;
            }
            catch (Exception ex)
            {
                WpfHelp.MboxError(ex);
            }
            
        }

        /// <summary>
        ///     Obter view Termo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTermo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = e.Source as Button;
                btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

                this.EmpresasBt.Background = Brushes.Transparent;
                this.ColaboradoresBt.Background = Brushes.Transparent;
                this.VeiculosBt.Background = Brushes.Transparent;
                this.EquipamentosBt.Background = Brushes.Transparent;
                this.ConfiguracoesBt.Background = Brushes.Transparent;
                this.RelatoriosBt.Background = Brushes.Transparent;


                DataContext = _viewSingleton.TermosView; //new ViewSingleton().TermosView;
            }
            catch (Exception ex)
            {
                WpfHelp.MboxError(ex);
            }
            
        }

        /// <summary>
        ///     Obter view Equipamentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipamento_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                Button btn = e.Source as Button;
                btn.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF007ACC");

                this.EmpresasBt.Background = Brushes.Transparent;
                this.ColaboradoresBt.Background = Brushes.Transparent;
                this.VeiculosBt.Background = Brushes.Transparent;
                this.ConfiguracoesBt.Background = Brushes.Transparent;
                this.RelatoriosBt.Background = Brushes.Transparent;
                this.TermosBt.Background = Brushes.Transparent;
                

                DataContext = _viewSingleton.EquipamentosView; 
            } 
            catch (Exception ex)
            {
                WpfHelp.MboxError(ex);
            }
        }
        #endregion

        private void OnFrm_Loaded(object sender, RoutedEventArgs e)
        {
            //Autor:Valnei Filho
            //Data:12/03/2019
            //Wrk:Ao fechar a janela, as coleções (observable) devem ser limpas para possibilitar uma nova pesquisa
            //Limpar dados dos observables principais de suas respectivas views
            try
            {
                var x1 = (ColaboradorViewModel)_viewSingleton.ColaboradorView.DataContext;
                x1.EntityObserver.Clear();
                var x2 = (EmpresaViewModel)_viewSingleton.EmpresaView.DataContext;
                x2.EntityObserver.Clear();
                var x3 = (VeiculoViewModel)_viewSingleton.VeiculoView.DataContext;
                x3.EntityObserver.Clear();

                //======================================================================= 
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex);               
            }
            
        }
    }
}