using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using iModSCCredenciamento.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using CrystalDecisions.CrystalReports.Engine;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using AutoMapper;
using IMOD.CrossCutting;
using iModSCCredenciamento.Views.Model;

namespace iModSCCredenciamento.ViewModels
{
    public class VeiculosCredenciaisViewModel : ViewModelBase
    {
        private readonly IVeiculoCredencialService _repositorio = new VeiculoCredencialService();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();

        #region Inicializacao
        public VeiculosCredenciaisViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();

        }

        private void CarregaUI()
        {
            //CarregaColecaoEmpresas();
            CarregaColecaoAreasAcessos();
            CarregaColeçãoFormatosCredenciais();
            CarregaColecaoTiposCredenciais();
            CarregaColecaoCredenciaisStatus();
            CarregaColecaoTecnologiasCredenciais();
            CarregaColecaoCredenciaisMotivos();

            CarregaColecaoVeiculosPrivilegios();

            CarregaColecaoVeiculosCredenciais();
        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseTiposCombustiveis.TipoCombustivel> _TiposCombustiveis;
        private ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculosCredenciais;

        private ObservableCollection<ClasseVinculos.Vinculo> _Vinculos;

        private ObservableCollection<EmpresaView> _Empresas;

        private ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> _FormatosCredenciais;

        private ObservableCollection<EmpresaLayoutCrachaView> _EmpresasLayoutsCrachas;

        private ObservableCollection<EmpresaContratoView> _Contratos;

        private ClasseVeiculosCredenciais.VeiculoCredencial _VeiculoCredencialSelecionado;

        private ClasseVeiculosCredenciais.VeiculoCredencial _VeiculoCredencialTemp = new ClasseVeiculosCredenciais.VeiculoCredencial();

        private List<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculosCredenciaisTemp = new List<ClasseVeiculosCredenciais.VeiculoCredencial>();

        private ObservableCollection<ClasseVeiculosPrivilegios.VeiculoPrivilegio> _VeiculosPrivilegios;

        PopupPesquisaVeiculosCredenciais popupPesquisaVeiculosCredenciais;

        private int _selectedIndex;

        private int _VeiculoSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        private string _Validade;

        private ObservableCollection<ClasseTiposCredenciais.TipoCredencial> _TiposCredenciais;

        private ObservableCollection<ClasseCredenciaisStatus.CredencialStatus> _CredenciaisStatus;

        private ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial> _TecnologiasCredenciais;

        private ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa> _VeiculosEmpresas;
        private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreasAcessos;
        private ObservableCollection<ClasseCredenciaisMotivos.CredencialMotivo> _CredenciaisMotivos;

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
        public ObservableCollection<ClasseCredenciaisMotivos.CredencialMotivo> CredenciaisMotivos
        {
            get
            {
                return this._CredenciaisMotivos;
            }
            set
            {
                if (_CredenciaisMotivos != value)
                {
                    _CredenciaisMotivos = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa> VeiculosEmpresas
        {
            get
            {
                return _VeiculosEmpresas;
            }

            set
            {
                if (_VeiculosEmpresas != value)
                {
                    _VeiculosEmpresas = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseTiposCredenciais.TipoCredencial> TiposCredenciais
        {
            get
            {
                return _TiposCredenciais;
            }

            set
            {
                if (_TiposCredenciais != value)
                {
                    _TiposCredenciais = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseCredenciaisStatus.CredencialStatus> CredenciaisStatus
        {
            get
            {
                return _CredenciaisStatus;
            }

            set
            {
                if (_CredenciaisStatus != value)
                {
                    _CredenciaisStatus = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial> TecnologiasCredenciais
        {
            get
            {
                return _TecnologiasCredenciais;
            }

            set
            {
                if (_TecnologiasCredenciais != value)
                {
                    _TecnologiasCredenciais = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> VeiculosCredenciais
        {
            get
            {
                return _VeiculosCredenciais;
            }

            set
            {
                if (_VeiculosCredenciais != value)
                {
                    _VeiculosCredenciais = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseVinculos.Vinculo> Vinculos
        {
            get
            {
                return _Vinculos;
            }

            set
            {
                if (_Vinculos != value)
                {
                    _Vinculos = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<EmpresaLayoutCrachaView> EmpresasLayoutsCrachas
        {
            get
            {
                return _EmpresasLayoutsCrachas;
            }

            set
            {
                if (_EmpresasLayoutsCrachas != value)
                {
                    _EmpresasLayoutsCrachas = value;
                    OnPropertyChanged();

                }
            }
        }
        public ClasseVeiculosCredenciais.VeiculoCredencial VeiculoCredencialSelecionado
        {
            get
            {
                return this._VeiculoCredencialSelecionado;
            }
            set
            {
                this._VeiculoCredencialSelecionado = value;
                //base.OnPropertyChanged("SelectedItem");
                base.OnPropertyChanged();
                if (VeiculoCredencialSelecionado != null)
                {
                    //CarregaColecaoVeiculosEmpresas(VeiculoCredencialSelecionado.VeiculoID);
                    //CarregaColeçãoEmpresasLayoutsCrachas(VeiculoCredencialSelecionado.EmpresaID);
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
        public ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> FormatosCredenciais

        {
            get
            {
                return _FormatosCredenciais;
            }

            set
            {
                if (_FormatosCredenciais != value)
                {
                    _FormatosCredenciais = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseVeiculosPrivilegios.VeiculoPrivilegio> VeiculosPrivilegios
        {
            get
            {
                return _VeiculosPrivilegios;
            }

            set
            {
                if (_VeiculosPrivilegios != value)
                {
                    _VeiculosPrivilegios = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<EmpresaContratoView> Contratos
        {
            get
            {
                return _Contratos;
            }

            set
            {
                if (_Contratos != value)
                {
                    _Contratos = value;
                    OnPropertyChanged();

                }
            }
        }
        public int VeiculoSelecionadaID
        {
            get
            {
                return this._VeiculoSelecionadaID;

            }
            set
            {
                this._VeiculoSelecionadaID = value;
                base.OnPropertyChanged();
                if (VeiculoSelecionadaID != null)
                {
                    //OnEmpresaSelecionada();
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
        public bool HabilitaEdicao
        {
            get
            {
                return this._HabilitaEdicao;
            }
            set
            {
                this._HabilitaEdicao = value;
                base.OnPropertyChanged();
            }
        }
        public string Criterios
        {
            get
            {
                return this._Criterios;
            }
            set
            {
                this._Criterios = value;
                base.OnPropertyChanged();
            }
        }
        public string Validade
        {
            get
            {
                return this._Validade;
            }
            set
            {
                this._Validade = value;
                base.OnPropertyChanged();
            }
        }
        //public string ComboEmpresaSelecionado
        //{
        //    get
        //    {
        //        return this._ComboEmpresaSelecionado;
        //    }
        //    set
        //    {
        //        this._ComboEmpresaSelecionado = value;
        //        base.OnPropertyChanged();
        //    }
        //}
        #endregion

        #region Comandos dos Botoes

        public void OnAtualizaCommand(object _VeiculoID)
        {
            try
            {
                VeiculoSelecionadaID = Convert.ToInt32(_VeiculoID);
                Thread CarregaColecaoVeiculosCredenciais_thr = new Thread(() =>
                {
                    CarregaColecaoVeiculosEmpresas(VeiculoSelecionadaID);
                    //CarregaColecaoEmpresas(VeiculoSelecionadaID);
                    // CarregaColeçãoEmpresasLayoutsCrachas(VeiculoCredencialSelecionado.EmpresaID);
                    CarregaColecaoVeiculosCredenciais(Convert.ToInt32(_VeiculoID));
                });
                CarregaColecaoVeiculosCredenciais_thr.Start();
                //CarregaColecaoVeiculosCredenciais(Convert.ToInt32(_VeiculoID));

            }
            catch (Exception ex)
            {

            }

        }


        public void OnEditarCommand()
        {
            try
            {

                _VeiculoCredencialTemp = VeiculoCredencialSelecionado.CriaCopia(VeiculoCredencialSelecionado);
                _selectedIndexTemp = SelectedIndex;
                //OnAtualizaCommand(VeiculoSelecionadaID);
                //CarregaColecaoContratos(_VeiculoCredencialTemp.EmpresaID);

                HabilitaEdicao = true;
            }
            catch (Exception)
            {
            }
        }

        public void OnCancelarEdicaoCommand()
        {
            try
            {
                VeiculosCredenciais[_selectedIndexTemp] = _VeiculoCredencialTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {

            }
        }

        public void OnSalvarEdicaoCommand()
        {
            try
            {

                if (VeiculoCredencialSelecionado.CredencialStatusID == 1)
                {
                    //CarregaColeçãoVinculos(VeiculoCredencialSelecionado.VeiculoCredencialID);

                    bool _resposta = SCManager.VincularVeiculo(VeiculoCredencialSelecionado);

                    //VeiculoCredencialSelecionado.CredencialGuid = Vinculos[0].CredencialGuid;

                    //VeiculoCredencialSelecionado.CardHolderGuid = Vinculos[0].CardHolderGuid;
                }


                HabilitaEdicao = false;
                //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculosCredenciais));

                //ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculoCredencialTemp = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>();
                //ClasseVeiculosCredenciais _ClasseVeiculoerEmpresasTemp = new ClasseVeiculosCredenciais();
                //_VeiculoCredencialTemp.Add(VeiculoCredencialSelecionado);
                //_ClasseVeiculoerEmpresasTemp.VeiculosCredenciais = _VeiculoCredencialTemp;

                //string xmlString;

                //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                //{

                //    using (XmlTextWriter xw = new XmlTextWriter(sw))
                //    {
                //        xw.Formatting = Formatting.Indented;
                //        serializer.Serialize(xw, _ClasseVeiculoerEmpresasTemp);
                //        xmlString = sw.ToString();
                //    }

                //}

                //InsereVeiculoCredencialBD(xmlString);

                var entity = Mapper.Map<IMOD.Domain.Entities.VeiculoCredencial>(VeiculoCredencialSelecionado);
                var repositorio = new IMOD.Application.Service.VeiculoCredencialService();
                _repositorio.Alterar(entity);

                Thread CarregaColecaoVeiculosCredenciais_thr = new Thread(() =>
                {
                    CarregaColecaoVeiculosCredenciais(VeiculoSelecionadaID);
                });
                CarregaColecaoVeiculosCredenciais_thr.Start();

                //_ClasseEmpresasSegurosTemp = null;

                //_SegurosTemp.Clear();
                //_seguroTemp = null;


            }
            catch (Exception ex)
            {

            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                if (VeiculosCredenciais != null)
                {
                    foreach (var x in VeiculosCredenciais)
                    {
                        _VeiculosCredenciaisTemp.Add(x);
                    }

                    _selectedIndexTemp = SelectedIndex;
                    VeiculosCredenciais.Clear();
                }

                _VeiculoCredencialTemp = new ClasseVeiculosCredenciais.VeiculoCredencial();
                _VeiculoCredencialTemp.VeiculoID = VeiculoSelecionadaID;
                VeiculosCredenciais.Add(_VeiculoCredencialTemp);
                //OnAtualizaCommand(VeiculoSelecionadaID);
                CarregaColecaoEmpresas(VeiculoSelecionadaID);
                SelectedIndex = 0;
                VeiculoCredencialSelecionado.Emissao = DateTime.Now;
                HabilitaEdicao = true;
                // SelectedIndex = 0;
            }
            catch (Exception ex)
            {

            }

        }
        //public void OnSelecionaEmpresaCommand(int _empresaID)
        //{
        //    CarregaColecaoContratos(_empresaID);
        //}
        public void OnSalvarAdicaoCommand()
        {
            //try
            //{

            //    string _xml = RequisitaVeiculosCredenciaisNovos(VeiculoCredencialSelecionado.VeiculoEmpresaID);

            //    XmlSerializer deserializer = new XmlSerializer(typeof(ClasseColaboradoresCredenciais));

            //    XmlDocument xmldocument = new XmlDocument();
            //    xmldocument.LoadXml(_xml);

            //    TextReader reader = new StringReader(_xml);
            //    ClasseVeiculosCredenciais classeVeiculosCredenciais = new ClasseVeiculosCredenciais();
            //    classeVeiculosCredenciais = (ClasseVeiculosCredenciais)deserializer.Deserialize(reader);
            //    VeiculoCredencialSelecionado.Cargo = classeVeiculosCredenciais.VeiculosCredenciais[0].Cargo;
            //    VeiculoCredencialSelecionado.CNPJ = classeVeiculosCredenciais.VeiculosCredenciais[0].CNPJ;
            //    VeiculoCredencialSelecionado.ColaboradorApelido = classeVeiculosCredenciais.VeiculosCredenciais[0].ColaboradorApelido;
            //    VeiculoCredencialSelecionado.VeiculoEmpresaID = classeVeiculosCredenciais.VeiculosCredenciais[0].VeiculoEmpresaID;
            //    VeiculoCredencialSelecionado.VeiculoFoto = classeVeiculosCredenciais.VeiculosCredenciais[0].VeiculoFoto;
            //    VeiculoCredencialSelecionado.VeiculoID = classeVeiculosCredenciais.VeiculosCredenciais[0].VeiculoID;
            //    VeiculoCredencialSelecionado.VeiculoNome = classeVeiculosCredenciais.VeiculosCredenciais[0].VeiculoNome;
            //    VeiculoCredencialSelecionado.ContratoDescricao = classeVeiculosCredenciais.VeiculosCredenciais[0].ContratoDescricao;
            //    VeiculoCredencialSelecionado.Placa = classeVeiculosCredenciais.VeiculosCredenciais[0].Placa;
            //    VeiculoCredencialSelecionado.EmpresaApelido = classeVeiculosCredenciais.VeiculosCredenciais[0].EmpresaApelido;
            //    VeiculoCredencialSelecionado.EmpresaID = classeVeiculosCredenciais.VeiculosCredenciais[0].EmpresaID;
            //    VeiculoCredencialSelecionado.EmpresaLogo = classeVeiculosCredenciais.VeiculosCredenciais[0].EmpresaLogo;
            //    VeiculoCredencialSelecionado.EmpresaNome = classeVeiculosCredenciais.VeiculosCredenciais[0].EmpresaNome;
            //    VeiculoCredencialSelecionado.Motorista = classeVeiculosCredenciais.VeiculosCredenciais[0].Motorista;

            //    VeiculoCredencialSelecionado.FormatoCredencialDescricao = FormatosCredenciais.First(i => i.FormatoCredencialID == VeiculoCredencialSelecionado.FormatoCredencialID).Descricao;
            //    VeiculoCredencialSelecionado.LayoutCrachaGUID = EmpresasLayoutsCrachas.First(i => i.LayoutCrachaID == VeiculoCredencialSelecionado.LayoutCrachaID).LayoutCrachaGUID;

            //    VeiculoCredencialSelecionado.PrivilegioDescricao1 = VeiculosPrivilegios.First(i => i.VeiculoPrivilegioID == VeiculoCredencialSelecionado.VeiculoPrivilegio1ID).Descricao;
            //    VeiculoCredencialSelecionado.PrivilegioDescricao2 = VeiculosPrivilegios.First(i => i.VeiculoPrivilegioID == VeiculoCredencialSelecionado.VeiculoPrivilegio2ID).Descricao;



            //    VeiculoCredencialSelecionado.Validade = (DateTime?)VerificarMenorData(VeiculoCredencialSelecionado.VeiculoID);

            //    var _index = SelectedIndex;

            //    //bool _resposta = SCManager.Vincular(VeiculoCredencialSelecionado);


            //    if (!VeiculoCredencialSelecionado.Ativa)
            //    {
            //        VeiculoCredencialSelecionado.Baixa = DateTime.Now;
            //    }
            //    else
            //    {
            //        VeiculoCredencialSelecionado.Baixa = null;
            //    }


            //    HabilitaEdicao = false;
            //    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculosCredenciais));

            //    ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculoCredencialPro = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>();
            //    ClasseVeiculosCredenciais _ClasseVeiculosEmpresasPro = new ClasseVeiculosCredenciais();
            //    _VeiculoCredencialPro.Add(VeiculoCredencialSelecionado);
            //    _ClasseVeiculosEmpresasPro.VeiculosCredenciais = _VeiculoCredencialPro;



            //    string xmlString;

            //    using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
            //    {

            //        using (XmlTextWriter xw = new XmlTextWriter(sw))
            //        {
            //            xw.Formatting = Formatting.Indented;
            //            serializer.Serialize(xw, _ClasseVeiculosEmpresasPro);
            //            xmlString = sw.ToString();
            //        }

            //    }
            //    //int _colaboradorCredencialID = InsereColaboradorCredencialBD(xmlString);

            //    int _colaboradorCredencialID = InsereVeiculoCredencialBD(xmlString);

            //    CarregaColecaoVeiculosCredenciais(VeiculoSelecionadaID);

            //    SelectedIndex = _index;

            //}
            //catch (Exception ex)
            //{
            //}
            try
            {
                if (VeiculoCredencialSelecionado.CredencialStatusID == 1)
                {
                    //CarregaColeçãoVinculos(VeiculoCredencialSelecionado.VeiculoCredencialID);

                    bool _resposta = SCManager.VincularVeiculo(VeiculoCredencialSelecionado);

                    //VeiculoCredencialSelecionado.CredencialGuid = Vinculos[0].CredencialGuid;

                    //VeiculoCredencialSelecionado.CardHolderGuid = Vinculos[0].CardHolderGuid;
                }

                //HabilitaEdicao = false;
                //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculosCredenciais));

                //ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculoCredencialPro = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>();
                //ClasseVeiculosCredenciais _ClasseVeiculoerEmpresasPro = new ClasseVeiculosCredenciais();
                //_VeiculoCredencialPro.Add(VeiculoCredencialSelecionado);
                //_ClasseVeiculoerEmpresasPro.VeiculosCredenciais = _VeiculoCredencialPro;

                //string xmlString;

                //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                //{

                //    using (XmlTextWriter xw = new XmlTextWriter(sw))
                //    {
                //        xw.Formatting = Formatting.Indented;
                //        serializer.Serialize(xw, _ClasseVeiculoerEmpresasPro);
                //        xmlString = sw.ToString();
                //    }

                //}

                //InsereVeiculoCredencialBD(xmlString);


                var entity = Mapper.Map<IMOD.Domain.Entities.VeiculoCredencial>(VeiculoCredencialSelecionado);
                var repositorio = new IMOD.Application.Service.VeiculoCredencialService();
                _repositorio.Criar(entity);

                Thread CarregaColecaoVeiculosCredenciais_thr = new Thread(() =>
                {
                    CarregaColecaoVeiculosCredenciais(VeiculoSelecionadaID);
                });
                CarregaColecaoVeiculosCredenciais_thr.Start();




                ////Thread CarregaColecaoVeiculosCredenciais_thr = new Thread(() => CarregaColecaoVeiculosCredenciais(VeiculoCredencialSelecionado.VeiculoID));
                ////CarregaColecaoVeiculosCredenciais_thr.Start();
                //_VeiculosCredenciaisTemp.Add(VeiculoCredencialSelecionado);
                //VeiculosCredenciais = null;
                //VeiculosCredenciais = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>(_VeiculosCredenciaisTemp);
                //SelectedIndex = _selectedIndexTemp;
                //_VeiculosCredenciaisTemp.Clear();



            }
            catch (Exception ex)
            {
            }
        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                VeiculosCredenciais = null;
                VeiculosCredenciais = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>(_VeiculosCredenciaisTemp);
                SelectedIndex = _selectedIndexTemp;
                _VeiculosCredenciaisTemp.Clear();
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
            }
        }

        public void OnExcluirCommand()
        {
            try
            {

                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    //if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    //{
                    //    if (SCManager.ExcluirCredencial(VeiculoCredencialSelecionado.CredencialGuid))
                    //    {
                    //        ExcluiVeiculoCredencialBD(VeiculoCredencialSelecionado.VeiculoCredencialID);
                    //        VeiculosCredenciais.Remove(VeiculoCredencialSelecionado);
                    //    }
                    //    else
                    //    {
                    //        Global.PopupBox("Não foi possível excluir esta credencial. Verifique no Gerenciador de Credenciais do Controle de Acesso.", 4);
                    //    }

                    //}
                    var entity = Mapper.Map<IMOD.Domain.Entities.VeiculoCredencial>(VeiculoCredencialSelecionado);
                    var repositorio = new IMOD.Application.Service.VeiculoCredencialService();
                    _repositorio.Remover(entity);
                    VeiculosCredenciais.Remove(VeiculoCredencialSelecionado);
                }

            }
            catch (Exception ex)
            {
            }

        }

        public void OnPesquisarCommand()
        {
            try
            {
                popupPesquisaVeiculosCredenciais = new PopupPesquisaVeiculosCredenciais();
                popupPesquisaVeiculosCredenciais.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaVeiculosCredenciais.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            try
            {
                object vetor = popupPesquisaVeiculosCredenciais.Criterio.Split((char)(20));
                int _veiculoID = VeiculoSelecionadaID;
                string _empresaNome = ((string[])vetor)[0];
                int _status = Convert.ToInt32(((string[])vetor)[1]);
                string _validade = ((string[])vetor)[2];

                CarregaColecaoVeiculosCredenciais(_veiculoID, _empresaNome, _status, _validade);
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {

            }

        }

        public void OnBuscarDataCommand()
        {
            try
            {
                DateTime _datavalidadeCredencial = validadeCursoContrato(VeiculoCredencialSelecionado.VeiculoID);
                VeiculosCredenciais[SelectedIndex].Validade = _datavalidadeCredencial;
                Validade = String.Format("{0:dd/MM/yyyy}", _datavalidadeCredencial);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoVeiculosCredenciais(int veiculoID = 0, string empresaNome = "", int status = 0, string _validade = "")
        {
            try
            {
                var service = new VeiculoCredencialService();
                if (!string.IsNullOrWhiteSpace(empresaNome)) empresaNome = $"%{empresaNome}%";
                //if (!int.IsNullOrWhiteSpace(status)) status = $"%{status}%";
                //if (!string.IsNullOrWhiteSpace(_validade)) _validade = $"%{_validade}%";
                var list1 = service.ListarView(veiculoID, 0, empresaNome, status, _validade);

                var list2 = Mapper.Map<List<ClasseVeiculosCredenciais.VeiculoCredencial>>(list1);

                var observer = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                VeiculosCredenciais = observer;

                //Hotfix auto-selecionar registro do topo da ListView
                var topList = observer.FirstOrDefault();
                VeiculoCredencialSelecionado = topList;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        private void CarregaColecaoVeiculosPrivilegios()
        {
            try
            {
                VeiculosPrivilegios = new ObservableCollection<ClasseVeiculosPrivilegios.VeiculoPrivilegio>();
                foreach (ClasseAreasAcessos.AreaAcesso _areaaAcesso in AreasAcessos)
                {
                    VeiculosPrivilegios.Add(new ClasseVeiculosPrivilegios.VeiculoPrivilegio() { VeiculoPrivilegioID = _areaaAcesso.AreaAcessoID, Descricao = _areaaAcesso.Identificacao });

                }

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
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        private void CarregaColecaoEmpresas(int idEmpresa = 0, string nome = "", string apelido = "", string cnpj = "", string _quantidaderegistro = "500")
        {
            try
            {
                var service = new EmpresaService();
                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                if (!string.IsNullOrWhiteSpace(cnpj)) cnpj = $"%{cnpj}%";

                var list1 = service.Listar(idEmpresa, nome, apelido, cnpj);
                var list2 = Mapper.Map<List<EmpresaView>>(list1);

                var observer = new ObservableCollection<EmpresaView>();
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
        private void CarregaColecaoVeiculosEmpresas(int veiculoID)
        {
            try
            {

                var service = new VeiculoEmpresaService();

                var list1 = service.ListarContratoView(veiculoID);
                var list2 = Mapper.Map<List<ClasseVeiculosEmpresas.VeiculoEmpresa>>(list1);

                var observer = new ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.VeiculosEmpresas = observer;
                SelectedIndex = 0;


            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void CarregaColeçãoEmpresasLayoutsCrachas(int _colaboradorEmpresaID = 0)
        {
            try
            {
                var list1 = _auxiliaresService.LayoutCrachaService.Listar();
                var list2 = Mapper.Map<List<EmpresaLayoutCrachaView>>(list1);

                var observer = new ObservableCollection<EmpresaLayoutCrachaView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.EmpresasLayoutsCrachas = observer;
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void CarregaColeçãoFormatosCredenciais()
        {
            try
            {
                var list1 = _auxiliaresService.FormatoCredencialService.Listar();
                var list2 = Mapper.Map<List<ClasseFormatosCredenciais.FormatoCredencial>>(list1);

                var observer = new ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.FormatosCredenciais = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoTiposCredenciais()
        {
            try
            {
                var list1 = _auxiliaresService.TipoCredencialService.Listar();
                var list2 = Mapper.Map<List<ClasseTiposCredenciais.TipoCredencial>>(list1);

                var observer = new ObservableCollection<ClasseTiposCredenciais.TipoCredencial>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.TiposCredenciais = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }

        private void CarregaColecaoTecnologiasCredenciais()
        {
            try
            {
                var list1 = _auxiliaresService.TecnologiaCredencialService.Listar();
                var list2 = Mapper.Map<List<ClasseTecnologiasCredenciais.TecnologiaCredencial>>(list1);

                var observer = new ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.TecnologiasCredenciais = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }

        private void CarregaColecaoCredenciaisStatus()
        {
            try
            {
                var list1 = _auxiliaresService.CredencialStatusService.Listar();
                var list2 = Mapper.Map<List<ClasseCredenciaisStatus.CredencialStatus>>(list1);

                var observer = new ObservableCollection<ClasseCredenciaisStatus.CredencialStatus>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.CredenciaisStatus = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }

        public void CarregaColecaoCredenciaisMotivos(int tipo = 0)
        {
            try
            {
                var list1 = _auxiliaresService.CredencialMotivoService.Listar();
                var list2 = Mapper.Map<List<ClasseCredenciaisMotivos.CredencialMotivo>>(list1);

                var observer = new ObservableCollection<ClasseCredenciaisMotivos.CredencialMotivo>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.CredenciaisMotivos = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }

        #endregion

        private DateTime validadeCursoContrato(int _colaborador = 0)
        {
            try
            {

                //DateTime _menorDataCurso = Convert.ToDateTime("01-01-2999");
                //DateTime _menorDataContrato = Convert.ToDateTime("01-01-2999");

                string _menorDataCurso = "";
                string _menorDataContrato = "";

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _strSql = "SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome,CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103) " +
                                 "as ValidadeCurso,DATEDIFF(DAY, GETDATE(), CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103)) AS Dias FROM dbo.Colaboradores " +
                                 "INNER JOIN dbo.ColaboradoresCursos ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresCursos.ColaboradorID where dbo.Colaboradores.Excluida = 0 And dbo.ColaboradoresCursos.Controlado = 1 And dbo.ColaboradoresCursos.ColaboradorID = " + _colaborador + " Order By Dias";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                if (_sqlreader.Read())
                {

                    //if (Convert.ToInt32(_sqlreader["Dias"]) < 30)
                    //{
                    //MessageBox.Show("Data de Vinculo!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _menorDataCurso = _sqlreader["ValidadeCurso"].ToString();
                    // break;
                    //}

                }
                _sqlreader.Close();


                //_strSql = "SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome, dbo.EmpresasContratos.EmpresaID, dbo.EmpresasContratos.NumeroContrato, " +
                //          "CONVERT(datetime,dbo.EmpresasContratos.Validade,103) as DataContrato, DATEDIFF ( DAY , GETDATE(),  CONVERT(datetime, dbo.EmpresasContratos.Validade,103))  AS Dias " +
                //          "FROM  dbo.EmpresasContratos INNER JOIN dbo.ColaboradoresCredenciais ON dbo.EmpresasContratos.EmpresaID = dbo.ColaboradoresCredenciais.EmpresaID INNER JOIN dbo.Colaboradores " +
                //          "ON dbo.ColaboradoresCredenciais.ColaboradorID = dbo.Colaboradores.ColaboradorID WHERE (dbo.Colaboradores.Excluida = 0) And dbo.Colaboradores.ColaboradorID = " + _colaborador + " Order By Dias";

                //_sqlcmd = new SqlCommand(_strSql, _Con);
                //_sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                //if (_sqlreader.Read())
                //{

                //    // if (Convert.ToInt32(_sqlreader["Dias"]) < 30)
                //    //{
                //    //MessageBox.Show("Data de Vinculo!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    _menorDataContrato = _sqlreader["DataContrato"].ToString();
                //    // break;
                //    //}

                //}
                //_sqlreader.Close();



                //if (Convert.ToDateTime(_menorDataCurso) < Convert.ToDateTime(_menorDataContrato))
                //{
                return Convert.ToDateTime(_menorDataCurso);
                //}
                //else if (Convert.ToDateTime(_menorDataCurso) > Convert.ToDateTime(_menorDataContrato))
                //{
                //    return Convert.ToDateTime(_menorDataContrato);
                //}

                //return DateTime.Now;

            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }

        #region Metodos Privados


        public void OnImprimirCommand()
        {
            //try
            //{
            //    if (VeiculoCredencialSelecionado.Validade == null || !VeiculoCredencialSelecionado.Ativa ||
            //        VeiculoCredencialSelecionado.LayoutCrachaID == 0)
            //    {
            //        Global.PopupBox("Não é possível imprimir esta credencial!", 4);
            //        return;
            //    }

            //    //if (!Global.PopupBox("Confirma Impressão da Credencial para " + ColaboradorCredencialSelecionado.ColaboradorNome, 2))
            //    //{
            //    //    return;
            //    //}

            //    string _xml = RequisitaCredencial(VeiculoCredencialSelecionado.VeiculoCredencialID);

            //    XmlSerializer deserializer = new XmlSerializer(typeof(ClasseCredencial));

            //    XmlDocument xmldocument = new XmlDocument();
            //    xmldocument.LoadXml(_xml);

            //    TextReader reader = new StringReader(_xml);
            //    ClasseCredencial Credencial = new ClasseCredencial();
            //    Credencial = (ClasseCredencial)deserializer.Deserialize(reader);

            //    ;
            //    string _ArquivoRPT = System.IO.Path.GetRandomFileName();
            //    _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;
            //    _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
            //    byte[] buffer = Convert.FromBase64String(Credencial.LayoutRPT.Trim());

            //    System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

            //    ReportDocument reportDocument = new ReportDocument();

            //    reportDocument.Load(_ArquivoRPT);

            //    //var report = new Cracha();
            //    var x = new List<ClasseCredencial>();
            //    x.Add(Credencial);
            //    reportDocument.SetDataSource(x);

            //    //Thread CarregaCracha_thr = new Thread(() =>
            //    //{

            //    //    System.Windows.Application.Current.Dispatcher.Invoke(() =>
            //    //    {

            //    PopupCredencial _popupCredencial = new PopupCredencial(reportDocument);
            //    _popupCredencial.ShowDialog();

            //    bool _result = _popupCredencial.Result;
            //    //    });

            //    //}
            //    //);

            //    //CarregaCracha_thr.Start();

            //    // GenericReportViewer.ViewerCore.ReportSource = reportDocument;




            //    //bool _resposta = SCManager.ImprimirCredencial(ColaboradorCredencialSelecionado);
            //    //if (_resposta)
            //    //{
            //    if (_result)
            //    {
            //        System.IO.File.Delete(_ArquivoRPT);
            //        InsereImpressaoDB(VeiculoCredencialSelecionado.VeiculoCredencialID);
            //        // Global.PopupBox("Impressão Efetuada com Sucesso!", 1);
            //        VeiculoCredencialSelecionado.Impressa = true;
            //        int _selectindex = SelectedIndex;
            //        CarregaColecaoVeiculosCredenciais(VeiculoCredencialSelecionado.VeiculoID); //revisar a necessidade do carregamento
            //        SelectedIndex = _selectindex;
            //    }

            //    //}

            //}
            //catch (Exception ex)
            //{
            //}
        }

        private DateTime VerificarMenorData(int _colaborador)
        {
            try
            {

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                // Dim _dataValidade As DateTime = Date.Now
                DateTime _menorData = DateTime.Now;
                // Dim _menorDataContrato As String = ""


                var sqlSelectCurso = @"SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome, dbo.ColaboradoresCursos.Validade, dbo.ColaboradoresCursos.Controlado
                       FROM dbo.Colaboradores INNER JOIN
                         dbo.ColaboradoresCursos ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresCursos.ColaboradorID
                       WHERE (dbo.Colaboradores.ColaboradorID = " + _colaborador + ") And (dbo.ColaboradoresCursos.Controlado = 1)";
                int idx = 0;
                // ---------------------------------------------------
                SqlCommand sqlcmd = new SqlCommand(sqlSelectCurso, _Con);
                SqlDataReader _sqlreader = sqlcmd.ExecuteReader(CommandBehavior.Default);
                var list = new List<Colaborador>();
                while (_sqlreader.Read()) // Popular dados
                {
                    list.Add(new Colaborador() { Id = idx, ColaboradorId = Convert.ToInt32(_sqlreader["ColaboradorID"].ToString()), DataValidade = Convert.ToDateTime(_sqlreader["Validade"].ToString()) });
                    idx = idx + 1;
                }

                var sqlSelectContrato = @"SELECT dbo.EmpresasContratos.EmpresaID, dbo.ColaboradoresEmpresas.EmpresaContratoID, dbo.ColaboradoresEmpresas.ColaboradorID,
                                    dbo.ColaboradoresEmpresas.Cargo, dbo.ColaboradoresEmpresas.Matricula, dbo.ColaboradoresCredenciais.NumeroCredencial,
                                    dbo.EmpresasContratos.Validade AS ValidadeContrato, dbo.ColaboradoresCredenciais.CredencialGUID, dbo.ColaboradoresEmpresas.Ativo
                                    FROM dbo.EmpresasContratos RIGHT OUTER JOIN dbo.ColaboradoresEmpresas ON dbo.EmpresasContratos.EmpresaContratoID = dbo.ColaboradoresEmpresas.EmpresaContratoID
                                    FULL OUTER JOIN dbo.ColaboradoresCredenciais ON dbo.ColaboradoresEmpresas.ColaboradorEmpresaID = dbo.ColaboradoresCredenciais.ColaboradorEmpresaID 
                                    WHERE dbo.ColaboradoresEmpresas.Ativo = 1 AND dbo.ColaboradoresEmpresas.ColaboradorID = " + _colaborador;

                SqlCommand sqlcmd2 = new SqlCommand(sqlSelectContrato, _Con);
                SqlDataReader _sqlreader2 = sqlcmd2.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader2.Read()) // Popular dados
                {
                    list.Add(new Colaborador() { Id = idx, CredencialGuidId = _sqlreader2["CredencialGUID"].ToString(), ColaboradorId = Convert.ToInt32(_sqlreader2["ColaboradorID"].ToString()), DataValidade = Convert.ToDateTime(_sqlreader2["ValidadeContrato"].ToString()) });
                    idx = idx + 1;
                }

                // group by colaboradores
                var colaboradoresGroup = list.GroupBy(n => n.ColaboradorId).Select(n => n.First()).ToList();
                // ----------------------------------------------------
                // Itera entre os colaboradores e retorna o item com a menor data
                foreach (Colaborador item in colaboradoresGroup)
                {
                    var d1 = (from d in list
                              where d.ColaboradorId == item.ColaboradorId
                              select d.DataValidade).Min();
                    var dados = list.Where(x => x.DataValidade.Equals(d1) & x.ColaboradorId.Equals(item.ColaboradorId)).FirstOrDefault();
                    _menorData = dados.DataValidade;
                }
                // ----------------------------------------------------
                return _menorData;
            }
            catch (Exception ex)
            {

            }

            return DateTime.Now;
        }

        #endregion
    }
}
