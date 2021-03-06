﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    ///     Interação lógica para ColaboradoresEmpresasView.xam
    /// </summary>
    public partial class ColaboradoresEmpresasView : UserControl
    {
        private readonly ColaboradoresEmpresasViewModel _viewModel;

        public ColaboradoresEmpresasView()
        {
            try
            {
                InitializeComponent();
                _viewModel = new ColaboradoresEmpresasViewModel();
                DataContext = _viewModel;
            }
            catch (Exception ex)
            {
                //WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }
        #region  Metodos

        private void Frm_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEmpresa.SelectionChanged += OnSelecionaContrato_SelectionChanged;
        }

        private void OnSelecionaContrato_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.Empresa == null) return;
            _viewModel.ListarContratos(_viewModel.Empresa);
            _viewModel.BuscarAnexo(_viewModel.Entity.ColaboradorEmpresaId);
            cmbContrato.Items.Refresh();
        }

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {
            //if (entity == null) return;
            _viewModel.AtualizarDados(entity, viewModelParent);
            //if (!_viewModel.IsEnableComboContrato)
            //{
            //    ListaSegnatarios_lv.Columns[2].Visible = false;
            //    ListaSegnatarios_lv.gri
            //}
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
                var filtro = "Imagem files (*.pdf)|*.pdf";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, _viewModel.IsTamanhoArquivo);
                if (arq == null) return;
                _viewModel.Entity.Anexo = arq.FormatoBase64;
                _viewModel.Entity.NomeAnexo = arq.Nome;
                txtNomeAnexo.Text = arq.Nome;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
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
                _viewModel.BuscarAnexo(_viewModel.Entity.ColaboradorEmpresaId);
                var arrBytes = Convert.FromBase64String(_viewModel.Entity.Anexo);
                WpfHelp.AbrirArquivoPdf(_viewModel.Entity.NomeAnexo, arrBytes);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OnFormatData_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDtValidade.Text;
                if (string.IsNullOrWhiteSpace(str))
                {
                    _viewModel.Entity.Validade = null;
                    return;
                }
                txtDtValidade.Text = str.FormatarData();

            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Validade", "Data inválida");
            }
        }

        #endregion

        private void TxtDtInicio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDtInicio.Text;
                if (string.IsNullOrWhiteSpace(str)) return;
                txtDtInicio.Text = str.FormatarData();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Data Inicio", "Data inválida");
            }
        }

        private void TxtDtFim_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDtFim.Text;
                if (string.IsNullOrWhiteSpace(str)) return;
                txtDtFim.Text = str.FormatarData();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Data Fim", "Data inválida");
            }
        }

        private void ChkAtivo_Checked(object sender, RoutedEventArgs e)
        {
            txtDtInicio.Text = DateTime.Today.Date.ToShortDateString();
            txtDtFim.Text = "";
        }

        private void ChkAtivo_Unchecked(object sender, RoutedEventArgs e)
        {
            //txtDtInicio.Text = "";
            //_viewModel.Entity.DataFim= DateTime.Today.Date;
            txtDtFim.Text = DateTime.Today.Date.ToShortDateString();
        }
    }
}