// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 07 - 2019
// ***********************************************************************

#region

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels;
using IMOD.CrossCutting;

#endregion

namespace iModSCCredenciamento.Views
{
    /// <summary>
    ///     Interaction logic for UsEmpresaContratoView.xaml
    /// </summary>
    public partial class UsEmpresaContratoView : UserControl
    {
        private readonly EmpresasContratosViewModel _viewModel;

        public UsEmpresaContratoView()
        {
            InitializeComponent();
            _viewModel = new EmpresasContratosViewModel();
            DataContext = _viewModel;
        }

        #region  Metodos

        private void Frm_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEstado.SelectionChanged += OnSelecionaMunicipio_SelectionChanged;
        }

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.EmpresaView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados (entity);
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
                var arrBytes = Convert.FromBase64String (arquivoStr);
                WpfHelp.DownloadArquivoDialog (_viewModel.Entity.NomeArquivo, arrBytes);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void OnSelecionaMunicipio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.Estado == null) return;
            _viewModel.ListarMunicipios (_viewModel.Estado.Uf);
        }

        #endregion

        private void NumberOnly(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}