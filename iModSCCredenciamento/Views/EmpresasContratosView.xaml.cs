using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.ViewModels;

namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interação lógica para EmpresasContratosView.xam
    /// </summary>
    public partial class EmpresasContratosView : UserControl
    {
        #region Inicializacao
        public EmpresasContratosView()
        {
            InitializeComponent();
            DataContext = new EmpresasContratosViewModel();
            Global.SetReadonly(Linha0_sp, true);
            //this.PreviewKeyDown += (ss, ee) =>
            //{
            //    if (ee.Key == Key.Escape)
            //    {
            //        //System.Windows.Forms.MessageBox.Show(ee.Key.ToString());
            //        Global._escape = true;
            //    }
            //};
        }
        #endregion

        #region Vinculo do UserControl
        static int _empresaIDFisrt;
        public int EmpresaSelecionadaIDView
        {
            get { return (int)GetValue(EmpresaSelecionadaIDViewProperty); }
            set { SetValue(EmpresaSelecionadaIDViewProperty, value); }
        }

        public static readonly DependencyProperty EmpresaSelecionadaIDViewProperty =
            DependencyProperty.Register("EmpresaSelecionadaIDView", typeof(int), typeof(EmpresasContratosView), new PropertyMetadata(0, PropertyChanged));
        private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            int _empresaID = Convert.ToInt32(e.NewValue);
            if (_empresaID != _empresaIDFisrt && _empresaID != 0)
            {
                ((EmpresasContratosViewModel)((FrameworkElement)source).DataContext).OnAtualizaCommand(_empresaID);
                _empresaIDFisrt = _empresaID;
            }
        }

        public bool Editando
        {
            get { return (bool)GetValue(EditandoProperty); }
            set { SetValue(EditandoProperty, value); }
        }

        public static readonly DependencyProperty EditandoProperty =
            DependencyProperty.Register("Editando", typeof(bool), typeof(EmpresasContratosView), new FrameworkPropertyMetadata(true,
                                                           FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EditandoPropertyChanged));

        private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region Comando dos Botoes
        private void BuscarContratoArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresasContratosViewModel)DataContext).OnBuscarArquivoCommand();
            NomeArquivo_tb.Text = ((EmpresasContratosViewModel)DataContext).Contratos[0].NomeArquivo;
        }

        private void AbrirContratoArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresasContratosViewModel)DataContext).OnAbrirArquivoCommand();
        }

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresasContratosViewModel)DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaContratos_lv.IsHitTestVisible = false;
            Global.SetReadonly(Linha0_sp, false);
            ((EmpresasContratosViewModel)DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Global.SetReadonly(Linha0_sp, false);
            ((EmpresasContratosViewModel)DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasContratosViewModel)DataContext).OnExcluirCommand();
        }

        private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            //Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //((EmpresasContratosViewModel)this.DataContext).ExecutarPesquisaCommand();
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
            ListaContratos_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
            ((EmpresasContratosViewModel)DataContext).OnCancelarEdicaoCommand();
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Contrato", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }

            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasContratosViewModel)DataContext).OnSalvarEdicaoCommand();
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaContratos_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasContratosViewModel)DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
        }

        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Contrato", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }

            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasContratosViewModel)DataContext).OnSalvarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
        }



        #endregion

        private void Emissao_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender, false);
        }

        private void Validade_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender);
        }

        private void ListaContratos_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaContratos_lv.SelectedIndex == -1)
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
            ListaContratos_lv.SelectedIndex = -1;
            Linha0_sp.IsEnabled = false;
            Editar_bt.IsEnabled = false;
        }
    }
}
