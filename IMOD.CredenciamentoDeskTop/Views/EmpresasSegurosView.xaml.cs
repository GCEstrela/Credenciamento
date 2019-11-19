// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  13 - 11 - 2019
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
    /// Interação lógica para EmpresasSegurosView.xam
    /// </summary>
    public partial class EmpresasSegurosView : UserControl
    {
        private readonly EmpresasSegurosViewModel _viewModel;
        #region Inicializacao
        public EmpresasSegurosView()
        {
            InitializeComponent();
            _viewModel = new EmpresasSegurosViewModel();
            DataContext = _viewModel;
            
        }
        #endregion

        #region  Metodos
        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModelParent"></param>
        public void AtualizarDados(Model.EmpresaView entity, EmpresaViewModel viewModelParent)
        {
            //if (entity == null) return;
            _viewModel.AtualizarDados(entity, viewModelParent);
        }
        #endregion

        private void ListaSeguros_lve_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (_viewModel.Entity == null) return;
                if (_viewModel.Contratos == null) return;
                _viewModel.Contrato = _viewModel.Contratos.Find(s => s.EmpresaContratoId == _viewModel.Entity.EmpresaContratoId);
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                throw ex;
            }
        }

        private void BtnBuscarArquivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                //var arq = WpfHelp.UpLoadArquivoDialog (filtro, 700);
                var filtro = "Imagem files (*.pdf)|*.pdf";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, _viewModel.IsTamanhoArquivo);
                if (arq == null) return;
                _viewModel.Entity.Arquivo = arq.FormatoBase64;
                _viewModel.Entity.NomeArquivo = arq.Nome;
                txtApoliceArquivo.Text = arq.Nome;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        private void AbrirArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.BuscarAnexo(_viewModel.Entity.EmpresaSeguroId);
                var arquivoStr = _viewModel.Entity.Arquivo;
                var arrBytes = Convert.FromBase64String(arquivoStr);
                WpfHelp.AbrirArquivoPdf(_viewModel.Entity.NomeArquivo, arrBytes);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
    }
}
