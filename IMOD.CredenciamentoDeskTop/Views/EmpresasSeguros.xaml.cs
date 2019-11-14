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
    /// Interação lógica para EmpresasSeguros.xam
    /// </summary>
    public partial class EmpresasSeguros : UserControl
    {
        private readonly EmpresasSegurosViewModel _viewModel;
        public EmpresasSeguros()
        {
            InitializeComponent();
            _viewModel = new EmpresasSegurosViewModel();
            DataContext = _viewModel;
        }
        #region  Metodos
        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModelParent"></param>
        public void AtualizarDados(Model.VeiculoView entity, VeiculoViewModel viewModelParent)
        {
            //if (entity == null) return;
            //_viewModel.AtualizarDados(entity, viewModelParent);
        }
        #endregion

        private void OnUpLoad_Click(object sender, RoutedEventArgs e)
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
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }
    }
}
