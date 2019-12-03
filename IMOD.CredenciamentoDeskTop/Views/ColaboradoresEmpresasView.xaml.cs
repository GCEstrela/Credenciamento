// ***********************************************************************
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
using IMOD.CredenciamentoDeskTop.Windows;
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
                Utils.TraceException(ex);
            }
        }
        #region  Metodos

        private void Frm_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEmpresa.SelectionChanged += OnSelecionaContrato_SelectionChanged;
            cmbContrato.SelectionChanged += OnSelecionaContratoEmpresa_SelectionChanged;
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }

        private void OnSelecionaContrato_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.Empresa == null) return;

            _viewModel.ListarContratos(_viewModel.Empresa);
            
            _viewModel.BuscarAnexo(_viewModel.Entity.ColaboradorEmpresaId);
            cmbContrato.Items.Refresh();
        }
        private void OnSelecionaContratoEmpresa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((IMOD.Domain.Entities.EmpresaContrato)((object[])e.AddedItems)[0]).Validade == null) return;
                _viewModel.Entity.Validade = ((IMOD.Domain.Entities.EmpresaContrato)((object[])e.AddedItems)[0]).Validade;
                
            }
            catch (Exception ex)
            {
                //throw;
            }
        }
        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {
            try
            {
                _viewModel.AtualizarDados(entity, viewModelParent);
            }
            catch (Exception ex)
            {

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
            try
            {
                txtDtFim.Text = DateTime.Today.Date.ToShortDateString();
            }
            catch (Exception ex)
            {

            }
            
        }
        PopUpGrupos popup;
        public System.Collections.Generic.List<Guid> cardholderGuids = new System.Collections.Generic.List<Guid>();
        private void Bnt_Gupos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                _viewModel.Entity.grupoAlterado = true;
                popup = new PopUpGrupos();
                
                if (cardholderGuids.Count != 0)
                {
                    popup.TCHG.CardHolderGroupGuid = cardholderGuids;
                }

                _viewModel.Entity.CardHolderGuid =_viewModel.EncontrarCardHolderGuid(_viewModel.Entity.ColaboradorId, _viewModel.Entity.ColaboradorEmpresaId);
                if (!string.IsNullOrEmpty(_viewModel.Entity.CardHolderGuid))
                {
                    popup.TCHG.CardHolderGroupGuid.Clear();
                    popup.TCHG.CardHolderGuid = new Guid(_viewModel.Entity.CardHolderGuid); //
                }
                else
                {
                    popup.TCHG.CardHolderGroupGuid.Clear();
                    popup.TCHG.CardHolderGuid = new Guid("ea3586f7-b6b7-42cc-8cca-04ef2ce7ebe8");
                }
                popup.TCHG.Initialize(Main.Engine);
                popup.ShowDialog();
                cardholderGuids = popup.TCHG.CardHolderGroupGuid;
                _viewModel.Entity.listadeGrupos = cardholderGuids;

            }
            catch (Exception ex)
            {

                //throw;
            }
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key.ToString() == "F5")
                {
                    _viewModel.AtualizarConfiguracoes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListaSegnatarios_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _viewModel.Entity.grupoAlterado = false;                
            }
            catch (Exception)
            {
                //throw;
            }
        }
    }
}