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
using System.Windows.Media;
using IMOD.CredenciamentoDeskTop.Enums;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.CredenciamentoDeskTop.Windows;
using Genetec.Sdk;
using System.Collections.Generic;

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
            _viewModel.AtualizarDados (entity, viewModelParent); 
        }

       

        private void EmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.ColaboradorEmpresa == null) return;
            //_viewModel.ListarCracha (_viewModel.ColaboradorEmpresa.EmpresaId);
            _viewModel.ObterValidade();
            _viewModel.CarregarCaracteresColete(_viewModel.ColaboradorEmpresa);

            
            if (_viewModel.ColaboradorEmpresa.ColaboradorId > 0 & _viewModel.ColaboradorEmpresa.EmpresaId > 0)
            {
                _viewModel.CarregarVinculosAtivos(_viewModel.ColaboradorEmpresa.ColaboradorId, _viewModel.ColaboradorEmpresa.EmpresaId);
            }

            //if (cmbEmpresaVinculo_cb.IsEnabled)
            //{
            //    _viewModel.HabilitaCriar(_viewModel.ColaboradorEmpresa,_viewModel);                
            //}
            //else
            //{
            //    _viewModel.Entity.ClearMessageErro();
            //}

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
                //if (credencialDias > 730)
                //{
                //    dataEncontrada = DateTime.Now.AddDays(730);
                //    str = dataEncontrada.ToString();
                //    txtDtValidade.Text = str;
                //}
                //else
                //{
                //    txtDtValidade.Text = str.FormatarData(); ;
                //}
                if (_viewModel.Entity.TipoCredencialId == 1)
                {
                    if (credencialDias > 730)
                    {
                        dataEncontrada = DateTime.Now.AddDays(730);
                        str = dataEncontrada.ToString();
                        txtDtValidade.Text = str;
                        WpfHelp.PopupBox("Validade da credencial PERMANENTE, não pode ser superior a 2 anos!",1);
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
                        WpfHelp.PopupBox("Validade da credencial TEMPORÁRIA, não pode ser superior a 90 dias!",1);
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

            btnImprimirCredencial.IsEnabled = true;
            _viewModel.ContentImprimir = "Imprimir Credencial";
            if (_viewModel.HabilitaImpressao)
            {
                btnImprimirCredencial.Content = "Imprimir Credencial";
                btnImprimirCredencial.ToolTip = "Imprimir Credencial";
            }
            else
            {
                btnImprimirCredencial.Content = "Visualizar Credencial";
                btnImprimirCredencial.ToolTip = "Visualizar Credencial";
            }

            if (_viewModel.ColaboradorEmpresa == null) return;
            if (_viewModel.ColaboradorEmpresa.ColaboradorId > 0 & _viewModel.ColaboradorEmpresa.EmpresaId > 0)
            {
                _viewModel.CarregarVinculosAtivosOutrasCredenciais(_viewModel.ColaboradorEmpresa.ColaboradorId, _viewModel.ColaboradorEmpresa.EmpresaId);
            }

            if (cmbCredencialStatus.SelectedItem != null &&
                        (((CredencialStatus)cmbCredencialStatus.SelectedItem).CredencialStatusId == 1))
            {
                chkDevolucaoMotivo.IsChecked = false;
                chkDevolucaoMotivo.Content = String.Empty;
                chkDevolucaoMotivo.Visibility = Visibility.Hidden;
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

                //int formatofredencialId = ((IMOD.Domain.Entities.FormatoCredencial)FormatoCredencial_cb.SelectedItem).FormatoCredencialId;
                //switch (formatofredencialId)
                //{
                //    case 1:
                //        FC_tb.ToolTip = "0 e 255";
                //        lblFC.ToolTip = "0 e 255";
                //        NumeroCredencial_tb.ToolTip = "0 e 65535";
                //        lblNumero.ToolTip = "0 e 65535";
                //        break;
                //    case 2:
                //        FC_tb.ToolTip = "0 e 65535";
                //        lblFC.ToolTip = "0 e 65535";
                //        NumeroCredencial_tb.ToolTip = "0 e 65535";
                //        lblNumero.ToolTip = "0 e 65535";
                //        break;
                //    case 3:
                //        //FC_tb.ToolTip = "Range entre 0 e 255";
                //        //lblFC.ToolTip = "Range entre 0 e 255";
                //        NumeroCredencial_tb.ToolTip = "0 e 34359738637";
                //        lblNumero.ToolTip = "0 e 34359738637";
                //        break;
                //    case 4:
                //        FC_tb.ToolTip = "0 e 65535";
                //        lblFC.ToolTip = "0 e 65535";
                //        NumeroCredencial_tb.ToolTip = "0 e 524287";
                //        lblNumero.ToolTip = "0 e 524287";
                //        break;
                //    case 5:
                //        FC_tb.ToolTip = "0 e 4095";
                //        lblFC.ToolTip = "0 e 4095";
                //        NumeroCredencial_tb.ToolTip = "0 e 1048575";
                //        lblNumero.ToolTip = "0 e 1048575";
                //        break;
                //    case 6:
                //        FC_tb.ToolTip = "0 e 4194303";
                //        lblFC.ToolTip = "0 e 4194303";
                //        NumeroCredencial_tb.ToolTip = "0 e 8388607";
                //        lblNumero.ToolTip = "0 e 8388607";
                //        break;
                //    case 7:
                //        NumeroCredencial_tb.ToolTip = "0 e 4294967295";
                //        lblNumero.ToolTip = "0 e 4294967295";
                //        break;
                //    default:
                //        //rengefc = 0;
                //        break;
                //}

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
                var nCredencial = _viewModel.Entity.NumeroCredencial;
                if (_viewModel.ExisteNumeroCredencial())
                {
                    _viewModel.Entity.SetMessageErro("NumeroCredencial", "Nº da Credencial já existe");
                    NumeroCredencial_tb.Text = nCredencial;
                }
                else
                {
                    _viewModel.Entity.ClearMessageErro();
                    //NumeroCredencial_tb.Background = Brushes.;
                    NumeroCredencial_tb.Text = nCredencial;
                }


                
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
                //if (_viewModel.Entity == null) return;
                //int tipo = ((IMOD.Domain.Entities.TipoCredencial)TecnologiaCredencial_cb.SelectedItem).TipoCredencialId;
                //int formato = ((IMOD.Domain.Entities.FormatoCredencial)FormatoCredencial_cb.SelectedItem).FormatoCredencialId;
                //int fc = Convert.ToInt32(FC_tb.Text);

                //int rangefc = 0;
                //if (!_viewModel.ValidaFC(tipo, formato, fc, out rangefc))
                //{
                //    WpfHelp.PopupBox("Para o formato selecionado o valor [FC] deve estar entre 0 e " + rangefc, 1);
                //    FC_tb.Focus();
                //}
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex.Message, 1);
            }
        }

        private void TipoCredencial_cb_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //    if(TipoCredencial_cb.SelectedItem == null) return;

            //    TecnologiaCredencial_cb.IsEnabled = true;
            //    if (((IMOD.Domain.Entities.TipoCredencial)TipoCredencial_cb.SelectedItem).Descricao.Trim().Equals("TEMPORÁRIA"))
            //    {
            //        TecnologiaCredencial_cb.IsEnabled = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    WpfHelp.PopupBox(ex.Message, 1);
            //}
        }
        private Engine m_sdkEngine = new Engine();
        PopUpGrupos popup;
        public System.Collections.Generic.List<Guid> cardholderGuids = new System.Collections.Generic.List<Guid>();
        private void PopUp_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (!m_sdkEngine.IsConnected)
                {
                    m_sdkEngine.ClientCertificate = "KxsD11z743Hf5Gq9mv3+5ekxzemlCiUXkTFY5ba1NOGcLCmGstt2n0zYE9NsNimv";
                    m_sdkEngine.LogOn("172.16.190.108", "Admin", "");
                }
                //_viewModel.CarregarListaGrupos(popup);
                popup = new PopUpGrupos();
                if (cardholderGuids.Count != 0)
                {
                    popup.TCHG.CardHolderGroupGuid = cardholderGuids;
                }

                //popup.TCHG.CardHolderGuid = new Guid(CardHolderGuid_tb.Text); //ea3586f7-b6b7-42cc-8cca-04ef2ce7ebe8
                if(_viewModel.Entity.CardHolderGuid != null)
                {
                    popup.TCHG.CardHolderGuid = new Guid(_viewModel.Entity.CardHolderGuid); //
                }
                else
                {
                    popup.TCHG.CardHolderGuid = new Guid("ea3586f7-b6b7-42cc-8cca-04ef2ce7ebe8");
                }
                popup.TCHG.Initialize(m_sdkEngine);
                popup.ShowDialog();
                cardholderGuids = popup.TCHG.CardHolderGroupGuid;
                _viewModel.Entity.listadeGrupos = cardholderGuids;
                //cagarlistaGrupo();


            }
            catch (Exception)
            {

                throw;
            }
        }
        private void cagarlistaGrupo()
        {
            try
            {
                List<Guid> groupos = new List<Guid>();
                //_viewModel.Entity.listadeGrupos.Clear();
                foreach (Guid cardholderGuid in cardholderGuids)
                {
                    Genetec.Sdk.Entities.CardholderGroup cardholdergroup = m_sdkEngine.GetEntity(cardholderGuid) as Genetec.Sdk.Entities.CardholderGroup;
                    //_viewModel.Entity.listadeGrupos += cardholdergroup.Name + ";";
                    groupos.Add(cardholdergroup.Guid);
                }
                _viewModel.Entity.listadeGrupos = groupos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Lista_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.Entity.listadeGrupos.Clear();
                foreach (Guid cardholderGuid in cardholderGuids)
                {
                    Genetec.Sdk.Entities.CardholderGroup cardholdergroup = m_sdkEngine.GetEntity(cardholderGuid) as Genetec.Sdk.Entities.CardholderGroup;
                    //_viewModel.Entity.listadeGrupos += cardholdergroup.Name + ";";
                    _viewModel.Entity.listadeGrupos.Add(cardholdergroup.Guid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}