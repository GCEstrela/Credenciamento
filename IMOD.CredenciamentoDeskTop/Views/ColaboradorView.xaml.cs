// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
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
            if (_viewModel.Entity == null) return;
            //Atualizar dados ao selecionar uma linha da listview
            _viewModel.AtualizarDadosPendencias();

            //Popular User Controls
            //////////////////////////////////////////////////////////////
            _viewModel.BucarFoto(_viewModel.Entity.ColaboradorId);
            _viewModel.Entity.Cpf =  _viewModel.Entity.Cpf.FormatarCpf();
            ColaboradorEmpresaUs.AtualizarDados(_viewModel.Entity, _viewModel);
            ColaboradorCurso.AtualizarDados(_viewModel.Entity, _viewModel);
            AnexoUs.AtualizarDados(_viewModel.Entity, _viewModel);
            ColaboradoresCredenciaisUs.AtualizarDados(_viewModel.Entity, _viewModel);
            ////////////////////////////////////////////////////////////// 
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
            catch (Exception )
            {
                _viewModel.Entity.SetMessageErro("PassaporteValidade", "Data inválida");
            }
            
        }

        private void WbeCam_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                PopupWebCam _PopupWebCam = new PopupWebCam();
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
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
       
    }
}