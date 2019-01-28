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
    public class EmpresasSignatariosViewModel : ViewModelBase, IComportamento
    {
        private readonly IEmpresaSignatarioService _service = new EmpresaSignatarioService();
        private EmpresaView _empresaView;

        #region  Propriedades

        public EmpresaSignatarioView Entity { get; set; }
        public ObservableCollection<EmpresaSignatarioView> EntityObserver { get; set; }


        EmpresaSignatarioView EntidadeTMP = new EmpresaSignatarioView();


        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        public EmpresasSignatariosViewModel()
        {
            ItensDePesquisaConfigura();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

        public void AtualizarDados(EmpresaView entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _empresaView = entity;
            //Obter dados
            var list1 = _service.Listar(entity.EmpresaId, null, null, null, null, null, null);
            var list2 = Mapper.Map<List<EmpresaSignatarioView>>(list1.OrderByDescending(n => n.EmpresaSignatarioId));
            EntityObserver = new ObservableCollection<EmpresaSignatarioView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "Nome"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "CPF"));
            PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
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

                var n1 = Mapper.Map<EmpresaSignatario>(Entity);
                n1.EmpresaId = _empresaView.EmpresaId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<EmpresaSignatarioView>(n1);
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
            EntidadeTMP = Entity;
            Entity = new EmpresaSignatarioView();
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
                EntidadeTMP = Entity;

                if (Validar()) return;
                
                var n1 = Mapper.Map<EmpresaSignatario>(Entity);
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
                Entity = EntidadeTMP;

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
                if (Entity == null)
                {
                    return;
                }

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes)
                {
                    return;
                }

                var n1 = Mapper.Map<EmpresaSignatario>(Entity);
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

        /// <summary>
        ///     Acionado antes de alterar
        /// </summary>
        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                WpfHelp.PopupBox("Selecione um Item na Lista de Representantes", 1);
                return;
            }
            EntidadeTMP = Entity;
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
        }

        /// <summary>
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                if (_empresaView == null)
                {
                    return;
                }

                var pesquisa = NomePesquisa;

                var num = PesquisarPor;

                //Por nome
                if (num.Key == 1)
                {
                    var l1 = _service.Listar(_empresaView.EmpresaId, $"%{pesquisa}%");
                    PopularObserver(l1);
                }
                //Por CPF
                if (num.Key == 2)
                {
                    var l1 = _service.Listar(_empresaView.EmpresaId, null, $"%{pesquisa}%", null, null, null);
                    PopularObserver(l1);
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PopularObserver(ICollection<EmpresaSignatario> list)
        {
            try
            {
                var list2 = Mapper.Map<List<EmpresaSignatarioView>>(list.OrderBy(n => n.Nome));
                EntityObserver = new ObservableCollection<EmpresaSignatarioView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
                //Empresas = observer;
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///  Validar Regras de Negócio 
        /// </summary>
        public bool Validar()
        {
            Entity.Validate();
            
            IsEnableLstView = true;
            Entity = EntidadeTMP;

            return true;

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
    }
}







