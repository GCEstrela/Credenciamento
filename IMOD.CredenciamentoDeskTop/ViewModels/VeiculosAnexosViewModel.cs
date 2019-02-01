// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
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
    public class VeiculosAnexosViewModel : ViewModelBase, IComportamento
    {
        private readonly IVeiculoAnexoService _service = new VeiculoAnexoService();
        private VeiculoView _veiculoView;

        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;

        #region  Propriedades

        public VeiculoAnexoView Entity { get; set; }
        public VeiculoAnexoView EntityTmp { get; set; }
        
        public ObservableCollection<VeiculoAnexoView> EntityObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        public VeiculosAnexosViewModel()
        {
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

        public void AtualizarDadosAnexo(VeiculoView entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _veiculoView = entity;
            //Obter dados
            var list1 = _service.Listar(entity.EquipamentoVeiculoId);
            var list2 = Mapper.Map<List<VeiculoAnexoView>>(list1.OrderByDescending(n => n.VeiculoAnexoId));
            EntityObserver = new ObservableCollection<VeiculoAnexoView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
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

                var n1 = Mapper.Map<VeiculoAnexo>(Entity);
                n1.VeiculoId = _veiculoView.EquipamentoVeiculoId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<VeiculoAnexoView>(n1);
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
            Entity = new VeiculoAnexoView();
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
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
                var n1 = Mapper.Map<VeiculoAnexo>(Entity);
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
                    //if (Entity.VeiculoAnexoId == 0)
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
                if (result != DialogResult.Yes)
                {
                    return;
                }

                var n1 = Mapper.Map<VeiculoAnexo>(Entity);
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


        #endregion

    

        #region Inicializacao

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

        #endregion

        private void PrepareSalvar()
        {
            if (!ErroValidacao)
                Comportamento.PrepareSalvar();
        }

        private void PrepareAlterar()
        {
            if (Entity == null) return;

            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
        }

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        /// <returns></returns>
        public bool Validar()
        {
            Entity.Validate();
            var hasErros = Entity.HasErrors;
            //Entity = EntityTmp;
            return hasErros;
        }

        /// <summary>
        /// Erro de validação
        /// True, Erro de validação
        /// </summary>
        public bool ErroValidacao { get { return Validar(); } }


    }
}