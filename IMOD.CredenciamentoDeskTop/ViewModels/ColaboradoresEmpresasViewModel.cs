﻿// ***********************************************************************
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
    public class ColaboradoresEmpresasViewModel : ViewModelBase, IComportamento
    {
        private readonly IEmpresaContratosService _empresaContratoService = new EmpresaContratoService();
        private readonly IEmpresaService _empresaService = new EmpresaService();
        private readonly IColaboradorEmpresaService _service = new ColaboradorEmpresaService();
        private ColaboradorView _colaboradorView;
        private ColaboradorViewModel _viewModelParent;

        #region  Propriedades

        public List<EmpresaContrato> Contratos { get; private set; }
        public List<Empresa> Empresas { get; private set; }
        public Empresa Empresa { get; set; } 
        public ColaboradorEmpresaView Entity { get; set; }
        public ObservableCollection<ColaboradorEmpresaView> EntityObserver { get; set; }
        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        public ColaboradoresEmpresasViewModel()
        {
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(false, true, true, false, false);
            EntityObserver =new ObservableCollection<ColaboradorEmpresaView>();
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
        ///     Listar dados auxilizares
        /// </summary>
        private void ListarDadosAuxiliares()
        { 
            Empresas = new List<Empresa>();
            Contratos = new List<EmpresaContrato>();
            ListarDadosEmpresaContratos();
        }

        public void ListarContratos(Empresa empresa)
        {
            if (empresa == null) return;

            var lstContratos = _empresaContratoService.Listar(empresa.EmpresaId).ToList();
            Contratos.Clear();
            //Manipular concatenaçção de conrato
            lstContratos.ForEach(n =>
            {
                n.Descricao = $"{n.Descricao} - {n.NumeroContrato}";
                Contratos.Add(n);
            });
        }

        /// <summary>
        ///  Listar dados de empresa e contratos
        /// </summary>
        private void ListarDadosEmpresaContratos()
        {

            try
            {
                var l2 = _empresaService.Listar().ToList();
                Empresas = l2;
                var l3 = _empresaContratoService.Listar().ToList();
                Contratos = l3;
                base.OnPropertyChanged ("Entity");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///     Acionado antes de remover
        /// </summary>
        private void PrepareRemover()
        {
            if (Entity == null) return;

            IsEnableLstView = true;
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

                var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
                n1.ColaboradorId = _colaboradorView.ColaboradorId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<ColaboradorEmpresaView>(n1);
                //Adicionar o nome da empresa e o contrato
                SetDadosEmpresaContrato(n2);
                EntityObserver.Insert(0, n2);
                IsEnableLstView = true;
               _viewModelParent.AtualizarDadosPendencias();
                SelectListViewIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void SetDadosEmpresaContrato(ColaboradorEmpresaView entity)
        {
            var empresa = Empresas.FirstOrDefault(n => n.EmpresaId == entity.EmpresaId);
            var contrato = Contratos.FirstOrDefault(n => n.EmpresaContratoId == entity.EmpresaContratoId);
            if (empresa != null)
                entity.EmpresaNome = empresa.Nome;//Setar o nome da empresa para ser exibida na list view
          

            if (contrato != null)
                entity.Descricao = contrato.Descricao;//Setar o nome do contrato para ser exibida na list view
             
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
           
            Entity = new ColaboradorEmpresaView();
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
            ListarDadosEmpresaContratos();
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
                var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
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
                if (Entity != null) Entity.ClearMessageErro();
                Entity = null;
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
                    return;

                var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
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
                WpfHelp.PopupBox("Selecione um item da lista", 1);
                return;
            }
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
            ListarDadosEmpresaContratos();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModel"></param>
        public void AtualizarDados(ColaboradorView entity, ColaboradorViewModel viewModel)
        {
            _viewModelParent = viewModel;
            AtualizarDados(entity);
             
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(ColaboradorView entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
             

            _colaboradorView = entity;
            //Obter dados
            var list1 = _service.Listar(entity.ColaboradorId);
            var list2 = Mapper.Map<List<ColaboradorEmpresaView>>(list1.OrderByDescending(n => n.ColaboradorEmpresaId));
            EntityObserver = new ObservableCollection<ColaboradorEmpresaView>();
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
            Entity.Validate();
            var hasErros = Entity.HasErrors;
            if (hasErros) return true;

            return Entity.HasErrors;
        }

        #endregion
 
    }
}