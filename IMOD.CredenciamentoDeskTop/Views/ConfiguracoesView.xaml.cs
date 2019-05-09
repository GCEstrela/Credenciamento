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

        private void SalvarEquipamento_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoEquipamento_bt.Content = "Novo";
            SalvarEquipamento_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposEquipamentos();

        }
        private void NovoEquipamento_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoEquipamento_tb.Focus();
            if (NovoEquipamento_bt.Content.ToString() == "Novo")
            {
                SalvarEquipamento_bt.IsEnabled = true;
                NovoEquipamento_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposEquipamentos();
            }
            else if (NovoEquipamento_bt.Content.ToString() == "Cancelar")
            {
                SalvarEquipamento_bt.IsEnabled = false;
                NovoEquipamento_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoTiposEquipamentos();
            }
            
        }
        private void DeletarSalvarEquipamento_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarEquipamento_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposEquipamentos();
        }

        #endregion

        #region Tipos Acessos

        private void NovoTiposAcesso_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoAcessos_tb.Focus();
            if (NovoTiposAcesso_bt.Content.ToString() == "Novo")
            {
                SalvarTiposAcesso_bt.IsEnabled = true;
                NovoTiposAcesso_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposAcesso();
            }
            else if (NovoTiposAcesso_bt.Content.ToString() == "Cancelar")
            {
                SalvarTiposAcesso_bt.IsEnabled = false;
                NovoTiposAcesso_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoTiposAcessos();
            }
                
        }
        private void SalvarTiposAcesso_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoTiposAcesso_bt.Content = "Novo";
            SalvarTiposAcesso_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposAcesso();
        }
        private void DeletarTiposAcesso_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarTiposAcesso_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposAcesso();
        }

        #endregion

        #region Tipos Areas Acessos

        private void NovoTiposAreasAcessos_bt_Click(object sender, RoutedEventArgs e)
        {
            IdentificacaoAreasAcessos_tb.Focus();
            if (NovoTiposAreasAcessos_bt.Content.ToString() == "Novo")
            {
                SalvarTiposAreasAcessos_bt.IsEnabled = true;
                NovoTiposAreasAcessos_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_AreaAcesso();
            }
            else if (NovoTiposAreasAcessos_bt.Content.ToString() == "Cancelar")
            {
                SalvarTiposAreasAcessos_bt.IsEnabled = false;
                NovoTiposAreasAcessos_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoAreasAcessos();
            }
           
        }
        private void SalvarTiposAreasAcessos_bt_Click(object sender, RoutedEventArgs e)
        {
            if (IdentificacaoAreasAcessos_tb.Text.Length <= 0)
            {
               
                MessageBox.Show("A(s) letra(s) de identificação da área de acesso é obrigatória.");
                IdentificacaoAreasAcessos_tb.Focus();
                return;
            }
            NovoTiposAreasAcessos_bt.Content = "Novo";
            SalvarTiposAreasAcessos_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_AreaAcesso();
        }
        private void DeletarTiposAreasAcessos_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarTiposAreasAcessos_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_AreaAcesso();
        }

        #endregion

        #region Tipos Atividades

        private void NovoTipoAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoAtividades_tb.Focus();
            if (NovoTipoAtividade_bt.Content.ToString() == "Novo")
            {
                SalvarTipoAtividade_bt.IsEnabled = true;
                NovoTipoAtividade_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposAtividades();
            }
            else if (NovoTipoAtividade_bt.Content.ToString() == "Cancelar")
            {
                SalvarTipoAtividade_bt.IsEnabled = false;
                NovoTipoAtividade_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoTiposAtividades();
            }


        }
        private void SalvarTipoAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoTipoAtividade_bt.Content = "Novo";
            SalvarTipoAtividade_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposAtividades();
        }
        private void DeletarTipoAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarTipoAtividade_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposAtividades();
        }

        #endregion

        #region Tipos Serviços


        private void NovoTipoServico_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoTipoServico_tb.Focus();
            if (NovoTipoServico_bt.Content.ToString() == "Novo")
            {
                SalvarTipoServico_bt.IsEnabled = true;
                NovoTipoServico_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TipoServico();
            }
            else if (NovoTipoServico_bt.Content.ToString() == "Cancelar")
            {
                SalvarTipoServico_bt.IsEnabled = false;
                NovoTipoServico_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoTipoServico();
            }
            
        }

        private void SalvarTipoServico_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoTipoServico_bt.Content = "Novo";
            SalvarTipoServico_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TipoServico();
        }

        private void DeletarTipoServico_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarTipoServico_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TipoServico();
        }

        #endregion

        #region Tecnologias Credenciais

        private void NovoTecnologiasCredenciais_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoTecnologiasCredenciais_tb.Focus();
            if (NovoTecnologiasCredenciais_bt.Content.ToString() == "Novo")
            {
                SalvarTecnologiasCredenciais_bt.IsEnabled = true;
                NovoTecnologiasCredenciais_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TecnologiasCredenciais();
            }
            else if (NovoTecnologiasCredenciais_bt.Content.ToString() == "Cancelar")
            {
                SalvarTecnologiasCredenciais_bt.IsEnabled = false;
                NovoTecnologiasCredenciais_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoTecnologiasCredenciais();
            }
                
        }

        private void SalvarTecnologiasCredenciais_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoTecnologiasCredenciais_bt.Content = "Novo";
            SalvarTecnologiasCredenciais_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TecnologiasCredenciais();
        }

        private void DeletarTecnologiasCredenciais_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarTecnologiasCredenciais_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TecnologiasCredenciais();
        }

        #endregion

        #region Tipos Cobranças

        private void NovoTiposCobranca_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoCobrancas_tb.Focus();
            if (NovoTiposCobranca_bt.Content.ToString() == "Novo")
            {
                SalvarTiposCobranca_bt.IsEnabled = true;
                NovoTiposCobranca_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposCobrancas();
            }
            else if (NovoTiposCobranca_bt.Content.ToString() == "Cancelar")
            {
                SalvarTiposCobranca_bt.IsEnabled = false;
                NovoTiposCobranca_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoTiposCobrancas();
            }
                
        }
        private void SalvarTiposCobranca_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoTiposCobranca_bt.Content = "Novo";
            SalvarTiposCobranca_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposCobrancas();
        }
        private void DeletarTiposCobranca_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarTiposCobranca_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposCobrancas();
        }

        #endregion

        #region Cursos 

        private void NovoTiposCursos_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoCursos_tb.Focus();
            if (NovoTiposCursos_bt.Content.ToString() == "Novo")
            {
                SalvarTiposCursos_bt.IsEnabled = true;
                NovoTiposCursos_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposCursos();
            }
            else if (NovoTiposCursos_bt.Content.ToString() == "Cancelar")
            {
                SalvarTiposCursos_bt.IsEnabled = false;
                NovoTiposCursos_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoCursos();
            }
                
        }
        private void SalvarTiposCursos_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoTiposCursos_bt.Content = "Novo";
            SalvarTiposCursos_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposCursos();
        }
        private void DeletarTiposCursos_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarTiposCursos_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposCursos();
        }

        #endregion

        #region Tipos Combustíveis

        private void NovoTiposCombustiveis_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoTiposCombustiveis_tb.Focus();
            if (NovoTiposAreasAcessosSs_bt.Content.ToString() == "Novo")
            {
                SalvarTiposAreasAcesssoSs_bt.IsEnabled = true;
                NovoTiposAreasAcessosSs_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposCombustiveis();
            }
            else if (NovoTiposAreasAcessosSs_bt.Content.ToString() == "Cancelar")
            {
                SalvarTiposAreasAcesssoSs_bt.IsEnabled = false;
                NovoTiposAreasAcessosSs_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoTipoCombustiveis();
            }
                
        }

        private void SalvarTiposCombustiveis_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoTiposAreasAcessosSs_bt.Content = "Novo";
            SalvarTiposAreasAcesssoSs_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposCombustiveis();
        }

        private void DeletarTiposCombustiveis_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarTiposAreasAcesssoSs_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposCombustiveis();
        }
        #endregion

        #region Tipos Status

        private void NovoTiposStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoStatus_tb.Focus();
            if (NovoTiposStatus_bt.Content.ToString() == "Novo")
            {
                SalvarTiposStatus_bt.IsEnabled = true;
                NovoTiposStatus_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposStatus();
            }
            else if (NovoTiposStatus_bt.Content.ToString() == "Cancelar")
            {
                SalvarTiposStatus_bt.IsEnabled = false;
                NovoTiposStatus_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoStatus();
            }
            
        }

        private void SalvarTiposStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoTiposStatus_bt.Content = "Novo";
            SalvarTiposStatus_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposStatus();
        }

        private void DeletarTiposStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarTiposStatus_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposStatus();
        }

        #endregion

        #region Credenciais Status
        private void NovoCredencialStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoCredencialStatus_tb.Focus();
            if (NovoCredencialStatus_bt.Content.ToString() == "Novo")
            {
                SalvarCredencialStatus_bt.IsEnabled = true;
                NovoCredencialStatus_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_CredenciaisStatus();
            }
            else if (NovoCredencialStatus_bt.Content.ToString() == "Cancelar")
            {
                SalvarCredencialStatus_bt.IsEnabled = false;
                NovoCredencialStatus_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoCredenciaisStatus();
            }
                
        }

        private void SalvarCredencialStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoCredencialStatus_bt.Content = "Novo";
            SalvarCredencialStatus_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_CredenciaisStatus();
        }

        private void DeletarCredencialStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarCredencialStatus_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_CredenciaisStatus();
        }

        #endregion

        #region Credenciais Status

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

        private void NovoFormatoCredencial_bt_Click(object sender, RoutedEventArgs e)
        {
            DescricaoFormatosCredenciais_tb.Focus();
            if (NovoFormatosCredenciais_bt.Content.ToString() == "Novo")
            {
                SalvarFormatosCredenciais_bt.IsEnabled = true;
                NovoFormatosCredenciais_bt.Content = "Cancelar";
                ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_FormatosCredenciais();
            }
            else if (NovoFormatosCredenciais_bt.Content.ToString() == "Cancelar")
            {
                SalvarFormatosCredenciais_bt.IsEnabled = false;
                NovoFormatosCredenciais_bt.Content = "Novo";
                ((ConfiguracoesViewModel)DataContext).CarregaColecaoFormatosCredenciais();
            }
                
        }

        private void SalvarFormatoCredencial_bt_Click(object sender, RoutedEventArgs e)
        {
            NovoFormatosCredenciais_bt.Content = "Novo";
            SalvarFormatosCredenciais_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_FormatosCredenciais();
        }

        private void DeletarFormatoCredencial_bt_Click(object sender, RoutedEventArgs e)
        {
            SalvarFormatosCredenciais_bt.IsEnabled = false;
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_FormatosCredenciais();
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









