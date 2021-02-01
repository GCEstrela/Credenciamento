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
//using IMOD.CredenciamentoDeskTop.Views;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Infra.Servicos;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class ColaboradoresEmpresasViewModel : ViewModelBase, IComportamento
    {
        private readonly IEmpresaContratosService _empresaContratoService = new EmpresaContratoService();
        private readonly IEmpresaService _empresaService = new EmpresaService();
        private readonly IColaboradorEmpresaService _service = new ColaboradorEmpresaService();
        private readonly IColaboradorCredencialService _serviceCredencial = new ColaboradorCredencialService();
        private readonly IColaboradorService _serviceColaborador = new ColaboradorService();

        private readonly IColaboradorCredencialService _serviceColaboradorCredencial = new ColaboradorCredencialService();
        private readonly IColaboradorEmpresaService _serviceColaboradorEmpresa = new ColaboradorEmpresaService();

        //private readonly object _auxiliaresService;
        private ColaboradorView _colaboradorView;

        private ColaboradorViewModel _viewModelParent;

        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();
        private ConfiguraSistema _configuraSistema;

        #region  Propriedades
        public string ExcluirVisivel { get; set; }
        private int _colaboradorid;
        public List<EmpresaContrato> Contratos { get; private set; }
        public List<Empresa> Empresas { get; private set; }
        public Empresa Empresa { get; set; }
        public ColaboradorEmpresaView Entity { get; set; }
        public string VisibleGruposRegras { get; set; }
        public string VisibleGrupo { get; set; }
        public int WidthGrupo { get; set; } = 80;
        public string VisibleRegra { get; set; }
        public int WidthRegra { get; set; } = 80;
        public string Alignment { get; set; }
        public ObservableCollection<ColaboradorEmpresaView> EntityObserver { get; set; }
        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;
        /// <summary>
        ///     Habilita Combo de Contratos
        /// </summary>
        public bool IsEnableComboContrato
        {
            get
            {
                return !_configuraSistema.Contrato;
            }
        }
        /// <summary>
        ///     Habilita Combo de Contratos
        /// </summary>
        public bool IsEnableColete { get; private set; } = true;
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

        public ColaboradoresEmpresasViewModel()
        {
            try
            {
                if (!Domain.EntitiesCustom.UsuarioLogado.Adm)
                {
                    ExcluirVisivel = "Collapsed";
                }
                else
                {
                    ExcluirVisivel = "Visible";
                }
                ListarDadosAuxiliares();

                Comportamento = new ComportamentoBasico(false, true, true, false, false);
                EntityObserver = new ObservableCollection<ColaboradorEmpresaView>();
                Comportamento.SalvarAdicao += OnSalvarAdicao;
                Comportamento.SalvarEdicao += OnSalvarEdicao;
                Comportamento.Remover += OnRemover;
                Comportamento.Cancelar += OnCancelar;
                base.PropertyChanged += OnEntityChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region  Metodos
        public void BuscarAnexo(int ColaboradorEmpresaId)
        {
            try
            {
                var anexo = _service.BuscarPelaChave(ColaboradorEmpresaId);
                if (anexo == null) return;
                if (anexo.Anexo != null)
                    Entity.Anexo = anexo.Anexo;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }
        public bool ExisteColaboradoContratoAtivo(int _colaborador)
        {
            try
            {
                //Verificar se existe numero de contrato
                var n1 = _service.Listar(_colaborador, Entity.Ativo, null, null, null, null, Entity.EmpresaContratoId);
                if (n1 != null && n1.Count() > 0)
                {
                    WpfHelp.Mbox("Colaborador já esta vinculado à este contrato. Operação cancelada.");
                    //System.Windows.MessageBox.Show("Colaborador já esta vinculado à este contrato. Operação cancelada.");
                    Comportamento.PrepareCancelar();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public bool statusVinculo(int vinculoId)
        {
            ICollection<ColaboradorEmpresa> n1 = new List<ColaboradorEmpresa>();
            n1 = _serviceColaboradorEmpresa.Listar(null, null, null, null, null, null, null, vinculoId);

            foreach (var item in n1)
            {
                if (!item.Ativo)
                    return false;
            }
            return true;
        }
        public bool DadosMotorista(int _colaborador)
        {
            try
            {
                //Verificar se os dados da CNH estão preenchidos na aba Geral
                ICollection<Colaborador> n1 = new List<Colaborador>();
                n1 = _serviceColaborador.Listar(_colaborador, null, null, null, null, null, null);

                foreach (var item in n1)
                {
                    if (item.Cnh == "" || item.CnhCategoria == "" || item.CnhEmissor == "" || item.Cnhuf == "" || item.CnhValidade == null)
                    {
                        return false;

                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            // if (e.PropertyName == "Entity") //habilitar botão alterar todas as vezes em que houver entidade diferente de null
            //Comportamento.IsEnableEditar = true;
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
            Empresas = new List<Empresa>();
            Contratos = new List<EmpresaContrato>();
            ListarDadosEmpresaContratos();
            //_configuraSistema = ObterConfiguracao();
            _configuraSistema = ObterConfiguracao();
            if (!_configuraSistema.Colete) //Se Cole não for automático false
            {
                IsEnableColete = false;
            }
            _configuraSistema.VisibleGruposRegras = false;
            if (_configuraSistema.AssociarGrupos == true || _configuraSistema.AssociarRegras == true)
            {
                _configuraSistema.VisibleGruposRegras = true;
            }
            VisibleGruposRegras = Helper.ExibirCampo(_configuraSistema.VisibleGruposRegras);
            VisibleGrupo = Helper.CollapsedCampo(_configuraSistema.AssociarGrupos);
            VisibleRegra = Helper.CollapsedCampo(_configuraSistema.AssociarRegras);
            if (_configuraSistema.AssociarGrupos == true && _configuraSistema.AssociarRegras == true)
            {
                WidthGrupo = 85;
                WidthRegra = 85;
            }
            else if (_configuraSistema.AssociarGrupos == true)
            {
                WidthGrupo = 180;
                Alignment = "Stretch";
            }
            else if (_configuraSistema.AssociarRegras == true)
            {

                WidthRegra = 180;
                Alignment = "Stretch";
            }

        }
        public void AtualizarConfiguracoes()
        {
            try
            {
                _configuraSistema = ObterConfiguracao();
                if (!_configuraSistema.Colete) //Se Cole não for automático false
                {
                    IsEnableColete = false;
                }
                _configuraSistema.VisibleGruposRegras = false;
                if (_configuraSistema.AssociarGrupos == true || _configuraSistema.AssociarRegras == true)
                {
                    _configuraSistema.VisibleGruposRegras = true;
                }

                VisibleGruposRegras = Helper.ExibirCampo(_configuraSistema.VisibleGruposRegras);
                VisibleGrupo = Helper.CollapsedCampo(_configuraSistema.AssociarGrupos);
                VisibleRegra = Helper.CollapsedCampo(_configuraSistema.AssociarRegras);
                if (_configuraSistema.AssociarGrupos == true && _configuraSistema.AssociarRegras == true)
                {
                    WidthGrupo = 85;
                    WidthRegra = 85;
                }
                else if (_configuraSistema.AssociarGrupos == true)
                {
                    WidthGrupo = 180;
                    Alignment = "Stretch";
                }
                else if (_configuraSistema.AssociarRegras == true)
                {

                    WidthRegra = 180;
                    Alignment = "Stretch";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void ListarContratos(Empresa empresa)
        {
            if (empresa == null) return;

            Contratos.Clear();

            //if (!_configuraSistema.Contrato)
            //{
            var lstContratos = _empresaContratoService.Listar(empresa.EmpresaId).OrderBy(n => n.Descricao).ToList();
            lstContratos.ForEach(n =>
            {
                n.Descricao = $"{n.Descricao} - {n.NumeroContrato}";
                Contratos.Add(n);

            });
            //}
            //else
            //{
            //    var lstContratos = _empresaContratoService.Listar(empresa.EmpresaId).OrderBy(n => n.Descricao).ToList().FirstOrDefault();
            //    if(lstContratos!=null)
            //        Contratos.Add(lstContratos);

            //    Entity.Validade = lstContratos.Validade;
            //}


            //Contratos.AddRange(lstContratos);
            //Manipular concatenaçção de conrato


        }

        /// <summary>
        ///  Listar dados de empresa e contratos
        /// </summary>
        private void ListarDadosEmpresaContratos()
        {

            try
            {
                if (Empresas.Count != 0) return;

                //Empresas.Clear();
                var l2 = _empresaService.Listar().ToList().OrderBy(m => m.Nome);
                Empresas.AddRange(l2);
                var l3 = _empresaContratoService.Listar().ToList();
                Contratos = l3;
                base.OnPropertyChanged("Entity");

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void ListarEmpresa_e_Contratos()
        {

            try
            {
                //if (Empresas.Count != 0) return;

                Empresas.Clear();
                var l2 = _empresaService.Listar().ToList().OrderBy(m => m.Nome);
                Empresas.AddRange(l2);
                var l3 = _empresaContratoService.Listar().ToList();
                Contratos = l3;
                base.OnPropertyChanged("Entity");

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        /// <summary>
        /// Obtem configuração de sistema
        /// </summary>
        /// <returns></returns>
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresServiceConfiguraSistema.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
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
                var empresa = _empresaService.BuscarPelaChave(Entity.EmpresaId);
                Entity.EmpresaNome = empresa.Nome;

                if (Validar()) return;
                if (ExisteColaboradoContratoAtivo(_colaboradorid)) return;

                var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
                n1.ColaboradorId = _colaboradorView.ColaboradorId;

                if (Entity.Validade != null)
                {
                    if (Entity.Validade.Value.AddHours(23).AddMinutes(59).AddSeconds(59) < DateTime.Now)
                    {
                        WpfHelp.PopupBox("Data de Validade não pode ser menor que a data atual. Não é possível continua essa ação.", 1);
                        return;
                    }
                }

                if (_configuraSistema.Contrato)
                {
                    n1.EmpresaContratoId = Contratos[0].EmpresaContratoId;
                }

                //n1.DataFim = DateTime.Today.Date;
                n1.Usuario = Domain.EntitiesCustom.UsuarioLogado.Nome;



                //n1.EmpresaNome = Entity.EmpresaNome;
                //if (Entity.Validade != null)
                //{
                //    n1.Validade = Entity.Validade.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                //    if (n1.Validade < DateTime.Now)
                //    {
                //        WpfHelp.PopupBox("Data de Validade [ " + n1.Validade + " ] do CardHolder é Inválida", 1);
                //        return;
                //    }
                //}
                //else
                //{
                //    //var validadeContrato = _empresaContratoService.Listar(null, null, null, null, null, null, null, null, Entity.EmpresaContratoId).FirstOrDefault();
                //    //n1.Validade = validadeContrato.Validade.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                //    ////var colaborador = _serviceCredencial.BuscarPelaChave(Entity.ColaboradorId);
                //    ////var menordata = _serviceCredencial.ObterDataValidadeCredencial(colaborador.TipoCredencialId, n1.ColaboradorId, validadeContrato.NumeroContrato, _serviceCredencial.TipoCredencial);                    
                //}

                _service.Criar(n1);

                #region Gerar CardHolder
                try
                {
                    var colaboradorAtivo = _serviceColaborador.Listar(n1.ColaboradorId).FirstOrDefault();
                    if (colaboradorAtivo.Ativo)
                    {
                        _serviceCredencial.CriarTitularCartao(new CredencialGenetecService(Main.Engine), new ColaboradorService(), n1);
                        Entity.grupoAlterado = false;
                    }
                }
                catch (Exception ex)
                {
                    WpfHelp.PopupBox(ex);
                }



                #endregion
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<ColaboradorEmpresaView>(n1);
                //Adicionar o nome da empresa e o contrato
                SetDadosEmpresaContrato(n2);
                EntityObserver.Insert(0, n2);
                IsEnableLstView = true;
                _viewModelParent.AtualizarDadosPendencias();
                SelectListViewIndex = 0;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);

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
                //Verificar essa linha
                //entity.EmpresaContratoId = Contratos[0].EmpresaContratoId;
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
            Entity.Ativo = true;
            Entity.grupoAlterado = false;
            Entity.DataInicio = DateTime.Today.Date;
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls(false, false, true, false, false, false);
            //ListarDadosEmpresaContratos();
            ListarEmpresa_e_Contratos();
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

                BuscarAnexo(Entity.ColaboradorEmpresaId);

                var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
                //Gerar matricula
                if (n1.Matricula == null)
                    _service.CriarNumeroMatricula(n1);


                n1.Usuario = Domain.EntitiesCustom.UsuarioLogado.Nome;
                Entity.Matricula = n1.Matricula;

                #region Gerar CardHolder
                try
                {
                    var colaboradorAtivo = _serviceColaborador.Listar(n1.ColaboradorId).FirstOrDefault();
                    if (colaboradorAtivo.Ativo)
                    {
                        _serviceCredencial.CriarTitularCartao(new CredencialGenetecService(Main.Engine), new ColaboradorService(), n1);
                        Entity.CardHolderGuid = n1.CardHolderGuid;
                    }

                }
                catch (Exception ex)
                {
                    WpfHelp.PopupBox(ex);
                }
                #endregion

                Entity.grupoAlterado = false;
                _service.Alterar(n1);
                IsEnableLstView = true;
                SetDadosEmpresaContrato(Entity);

                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
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
                EntityObserver.Clear();
                EntityObserver = new ObservableCollection<ColaboradorEmpresaView>(_entityObserverCloned);
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
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

                var verificarCredencialExistente = _serviceColaboradorCredencial.Listar(null, null, null, null, Entity.ColaboradorId, null, null, null, null, null, null, Entity.EmpresaContratoId);
                if (verificarCredencialExistente.Count == 0)
                {
                    var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
                    _service.Remover(n1);


                    _serviceCredencial.RemoverCardHolder(new CredencialGenetecService(Main.Engine), new ColaboradorService(), n1);

                    //Retirar empresa da coleção
                    EntityObserver.Remove(Entity);
                }
                else
                {
                    if (verificarCredencialExistente.Count == 1)
                    {
                        WpfHelp.PopupBox("Este vínculo não pode ser DELETADO, pois existe " + verificarCredencialExistente.Count + " credencial vinculada." + "\n" + "Remova a credencial associada na aba Credenciais" + "\n" + "Ação cancelada pelo sistema.", 1);
                    }
                    else
                    {
                        WpfHelp.PopupBox("Este vínculo não pode ser DELETADO, pois existem " + verificarCredencialExistente.Count + " credenciais vinculadas." + "\n" + "Remova todas as credenciais associadas na aba Credenciais" + "\n" + "Ação cancelada pelo sistema.", 1);
                    }

                }

                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
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

        private List<ColaboradorEmpresaView> _entityObserverCloned = new List<ColaboradorEmpresaView>();
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
            Entity.grupoAlterado = false;
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls(false, false, true, false, false, false);
            ListarEmpresa_e_Contratos();
            
            CloneObservable();

        }

        /// <summary>
        /// Clone Observable
        /// </summary>
        private void CloneObservable()
        {
            try
            {
                _entityObserverCloned.Clear();
                EntityObserver.ToList().ForEach(n => { _entityObserverCloned.Add((ColaboradorEmpresaView)n.Clone()); });

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            EntityObserver.Clear();
            if (entity == null) return;
            //throw new ArgumentNullException(nameof(entity));
            _colaboradorid = entity.ColaboradorId;

            _colaboradorView = entity;
            //Obter dados
            var list1 = _service.Listar(entity.ColaboradorId);
            var list2 = Mapper.Map<List<ColaboradorEmpresaView>>(list1.OrderByDescending(n => n.ColaboradorEmpresaId));

            EntityObserver = new ObservableCollection<ColaboradorEmpresaView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
            ListarDadosEmpresaContratos();
        }
        public string EncontrarCardHolderGuid(int colaboradorid, int colaboradorEmpresaid)
        {
            try
            {
                if (colaboradorEmpresaid > 0)
                {
                    var list1 = _service.BuscarPelaChave(colaboradorEmpresaid);
                    if (list1 != null)
                    {
                        return list1.CardHolderGuid;
                    }
                }
                //var list1 = _service.Listar(colaboradorid, null, null, null, null, null, null, colaboradorEmpresaid).FirstOrDefault();

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
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


            if (!_configuraSistema.Contrato && Entity.EmpresaContratoId <= 0)
            {
                Entity.SetMessageErro("EmpresaContratoId", "Favor informar o contrato.");
                return true;
            }
            


            var hasErros = Entity.HasErrors;
            if (hasErros) return true;

            return Entity.HasErrors;
        }

        #endregion

    }
}