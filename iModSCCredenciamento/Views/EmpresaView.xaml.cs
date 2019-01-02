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
using System.Windows.Data;
using System.Windows.Input;
using iModSCCredenciamento.Enums;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels;
using iModSCCredenciamento.Windows;
using IMOD.CrossCutting;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace iModSCCredenciamento.Views
{
    public partial class EmpresaView : UserControl
    {
        private readonly EmpresaViewModel _viewModel; 

        public EmpresaView()
        {
            InitializeComponent();
            _viewModel = new EmpresaViewModel();
            DataContext = _viewModel;
        }

        #region  Metodos

        private void Frm_Loaded(object sender, RoutedEventArgs e)
        {
            lstView.SelectionChanged += OnListView_SelectionChanged;
            cmbEstado.SelectionChanged += OnSelecionaMunicipio_SelectionChanged;
        }

        private void OnSelecionaMunicipio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.Estado == null) return;
            _viewModel.ListarMunicipios(_viewModel.Estado.Uf);
        }

        private void OnListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.AtualizarDadosPendencias();
            _viewModel.AtualizarDadosTiposAtividades();

        }

        #endregion

        #region Pendencias

        private void AbrirPendencias(int codigo, PendenciaTipo tipoPendecia)
        {
            try
            {
                if (_viewModel.Empresa == null) return;
                var frm = new PopupPendencias();
                frm.Inicializa (codigo, _viewModel.Empresa.EmpresaId, tipoPendecia);
                frm.ShowDialog();
                _viewModel.AtualizarDadosPendencias();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void OnPendenciaGeral_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias (21, PendenciaTipo.Empresa);
        }

        private void OnPendenciaRepresentantes_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias (12, PendenciaTipo.Empresa);
        }

        private void OnPendenciaGeralContratos_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias (14, PendenciaTipo.Empresa);
        }

        private void OnPendenciaGeralAnexos_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias (24, PendenciaTipo.Empresa);
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex ("[^0-9]+");
            e.Handled = regex.IsMatch (e.Text);
        }

        private void OnValidaCnpj_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.ValidarCnpj();
                if (_viewModel.Empresa == null) return;
                txtCnpj.Text = _viewModel.Empresa.Cnpj.FormatarCnpj();

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox($"Não foi realizar a operação solicitada\n{ex.Message}", 3);
            }
 
        }

        private void OnSelecionaFoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog (filtro);
                if (arq == null) return;
                _viewModel.Empresa.Logo = arq.FormatoBase64;
                //((EmpresaView)ListaEmpresas_lv.SelectedItem).Logo = arq.FormatoBase64;
                var binding = BindingOperations.GetBindingExpression(Logo_im, Image.SourceProperty);
                binding?.UpdateTarget();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        } 
        private void IncluirAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            //if (TipoAtividade_cb.Text != "" & TipoAtividade_cb.Text != "N/D")
            //{
            //    ((EmpresaViewModel)DataContext).OnInserirAtividadeCommand(TipoAtividade_cb.SelectedValue.ToString(), TipoAtividade_cb.Text);
            //    //TipoAtividade_cb.SelectedIndex = 0;
            //    TipoAtividade_cb.Text = "";

            //}
        }



        #endregion

        private void OnRemoverTipoAtividade_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TipoAtividade == null) return;
            var idx = lstBoxTipoAtividade.Items.IndexOf(lstBoxTipoAtividade.SelectedItem);
             _viewModel.TiposAtividades.RemoveAt(idx);
        }

        private void OnAdicionarTipoAtividade_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TipoAtividade == null) return;
            var n1 = new Model.EmpresaTipoAtividadeView
            {
                TipoAtividadeId = _viewModel.TipoAtividade.TipoAtividadeId,
                Descricao = _viewModel.TipoAtividade.Descricao
            };
            _viewModel.TiposAtividades.Add(n1);   
        }
    }
}