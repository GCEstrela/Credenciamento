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
    /// Interação lógica para VeiculoCredencialView.xam
    /// </summary>
    public partial class VeiculosCredenciaisView : UserControl
    {

        #region Inicializacao
        public VeiculosCredenciaisView()
        {
            InitializeComponent();
            this.DataContext = new VeiculosCredenciaisViewModel();
            ImprimirCredencial_bt.IsHitTestVisible = true;
        }
        #endregion

        #region Vinculo do UserControl
        static int _colaboradorIDFisrt = 0;
        public int VeiculoSelecionadoIDView
        {
            get { return (int)GetValue(VeiculoSelecionadoIDViewProperty); }
            set { SetValue(VeiculoSelecionadoIDViewProperty, value); }
        }

        public static readonly DependencyProperty VeiculoSelecionadoIDViewProperty =
            DependencyProperty.Register("VeiculoSelecionadoIDView", typeof(int), typeof(VeiculosCredenciaisView), new PropertyMetadata(0, PropertyChanged));
        private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            int _colaboradorID = Convert.ToInt32(e.NewValue);
            if (_colaboradorID != _colaboradorIDFisrt && _colaboradorID != 0)
            {
                ((iModSCCredenciamento.ViewModels.VeiculosCredenciaisViewModel)((System.Windows.FrameworkElement)source).DataContext).OnAtualizaCommand(_colaboradorID);
                _colaboradorIDFisrt = _colaboradorID;
            }
        }

        public bool Editando
        {
            get { return (bool)GetValue(EditandoProperty); }
            set { SetValue(EditandoProperty, value); }
        }

        public static readonly DependencyProperty EditandoProperty =
            DependencyProperty.Register("Editando", typeof(bool), typeof(VeiculosCredenciaisView), new FrameworkPropertyMetadata(true,
                                                           FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(EditandoPropertyChanged)));

        private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Comando dos Botoes
        //private void BuscarApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    ((VeiculosCredenciaisViewModel)this.DataContext).OnBuscarArquivoCommand();
        //}

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculosCredenciaisViewModel)this.DataContext).OnPesquisarCommand();
        }

        private void BuscarValidade_bt_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculosCredenciaisViewModel)this.DataContext).OnBuscarDataCommand();
            ListaVeiculosCredenciais_lv.Items.Refresh();
        }

        private void ImprimirCredencial_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string _credencialInfo = ModeloCredencial_tb.SelectedValue.ToString() + (char)(20) + FC_tb.Text +
                //    (char)(20) + NumeroCredencial_tb.Text + (char)(20) + FormatoCredencial_cb.Text + (char)(20) + Validade_tb.Text;

                ((VeiculosCredenciaisViewModel)this.DataContext).OnImprimirCommand();
            }
            catch (Exception ex)
            {

            }
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {

            Editando = false;
            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaVeiculosCredenciais_lv.IsHitTestVisible = false;
            Global.SetReadonly(Linha0_sp, false);
            ((VeiculosCredenciaisViewModel)this.DataContext).OnEditarCommand();

        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            //Inserido pois não está funcionando o SelectedIndex=0!
            Linha0_sp.IsEnabled = true;
            Editar_bt.IsEnabled = true;
            ///

            Editando = false;
            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Global.SetReadonly(Linha0_sp, false);
            ((VeiculosCredenciaisViewModel)this.DataContext).OnAdicionarCommand();

        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosCredenciaisViewModel)this.DataContext).OnExcluirCommand();
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
            EmpresaVinculo_cb.IsEnabled = true;
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaVeiculosCredenciais_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
            ImprimirCredencial_bt.IsHitTestVisible = true;
            ((VeiculosCredenciaisViewModel)this.DataContext).OnCancelarEdicaoCommand();

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
            EmpresaVinculo_cb.IsEnabled = true;
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosCredenciaisViewModel)this.DataContext).OnSalvarAdicaoCommand();
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaVeiculosCredenciais_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
            ImprimirCredencial_bt.IsHitTestVisible = true;
            Editando = true;

        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;
            Editando = true; Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculosCredenciaisViewModel)this.DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
            ImprimirCredencial_bt.IsHitTestVisible = true;

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
            ((VeiculosCredenciaisViewModel)this.DataContext).OnSalvarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
            ImprimirCredencial_bt.IsHitTestVisible = true;
            Editando = true;

        }

        #endregion

        private void Validade_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender);
        }

        private void ListaVeiculosCredenciais_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaVeiculosCredenciais_lv.SelectedIndex == -1)
            {
                //Retirado pois não está funcionando o SelectedIndex=0!
                //Linha0_sp.IsEnabled = false;
                //Editar_bt.IsEnabled = false;
            }
            else
            {
                Linha0_sp.IsEnabled = true;
                Editar_bt.IsEnabled = true;
                ImprimirCredencial_bt.IsHitTestVisible = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Global.SetReadonly(Linha0_sp, true);
            ListaVeiculosCredenciais_lv.SelectedIndex = -1;
            Linha0_sp.IsEnabled = false;
            Editar_bt.IsEnabled = false;
            ImprimirCredencial_bt.IsHitTestVisible = true;

        }

        private void StatusCredencial_tb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (((iModSCCredenciamento.Models.ClasseCredenciaisStatus.CredencialStatus)((object[])e.AddedItems)[0]).CredencialStatusID == 1)
                {
                    Ativa_tw.IsChecked = true;
                    ((VeiculosCredenciaisViewModel)this.DataContext).CarregaColecaoCredenciaisMotivos(1);
                }
                else
                {
                    Ativa_tw.IsChecked = false;
                    ((VeiculosCredenciaisViewModel)this.DataContext).CarregaColecaoCredenciaisMotivos(2);
                }
            }

        }
    }
}
