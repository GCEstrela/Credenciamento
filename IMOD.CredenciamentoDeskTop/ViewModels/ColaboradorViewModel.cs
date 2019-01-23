﻿// ***********************************************************************
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
    public class ColaboradorViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IColaboradorService _service = new ColaboradorService();

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
        ///     True, empresa possui pendência na aba Empresas Vinculos
        /// </summary>
        public bool PendenciaEmpresasVinculos { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Treinamento
        /// </summary>
        public bool PendenciaTreinamento { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Anexo
        /// </summary>
        public bool PendenciaAnexo { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Credencial
        /// </summary>
        public bool PendenciaCredencial { get; set; }

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

        ColaboradorView EntityTmp = new ColaboradorView();
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
        ///     Dados de municipio armazendas em memoria
        /// </summary>
        public List<Municipio> _municipios { get; set; }

        #endregion

        public ColaboradorViewModel()
        {
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        private void ListarDadosAuxiliares()
        {
            var lst3 = _auxiliaresService.EstadoService.Listar();
            Estados = Mapper.Map<List<Estados>>(lst3);
        }

        #region  Metodos

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "CPF"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Nome"));
            PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
        }

        private void PopularObserver(ICollection<Colaborador> list)
        {
            try
            {
                var list2 = Mapper.Map<List<ColaboradorView>>(list.OrderByDescending(n => n.ColaboradorId));
                EntityObserver = new ObservableCollection<ColaboradorView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void Pesquisar()
        {
            try
            {
                var pesquisa = NomePesquisa;

                var num = PesquisarPor;

                //Por nome
                if (num.Key == 2)
                {
                    var l1 = _service.Listar(null, null, $"%{pesquisa}%");
                    PopularObserver(l1);
                }
                //Por cpf
                if (num.Key == 1)
                {
                    if (string.IsNullOrWhiteSpace(pesquisa))
                    {
                        pesquisa = "";
                    }

                    var l1 = _service.Listar(null, pesquisa.RetirarCaracteresEspeciais(), null);
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

        /// <summary>
        ///     Atualizar dados de pendências
        /// </summary>
        public void AtualizarDadosPendencias()
        {
            if (Entity == null)
            {
                return;
            }

            var pendencia = _service.Pendencia.ListarPorColaborador(Entity.ColaboradorId).ToList();
            //Set valores
            PendenciaGeral = false;
            PendenciaEmpresasVinculos = false;
            PendenciaTreinamento = false;
            PendenciaAnexo = false;
            PendenciaCredencial = false;
            //Buscar pendências referente aos códigos: 21;22;23;24;25
            PendenciaGeral = pendencia.Any(n => n.CodPendencia == 21);
            PendenciaEmpresasVinculos = pendencia.Any(n => n.CodPendencia == 22);
            PendenciaTreinamento = pendencia.Any(n => n.CodPendencia == 23);
            PendenciaAnexo = pendencia.Any(n => n.CodPendencia == 24);
            PendenciaCredencial = pendencia.Any(n => n.CodPendencia == 25);
            //Indica se a empresa possue pendências
            Pendencias = PendenciaGeral || PendenciaEmpresasVinculos || PendenciaTreinamento || PendenciaAnexo || PendenciaCredencial;
        }

        #endregion

        #region Regras de Negócio

        public void ValidarCpf()
        {
            if (Entity == null)
            {
                return;
            }

            var cnpj = Entity.Cpf.RetirarCaracteresEspeciais();

            //Verificar dados antes de salvar uma criação
            if (_prepareCriarCommandAcionado)
            {
                if (_service.ExisteCpf(cnpj))
                {
                    throw new Exception("CPF já cadastrado.");
                }
            }
            //Verificar dados antes de salvar uma alteraçao
            if (_prepareAlterarCommandAcionado)
            {
                var n1 = _service.BuscarPelaChave(Entity.ColaboradorId);
                if (n1 == null)
                {
                    return;
                }
                //Comparar o CNPJ antes e o depois
                if (string.Compare(n1.Cpf.RetirarCaracteresEspeciais(),
                    cnpj, StringComparison.Ordinal) != 0)
                {
                    //verificar se há cnpj exisitente
                    if (_service.ExisteCpf(cnpj))
                    {
                        throw new Exception("CPF já cadastrado.");
                    }
                }
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
            EntityTmp = Entity;
            Entity = new ColaboradorView();
            IsEnableTabItem = false;
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = true;
            SelectedTabIndex = 0;
            Comportamento.PrepareCriar();
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
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
        ///     Validar Regras de Negócio
        /// </summary>
        public void Validar()
        {

        }

        /// <summary>
        ///     Pesquisar
        /// </summary>
        public ICommand PesquisarCommand => new CommandBase(Pesquisar, true);

        #endregion

        #region Salva Dados

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

                var n1 = Mapper.Map<Colaborador>(Entity);
                Validar();
                _service.Criar(n1);
                var n2 = Mapper.Map<ColaboradorView>(n1);
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

        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null)
                {
                    return;
                }

                var n1 = Mapper.Map<Colaborador>(Entity);
                Validar();
                _service.Alterar(n1);
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
                if (Entity.ColaboradorId == 0)
                {
                    Entity = EntityTmp;
                }

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
                if (Entity == null)
                {
                    return;
                }

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes)
                {
                    return;
                }

                var n1 = Mapper.Map<Colaborador>(Entity);
                _service.Remover(n1);
                //Retirar empresa da coleção
                EntityObserver.Remove(Entity);
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

 