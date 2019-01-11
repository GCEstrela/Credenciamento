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
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using AutoMapper;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels.Commands;
using iModSCCredenciamento.ViewModels.Comportamento;
using iModSCCredenciamento.Views.Model;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using EmpresaLayoutCrachaView = IMOD.Domain.EntitiesCustom.EmpresaLayoutCrachaView;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresaViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaService _service = new EmpresaService();

        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;

        #region  Propriedades

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
        ///     True, empresa possui pendências
        /// </summary>
        public bool Pendencias { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Geral
        /// </summary>
        public bool PendenciaGeral { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Represenante
        /// </summary>
        public bool PendenciaRepresentante { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Contrato
        /// </summary>
        public bool PendenciaContrato { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Anexo
        /// </summary>
        public bool PendenciaAnexo { get; set; }

        /// <summary>
        ///     Habilita abas
        /// </summary>
        public bool IsEnableTabItem { get; private set; } = true;
        /// <summary>
        /// Seleciona o indice da tabcontrol desejada
        /// </summary>
        public short SelectedTabIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        EmpresaView EntidadeTMP = new EmpresaView();
        public EmpresaView Empresa { get; set; }
        public ObservableCollection<EmpresaView> Empresas { get; set; }
        public ObservableCollection<EmpresaLayoutCrachaView> TiposLayoutCracha { get; set; }
        public LayoutCrachaView TipoCracha { get; set; }
        public ObservableCollection<EmpresaTipoAtividadeView> TiposAtividades { get; set; }
        public TipoAtividadeView TipoAtividade { get; set; }

        /// <summary>
        ///     LayouCrachas
        /// </summary>
        public List<LayoutCrachaView> ListaCrachas { get; set; }

        /// <summary>
        ///     Tipos de atividade
        /// </summary>
        public List<TipoAtividadeView> ListaAtividades { get; set; }

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
        ///     Dados de municipio armazendas em memoria
        /// </summary>
        public List<Municipio> _municipios { get; set; }


        #endregion

        public EmpresaViewModel()
        {
            ListarTodos();
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            TiposAtividades = new ObservableCollection<EmpresaTipoAtividadeView>();
            TiposLayoutCracha = new ObservableCollection<EmpresaLayoutCrachaView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

        /// <summary>
        ///     Atualizar dados de atividade
        /// </summary>
        public void AtualizarDadosTiposAtividades()
        {
            if (Empresa == null) return;
            TiposAtividades.Clear();
            var id = Empresa.EmpresaId;
            var list = _service.Atividade.ListarEmpresaTipoAtividadeView(null, id, null, null).ToList();
            var list2 = Mapper.Map<List<EmpresaTipoAtividadeView>>(list);
            list2.ForEach(n => TiposAtividades.Add(n));
        }

        /// <summary>
        ///     Atualizar dados de atividade
        /// </summary>
        public void AtualizarDadosTipoCrachas()
        {
            if (Empresa == null) return;
            TiposLayoutCracha.Clear();
            var id = Empresa.EmpresaId;
            var list = _service.CrachaService.ListarLayoutCrachaPorEmpresaView(id).ToList();
            var list2 = Mapper.Map<List<EmpresaLayoutCrachaView>>(list);
            list2.ForEach(n => TiposLayoutCracha.Add(n));
        }



        /// <summary>
        ///     Atualizar dados de pendências
        /// </summary>ValidarCnpj
        public void AtualizarDadosPendencias()
        {
            if (Empresa == null) return;
            var pendencia = _service.Pendencia.ListarPorEmpresa(Empresa.EmpresaId).ToList();
            //Set valores
            PendenciaGeral = false;
            PendenciaRepresentante = false;
            PendenciaContrato = false;
            PendenciaAnexo = false;
            //Buscar pendências referente aos códigos: 21; 12;14;24
            PendenciaGeral = pendencia.Any(n => n.CodPendencia == 21);
            PendenciaRepresentante = pendencia.Any(n => n.CodPendencia == 12);
            PendenciaContrato = pendencia.Any(n => n.CodPendencia == 14);
            PendenciaAnexo = pendencia.Any(n => n.CodPendencia == 24);
            //Indica se a empresa possue pendências
            Pendencias = PendenciaGeral || PendenciaRepresentante || PendenciaContrato || PendenciaAnexo;
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "Razão Social"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Código"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(3, "CNPJ"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(4, "Todos"));
            PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
        }

        private void ListarTodos()
        {
            try
            {
                var list1 = _service.Listar();
                PopularObserver(list1);
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///     Listar dados auxilizares
        /// </summary>
        private void ListarDadosAuxiliares()
        {
            var lst1 = _auxiliaresService.LayoutCrachaService.Listar();
            var lst2 = _auxiliaresService.TipoAtividadeService.Listar();
            var lst3 = _auxiliaresService.EstadoService.Listar();
            ListaCrachas = Mapper.Map<List<LayoutCrachaView>>(lst1);
            ListaAtividades = Mapper.Map<List<TipoAtividadeView>>(lst2);
            Estados = Mapper.Map<List<Estados>>(lst3);
        }

        #endregion

        #region Regras de Negócio

        public void ValidarCnpj()
        {
            if (Empresa == null) return;
            var cnpj = Empresa.Cnpj.RetirarCaracteresEspeciais();

            //Verificar dados antes de salvar uma criação
            if (_prepareCriarCommandAcionado)
                if (_service.ExisteCnpj(cnpj))
                    throw new Exception("CNPJ já cadastrado.");
            //Verificar dados antes de salvar uma alteraçao
            if (_prepareAlterarCommandAcionado)
            {
                var n1 = _service.BuscarPelaChave(Empresa.EmpresaId);
                if (n1 == null) return;
                //Comparar o CNPJ antes e o depois
                if (string.Compare(n1.Cnpj.RetirarCaracteresEspeciais(),
                    cnpj, StringComparison.Ordinal) != 0)
                {
                    //verificar se há cnpj exisitente
                    if (_service.ExisteCnpj(cnpj))
                        throw new Exception("CNPJ já cadastrado.");
                }
            }
        }

        /// <summary>
        ///     Validar Regras de Negócio
        ///     <para>True, dados válidos</para>
        /// </summary>
        public void Validar()
        {
            ValidarCnpj();
            if (string.IsNullOrWhiteSpace(Empresa.Nome))
                throw new InvalidOperationException("O nome é requerido");
            if (string.IsNullOrWhiteSpace(Empresa.Cnpj))
                throw new InvalidOperationException("O CNPJ é requerido");
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
                if (Municipios == null) Municipios = new List<Municipio>();
                if (_municipios == null) _municipios = new List<Municipio>();
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
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase(PrepareCriar, true);

        private void PrepareCriar()
        {

            EntidadeTMP = Empresa;
            Empresa = new EmpresaView();
            IsEnableTabItem = false;
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = true;
            SelectedTabIndex = 0;
            Comportamento.PrepareCriar();
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            TiposLayoutCracha.Clear();
            TiposAtividades.Clear();
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
        public ICommand PrepareSalvarCommand => new CommandBase(Comportamento.PrepareSalvar, true);

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

        private void Pesquisar()

        {
            try
            {
                var pesquisa = NomePesquisa;

                var num = PesquisarPor;

                //Por Razão Social
                if (num.Key == 1)
                {
                    var l1 = _service.Listar($"%{pesquisa}%", null, null);
                    PopularObserver(l1);
                }
                //Por código
                if (num.Key == 2)
                {
                    if (string.IsNullOrWhiteSpace(pesquisa)) return;
                    var cod = 0;
                    int.TryParse(pesquisa, out cod);
                    var n1 = _service.BuscarPelaChave(cod);
                    if (n1 == null) return;
                    Empresas.Clear();
                    var n2 = Mapper.Map<EmpresaView>(n1);
                    var observer = new ObservableCollection<EmpresaView>();
                    observer.Add(n2);
                    Empresas = observer;
                }
                //Por CNPJ
                if (num.Key == 3)
                {
                    var l1 = _service.Listar(null, null, pesquisa);
                    PopularObserver(l1);
                }
                //Todos
                if (num.Key == 4)
                {
                    var l1 = _service.Listar();
                    PopularObserver(l1);
                }

                IsEnableLstView = true;
                IsEnableTabItem = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PopularObserver(ICollection<Empresa> list)
        {
            try
            {
                var list2 = Mapper.Map<List<EmpresaView>>(list.OrderBy(n => n.Nome));
                Empresas = new ObservableCollection<EmpresaView>();
                list2.ForEach(n => { Empresas.Add(n); });
                //Empresas = observer;
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PrepareAlterar()
        {
            if (Empresa == null) return;
            Comportamento.PrepareAlterar();
            IsEnableTabItem = false;
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = false;
            SelectedTabIndex = 0;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            AtualizarDadosTiposAtividades();
            AtualizarDadosTipoCrachas();
        }

        private void PrepareRemover()
        {
            if (Empresa == null) return;
            IsEnableLstView = true;
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = false;
            SelectedTabIndex = 0;
            Comportamento.PrepareRemover();

        }

        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Empresa == null) return;
                var n1 = Mapper.Map<Empresa>(Empresa);
                Validar();
                _service.Criar(n1);
                //Salvar Tipo de Atividades
                SalvarTipoAtividades(n1.EmpresaId);
                //Salvar Tipo Cracha
                SalvarTipoCracha(n1.EmpresaId);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<EmpresaView>(n1);
                Empresas.Insert(0, n2);
                IsEnableTabItem = true;
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void SalvarTipoAtividades(int empresaId)
        {
            //Remover
            _service.Atividade.RemoverPorEmpresa(empresaId);
            //Adicionar
            var lst = TiposAtividades.ToList();
            lst.ForEach(n =>
           {
               var n1 = Mapper.Map<EmpresaTipoAtividade>(n);
               n1.EmpresaId = empresaId;
               _service.Atividade.Criar(n1);
           });
        }

        private void SalvarTipoCracha(int empresaId)
        {
            //Remover
            _service.CrachaService.RemoverPorEmpresa(empresaId);
            //Adicionar
            var lst = TiposLayoutCracha.ToList();
            lst.ForEach(n =>
           {
               var n1 = Mapper.Map<EmpresaLayoutCracha>(n);
               n1.EmpresaId = empresaId;
               _service.CrachaService.Criar(n1);
           });
        }

        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Empresa == null) return;
                Validar();
                var n1 = Mapper.Map<Empresa>(Empresa);
                _service.Alterar(n1);
                //Salvar Tipo de Atividades
                SalvarTipoAtividades(n1.EmpresaId);
                //Salvar Tipo Cracha
                SalvarTipoCracha(n1.EmpresaId);
                IsEnableTabItem = true;
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            try
            {
                IsEnableTabItem = true;
                IsEnableLstView = true;
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                TiposAtividades.Clear();
                TiposLayoutCracha.Clear();
                Empresa = EntidadeTMP;
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
                if (Empresa == null) return;
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;

                var n1 = Mapper.Map<Empresa>(Empresa);
                _service.Remover(n1);
                //Retirar empresa da coleção
                Empresas.Remove(Empresa);

                IsEnableLstView = true;
                IsEnableTabItem = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        #endregion
    }
}