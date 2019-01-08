using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interação lógica para VeiculosEmpresasView.xam
    /// </summary>
    public partial class VeiculosEmpresasView : UserControl
    {
        #region Inicializacao
        public VeiculosEmpresasView()
        {
            InitializeComponent();
            this.DataContext = new VeiculosEmpresasViewModel();
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
            DependencyProperty.Register("VeiculoSelecionadoIDView", typeof(int), typeof(VeiculosEmpresasView), new PropertyMetadata(0, PropertyChanged));
        private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            int _veiculoID = Convert.ToInt32(e.NewValue);
            if (_veiculoID != _veiculoIDFisrt && _veiculoID != 0)
            {
                ((VeiculosEmpresasViewModel)((System.Windows.FrameworkElement)source).DataContext).OnAtualizaCommand(_veiculoID);
                _veiculoIDFisrt = _veiculoID;
            }
        }

        public bool Editando
        {
            get { return (bool)GetValue(EditandoProperty); }
            set { SetValue(EditandoProperty, value); }
        }

        public static readonly DependencyProperty EditandoProperty =
            DependencyProperty.Register("Editando", typeof(bool), typeof(VeiculosEmpresasView), new FrameworkPropertyMetadata(true,
                                                           FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(EditandoPropertyChanged)));

        private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Comando dos Botoes
        private void BuscarApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            //((VeiculosEmpresasViewModel)this.DataContext).OnBuscarArquivoCommand();
        }

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculosEmpresasViewModel)this.DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false;
            //EmpresaRazaoSocial_cb.IsEnabled = false;
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaVeiculosEmpresas_lv.IsHitTestVisible = false;
            Global.SetReadonly(Linha0_sp, false);
            ((VeiculosEmpresasViewModel)this.DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false;
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Global.SetReadonly(Linha0_sp, false);
            ((VeiculosEmpresasViewModel)this.DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosEmpresasViewModel)this.DataContext).OnExcluirCommand();
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
            Editando = true;
            EmpresaRazaoSocial_cb.IsEnabled = true;
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaVeiculosEmpresas_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
            ((VeiculosEmpresasViewModel)this.DataContext).OnCancelarEdicaoCommand();
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Vínculo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }
            EmpresaRazaoSocial_cb.IsEnabled = true;
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosEmpresasViewModel)this.DataContext).OnSalvarEdicaoCommand();
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaVeiculosEmpresas_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
            Editando = true;
        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosEmpresasViewModel)this.DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
        }

        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Vínculo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosEmpresasViewModel)this.DataContext).OnSalvarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
            Editando = true;
        }

        #endregion

        private void Validade_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender);
        }

        private void ListaVeiculosEmpresas_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaVeiculosEmpresas_lv.SelectedIndex == -1)
            {
                Linha0_sp.IsEnabled = false;
                Editar_bt.IsEnabled = false;
            }
            else
            {
                Linha0_sp.IsEnabled = true;
                Editar_bt.IsEnabled = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Global.SetReadonly(Linha0_sp, true);
            ListaVeiculosEmpresas_lv.SelectedIndex = -1;
            Linha0_sp.IsEnabled = false;
            Editar_bt.IsEnabled = false;
        }

        private void Ativo_cb_Checked(object sender, RoutedEventArgs e)
        {
            if (((VeiculosEmpresasViewModel)this.DataContext).VerificaVinculo() && !Editando)
            {
                SalvarEdicao_bt.IsEnabled = false;
                SalvarAdicao_bt.IsEnabled = false;
                Global.PopupBox("Este veiculo/equipamento já possui vínculo ativo com este contrato!", 1);

            }
            else
            {
                SalvarEdicao_bt.IsEnabled = true;
                SalvarAdicao_bt.IsEnabled = true;
            }
        }

        private void Ativo_cb_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!Editando)
            {
                if (!Global.PopupBox("Todas as credenciais deste veiculo/equipamento serão canceladas! Confirma desligamento?", 2))
                {
                    Ativo_cb.IsChecked = true;
                }
                return;
            }
        }

        private void Contrato_cb_DropDownClosed(object sender, EventArgs e)
        {
            if (((VeiculosEmpresasViewModel)this.DataContext).VerificaVinculo() && !Editando)
            {
                SalvarEdicao_bt.IsEnabled = false;
                SalvarAdicao_bt.IsEnabled = false;
                Global.PopupBox("Este veiculo/equipamento já possui vínculo ativo com este contrato!", 1);

            }
            else
            {
                SalvarEdicao_bt.IsEnabled = true;
                SalvarAdicao_bt.IsEnabled = true;
            }
            var display1 = EmpresaRazaoSocial_cb.Text;
            var display2 = Contrato_cb.Text;
            var data1 = ((VeiculosEmpresasViewModel)this.DataContext).VeiculoEmpresaSelecionado;
            data1.EmpresaNome = display1;
            data1.Descricao = display2;
            //ListaColaboradoresEmpresas_lv.ItemsSource = data1;
            ListaVeiculosEmpresas_lv.Items.Refresh();
        }
    }
}
