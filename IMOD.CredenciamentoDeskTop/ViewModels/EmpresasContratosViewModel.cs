﻿// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels.Commands;
using IMOD.CredenciamentoDeskTop.ViewModels.Comportamento;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class EmpresasContratosViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaContratosService _service = new EmpresaContratoService();
        private EmpresaView _empresaView;
        private readonly IColaboradorEmpresaService _colaboradorEmpresaservice = new ColaboradorEmpresaService();
        private readonly IEmpresaContratosService _serviceContratos = new EmpresaContratoService();
        private ConfiguraSistema _configuraSistema;

        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;

        private EmpresaViewModel _viewModelParent;

        #region  Propriedades
        public string ExcluirVisivel { get; set; }
        /// <summary>
        ///     Lista de municipios
        /// </summary>
        public List<Municipio> Municipios { get; private set; }

        public Estados Estado { get; set; }

        /// <summary>
        ///     Dados de municipio armazendas em memoria
        /// </summary>
        private List<Municipio> _municipios;

        /// <summary>
        ///     Lista de estados
        /// </summary>
        public List<Estados> Estados { get; private set; }

        /// <summary>
        ///     Lista de sattus
        /// </summary>
        public List<Status> Status { get; private set; }

        /// <summary>
        ///     Lista de tipos de cobrança
        /// </summary>
        public List<TipoCobranca> TiposCobranca { get; private set; }

        /// <summary>
        ///     Lista de tipos de acessos
        /// </summary>
        public List<TipoAcesso> ListaTipoAcessos { get; private set; }

        public ObservableCollection<EmpresaContratoView> EntityObserver { get; set; }
        public EmpresaContratoView Entity { get; set; }

        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;
        /// <summary>
        ///     Tamanho da Imagem
        /// </summary>
        public int IsTamanhoArquivo
        {
            get
            {
                return _configuraSistema.arquivoTamanho;
            }
        }
        #endregion

        public EmpresasContratosViewModel()
        {
            if (!Domain.EntitiesCustom.UsuarioLogado.Adm)
            {
                ExcluirVisivel = "Collapsed";
            }
            else
            {
                ExcluirVisivel = "Visible";
            }
            ListarDadosAuxiliares();
            ItensDePesquisaConfigura();
            Comportamento = new ComportamentoBasico (false, true, true, false, false);
            EntityObserver = new ObservableCollection<EmpresaContratoView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
            PropertyChanged += OnEntityChanged;
        }

        #region  Metodos
        public void BuscarAnexo(int EmpresaContratoId)
        {
            try
            {
                if (Entity.Arquivo != null) return;
                var anexo = _service.BuscarPelaChave(EmpresaContratoId);
                Entity.Arquivo = anexo.Arquivo;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Entity")
            {
                Comportamento.IsEnableEditar = Entity != null;
                Comportamento.isEnableRemover = Entity != null;
            }
        }

        /// <summary>
        ///     Carregar dados auxiliares em memória
        /// </summary>
        private void ListarDadosAuxiliares()
        {
            //Estados
            var lst1 = _auxiliaresService.EstadoService.Listar();
            Estados = new List<Estados>();
            Estados.AddRange (lst1);
            //Status
            var lst2 = _auxiliaresService.StatusService.Listar();
            Status = new List<Status>();
            Status.AddRange (lst2);
            //Tipos Cobrança
            var lst3 = _auxiliaresService.TipoCobrancaService.Listar();
            TiposCobranca = new List<TipoCobranca>();
            TiposCobranca.AddRange (lst3);
            //Tipo de Acesso
            var lst4 = _auxiliaresService.TiposAcessoService.Listar();
            ListaTipoAcessos = new List<TipoAcesso>();
            ListaTipoAcessos.AddRange (lst4);
            _configuraSistema = ObterConfiguracao();
        }
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresService.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }
        /// <summary>
        ///     Listar Municipios
        /// </summary>
        /// <param name="uf"></param>
        public void ListarMunicipios(string uf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace (uf)) return;

                if (Municipios == null) Municipios = new List<Municipio>();

                if (_municipios == null) _municipios = new List<Municipio>();

                if (Estado == null) return;

                //Verificar se há municipios já carregados...
                var l1 = _municipios.Where (n => n.Uf == uf);
                Municipios.Clear();
                //Nao havendo municipios... obter do repositorio
                if (!l1.Any())
                {
                    var l2 = _auxiliaresService.MunicipioService.Listar (null, uf);
                    _municipios.AddRange (Mapper.Map<List<Municipio>> (l2));
                }

                var municipios = _municipios.Where (n => n.Uf == uf).ToList();
                Municipios = municipios;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void AtualizarDados(EmpresaView entity, EmpresaViewModel viewModelParent)
        {
            EntityObserver.Clear();
            if (entity == null) return;// throw new ArgumentNullException (nameof(entity));
            _empresaView = entity;
            _viewModelParent = viewModelParent;
            //Obter dados
            var list1 = _service.Listar (entity.EmpresaId, null, null, null, null, null, null, null, 1);
            //var list2 = Mapper.Map<List<EmpresaContratoView>> (list1.OrderByDescending (n => n.EmpresaContratoId));
            var list2 = Mapper.Map<List<EmpresaContratoView>>(list1.OrderBy(n => n.Validade));
            EntityObserver = new ObservableCollection<EmpresaContratoView>();
            list2.ForEach (n => { EntityObserver.Add (n); });
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add (new KeyValuePair<int, string> (1, "Nome"));
            ListaPesquisa.Add (new KeyValuePair<int, string> (2, "Todos"));
            PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
        }

        /// <summary>
        ///     Acionado antes de remover
        /// </summary>
        private void PrepareRemover()
        {
            Comportamento.PrepareRemover();
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = false;
        }

        /// <summary>
        ///     Criar Dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;
                if (Validar()) return;

                var n1 = Mapper.Map<EmpresaContrato> (Entity);
                n1.EmpresaId = _empresaView.EmpresaId;
                n1.Usuario = Domain.EntitiesCustom.UsuarioLogado.Nome;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<EmpresaContratoView> (n1);
                EntityObserver.Insert (0, n2);
                IsEnableLstView = true;
                _viewModelParent.AtualizarDadosPendencias();
                SelectListViewIndex = 0;
                _viewModelParent.HabilitaControleTabControls (true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            Entity = new EmpresaContratoView();
            Comportamento.PrepareCriar();
            _prepareCriarCommandAcionado = true;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls (false, false, false, true);
        }

        /// <summary>
        ///     Editar dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;

                if (Validar()) return;

                var n1 = Mapper.Map<EmpresaContrato> (Entity);
                n1.Usuario = Domain.EntitiesCustom.UsuarioLogado.Nome;
                _service.Alterar (n1);
                IsEnableLstView = true;
                _viewModelParent.HabilitaControleTabControls (true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        ///     Cancelar operação
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            try
            {
                IsEnableLstView = true;
                if (Entity != null) Entity.ClearMessageErro();
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                Entity = null;
                _viewModelParent.HabilitaControleTabControls (true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        /// <summary>
        ///     Remover dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemover(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;

                var verificarColaborador = _colaboradorEmpresaservice.Listar(null, null, null, null, null, null, Entity.EmpresaContratoId);
                if (verificarColaborador.Count == 0)
                {
                    var podeCobrarResult = WpfHelp.MboxDialogYesNo("Confirma DELEÇÃO desse contrato?",true);
                    if (podeCobrarResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        var n1 = Mapper.Map<EmpresaContrato>(Entity);
                        _service.Remover(n1);
                        //Retirar empresa da coleção
                        EntityObserver.Remove(Entity);
                    }                       
                }
                else
                {
                    WpfHelp.PopupBox("Contrato não pode ser DELETADO, existem " + verificarColaborador.Count + " colaboradores vinculados. Ação cancelada.", 1);
                }
                _viewModelParent.HabilitaControleTabControls (true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        private void PrepareSalvar()
        {
            if (Validar()) return;
            Comportamento.PrepareSalvar();
        }

        /// <summary>
        ///     Acionado antes de alterar
        /// </summary>
        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                WpfHelp.PopupBox ("Selecione um item da lista", 1);
                return;
            }

            Comportamento.PrepareAlterar();
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls (false, false, false, true, false);
        }

        /// <summary>
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                var pesquisa = NomePesquisa;
                var num = PesquisarPor;

                //Por nome
                if (num.Key == 1)
                {
                    if (string.IsNullOrWhiteSpace (pesquisa)) return;

                    var l1 = EntityObserver.Where (n => n.Descricao
                        .ToLower()
                        .Contains (pesquisa.ToLower())).ToList();
                    EntityObserver = new ObservableCollection<EmpresaContratoView>();
                    l1.ForEach (n => { EntityObserver.Add (n); });
                }

                if (num.Key == 2)
                {
                    //Obet itens do observer
                    var l1 = _service.Listar (Entity.EmpresaId);
                    PopularObserver (l1);
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void PopularObserver(ICollection<EmpresaContrato> list)
        {
            try
            {
                var list2 = Mapper.Map<List<EmpresaContratoView>> (list);
                EntityObserver = new ObservableCollection<EmpresaContratoView>();
                list2.ForEach (n => { EntityObserver.Add (n); });
            }

            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #region Regras de Negócio

        private bool ExisteNumContrato()
        {
            if (Entity == null) return false;
            var numContrato = Entity.NumeroContrato;
            if (string.IsNullOrWhiteSpace (numContrato)) throw new ArgumentNullException ("Informe um número de contrato para pesquisar.");
            //Verificar dados antes de salvar uma criação
            if (_prepareCriarCommandAcionado)
            {
                //Verificar se existe numero de contrato
                var n1 = _service.BuscarContrato (numContrato);
                if (n1 != null) return true;
            }

            //Verificar dados antes de salvar uma alteraçao
            if (!_prepareAlterarCommandAcionado) return false;
            //var n2 = _service.BuscarPelaChave (Entity.EmpresaContratoId);
            var n2 = _service.Listar(null,Entity.NumeroContrato).FirstOrDefault();
            if (n2 != null)
            {
                return string.Compare(n2.NumeroContrato,
                                       numContrato, StringComparison.Ordinal) != 0;
            }
            return false;
        }

        #endregion

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        public bool Validar()
        {
            if (Entity == null) return true;
            Entity.Validate();
            var hasErros = Entity.HasErrors;
            if (hasErros) return true;

            if (ExisteNumContrato())
                Entity.SetMessageErro ("NumeroContrato", "Número de contrato já existente.");

            return Entity.HasErrors;
        }

        #endregion

        #region Propriedade de Pesquisa

        /// <summary>
        ///     String contendo o nome a pesquisa;
        /// </summary>
        public string NomePesquisa { get; set; }

        /// <summary>
        ///     Opções de pesquisa
        /// </summary>
        public List<KeyValuePair<int, string>> ListaPesquisa { get; private set; }

        /// <summary>
        ///     Pesquisar por
        /// </summary>
        public KeyValuePair<int, string> PesquisarPor { get; set; }

        #endregion

        #region Propriedade Commands

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase (PrepareCriar, true);

        public ComportamentoBasico Comportamento { get; set; }

        /// <summary>
        ///     Editar
        /// </summary>
        public ICommand PrepareAlterarCommand => new CommandBase (PrepareAlterar, true);

        /// <summary>
        ///     Cancelar
        /// </summary>
        public ICommand PrepareCancelarCommand => new CommandBase (Comportamento.PrepareCancelar, true);

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareSalvarCommand => new CommandBase (PrepareSalvar, true);

        /// <summary>
        ///     Remover
        /// </summary>
        public ICommand PrepareRemoverCommand => new CommandBase (PrepareRemover, true);

        /// <summary>
        ///     Pesquisar
        /// </summary>
        public ICommand PesquisarCommand => new CommandBase (Pesquisar, true);

        #endregion
    }
}