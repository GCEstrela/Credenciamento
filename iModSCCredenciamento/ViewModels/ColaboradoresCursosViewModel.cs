using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Xml;
using AutoMapper;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using iModSCCredenciamento.Views.Model;
using System.Windows;
using System.Windows.Input;
using iModSCCredenciamento.ViewModels.Commands;
using iModSCCredenciamento.ViewModels.Comportamento;

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradoresCursosViewModel : ViewModelBase
    {
        //private readonly IColaboradorCursoService _empresaContratoService = new ColaboradorCursosService();
        //private readonly IEmpresaService _empresaService = new EmpresaService();
        private readonly IColaboradorCursoService _service = new ColaboradorCursosService();
        //private readonly IColaboradorService _serviceCurso = new ColaboradorService();
        private ColaboradorCursoView _colaboradorCursoView;

        

        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        //public List<ClasseCursos.Curso> ObterListaListaCursos { get; private set; }
        #region  Propriedades

        public List<Curso> Cursos { get; private set; }
        ColaboradorCursoView EntityTemp = new ColaboradorCursoView();
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
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
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
            //var lst2 = _empresaContratoService.Listar();
            Cursos = new List<Curso>();
            //Contratos = new List<EmpresaContrato>();
            Cursos.AddRange(lst1);
            //Contratos.AddRange(lst2);
        }

        public void ListarContratos(Empresa empresa)
        {


            if (empresa == null) return;

           // var lstContratos = _empresaContratoService.Listar(empresa.EmpresaId);
            //Contratos.AddRange(lstContratos);


        }

        /// <summary>
        ///     Acionado antes de remover
        /// </summary>
        private void PrepareRemover()
        {
            //Comportamento.PrepareRemover();
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
                var n1 = Mapper.Map<ColaboradorCurso>(Entity);
                n1.ColaboradorId = _colaboradorCursoView.ColaboradorCursoId;
                _service.Criar(n1);
                ////Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<ColaboradorCursoView>(n1);
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
            EntityTemp = Entity;
            Entity = new ColaboradorCursoView();
            //Entity.Matricula = string.Format("{0:#,##0}", Global.GerarID()) + "-" + String.Format("{0:yy}", DateTime.Now);

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
                var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
                //_service.Alterar(n1);
                //IsEnableLstView = true;
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
               // IsEnableLstView = true;
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
                //if (result != DialogResult.Yes) return;

                var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
                //_service.Remover(n1);
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
            //Comportamento.PrepareAlterar();
            //IsEnableLstView = false;
        }

        public void AtualizarDados(ColaboradorCursoView entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _colaboradorCursoView = entity;
            //Obter dados
            var list1 = _service.Listar(entity.ColaboradorId);
            var list2 = Mapper.Map<List<ColaboradorCursoView>>(list1);
            EntityObserver = new ObservableCollection<ColaboradorCursoView>();
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

        #endregion
    }
}
