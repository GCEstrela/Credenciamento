// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IMOD.CredenciamentoDeskTop.Enums;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;
using iModSCCredenciamento.Windows;
using UserControl = System.Windows.Controls.UserControl;

#endregion
 

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    ///     Interação lógica para ColaboradorView.xam
    /// </summary>
    public partial class ColaboradorView : UserControl
    {
        private readonly ColaboradorViewModel _viewModel;
        public string _importarBNT = "Hidden";
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
            try
            {
                if (_viewModel.Estado == null) return;
                _viewModel.ListarMunicipios(_viewModel.Estado.Uf);
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
            }
        }

        private void OnListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Geral_ti.IsSelected = true;
                if (_viewModel.Entity == null) return;
                //Atualizar dados ao selecionar uma linha da listview
                _viewModel.AtualizarDadosPendencias();

                //Popular User Controls
                //////////////////////////////////////////////////////////////
                if (_viewModel.Entity != null)
                    _viewModel.BucarFoto(_viewModel.Entity.ColaboradorId);
                if (_viewModel.Entity != null)
                    _viewModel.Entity.Cpf = _viewModel.Entity.Cpf.FormatarCpf();

                //ColaboradorEmpresaUs.AtualizarDados(_viewModel.Entity, _viewModel);
                //ColaboradorCurso.AtualizarDados(_viewModel.Entity, _viewModel);
                //AnexoUs.AtualizarDados(_viewModel.Entity, _viewModel);
                //ColaboradoresCredenciaisUs.AtualizarDados(_viewModel.Entity, _viewModel);
                ////////////////////////////////////////////////////////////// 
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
            }
        }


        #endregion

        #region Pendencias

        private void AbrirPendencias(int codigo, PendenciaTipo tipoPendecia)
        {
            if (_viewModel.Entity == null) return;
            try
            { 
                var frm = new PopupPendencias();
                frm.Inicializa(codigo, _viewModel.Entity.ColaboradorId, tipoPendecia);
                frm.ShowDialog();
                _viewModel.AtualizarDadosPendencias();
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
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
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, _viewModel.IsTamanhoImagem);
                if (arq == null) return;
                _viewModel.Entity.Foto = arq.FormatoBase64;
                var binding = BindingOperations.GetBindingExpression(Logo_im, Image.SourceProperty);
                binding?.UpdateTarget();

            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
            }
        }

        #endregion
        private void OnFormatCpf_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                _viewModel.Entity.Cpf = _viewModel.Entity.Cpf.FormatarCpf();
                var cpf = _viewModel.Entity.Cpf;
                if (!Utils.IsValidCpf(cpf)) throw new Exception();
                //_viewModel.Entity.Cpf.FormatarCpf();
                //Verificar existência de CPF
                if (_viewModel.ExisteCpf())
                    _viewModel.Entity.SetMessageErro("Cpf", "CPF já existe");

                txtCpf.Text = cpf;

            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception)
            {
                 _viewModel.Entity.SetMessageErro ("Cpf", "CPF inválido");
            }           
        }
        private void OnFormatDateNascimento_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDateNascimento.Text;
                if (string.IsNullOrWhiteSpace (str)) return;
                txtDateNascimento.Text = str.FormatarData();
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("DataNascimento", "Data inválida");
            }
            
        }

        private void OnFormatEmissao_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDateEmissao.Text;
                if (string.IsNullOrWhiteSpace(str)) return;
                txtDateEmissao.Text = str.FormatarData();
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("DataEmissao", "Data inválida");
            }
           
        }

        private void OnFormatDateValidade_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDateValidade.Text;
                if (string.IsNullOrWhiteSpace(str)) return;
                txtDateValidade.Text = str.FormatarData();
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("CnhValidade", "Data inválida");
            }
            
        }

        private void OnFormatDatePassaporteValidade_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDatePassaporte.Text;
                if (string.IsNullOrWhiteSpace(str)) return;
                txtDatePassaporte.Text = str.FormatarData();
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception )
            {
                _viewModel.Entity.SetMessageErro("PassaporteValidade", "Data inválida");
            }
            
        }

        private void WbeCam_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                PopupWebCam _PopupWebCam = new PopupWebCam(_viewModel.IsResolucao);
                _PopupWebCam.ShowDialog();

                if (_PopupWebCam.aceitarImg)
                {
                    BitmapSource _img = _PopupWebCam.Captura;
                    if (_img != null)
                    {
                        string _imgstr = WpfHelp.IMGtoSTR(_img);
                        Logo_im.Source = _img;
                        _viewModel.Entity.Foto = _imgstr;
                    }
                }
                

            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
            }
        }

        private void LstView_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
                        if (currentIndex == lstView.Items.Count - 1) return;
                        ((ListViewItem)(lstView.ItemContainerGenerator.ContainerFromIndex(currentIndex + 1))).Focus();
                    }
                    else
                    {
                        if (currentIndex == 0) return;
                        ((ListViewItem)(lstView.ItemContainerGenerator.ContainerFromIndex(currentIndex - 1))).Focus();
                    }
                }
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
            }
        }

        //private void Precadastro_Checked(object sender, RoutedEventArgs e)
        //{
        //    _viewModel.IsEnablePreCadastro = precadastro.IsChecked.Value;
        //    _viewModel.IsEnablePreCadastroCredenciamento = false;
        //    _importarBNT = "Visible";
        //}

        //private void Precadastro_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    _viewModel.IsEnablePreCadastro = precadastro.IsChecked.Value;
        //    _viewModel.IsEnablePreCadastroCredenciamento = true;
        //    _importarBNT = "Hidden";
        //}

        private void Rd_precadastro_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel == null) return;
                //Geral_ti.IsSelected = true;
                _viewModel.EntityObserver.Clear();
                _viewModel.IsEnablePreCadastro = true;
                _viewModel.IsEnablePreCadastroCredenciamento = false;
                _viewModel.IsEnablePreCadastroColor = "Orange";
                _importarBNT = "Visible";
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
            }
        }

        private void Rd_cadastro_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel == null) return;
                _viewModel.EntityObserver.Clear();
                _viewModel.IsEnablePreCadastro = false;
                _viewModel.IsEnablePreCadastroCredenciamento = true;
                _viewModel.IsEnablePreCadastroColor = "#FFD0D0D0";
                _importarBNT = "Collapsed";
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
            }
        }
        private void Pesquisa_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtPesquisa.Focus();
                var num = _viewModel.PesquisarPor;
                if (num.Key == 6)
                {
                    _viewModel.Pesquisar();
                }
            }
            catch (SqlException ex)
            {
                WpfHelp.PopupBox(ex);
                _viewModel.Comportamento.PrepareCancelar();
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ColaboradorEmpresaUs.AtualizarDados(_viewModel.Entity, _viewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ColaboradorCurso.AtualizarDados(_viewModel.Entity, _viewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void TextBlock_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AnexoUs.AtualizarDados(_viewModel.Entity, _viewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void TextBlock_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ColaboradoresCredenciaisUs.AtualizarDados(_viewModel.Entity, _viewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}