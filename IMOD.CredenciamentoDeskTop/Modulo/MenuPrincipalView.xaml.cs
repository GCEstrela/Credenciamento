// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 12 - 2018
// ***********************************************************************

#region

using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Genetec.Sdk.Workspace;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;
using IMOD.Domain.EntitiesCustom;

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
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                txtVersao.Text = "";
                txtVersao.Text = VersaoSoftware;
                _viewSingleton = new ViewSingleton();
                //var tt = UsuarioLogado.Nome;
                
            }
            catch (Exception ex)
            {
               // WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
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
                return ConfiguracaoService.ObterVersaoSoftware(Assembly.GetExecutingAssembly());
            }
        }

        private static string ObterNomeDataBase
        {
            get
            {
                var config = new ConfiguracaoService();
                var nomeDataBase = config.ObterInformacaoBancoDeDados.BaseDados;
                return nomeDataBase;
            }
        }
        #region  Metodos 

        public void Initialize(Workspace wrk)
        {
            if (wrk == null)
                throw new ArgumentNullException(nameof(wrk));
            Workspace = wrk;

            DataContext = null; //Iniciar sem conteudo na tela do frame
            if (!UsuarioLogado.Adm)
            {
                this.ConfiguracoesBt.IsEnabled = false;
                return;
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
            try
            {
                //Autor:Valnei Filho
                //Data:12/03/2019
                //Wrk:Ao fechar a janela, as coleções (observable) devem ser limpas para possibilitar uma nova pesquisa
                //Limpar dados dos observables principais de suas respectivas views
                txtVersao.Text = txtVersao.Text + " - " + UsuarioLogado.sdiLicenca;
                var x1 = (ColaboradorViewModel)_viewSingleton.ColaboradorView.DataContext;
                x1.EntityObserver.Clear();
                var x2 = (EmpresaViewModel)_viewSingleton.EmpresaView.DataContext;
                x2.EntityObserver.Clear();
                var x3 = (VeiculoViewModel)_viewSingleton.VeiculoView.DataContext;
                x3.EntityObserver.Clear();
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox("Erro ao conectar com a base de dados SQL. O sistema estará inoperante até que a conexão seja normalizada novamente. Por favor, entre em contato com o administrador do banco!",1);
                this.EmpresasBt.IsEnabled = false;
                this.ColaboradoresBt.IsEnabled = false;
                this.VeiculosBt.IsEnabled = false;
                this.EquipamentosBt.IsEnabled = false;
                this.ConfiguracoesBt.IsEnabled = false;
                this.RelatoriosBt.IsEnabled = false;
                this.TermosBt.IsEnabled = false;
               
            }
            catch (Exception ex)
            {
               
                WpfHelp.PopupBox(ex);
                
                //throw ex;
            }


            //======================================================================= 
        }

        //private IAsyncResult m_loggingOnResult;
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //Workspace.Sdk.EndLogOn(m_loggingOnResult);
            //_viewSingleton = null;
            //Workspace = null;
        }
    }
}