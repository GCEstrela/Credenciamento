using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            //if (entity == null) return;
            _viewModel.AtualizarDados(entity, viewModelParent);
        }

        private void OnUpLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                //var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                var filtro = "Imagem files (*.pdf)|*.pdf";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, _viewModel.IsTamanhoArquivo);
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
                _viewModel.BuscarAnexo(_viewModel.Entity.ColaboradorCursoId);
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
            try
            {
                var str = txtDate.Text;
                txtDate.Text = str.FormatarData();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Validade", "Data inválida");
            }

        }

        private void OnFrm_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCurso.Focus();
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        
    }
}
