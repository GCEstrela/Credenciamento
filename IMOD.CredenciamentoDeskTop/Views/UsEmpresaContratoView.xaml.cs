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
        public void AtualizarDados(Model.EmpresaView entity, EmpresaViewModel viewModelParent)
        {
            try
            {
                //if (entity == null) return;
                _viewModel.AtualizarDados(entity, viewModelParent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                var arq = WpfHelp.UpLoadArquivoDialog(filtro,_viewModel.IsTamanhoArquivo);
                if (arq == null) return;
                _viewModel.Entity.Arquivo = arq.FormatoBase64;
                _viewModel.Entity.NomeArquivo = arq.Nome;
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
                _viewModel.BuscarAnexo(_viewModel.Entity.EmpresaContratoId);
                var arrayByes = Convert.FromBase64String(_viewModel.Entity.Arquivo);
                WpfHelp.AbrirArquivoPdf(_viewModel.Entity.NomeArquivo, arrayByes);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void OnSelecionaMunicipio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.Estado == null) return;
            _viewModel.ListarMunicipios(_viewModel.Estado.Uf);
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TipoCobranca_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TipoCobranca_cb.SelectedValue == null) return;

            var _tipocobranca = TipoCobranca_cb.SelectedValue;
            if (_tipocobranca.ToString() == "0")
            {
                IsencaoCobranca_cb.IsEnabled = true;
            }
            else
            {
                IsencaoCobranca_cb.IsEnabled = false;
                IsencaoCobranca_cb.IsChecked = false;
            }
        }

        private void OnFormatDateEmissao_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDateEmissao.Text;
                if (string.IsNullOrWhiteSpace(str)) return;
                txtDateEmissao.Text = str.FormatarData();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Emissao", "Data inválida");
            }
        }

        private void OnFormatData_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDtValidade.Text;
                if (string.IsNullOrWhiteSpace(str)) return;
                txtDtValidade.Text = str.FormatarData();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Validade", "Data inválida");
            }
        }

        #endregion

        private void Terceirizada_cb_Checked(object sender, RoutedEventArgs e)
        {
            lblNome.Visibility = Visibility.Hidden;
            Terceira_tb.Visibility = Visibility.Hidden;
            lblSigla.Visibility = Visibility.Hidden;
            txtSiglaTerceira.Visibility = Visibility.Hidden;
            bool terceiraCB = (bool)Terceira_cb.IsChecked;

            if (terceiraCB)
            {
                lblNome.Visibility = Visibility.Visible;
                Terceira_tb.Visibility = Visibility.Visible;
                lblSigla.Visibility = Visibility.Visible;
                txtSiglaTerceira.Visibility = Visibility.Visible;
            }
        }

        private void ListaContratos_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (_viewModel.Entity == null) return;

                lblNome.Visibility = Visibility.Hidden;
                Terceira_tb.Visibility = Visibility.Hidden;
                lblSigla.Visibility = Visibility.Hidden;
                txtSiglaTerceira.Visibility = Visibility.Hidden;
                
                if (_viewModel.Entity.Terceirizada)
                {
                    lblNome.Visibility = Visibility.Visible;
                    Terceira_tb.Visibility = Visibility.Visible;
                    lblSigla.Visibility = Visibility.Visible;
                    txtSiglaTerceira.Visibility = Visibility.Visible;
                }
                _viewModel.BuscarAnexo(_viewModel.Entity.EmpresaContratoId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}