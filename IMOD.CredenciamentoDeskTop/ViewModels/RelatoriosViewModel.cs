using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;
using IMOD.Domain.Entities; 

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class RelatoriosViewModel : ViewModelBase
    {
        #region Inicializacao
        public RelatoriosViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
        }

        private void CarregaUI()
        {
            CarregaColecaoRelatorios();
            CarregaColecaoEmpresas();
            CarregaColecaoAreasAcessos();
        }

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<AreaAcessoView> _AreasAcessos;

        private ObservableCollection<EmpresaView> _Empresas;


        private ObservableCollection<RelatorioView> _Relatorios;

        private RelatorioView _RelatorioSelecionado;

        private RelatorioView _relatorioTemp = new RelatorioView();

        private List<RelatorioView> _RelatoriosTemp = new List<RelatorioView>();

        PopupMensagem _PopupSalvando;

        private int _selectedIndex;

        private int _selectedIndexTemp = 0;

        private bool _atualizandoFoto = false;

        private BitmapImage _Waiting;

        private string formula;
        private string verbo;
        private string mensagem;

        private readonly IRelatorioService _relatorioService = new RelatorioService();
        private readonly IRelatorioGerencialService _relatorioGerencialServiceService = new RelatorioGerencialService();
        private readonly IEmpresaService _empresaService = new EmpresaService();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();

        private Relatorios relatorio = new Relatorios();
        private RelatoriosGerenciais relatorioGerencial = new RelatoriosGerenciais();

        #endregion

        #region Contrutores

        public ObservableCollection<AreaAcessoView> AreasAcessos
        {
            get
            {
                return _AreasAcessos;
            }

            set
            {
                if (_AreasAcessos != value)
                {
                    _AreasAcessos = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<EmpresaView> Empresas

        {
            get
            {
                return _Empresas;
            }

            set
            {
                if (_Empresas != value)
                {
                    _Empresas = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<RelatorioView> Relatorios
        {
            get
            {
                return _Relatorios;
            }

            set
            {
                if (_Relatorios != value)
                {
                    _Relatorios = value;
                    OnPropertyChanged();

                }
            }
        }

        public RelatorioView RelatorioSelecionado
        {
            get
            {
                return _RelatorioSelecionado;
            }
            set
            {
                _RelatorioSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (RelatorioSelecionado != null)
                {

                }

            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public BitmapImage Waiting
        {
            get
            {
                return _Waiting;
            }
            set
            {
                _Waiting = value;
                base.OnPropertyChanged();
            }
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoRelatorios()
        {
            try
            {
                var list1 = _relatorioService.Listar();

                var list2 = Mapper.Map<List<RelatorioView>>(list1);
                var observer = new ObservableCollection<RelatorioView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Relatorios = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoAreasAcessos()
        {
            try
            {
                var list1 = _auxiliaresService.AreaAcessoService.Listar();

                var list2 = Mapper.Map<List<AreaAcessoView>>(list1);
                var observer = new ObservableCollection<AreaAcessoView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                AreasAcessos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoEmpresas(int? idEmpresa = null, string nome = null, string apelido = null, string cnpj = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                if (!string.IsNullOrWhiteSpace(cnpj)) cnpj = $"%{cnpj}%";

                var list1 = _empresaService.Listar(idEmpresa, nome, apelido, cnpj);
                var list2 = Mapper.Map<List<EmpresaView>>(list1.OrderByDescending(a => a.EmpresaId));

                var observer = new ObservableCollection<EmpresaView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Empresas = observer;
                SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

        #region Comandos dos Botoes

        public void OnAbrirRelatorioCommand(string _Tag)
        {
            try
            {
                relatorio = _relatorioService.BuscarPelaChave(Convert.ToInt32(_Tag));

                var arrayFile = Convert.FromBase64String(relatorio.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "Relatorio " + _Tag, formula, "");

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #region Comandos dos Botoes (RELATÓRIOS GERENCIAIS)

        /// <summary>
        /// Filtrar Relatório de Credenciais Permanentes/Temporárias
        /// </summary>
        /// <param name="_tipo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltroRelatorioCredencialCommand(bool _tipo, string _dataIni, string _dataFim)
        {
            try
            {
                //CREDENCIAIS PERMANENTES - (1)
                if (_tipo)
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(1);
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //Listar apenas Credenciais Permanentes e Ativas
                        formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 ";

                        mensagem = "Todas as CREDENCIAIS PERMANENTES ativas ";
                    }
                    else
                    {   //Listar Credenciais Permanentes e Ativas por período
                        formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                                  " and ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " and {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as CREDENCIAIS PERMANENTES ativas concedidas entre " + _dataIni + " e " + _dataFim + "";
                    }

                }
                //CREDENCIAIS TEMPORÁRIAS - (3)
                else
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(3);
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //Listar apenas Credenciais Temporárias e Ativas
                        formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 ";

                        mensagem = "Todas as CREDENCIAIS TEMPORÁRIAS ativas ";
                    }
                    else
                    {   //Listar Credenciais Permanentes e Ativas por período
                        formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                                  " and ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " and {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as CREDENCIAIS TEMPORÁRIAS ativas concedidas entre " + _dataIni + " e " + _dataFim + "";
                    }
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioCredenciais", formula, mensagem);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de Autorizações Permanentes/Temporárias
        /// </summary>
        /// <param name="_check"></param>
        public void OnFiltroRelatorioAutorizacoesCommand(bool _tipo, string _dataIni, string _dataFim)
        {
            try
            {


                //AUTORIZAÇÕES PERMANENTES - (2)
                if (_tipo)
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(2);
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //Filtrar Autorizações Permanentes e Ativas
                        formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                                  " AND {CredenciaisStatus.CredencialStatusID} = 1 ";

                        mensagem = "Todas as AUTORIZAÇÕES PERMANENTES ativas ";
                    }
                    else
                    {   //Listar Autorizações Permanentes e Ativas por período
                        formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                                  " and ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " and {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as AUTORIZAÇÕES PERMANENTES ativas concedidas entre " + _dataIni + " e " + _dataFim + "";
                    }
                }
                //AUTORIZAÇÕES TEMPORÁRIAS - (3)
                else
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(4);
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //Filtrar Autorizações Temporárias e Ativas
                        formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                                  "AND {CredenciaisStatus.CredencialStatusID} = 1 ";

                        mensagem = "Todas as AUTORIZAÇÕES TEMPORÁRIAS ativas ";
                    }
                    else
                    {
                        //Listar Autorizações Permanentes e Ativas por período
                        formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                                  " and ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " and {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as AUTORIZAÇÕES TEMPORÁRIAS ativas concedidas entre " + _dataIni + " e " + _dataFim + "";
                    }
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioAutorizacoes", formula, mensagem);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de Credenciais Inválidas
        /// (Indeferidas,Roubadas,Extraviadas,Não-Devolvidas...)
        /// </summary>
        /// <param name="_status"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnRelatorioCredenciaisInvalidasFiltroCommand(int _status, string _dataIni, string _dataFim)
        {
            try
            {
                //CREDENCIAIS PERMANENTES - (5)
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(5);

                //Caso período de datas seja vazio (Todas Inválidas)
                if ((_dataFim == "" || _dataIni == "") && _status == 0)
                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 ";

                    mensagem = "Todas as CREDENCIAIS INVÁLIDAS (vencidas/indeferidas/canceladas/extraviadas/destruídas) ";
                }
                //Caso período de datas seja vazio e status informado
                else if ((_dataFim == "" && _dataIni == "") && _status != 0)
                {
                    //Credenciais Roubadas
                    if (_status == 2)
                    {

                        formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 10";

                        mensagem = "Todas as CREDENCIAIS ROUBADAS ";

                    }
                    //Credenciais Extraviadas
                    else if (_status == 1)
                    {

                        formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 9";

                        mensagem = "Todas as CREDENCIAIS EXTRAVIADAS ";
                    }

                    //Credenciais (Indefereidas, destruídas ou não-devolvidas)
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _status + "";
                        switch (_status)
                        {
                            case 3:
                                verbo = "DESTRUÍDAS";
                                break;
                            case 4:
                                verbo = "NÃO DEVOLVIDAS";
                                break;
                            case 5:
                                verbo = "INDEFERIDAS";
                                break;
                        }
                        mensagem = "Todas as CREDENCIAIS " + verbo;
                    }
                }

                else
                {
                    //(Todas Inválidas no período)
                    if (_status == 0)
                    {
                        formula = " ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as CREDENCIAIS INVÁLIDAS (vencidas/indeferidas/canceladas/extraviadas/destruídas) entre " + _dataIni + " e " + _dataFim + "";
                    }
                    //Credenciais Roubadas
                    else if (_status == 2)
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 10" +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as CREDENCIAIS ROUBADAS entre " + _dataIni + " e " + _dataFim + "";
                    }
                    //Credenciais Extraviadas
                    else if (_status == 1)
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 9" +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as CREDENCIAIS EXTRAVIADAS entre " + _dataIni + " e " + _dataFim + "";
                    }
                    //Credenciais (Indefereidas, destruídas ou não-devolvidas)
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _status +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as CREDENCIAIS ????? entre " + _dataIni + " e " + _dataFim + "";

                        switch (_status)
                        {
                            case 3:
                                verbo = "DESTRUÍDAS";
                                break;
                            case 4:
                                verbo = "NÃO DEVOLVIDAS";
                                break;
                            case 5:
                                verbo = "INDEFERIDAS";
                                break;
                        }
                        mensagem = "Todas as CREDENCIAIS " + verbo + "entre " + _dataIni + " e " + _dataFim + "";
                    }
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioCredenciaisInvalidas", formula, mensagem);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_status"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnRelatorioAutorizacoesInvalidasFiltroCommand(int _status, string _dataIni, string _dataFim)
        {
            try
            {
                //6_Relatório_AutorizacoesInvalidas.rpt
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(6);

                //Caso período de datas seja vazio (Todas Inválidas)
                if ((_dataFim == "" || _dataIni == "") && _status == 0)

                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 ";
                    mensagem = "Todas as AUTORIZAÇÕES INVÁLIDAS (vencidas/indeferidas/canceladas/extraviadas/destruídas) ";
                }
                //Caso período de datas seja vazio e status informado
                else if ((_dataFim == "" && _dataIni == "") && _status != 0)
                {

                    //Credenciais Roubadas
                    if (_status == 2)
                    {

                        formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 10";

                        mensagem = "Todas as AUTORIZAÇÕES ROUBADAS ";

                    }
                    //Credenciais Extraviadas
                    else if (_status == 1)
                    {

                        formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 9";

                        mensagem = "Todas as AUTORIZAÇÕES EXTRAVIADAS ";
                    }

                    //Credenciais (Indefereidas, destruídas ou não-devolvidas)
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _status + "";
                        switch (_status)
                        {
                            case 3:
                                verbo = "DESTRUÍDAS";
                                break;
                            case 4:
                                verbo = "NÃO DEVOLVIDAS";
                                break;
                            case 5:
                                verbo = "INDEFERIDAS";
                                break;

                        }
                        mensagem = "Todas as AUTORIZAÇÕES " + verbo;
                    }
                }
                else
                {
                    //(Todas Inválidas no período)
                    if (_status == 0)
                    {
                        formula = " ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as AUTORIZAÇÕES INVÁLIDAS (vencidas/indeferidas/canceladas/extraviadas/destruídas) " +
                                   "entre " + _dataIni + " e " + _dataFim + "";

                    }
                    //Autorizações Roubadas
                    else if (_status == 2)
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 10" +
                                  " AND ({VeiculosCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as AUTORIZAÇÕES ROUBADAS entre " + _dataIni + " e " + _dataFim + "";
                    }
                    //Autorizaçõess Extraviadas
                    else if (_status == 1)
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 9" +
                                  " AND ({VeiculosCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as AUTORIZAÇÕES EXTRAVIADAS entre " + _dataIni + " e " + _dataFim + "";
                    }
                    //Autorizações (Indefereidas, destruídas ou não-devolvidas)
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _status +
                                  " AND ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }
                    mensagem = "Todas as AUTORIZAÇÕES " + verbo + "entre " + _dataIni + " e " + _dataFim + "";

                    switch (_status)
                    {
                        case 3:
                            verbo = "DESTRUÍDAS";
                            break;
                        case 4:
                            verbo = "NÃO DEVOLVIDAS";
                            break;
                        case 5:
                            verbo = "INDEFERIDAS";
                            break;
                    }
                    mensagem = "Todas as AUTORIZAÇÕES " + verbo + "entre " + _dataIni + " e " + _dataFim + "";
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioAutorizacoesInvalidas", formula, mensagem);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de Credenciais por Área de Acesso
        /// </summary>
        /// <param name="_area"></param>
        /// <param name="_check"></param>
        public void OnRelatorioFiltroPorAreaCommand(string _area, bool _check)
        {
            try
            {
                //Relatório de Credenciais
                if (_check)
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(7);
                    mensagem = "Todas as CREDENCIAIS PERMANENTES E VÁLIDAS por ÁREA DE ACESSO";

                }
                //Relatório de Autorizações
                else
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(8);
                    mensagem = "Todas as AUTORIZAÇÕES PERMANENTES E VÁLIDAS por ÁREA DE ACESSO";
                }

                if (_area != "")
                {
                    formula = " {AreasAcessos_0.AreaAcessoID} = " + _area;
                }
                else
                {
                    formula = "";
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioPorArea", formula, mensagem);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        /// <summary>
        /// Filtrar Relatório de Credenciais e Autorizações por Empresas
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="_check"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnRelatorioFiltroPorEmpresaCommand(string empresa, bool _check, string _dataIni, string _dataFim)
        {
            try
            {
                //CREDENCIAIS
                if (_check)
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(9);
                }
                //AUTORIZAÇÕES
                else
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(10);
                }


                //Filtra Todas Empresas
                if (empresa == "" && _dataIni == "" && _dataFim == "")
                {
                    formula = "";
                    mensagem = "Todas as CREDENCIAIS emitidas por entidade solicitante";
                }

                //Uma Empresa
                else if (_dataIni == "" && _dataFim == "" && _check)
                {
                    formula = " {Empresas.EmpresaID} = " + empresa;
                    mensagem = "Todas as CREDENCIAIS emitidas por entidade solicitante";
                }

                //Credenciais
                else if (_check)
                {
                    //Uma empresas (filtrando por data)
                    if (empresa != "")
                    {
                        formula = "{ColaboradoresCredenciais.Emissao}  <= CDate ('" + _dataFim + "')" +
                                  " and {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "')" +
                                  " and {Empresas.EmpresaID} = " + empresa + "";
                    }
                    //Todas empresas (filtrando por data)
                    else
                    {
                        formula = "{ColaboradoresCredenciais.Emissao}  <= CDate ('" + _dataFim + "')" +
                                  " and {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "')";
                    }

                    mensagem = "Todas as CREDENCIAIS emitidas por entidade solicitante no período de " + _dataIni + " a " + _dataFim;
                }

                //Autorizacoes
                else
                {
                    //Uma empresa (filtrando por data)
                    if (empresa != "")
                    {
                        formula = "{VeiculosCredenciais.Emissao}  <= CDate ('" + _dataFim + "')" +
                                  " and {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "')" +
                                  " and {Empresas.EmpresaID} = " + empresa + "";
                    }
                    //Todas empresa (filtrando por data)
                    else
                    {
                        formula = "{VeiculosCredenciais.Emissao}  <= CDate ('" + _dataFim + "')" +
                                  " and {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "')";
                    }

                    mensagem = "Todas as AUTORIZAÇÕES emitidas por entidade solicitante no período de " + _dataIni + " a " + _dataFim;

                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioPorEmpresa", formula, mensagem);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de impressões de Credenciais e Autorizações por Empresas
        /// </summary>
        /// <param name="_empresa"></param>
        /// <param name="_check"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltrosImpressoesCommand(string _empresa, bool _check, string _dataIni, string _dataFim)
        {
            try
            {
                //CREDENCIAIS
                if (_check)
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(23);
                }
                //AUTORIZAÇÕES
                else
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(24);
                }

                if (_dataIni == "" || _dataFim == "")
                {
                    if (_empresa != "")
                    {
                        formula = "{ Empresas.EmpresaID} = " + _empresa + "";
                    }
                    else
                    {
                        formula = "";
                    }
                }

                else
                {
                    //CREDENCIAIS
                    if (_check)
                    {
                        //Uma empresas (filtrando por data)
                        if (_empresa != "")
                        {
                            formula = "{ColaboradoresCredenciaisImpressoes.DataImpressao} <= cdate('" + _dataFim + "') " +
                                      " and {ColaboradoresCredenciaisImpressoes.DataImpressao} >= cdate('" + _dataIni + "')" +
                                      " and {Empresas.EmpresaID} = " + _empresa + "";
                        }
                        else
                        {
                            formula = "{ColaboradoresCredenciaisImpressoes.DataImpressao} <= cdate('" + _dataFim + "') " +
                                      " and {ColaboradoresCredenciaisImpressoes.DataImpressao} >= cdate('" + _dataIni + "')";
                        }
                    }

                    //AUTORIZAÇÕES
                    else
                    {
                        //Uma empresas (filtrando por data)
                        if (_empresa != "")
                        {
                            formula = "{VeiculosCredenciaisImpressoes.DataImpressao} <= cdate('" + _dataFim + "') " +
                                      " and {VeiculosCredenciaisImpressoes.DataImpressao} >= cdate('" + _dataIni + "')" +
                                      " and {Empresas.EmpresaID} = " + _empresa + "";
                        }
                        else
                        {
                            formula = "{VeiculosCredenciaisImpressoes.DataImpressao} <= cdate('" + _dataFim + "') " +
                                      " and {VeiculosCredenciaisImpressoes.DataImpressao} >= cdate('" + _dataIni + "')";
                        }
                    }
                    mensagem = "Impressões de Credenciais registradas entre " + _dataIni + " e " + _dataFim;
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioImpressoes", formula, "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de vias adicionais de Credenciais
        /// </summary>
        /// <param name="_tipo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltroCredencialViasAdicionaisCommand(int _tipo, string _dataIni, string _dataFim)
        {
            try
            {
                //CREDENCIAIS
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(21);

                if (_tipo == 0)
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //TODAS AS VIAS ADICIONAIS EMITIDAS
                        formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        mensagem = "Todas as VIAS ADICIONAIS de CREDENCIAIS emitidas";
                    }
                    else
                    {
                        formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3] " +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as VIAS ADICIONAIS de CREDENCIAIS emitidas entre " + _dataIni + " e " + _dataFim;
                    }
                }
                else
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //VIAS ADICIONAIS EMITIDAS (1a,2a,3a VIA)
                        formula = " {CredenciaisMotivos.CredencialmotivoID}  =  " + _tipo;

                        switch (_tipo)
                        {
                            case 2:
                                verbo = "SEGUNDA EMISSÃO";
                                break;
                            case 3:
                                verbo = "TERCEIRA EMISSÃO";
                                break;
                        }

                        mensagem = "Todas as VIAS ADICIONAIS (" + verbo + ") de CREDENCIAIS emitidas";
                    }
                    else
                    {
                        formula = " {CredenciaisMotivos.CredencialmotivoID}  =  " + _tipo +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as VIAS ADICIONAIS de CREDENCIAIS emitidas entre " + _dataIni + " e " + _dataFim;
                    }

                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioCredenciaisViasAdicionais", formula, mensagem);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        /// <summary>
        /// Filtrar Relatório de vias adicionais de Credenciais
        /// </summary>
        /// <param name="_tipo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltroAutorizacaoViasAdicionaisCommand(int _tipo, string _dataIni, string _dataFim)
        {
            try
            {
                //CREDENCIAIS
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(22);

                if (_tipo == 0)
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //TODAS AS VIAS ADICIONAIS EMITIDAS
                        formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        mensagem = "Todas as VIAS ADICIONAIS de AUTORIZAÇÕES ATIV emitidas";
                    }
                    else
                    {
                        formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3] " +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as VIAS ADICIONAIS de AUTORIZAÇÕES ATIV emitidas entre " + _dataIni + " e " + _dataFim;
                    }
                }
                else
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //VIAS ADICIONAIS EMITIDAS (1a,2a,3a VIA)
                        formula = " {CredenciaisMotivos.CredencialmotivoID}  =  " + _tipo;

                        switch (_tipo)
                        {
                            case 2:
                                verbo = "SEGUNDA EMISSÃO";
                                break;
                            case 3:
                                verbo = "TERCEIRA EMISSÃO";
                                break;
                        }

                        mensagem = "Todas as VIAS ADICIONAIS (" + verbo + ") de AUTORIZAÇÕES ATIV emitidas";
                    }
                    else
                    {
                        formula = " {CredenciaisMotivos.CredencialmotivoID}  =  " + _tipo +
                                  " AND ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";

                        mensagem = "Todas as VIAS ADICIONAIS de AUTORIZAÇÕES ATIV emitidas entre " + _dataIni + " e " + _dataFim;
                    }

                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioAutorizacoesViasAdicionais", formula, mensagem);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion


        #endregion



    }
}
