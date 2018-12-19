// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresasContratosViewModel : ViewModelBase
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaContratosService _empresaContratosService = new EmpresaContratoService();

        #region  Propriedades

        /// <summary>
        ///     Lista de municipios
        /// </summary>
        public List<ClasseMunicipios.Municipio> ObterListaListaMunicipios { get; private set; }

        /// <summary>
        ///     Lista de estados
        /// </summary>
        public List<ClasseEstados.Estado> ObterListaEstadosFederacao { get; private set; }

        /// <summary>
        ///     Lista de sattus
        /// </summary>
        public List<ClasseStatus.Status> ObterListaStatus { get; private set; }

        /// <summary>
        ///     Lista de tipos de cobrança
        /// </summary>
        public List<ClasseTiposCobrancas.TipoCobranca> ObterListaTiposCobranca { get; private set; }

        /// <summary>
        ///     Lista de tipos de acessos
        /// </summary>
        public List<ClasseTiposAcessos.TipoAcesso> ObterListaTipoAcessos { get; private set; }

        public ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> Contratos
        {
            get { return _contratos; }

            set
            {
                if (_contratos != value)
                {
                    _contratos = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ClasseEstados.Estado> Estados
        {
            get { return _estados; }

            set
            {
                if (_estados != value)
                {
                    _estados = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ClasseMunicipios.Municipio> Municipios
        {
            get { return _municipios; }

            set
            {
                if (_municipios != value)
                {
                    _municipios = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ClasseStatus.Status> Status
        {
            get { return _statuss; }

            set
            {
                if (_statuss != value)
                {
                    _statuss = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ClasseTiposAcessos.TipoAcesso> TiposAcessos
        {
            get { return _tiposAcessos; }

            set
            {
                if (_tiposAcessos != value)
                {
                    _tiposAcessos = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ClasseTiposCobrancas.TipoCobranca> TiposCobrancas
        {
            get { return _tiposCobrancas; }

            set
            {
                if (_tiposCobrancas != value)
                {
                    _tiposCobrancas = value;
                    OnPropertyChanged();
                }
            }
        }

        public ClasseEmpresasContratos.EmpresaContrato ContratoSelecionado
        {
            get { return _contratoSelecionado; }
            set
            {
                _contratoSelecionado = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public int EmpresaSelecionadaId
        {
            get { return _empresaSelecionadaId; }
            set
            {
                _empresaSelecionadaId = value;
                OnPropertyChanged();
                if (EmpresaSelecionadaId != null)
                {
                    //OnEmpresaSelecionada();
                }
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public bool HabilitaEdicao
        {
            get { return _habilitaEdicao; }
            set
            {
                _habilitaEdicao = value;
                OnPropertyChanged();
            }
        }

        public string Criterios
        {
            get { return _criterios; }
            set
            {
                _criterios = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region  Metodos

        #region Data Access

        private void ObterContratos(int empresaId, string descricao, string numContrato)
        {
            try
            {
                Contratos = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato>();
                ICollection<EmpresaContrato> p1;

                if (empresaId != 0)
                {
                    p1 = _empresaContratosService.ListarPorEmpresa(empresaId);
                    var convert = Mapper.Map<List<ClasseEmpresasContratos.EmpresaContrato>>(p1);
                    convert.ForEach(n => { Contratos.Add(n); });
                    return;
                }

                if (!string.IsNullOrWhiteSpace(descricao))
                {
                    p1 = _empresaContratosService.ListarPorDescricao(descricao);
                    var convert = Mapper.Map<List<ClasseEmpresasContratos.EmpresaContrato>>(p1);
                    convert.ForEach(n => { Contratos.Add(n); });
                    return;
                }

                if (!string.IsNullOrWhiteSpace(numContrato))
                {
                    p1 = _empresaContratosService.ListarPorNumeroContrato(numContrato);
                    var convert = Mapper.Map<List<ClasseEmpresasContratos.EmpresaContrato>>(p1);
                    convert.ForEach(n => { Contratos.Add(n); });
                    return;
                }

                //Hotfix auto-selecionar registro do topo da ListView
                var topList = Contratos.FirstOrDefault();
                ContratoSelecionado = topList;

                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

        #endregion

        #region Inicializacao

        public EmpresasContratosViewModel()
        {
            CarregarDadosComunsEmMemoria();

            CarregaColecaoEstados();
            CarregaColecaoStatus();
            CarregaColeçãoTiposAcessos();
            CarregaColeçãoTiposCobrancas();
        }

        /// <summary>
        ///     Carregar dados auxiliares em memória
        /// </summary>
        private void CarregarDadosComunsEmMemoria()
        {
            //Estados
            var e1 = _auxiliaresService.EstadoService.Listar();
            ObterListaEstadosFederacao = Mapper.Map<List<ClasseEstados.Estado>>(e1);
            //Municipios
            var list = _auxiliaresService.MunicipioService.Listar();
            ObterListaListaMunicipios = Mapper.Map<List<ClasseMunicipios.Municipio>>(list);
            //Status
            var e3 = _auxiliaresService.StatusService.Listar();
            ObterListaStatus = Mapper.Map<List<ClasseStatus.Status>>(e3);
            //Tipos Cobrança
            var e4 = _auxiliaresService.TipoCobrancaService.Listar();
            ObterListaTiposCobranca = Mapper.Map<List<ClasseTiposCobrancas.TipoCobranca>>(e4);
            //Tipo de Acesso
            var e5 = _auxiliaresService.TiposAcessoService.Listar();
            ObterListaTipoAcessos = Mapper.Map<List<ClasseTiposAcessos.TipoAcesso>>(e5);
        }

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> _contratos;

        private ClasseEmpresasContratos.EmpresaContrato _contratoSelecionado;

        private ClasseEmpresasContratos.EmpresaContrato _contratoTemp = new ClasseEmpresasContratos.EmpresaContrato();

        private readonly List<ClasseEmpresasContratos.EmpresaContrato> _contratosTemp = new List<ClasseEmpresasContratos.EmpresaContrato>();

        private ObservableCollection<ClasseEstados.Estado> _estados;

        private ObservableCollection<ClasseMunicipios.Municipio> _municipios;

        private ObservableCollection<ClasseStatus.Status> _statuss;

        private ObservableCollection<ClasseTiposAcessos.TipoAcesso> _tiposAcessos;

        private ObservableCollection<ClasseTiposCobrancas.TipoCobranca> _tiposCobrancas;

        private PopupPesquisaContrato _popupPesquisaContrato;

        private int _selectedIndex;

        private int _empresaSelecionadaId;

        private bool _habilitaEdicao;

        private string _criterios = "";

        private int _selectedIndexTemp;

        #endregion

        #region Comandos dos Botoes

        public void OnAtualizaCommand(object idEmpresa)
        {
            EmpresaSelecionadaId = Convert.ToInt32(idEmpresa);
            ObterContratos(EmpresaSelecionadaId, "", "");
        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _contratoTemp.NomeArquivo = arq.Nome;
                _contratoTemp.Arquivo = arq.FormatoBase64;
                if (Contratos != null)
                    Contratos[0].NomeArquivo = arq.Nome;

            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }

        }

        public void OnAbrirArquivoCommand()
        {
            try
            {
                var arquivoStr = ContratoSelecionado.Arquivo;
                var nomeArquivo = ContratoSelecionado.NomeArquivo;
                var arrBytes = Convert.FromBase64String(arquivoStr);
                WpfHelp.DownloadArquivoDialog(nomeArquivo, arrBytes);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnEditarCommand()
        {
            try
            {
                //BuscaBadges();
                _contratoTemp = ContratoSelecionado.CriaCopia(ContratoSelecionado);
                _selectedIndexTemp = SelectedIndex;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Contratos)
                {
                    _contratosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                Contratos.Clear();
                _contratoTemp = new ClasseEmpresasContratos.EmpresaContrato { EmpresaID = EmpresaSelecionadaId };
                Contratos.Add(_contratoTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnCancelarEdicaoCommand()
        {
            try
            {
                Contratos[_selectedIndexTemp] = _contratoTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand()
        {
            try
            {
                var entity = Mapper.Map<EmpresaContrato>(ContratoSelecionado);
                _empresaContratosService.Alterar(entity);

                _contratosTemp.Clear();
                _contratoTemp = null;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                var entity = Mapper.Map<EmpresaContrato>(ContratoSelecionado);
                _empresaContratosService.Criar(entity);

                _contratosTemp.Clear();
                _contratoTemp = null;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Contratos = null;
                Contratos = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato>(_contratosTemp);
                SelectedIndex = _selectedIndexTemp;
                _contratosTemp.Clear();
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnExcluirCommand()
        {
            try
            {

                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        var entity = Mapper.Map<EmpresaContrato>(ContratoSelecionado);
                        _empresaContratosService.Remover(entity);

                        Contratos.Remove(ContratoSelecionado);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnPesquisarCommand()
        {
            try
            {
                _popupPesquisaContrato = new PopupPesquisaContrato();
                _popupPesquisaContrato.EfetuarProcura += On_EfetuarProcura;
                _popupPesquisaContrato.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = _popupPesquisaContrato.Criterio.Split((char)20);
            var descricao = ((string[])vetor)[0];
            var numContrato = ((string[])vetor)[1];
            ObterContratos(0, descricao, numContrato);
            SelectedIndex = 0;
        }

        #endregion

        #region Dados Auxiliares

        private void CarregaColecaoEstados()
        {
            try
            {
                var convert = Mapper.Map<List<ClasseEstados.Estado>>(ObterListaEstadosFederacao);
                Estados = new ObservableCollection<ClasseEstados.Estado>();
                convert.ForEach(n => { Estados.Add(n); });
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoMunicipios(string uf)
        {
            try
            {
                var list = ObterListaListaMunicipios.Where(n => n.UF == uf).ToList();
                Municipios = new ObservableCollection<ClasseMunicipios.Municipio>();
                list.ForEach(n => Municipios.Add(n));
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoStatus()
        {
            try
            {
                Status = new ObservableCollection<ClasseStatus.Status>();
                ObterListaStatus.ForEach(n => { Status.Add(n); });
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColeçãoTiposAcessos()
        {
            try
            {
                TiposAcessos = new ObservableCollection<ClasseTiposAcessos.TipoAcesso>();
                ObterListaTipoAcessos.ForEach(n => { TiposAcessos.Add(n); });
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColeçãoTiposCobrancas()
        {
            try
            {
                TiposCobrancas = new ObservableCollection<ClasseTiposCobrancas.TipoCobranca>();
                ObterListaTiposCobranca.ForEach(n => { TiposCobrancas.Add(n); });
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

    }
}