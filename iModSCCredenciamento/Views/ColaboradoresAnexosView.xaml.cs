using System;
using System.Windows;
using System.Windows.Controls;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.ViewModels;
//using IMOD.Application.Service;

namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interação lógica para ColaboradoresAnexosView.xam
    /// </summary>
    public partial class ColaboradoresAnexosView : UserControl
    {
      
        #region Inicializacao
        public ColaboradoresAnexosView()
        {
            InitializeComponent();
            DataContext = new ColaboradoresAnexosViewModel();
        }
        #endregion

        #region Vinculo do UserControl
        static int _colaboradorIDFisrt;
        public int ColaboradorSelecionadoIDView
        {
            get { return (int)GetValue(ColaboradorSelecionadoIDViewProperty); }
            set { SetValue(ColaboradorSelecionadoIDViewProperty, value); }
        }

        public static readonly DependencyProperty ColaboradorSelecionadoIDViewProperty =
            DependencyProperty.Register("ColaboradorSelecionadoIDView", typeof(int), typeof(ColaboradoresAnexosView), new PropertyMetadata(0, PropertyChanged));
        private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            int _colaboradorID = Convert.ToInt32(e.NewValue);
            if (_colaboradorID != _colaboradorIDFisrt && _colaboradorID != 0)
            {
            ((ColaboradoresAnexosViewModel)((FrameworkElement)source).DataContext).OnAtualizaCommand(_colaboradorID);
                _colaboradorIDFisrt = _colaboradorID;
            }

        }

        public bool Editando
        {
            get { return (bool)GetValue(EditandoProperty); }
            set { SetValue(EditandoProperty, value); }
        }

        public static readonly DependencyProperty EditandoProperty =
            DependencyProperty.Register("Editando", typeof(bool), typeof(ColaboradoresAnexosView), new FrameworkPropertyMetadata(true,
                                                           FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EditandoPropertyChanged));

        private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region Comando dos Botoes
        //
        private void BuscarApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ColaboradoresAnexosViewModel)DataContext).OnBuscarArquivoCommand();
            Arquivo_tb.Text = ((ColaboradoresAnexosViewModel)DataContext).ColaboradoresAnexos[0].NomeArquivo;
        }

        private void AbrirApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ColaboradoresAnexosViewModel)DataContext).OnAbrirArquivoCommand();
        }

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ColaboradoresAnexosViewModel)DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaColaboradoresAnexos_lv.IsHitTestVisible = false;
            Global.SetReadonly(Linha0_sp, false);

            ((ColaboradoresAnexosViewModel)DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Global.SetReadonly(Linha0_sp, false);
            ((ColaboradoresAnexosViewModel)DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((ColaboradoresAnexosViewModel)DataContext).OnExcluirCommand();
        }

        private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            //Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //((EmpresasSegurosViewModel)this.DataContext).ExecutarPesquisaCommand();
        }

        private void CancelarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Pesquisar_sp.Visibility = Visibility.Hidden;

        }

        private void CancelarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaColaboradoresAnexos_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);;
            ((ColaboradoresAnexosViewModel)DataContext).OnCancelarEdicaoCommand();
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Anexo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?",2))
            {
                return;
            }

           Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((ColaboradoresAnexosViewModel)DataContext).OnSalvarEdicaoCommand();
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaColaboradoresAnexos_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);;
        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((ColaboradoresAnexosViewModel)DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);;
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

            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((ColaboradoresAnexosViewModel)DataContext).OnSalvarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);;
        }
        #endregion

        private void ListaColaboradoresAnexos_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaColaboradoresAnexos_lv.SelectedIndex == -1)
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
            ListaColaboradoresAnexos_lv.SelectedIndex = -1;
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
