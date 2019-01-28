// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
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
    public class ColaboradoresCursosViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly ICursoService _cursoService = new CursoService();
        private readonly IColaboradorCursoService _service = new ColaboradorCursosService();
        private ColaboradorView _colaboradorView;
        private ColaboradorCursoView _entidadeTmp = new ColaboradorCursoView();

        #region  Propriedades

        public List<Curso> Cursos { get; private set; }
        public ColaboradorCursoView Entity { get; set; }
        public ObservableCollection<ColaboradorCursoView> EntityObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        #region Inicializacao

        public ColaboradoresCursosViewModel()
        {
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico (true, true, true, false, false);
            EntityObserver = new ObservableCollection<ColaboradorCursoView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Listar dados auxilizares
        /// </summary>
        private void ListarDadosAuxiliares()
        {
            var lst1 = _auxiliaresService.CursoService.Listar();
            Cursos = new List<Curso>();
            Cursos.AddRange (lst1);
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

                Entity.ColaboradorId = _colaboradorView.ColaboradorId;
                var n1 = Mapper.Map<ColaboradorCurso> (Entity);
                n1.ColaboradorId = _colaboradorView.ColaboradorId;
                _service.Criar (n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<ColaboradorCursoView> (n1);
                n2.CursoNome = NomeCurso (n2.CursoId);
                EntityObserver.Insert (0, n2);
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        /// Obter o nome do curso
        /// </summary> 
        /// <param name="cursoId">Identificador curso</param>
        private string NomeCurso(int cursoId)
        { 
            var curso = _auxiliaresService.CursoService.BuscarPelaChave (cursoId);
            return curso.Descricao;
        }

        private void PrepareSalvar()
        {
            if (Validar()) return;
            Comportamento.PrepareSalvar();
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            _entidadeTmp = Entity;
            Entity = new ColaboradorCursoView();
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

                var n1 = Mapper.Map<ColaboradorCurso> (Entity);
                _service.Alterar (n1);
                var n2 = EntityObserver.FirstOrDefault(n => n.ColaboradorCursoId == n1.ColaboradorCursoId);
                if (n2 == null) return;
                n2.CursoNome = NomeCurso(n2.CursoId);
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
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
                Entity = _entidadeTmp;
                Entity.ClearMessageErro();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
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

                var n1 = Mapper.Map<ColaboradorCurso> (Entity);
                _service.Remover (n1);
                //Retirar empresa da coleção
                EntityObserver.Remove (Entity);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        /// <summary>
        ///     Acionado antes de alterar
        /// </summary>
        private void PrepareAlterar()
        {
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
        }

        public void AtualizarDados(ColaboradorView entity)
        {
            if (entity == null)
                throw new ArgumentNullException (nameof (entity));

            _colaboradorView = entity;
            //Obter dados
            var list1 = _service.Listar (entity.ColaboradorId);
            var list2 = Mapper.Map<List<ColaboradorCursoView>> (list1.OrderByDescending (n => n.ColaboradorCursoId));
            EntityObserver = new ObservableCollection<ColaboradorCursoView>();
            list2.ForEach (n =>
            {
                EntityObserver.Add (n);
                n.CursoNome = _cursoService.BuscarPelaChave (n.CursoId).Descricao;
            });
        }

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        public bool Validar()
        {
            Entity.Validate();
            var hasErro = Entity.HasErrors;

            return hasErro;
        }

        #endregion

        #region Propriedade Commands

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase (PrepareCriar, true);

        public ComportamentoBasico Comportamento { get; set; }

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
        public ICommand PrepareRemoverCommand => new CommandBase (Comportamento.PrepareRemover, true);

        #endregion
    }
}