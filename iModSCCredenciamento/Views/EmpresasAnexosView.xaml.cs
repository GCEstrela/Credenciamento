﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels;
using IMOD.CrossCutting;

#endregion

namespace iModSCCredenciamento.Views
{
    /// <summary>
    ///     Interação lógica para EmpresasAnexosView.xam
    /// </summary>
    public partial class EmpresasAnexosView : UserControl
    {
        private readonly EmpresasAnexosViewModel _viewModel;

        public EmpresasAnexosView()
        {
            InitializeComponent();
            _viewModel = new EmpresasAnexosViewModel();
            DataContext = _viewModel;
        }

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDadosAnexo(Model.EmpresaView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDadosAnexo (entity);
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
                var arq = WpfHelp.UpLoadArquivoDialog (filtro, 700);
                if (arq == null) return;
                _viewModel.Entity.Anexo = arq.FormatoBase64;
                _viewModel.Entity.NomeAnexo = arq.Nome;
                txtNomeAnexo.Text = arq.Nome;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox (ex.Message);
                Utils.TraceException (ex);
            }
        }

        /// <summary>
        ///     Downlaod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var arquivoStr = _viewModel.Entity.Anexo;
                var arrBytes = Convert.FromBase64String (arquivoStr);
                WpfHelp.DownloadArquivoDialog (_viewModel.Entity.NomeAnexo, arrBytes);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void ListaAnexos_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (ListaAnexos_lv.SelectedIndex == -1)
            //{
            //    Linha0_sp.IsEnabled = false;
            //    Editar_bt.IsEnabled = false;
            //}
            //else
            //{
            //    Linha0_sp.IsEnabled = true;
            //    Editar_bt.IsEnabled = true;
            //    AbrirContratoArquivo_bt.IsHitTestVisible = true;
            //}
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Global.SetReadonly(Linha0_sp, true);
            //ListaAnexos_lv.SelectedIndex = -1;
            //Linha0_sp.IsEnabled = false;
            //Editar_bt.IsEnabled = false;
        }

        #endregion

        //#region Vinculo do UserControl
        //static int _empresaIDFisrt;
        //public int EmpresaSelecionadaIDView
        //{
        //    get { return (int)GetValue(EmpresaSelecionadaIDViewProperty); }
        //    set { SetValue(EmpresaSelecionadaIDViewProperty, value); }
        //}

        //public static readonly DependencyProperty EmpresaSelecionadaIDViewProperty =
        //    DependencyProperty.Register("EmpresaSelecionadaIDView", typeof(int), typeof(EmpresasAnexosView), new PropertyMetadata(0, PropertyChanged));
        //private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        //{
        //    int _empresaID = Convert.ToInt32(e.NewValue);
        //    if (_empresaID != _empresaIDFisrt && _empresaID != 0)
        //    {
        //        ((EmpresasAnexosViewModel)((FrameworkElement)source).DataContext).OnAtualizaCommand(_empresaID);
        //        _empresaIDFisrt = _empresaID;
        //    }
        //}

        //public bool Editando
        //{
        //    get { return (bool)GetValue(EditandoProperty); }
        //    set { SetValue(EditandoProperty, value); }
        //}

        //public static readonly DependencyProperty EditandoProperty =
        //    DependencyProperty.Register("Editando", typeof(bool), typeof(EmpresasAnexosView), new FrameworkPropertyMetadata(true,
        //                                                   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EditandoPropertyChanged));

        //private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}
        //    #endregion

        #region Comando dos Botoes

        private void BuscarApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            //((EmpresasAnexosViewModel)DataContext).OnBuscarArquivoCommand();
            // ApoliceArquivo_tb.Text = ((EmpresasAnexosViewModel)DataContext).Anexos[0].NomeAnexo;
        }

        private void AbrirApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            //((EmpresasAnexosViewModel)DataContext).OnAbrirArquivoCommand();
        }

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            //((EmpresasAnexosViewModel)DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            //Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //Botoes_Editar_sp.Visibility = Visibility.Visible;
            //ListaAnexos_lv.IsHitTestVisible = false;
            //Global.SetReadonly(Linha0_sp, false);
            //((EmpresasAnexosViewModel)DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            //Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            //Global.SetReadonly(Linha0_sp, false);
            //((EmpresasAnexosViewModel)DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            //((EmpresasAnexosViewModel)DataContext).OnExcluirCommand();
        }

        private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            ////Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
            //Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            ////((EmpresasSegurosViewModel)this.DataContext).ExecutarPesquisaCommand();
        }

        private void CancelarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            //Botoes_Pesquisar_sp.Visibility = Visibility.Hidden;
        }

        private void CancelarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            //Botoes_Editar_sp.Visibility = Visibility.Hidden;
            //ListaAnexos_lv.IsHitTestVisible = true;
            //Global.SetReadonly(Linha0_sp, true);
            //((EmpresasAnexosViewModel)DataContext).OnCancelarEdicaoCommand();
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            ////if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Empresa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            ////{
            ////    return;
            ////}
            //if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            //{
            //    return;
            //}

            //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            //((EmpresasAnexosViewModel)DataContext).OnSalvarEdicaoCommand();
            //Botoes_Editar_sp.Visibility = Visibility.Hidden;
            //ListaAnexos_lv.IsHitTestVisible = true;
            //Global.SetReadonly(Linha0_sp, true);
        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            //((EmpresasAnexosViewModel)DataContext).OnCancelarAdicaoCommand();
            //Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            //Global.SetReadonly(Linha0_sp, true);
        }

        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            ////if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Empresa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            ////{
            ////    return;
            ////}
            //if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            //{
            //    return;
            //}

            //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
            //((EmpresasAnexosViewModel)DataContext).OnSalvarAdicaoCommand();
            //Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            //Global.SetReadonly(Linha0_sp, true);
        }

        #endregion
    }
}