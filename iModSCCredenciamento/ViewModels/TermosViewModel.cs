using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

namespace iModSCCredenciamento.ViewModels
{
    public class TermosViewModel : ViewModelBase
    {

        #region Variaveis Privadas

        private ObservableCollection<ClasseRelatorios.Relatorio> _Relatorios;

        private ClasseRelatorios.Relatorio _RelatorioSelecionado;

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

        //TODO: OnFiltrosTermosCommand (Ajustar fórmulas/relatórios) - Mihai (06/12/2018)
        public void OnFiltrosTermosCommand(int _report, int _status, int _periodo, string _dataIni, string _dataFim)
        {
            string _xmlstring;

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
                        formula = " {ColaboradoresCredenciais.Emissao} = CurrentDate " +
                                  " and {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        _mensagem = "Na data " + DateTime.Now.ToShortDateString() + " " +
                                    "este setor de credenciamento do AEROPORTO INTERNACIONAL " +
                                    "DE PORTO ALEGRE, " + verbo + " as seguintes vias adicionais de credenciais:";
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
                                    "DE PORTO ALEGRE, " + verbo + " as seguintes credenciais:";
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
                                    "AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " + verbo +
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
                                    "AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " + verbo +
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
                                    "AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " + verbo +
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
                                    "AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " + verbo +
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
                                    "esse setor de credenciamento do AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " +
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
                                    "esse setor de credenciamento do AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " +
                                    "" + verbo + " as seguintes credenciais:";
                    }

                }
                var arrayFile = Convert.FromBase64String(termo.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "Termo ", "", _mensagem);
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        #endregion


    }
}
