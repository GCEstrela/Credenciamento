// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

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
    ///     Interação lógica para EmpresasSegnatariosView.xam
    /// </summary>
    public partial class EmpresasSignatariosView : UserControl
    {
        private readonly EmpresasSignatariosViewModel _viewModel;

        public EmpresasSignatariosView()
        {
            InitializeComponent();
            _viewModel = new EmpresasSignatariosViewModel();
            DataContext = _viewModel;
        }

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.EmpresaView entity,EmpresaViewModel viewModelParent)
        {
            //if (entity == null) return;
            _viewModel.AtualizarDados(entity, viewModelParent);
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
                //var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                var filtro = "Imagem files (*.pdf)|*.pdf";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, _viewModel.IsTamanhoArquivo);
                if (arq == null) return;
                _viewModel.Entity.Assinatura = arq.FormatoBase64;
                NomeArquivo.Text = arq.Nome;
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
        private void OnAbrirArquivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.BuscarAnexo(_viewModel.Entity.EmpresaSignatarioId);
                var arrayByes = Convert.FromBase64String(_viewModel.Entity.Assinatura);
                WpfHelp.AbrirArquivoPdf(_viewModel.Entity.Nome, arrayByes);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        #endregion

        private void OnFormatCpf_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            var cpf = _viewModel.Entity.Cpf.FormatarCpf();
            txtCpf.Text = cpf;

        }

        private void ListaSignatarios_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (_viewModel.Entity == null) return;
                _viewModel.BuscarAnexo(_viewModel.Entity.EmpresaSignatarioId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
 