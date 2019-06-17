﻿
// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.Enums;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
{
   
    /// <summary>
    ///     Interação lógica para VeiculoView.xam
    /// </summary>
    public partial class VeiculoView : UserControl
    {
        private readonly   VeiculoViewModel _viewModel;

        public VeiculoView()
        {
            try
            {
                InitializeComponent();
                _viewModel = new VeiculoViewModel();
                DataContext = _viewModel;
            }
            catch (SqlException)
            {
                if (_viewModel.EntityObserver != null)
                    _viewModel.EntityObserver.Clear();


                Exception exs = new Exception($"Ocorreu uma falha ao conectar com o banco de dados.");
                WpfHelp.PopupBox(exs.Message, 1);
            }
            catch (Exception ex)
            {

                WpfHelp.PopupBox(ex.Message, 1);
                Utils.TraceException(ex);
            }
        }

        #region  Metodos

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OnListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Geral_ti.IsSelected = true;
                if (_viewModel.Entity == null) return;
                //Atualizar dados ao selecionar uma linha da listview
                _viewModel.AtualizarDadosPendencias();
                _viewModel.AtualizarDadosTiposServico();
                _viewModel.AtualizarDadosTiposServico();
                if (_viewModel.Entity != null)
                    _viewModel.BucarFoto(_viewModel.Entity.EquipamentoVeiculoId);
                //Popular User Controls
                //////////////////////////////////////////////////////////////
                VeiculosEmpresasUs.AtualizarDados(_viewModel.Entity, _viewModel);
                EmpresaSeguroUs.AtualizarDados(_viewModel.Entity, _viewModel);
                AnexoUs.AtualizarDados(_viewModel.Entity, _viewModel);
                VeiculoCredenciaisUs.AtualizarDados(_viewModel.Entity, _viewModel);
                /////////////////////////////////////////////////////////////
                //_viewModel.IsEnableTabItem = true;
            }
            catch (SqlException)
            {
                if (_viewModel.EntityObserver != null)
                    _viewModel.EntityObserver.Clear();


                Exception exs = new Exception($"Ocorreu uma falha ao conectar com o banco de dados.");
                WpfHelp.PopupBox(exs.Message, 1);
            }
            catch (Exception ex)
            {

                WpfHelp.PopupBox(ex.Message, 1);
                Utils.TraceException(ex);
            }
        }

        private void OnSelecionaMunicipio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.Estado == null) return;
            _viewModel.ListarMunicipios(_viewModel.Estado.Uf);
        }

        private void Frm_Loaded(object sender, RoutedEventArgs e)
        {
             lstView.SelectionChanged += OnListView_SelectionChanged;
            cmbEstado.SelectionChanged += OnSelecionaMunicipio_SelectionChanged;
            txtPesquisa.Focus();
        }

        /// <summary>
        ///     Remover tipo serviço
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoverTipoServico_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TipoServico == null) return;
            var item = lstBoxTipoAtividade.SelectedItem;
            if (item == null) return ;
            var idx =  lstBoxTipoAtividade.Items.IndexOf(item) ;
            _viewModel.TiposEquipamentoServico.RemoveAt(idx);
        }

        /// <summary>
        ///     Adicionar Tipo serviço
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAdicionarTipoServico_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TipoServico == null) return;

            var n1 = new EquipamentoVeiculoTipoServicoView
            {
                EquipamentoVeiculoId = _viewModel.Entity.EquipamentoVeiculoId,
                TipoServicoId = _viewModel.TipoServico.TipoServicoId,
                Descricao = _viewModel.TipoServico.Descricao
            };
            _viewModel.TiposEquipamentoServico.Add(n1);
        }

        private void AbrirPendencias(int codigo, PendenciaTipo tipoPendecia)
        {
            try
            {
                if (_viewModel.Entity == null) return;

                var frm = new PopupPendencias();
                frm.Inicializa(codigo, _viewModel.Entity.EquipamentoVeiculoId, tipoPendecia);
                frm.ShowDialog();
                _viewModel.AtualizarDadosPendencias();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void OnSelecionaFoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro,_viewModel.IsTamanhoImagem);
                if (arq == null) return;
                _viewModel.Entity.Foto = arq.FormatoBase64;
                var binding = BindingOperations.GetBindingExpression(Logo_im, Image.SourceProperty);
                binding?.UpdateTarget();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void OnPendenciaGeralAnexos_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(24, PendenciaTipo.Veiculo);
        }

        private void OnPendenciaVinculos_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(22, PendenciaTipo.Veiculo);
        }

        private void OnPendenciaSeguros_Click(object sender, RoutedEventArgs e)
        {
           AbrirPendencias(19, PendenciaTipo.Veiculo);
        }

        private void OnPendenciaCredencias_Click(object sender, RoutedEventArgs e)
        {
             AbrirPendencias(25, PendenciaTipo.Veiculo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPendenciaGeral_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(21, PendenciaTipo.Veiculo);
        }


        #endregion

        private void LstView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (lstView.SelectedIndex > -1)
                {
                    int currentIndex = lstView.SelectedIndex;
                    int Sum = lstView.Items.Count;
                    if (currentIndex > Sum)
                        currentIndex -= Sum;
                    if (e.Key.ToString() != "Up")
                    {
                        if (currentIndex == lstView.Items.Count - 1) return;
                        ((ListViewItem)(lstView.ItemContainerGenerator.ContainerFromIndex(currentIndex + 1))).Focus();
                    }
                    else
                    {
                        if (currentIndex == 0) return;
                        ((ListViewItem)(lstView.ItemContainerGenerator.ContainerFromIndex(currentIndex - 1))).Focus();
                    }
                }


            }
            catch (Exception)
            {
                //WpfHelp.Mbox(ex.ToString());
            }
        }

        private void Rd_cadastro_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.EntityObserver.Clear();
                _viewModel.IsEnablePreCadastro = false;
                _viewModel.IsEnablePreCadastroCredenciamento = true;
                _viewModel.IsEnablePreCadastroColor = "#FFD0D0D0";
                //_importarBNT = "Collapsed";
            }
            catch (Exception)
            {
                //WpfHelp.Mbox(ex.ToString());
            }
        }

        private void Rd_precadastro_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.EntityObserver.Clear();
                _viewModel.IsEnablePreCadastro = true;
                _viewModel.IsEnablePreCadastroCredenciamento = false;
                _viewModel.IsEnablePreCadastroColor = "Orange";
                //_importarBNT = "Visible";
            }
            catch (Exception)
            {
                //WpfHelp.Mbox(ex.ToString());
            }
        }
    }
}