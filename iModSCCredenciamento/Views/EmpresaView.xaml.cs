// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using iModSCCredenciamento.Enums;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels;
using iModSCCredenciamento.Views.Model;
using iModSCCredenciamento.Windows;
using IMOD.CrossCutting;

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
            _viewModel.ListarMunicipios (_viewModel.Estado.Uf);
        }

        private void OnListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Atualizar dados ao selecionar uma linha da listview
            _viewModel.AtualizarDadosPendencias();
            _viewModel.AtualizarDadosTiposAtividades();
            _viewModel.AtualizarDadosTipoCrachas();
            //Popular User Controls
            RepresentanteUs.AtualizarDados(_viewModel.Entity);
            AnexoUs.AtualizarDados(_viewModel.Entity);
            EmpresaContratosUs.AtualizarDados(_viewModel.Entity);



        }

        /// <summary>
        ///     Remover tipo atividade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoverTipoAtividade_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TipoAtividade == null) return;
            var idx = lstBoxTipoAtividade.Items.IndexOf (lstBoxTipoAtividade.SelectedItem);
            _viewModel.TiposAtividades.RemoveAt (idx);
        }

        /// <summary>
        ///     Adicionar Tipo Atividade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAdicionarTipoAtividade_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TipoAtividade == null) return;
            var n1 = new EmpresaTipoAtividadeView
            {
                TipoAtividadeId = _viewModel.TipoAtividade.TipoAtividadeId,
                Descricao = _viewModel.TipoAtividade.Descricao
            };
            _viewModel.TiposAtividades.Add (n1);
        }

        /// <summary>
        ///     Adcionar cracha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAdicionarTipoCracha_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TipoCracha == null) return;
            var n1 = new IMOD.Domain.EntitiesCustom.EmpresaLayoutCrachaView
            {
                LayoutCrachaId = _viewModel.TipoCracha.LayoutCrachaId,
                Nome = _viewModel.TipoCracha.Nome
            };
            _viewModel.TiposLayoutCracha.Add (n1);
        }

        /// <summary>
        ///     Remover tipo cracha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoverTipoCracha_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TipoCracha == null) return;
            var idx = lstBoxLayoutCracha.Items.IndexOf (lstBoxLayoutCracha.SelectedItem);
            _viewModel.TiposLayoutCracha.RemoveAt (idx);
        }

        #endregion

        #region Pendencias

        private void AbrirPendencias(int codigo, PendenciaTipo tipoPendecia)
        {
            try
            {
                if (_viewModel.Entity == null) return;
                var frm = new PopupPendencias();
                frm.Inicializa (codigo, _viewModel.Entity.EmpresaId, tipoPendecia);
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
                if (_viewModel.Entity == null) return;
                txtCnpj.Text = _viewModel.Entity.Cnpj.FormatarCnpj();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox ($"Não foi realizar a operação solicitada\n{ex.Message}", 3);
            }
        }

        private void OnSelecionaFoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro);
                if (arq == null) return;
                _viewModel.Entity.Logo = arq.FormatoBase64;
                var binding = BindingOperations.GetBindingExpression(Logo_im, Image.SourceProperty);
                binding?.UpdateTarget();

            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }


        #endregion

        private void TabGeral_tc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}