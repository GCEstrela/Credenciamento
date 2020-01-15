using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Funcoes;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using Microsoft.Win32;

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    /// Interação lógica para ConfiguracoesView.xam
    /// </summary>
    public partial class ConfiguracoesView : UserControl
    {


        #region Inicializacao
        private readonly ConfiguracoesViewModel _viewModel;
        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();
        public ConfiguracoesView()
        {
            InitializeComponent();
            _viewModel = new ConfiguracoesViewModel();
            DataContext = _viewModel;
            _viewModel.CarregaConfiguracaoSistema();
              var config = _auxiliaresServiceConfiguraSistema.ConfiguraSistemaService.Listar().FirstOrDefault();
            Group_cb_.Text = config.GrupoPadrao.ToString();
        }
        #endregion

        #region Comando dos Botoes

        #region Relatórios 

        private void AbrirRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {

            ((ConfiguracoesViewModel)DataContext).OnAbrirRelatorioCommand();
        }

        private void BuscarRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnBuscarRelatorioCommand();

            CodigoRelatorio_tb.Text = ((ConfiguracoesViewModel)DataContext).RelatorioTemp.RelatorioId.ToString();
            DescricaoRelatorio_tb.Text = ((ConfiguracoesViewModel)DataContext).RelatorioTemp.NomeArquivoRpt;
        }

        private void btnAdicionarRelatorio_Click(object sender, RoutedEventArgs e)
        {
            DescricaoRelatorio_tb.Focus();
            BuscarRelatorio_bt.IsEnabled = true;

            btnSalvarRelatorio.IsEnabled = true;
            btnCancelarRelatorio.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarRelatorioCommand();
        }

        private void btnDeletarRelatorio_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarRelatorio.IsEnabled = true;
            btnSalvarRelatorio.IsEnabled = false;

            ((ConfiguracoesViewModel)DataContext).OnExcluirRelatorioCommand();
        }

        private void btnCancelarRelatorio_Click(object sender, RoutedEventArgs e)
        {
            btnSalvarRelatorio.IsEnabled = false;
            btnAdicionarRelatorio.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoRelatorios();
        }

        private void btnSalvarRelatorio_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarRelatorio.IsEnabled = true;
            btnSalvarRelatorio.IsEnabled = false;
            BuscarRelatorio_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarRelatorioCommand();
        }

        #endregion 

        #region Relatórios Gerenciais
        private void AbrirRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAbrirRelatorioGerencialCommand();
        }

        private void BuscarRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnBuscarRelatorioGerencialCommand();
            CodigoRelatorioGerencial_tb.Text = ((ConfiguracoesViewModel)DataContext).RelatorioGerencialTemp.RelatorioId.ToString();
            DescricaoRelatorioGerencial_tb.Text = ((ConfiguracoesViewModel)DataContext).RelatorioGerencialTemp.NomeArquivoRpt;

        }

        private void btnAdicionarRelatorioGerencial_Click(object sender, RoutedEventArgs e)
        {
            DescricaoRelatorioGerencial_tb.Focus();
            BuscarRelatorioGerencial_bt.IsEnabled = true;

            btnSalvarRelatorioGerencial.IsEnabled = true;
            btnCancelarRelatorioGerencial.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarRelatorioGerencialCommand();

        }

        private void btnSalvarRelatorioGerencial_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarRelatorioGerencial.IsEnabled = true;
            btnSalvarRelatorioGerencial.IsEnabled = false;
            BuscarRelatorioGerencial_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarRelatorioGerencialCommand();
        }

        private void btnDeletarRelatorioGerencial_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarRelatorioGerencial.IsEnabled = true;
            btnSalvarRelatorioGerencial.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirRelatorioGerencialCommand();
        }

        private void btnCancelarRelatorioGerencial_Click(object sender, RoutedEventArgs e)
        {
            btnSalvarRelatorioGerencial.IsEnabled = false;
            btnAdicionarRelatorioGerencial.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoRelatoriosGerenciais();
        }

        #endregion

        #region Layouts Crachás

        private void AbrirCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAbrirLayoutCrachaCommand();
        }

        private void BuscarCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnBuscarLayoutCrachaCommand();

            CodigoCracha_tb.Text = ((ConfiguracoesViewModel)DataContext).LayoutCrachaTemp.LayoutCrachaId.ToString();
            Nome_tb.Text = ((ConfiguracoesViewModel)DataContext).LayoutCrachaTemp.Nome;
        }

        private void btnAdicionarCracha_Click(object sender, RoutedEventArgs e)
        {
            Nome_tb.Focus();
            BuscarCracha_bt.IsEnabled = true;
            btnSalvarCracha.IsEnabled = true;
            btnCancelarCracha.IsEnabled = true;
            btnAdicionarCracha.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarLayoutCrachaCommand();
        }

        private void btnSalvarCracha_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarCracha.IsEnabled = true;
            btnSalvarCracha.IsEnabled = false;
            BuscarCracha_bt.IsEnabled = false;
            btnCancelarCracha.IsEnabled = false;

            BuscarRelatorioGerencial_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarLayoutCrachaCommand();
        }

        private void btnDeletarCracha_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarCracha.IsEnabled = true;
            btnSalvarCracha.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirLayoutCrachaCommand();
        }

        private void btnCancelarCracha_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarCracha.IsEnabled = true;
            btnSalvarCracha.IsEnabled = false;
            BuscarCracha_bt.IsEnabled = false;
            btnCancelarCracha.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoLayoutsCrachas();
        }

        #endregion

        #region Tipos Equipamentos

        private void btnAdicionarEquipamento_Click(object sender, RoutedEventArgs e)
        {
            DescricaoEquipamento_tb.Focus();
            btnSalvarEquipamento.IsEnabled = true;
            btnCancelarEquipamento.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposEquipamentos();
        }

        private void btnSalvarEquipamento_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarEquipamento.IsEnabled = true;
            btnSalvarEquipamento.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposEquipamentos();
        }

        private void btnDeletarEquipamento_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarEquipamento.IsEnabled = true;
            btnSalvarEquipamento.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposEquipamentos();
        }

        private void btnCancelarEquipamento_Click(object sender, RoutedEventArgs e)
        {
            btnSalvarEquipamento.IsEnabled = false;
            btnAdicionarEquipamento.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoTiposEquipamentos();
        }

        #endregion

        #region Tipos Acessos

        private void btnAdicionarTiposAcesso_Click(object sender, RoutedEventArgs e)
        {
            DescricaoAcessos_tb.Focus();
            btnSalvarTiposAcesso.IsEnabled = true;
            btnCancelarTiposAcesso.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposAcesso();
        }
        private void btnSalvarTiposAcesso_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposAcesso.IsEnabled = true;
            btnSalvarTiposAcesso.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposAcesso();
        }

        private void btnDeletarTiposAcesso_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposAcesso.IsEnabled = true;
            btnSalvarTiposAcesso.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposAcesso();
        }

        private void btnCancelarTiposAcesso_Click(object sender, RoutedEventArgs e)
        {
            btnSalvarTiposAcesso.IsEnabled = false;
            btnAdicionarTiposAcesso.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoTiposAcessos();
        }

        #endregion

        #region Tipos Areas Acessos

        private void NovoTiposAreasAcessos_bt_Click(object sender, RoutedEventArgs e)
        {
            IdentificacaoAreasAcessos_tb.Focus();



            btnSalvarTiposAreasAcessos.IsEnabled = true;
            btnCancelarTiposAreasAcessos.IsEnabled = true;

            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_AreaAcesso();





        }
        private void SalvarTiposAreasAcessos_bt_Click(object sender, RoutedEventArgs e)
        {
            if (IdentificacaoAreasAcessos_tb.Text.Length <= 0)
            {
                MessageBox.Show("A(s) letra(s) de identificação da área de acesso é obrigatória.");
                IdentificacaoAreasAcessos_tb.Focus();
                return;
            }
            btnSalvarTiposAreasAcessos.IsEnabled = false;
            btnCancelarTiposAreasAcessos.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_AreaAcesso();
        }
        private void DeletarTiposAreasAcessos_bt_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposAreasAcessos.IsEnabled = true;
            btnSalvarTiposAreasAcessos.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_AreaAcesso();
        }
        private void CancelarTiposAreasAcessos_bt_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposAreasAcessos.IsEnabled = true;
            btnSalvarTiposAreasAcessos.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoAreasAcessos();
        }


        #endregion

        #region Tipos Atividades

        private void NovoTipoAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoAtividades_tb.Focus();
            btnSalvarTipoAtividade.IsEnabled = true;
            btnCancelarTipoAtividade.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposAtividades();
        }
        private void SalvarTipoAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTipoAtividade.IsEnabled = true;
            btnSalvarTipoAtividade.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposAtividades();
        }
        private void DeletarTipoAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTipoAtividade.IsEnabled = true;
            btnSalvarTipoAtividade.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposAtividades();
        }
        private void CancelarTipoAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            btnSalvarTipoAtividade.IsEnabled = false;
            btnAdicionarTipoAtividade.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoTiposAtividades();
        }

        #endregion

        #region Tipos Serviços


        private void btnAdicionarTipoServico_Click(object sender, RoutedEventArgs e)
        {
            DescricaoTipoServico_tb.Focus();
            btnSalvarTipoServico.IsEnabled = true;
            btnCancelarTipoServico.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TipoServico();
        }

        private void btnSalvarTipoServico_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTipoServico.IsEnabled = true;
            btnSalvarTipoServico.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TipoServico();
        }

        private void btnDeletarTipoServico_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTipoServico.IsEnabled = true;
            btnSalvarTipoServico.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TipoServico();
        }

        private void btnCancelarTipoServico_Click(object sender, RoutedEventArgs e)
        {
            btnSalvarTipoAtividade.IsEnabled = false;
            btnAdicionarTipoAtividade.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoTipoServico();
        }

        #endregion

        #region Tecnologias Credenciais

        private void btnAdicionarTecnologiasCredenciais_Click(object sender, RoutedEventArgs e)
        {
            DescricaoTecnologiasCredenciais_tb.Focus();
            btnSalvarTecnologiasCredenciais.IsEnabled = true;
            btnCancelarTecnologiasCredenciais.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TecnologiasCredenciais();
        }

        private void btnSalvarTecnologiasCredenciais_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTecnologiasCredenciais.IsEnabled = true;
            btnSalvarTecnologiasCredenciais.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TecnologiasCredenciais();
        }

        private void btnDeletarTecnologiasCredenciais_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTecnologiasCredenciais.IsEnabled = true;
            btnSalvarTecnologiasCredenciais.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TecnologiasCredenciais();
        }

        private void btnCancelarTecnologiasCredenciais_Click(object sender, RoutedEventArgs e)
        {
            btnSalvarTecnologiasCredenciais.IsEnabled = false;
            btnAdicionarTecnologiasCredenciais.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoTecnologiasCredenciais();
        }

        #endregion

        #region Tipos Cobranças

        private void btnAdicionarTiposCobranca_Click(object sender, RoutedEventArgs e)
        {
            DescricaoCobrancas_tb.Focus();
            btnSalvarTiposCobranca.IsEnabled = true;
            btnCancelarTiposCobranca.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposCobrancas();
        }

        private void btnSalvarTiposCobranca_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposCobranca.IsEnabled = true;
            btnSalvarTiposCobranca.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposCobrancas();
        }

        private void btnDeletarTiposCobranca_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposCobranca.IsEnabled = true;
            btnSalvarTiposCobranca.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposCobrancas();
        }

        private void btnCancelarTiposCobranca_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposCobranca.IsEnabled = true;
            btnSalvarTiposCobranca.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoTiposCobrancas();
        }


        #endregion

        #region Cursos 

        private void btnAdicionarTiposCursos_Click(object sender, RoutedEventArgs e)
        {
            DescricaoCursos_tb.Focus();
            btnSalvarTiposCursos.IsEnabled = true;
            btnCancelarTiposCursos.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposCursos();
        }

        private void btnSalvarTiposCursos_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposCursos.IsEnabled = true;
            btnSalvarTiposCursos.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposCursos();
        }

        private void btnDeletarTiposCursos_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposCursos.IsEnabled = true;
            btnSalvarTiposCursos.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposCursos();
        }

        private void btnCancelarTiposCursos_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposCursos.IsEnabled = true;
            btnSalvarTiposCursos.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoCursos();
        }
        #endregion

        #region Tipos Combustíveis

        private void btnAdicionarTiposCombustiveis_Click(object sender, RoutedEventArgs e)
        {
            DescricaoTiposCombustiveis_tb.Focus();
            btnSalvarTiposCombustiveis.IsEnabled = true;
            btnCancelarTiposCombustiveis.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposCombustiveis();
        }

        private void btnSalvarTiposCombustiveis_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposCombustiveis.IsEnabled = true;
            btnSalvarTiposCombustiveis.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposCombustiveis();
        }

        private void btnDeletarTiposCombustiveis_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposCombustiveis.IsEnabled = true;
            btnSalvarTiposCombustiveis.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposCombustiveis();
        }

        private void btnCancelarTiposCombustiveis_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposCombustiveis.IsEnabled = true;
            btnSalvarTiposCombustiveis.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoTipoCombustiveis();
        }

        #endregion

        #region Tipos Status

        private void btnAdicionarTiposStatus_Click(object sender, RoutedEventArgs e)
        {
            DescricaoStatus_tb.Focus();
            btnSalvarTiposStatus.IsEnabled = true;
            btnCancelarTiposStatus.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposStatus();
        }

        private void btnSalvarTiposStatus_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposStatus.IsEnabled = true;
            btnSalvarTiposStatus.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposStatus();
        }

        private void btnDeletarTiposStatus_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposStatus.IsEnabled = true;
            btnSalvarTiposStatus.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposStatus();
        }

        private void btnCancelarTiposStatus_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarTiposStatus.IsEnabled = true;
            btnSalvarTiposStatus.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoStatus();
        }


        #endregion

        #region Credenciais Status
        private void btnAdicionarCredencialStatus_Click(object sender, RoutedEventArgs e)
        {

            DescricaoCredencialStatus_tb.Focus();
            btnSalvarCredencialStatus.IsEnabled = true;
            btnCancelarCredencialStatus.IsEnabled = true;

            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_CredenciaisStatus();

        }

        private void btnSalvarCredencialStatus_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_CredenciaisStatus();
        }

        private void btnDeletarCredencialStatus_Click(object sender, RoutedEventArgs e)
        {

            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_CredenciaisStatus();
        }

        private void btnCancelarCredencialStatus_Click(object sender, RoutedEventArgs e)
        {

            ((ConfiguracoesViewModel)DataContext).CarregaColecaoCredenciaisStatus();
        }

        #endregion

        #region Credenciais Motivo Status

        private void NovoCredencialMotivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_CredenciaisMotivos();
        }

        private void SalvarCredencialMotivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_CredenciaisMotivos();
        }

        private void DeletarCredencialMotivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_CredenciaisMotivos();
        }


        #endregion

        #region Formatos Credenciais

        private void btnAdicionarFormatosCredenciais_Click(object sender, RoutedEventArgs e)
        {
            DescricaoFormatosCredenciais_tb.Focus();
            btnSalvarFormatosCredenciais.IsEnabled = true;
            btnCancelarFormatosCredenciais.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_FormatosCredenciais();
        }

        private void btnSalvarFormatosCredenciais_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarFormatosCredenciais.IsEnabled = true;
            btnSalvarFormatosCredenciais.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_FormatosCredenciais();
        }

        private void btnDeletarFormatosCredenciais_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarFormatosCredenciais.IsEnabled = true;
            btnSalvarFormatosCredenciais.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_FormatosCredenciais();
        }

        private void btnCancelarFormatosCredenciais_Click(object sender, RoutedEventArgs e)
        {
            btnAdicionarFormatosCredenciais.IsEnabled = true;
            btnSalvarFormatosCredenciais.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).CarregaColecaoFormatosCredenciais();
        }

        #endregion

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 200);
                if (arq == null) return;
                _viewModel.Entity.EmpresaLOGO = arq.FormatoBase64;
                var binding = BindingOperations.GetBindingExpression(Logo_im__, Image.SourceProperty);
                binding?.UpdateTarget();

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void BtnSalvarConfiguracoesSistema_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            btnAdicionarTipoAtividade.IsEnabled = true;
            btnSalvarTipoAtividade.IsEnabled = false;
            if (Senha_tb_.Password.Length > 0)
            {
                _viewModel.Entity.EmailSenha = this.Senha_tb_.Password;
            }
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_ConfiguracoesSistema();

            _viewModel.CarregaConfiguracaoSistema();
            var config = _auxiliaresServiceConfiguraSistema.ConfiguraSistemaService.Listar().FirstOrDefault();
            Group_cb_.Text = config.GrupoPadrao.Trim();

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            MessageBox.Show("Alteração realizada com sucesso!", "Credenciamento", MessageBoxButton.OK);
        }



        private void Colaborador_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                IColaboradorService _serviceColaborador = new Application.Service.ColaboradorService();

                var forderDialog = new System.Windows.Forms.FolderBrowserDialog();
                var result = forderDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@forderDialog.SelectedPath);
                    System.IO.FileInfo[] rgFiles = di.GetFiles("*.jpg");

                    foreach (System.IO.FileInfo fi in rgFiles)
                    {
                        var id = fi.Name.Split('.');
                        var path = fi.FullName;
                        var tamBytes = new System.IO.FileInfo(path).Length;
                        var tam = decimal.Divide(tamBytes, 1024);
                        var arq = new ArquivoInfo
                        {
                            Nome = fi.FullName
                        };


                        arq.ArrayBytes = File.ReadAllBytes(path);
                        arq.FormatoBase64 = Convert.ToBase64String(arq.ArrayBytes);
                        arq.ArrayBytes = File.ReadAllBytes(path);
                        arq.FormatoBase64 = Convert.ToBase64String(arq.ArrayBytes);

                        try
                        {

                            if (arq != null)
                            {
                                var n2 = _serviceColaborador.Listar(null, null, null, null, id[0]).FirstOrDefault();
                                if (n2 != null)
                                {
                                    n2.Foto = arq.FormatoBase64;
                                    _serviceColaborador.Alterar(n2);
                                }

                            }

                        }

                        catch (Exception ex)
                        {
                            //System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                            throw ex;
                        }
                    }

                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("Importação concluida!", "Credenciamento", MessageBoxButton.OK);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Emprasa_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                IEmpresaService _serviceEmpresa = new Application.Service.EmpresaService();

                var forderDialog = new System.Windows.Forms.FolderBrowserDialog();
                var result = forderDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@forderDialog.SelectedPath);
                    System.IO.FileInfo[] rgFiles = di.GetFiles("*.jpg");

                    foreach (System.IO.FileInfo fi in rgFiles)
                    {
                        var id = fi.Name.Split('.');
                        var path = fi.FullName;
                        var tamBytes = new System.IO.FileInfo(path).Length;
                        var tam = decimal.Divide(tamBytes, 1024);
                        var arq = new ArquivoInfo
                        {
                            Nome = fi.FullName
                        };

                        arq.ArrayBytes = File.ReadAllBytes(path);
                        arq.FormatoBase64 = Convert.ToBase64String(arq.ArrayBytes);
                        arq.ArrayBytes = File.ReadAllBytes(path);
                        arq.FormatoBase64 = Convert.ToBase64String(arq.ArrayBytes);

                        try
                        {

                            if (arq != null)
                            {
                                var n2 = _serviceEmpresa.Listar(null, null, null, null, null, null, null, id[0]).FirstOrDefault();
                                if (n2 != null)
                                {
                                    n2.Logo = arq.FormatoBase64;
                                    _serviceEmpresa.Alterar(n2);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            //System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                            throw ex;
                        }
                    }

                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("Importação concluida!", "Credenciamento", MessageBoxButton.OK);
                }

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw ex;
            }
        }

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                var rpt = (LayoutCrachaView)ListaLayoutsCrachas_lv.SelectedValue;
                _viewModel.VisualizarCrach(rpt);

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                Utils.TraceException(ex);
                throw ex;
            }
        }

        private void BntContencao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                ((ConfiguracoesViewModel)DataContext).OnExcluirRegistroLogCommand_ConfiguracoesSistema();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}









