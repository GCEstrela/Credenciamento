using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using UserControl = System.Windows.Controls.UserControl;

namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interação lógica para VeiculosAnexosView.xam
    /// </summary>
    public partial class VeiculosAnexosView : UserControl
    {
        #region Inicializacao
        public VeiculosAnexosView()
        {
            InitializeComponent();
            this.DataContext = new VeiculosAnexosViewModel();
        }
        #endregion

        #region Vinculo do UserControl
        static int _veiculoIDFisrt = 0;
        public int VeiculoSelecionadoIDView
        {
            get { return (int)GetValue(VeiculoSelecionadoIDViewProperty); }
            set { SetValue(VeiculoSelecionadoIDViewProperty, value); }
        }

        public static readonly DependencyProperty VeiculoSelecionadoIDViewProperty =
            DependencyProperty.Register("VeiculoSelecionadoIDView", typeof(int), typeof(VeiculosAnexosView), new PropertyMetadata(0, PropertyChanged));
        private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            int _veiculoID = Convert.ToInt32(e.NewValue);
            if (_veiculoID != _veiculoIDFisrt && _veiculoID != 0)
            {
                ((iModSCCredenciamento.ViewModels.VeiculosAnexosViewModel)((System.Windows.FrameworkElement)source).DataContext).OnAtualizaCommand(_veiculoID);
                _veiculoIDFisrt = _veiculoID;
            }

        }

        public bool Editando
        {
            get { return (bool)GetValue(EditandoProperty); }
            set { SetValue(EditandoProperty, value); }
        }

        public static readonly DependencyProperty EditandoProperty =
            DependencyProperty.Register("Editando", typeof(bool), typeof(VeiculosAnexosView), new FrameworkPropertyMetadata(true,
                                                           FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(EditandoPropertyChanged)));

        private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region Comando dos Botoes
        //
        private void BuscarApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculosAnexosViewModel)this.DataContext).OnBuscarArquivoCommand();
            Arquivo_tb.Text = ((VeiculosAnexosViewModel)this.DataContext).VeiculosAnexos[0].NomeArquivo;
        }

        private void AbrirApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculosAnexosViewModel)this.DataContext).OnAbrirArquivoCommand();
        }

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculosAnexosViewModel)this.DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaVeiculosAnexos_lv.IsHitTestVisible = false;
            Global.SetReadonly(Linha0_sp, false);

            ((VeiculosAnexosViewModel)this.DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {

            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Global.SetReadonly(Linha0_sp, false);
            ((VeiculosAnexosViewModel)this.DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosAnexosViewModel)this.DataContext).OnExcluirCommand();
        }

        private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            //Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //((EmpresasSegurosViewModel)this.DataContext).ExecutarPesquisaCommand();
        }

        private void CancelarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Pesquisar_sp.Visibility = Visibility.Hidden;

        }

        private void CancelarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaVeiculosAnexos_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true); ;
            ((VeiculosAnexosViewModel)this.DataContext).OnCancelarEdicaoCommand();
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Anexo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }

            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosAnexosViewModel)this.DataContext).OnSalvarEdicaoCommand();
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaVeiculosAnexos_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true); ;
        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosAnexosViewModel)this.DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true); ;
        }

        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Anexo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }

            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosAnexosViewModel)this.DataContext).OnSalvarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true); ;
        }
        #endregion

        private void ListaVeiculosAnexos_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaVeiculosAnexos_lv.SelectedIndex == -1)
            {
                Linha0_sp.IsEnabled = false;
                Editar_bt.IsEnabled = false;
            }
            else
            {
                Linha0_sp.IsEnabled = true;
                Editar_bt.IsEnabled = true;
                AbrirContratoArquivo_bt.IsHitTestVisible = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Global.SetReadonly(Linha0_sp, true);
            ListaVeiculosAnexos_lv.SelectedIndex = -1;
            Linha0_sp.IsEnabled = false;
            Editar_bt.IsEnabled = false;
        }
        //private void UserControl_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    e.Handled = true;
        //}

        //private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    e.Handled = true;
        //    this.Focus();
        //}
    }
}
