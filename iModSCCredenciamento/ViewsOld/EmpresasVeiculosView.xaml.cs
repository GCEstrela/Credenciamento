﻿using System;
using System.Windows;
using System.Windows.Controls;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.ViewModels;

namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interação lógica para EmpresasVeiculosView.xam
    /// </summary>
    public partial class EmpresasVeiculosView : UserControl
    {
        #region Inicializacao
        public EmpresasVeiculosView()
        {
            InitializeComponent();
            DataContext = new EmpresasVeiculosViewModel();
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
            DependencyProperty.Register("EmpresaSelecionadaIDView", typeof(int), typeof(EmpresasVeiculosView), new PropertyMetadata(0, PropertyChanged));
        private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            int _empresaID = Convert.ToInt32(e.NewValue);
            if (_empresaID != _empresaIDFisrt && _empresaID != 0)
            {
                ((EmpresasVeiculosViewModel)((FrameworkElement)source).DataContext).OnAtualizaCommand(_empresaID);
                _empresaIDFisrt = _empresaID;
            }
        }
        public bool Editando
        {
            get { return (bool)GetValue(EditandoProperty); }
            set { SetValue(EditandoProperty, value); }
        }

        public static readonly DependencyProperty EditandoProperty =
            DependencyProperty.Register("Editando", typeof(bool), typeof(EmpresasVeiculosView), new FrameworkPropertyMetadata(true,
                                                           FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EditandoPropertyChanged));

        private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region Comando dos Botoes
        private void VincularCredencial_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string _credencialInfo = ModeloCredencial_tb.SelectedValue.ToString() + (char)(20) + FC_tb.Text + (char)(20) + NumeroCredencial_tb.Text + (char)(20) + FormatoCredencial_cb.Text;

                ((EmpresasVeiculosViewModel)DataContext).OnVincularCommand(_credencialInfo);
            }
            catch (Exception ex)
            {

            }
        }
        private void ImprimirCredencial_bt_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresasVeiculosViewModel)DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaVeiculos_lv.IsHitTestVisible = false;
            Global.SetReadonly(Linha0_sp, false);
            ((EmpresasVeiculosViewModel)DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Global.SetReadonly(Linha0_sp, false);
            ((EmpresasVeiculosViewModel)DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasVeiculosViewModel)DataContext).OnExcluirCommand();
        }

        private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            //Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
            Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //((EmpresasVeiculosViewModel)this.DataContext).ExecutarPesquisaCommand();
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
            ListaVeiculos_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
            ((EmpresasVeiculosViewModel)DataContext).OnCancelarEdicaoCommand();
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Veículo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }

            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasVeiculosViewModel)DataContext).OnSalvarEdicaoCommand();
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaVeiculos_lv.IsHitTestVisible = true;
            Global.SetReadonly(Linha0_sp, true);
        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasVeiculosViewModel)DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
        }

        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Veículo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }

            Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresasVeiculosViewModel)DataContext).OnSalvarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Global.SetReadonly(Linha0_sp, true);
        }


        #endregion

        private void ListaVeiculos_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (ListaVeiculos_lv.SelectedIndex == -1)
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
            ListaVeiculos_lv.SelectedIndex = -1;
            Linha0_sp.IsEnabled = false;
            Editar_bt.IsEnabled = false;
        }
    }
}
