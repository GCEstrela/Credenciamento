using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Xml;
using AutoMapper;
using iModSCCredenciamento.Helpers;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.Domain.Entities;
using Utils = IMOD.CrossCutting.Utils;

namespace iModSCCredenciamento.ViewModels
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

        private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreasAcessos;

        private ObservableCollection<ClasseEmpresas.Empresa> _Empresas;


        private ObservableCollection<ClasseRelatorios.Relatorio> _Relatorios;

        private ClasseRelatorios.Relatorio _RelatorioSelecionado;

        private ClasseRelatorios.Relatorio _relatorioTemp = new ClasseRelatorios.Relatorio();

        private List<ClasseRelatorios.Relatorio> _RelatoriosTemp = new List<ClasseRelatorios.Relatorio>();

        PopupMensagem _PopupSalvando;

        private int _selectedIndex;

        private int _selectedIndexTemp = 0;

        private bool _atualizandoFoto = false;

        private BitmapImage _Waiting;

        private string formula;

        private readonly IRelatorioService _relatorioService = new RelatorioService();
        private readonly IRelatorioGerencialService _relatorioGerencialServiceService = new RelatorioGerencialService();
        private readonly IEmpresaService _empresaService = new EmpresaService();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();

        private Relatorios relatorio = new Relatorios();
        private RelatoriosGerenciais relatorioGerencial = new RelatoriosGerenciais();

        #endregion

        #region Contrutores

        public ObservableCollection<ClasseAreasAcessos.AreaAcesso> AreasAcessos
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
        public ObservableCollection<ClasseEmpresas.Empresa> Empresas

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
        public ObservableCollection<ClasseRelatorios.Relatorio> Relatorios
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

        public ClasseRelatorios.Relatorio RelatorioSelecionado
        {
            get
            {
                return this._RelatorioSelecionado;
            }
            set
            {
                this._RelatorioSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (RelatorioSelecionado != null)
                {
                    //    //BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
                    //    //string _imgstr = Conversores.IMGtoSTR(_img);
                    //    //ColaboradorSelecionado.Foto = _imgstr;
                    //    if (!_atualizandoFoto)
                    //    {
                    //        Thread CarregaRelatorio_thr = new Thread(() => CarregaRelatorio(RelatorioSelecionado.RelatorioID));
                    //        CarregaRelatorio_thr.Start();
                    //    }

                    //    //CarregaFoto(ColaboradorSelecionado.ColaboradorID);
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
                return this._Waiting;
            }
            set
            {
                this._Waiting = value;
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

                var list2 = Mapper.Map<List<ClasseRelatorios.Relatorio>>(list1);
                var observer = new ObservableCollection<ClasseRelatorios.Relatorio>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Relatorios = observer;

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

                var list2 = Mapper.Map<List<ClasseAreasAcessos.AreaAcesso>>(list1);
                var observer = new ObservableCollection<ClasseAreasAcessos.AreaAcesso>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.AreasAcessos = observer;
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
                var list2 = Mapper.Map<List<ClasseEmpresas.Empresa>>(list1.OrderByDescending(a => a.EmpresaId));

                var observer = new ObservableCollection<ClasseEmpresas.Empresa>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Empresas = observer;
                SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

        #region Data Access (Vazia)


        #endregion

        #region Comandos dos Botoes

        public void OnAbrirRelatorioCommand(string _Tag)
        {
            try
            {
                relatorio = _relatorioService.BuscarPelaChave(Convert.ToInt32(_Tag));

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "Relatorio " + _Tag, formula, "");

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #region Comandos dos Botoes (RELATÓRIOS GERENCIAIS)

        public void OnFiltroRelatorioCredencialCommand(bool _tipo, string _dataIni, string _dataFim)
        {
            try
            {
                //Permanentes
                if (_tipo)
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(1);
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //Filtra todas Credenciais Permanentes e Ativas
                        formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 ";
                    }
                    else
                    {   //Filtra todas Credenciais Permanentes e Ativas (Período)
                        formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                                  " and ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " and {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }

                }
                //Temporárias
                else
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(5);
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //Filtra todas Credenciais Temporárias e Ativas
                        formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 ";
                    }
                    else
                    {   //Filtra todas Credenciais Permanentes e Ativas (Período)
                        formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                                  " and ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " and {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioCredenciais", formula, "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnFiltroRelatorioAutorizacoesCommand(bool _check)
        {
            try
            {
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(2);

                if (_check)
                {   //Filtrar Autorizações Permanentes e Ativas
                    formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                              " AND {CredenciaisStatus.CredencialStatusID} = 1 ";
                }
                else
                {
                    //Filtrar Autorizações Temporárias e Ativas
                    formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                              "AND {CredenciaisStatus.CredencialStatusID} = 1 ";
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioAutorizacoes", formula, "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnRelatorioCredenciaisInvalidasFiltroCommand(int _status, string _dataIni, string _dataFim)
        {
            try
            {
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(3);

                //Período de datas vazio (TODOS)
                if ((_dataFim == "" || _dataIni == "") && _status == 0)
                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 ";
                }
                //Período de datas vazio (TODOS)
                else if ((_dataFim == "" && _dataIni == "") && _status != 0)
                {
                    if (_status == 2)
                    {
                        //Credenciais Roubadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 10";
                    }
                    else if (_status == 1)
                    {
                        //Credenciais Extraviadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 9";
                    }
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _status + "";
                    }

                }
                else
                {
                    if (_status == 0)
                    {
                        formula = " ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }

                    else if (_status == 2)
                    {
                        //Credenciais Roubadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 10" +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }
                    else if (_status == 1)
                    {
                        //Credenciais Extraviadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 9" +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _status +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioCredenciaisInvalidas", formula, "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnRelatorioAutorizacoesInvalidasFiltroCommand(int _check, string _dataIni, string _dataFim)
        {
            try
            {
                //4_Relatório_AutorizacoesInvalidas.rpt
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(4);

                //DATA VAZIA (TODOS)
                if ((_dataFim == "" || _dataIni == "") && _check == 10)

                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 ";
                }
                //CAMPOS VAZIOS (TODOS)
                else if ((_dataFim == "" && _dataIni == "") && _check != 10)
                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                    " AND {CredenciaisStatus.CredencialStatusID} = " + _check + "";
                }
                else
                {
                    if (_check == 10)
                    {
                        formula = " ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _check +
                                  " AND ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioAutorizacoesInvalidas", formula, "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnRelatorioFiltroPorAreaCommand(string _area, bool _check)
        {
            try
            {
                if (_check)
                {
                    //6_Relatorio_CredenciaisPorArea.rpt
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(6);
                }
                else
                {
                    //7_Relatorio_AutorizacoesPorArea.rpt
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(7);
                }

                formula = " {AreasAcessos_0.AreaAcessoID} = " + _area;

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioCredenciaisAutorizacoesPorArea", formula, "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnRelatorioFiltroPorEmpresaCommand(string empresa, bool _check, string _dataIni, string _dataFim)
        {
            try
            {
                if (_check)
                {
                    //8_Relatorio_CredenciaisNoPeriodoPorEntidade.rpt
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(8);
                }

                else
                {
                    //9_Relatorio_AutorizacoesNoPeriodoPorEntidade.rpt
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(9);
                }



                //Todas Empresas
                if (empresa == "" && _dataIni == "" && _dataFim == "")
                {
                    formula = "";
                }

                //Uma Empresa
                else if (_dataIni == "" && _dataFim == "" && _check)
                {
                    formula = " {Empresas.EmpresaID} = " + empresa;
                }

                //Credenciais
                else if (_check)
                {
                    formula = "{ColaboradoresEmpresas.Emissao}  <= '" + _dataFim + "' " +
                              " and {ColaboradoresEmpresas.Emissao} >= '" + _dataIni +
                              "' and {Empresas.EmpresaID} = " + empresa + "";

                }

                //Autorizacoes
                else
                {
                    formula = "{EmpresasVeiculos.Emissao}  <= '" + _dataFim + "' " +
                          " and {EmpresasVeiculos.Emissao} >= '" + _dataIni +
                          "' and {Empresas.EmpresaID} = " + empresa + "";

                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioCredenciaisAutorizacoesPorEmpresa", formula, "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnFiltrosImpressoesCommand(string _empresa, string _area, bool _check, string _dataIni, string _dataFim)
        {
            try
            {
                if (_check)
                {
                    //10_Relatório_ImpressoesCredenciais.rpt
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(10);
                }
                else
                {
                    //11_Relatório_ImpressoesAutorizacoes.rpt
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(11);
                }

                if (_empresa == "" || _dataIni == "" || _dataFim == "" || _area == "")
                {
                    formula = "";
                }

                else
                {
                    formula = " 1=1 and {ColaboradoresCredenciais.Emissao} <= cdate('" + _dataFim + "') " +
                          " and {ColaboradoresCredenciais.Emissao} <= cdate('" + _dataIni + "')" +
                          " and  {EmpresasAreasAcessos.AreaAcessoID} = " + _area + "" +
                          " and {Empresas.EmpresaID} = " + _empresa + "";
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioImpressoesCredenciaisAutorizacoes", formula, "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnFiltroCredencialViasAdicionaisCommand(int _tipo, string _dataIni, string _dataFim)
        {
            try
            {
                //21_Relatório_ViasAdicionaisCredenciais.rpt
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(21);

                if (_tipo == 0)
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //TODAS AS VIAS ADICIONAIS EMITIDAS
                        formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3]";
                    }
                    else
                    {
                        formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3] " +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }
                }
                else
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //VIAS ADICIONAIS EMITIDAS (1a,2a,3a VIA)
                        formula = " {CredenciaisMotivos.CredencialmotivoID}  =  " + _tipo;
                    }
                    else
                    {
                        formula = " {CredenciaisMotivos.CredencialmotivoID}  =  " + _tipo +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }

                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioCredenciaisViasAdicionais", formula, "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion


        #endregion

        #region Metodos privados (Vazia)

        #endregion

    }
}
