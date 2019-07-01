// ***********************************************************************
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
using System.Windows.Data;
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
       
        private  ColaboradorViewModel _viewModelParent;

        private readonly IEmpresaContratosService _serviceContratos = new EmpresaContratoService();
        private ConfiguraSistema _configuraSistema;

        #region  Propriedades
        /// <summary>
        ///     Seleciona o indice da tabcontrol desejada
        /// </summary>
        public short SelectListViewIndex { get; set; } 

        public List<Curso> Cursos { get; private set; }       
        public ColaboradorCursoView Entity { get; set; }
        public ObservableCollection<ColaboradorCursoView> EntityObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;
        /// <summary>
        ///     Tamanho do Arquivo
        /// </summary>
        public int IsTamanhoArquivo
        {
            get
            {
                return _configuraSistema.arquivoTamanho;
            }
        }
        #endregion

        #region Inicializacao

        public ColaboradoresCursosViewModel()
        {
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(false, true, false, false, false);
            EntityObserver = new ObservableCollection<ColaboradorCursoView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
            base.PropertyChanged += OnEntityChanged;
        }

        #endregion

        #region  Metodos
        public void BuscarAnexo(int ColaboradorCursoID)
        {
            try
            {
                if (Entity.Arquivo != null) return;
                var anexo = _service.BuscarPelaChave(ColaboradorCursoID);
                Entity.Arquivo = anexo.Arquivo;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == "Entity") //habilitar botão alterar todas as vezes em que houver entidade diferente de null
            //  Comportamento.IsEnableEditar = true;
            if (e.PropertyName == "Entity")
            {
                Comportamento.IsEnableEditar = Entity != null;
                Comportamento.isEnableRemover = Entity != null;

            }
        }

        /// <summary>
        ///     Listar dados auxilizares
        /// </summary>
        private void ListarDadosAuxiliares()
        {
            var lst1 = _auxiliaresService.CursoService.Listar();
            Cursos = new List<Curso>();
            Cursos.AddRange (lst1);
            
            _configuraSistema = ObterConfiguracao();
        }
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresService.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
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
                _viewModelParent.AtualizarDadosPendencias();
                SelectListViewIndex = 0;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
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
            var curso = Cursos.FirstOrDefault (n => n.CursoId == cursoId);  
            return curso == null ? "" : curso.Descricao;
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
            if (Cursos!=null)
                Cursos.Clear();

            ListarDadosAuxiliares();
            CollectionViewSource.GetDefaultView(Cursos).Refresh();

            Entity = new ColaboradorCursoView();
            Entity.Controlado = true;
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
            _viewModelParent.AtualizarDadosPendencias();
            _viewModelParent.HabilitaControleTabControls(false, false, false, true, false, false);
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

                BuscarAnexo(Entity.ColaboradorCursoId);

                var n1 = Mapper.Map<ColaboradorCurso> (Entity);
                _service.Alterar (n1);
                var n2 = EntityObserver.FirstOrDefault(n => n.ColaboradorCursoId == n1.ColaboradorCursoId);
                if (n2 == null) return;
                n2.CursoNome = NomeCurso(n2.CursoId);
                IsEnableLstView = true;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
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
                if (Entity != null) Entity.ClearMessageErro();
                Entity = null;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
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
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
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
            if (Entity == null)
            {
                WpfHelp.PopupBox("Selecione um item da lista", 1);
                return;
            }
            //if (Cursos != null)
            //    Cursos.Clear();

            //ListarDadosAuxiliares();
            CollectionViewSource.GetDefaultView(Cursos).Refresh();

            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls(false, false, false, true, false, false);
        }

        public void AtualizarDados(ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {
            _viewModelParent = viewModelParent;
            this.AtualizarDados(entity);
        }

        public void AtualizarDados(ColaboradorView entity)
        {
            EntityObserver.Clear();
            if (entity == null) return;
                //throw new ArgumentNullException (nameof (entity));

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
            if (Entity == null) return true;
            Entity.Validate();
            var hasErros = Entity.HasErrors;
            if (hasErros) return true;

            return Entity.HasErrors;
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