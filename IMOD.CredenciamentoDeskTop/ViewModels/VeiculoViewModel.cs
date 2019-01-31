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
    internal class VeiculoViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IVeiculoService _service = new VeiculoService();

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
        ///     Seleciona o indice da tabcontrol desejada
        /// </summary>
        public short SelectedTabIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        public VeiculoView Entity { get; set; }
        public ObservableCollection<VeiculoView> EntityObserver { get; set; }

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
        ///     Tipos Combustivel
        /// </summary>
        public List<TipoCombustivel> TiposCombustiveis { get; set; }

        /// <summary>
        ///     Tipos Serviços
        /// </summary>
        public List<TipoServico> ListaTipoServico { get; set; }

        /// <summary>
        ///     Tipos Equipamentos
        /// </summary>
        public List<TipoEquipamento> ListaEquipamentos { get; set; }

        public ObservableCollection<EquipamentoVeiculoTipoServicoView> TiposEquipamentoServico { get; set; }

        /// <summary>
        ///     Tipos Serviços
        /// </summary>
        public TipoServico TipoServico { get; set; }

        /// <summary>
        ///     Dados de municipio armazendas em memoria
        /// </summary>
        public List<Municipio> _municipios { get; set; }

        #endregion

        public VeiculoViewModel()
        {
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
           Comportamento = new ComportamentoBasico(false, true, true, false, false);
            TiposEquipamentoServico = new ObservableCollection<EquipamentoVeiculoTipoServicoView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "Placa/Identificador"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Série/Chassi"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(3, "Código"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(4, "Descrição"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(5, "Marca"));
            PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
        }

        /// <summary>
        ///     Listar dados auxilizares
        /// </summary>
        private void ListarDadosAuxiliares()
        {
            var lst1 = _auxiliaresService.TipoServico.Listar();
            var lst2 = _auxiliaresService.TipoCombustivelService.Listar();
            var lst3 = _auxiliaresService.EstadoService.Listar();
            var lst4 = _auxiliaresService.TipoEquipamentoService.Listar();


            ListaTipoServico = Mapper.Map<List<TipoServico>>(lst1);
            TiposCombustiveis = Mapper.Map<List<TipoCombustivel>>(lst2);
            Estados = Mapper.Map<List<Estados>>(lst3);
            ListaEquipamentos = Mapper.Map<List<TipoEquipamento>>(lst4);
        }

        /// <summary>
        ///     Atualizar dados de pendências
        /// </summary>
        /// ValidarCnpj
        public void AtualizarDadosPendencias()
        {
            if (Entity == null)
            {
                return;
            }

            var pendencia = _service.Pendencia.ListarPorVeiculo(Entity.EquipamentoVeiculoId).ToList();
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
        ///     Atualizar dados de atividade
        /// </summary>
        public void AtualizarDadosTiposServico()
        {
            if (Entity == null)
            {
                return;
            }

            TiposEquipamentoServico.Clear();
            var id = Entity.EquipamentoVeiculoId;
            var list = _service.Equipamento.ListarEquipamentoVeiculoTipoServicoView(id).ToList();
            var list2 = Mapper.Map<List<EquipamentoVeiculoTipoServicoView>>(list);
            list2.ForEach(n => TiposEquipamentoServico.Add(n));
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
                if (string.IsNullOrWhiteSpace(uf))
                {
                    return;
                }

                if (Municipios == null)
                {
                    Municipios = new List<Municipio>();
                }

                if (_municipios == null)
                {
                    _municipios = new List<Municipio>();
                }

                if (Estado == null)
                {
                    return;
                }

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
            Entity = new VeiculoView();
            IsEnableTabItem = false;
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = true;
            SelectedTabIndex = 0;
            Comportamento.PrepareCriar();
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            TiposEquipamentoServico.Clear();
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
        ///  Validar Regras de Negócio 
        /// </summary>
        public bool Validar()
        {
            return false;

        }

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

                //Descricao
                if (num.Key == 1)
                {
                    var l1 = _service.Listar($"%{pesquisa}%", null);
                    PopularObserver(l1);
                }
                //Por Modelo
                if (num.Key == 2)
                {
                    var l1 = _service.Listar(null, $"%{pesquisa}%");
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

        private void PopularObserver(ICollection<Veiculo> list)
        {
            try
            {
                var list2 = Mapper.Map<List<VeiculoView>>(list.OrderByDescending(n => n.EquipamentoVeiculoId));
                EntityObserver = new ObservableCollection<VeiculoView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                return;
            }

            Comportamento.PrepareAlterar();
            IsEnableTabItem = false;
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = false;
            SelectedTabIndex = 0;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            AtualizarDadosTiposServico();
        }

        private void PrepareRemover()
        {
            if (Entity == null)
            {
                return;
            }

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
                if (Entity == null)
                {
                    return;
                }

                var n1 = Mapper.Map<Veiculo>(Entity);
                Validar();
                _service.Criar(n1);
                //Salvar Tipo de Servico
                SalvarTipoServico(n1.EquipamentoVeiculoId);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<VeiculoView>(n1);
                EntityObserver.Insert(0, n2);
                IsEnableTabItem = true;
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void SalvarTipoServico(int veiculoId)
        {
            //Remover
            _service.Equipamento.RemoverPorVeiculo(veiculoId);
            //Adicionar
            var lst = TiposEquipamentoServico.ToList();
            lst.ForEach(n =>
            {
                var n1 = Mapper.Map<EquipamentoVeiculoTipoServico>(n);
                n1.EquipamentoVeiculoId = veiculoId;
                _service.Equipamento.Criar(n1);
            });
        }


        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null)
                {
                    return;
                }

                Validar();
                var n1 = Mapper.Map<Veiculo>(Entity);
                _service.Alterar(n1);
                //Salvar Tipo de Servico
                SalvarTipoServico(n1.EquipamentoVeiculoId);
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
            //try
            //{
            //    IsEnableTabItem = true;
            //    IsEnableLstView = true;
            //    _prepareCriarCommandAcionado = false;
            //    _prepareAlterarCommandAcionado = false;
            //    TiposAtividades.Clear();
            //    TiposLayoutCracha.Clear();
            //}
            //catch (Exception ex)
            //{
            //    Utils.TraceException(ex);
            //    WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            //}
        }

        private void OnRemover(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Entity == null) return;
                //var result = WpfHelp.MboxDialogRemove();
                //if (result != DialogResult.Yes) return;

                //var n1 = Mapper.Map<Empresa>(Entity);
                //_service.Remover(n1);
                ////Retirar empresa da coleção
                //EntityObserver.Remove(Entity);

                //IsEnableLstView = true;
                //IsEnableTabItem = true;
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