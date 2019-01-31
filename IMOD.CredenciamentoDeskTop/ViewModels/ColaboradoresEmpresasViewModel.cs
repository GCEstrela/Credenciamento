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
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        public ColaboradoresEmpresasViewModel()
        {
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            EntityObserver=new ObservableCollection<ColaboradorEmpresaView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

        /// <summary>
        ///     Listar dados auxilizares
        /// </summary>
        private void ListarDadosAuxiliares()
        {
            var lst1 = _empresaService.Listar();
            var lst2 = _empresaContratoService.Listar();
            Empresas = new List<Empresa>();
            Contratos = new List<EmpresaContrato>();
            Empresas.AddRange(lst1);
            Contratos.AddRange(lst2);
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
                Entity.ClearMessageErro();
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
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
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
            Entity.Validate();
            var hasErro = Entity.HasErrors;

            return hasErro;
        }

        #endregion
 
    }
}