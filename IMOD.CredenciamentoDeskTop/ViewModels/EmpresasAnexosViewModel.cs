// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
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
    public class EmpresasAnexosViewModel : ViewModelBase, IComportamento
    {
        private readonly IEmpresaAnexoService _service = new EmpresaAnexoService();
        private EmpresaView _empresaView;
        private EmpresaViewModel _viewModelParent;

        #region  Propriedades

        public EmpresaAnexoView Entity { get; set; }
        public ObservableCollection<EmpresaAnexoView> EntityObserver { get; set; }

        EmpresaAnexoView EntityTmp = new EmpresaAnexoView();

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        public EmpresasAnexosViewModel()
        {
            Comportamento = new ComportamentoBasico(false, true, true, false, false);
            EntityObserver = new ObservableCollection<EmpresaAnexoView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
            base.PropertyChanged += OnEntityChanged;
        }

        #region  Metodos

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

        /// <summary>
        ///     Acionado antes de remover
        /// </summary>
        private void PrepareRemover()
        {
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
                if (Entity == null) return;

                if (Validar()) return;
                var n1 = Mapper.Map<EmpresaAnexo>(Entity);
                n1.EmpresaId = _empresaView.EmpresaId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<EmpresaAnexoView>(n1);
                EntityObserver.Insert(0, n2);
                IsEnableLstView = true;
                _viewModelParent.AtualizarDadosPendencias();
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
            Entity = new EmpresaAnexoView();
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
                if (Entity == null) return;

                if (Validar()) return;
                var n1 = Mapper.Map<EmpresaAnexo>(Entity);
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
                if (Entity != null)
                {
                    Entity.ClearMessageErro();
                    Entity = EntityTmp;
                   
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

                var n1 = Mapper.Map<EmpresaAnexo>(Entity);
                _service.Remover(n1);
                //Retirar empresa da coleção
                EntityObserver.Remove(Entity);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
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
                WpfHelp.PopupBox("Selecione um Item na Lista de Anexos", 1);
                return;
            }
            EntityTmp = Entity;
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
        }

        public void AtualizarDadosAnexo(EmpresaView entity, EmpresaViewModel viewModelParent)
        {          
            _empresaView = entity ?? throw new ArgumentNullException(nameof(entity));
            _viewModelParent = viewModelParent;
            //Obter dados
            var list1 = _service.Listar(entity.EmpresaId);
            var list2 = Mapper.Map<List<EmpresaAnexoView>>(list1.OrderByDescending(n => n.EmpresaAnexoId));
            EntityObserver = new ObservableCollection<EmpresaAnexoView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
        }

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
        ///  Validar Regras de Negócio 
        /// </summary>
        public bool Validar()
        {
            if (Entity == null) return true;
            //Verificar valiade de Descricao do Anexo
            if (EInValidandoDescricao())
            {
                Entity.SetMessageErro("Descricao", "Descrição inválida");
                return true;
            }

            //Verificar valiade do Anexo
            if (EInValidandoAnexo())
            {
                Entity.SetMessageErro("NomeAnexo", "Nome do Anexo é inválido");
                return true;
            } 
            var hasErros = Entity.HasErrors;
            return hasErros;
            
        }
        #endregion

        #region Regras de Negócio
        private bool EInValidandoDescricao()
        {
            if (Entity == null) return false;
            var descricao = Entity.Descricao;
            if (descricao == null) return true;
            return false;
        }
        private bool EInValidandoAnexo()
        {
            if (Entity == null) return false;
            var nomeAnexo = Entity.NomeAnexo;
            if (nomeAnexo == null) return true;
            return false;
        }

        #endregion
    }
}