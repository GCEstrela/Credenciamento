#region

using System;
using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    ///     Interação lógica para EquipamentosAnexosView.xaml
    /// </summary>
    public partial class EquipamentosAnexosView : UserControl
    {
        private readonly EquipamentosAnexosViewModel _viewModel;

        #region Inicializacao

        public EquipamentosAnexosView()
        {
            InitializeComponent();
            _viewModel = new EquipamentosAnexosViewModel();
            DataContext = _viewModel;
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.VeiculoView entity,  EquipamentosViewModel viewModelParent)
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
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 2000);
                if (arq == null) return;
                _viewModel.Entity.Arquivo = arq.FormatoBase64;
                _viewModel.Entity.NomeArquivo = arq.Nome;
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
                
                var arquivoStr = _viewModel.Entity.Arquivo;
                var arrBytes = Convert.FromBase64String(arquivoStr);
                WpfHelp.AbrirArquivoPdf(_viewModel.Entity.NomeArquivo, arrBytes);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion
    }
}