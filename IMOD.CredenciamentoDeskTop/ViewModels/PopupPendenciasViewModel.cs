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
using IMOD.CredenciamentoDeskTop.Enums;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels.Commands;
using IMOD.CredenciamentoDeskTop.ViewModels.Comportamento;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    internal class PopupPendenciasViewModel : ViewModelBase,IComportamento
    {
        private readonly int _codPendencia;
        private readonly int _identificador;
        private readonly IPendenciaService _service;
        private readonly PendenciaTipo _tipoPendencia;

        #region  Propriedades

        public ComportamentoBasico Comportamento { get; set; }

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase (PrepareCriar, true);

        /// <summary>
        ///     Editar
        /// </summary>
        public ICommand PrepareAlterarCommand => new CommandBase (PrepareAlterar, true);

        /// <summary>
        ///     Cancelar
        /// </summary>
        public ICommand PrepareCancelarCommand => new CommandBase(Comportamento.PrepareCancelar, true);

        /// <summary>
        ///     Salvar
        /// </summary>
        public ICommand PrepareSalvarCommand => new CommandBase (Comportamento.PrepareSalvar, true);

        /// <summary>
        ///     Remover
        /// </summary>
        public ICommand PrepareRemoverCommand => new CommandBase (PrepareRemover, true);

        /// <summary>
        ///  Validar Regras de Negócio 
        /// </summary>
        public bool Validar()
        {
            return false;

        }
    

        /// <summary>
        ///     Tipos de pendencias
        /// </summary>
        private List<TipoPendenciaView> Tipos { get; set; }

        /// <summary>
        ///     Descrição do codigo da pendencia
        /// </summary>
        public string CodPendeciaDescricao { get; private set; }

        public ObservableCollection<PendenciaView> PendenciasObserver { get; set; }
        public PendenciaView Pendencia { get; set; }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="codPendencia">Codido da pendencia</param>
        /// <param name="id">Identificador de uma empresa, veiculo ou colabrador</param>
        /// <param name="tipoPendencia">Tipo de pendencia</param>
        public PopupPendenciasViewModel(int codPendencia, int id, PendenciaTipo tipoPendencia)
        {
            _service = new PendenciaService();

            Comportamento = new ComportamentoBasico (true, true, true, true, true);
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;

            _codPendencia = codPendencia;
            _identificador = id;
            _tipoPendencia = tipoPendencia;

            PendenciasObserver = new ObservableCollection<PendenciaView>();
            ListarTipos();
            ListarPendencias();
        }

        #region  Metodos

       
        private void PrepareCriar()
        {
            Comportamento.PrepareCriar();

            Pendencia = new PendenciaView {CodPendencia = _codPendencia};
            //Set identificador adequado o tipo informado
            if (_tipoPendencia == PendenciaTipo.Empresa)
                Pendencia.EmpresaId = _identificador;
            if (_tipoPendencia == PendenciaTipo.Veiculo)
                Pendencia.VeiculoId = _identificador;
            if (_tipoPendencia == PendenciaTipo.Colaborador)
                Pendencia.ColaboradorId = _identificador;
            //Adicionar um objeto a coleção
            PendenciasObserver.Add (Pendencia);
        }
        private void PrepareAlterar()
        {
            if (Pendencia == null) return;
            Comportamento.PrepareAlterar();
        }
        private void PrepareRemover()
        {
            if (Pendencia == null) return;
            Comportamento.PrepareRemover();
        }

        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                var entity = Pendencia;
                if (entity == null) return;
                var entityConv = Mapper.Map<Pendencia> (entity);
                _service.Criar (entityConv);
                entity.PendenciaId = entityConv.PendenciaId;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
              
                var entity = Pendencia;
                var entityConv = Mapper.Map<Pendencia> (entity);
                _service.Alterar (entityConv);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PendenciasObserver == null) return;
                var lst = PendenciasObserver.Where (n => n.PendenciaId == 0).ToList();
                foreach (var item in lst)
                    PendenciasObserver.Remove (item);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        private void OnRemover(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = Pendencia;
                var entityConv = Mapper.Map<Pendencia> (entity);
                _service.Remover (entityConv);
                PendenciasObserver.Remove (entity);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        private void ListarTipos()
        {
            var aux = new DadosAuxiliaresFacadeService();
            var lst = _service.TipoPendenciaService.Listar();
            var lst2 = Mapper.Map<List<TipoPendenciaView>> (lst);
            Tipos = new List<TipoPendenciaView>();
            Tipos.AddRange (lst2);
            //Obter descrição
            if (Tipos.Exists (n => n.CodPendencia == _codPendencia))
                CodPendeciaDescricao = Tipos.Single (n => n.CodPendencia == _codPendencia).Descricao;
        }

        /// <summary>
        ///     Listar
        /// </summary>
        private void ListarPendencias()
        {
            var lst = new List<Pendencia>();
            switch (_tipoPendencia)
            {
                case PendenciaTipo.Empresa:
                    var c1 = _service.Listar().Where (n => n.EmpresaId == _identificador & n.CodPendencia == _codPendencia);
                    lst.AddRange (c1);
                    break;
                case PendenciaTipo.Veiculo:
                    var c2 = _service.Listar().Where (n => n.VeiculoId == _identificador & n.CodPendencia == _codPendencia);
                    lst.AddRange (c2);
                    break;
                case PendenciaTipo.Colaborador:
                    var c3 = _service.Listar().Where (n => n.ColaboradorId == _identificador & n.CodPendencia == _codPendencia);
                    lst.AddRange (c3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var lst2 = Mapper.Map<List<PendenciaView>> (lst);
            PendenciasObserver.Clear();
            lst2.ForEach (n =>
            {
                n.CodPendenciaDescricao = CodPendeciaDescricao;
                PendenciasObserver.Add (n);
            });
        }

        #endregion
    }
}