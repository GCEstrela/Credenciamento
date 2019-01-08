using System;
using System.Windows;
using System.Windows.Controls;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.ViewModels;

namespace iModSCCredenciamento.Views
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
        private void SalvarEquipamento_bt_Click(object sender, RoutedEventArgs e)
        {
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }
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
        //IEngine _sdk = Main.engine;
        //private void Carregar_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Schedule agenda = _sdk.GetEntity(new Guid("00000000-0000-0000-0000-000000000006")) as Schedule;
        //        SystemConfiguration systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
        //        var service = systemConfiguration.CustomFieldService;
        //        string instancia = service.GetValue<string>("Instancia", agenda.Guid);
        //        string bancoDados = service.GetValue<string>("Banco de Dados", agenda.Guid);
        //        string usuario = service.GetValue<string>("Usuario", agenda.Guid);
        //        string senha = service.GetValue<string>("Senha", agenda.Guid);

        //        Global._connectionString = "Data Source=" + instancia + "; Initial Catalog=" + bancoDados + "; User ID=" + usuario + "; Password=" + senha +
        //            "; Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";
        //    }
        //    catch
        //    {

        //    }
        //}
        private void BuscarRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnBuscarRelatorioCommand();
            CodigoRelatorio_tb.Text = ((ConfiguracoesViewModel)DataContext).Relatorios[0].RelatorioID.ToString();
            DescricaoRelatorio_tb.Text = ((ConfiguracoesViewModel)DataContext).Relatorios[0].NomeArquivoRPT;
        }
        private void SalvarRelatorio_bt_Click(object sender, RoutedEventArgs e)
        {

            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }
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
        private void BuscarRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnBuscarRelatorioGerencialCommand();
            CodigoRelatorioGerencial_tb.Text = ((ConfiguracoesViewModel)DataContext).RelatoriosGerenciais[0].RelatorioID.ToString();
            DescricaoRelatorioGerencial_tb.Text = ((ConfiguracoesViewModel)DataContext).RelatoriosGerenciais[0].NomeArquivoRPT;
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
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }
            ((ConfiguracoesViewModel)DataContext).OnSalvarRelatorioGerencialCommand();
        }
        private void ExcluirRelatorioGerencial_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirRelatorioGerencialCommand();
        }
        private void NovoCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            BuscarCracha_bt.IsEnabled = true;
            ((ConfiguracoesViewModel)DataContext).OnAdicionarLayoutCrachaCommand();
        }
        private void SalvarCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }
            ((ConfiguracoesViewModel)DataContext).OnSalvarLayoutCrachaCommand();
        }
        private void ExcluirCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnExcluirLayoutCrachaCommand();
        }
        private void BuscarCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnBuscarLayoutCrachaCommand();
            Nome_tb.Text = ((ConfiguracoesViewModel)DataContext).LayoutsCrachas[0].Nome;
        }
        private void AbrirCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ConfiguracoesViewModel)DataContext).OnAbrirLayoutCrachaCommand();
        }

        #endregion
    }
}
