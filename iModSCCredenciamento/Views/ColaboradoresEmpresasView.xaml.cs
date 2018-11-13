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
    /// Interação lógica para ColaboradoresEmpresasView.xam
    /// </summary>
    public partial class ColaboradoresEmpresasView : UserControl
    {

        #region Inicializacao
        public ColaboradoresEmpresasView()
        {
            InitializeComponent();
            this.DataContext = new ColaboradoresEmpresasViewModel();
        }
        #endregion

        #region Vinculo do UserControl
        static int _colaboradorIDFisrt = 0;
        public int ColaboradorSelecionadoIDView
        {
            get { return (int)GetValue(ColaboradorSelecionadoIDViewProperty); }
            set { SetValue(ColaboradorSelecionadoIDViewProperty, value); }
        }

        public static readonly DependencyProperty ColaboradorSelecionadoIDViewProperty =
            DependencyProperty.Register("ColaboradorSelecionadoIDView", typeof(int), typeof(ColaboradoresEmpresasView), new PropertyMetadata(0, PropertyChanged));
        private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            int _colaboradorID = Convert.ToInt32(e.NewValue);
            if (_colaboradorID != _colaboradorIDFisrt && _colaboradorID != 0)
            {
                ((iModSCCredenciamento.ViewModels.ColaboradoresEmpresasViewModel)((System.Windows.FrameworkElement)source).DataContext).OnAtualizaCommand(_colaboradorID);
                _colaboradorIDFisrt = _colaboradorID;
            }
        }

        public bool Editando
        {
            get { return (bool)GetValue(EditandoProperty); }
            set { SetValue(EditandoProperty, value); }
        }

        public static readonly DependencyProperty EditandoProperty =
            DependencyProperty.Register("Editando", typeof(bool), typeof(ColaboradoresEmpresasView), new FrameworkPropertyMetadata(true,
                                                           FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(EditandoPropertyChanged)));

        private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Comando dos Botoes
        private void BuscarApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ColaboradoresEmpresasViewModel)this.DataContext).OnBuscarArquivoCommand();
        }

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ColaboradoresEmpresasViewModel)this.DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false;
            EmpresaRazaoSocial_cb.IsEnabled = false;
            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaColaboradoresEmpresas_lv.IsHitTestVisible = false;
            Global.SetReadonly(Linha0_sp, false);
            ((ColaboradoresEmpresasViewModel)this.DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false;
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Global.SetReadonly(Linha0_sp, false);
            ((ColaboradoresEmpresasViewModel)this.DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((ColaboradoresEmpresasViewModel)this.DataContext).OnExcluirCommand();
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
            Editando = true;
            EmpresaRazaoSocial_cb.IsEnabled = true;
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaColaboradoresEmpresas_lv.IsHitTestVisible = true;
            SalvarEdicao_bt.IsEnabled = true;
            Global.SetReadonly(Linha0_sp, true);
            ((ColaboradoresEmpresasViewModel)this.DataContext).OnCancelarEdicaoCommand();
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
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((ColaboradoresEmpresasViewModel)this.DataContext).OnSalvarEdicaoCommand();
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaColaboradoresEmpresas_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
            Editando = true;
        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((ColaboradoresEmpresasViewModel)this.DataContext).OnCancelarAdicaoCommand();
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
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((ColaboradoresEmpresasViewModel)this.DataContext).OnSalvarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
            Editando = true;
        }

        #endregion

        private void Validade_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender);
        }

        private void ListaColaboradoresEmpresas_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaColaboradoresEmpresas_lv.SelectedIndex == -1)
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
            ListaColaboradoresEmpresas_lv.SelectedIndex = -1;
            Linha0_sp.IsEnabled = false;
            Editar_bt.IsEnabled = false;
        }

        //private void Contrato_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
            
        //    if (((ColaboradoresEmpresasViewModel)this.DataContext).VerificaVinculo() && !Editando)
        //    {
        //        SalvarEdicao_bt.IsEnabled = false;
        //        SalvarAdicao_bt.IsEnabled = false;
        //        Global.PopupBox("Este colaborador já possui vínculo ativo com este contrato!", 1);
                
        //    }
        //    else
        //    {
        //        SalvarEdicao_bt.IsEnabled = true;
        //        SalvarAdicao_bt.IsEnabled = true;
        //    }

            

        //}

        private void Contrato_cb_DropDownClosed(object sender, EventArgs e)
        {

            if (((ColaboradoresEmpresasViewModel)this.DataContext).VerificaVinculo() && !Editando)
            {
                SalvarEdicao_bt.IsEnabled = false;
                SalvarAdicao_bt.IsEnabled = false;
                Global.PopupBox("Este colaborador já possui vínculo ativo com este contrato!", 1);

            }
            else
            {
                SalvarEdicao_bt.IsEnabled = true;
                SalvarAdicao_bt.IsEnabled = true;
            }
            var display1 = EmpresaRazaoSocial_cb.Text;
            var display2 = Contrato_cb.Text;
            var data1 = ((ColaboradoresEmpresasViewModel)this.DataContext).ColaboradorEmpresaSelecionado;
            data1.EmpresaNome = display1;
            data1.Descricao = display2;
            //ListaColaboradoresEmpresas_lv.ItemsSource = data1;
            ListaColaboradoresEmpresas_lv.Items.Refresh();

        }

        private void Ativo_cb_Checked(object sender, RoutedEventArgs e)
        {
            if (((ColaboradoresEmpresasViewModel)this.DataContext).VerificaVinculo() && !Editando)
            {
                SalvarEdicao_bt.IsEnabled = false;
                SalvarAdicao_bt.IsEnabled = false;
                Global.PopupBox("Este colaborador já possui vínculo ativo com este contrato!", 1);

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
                if (!Global.PopupBox("Todas as credenciais deste colaborador serão canceladas! Confirma desligamento?",2))
                {
                    Ativo_cb.IsChecked = true;
                }
                return;
            }
        }

        //private void EmpresaRazaoSocial_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    int _empresa = Convert.ToInt32(EmpresaRazaoSocial_cb.SelectedValue);
        //    ((ColaboradoresEmpresasViewModel)this.DataContext).OnSelecionaEmpresaCommand(_empresa);
        //}
    }
}
