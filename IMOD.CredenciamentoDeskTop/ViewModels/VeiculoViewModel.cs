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
    public class VeiculoViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IVeiculoService _service = new VeiculoService();

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
        ///     True, empresa possui pendência de codigo 21
        /// </summary>
        public bool Pendencia21 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 22
        /// </summary>
        public bool Pendencia22 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 19
        /// </summary>
        public bool Pendencia19 { get; set; }

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
        ///     Indice da tabela de controle selecionada geral
        /// </summary>
        public bool IsEnableTabItemGeral { get; set; }
        /// <summary>
        ///     Indice da tabela de controle selecionada empresa vinculo
        /// </summary>
        public bool IsEnableTabItemEmpresaVinculo { get; set; }

        /// <summary>
        ///     Indice da tabela de controle selecionada seguros
        /// </summary>
        public bool IsEnableTabItemSeguros { get; set; }

        /// <summary>
        ///     Indice da tabela de controle selecionada Credenciais
        /// </summary>
        public bool IsEnableTabItemCredenciais { get; set; }

        /// <summary>
        ///     Indice da tabela de controle selecionada anexos
        /// </summary>
        public bool IsEnableTabItemAnexos { get; set; }

        /// <summary>
        ///     Seleciona o indice da tabcontrol desejada
        /// </summary>
        public short SelectedTabIndex { get; set; }

        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }

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
            Comportamento = new ComportamentoBasico (false, true, true, false, false);
            EntityObserver = new ObservableCollection<VeiculoView>();
            TiposEquipamentoServico = new ObservableCollection<EquipamentoVeiculoTipoServicoView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
            PropertyChanged += OnEntityChanged;
        }

        #region  Metodos

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)

        {
            if (e.PropertyName == "Entity")
            {
                var enableControls = Entity != null;
                Comportamento.IsEnableEditar = Entity != null;
                HabilitaControleTabControls(true, enableControls, enableControls, enableControls, enableControls, enableControls);
            }
            //===================================
            //Autor:Valnei Filho
            //Data:08/03/19
            //A listView principal deve estar desabilitada ao navegar por entre as abas
            if (e.PropertyName == "SelectedTabIndex")
            {
                //Habilita/Desabilita botoes principais...
                HabilitaCommandPincipal = SelectedTabIndex == 0;
                //IsEnableLstView = SelectedTabIndex == 0;
            }
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add (new KeyValuePair<int, string> (1, "Placa/Identificador"));
            ListaPesquisa.Add (new KeyValuePair<int, string> (2, "Série/Chassi"));
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
            ListaTipoServico = Mapper.Map<List<TipoServico>> (lst1);
            TiposCombustiveis = Mapper.Map<List<TipoCombustivel>> (lst2);
            Estados = Mapper.Map<List<Estados>> (lst3);
            ListaEquipamentos = Mapper.Map<List<TipoEquipamento>> (lst4);
        }

        /// <summary>
        ///     Atualizar dados de pendências
        /// </summary>
        /// ValidarCnpj
        public void AtualizarDadosPendencias()
        {
            if (Entity == null) return;

            var pendencia = _service.Pendencia.ListarPorVeiculo (Entity.EquipamentoVeiculoId).ToList();
            //Set valores
            SetPendenciaFalse();
            //Buscar pendências referente aos códigos: 21; 12;14;24
            Pendencia21 = pendencia.Any (n => n.CodPendencia == 21 & n.Ativo);
            Pendencia22 = pendencia.Any (n => n.CodPendencia == 22 & n.Ativo);
            Pendencia19 = pendencia.Any (n => n.CodPendencia == 19 & n.Ativo);
            Pendencia24 = pendencia.Any (n => n.CodPendencia == 24 & n.Ativo);
            Pendencia25 = pendencia.Any (n => n.CodPendencia == 25 & n.Ativo);
        }

        private void SetPendenciaFalse()
        {
            Pendencia21 = false;
            Pendencia22 = false;
            Pendencia19 = false;
            Pendencia24 = false;
            Pendencia25 = false;
        }

        /// <summary>
        ///     Atualizar dados de atividade
        /// </summary>
        public void AtualizarDadosTiposServico()
        {
            if (Entity == null) return;

            TiposEquipamentoServico.Clear();
            var id = Entity.EquipamentoVeiculoId;
            var list = _service.Equipamento.ListarEquipamentoVeiculoTipoServicoView (id).ToList();
            var list2 = Mapper.Map<List<EquipamentoVeiculoTipoServicoView>> (list);
            list2.ForEach (n => TiposEquipamentoServico.Add (n));
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

        private void PrepareCriar()
        {
            Entity = new VeiculoView();
            Comportamento.PrepareCriar();
            TiposEquipamentoServico.Clear();
            HabilitaControle (false, false);
            SetPendenciaFalse();
        }

        /// <summary>
        ///     Habilita controles
        /// </summary>
        /// <param name="isEnableTabItem"></param>
        /// <param name="isEnableLstView"></param>
        private void HabilitaControle(bool isEnableTabItem, bool isEnableLstView)
        {
            HabilitaControleTabControls(isEnableLstView, isEnableTabItem, isEnableTabItem, isEnableTabItem, isEnableTabItem, isEnableTabItem);
            //IsEnableTabItem = isEnableTabItem;
            IsEnableLstView = isEnableLstView;
        }

        public void HabilitaControleTabControls(bool lstViewSuperior = true, bool isItemGeral = true, 
                bool isItemEmpresa = false, bool isItemSeguro = false, bool isItemAnexo = false, bool isItemCredenciais = false)
        {
            IsEnableLstView = lstViewSuperior;

            IsEnableTabItemGeral = isItemGeral;
            IsEnableTabItemEmpresaVinculo = isItemEmpresa;
            IsEnableTabItemSeguros = isItemSeguro;
            IsEnableTabItemAnexos = isItemAnexo;
            IsEnableTabItemCredenciais = isItemCredenciais;
        }

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

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase (PrepareCriar, true);

        #endregion

        #region Salva Dados

        private void Pesquisar()
        {
            try
            {
                var pesquisa = NomePesquisa;

                var num = PesquisarPor;

                //Placa/Identificador
                if (num.Key == 1)
                {
                    if (string.IsNullOrWhiteSpace (pesquisa)) return;
                    var l1 = _service.Listar (null, null, $"%{pesquisa}%", null);
                    PopularObserver (l1);
                }
                //Por Série/Chassi
                if (num.Key == 2)
                {
                    if (string.IsNullOrWhiteSpace (pesquisa)) return;
                    var l1 = _service.Listar (null, null, null, $"%{pesquisa}%");
                    PopularObserver (l1);
                }

                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void PopularObserver(ICollection<Veiculo> list)
        {
            try
            {
                var list2 = Mapper.Map<List<VeiculoView>> (list.OrderByDescending (n => n.EquipamentoVeiculoId).ToList());
                EntityObserver = new ObservableCollection<VeiculoView>();
                list2.ForEach (n => { EntityObserver.Add (n); });
            }

            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void PrepareSalvar()
        {
            if (Validar()) return;
            Comportamento.PrepareSalvar();
        }

        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                WpfHelp.PopupBox ("Selecione um item da lista", 1);
                return;
            }
            Comportamento.PrepareAlterar();
            AtualizarDadosTiposServico();
            HabilitaControle (false, false);
        }

        private void PrepareRemover()
        {
            if (Entity == null) return;

            Comportamento.PrepareRemover();
            HabilitaControle (true, true);
        }

        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;
                if (Validar()) return;

                var n1 = Mapper.Map<Veiculo> (Entity);
                _service.Criar (n1);
                //Salvar Tipo de Servico
                SalvarTipoServico (n1.EquipamentoVeiculoId);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<VeiculoView> (n1);
                EntityObserver.Insert (0, n2);
                HabilitaControle (true, true);
                SelectListViewIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        private void SalvarTipoServico(int veiculoId)
        {
            //Remover
            _service.Equipamento.RemoverPorVeiculo (veiculoId);
            //Adicionar
            var lst = TiposEquipamentoServico.ToList();
            lst.ForEach (n =>
            {
                var n1 = Mapper.Map<EquipamentoVeiculoTipoServico> (n);
                n1.EquipamentoVeiculoId = veiculoId;
                _service.Equipamento.Criar (n1);
            });
        }

        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;
                if (Validar()) return;

                var n1 = Mapper.Map<Veiculo> (Entity);
                _service.Alterar (n1);
                //Salvar Tipo de Servico
                SalvarTipoServico (n1.EquipamentoVeiculoId);
                HabilitaControle (true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity != null) Entity.ClearMessageErro();
                AtualizarDadosTiposServico();
                TiposEquipamentoServico.Clear();
                Entity = null;
                HabilitaControle(true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        private void OnRemover(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;

                var n1 = Mapper.Map<Veiculo> (Entity);
                _service.Remover (n1);
                //Retirar empresa da coleção
                EntityObserver.Remove (Entity);
                HabilitaControle (true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        /// <returns></returns>
        public bool Validar()
        {
            if (Entity == null) return true;
            Entity.Validate();
            var hasErros = Entity.HasErrors;
            if (hasErros) return true;         

            return Entity.HasErrors;
        }

        #endregion
    }
}