using System;
using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    /// Interação lógica para ConfiguracoesView.xam
    /// </summary>
    public partial class ConfiguracoesView : UserControl
    {


        #region Inicializacao
        public ConfiguracoesView()
        {
            InitializeComponent();
            DataContext = new ConfiguracoesViewModel();
        }
        #endregion

        #region Comando dos Botoes

        #region Relatórios

        private void BuscarRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnBuscarRelatorioCommand();

            CodigoRelatorio_tb.Text = ((ConfiguracoesViewModel)DataContext).RelatorioTemp.RelatorioId.ToString();
            DescricaoRelatorio_tb.Text = ((ConfiguracoesViewModel)DataContext).RelatorioTemp.NomeArquivoRpt;
        }
        private void SalvarRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoRelatorio_bt.Content = "Novo";
            SalvarRelatorio_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarRelatorioCommand();

        }
        private void NovoRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoRelatorio_tb.Focus();
            BuscarRelatorio_bt.IsEnabled = true;
            if (NovoRelatorio_bt.Content.ToString() == "Novo")
            {
                SalvarRelatorio_bt.IsEnabled = true;
                NovoRelatorio_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarRelatorioCommand();
            }
            else if (NovoRelatorio_bt.Content.ToString() == "Cancelar")
            {
                SalvarRelatorio_bt.IsEnabled = false;
                NovoRelatorio_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoRelatorios();
            }
                
            
        }
        private void ExcluirRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarRelatorio_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirRelatorioCommand();
        }
        private void AbrirRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarRelatorio_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnAbrirRelatorioCommand();
        }

        #endregion

        #region Relatórios Gerenciais

        private void BuscarRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnBuscarRelatorioGerencialCommand();
            CodigoRelatorioGerencial_tb.Text = ((ConfiguracoesViewModel)DataContext).RelatorioGerencialTemp.RelatorioId.ToString();
            DescricaoRelatorioGerencial_tb.Text = ((ConfiguracoesViewModel)DataContext).RelatorioGerencialTemp.NomeArquivoRpt;

        }
        private void AbrirRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAbrirRelatorioGerencialCommand();
        }
        private void NovoRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoRelatorioGerencial_tb.Focus();
            BuscarRelatorioGerencial_bt.IsEnabled = true;
            if (NovoRelatorioGerencial_bt.Content.ToString() == "Novo")
            {
                SalvarRelatorioGerencial_bt.IsEnabled = true;
                NovoRelatorioGerencial_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarRelatorioGerencialCommand();
            }
            else if (NovoRelatorioGerencial_bt.Content.ToString() == "Cancelar")
            {
                SalvarRelatorioGerencial_bt.IsEnabled = false;
                NovoRelatorioGerencial_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoRelatoriosGerenciais();
            }
                
        }
        private void SalvarRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoRelatorioGerencial_bt.Content = "Novo";
            SalvarRelatorioGerencial_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarRelatorioGerencialCommand();
        }
        private void ExcluirRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarRelatorioGerencial_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirRelatorioGerencialCommand();
        }

        #endregion

        #region Layouts Crachás

        private void NovoCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            Nome_tb.Focus();
            BuscarCracha_bt.IsEnabled = true;
            if (NovoCracha_bt.Content.ToString() == "Novo")
            {
                SalvarCracha_bt.IsEnabled = true;
                NovoCracha_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarLayoutCrachaCommand();
            }
            else if (NovoCracha_bt.Content.ToString() == "Cancelar")
            {
                SalvarCracha_bt.IsEnabled = false;
                NovoCracha_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoLayoutsCrachas();
            }
                
        }
        private void SalvarCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoCracha_bt.Content = "Novo";
            SalvarCracha_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarLayoutCrachaCommand();
        }
        private void ExcluirCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarCracha_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirLayoutCrachaCommand();
        }
        private void BuscarCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnBuscarLayoutCrachaCommand();

            CodigoCracha_tb.Text = ((ConfiguracoesViewModel)DataContext).LayoutCrachaTemp.LayoutCrachaId.ToString();
            Nome_tb.Text = ((ConfiguracoesViewModel)DataContext).LayoutCrachaTemp.Nome;

        }
        private void AbrirCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAbrirLayoutCrachaCommand();
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

        private void TipoCracha_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (((System.Windows.Controls.TextBox)e.Source).Text == "") return;

                int tipo = Convert.ToInt32(((System.Windows.Controls.TextBox)e.Source).Text);
                if (tipo > 2 || tipo < 1)
                {
                    TipoCracha_tb.Text = "";
                    return;
                }
                
            }
            catch (Exception)
            {
                //return;
            }

        }
    }
}









