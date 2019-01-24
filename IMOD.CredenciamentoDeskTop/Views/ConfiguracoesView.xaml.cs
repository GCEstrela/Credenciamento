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
 
            ((ConfiguracoesViewModel)DataContext).OnSalvarRelatorioCommand();

        }
        private void NovoRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {
            BuscarRelatorio_bt.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarRelatorioCommand();
        }
        private void ExcluirRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirRelatorioCommand();
        }
        private void AbrirRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {
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
            BuscarRelatorioGerencial_bt.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarRelatorioGerencialCommand();
        }
        private void SalvarRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
             
            ((ConfiguracoesViewModel)DataContext).OnSalvarRelatorioGerencialCommand();
        }
        private void ExcluirRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirRelatorioGerencialCommand();
        }

        #endregion

        #region Layouts Crachás

        private void NovoCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            BuscarCracha_bt.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarLayoutCrachaCommand();
        }
        private void SalvarCracha_bt_Click(object sender, RoutedEventArgs e)
        { 
            ((ConfiguracoesViewModel)DataContext).OnSalvarLayoutCrachaCommand();
        }
        private void ExcluirCracha_bt_Click(object sender, RoutedEventArgs e)
        {
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
             
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposEquipamentos();

        }
        private void NovoEquipamento_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposEquipamentos();
        }
        private void DeletarSalvarEquipamento_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposEquipamentos();
        }

        #endregion

        #region Tipos Acessos

        private void NovoTiposAcesso_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposAcesso();
        }
        private void SalvarTiposAcesso_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposAcesso();
        }
        private void DeletarTiposAcesso_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposAcesso();
        }

        #endregion

        #region Tipos Areas Acessos

        private void NovoTiposAreasAcessos_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_AreaAcesso();
        }
        private void SalvarTiposAreasAcessos_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_AreaAcesso();
        }
        private void DeletarTiposAreasAcessos_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_AreaAcesso();
        }

        #endregion

        #region Tipos Atividades

        private void NovoTipoAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposAtividades();
        }
        private void SalvarTipoAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposAtividades();
        }
        private void DeletarTipoAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposAtividades();
        }

        #endregion

        #region Tipos Serviços


        private void NovoTipoServico_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TipoServico();
        }

        private void SalvarTipoServico_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TipoServico();
        }

        private void DeletarTipoServico_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TipoServico();
        }

        #endregion

        #region Tecnologias Credenciais

        private void NovoTecnologiasCredenciais_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TecnologiasCredenciais();
        }

        private void SalvarTecnologiasCredenciais_bt_Click(object sender, RoutedEventArgs e)
        { 
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TecnologiasCredenciais();
        }

        private void DeletarTecnologiasCredenciais_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TecnologiasCredenciais();
        }

        #endregion

        #region Tipos Cobranças

        private void NovoTiposCobranca_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposCobrancas();
        }
        private void SalvarTiposCobranca_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposCobrancas();
        }
        private void DeletarTiposCobranca_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposCobrancas();
        }

        #endregion

        #region Cursos 

        private void NovoTiposCursos_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposCursos();
        }
        private void SalvarTiposCursos_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposCursos();
        }
        private void DeletarTiposCursos_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposCursos();
        }

        #endregion

        #region Tipos Combustíveis

        private void NovoTiposCombustiveis_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposCombustiveis();
        }

        private void SalvarTiposCombustiveis_bt_Click(object sender, RoutedEventArgs e)
        {
            
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposCombustiveis();
        }

        private void DeletarTiposCombustiveis_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposCombustiveis();
        }
        #endregion

        #region Tipos Status

        private void NovoTiposStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_TiposStatus();
        }

        private void SalvarTiposStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_TiposStatus();
        }

        private void DeletarTiposStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_TiposStatus();
        }

        #endregion

        #region Credenciais Status
        private void NovoCredencialStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_CredenciaisStatus();
        }

        private void SalvarCredencialStatus_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_CredenciaisStatus();
        }

        private void DeletarCredencialStatus_bt_Click(object sender, RoutedEventArgs e)
        {
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
            ((ConfiguracoesViewModel)DataContext).OnAdicionarCommand_FormatosCredenciais();
        }

        private void SalvarFormatoCredencial_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnSalvarEdicaoCommand_FormatosCredenciais();
        }

        private void DeletarFormatoCredencial_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirCommand_FormatosCredenciais();
        }

        #endregion

        #endregion


    }
}









