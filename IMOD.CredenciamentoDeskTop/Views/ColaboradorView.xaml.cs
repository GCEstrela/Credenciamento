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
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;

#endregion
 

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    ///     Interação lógica para ColaboradorView.xam
    /// </summary>
    public partial class ColaboradorView : UserControl
    {
        private readonly ColaboradorViewModel _viewModel;

        public ColaboradorView()
        {
            InitializeComponent();
            _viewModel = new ColaboradorViewModel();
            DataContext = _viewModel;
        }

        #region  Metodos
        private void Frm_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEstado.SelectionChanged += OnSelecionaMunicipio_SelectionChanged;
            lstView.SelectionChanged += OnListView_SelectionChanged;
            txtPesquisa.Focus();
        }
        private void OnSelecionaMunicipio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.Estado == null) return;
            _viewModel.ListarMunicipios(_viewModel.Estado.Uf);
        }

        private void OnListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Atualizar dados ao selecionar uma linha da listview
            _viewModel.AtualizarDadosPendencias();

            //Popular User Controls
            //////////////////////////////////////////////////////////////
            ColaboradorEmpresaUs.AtualizarDados(_viewModel.Entity, _viewModel);
            ColaboradorCurso.AtualizarDados(_viewModel.Entity, _viewModel);
            AnexoUs.AtualizarDados(_viewModel.Entity, _viewModel);
            ColaboradoresCredenciaisUs.AtualizarDados(_viewModel.Entity);
            ////////////////////////////////////////////////////////////// 
        }
        

        #endregion

        #region Pendencias

        private void AbrirPendencias(int codigo, PendenciaTipo tipoPendecia)
        {
            try
            {
                if (_viewModel.Entity == null) return;
                var frm = new PopupPendencias();
                frm.Inicializa(codigo, _viewModel.Entity.ColaboradorId, tipoPendecia);
                frm.ShowDialog();
                _viewModel.AtualizarDadosPendencias();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void OnPendenciaGeral_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(21, PendenciaTipo.Colaborador);
        }

        private void OnPendenciaEmpresaVinculo_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(22, PendenciaTipo.Colaborador);
        }

        private void OnPendenciaTreinamentoCertificacao_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(23, PendenciaTipo.Colaborador);
        }

        private void OnPendenciaAnexos_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(24, PendenciaTipo.Colaborador);
        }

        private void OnPendenciaCredencial_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(25, PendenciaTipo.Colaborador);
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
 

        #endregion 
        
         

    }
}