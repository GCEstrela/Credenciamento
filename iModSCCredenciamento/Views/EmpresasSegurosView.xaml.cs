using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.ViewModels;
using IMOD.CrossCutting;

namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interaction logic for ContratosEmpresaView.xaml
    /// </summary>
    public partial class EmpresasSegurosView : UserControl
    {

        #region Inicializacao
        public EmpresasSegurosView()
        {
            InitializeComponent();
            DataContext = new EmpresasSegurosViewModel();
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
            DependencyProperty.Register("EmpresaSelecionadaIDView", typeof(int), typeof(EmpresasSegurosView), new PropertyMetadata(0, PropertyChanged));
        private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            int _empresaID = Convert.ToInt32(e.NewValue);
            if (_empresaID != _empresaIDFisrt && _empresaID != 0)
            {
                ((EmpresasSegurosViewModel)((FrameworkElement)source).DataContext).OnAtualizaCommand(_empresaID);
                _empresaIDFisrt = _empresaID;
            }
        }
        public bool Editando
        {
            get { return (bool)GetValue(EditandoProperty); }
            set { SetValue(EditandoProperty, value); }
        }

        public static readonly DependencyProperty EditandoProperty =
            DependencyProperty.Register("Editando", typeof(bool), typeof(EmpresasSegurosView), new FrameworkPropertyMetadata(true,
                                                           FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EditandoPropertyChanged));

        private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region Comando dos Botoes
        private void BuscarApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresasSegurosViewModel)DataContext).OnBuscarArquivoCommand();
            ApoliceArquivo_tb.Text = ((EmpresasSegurosViewModel)DataContext).Seguros[0].NomeArquivo;
        }

        private void AbrirApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresasSegurosViewModel)DataContext).OnAbrirArquivoCommand();
        }

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresasSegurosViewModel)DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaSeguros_lv.IsHitTestVisible = false;
            Global.SetReadonly(Linha0_sp, false);
            ((EmpresasSegurosViewModel)DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Global.SetReadonly(Linha0_sp, false);
            ((EmpresasSegurosViewModel)DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasSegurosViewModel)DataContext).OnExcluirCommand();
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
            ListaSeguros_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
            ((EmpresasSegurosViewModel)DataContext).OnCancelarEdicaoCommand();
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Seguro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }

            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasSegurosViewModel)DataContext).OnSalvarEdicaoCommand();
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaSeguros_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasSegurosViewModel)DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
        }

        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Seguro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }

            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasSegurosViewModel)DataContext).OnSalvarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
        }

        #endregion

        private void Emissao_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender, false);
        }

        private void ListaSeguros_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaSeguros_lv.SelectedIndex == -1)
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
            ListaSeguros_lv.SelectedIndex = -1;
            Linha0_sp.IsEnabled = false;
            Editar_bt.IsEnabled = false;
        }

        private void Validade_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender);
        }

        private void FormatCurrency(object sender, RoutedEventArgs e)
        {
            ValorCobertura_tb.Text = ValorCobertura_tb.Text.FormatarCpf();
        }


    }
}
