// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
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
    ///     Interação lógica para VeiculosSegurosView.xam
    /// </summary>
    public partial class VeiculosSegurosView : UserControl
    {
        private readonly VeiculosSegurosViewModel _viewModel;

        #region Inicializacao

        public VeiculosSegurosView()
        {
            InitializeComponent();
            _viewModel = new VeiculosSegurosViewModel();
            DataContext = _viewModel;
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModelParent"></param>
        public void AtualizarDados(Model.VeiculoView entity, VeiculoViewModel viewModelParent)
        {
            //if (entity == null) return;
            _viewModel.AtualizarDados (entity, viewModelParent);
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
                //var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                //var arq = WpfHelp.UpLoadArquivoDialog (filtro, 700);
                var filtro = "Imagem files (*.pdf)|*.pdf";
                var arq = WpfHelp.UpLoadArquivoDialog (filtro, 2000);
                if (arq == null) return;
                _viewModel.Entity.Arquivo = arq.FormatoBase64;
                _viewModel.Entity.NomeArquivo = arq.Nome;
                txtApoliceArquivo.Text = arq.Nome;
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
                var arquivoStr = _viewModel.Entity.Arquivo;
                var arrBytes = Convert.FromBase64String (arquivoStr);
                WpfHelp.AbrirArquivoPdf (_viewModel.Entity.NomeArquivo, arrBytes);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex ("[^0-9]+");
            e.Handled = regex.IsMatch (e.Text);
        }

        private void OnFormatData1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDtEmissao.Text;
                if (string.IsNullOrWhiteSpace (str)) return;
                txtDtEmissao.Text = str.FormatarData();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro ("Validade", "Data inválida");
            }
        }

        private void OnFormatData2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDtValidade.Text;
                if (string.IsNullOrWhiteSpace (str)) return;
                txtDtValidade.Text = str.FormatarData();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro ("Validade", "Data inválida");
            }
        }

        #endregion
    }
}