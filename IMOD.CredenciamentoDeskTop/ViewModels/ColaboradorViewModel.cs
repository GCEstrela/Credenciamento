// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Funcoes;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels.Commands;
using IMOD.CredenciamentoDeskTop.ViewModels.Comportamento;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Infra.Servicos;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class ColaboradorViewModel : ViewModelBase, IComportamento, IAtualizarDados
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IColaboradorService _service = new ColaboradorService();
        private readonly IColaboradorWebService _serviceWeb = new ColaboradorWebService();
        private readonly IColaboradorEmpresaService _serviceColaboradorEmpresa = new ColaboradorEmpresaService();
        private readonly IColaboradorEmpresaWebService _serviceColaboradorEmpresaWeb = new ColaboradorEmpresaWebService();
        private readonly IColaboradorAnexoWebService _serviceColaboradorAnexoWeb = new ColaboradorAnexoWebService();
        private readonly IColaboradorAnexoService _serviceColaboradorAnexo = new ColaboradorAnexoService();
        private readonly IColaboradorCursoWebService _serviceColaboradorCursoWeb = new ColaboradorCursosWebService();
        private readonly IColaboradorCursoService _serviceColaboradorCurso = new ColaboradorCursosService();
        private readonly IColaboradorObservacaoService _serviceColaboradorObservacao = new ColaboradorObservacaoService();
        private readonly IColaboradorCredencialService _serviceColaboradorCredencial = new ColaboradorCredencialService();
        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();

        private readonly IColaboradorCredencialService _serviceCredencialSC = new ColaboradorCredencialService();

        private ConfiguraSistema _configuraSistema;
        private bool isReprovacao = false;

        #region  Propriedades
        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;
        public bool IsEnablePreCadastro { get; set; } = false;
        public bool IsEnablePreCadastroCredenciamento { get; set; } = true;
        public string IsEnablePreCadastroColor { get; set; } = "#FFD0D0D0";
        public string IsLabelAtivoInativo { get; set; } = "Ativo";
        public string IsBotaoAtivoInativo { get; set; } = "Inativar";
        public string IsBotaoCorAtivoInativo { get; set; } = "#FF008000";
        public string IsLabelCorAtivoInativo { get; set; } = "#FF008000";
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }
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

        /// <summary>
        ///     True, empresa possui pendência de codigo 21
        /// </summary>
        public bool Pendencia21 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 22
        /// </summary>
        public bool Pendencia22 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 23
        /// </summary>
        public bool Pendencia23 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 24
        /// </summary>
        public bool Pendencia24 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 25
        /// </summary>
        public bool Pendencia25 { get; set; }

        public bool HabilitaCommandPincipal { get; set; } = true;
        /// <summary>
        ///     Resolução da webCam
        /// </summary>
        public int IsResolucao
        {
            get
            {
                return _configuraSistema.imagemResolucao;
            }
        }
        /// <summary>
        ///     Resolução da webCam
        /// </summary>
        public int IsTamanhoImagem
        {
            get
            {
                return _configuraSistema.imagemTamanho;
            }
        }
        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }


        public bool IsEnableTabItemGeral { get; set; }
        public bool IsEnableTabItemEmpresas { get; set; }
        public bool IsEnableTabItemCursos { get; set; }
        public bool IsEnableTabItemAnexos { get; set; }
        public bool IsEnableTabItemCredenciais { get; set; }
        public bool IsEnableTabItemObservacoes { get; set; }

        /// <summary>
        ///     Seleciona o indice da tabcontrol desejada
        /// </summary>
        public short SelectedTabIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        public ColaboradorView Entity { get; set; }
        public ObservableCollection<ColaboradorView> EntityObserver { get; set; }

        /// <summary>
        ///     Estados
        /// </summary>
        public List<Estados> Estados { get; set; }

        public Estados Estado { get; set; }

        /// <summary>
        ///     Municipios
        /// </summary>
        public List<Municipio> Municipios { get; set; }

        /// <summary>
        ///     Dados de municipio armazenadas em memoria
        /// </summary>
        public List<Municipio> _municipios { get; set; }

        public bool existePreCadastro { get; set; }

        public string msgPreCadastro { get; set; }


        #endregion

        public ColaboradorViewModel()
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                ItensDePesquisaConfigura();
                ListarDadosAuxiliares();

                Comportamento = new ComportamentoBasico(false, true, true, false, false);
                EntityObserver = new ObservableCollection<ColaboradorView>();
                VerificaPreCadastroAprovacao();
                Comportamento.SalvarAdicao += OnSalvarAdicao;
                Comportamento.SalvarEdicao += OnSalvarEdicao;
                Comportamento.Remover += OnRemover;
                Comportamento.Cancelar += OnCancelar;
                PropertyChanged += OnEntityChanged;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
                Utils.TraceException(ex);
                throw ex;
            }
        }

        #region  Metodos

        /// <summary>
        ///     Habilita controles
        /// </summary>
        /// <param name="isEnableTabItem"></param>
        /// <param name="isEnableLstView"></param>
        private void HabilitaControle(bool isEnableTabItem, bool isEnableLstView)
        {
            HabilitaControleTabControls(isEnableLstView, isEnableTabItem, isEnableTabItem, isEnableTabItem, isEnableTabItem, isEnableTabItem, isEnableTabItem);
        }

        public void HabilitaControleTabControls(bool lstViewSuperior = true, bool isItemGeral = true, bool isItemEmpresas = false, bool isItemCursos = false, bool isItemAnexos = false, bool isItemCredenciais = false, bool isItemObservacoes = true)
        {
            try
            {
                IsEnableLstView = lstViewSuperior;

                IsEnableTabItemGeral = isItemGeral;
                IsEnableTabItemEmpresas = isItemEmpresas;
                IsEnableTabItemCursos = isItemCursos;
                IsEnableTabItemAnexos = isItemAnexos;
                IsEnableTabItemCredenciais = isItemCredenciais;
                IsEnableTabItemObservacoes = isItemObservacoes;
                Comportamento.IsEnableCriar = lstViewSuperior;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "Entity")

                {
                    var enableControls = Entity != null;
                    Comportamento.IsEnableEditar = enableControls;
                    HabilitaControleTabControls(true, enableControls, enableControls, enableControls, enableControls, enableControls, enableControls);
                }

                if (e.PropertyName == "SelectedTabIndex")
                {
                    //Habilita/Desabilita botoes principais...
                    HabilitaCommandPincipal = SelectedTabIndex == 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            try
            {
                ListaPesquisa = new List<KeyValuePair<int, string>>();
                ListaPesquisa.Add(new KeyValuePair<int, string>(1, "CPF"));
                ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Nome"));
                ListaPesquisa.Add(new KeyValuePair<int, string>(3, "Empresa"));
                ListaPesquisa.Add(new KeyValuePair<int, string>(4, "Colete"));
                ListaPesquisa.Add(new KeyValuePair<int, string>(5, "Matrícula"));
                ListaPesquisa.Add(new KeyValuePair<int, string>(6, "Todos os Colaboradores"));
                PesquisarPor = ListaPesquisa[1]; //Pesquisa Default
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PopularObserver(ICollection<Colaborador> list)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (!IsEnablePreCadastro)
                {
                    var list2 = Mapper.Map<List<ColaboradorView>>(list.ToList().OrderBy(c => c.Nome));
                    //var list2 = Mapper.Map<List<ColaboradorView>>(list.ToList());
                    //var list2 = Mapper.Map<List<ColaboradorView>>(list.ToList().OrderBy(c => c.Nome).Take(100));
                    //var list2 = Mapper.Map<List<ColaboradorView>>(list.ToList());
                    EntityObserver = new ObservableCollection<ColaboradorView>();
                    list2.ForEach(n => { EntityObserver.Add(n); });

                    //Havendo registros, selecione o primeiro
                    //if (EntityObserver.Any()) SelectListViewIndex = 0;
                }
                else
                {
                    var list2 = Mapper.Map<List<ColaboradorView>>(list.ToList().OrderBy(c => c.Nome));
                    EntityObserver = new ObservableCollection<ColaboradorView>();
                    list2.ForEach(n => { EntityObserver.Add(n); });
                    //Havendo registros, selecione o primeiro
                    //if (EntityObserver.Any()) SelectListViewIndex = 0;
                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
            }

            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
                Utils.TraceException(ex);
            }
        }
        public void carregaFoto(ImageSource minhaFoto)
        {


            //WriteableBitmap wbmp = new WriteableBitmap((BitmapSource)minhaFoto);
            //MemoryStream ms = new MemoryStream();
            ////wbmp.s(ms, wbmp.PixelWidth, wbmp.PixelHeight, 0, 100);
            ////return ms.ToArray();

            //var width = wbmp.PixelWidth;
            //var height = wbmp.PixelHeight;
            //var stride = width * ((wbmp.Format.BitsPerPixel + 7) / 8);

            //var bitmapData = new byte[height * stride];

            //wbmp.CopyPixels(bitmapData, stride, 0);


            //Entity.Foto = bitmapData.ToString();
        }


        public void Pesquisar()
        {
            try
            {
                var pesquisa = NomePesquisa;

                var num = PesquisarPor;

                //Todos
                if (num.Key == 6)
                {

                    var colaboradores = new List<Colaborador>();
                    //var l1 = _service.Listar();
                    if (IsEnablePreCadastro)
                    {
                        colaboradores = _serviceWeb.Listar(null, null, null, IsEnablePreCadastro).ToList();
                    }
                    else
                    {
                        colaboradores = _service.Listar(null, null, null, IsEnablePreCadastro).ToList();
                    }

                    PopularObserver(colaboradores);

                }
                //Matrícula
                if (num.Key == 5)
                {

                    if (string.IsNullOrWhiteSpace(pesquisa)) return;
                    var l1 = _service.Listar(null, null, null, IsEnablePreCadastro);
                    var l2 = _serviceColaboradorEmpresa.Listar(null, null, null, $"%{pesquisa}%", null, null);
                    var l3 = l2.Select(c => c.ColaboradorId).ToList<int>();
                    var l4 = l1.Where(c => l3.Contains(c.ColaboradorId)).ToList();

                    PopularObserver(l4);

                }
                //Colete
                if (num.Key == 4)
                {

                    if (string.IsNullOrWhiteSpace(pesquisa)) return;
                    var l1 = _service.Listar(null, null, null, IsEnablePreCadastro);
                    var l2 = _serviceColaboradorCredencial.Listar(null, null, null, null, null, null, null, null, null, $"%{pesquisa}%");
                    var l3 = l2.Select(c => c.ColaboradorId).ToList<int>();
                    var l4 = l1.Where(c => l3.Contains(c.ColaboradorId)).ToList();

                    PopularObserver(l4);

                }
                //Empresa
                if (num.Key == 3)
                {

                    if (string.IsNullOrWhiteSpace(pesquisa)) return;
                    var l1 = _service.Listar(null, null, null, IsEnablePreCadastro);
                    var l2 = _serviceColaboradorEmpresa.Listar(null, null, null, null, $"%{pesquisa}%", null);
                    var l3 = l2.Select(c => c.ColaboradorId).ToList<int>();
                    var l4 = l1.Where(c => l3.Contains(c.ColaboradorId)).ToList();

                    PopularObserver(l4);

                }

                //Por nome
                if (num.Key == 2)
                {
                    pesquisa.RetirarCaracteresEspeciais();
                    if (string.IsNullOrWhiteSpace(pesquisa)) return;
                    var l1 = _service.Listar(null, null, $"%{pesquisa}%", IsEnablePreCadastro);

                    PopularObserver(l1);

                }
                //Por cpf
                if (num.Key == 1)
                {
                    if (string.IsNullOrWhiteSpace(pesquisa)) return;
                    var l1 = _service.Listar(null, $"%{pesquisa.RetirarCaracteresEspeciais()}%", null, IsEnablePreCadastro);
                    PopularObserver(l1);

                }

                IsEnableLstView = true;
                VerificaPreCadastroAprovacao();
                if (Entity != null)
                    if (Entity.Ativo)
                    {
                        IsLabelAtivoInativo = "Ativo";
                        IsBotaoAtivoInativo = "Desativar";
                    }
                    else
                    {
                        IsLabelAtivoInativo = "Inativo";
                        IsBotaoAtivoInativo = "Ativar";
                    }
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex.Message, 1);
                Utils.TraceException(ex);
            }
        }
        public void BucarFoto(int colaborador)
        {
            try
            {
                Colaborador listaFoto;

                if (Entity.Foto != null) return;
                if (Entity.Precadastro)
                {
                    listaFoto = _serviceWeb.BuscarPelaChave(colaborador);
                }
                else
                {
                    listaFoto = _service.BuscarPelaChave(colaborador);
                }


                if (listaFoto != null)
                    Entity.Foto = listaFoto.Foto;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }
        public void ListarDadosAuxiliares()
        {
            try
            {
                var lst3 = _auxiliaresService.EstadoService.Listar();
                Estados = Mapper.Map<List<Estados>>(lst3);
                Municipios = new List<Municipio>();
                _municipios = new List<Municipio>();

                _configuraSistema = ObterConfiguracao();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Atualizar dados de pendências
        /// </summary>
        public void AtualizarDadosPendencias()
        {
            try
            {
                if (Entity == null) return;

                var pendencia = _service.Pendencia.ListarPorColaborador(Entity.ColaboradorId).ToList();
                //Set valores
                SetPendenciaFalse();
                //Buscar pendências referente aos códigos: 21;22;23;24;25
                Pendencia21 = pendencia.Any(n => n.CodPendencia == 21 & n.Ativo);
                Pendencia22 = pendencia.Any(n => n.CodPendencia == 22 & n.Ativo);
                Pendencia23 = pendencia.Any(n => n.CodPendencia == 23 & n.Ativo);
                Pendencia24 = pendencia.Any(n => n.CodPendencia == 24 & n.Ativo);
                Pendencia25 = pendencia.Any(n => n.CodPendencia == 25 & n.Ativo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetPendenciaFalse()
        {
            Pendencia21 = false;
            Pendencia22 = false;
            Pendencia23 = false;
            Pendencia24 = false;
            Pendencia25 = false;
        }

        #endregion

        #region Regras de Negócio

        public bool ExisteCpf()
        {
            try
            {
                if (Entity == null) return false;
                var cpf = Entity.Cpf.RetirarCaracteresEspeciais();

                //Verificar dados antes de salvar uma criação
                if (_prepareCriarCommandAcionado)
                    if (_service.ExisteCpf(cpf)) return true;
                //Verificar dados antes de salvar uma alteraçao
                if (!_prepareAlterarCommandAcionado) return false;
                var n1 = _service.BuscarPelaChave(Entity.ColaboradorId);
                if (n1 == null) return false;
                //Comparar o CNPJ antes e o depois
                //Verificar se há cnpj exisitente
                return string.Compare(n1.Cpf.RetirarCaracteresEspeciais(),
                    cpf, StringComparison.Ordinal) != 0 && _service.ExisteCpf(cpf);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///     Verificar se dados válidos
        ///     <para>True, inválido</para>
        /// </summary>
        /// <returns></returns>
        private bool EInValidoCpf()
        {
            try
            {
                if (Entity == null) return false;
                var cpf = Entity.Cpf.RetirarCaracteresEspeciais();
                if (!Utils.IsValidCpf(cpf)) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Validar Regras de Negócio
        ///     True, regra de negócio violada
        /// </summary>
        /// <returns></returns>
        public bool Validar()
        {
            try
            {
                if (Entity == null) return true;
                if (isReprovacao) return false;
                if (Entity.Cpf != null & Entity.Cpf != "")
                {
                    Entity.Validate();
                    var hasErros = Entity.HasErrors;
                    if (hasErros) return true;
                    //Verificar valiade de cpf
                    if (EInValidoCpf())
                    {
                        Entity.SetMessageErro("Cpf", "CPF inválido");
                        return true;
                    }
                    //Verificar existência de CPF
                    if (ExisteCpf())
                    {
                        Entity.SetMessageErro("Cpf", "CPF já existe");
                        return true;
                    }
                    return Entity.HasErrors;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void VerificaPreCadastroAprovacao()
        {
            var l1 = _service.Listar(null, $"%{string.Empty}%", null, true);
            existePreCadastro = false;
            if (l1 != null && l1.Count > 0)
            {
                existePreCadastro = true;
                msgPreCadastro = string.Format("Existe(m) {0} pré-cadastro(s) pendente(s) de aprovação", l1.Count());
            }
        }

        #endregion

        #region Commands

        public ComportamentoBasico Comportamento { get; set; }

        /// <summary>
        ///     Listar Municipios
        /// </summary>
        /// <param name="uf"></param>
        public void ListarMunicipios(string uf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uf)) return;
                if (Estado == null) return;

                //Verificar se há municipios já carregados...
                var l1 = _municipios.Where(n => n.Uf == uf);
                Municipios.Clear();
                //Nao havendo municipios... obter do repositorio
                if (!l1.Any())
                {
                    var l2 = _auxiliaresService.MunicipioService.Listar(null, uf);
                    _municipios.AddRange(Mapper.Map<List<Municipio>>(l2));
                }

                var municipios = _municipios.Where(n => n.Uf == uf).ToList();
                Municipios = municipios;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///     Pré-Cadastro
        /// </summary>
        public ICommand PrepareIportarCommand => new CommandBase(PrepareImportar, true);
        public ICommand PrepareReprovaCommand => new CommandBase(PrepareReprovar, true);
        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase(PrepareCriar, true);

        private void PrepareCriar()
        {
            try
            {
                Entity = new ColaboradorView();
                _prepareCriarCommandAcionado = true;
                Comportamento.PrepareCriar();
                _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
                HabilitaControle(false, false);
                CloneObservable();
                SetPendenciaFalse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Editar
        /// </summary>
        public ICommand PrepareAlterarCommand => new CommandBase(PrepareAlterar, true);

        /// <summary>
        ///     Cancelar
        /// </summary>
        public ICommand PrepareCancelarCommand => new CommandBase(Comportamento.PrepareCancelar, true);

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareSalvarCommand => new CommandBase(PrepareSalvar, true);

        /// <summary>
        ///     Remover
        /// </summary>
        public ICommand PrepareRemoverCommand => new CommandBase(PrepareRemover, true);

        /// <summary>
        ///     Pesquisar
        /// </summary>
        public ICommand PesquisarCommand => new CommandBase(Pesquisar, true);

        #endregion

        #region Salva Dados

        private void PrepareSalvar()
        {
            if (Validar()) return;
            Comportamento.PrepareSalvar();
        }

        private List<ColaboradorView> _entityObserverCloned = new List<ColaboradorView>();
        private void PrepareAlterar()
        {
            try
            {
                if (Entity == null)
                {
                    WpfHelp.PopupBox("Selecione um item da lista", 1);
                    return;
                }

                Comportamento.PrepareAlterar();
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
                HabilitaControle(false, false);

                CloneObservable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Clone Observable
        /// </summary>
        private void CloneObservable()
        {
            try
            {
                _entityObserverCloned.Clear();
                EntityObserver.ToList().ForEach(n => { _entityObserverCloned.Add((ColaboradorView)n.Clone()); });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PrepareRemover()
        {
            try
            {
                if (Entity == null) return;

                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                Comportamento.PrepareRemover();
                HabilitaControle(true, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;
                if (Validar()) return;

                var n1 = Mapper.Map<Colaborador>(Entity);
                _service.Criar(n1);
                if (Entity.Ativo)
                {
                    IsLabelAtivoInativo = "Ativo";
                    IsBotaoAtivoInativo = "Inativar";
                }
                else
                {
                    IsLabelAtivoInativo = "Inativo";
                    IsBotaoAtivoInativo = "Ativar";
                }
                var n2 = Mapper.Map<ColaboradorView>(n1);
                EntityObserver.Insert(0, n2);
                HabilitaControle(true, true);
                SelectListViewIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;

                if (Validar()) return;

                var n1 = Mapper.Map<Colaborador>(Entity);
                n1.Nome = n1.Nome.TrimStart().TrimEnd();
                _service.Alterar(n1);
                if (Entity.Ativo)
                {
                    IsLabelAtivoInativo = "Ativo";
                    IsBotaoAtivoInativo = "Inativar";
                }
                else
                {
                    IsLabelAtivoInativo = "Inativo";
                    IsBotaoAtivoInativo = "Ativar";
                }

                #region Gerar CardHolder
                try
                {
                    var colamoradorEmpresasContratos = _serviceColaboradorEmpresa.ListarView(Entity.ColaboradorId).ToList();
                    foreach (var item in colamoradorEmpresasContratos)
                    {
                        var colamoradorContrato = _serviceColaboradorEmpresa.ListarView(item.ColaboradorId, null, null, item.Matricula).ToList().FirstOrDefault();
                        if (!Entity.Ativo)
                        {
                            colamoradorContrato.Ativo = Entity.Ativo;
                        }
                        _serviceCredencialSC.CriarTitularCartao(new CredencialGenetecService(Main.Engine), new ColaboradorService(), colamoradorContrato);
                    }

                }
                catch (Exception ex)
                {
                    WpfHelp.PopupBox(ex);
                }
                #endregion



                HabilitaControle(true, true);
                isReprovacao = false;
            }
            catch (Exception ex)
            {
                isReprovacao = false;
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            try
            {
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                if (Entity != null) Entity.ClearMessageErro();
                Entity = null;
                EntityObserver.Clear();
                EntityObserver = new ObservableCollection<ColaboradorView>(_entityObserverCloned);
                HabilitaControle((Entity != null), true);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        private void OnRemover(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;

                var n1 = Mapper.Map<Colaborador>(Entity);

                //So remove de o colaborador for Precadastro=True
                if (n1.Precadastro)
                {
                    _service.Remover(n1);
                    //Retirar empresa da coleção
                    EntityObserver.Remove(Entity);

                }
                HabilitaControle(true, true);
                VerificaPreCadastroAprovacao();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }
        /// <summary>
        /// Obtem configuração de sistema
        /// </summary>
        /// <returns></returns>
        private ConfiguraSistema ObterConfiguracao()
        {
            try
            {
                //Obter configuracoes de sistema
                var config = _auxiliaresServiceConfiguraSistema.ConfiguraSistemaService.Listar();
                //Obtem o primeiro registro de configuracao
                if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
                return config.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PrepareImportar()
        {
            try
            {
                if (Entity == null) return;
                isReprovacao = false;
                if (Validar()) return;

                
                var colaboradorWebId = Entity.ColaboradorId;

                Colaborador colaborador = new Colaborador();
                //testar se é aprovacao inclusao ou edicao revisao
                //Precadastro = true -> aprovacao inclusao
                //Precadastro = false -> edicao revisao
                if (Entity.Precadastro)
                {

                    var colaboradorWeb = _serviceWeb.Listar(Entity.ColaboradorId, null, null, null, null);
                    var vinculoWeb = _serviceColaboradorEmpresaWeb.Listar(Entity.ColaboradorId, null, null, null, null, null, null, null);
                    var cursoWeb = _serviceColaboradorCursoWeb.Listar(Entity.ColaboradorId, null, null, null, null);
                    var anexoWeb = _serviceColaboradorAnexoWeb.Listar(Entity.ColaboradorId, null, null);
                    var observacaoWeb = _serviceColaboradorObservacao.Listar(Entity.ColaboradorId);

                    foreach (var colaboradorWebNovo in colaboradorWeb)
                    {
                        colaboradorWebNovo.ColaboradorId = 0;
                        colaborador = Mapper.Map<Colaborador>(colaboradorWebNovo);

                        colaborador.Precadastro = false;
                        colaborador.Observacao = null;
                        colaborador.StatusCadastro = 3;

                        _service.Criar(colaborador);
                    }

                    foreach (var vinculoWebNovo in vinculoWeb)
                    {
                        var vinculo = Mapper.Map<ColaboradorEmpresa>(vinculoWebNovo);
                        vinculo.ColaboradorId = colaborador.ColaboradorId;

                        _serviceColaboradorEmpresa.Criar(vinculo);
                    }

                    foreach (var cursoWebNovo in cursoWeb)
                    {
                        var curso = Mapper.Map<ColaboradorCurso>(cursoWebNovo);
                        curso.ColaboradorId = colaborador.ColaboradorId;

                        _serviceColaboradorCurso.Criar(curso);
                    }

                    foreach (var anexoWebNovo in anexoWeb)
                    {
                        var anexo = Mapper.Map<ColaboradorAnexo>(anexoWebNovo);
                        anexo.ColaboradorId = colaborador.ColaboradorId;

                        _serviceColaboradorAnexo.Criar(anexo);
                    }

                    foreach (var item in colaboradorWeb)
                    {
                        item.ColaboradorId = colaboradorWebId;
                        _serviceWeb.Remover(item);
                    }
                }
                else
                {
                    var colaboradorWeb = _serviceWeb.Listar(Entity.ColaboradorId, null, null, null, null);
                    var vinculoWeb = _serviceColaboradorEmpresaWeb.Listar(Entity.ColaboradorId, null, null, null, null, null, null, null);
                    var cursoWeb = _serviceColaboradorCursoWeb.Listar(Entity.ColaboradorId, null, null, null, null);
                    var anexoWeb = _serviceColaboradorAnexoWeb.Listar(Entity.ColaboradorId, null, null);
                    var observacaoWeb = _serviceColaboradorObservacao.Listar(Entity.ColaboradorId);

                    foreach (var colaboradorWebNovo in colaboradorWeb)
                    {
                        colaborador = Mapper.Map<Colaborador>(colaboradorWebNovo);

                        colaborador.Observacao = null;
                        colaborador.StatusCadastro = 3;

                        _service.Alterar(colaborador);
                    }

                    foreach (var vinculoWebNovo in vinculoWeb)
                    {
                        var vinculo = Mapper.Map<ColaboradorEmpresa>(vinculoWebNovo);
                        vinculo.ColaboradorId = Entity.ColaboradorId;

                        _serviceColaboradorEmpresa.Alterar(vinculo);
                    }

                    foreach (var cursoWebNovo in cursoWeb)
                    {
                        var curso = Mapper.Map<ColaboradorCurso>(cursoWebNovo);
                        curso.ColaboradorId = Entity.ColaboradorId;

                        _serviceColaboradorCurso.Alterar(curso);
                    }

                    foreach (var anexoWebNovo in anexoWeb)
                    {
                        var anexo = Mapper.Map<ColaboradorAnexo>(anexoWebNovo);
                        anexo.ColaboradorId = Entity.ColaboradorId;

                        _serviceColaboradorAnexo.Alterar(anexo);
                    }

                    foreach (var item in colaboradorWeb)
                    {
                        item.ColaboradorId = colaboradorWebId;
                        _serviceWeb.Remover(item);
                    }
                }



                #region Remover dados das tabelas auxiliares
                //n1.Pendente21 = true;
                //n1.Pendente22 = true;
                //n1.Pendente23 = true;
                //n1.Pendente24 = true;
                //n1.Pendente25 = true;

                //var pendencia = new Pendencia();
                //pendencia.ColaboradorId = Entity.ColaboradorId;
                ////--------------------------
                //pendencia.CodPendencia = 22;
                //pendencia.Impeditivo = true;
                //Pendencia.CriarPendenciaSistema(pendencia);

                ////--------------------------
                //pendencia.CodPendencia = 23;
                //pendencia.Impeditivo = true;
                //Pendencia.CriarPendenciaSistema(pendencia);
                ////--------------------------
                //pendencia.CodPendencia = 24;
                //pendencia.Impeditivo = true;
                //Pendencia.CriarPendenciaSistema(pendencia);

                
                #endregion

                //_service.Alterar(n1);
                EntityObserver.RemoveAt(SelectListViewIndex);
                HabilitaControle(true, true);
                VerificaPreCadastroAprovacao();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void PrepareReprovar()
        {
            if (Entity == null) return;
            isReprovacao = true;
            /*
             * Aguardando Aprovação Revisão
             * ENUM StatusCadastro
             */
            Entity.StatusCadastro = 2;
            if (string.IsNullOrEmpty(Entity.Observacao))
            {
                WpfHelp.PopupBox("Informe no campo observação o motivo da reprovação", 1);
                PrepareAlterar();
            }
            else
            {
                var n1 = Mapper.Map<Colaborador>(Entity);
                _service.Alterar(n1);

                WpfHelp.PopupBox("Cadastro Reprovado e enviado de volta para Revisão.", 1);
            }
        }
        #endregion
    }
}