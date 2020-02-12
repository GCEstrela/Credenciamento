// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IMOD.CredenciamentoDeskTop.Enums;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.CredenciamentoDeskTop.Windows;
using Genetec.Sdk;
using System.Collections.Generic;
using System.Windows.Data;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    ///     Interação lógica para ColaboradorCredencialView.xam
    /// </summary>
    public partial class ColaboradoresCredenciaisView : UserControl
    {
        private readonly ColaboradoresCredenciaisViewModel _viewModel;

        #region Inicializacao

        public ColaboradoresCredenciaisView()
        {
            try
            {
                InitializeComponent();
                _viewModel = new ColaboradoresCredenciaisViewModel();
                DataContext = _viewModel;
                _viewModel.HabilitarVias = "Collapsed";
                _viewModel.ExibeCursosArea = "Collapsed";
            }
            catch (Exception ex)
            {
                //WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }

        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {
            //if (entity == null) return;
            
            _viewModel.AtualizarDados(entity, viewModelParent);
        }



        private void EmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.ColaboradorEmpresa == null) return;
                _viewModel.ObterValidade();

            if(_viewModel.Entity==null) return;
                _viewModel.CarregarCaracteresColete(_viewModel.ColaboradorEmpresa);
                _viewModel.Entity.Terceirizada = _viewModel.ColaboradorEmpresa.Terceirizada;
                _viewModel.Entity.TerceirizadaSigla = _viewModel.ColaboradorEmpresa.TerceirizadaSigla;
                if (_viewModel.Entity.Terceirizada)
                {
                    SigleColete_tb.Text = _viewModel.ColaboradorEmpresa.TerceirizadaSigla;
                    _viewModel.Entity.EmpresaSigla= _viewModel.ColaboradorEmpresa.TerceirizadaSigla;
            }

                if (_viewModel.ColaboradorEmpresa.ColaboradorId > 0 & _viewModel.ColaboradorEmpresa.EmpresaId > 0)
                {
                    _viewModel.CarregarVinculosAtivos(_viewModel.ColaboradorEmpresa.ColaboradorId, _viewModel.ColaboradorEmpresa.EmpresaId, _viewModel.ColaboradorEmpresa.EmpresaContratoId);
                }
            
        }

        private void TipoCredencial_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            if (_viewModel.ColaboradorEmpresa == null) return;
            _viewModel.ListarCracha(_viewModel.ColaboradorEmpresa.EmpresaId, _viewModel.Entity.TipoCredencialId);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEmpresaVinculo_cb.SelectionChanged += EmpresaVinculo_cb_SelectionChanged;
            cmbCredencialStatus.SelectionChanged += OnAlterarStatus_SelectonChanged;
            TipoCredencial_cb.SelectionChanged += TipoCredencial_cb_SelectionChanged;
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;

            //_viewModel.SelectListViewIndex = 0;
            //var original = PopUp_bt.Background;
            //if (cardholderGuids.Count != 0)
            //{
            //    PopUp_bt.Background = Brushes.Green;
            //}
            //else
            //{
            //    PopUp_bt.Background = original;
            //}
            ////if (ListaColaboradoresCredenciais_lv.Items.Count > 0)
            ////{
            ////    ListaColaboradoresCredenciais_lv.SelectedItems[0] = true;
            ////}
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
                if (string.IsNullOrWhiteSpace(str)) return;
                txtDtValidade.Text = str.FormatarData();

                DateTime dataEncontrada;
                TimeSpan diferenca = Convert.ToDateTime(txtDtValidade.Text) - DateTime.Now.Date;
                int credencialDias = int.Parse(diferenca.Days.ToString());

                if (_viewModel.Entity.TipoCredencialId == 1)
                {
                    if (credencialDias > 730)
                    {
                        dataEncontrada = DateTime.Now.AddDays(730);
                        str = dataEncontrada.ToString();
                        txtDtValidade.Text = str;
                        WpfHelp.PopupBox("Validade da credencial PERMANENTE, não pode ser superior a 2 anos!", 1);
                    }
                    else
                    {
                        txtDtValidade.Text = str.FormatarData();
                        _viewModel.HabilitaImpressao = true;
                    }

                }
                else if (_viewModel.Entity.TipoCredencialId == 2)
                {
                    if (credencialDias > 90)
                    {
                        dataEncontrada = DateTime.Now.AddDays(90);
                        str = dataEncontrada.ToString();
                        txtDtValidade.Text = str;
                        WpfHelp.PopupBox("Validade da credencial TEMPORÁRIA, não pode ser superior a 90 dias!", 1);
                    }
                    else
                    {
                        txtDtValidade.Text = str.FormatarData();
                        _viewModel.HabilitaImpressao = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _viewModel.Entity.SetMessageErro("Validade", "Data inválida");
            }
        }

        #endregion

        private void OnAlterarStatus_SelectonChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnImprimirCredencial.IsEnabled = true;
                _viewModel.ContentImprimir = "Imprimir Credencial";
                Image img = new Image();

                if (_viewModel.HabilitaImpressao)
                {

                    img.Source = new BitmapImage(new Uri(@"../Resources/CardPrinter.png", UriKind.Relative));
                    btnImprimirCredencial.Content = img;
                    btnImprimirCredencial.ToolTip = "Imprimir Credencial";
                }
                else
                {
                    img.Source = new BitmapImage(new Uri(@"../Resources/Olho.png", UriKind.Relative));
                    btnImprimirCredencial.Content = img;
                    btnImprimirCredencial.ToolTip = "Visualizar Credencial";
                }

                if (_viewModel.ColaboradorEmpresa == null) return;
                if (_viewModel.ColaboradorEmpresa.ColaboradorId > 0 & _viewModel.ColaboradorEmpresa.EmpresaId > 0)
                {
                    _viewModel.CarregarVinculosAtivosOutrasCredenciais(_viewModel.ColaboradorEmpresa.ColaboradorId, _viewModel.ColaboradorEmpresa.EmpresaId, _viewModel.ColaboradorEmpresa.EmpresaContratoId);
                }

                if (cmbCredencialStatus.SelectedItem != null &&
                            (((CredencialStatus)cmbCredencialStatus.SelectedItem).CredencialStatusId == 1))
                {
                    chkDevolucaoMotivo.IsChecked = false;
                    chkDevolucaoMotivo.Content = String.Empty;
                    chkDevolucaoMotivo.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbMotivacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.Motivacao_Select();
            this.lblCredencialVia.Content = _viewModel._viaAdicional;

            if (cmbCredencialStatus.SelectedItem != null && cmbMotivacao.SelectedItem != null)
            {
                if (((CredencialStatus)cmbCredencialStatus.SelectedItem).CredencialStatusId == 2
                                      && ((CredencialMotivo)cmbMotivacao.SelectedItem).CredencialMotivoId > 0)
                {
                    switch (((CredencialMotivo)cmbMotivacao.SelectedItem).CredencialMotivoId)
                    {
                        case 6:
                        case 8:
                        case 15:
                            chkDevolucaoMotivo.Content = DevoluçãoCredencial.Devolucao.Descricao();
                            chkDevolucaoMotivo.Visibility = Visibility.Visible;
                            break;
                        case 9:
                        case 10:
                        case 18:
                            chkDevolucaoMotivo.Content = DevoluçãoCredencial.EntregaBO.Descricao();
                            chkDevolucaoMotivo.Visibility = Visibility.Visible;
                            break;
                        default:
                            chkDevolucaoMotivo.IsChecked = false;
                            chkDevolucaoMotivo.Content = String.Empty;
                            chkDevolucaoMotivo.Visibility = Visibility.Hidden;
                            break;
                    }
                }
                else
                {
                    chkDevolucaoMotivo.IsChecked = false;
                    chkDevolucaoMotivo.Content = String.Empty;
                    chkDevolucaoMotivo.Visibility = Visibility.Hidden;
                }
            }
        }

        private void TecnologiaCredencial_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TecnologiaCredencial_cb.SelectedItem != null)
            {
                if (_viewModel.HabilitarOpcoesCredencial)
                {
                    FormatoCredencial_cb.IsEnabled = (!((IMOD.Domain.Entities.TecnologiaCredencial)TecnologiaCredencial_cb.SelectedItem).Descricao.Equals("N/D"));
                }
                else
                {
                    FormatoCredencial_cb.IsEnabled = false;
                }


                if (((IMOD.Domain.Entities.TecnologiaCredencial)TecnologiaCredencial_cb.SelectedItem).Descricao.Equals("N/D"))
                {
                    FormatoCredencial_cb.SelectedIndex = 0;
                }
            }
        }

        private void FormatoCredencial_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!FormatoCredencial_cb.IsEnabled)
            {
                FC_tb.Visibility = Visibility.Hidden;
                lblFC.Visibility = Visibility.Hidden;
                NumeroCredencial_tb.Visibility = Visibility.Hidden;
                lblNumero.Visibility = Visibility.Hidden;
            }
            if (FormatoCredencial_cb.SelectedItem != null)
            {
                if (((IMOD.Domain.Entities.FormatoCredencial)FormatoCredencial_cb.SelectedItem).Descricao.Trim().Equals("CSN"))
                {
                    FC_tb.Visibility = Visibility.Hidden;
                    lblFC.Visibility = Visibility.Hidden;
                    NumeroCredencial_tb.Visibility = Visibility.Visible;
                    lblNumero.Visibility = Visibility.Visible;
                    NumeroCredencial_tb.Focus();
                }
                else if (((IMOD.Domain.Entities.FormatoCredencial)FormatoCredencial_cb.SelectedItem).Descricao.Trim().Equals("HID H10302 37 Bits"))
                {
                    FC_tb.Visibility = Visibility.Hidden;
                    lblFC.Visibility = Visibility.Hidden;
                    NumeroCredencial_tb.Visibility = Visibility.Visible;
                    lblNumero.Visibility = Visibility.Visible;
                    NumeroCredencial_tb.Focus();
                }
                else if (((IMOD.Domain.Entities.FormatoCredencial)FormatoCredencial_cb.SelectedItem).Descricao.Trim().Equals("N/D"))
                {
                    FC_tb.Visibility = Visibility.Hidden;
                    lblFC.Visibility = Visibility.Hidden;
                    NumeroCredencial_tb.Visibility = Visibility.Hidden;
                    lblNumero.Visibility = Visibility.Hidden;
                }
                else
                {
                    FC_tb.Visibility = Visibility.Visible;
                    lblFC.Visibility = Visibility.Visible;
                    NumeroCredencial_tb.Visibility = Visibility.Visible;
                    lblNumero.Visibility = Visibility.Visible;
                    //NumeroCredencial_tb.Focus();
                    FC_tb.Focus();
                }

            }
        }

        private void NumeroCredencial_tb_LostFocus(object sender, RoutedEventArgs e)
        {

            if (_viewModel.Entity == null) return;
            try
            {
               
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Cnpj", "CNPJ inválido");
            }
        }

        private void FC_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex.Message, 1);
            }
        }

        private void TipoCredencial_cb_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex.Message, 1);
            }
        }        
        private void Lista_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key.ToString() == "F5")
                {
                    if (_viewModel.Entity != null)
                    {
                        _viewModel.ListarAtualizar(_viewModel.Entity.EmpresaId, 1);
                    }
                    
                    if (ListaColaboradoresCredenciais_lv.SelectedItems.Count > 0)
                    {
                        ListaColaboradoresCredenciais_lv.SelectedItems[0] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Privilegio1_tb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.ValidaCursosDoColaboradorParaAreaSelecionada();
        }
    }
}