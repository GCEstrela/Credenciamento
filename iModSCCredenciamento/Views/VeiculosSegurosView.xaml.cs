using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.ViewModels;
using IMOD.CrossCutting;
using iModSCCredenciamento.Helpers;

namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interação lógica para VeiculosSegurosView.xam
    /// </summary>
    public partial class VeiculosSegurosView : UserControl
    {
        private readonly VeiculosSegurosViewModel _viewModel;

        #region Inicializacao
        public VeiculosSegurosView()
        {
            InitializeComponent();
            _viewModel = new VeiculosSegurosViewModel();
            DataContext = _viewModel;
        }
        #endregion

        #region Metodos
        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.VeiculoView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados(entity);
        }


        /// <summary>
        ///     UpLoad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _viewModel.Entity.Arquivo = arq.FormatoBase64;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///     Downlaod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAbrirArquivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var arquivoStr = _viewModel.Entity.Arquivo;
                Global.PopupPDF(arquivoStr);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void FormatCurrency(object sender, RoutedEventArgs e)
        {
            txtValorCobertura.Text = txtValorCobertura.Text.FormatarMoeda();
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion

    }

}

//#region Vinculo do UserControl
//static int __EquipamentoVeiculoIDFisrt;
//public int VeiculoSelecionadaIDView
//{
//    get { return (int)GetValue(VeiculoSelecionadaIDViewProperty); }
//    set { SetValue(VeiculoSelecionadaIDViewProperty, value); }
//}

//public static readonly DependencyProperty VeiculoSelecionadaIDViewProperty =
//    DependencyProperty.Register("VeiculoSelecionadaIDView", typeof(int), typeof(VeiculosSegurosView), new PropertyMetadata(0, PropertyChanged));
//private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
//{
//    int _EquipamentoVeiculoID = Convert.ToInt32(e.NewValue);
//    if (_EquipamentoVeiculoID != __EquipamentoVeiculoIDFisrt && _EquipamentoVeiculoID != 0)
//    {
//        ((VeiculosSegurosViewModel)((FrameworkElement)source).DataContext).OnAtualizaCommand(_EquipamentoVeiculoID);
//        __EquipamentoVeiculoIDFisrt = _EquipamentoVeiculoID;
//    }
//}
//public bool Editando
//{
//    get { return (bool)GetValue(EditandoProperty); }
//    set { SetValue(EditandoProperty, value); }
//}

//public static readonly DependencyProperty EditandoProperty =
//    DependencyProperty.Register("Editando", typeof(bool), typeof(VeiculosSegurosView), new FrameworkPropertyMetadata(true,
//                                                   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EditandoPropertyChanged));

//private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//{
//    //throw new NotImplementedException();
//}
//#endregion

//#region Comando dos Botoes
//private void BuscarApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
//{
//    ((VeiculosSegurosViewModel)DataContext).OnBuscarArquivoCommand();
//    ApoliceArquivo_tb.Text = ((VeiculosSegurosViewModel)DataContext).Seguros[0].NomeArquivo;
//}

//private void AbrirApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
//{
//    ((VeiculosSegurosViewModel)DataContext).OnAbrirArquivoCommand();
//}

//private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
//{
//    ((VeiculosSegurosViewModel)DataContext).OnPesquisarCommand();
//}

//private void Editar_bt_Click(object sender, RoutedEventArgs e)
//{
//    Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
//    Botoes_Editar_sp.Visibility = Visibility.Visible;
//    ListaSeguros_lv.IsHitTestVisible = false;
//    Global.SetReadonly(Linha0_sp, false);
//    ((VeiculosSegurosViewModel)DataContext).OnEditarCommand();
//}

//private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
//{
//    Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
//    Botoes_Adicionar_sp.Visibility = Visibility.Visible;
//    Global.SetReadonly(Linha0_sp, false);
//    ((VeiculosSegurosViewModel)DataContext).OnAdicionarCommand();
//}

//private void Excluir_bt_Click(object sender, RoutedEventArgs e)
//{
//    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
//    ((VeiculosSegurosViewModel)DataContext).OnExcluirCommand();
//}

//private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
//{
//    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
//    //Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
//    Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
//    //((VeiculosSegurosViewModel)this.DataContext).ExecutarPesquisaCommand();
//}

//private void CancelarPesquisa_bt_Click(object sender, RoutedEventArgs e)
//{
//    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
//    Botoes_Pesquisar_sp.Visibility = Visibility.Hidden;

//}

//private void CancelarEdicao_bt_Click(object sender, RoutedEventArgs e)
//{
//    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
//    Botoes_Editar_sp.Visibility = Visibility.Hidden;
//    ListaSeguros_lv.IsHitTestVisible = true;
//    Global.SetReadonly(Linha0_sp, true);
//    ((VeiculosSegurosViewModel)DataContext).OnCancelarEdicaoCommand();
//}

//private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
//{

//    //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Seguro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//    //{
//    //    return;
//    //}
//    if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
//    {
//        return;
//    }

//    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
//    ((VeiculosSegurosViewModel)DataContext).OnSalvarEdicaoCommand();
//    Botoes_Editar_sp.Visibility = Visibility.Hidden;
//    ListaSeguros_lv.IsHitTestVisible = true;
//    Global.SetReadonly(Linha0_sp, true);
//}

//private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
//{
//    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
//    ((VeiculosSegurosViewModel)DataContext).OnCancelarAdicaoCommand();
//    Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
//    Global.SetReadonly(Linha0_sp, true);
//}

//private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
//{

//    //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Seguro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//    //{
//    //    return;
//    //}
//    if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
//    {
//        return;
//    }

//    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
//    ((VeiculosSegurosViewModel)DataContext).OnSalvarAdicaoCommand();
//    Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
//    Global.SetReadonly(Linha0_sp, true);
//}

//#endregion

//#region Metodos privados

//private void Emissao_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
//{
//    Global.CheckField(sender, false);
//}

//private void ListaSeguros_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
//{
//    if (ListaSeguros_lv.SelectedIndex == -1)
//    {
//        Linha0_sp.IsEnabled = false;
//        Editar_bt.IsEnabled = false;
//    }
//    else
//    {
//        Linha0_sp.IsEnabled = true;
//        Editar_bt.IsEnabled = true;
//        AbrirContratoArquivo_bt.IsHitTestVisible = true;
//    }
//}


//private void UserControl_Loaded(object sender, RoutedEventArgs e)
//{
//    Global.SetReadonly(Linha0_sp, true);
//    ListaSeguros_lv.SelectedIndex = -1;
//    Linha0_sp.IsEnabled = false;
//    Editar_bt.IsEnabled = false;
//}

//private void Validade_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
//{
//    Global.CheckField(sender);
//}

//private void FormatCurrency(object sender, RoutedEventArgs e)
//{
//    ValorCobertura_tb.Text = ValorCobertura_tb.Text.FormatarMoeda();
//}

//private void NumberOnly(object sender, TextCompositionEventArgs e)
//{
//    Regex regex = new Regex("[^0-9]+");
//    e.Handled = regex.IsMatch(e.Text);
//}

//#endregion
