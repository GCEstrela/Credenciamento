using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    /// Interação lógica para ColaboradoresCursosView.xam
    /// </summary>
    public partial class UsColaboradoresCursosView : UserControl
    {
        private readonly ColaboradoresCursosViewModel _viewModel;
        #region Inicializacao
        public UsColaboradoresCursosView()
        {
            InitializeComponent(); 
            _viewModel = new ColaboradoresCursosViewModel();
            DataContext = _viewModel;
        }
        #endregion

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModelParent"></param>
        public void AtualizarDados(Model.ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados(entity, viewModelParent);
        }

        private void OnUpLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _viewModel.Entity.Arquivo = arq.FormatoBase64;
                _viewModel.Entity.NomeArquivo = arq.Nome;
                NomeArquivo_tb.Text = arq.Nome;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        private void OnAbrirArquivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var arrayByes = Convert.FromBase64String(_viewModel.Entity.Arquivo);
                WpfHelp.AbrirArquivoPdf(_viewModel.Entity.NomeArquivo, arrayByes);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void OnFormatDate_LostFocus(object sender, RoutedEventArgs e)
        {
            var str = txtDate.Text;
            txtDate.Text = str.FormatarData(); 

        }
    }
}
