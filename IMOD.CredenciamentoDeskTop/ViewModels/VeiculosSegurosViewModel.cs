﻿using System;
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

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    class VeiculosSegurosViewModel : ViewModelBase, IComportamento
    {

        #region  Propriedades

        private readonly IVeiculoSeguroService _service = new VeiculoSeguroService();
        private VeiculoView _veiculoView;
        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;


        public VeiculoSeguroView Entity { get; set; }
        public VeiculoSeguroView EntityTmp { get; set; }

        public ObservableCollection<VeiculoSeguroView> EntityObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        #region Inicializacao

        public void AtualizarDados(VeiculoView entity)
        {
            _veiculoView = entity ?? throw new ArgumentNullException(nameof(entity));
            //Obter dados
            var list1 = _service.Listar(entity.EquipamentoVeiculoId, null, null);
            var list2 = Mapper.Map<List<VeiculoSeguroView>>(list1.OrderByDescending(n => n.VeiculoSeguroId));
            var observer = new ObservableCollection<VeiculoSeguro>(); 
            EntityObserver = new ObservableCollection<VeiculoSeguroView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
        }

        public VeiculosSegurosViewModel()
        { 
            Comportamento = new ComportamentoBasico(false, true, true, false, false);
            EntityObserver = new ObservableCollection<VeiculoSeguroView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
            base.PropertyChanged += OnEntityChanged;
        }

        #endregion

        #region Metódos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Entity") //habilitar botão alterar todas as vezes em que houver entidade diferente de null
                Comportamento.IsEnableEditar = true;
        }
         

        private void PrepareRemover()
        {
            if (Entity == null) return;

            IsEnableLstView = true;
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = false;
            Comportamento.PrepareRemover();
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
                if (Entity == null)
                {
                    return;
                }
                if (Validar()) return;

                var n1 = Mapper.Map<VeiculoSeguro>(Entity);
                n1.VeiculoId = _veiculoView.EquipamentoVeiculoId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<VeiculoSeguroView>(n1);
                EntityObserver.Insert(0, n2);
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>

        private void PrepareCriar()
        {
            EntityTmp = Entity;
            Entity = new VeiculoSeguroView();
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = true;
            Comportamento.PrepareCriar();
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
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
                if (Entity == null)
                {
                    return;
                }
                if (Validar()) return;
                var n1 = Mapper.Map<VeiculoSeguro>(Entity);
                _service.Alterar(n1);
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
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
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                Entity = EntityTmp;

                if (Entity != null)
                {
                    //if (Entity.VeiculoSeguroId == 0)
                    //{
                    //    Entity = EntityTmp;
                    //}
                    Entity.ClearMessageErro();
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
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

                var n1 = Mapper.Map<VeiculoSeguro>(Entity);
                _service.Remover(n1);
                //Retirar empresa da coleção
                EntityObserver.Remove(Entity);
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        private void PrepareSalvar()
        {
            if (!ErroValidacao)
                Comportamento.PrepareSalvar();
        }
        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                WpfHelp.PopupBox("Selecione um item da lista", 1);
                return;
            }

            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
        }

        /// <summary>
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
             
        }

        private void PopularObserver(ICollection<VeiculoSeguro> list)
        {
            try
            {
                var list2 = Mapper.Map<List<VeiculoSeguroView>>(list.OrderBy(n => n.NomeArquivo));
                EntityObserver = new ObservableCollection<VeiculoSeguroView>();
                list2.ForEach(n => { EntityObserver.Add(n); }); 
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
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
        public ICommand PrepareCriarCommand => new CommandBase(PrepareCriar, true);

        public ComportamentoBasico Comportamento { get; set; }

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

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        /// <returns></returns>
        public bool Validar()
        {
            Entity.Validate();
            var hasErros = Entity.HasErrors; 
            return hasErros;
        }

        /// <summary>
        /// Erro de validação
        /// True, Erro de validação
        /// </summary>
        public bool ErroValidacao { get { return Validar(); } }
    }
    

}
