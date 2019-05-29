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
            txtPesquisa.Focus();
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
            fake1.IsChecked = false;
            if (_viewModel.Entity == null) return;
            //Atualizar dados ao selecionar uma linha da listview
            _viewModel.AtualizarDadosPendencias();
            _viewModel.AtualizarDadosTiposAtividades();
            _viewModel.AtualizarDadosTipoCrachas();
            if (_viewModel.Entity!=null)
                _viewModel.bucarLogo(_viewModel.Entity.EmpresaId);

            if (_viewModel.Entity != null)
                _viewModel.Entity.Cnpj = _viewModel.Entity.Cnpj.FormatarCnpj();
            //Popular User Controls
            //////////////////////////////////////////////////////////////
            RepresentanteUs.AtualizarDados(_viewModel.Entity, _viewModel);
            AnexoUs.AtualizarDados(_viewModel.Entity, _viewModel);
            EmpresaContratosUs.AtualizarDados(_viewModel.Entity, _viewModel);
            //////////////////////////////////////////////////////////////
            _viewModel.CarregarQuantidadeTipoCredencial();
        }

        /// <summary>
        ///     Remover tipo atividade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoverTipoAtividade_Click(object sender, RoutedEventArgs e)
        {
            if (lstBoxTipoAtividade.SelectedItem == null) return;
            var item = lstBoxTipoAtividade.SelectedItem;
            if (item == null) return;
            var idx = lstBoxTipoAtividade.Items.IndexOf(item);
            _viewModel.TiposAtividades.RemoveAt(idx); 
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
            _viewModel.TiposAtividades.Add(n1);
            _viewModel.TipoAtividade = null;
            //TipoAtividade_cb.SelectedIndex = 0;

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
            _viewModel.TiposLayoutCracha.Add(n1);
            _viewModel.TipoCracha = null;
        }

        /// <summary>
        ///     Remover tipo cracha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoverTipoCracha_Click(object sender, RoutedEventArgs e)
        {
            if (lstBoxLayoutCracha.SelectedItem == null) return;
            var idx = lstBoxLayoutCracha.Items.IndexOf(lstBoxLayoutCracha.SelectedItem);
            _viewModel.TiposLayoutCracha.RemoveAt(idx); 
        }

        #endregion

        #region Pendencias

        private void AbrirPendencias(int codigo, PendenciaTipo tipoPendecia)
        {
            try
            {
                if (_viewModel.Entity == null) return;
                var frm = new PopupPendencias();
                frm.Inicializa(codigo, _viewModel.Entity.EmpresaId, tipoPendecia);
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
            AbrirPendencias(21, PendenciaTipo.Empresa);
        }

        private void OnPendenciaRepresentantes_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(12, PendenciaTipo.Empresa);
        }

        private void OnPendenciaGeralContratos_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(14, PendenciaTipo.Empresa);
        }

        private void OnPendenciaGeralAnexos_Click(object sender, RoutedEventArgs e)
        {
            AbrirPendencias(24, PendenciaTipo.Empresa);
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
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, _viewModel.IsTamanhoImagem);
                if (arq == null) return;
                _viewModel.Entity.Logo = arq.FormatoBase64;
                var binding = BindingOperations.GetBindingExpression(Logo_im, Image.SourceProperty);
                binding?.UpdateTarget();

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }



        #endregion

        


        private void TxtCnpj_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                _viewModel.Entity.Cnpj = _viewModel.Entity.Cnpj.FormatarCnpj();
                var cnpj = _viewModel.Entity.Cnpj;
                if (!Utils.IsValidCnpj(cnpj)) throw new Exception();
                //_viewModel.Entity.Cnpj.FormatarCnpj();
                //Verificar existência de CPF
                if (_viewModel.ExisteCnpj())
                    _viewModel.Entity.SetMessageErro("Cnpj", "CNPJ já existe");

                txtCnpj.Text = cnpj;
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Cnpj", "CNPJ inválido");
            }
             
        }

        private void Sigla_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
                try
                {
                    var sigla = _viewModel.Entity.Sigla;
                    if (_viewModel.ExisteSigla())
                        _viewModel.Entity.SetMessageErro("Sigla", "Sigla já existe");

                    Sigla_tb.Text = sigla;
                }
                catch (Exception)
                {
                    _viewModel.Entity.SetMessageErro("Cnpj", "CNPJ inválido");
                }
        }
        
        private void LstView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (lstView.SelectedIndex > -1)
                {
                    int currentIndex = lstView.SelectedIndex;
                    int Sum = lstView.Items.Count;
                    if (currentIndex > Sum)
                        currentIndex -= Sum;
                    if (e.Key.ToString() != "Up")
                    {
                        if (currentIndex == lstView.Items.Count-1) return;
                        ((ListViewItem)(lstView.ItemContainerGenerator.ContainerFromIndex(currentIndex + 1))).Focus();
                    }
                    else
                    {
                        if (currentIndex == 0) return;
                        ((ListViewItem)(lstView.ItemContainerGenerator.ContainerFromIndex(currentIndex - 1))).Focus();
                    }
                }

                
            }
            catch (Exception ex)
            {
                //WpfHelp.Mbox(ex.ToString());
            }
        }

        private void Fake1_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.empresaFake = true;
                txtCnpj.Text = "00.000.000/0000-00";
                txtCnpj.IsEnabled = false;
            }
            catch (Exception ex)
            {

            }
        }

        private void Fake1_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewModel.empresaFake = false;
            txtCnpj.Text = "";
            txtCnpj.IsEnabled = true;
        }

        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            fake1.IsChecked = false;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            fake1.IsChecked = false;
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            fake1.IsChecked = false;
        }
    }
}