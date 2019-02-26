using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class TermosViewModel : ViewModelBase
    {

        #region Variaveis Privadas

        private ObservableCollection<RelatorioView> _Relatorios;

        private RelatorioView _RelatorioSelecionado;

        PopupMensagem _PopupSalvando;

        private int _selectedIndex;

        private int _selectedIndexTemp = 0;

        private bool _atualizandoFoto = false;

        private BitmapImage _Waiting;

        private string formula;
        private string _mensagem;
        private string verbo;

        private readonly IRelatorioGerencialService _relatorioGerencialServiceService = new RelatorioGerencialService();

        private RelatoriosGerenciais termo = new RelatoriosGerenciais();

        #endregion

        #region Comandos dos Botoes 

        /// <summary>
        /// Filtrar Termos de Credenciais
        /// </summary>
        /// <param name="_report"></param>
        /// <param name="_status"></param>
        /// <param name="_periodo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltrosTermosCredenciaisCommand(int _report, int _status, int _periodo, string _dataIni, string _dataFim)
        {
            switch (_report)
            {
                case 11:
                    verbo = "concedeu";
                    break;
                case 15:
                    verbo = "indeferiu";
                    break;
                case 13:
                    verbo = "cancelou";
                    break;
                case 17:
                    verbo = "destruiu";
                    break;
                default:
                    verbo = "emitiu";
                    break;
            }

            try
            {
                termo = _relatorioGerencialServiceService.BuscarPelaChave(_report);

                //Filtro Hoje
                if (_periodo == 1)
                {
                    //Termos com status Vias Adicionais (1)
                    if (_report == 20 && _status == 1)
                    {
                        formula = " {ColaboradoresCredenciais.Emissao} = CurrentDate " +
                                  " and {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        _mensagem = "Na data " + DateTime.Now.ToShortDateString() + " " +
                                    "este setor de credenciamento do AEROPORTO INTERNACIONAL " +
                                    ", " + verbo + " as seguintes vias adicionais de credenciais:";
                    }

                    //Outros status (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = "{ColaboradoresCredenciais.Emissao} = CurrentDate ";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {ColaboradoresCredenciais.Baixa} = CurrentDate " +
                                      " and {ColaboradoresCredenciais.CredencialStatusID} = " + _status;
                        }


                        _mensagem = "Na data " + DateTime.Now.ToShortDateString() + " " +
                                    "este setor de credenciamento do AEROPORTO INTERNACIONAL " +
                                    ", " + verbo + " as seguintes credenciais:";
                    }
                }
                //Filtro últimos 7 dias
                else if (_periodo == 2)
                {
                    //Termo de Vias Adicionais
                    if (_report == 20 && _status == 1)
                    {
                        formula = " {ColaboradoresCredenciais.Emissao} >= CurrentDate-7 " +
                                  " and {ColaboradoresCredenciais.Emissao} <= CurrentDate " +
                                  " and {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-7).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL , " + verbo +
                                    " as seguintes vias adicionais de credenciais:";
                    }

                    //Outros termos (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = " {ColaboradoresCredenciais.Emissao} >= CurrentDate-7" +
                                      " and {ColaboradoresCredenciais.Emissao} <= CurrentDate ";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {ColaboradoresCredenciais.Baixa} >= CurrentDate-7" +
                                      " and {ColaboradoresCredenciais.Baixa} <= CurrentDate " +
                                      " and {ColaboradoresCredenciais.CredencialStatusID} = " + _status;
                        }

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-7).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL , " + verbo +
                                    " as seguintes vias adicionais de credenciais:";
                    }
                }
                //Filtro Último Mês
                else if (_periodo == 3)
                {
                    //Termo Vias Adicionais
                    if (_report == 20 && _status == 1)
                    {
                        formula = " {ColaboradoresCredenciais.Emissao} >= CurrentDate-30 " +
                                  " and {ColaboradoresCredenciais.Emissao} <= CurrentDate " +
                                  " and {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-30).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL , " + verbo +
                                    " as seguintes vias adicionais de credenciais:";
                    }

                    //Outros termos (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = " {ColaboradoresCredenciais.Emissao} >= CurrentDate-30 " +
                                      " and {ColaboradoresCredenciais.Emissao} <= CurrentDate ";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {ColaboradoresCredenciais.Baixa} >= CurrentDate-30 " +
                                      " and {ColaboradoresCredenciais.Baixa} <= CurrentDate " +
                                      " and {ColaboradoresCredenciais.CredencialStatusID} = " + _status;
                        }

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-30).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL , " + verbo +
                                    " as seguintes vias adicionais de credenciais:";
                    }
                }
                //Filtro por Período Determinado (Entre datas)
                else
                {
                    //Termo Vias Adicionais
                    if (_report == 20 && _status == 1)
                    {
                        formula = "  {CredenciaisMotivos.CredencialmotivoID} in [2,3]" +
                                  " and {ColaboradoresCredenciais.Emissao} >= cdate('" + _dataIni + "')" +
                                  " and {ColaboradoresCredenciais.Emissao} <= cdate('" + _dataFim + "') ";


                        _mensagem = "Durante o período de " + _dataIni + " a " + _dataFim + " " +
                                    "esse setor de credenciamento do AEROPORTO INTERNACIONAL , " +
                                    "" + verbo + " as seguintes vias adicionais de credenciais:";
                    }

                    //Outros termos (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = " {ColaboradoresCredenciais.Emissao} <= cdate('" + _dataFim + "') " +
                                      " and {ColaboradoresCredenciais.Emissao} >= cdate('" + _dataIni + "')";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {ColaboradoresCredenciais.Baixa} >= cdate('" + _dataIni + "')" +
                                      " and {ColaboradoresCredenciais.Baixa} <= cdate('" + _dataFim + "') " +
                                      " and {ColaboradoresCredenciais.CredencialStatusID} = " + _status;
                        }

                        _mensagem = "Durante o período de " + _dataIni + " a " + _dataFim + " " +
                                    "esse setor de credenciamento do AEROPORTO INTERNACIONAL , " +
                                    "" + verbo + " as seguintes credenciais:";
                    }

                }
                var arrayFile = Convert.FromBase64String(termo.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "Termo ", formula, _mensagem);
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        /// <summary>
        /// Filtrar Termos de Credenciais
        /// </summary>
        /// <param name="_report"></param>
        /// <param name="_status"></param>
        /// <param name="_periodo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltrosTermosAutorizacoesCommand(int _report, int _status, int _periodo, string _dataIni, string _dataFim)
        {
            switch (_report)
            {
                case 12:
                    verbo = "concedeu";
                    break;
                case 16:
                    verbo = "indeferiu";
                    break;
                case 14:
                    verbo = "cancelou";
                    break;
                case 18:
                    verbo = "destruiu";
                    break;
                default:
                    verbo = "emitiu";
                    break;
            }

            try
            {
                termo = _relatorioGerencialServiceService.BuscarPelaChave(_report);

                //Filtro Hoje
                if (_periodo == 1)
                {
                    //Termos com status Vias Adicionais (1)
                    if (_report == 20 && _status == 1)
                    {
                        formula = " {VeiculosCredenciais.Emissao} = CurrentDate " +
                                  " and {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        _mensagem = "Na data " + DateTime.Now.ToShortDateString() + " " +
                                    "este setor de credenciamento do AEROPORTO INTERNACIONAL " +
                                    ", " + verbo + " as seguintes vias adicionais de autorizações:";
                    }

                    //Outros status (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = "{VeiculosCredenciais.Emissao} = CurrentDate ";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {VeiculosCredenciais.Baixa} = CurrentDate " +
                                      " and {VeiculosCredenciais.CredencialStatusID} = " + _status;
                        }


                        _mensagem = "Na data " + DateTime.Now.ToShortDateString() + " " +
                                    "este setor de credenciamento do AEROPORTO INTERNACIONAL " +
                                    ", " + verbo + " as seguintes autorizações:";
                    }
                }
                //Filtro últimos 7 dias
                else if (_periodo == 2)
                {
                    //Termo de Vias Adicionais
                    if (_report == 20 && _status == 1)
                    {
                        formula = " {VeiculosCredenciais.Emissao} >= CurrentDate-7 " +
                                  " and {VeiculosCredenciais.Emissao} <= CurrentDate " +
                                  " and {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-7).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL , " + verbo +
                                    " as seguintes vias adicionais de autorizações:";
                    }

                    //Outros termos (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = " {VeiculosCredenciais.Emissao} >= CurrentDate-7" +
                                      " and {VeiculosCredenciais.Emissao} <= CurrentDate ";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {VeiculosCredenciais.Baixa} >= CurrentDate-7" +
                                      " and {VeiculosCredenciais.Baixa} <= CurrentDate " +
                                      " and {VeiculosCredenciais.CredencialStatusID} = " + _status;
                        }

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-7).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL , " + verbo +
                                    " as seguintes vias adicionais de autorizações:";
                    }
                }
                //Filtro Último Mês
                else if (_periodo == 3)
                {
                    //Termo Vias Adicionais
                    if (_report == 20 && _status == 1)
                    {
                        formula = " {VeiculosCredenciais.Emissao} >= CurrentDate-30 " +
                                  " and {VeiculosCredenciais.Emissao} <= CurrentDate " +
                                  " and {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-30).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL , " + verbo +
                                    " as seguintes vias adicionais de autorizações:";
                    }

                    //Outros termos (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = " {VeiculosCredenciais.Emissao} >= CurrentDate-30 " +
                                      " and {VeiculosCredenciais.Emissao} <= CurrentDate ";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {VeiculosCredenciais.Baixa} >= CurrentDate-30 " +
                                      " and {VeiculosCredenciais.Baixa} <= CurrentDate " +
                                      " and {VeiculosCredenciais.CredencialStatusID} = " + _status;
                        }

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-30).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL , " + verbo +
                                    " as seguintes vias adicionais de autorizações:";
                    }
                }
                //Filtro por Período Determinado (Entre datas)
                else
                {
                    //Termo Vias Adicionais
                    if (_report == 20 && _status == 1)
                    {
                        formula = "  {CredenciaisMotivos.CredencialmotivoID} in [2,3]" +
                                  " and {VeiculosCredenciais.Emissao} >= cdate('" + _dataIni + "')" +
                                  " and {VeiculosCredenciais.Emissao} <= cdate('" + _dataFim + "') ";


                        _mensagem = "Durante o período de " + _dataIni + " a " + _dataFim + " " +
                                    "esse setor de credenciamento do AEROPORTO INTERNACIONAL , " +
                                    "" + verbo + " as seguintes vias adicionais de autorizações:";
                    }

                    //Outros termos (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = " {VeiculosCredenciais.Emissao} <= cdate('" + _dataFim + "') " +
                                      " and {VeiculosCredenciais.Emissao} >= cdate('" + _dataIni + "')";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {VeiculosCredenciais.Baixa} >= cdate('" + _dataIni + "')" +
                                      " and {VeiculosCredenciais.Baixa} <= cdate('" + _dataFim + "') " +
                                      " and {VeiculosCredenciais.CredencialStatusID} = " + _status;
                        }

                        _mensagem = "Durante o período de " + _dataIni + " a " + _dataFim + " " +
                                    "esse setor de credenciamento do AEROPORTO INTERNACIONAL , " +
                                    "" + verbo + " as seguintes autorizações:";
                    }

                }
                var arrayFile = Convert.FromBase64String(termo.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "Termo ", formula, _mensagem);
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        #endregion


    }
}
