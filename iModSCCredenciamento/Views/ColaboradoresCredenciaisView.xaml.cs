// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System.Windows.Controls;
using iModSCCredenciamento.ViewModels;

#endregion

namespace iModSCCredenciamento.Views
{
    /// <summary>
    ///     Interação lógica para ColaboradorCredencialView.xam
    /// </summary>
    public partial class ColaboradoresCredenciaisView : UserControl
    {
        private readonly ColaboradoresCredenciaisViewModel _viewModel;
        #region Inicializacao

        public ColaboradoresCredenciaisView()
        {
            InitializeComponent();
            _viewModel = new ColaboradoresCredenciaisViewModel();
            DataContext = _viewModel;
            //DataContext = new ColaboradoresCredenciaisViewModel();
            //ImprimirCredencial_bt.IsHitTestVisible = true;
        }

        #endregion
      

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.ColaboradorView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados(entity);
        }
        //private void Validade_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)

        //#region Vinculo do UserControl
        //static int _colaboradorIDFisrt;
        //public int ColaboradorSelecionadoIDView
        //{
        //    get { return (int)GetValue(ColaboradorSelecionadoIDViewProperty); }
        //    set { SetValue(ColaboradorSelecionadoIDViewProperty, value); }
        //}

        //public static readonly DependencyProperty ColaboradorSelecionadoIDViewProperty =
        //    DependencyProperty.Register("ColaboradorSelecionadoIDView", typeof(int), typeof(ColaboradoresCredenciaisView), new PropertyMetadata(0, PropertyChanged));
        //private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        //{
        //    int _colaboradorID = Convert.ToInt32(e.NewValue);
        //    if (_colaboradorID != _colaboradorIDFisrt && _colaboradorID != 0)
        //    {
        //        ((ColaboradoresCredenciaisViewModel)((FrameworkElement)source).DataContext).OnAtualizaCommand(_colaboradorID);
        //        _colaboradorIDFisrt = _colaboradorID;
        //    }
        //}

        //public bool Editando
        //{
        //    get { return (bool)GetValue(EditandoProperty); }
        //    set { SetValue(EditandoProperty, value); }
        //}

        //public static readonly DependencyProperty EditandoProperty =
        //    DependencyProperty.Register("Editando", typeof(bool), typeof(ColaboradoresCredenciaisView), new FrameworkPropertyMetadata(true,
        //                                                   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EditandoPropertyChanged));

        //private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        //#endregion

        #region Comando dos Botoes

        //private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    ((ColaboradoresCredenciaisViewModel)DataContext).OnPesquisarCommand();
        //}

        //private void BuscarValidade_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    ((ColaboradoresCredenciaisViewModel)DataContext).OnBuscarDataCommand();
        //    ListaColaboradoresCredenciais_lv.Items.Refresh();
        //}

        //private void ImprimirCredencial_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //string _credencialInfo = ModeloCredencial_tb.SelectedValue.ToString() + (char)(20) + FC_tb.Text +
        //        //    (char)(20) + NumeroCredencial_tb.Text + (char)(20) + FormatoCredencial_cb.Text + (char)(20) + Validade_tb.Text;

        //        ((ColaboradoresCredenciaisViewModel)DataContext).OnImprimirCommand();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private void Editar_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    EmpresaVinculo_cb.IsEnabled = false;
        //    Editando = false;
        //    Botoes_Principais_sp.Visibility = Visibility.Hidden;
        //    Botoes_Editar_sp.Visibility = Visibility.Visible;
        //    ListaColaboradoresCredenciais_lv.IsHitTestVisible = false;
        //    Global.SetReadonly(Linha0_sp, false);
        //    ((ColaboradoresCredenciaisViewModel)DataContext).OnEditarCommand();

        //}

        //private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    //Inserido pois não está funcionando o SelectedIndex=0!
        //    Linha0_sp.IsEnabled = true;
        //    Editar_bt.IsEnabled = true;
        //    ///

        //    Editando = false;
        //    Botoes_Principais_sp.Visibility = Visibility.Hidden;
        //    Botoes_Adicionar_sp.Visibility = Visibility.Visible;
        //    Global.SetReadonly(Linha0_sp, false);
        //    ((ColaboradoresCredenciaisViewModel)DataContext).OnAdicionarCommand();

        //}

        //private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    Editando = true;
        //    Botoes_Principais_sp.Visibility = Visibility.Visible;
        //    ((ColaboradoresCredenciaisViewModel)DataContext).OnExcluirCommand();
        //}

        //private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
        //    //Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
        //    Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
        //    //((EmpresasSegurosViewModel)this.DataContext).ExecutarPesquisaCommand();
        //}

        //private void CancelarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
        //    Botoes_Pesquisar_sp.Visibility = Visibility.Hidden;

        //}

        //private void CancelarEdicao_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    EmpresaVinculo_cb.IsEnabled = true;
        //    Editando = true;
        //    EmpresaVinculo_cb.IsEnabled = true;
        //    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
        //    Botoes_Editar_sp.Visibility = Visibility.Hidden;
        //    ListaColaboradoresCredenciais_lv.IsHitTestVisible = true;
        //    Global.SetReadonly(Linha0_sp, true);
        //    ImprimirCredencial_bt.IsHitTestVisible = true;
        //    ((ColaboradoresCredenciaisViewModel)DataContext).OnCancelarEdicaoCommand();

        //}

        //private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        //{

        //    //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Vínculo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //    //{
        //    //    return;
        //    //}
        //    if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
        //    {
        //        return;
        //    }
        //    EmpresaVinculo_cb.IsEnabled = true;
        //    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
        //    ((ColaboradoresCredenciaisViewModel)DataContext).OnSalvarAdicaoCommand();
        //    Botoes_Editar_sp.Visibility = Visibility.Hidden;
        //    ListaColaboradoresCredenciais_lv.IsHitTestVisible = true;
        //    Global.SetReadonly(Linha0_sp, true);
        //    ImprimirCredencial_bt.IsHitTestVisible = true;
        //    Editando = true;
        //    EmpresaVinculo_cb.IsEnabled = true;
        //}

        //private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    Editando = true;
        //    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
        //    ((ColaboradoresCredenciaisViewModel)DataContext).OnCancelarAdicaoCommand();
        //    Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
        //    Global.SetReadonly(Linha0_sp, true);
        //    ImprimirCredencial_bt.IsHitTestVisible = true;

        //}

        //private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        //{

        //    //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Vínculo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //    //{
        //    //    return;
        //    //}
        //    if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
        //    {
        //        return;
        //    }
        //    Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
        //    ((ColaboradoresCredenciaisViewModel)DataContext).OnSalvarAdicaoCommand();
        //    Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
        //    Global.SetReadonly(Linha0_sp, true);
        //    ImprimirCredencial_bt.IsHitTestVisible = true;
        //    Editando = true;

        //}

        #endregion

        //{
        //    Global.CheckField(sender);
        //}

        //private void ListaColaboradoresCredenciais_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (ListaColaboradoresCredenciais_lv.SelectedIndex == -1)
        //    {
        //        //Retirado pois não está funcionando o SelectedIndex=0!
        //        //Linha0_sp.IsEnabled = false;
        //        //Editar_bt.IsEnabled = false;
        //    }
        //    else
        //    {
        //        Linha0_sp.IsEnabled = true;
        //        Editar_bt.IsEnabled = true;
        //        ImprimirCredencial_bt.IsHitTestVisible = true;
        //    }
        //}

        //private void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{

        //    if (Editando)
        //    {
        //        Global.SetReadonly(Linha0_sp, true);
        //        ListaColaboradoresCredenciais_lv.SelectedIndex = -1;
        //        Linha0_sp.IsEnabled = false;
        //        Editar_bt.IsEnabled = false;
        //        ImprimirCredencial_bt.IsHitTestVisible = true;

        //        if (_colaboradorIDFisrt != 0)
        //        {
        //            ((ColaboradoresCredenciaisViewModel)DataContext).OnAtualizaCommand(_colaboradorIDFisrt);
        //        }
        //    }
        //}

        //private void StatusCredencial_tb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (e.AddedItems.Count > 0)
        //    {
        //        if (((ClasseCredenciaisStatus.CredencialStatus)((object[])e.AddedItems)[0]).CredencialStatusID == 1)
        //        {
        //            Ativa_tw.IsChecked = true;
        //            ((ColaboradoresCredenciaisViewModel)DataContext).CarregaColecaoCredenciaisMotivos(1);
        //        }
        //        else
        //        {
        //            Ativa_tw.IsChecked = false;
        //            ((ColaboradoresCredenciaisViewModel)DataContext).CarregaColecaoCredenciaisMotivos(2);
        //        }
        //    }

        //}
    }
}