
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
using IMOD.CredenciamentoDeskTop.Enums;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
{
   
    /// <summary>
    ///     Interação lógica para VeiculoView.xam
    /// </summary>
    public partial class VeiculoView : UserControl
    {
        private readonly   VeiculoViewModel _viewModel;

        public VeiculoView()
        {
            InitializeComponent();
           _viewModel = new VeiculoViewModel();
            DataContext = _viewModel;
        }

        #region  Metodos

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OnListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Atualizar dados ao selecionar uma linha da listview
            _viewModel.AtualizarDadosPendencias();
            _viewModel.AtualizarDadosTiposServico();
            _viewModel.AtualizarDadosTiposServico();
            _viewModel.BucarFoto(_viewModel.Entity.EquipamentoVeiculoId);
            //Popular User Controls
            //////////////////////////////////////////////////////////////
            VeiculosEmpresasUs.AtualizarDados(_viewModel.Entity, _viewModel);
            EmpresaSeguroUs.AtualizarDados(_viewModel.Entity, _viewModel);
            AnexoUs.AtualizarDados(_viewModel.Entity, _viewModel);
            VeiculoCredenciaisUs.AtualizarDados(_viewModel.Entity, _viewModel);
            /////////////////////////////////////////////////////////////
            //_viewModel.IsEnableTabItem = true;
        }

        private void OnSelecionaMunicipio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.Estado == null) return;
            _viewModel.ListarMunicipios(_viewModel.Estado.Uf);
        }

        private void Frm_Loaded(object sender, RoutedEventArgs e)
        {
             lstView.SelectionChanged += OnListView_SelectionChanged;
            cmbEstado.SelectionChanged += OnSelecionaMunicipio_SelectionChanged;
            txtPesquisa.Focus();
        }

        /// <summary>
        ///     Remover tipo serviço
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoverTipoServico_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TipoServico == null) return;
            var item = lstBoxTipoAtividade.SelectedItem;
            if (item == null) return ;
            var idx =  lstBoxTipoAtividade.Items.IndexOf(item) ;
            _viewModel.TiposEquipamentoServico.RemoveAt(idx);
        }

        /// <summary>
        ///     Adicionar Tipo serviço
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAdicionarTipoServico_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TipoServico == null) return;

            var n1 = new EquipamentoVeiculoTipoServicoView
            {
                EquipamentoVeiculoId = _viewModel.Entity.EquipamentoVeiculoId,
                TipoServicoId = _viewModel.TipoServico.TipoServicoId,
                Descricao = _viewModel.TipoServico.Descricao
            };
            _viewModel.TiposEquipamentoServico.Add(n1);
        }

        private void AbrirPendencias(int codigo, PendenciaTipo tipoPendecia)
        {
            try
            {
                if (_viewModel.Entity == null) return;

                var frm = new PopupPendencias();
                frm.Inicializa(codigo, _viewModel.Entity.EquipamentoVeiculoId, tipoPendecia);
                frm.ShowDialog();
                _viewModel.AtualizarDadosPendencias();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void OnSelecionaFoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro);
                if (arq == null) return;
                _viewModel.Entity.Foto = arq.FormatoBase64;
                var binding = BindingOperations.GetBindingExpression(Logo_im, Image.SourceProperty);
                binding?.UpdateTarget();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void OnPendenciaGeralAnexos_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(24, PendenciaTipo.Veiculo);
        }

        private void OnPendenciaVinculos_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(22, PendenciaTipo.Veiculo);
        }

        private void OnPendenciaSeguros_Click(object sender, RoutedEventArgs e)
        {
           AbrirPendencias(19, PendenciaTipo.Veiculo);
        }

        private void OnPendenciaCredencias_Click(object sender, RoutedEventArgs e)
        {
             AbrirPendencias(25, PendenciaTipo.Veiculo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPendenciaGeral_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(21, PendenciaTipo.Veiculo);
        }

        #endregion

        
    }
}