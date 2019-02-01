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
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Funcoes;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels.Commands;
using IMOD.CredenciamentoDeskTop.ViewModels.Comportamento;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using AutorizacaoView = IMOD.CredenciamentoDeskTop.Views.Model.AutorizacaoView;
//using IMOD.Domain.EntitiesCustom;
//using VeiculosCredenciaisView = IMOD.Domain.EntitiesCustom.VeiculosCredenciaisView;
//using EmpresaView = iModSCCredenciamento.Views.Model.EmpresaView;
using VeiculoEmpresaView = IMOD.CredenciamentoDeskTop.Views.Model.VeiculoEmpresaView;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class VeiculosCredenciaisViewModel : ViewModelBase, IComportamento
    {
        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado; 
        private readonly IVeiculoCredencialService _service = new VeiculoCredencialService(); 
        private readonly IVeiculoEmpresaService _VeiculoEmpresaService = new VeiculoEmpresaService();
        private readonly IEmpresaLayoutCrachaService _EmpresaLayoutCrachaService = new EmpresaLayoutCrachaService(); 
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaLayoutCrachaService _empresaLayoutCracha = new EmpresaLayoutCrachaService(); 
        private VeiculoView _VeiculoView;
        private VeiculoEmpresaView _VeiculoEmpresaView;
        private VeiculoCredencial _VeiculoCredencial;

        private readonly IVeiculoCredencialimpressaoService _ImpressaoService = new VeiculoCredencialimpressaoService();

        VeiculoCredencialimpressao _veiculoCredencialImpressao = new VeiculoCredencialimpressao();
        SCManager _sc = new SCManager();

        #region  Propriedades

        public List<CredencialStatus> CredencialStatus { get; set; }
        public List<CredencialMotivo> CredencialMotivo { get; set; }
        public List<FormatoCredencial> FormatoCredencial { get; set; }
        public List<TipoCredencial> TipoCredencial { get; set; }
        public List<EmpresaLayoutCracha> EmpresaLayoutCracha { get; set; }
        public List<TecnologiaCredencial> TecnologiasCredenciais { get; set; }
        public List<VeiculoEmpresa> VeiculosEmpresas { get; set; }
        public VeiculoEmpresa VeiculoEmpresa { get; set; }
        public List<AreaAcesso> VeiculoPrivilegio { get; set; }

        //public EmpresaLayoutCrachaView EntityEmpresasLayoutsCrachas { get; set; }
        //public LayoutCrachaView EntityLayoutCrachaView { get; set; }
        //public FormatoCredencialView EntityFormatoCredencialView { get; set; }
        //public EmpresaContratoView EntityEmpresaContratoView { get; set; }

        public IMOD.CredenciamentoDeskTop.Views.Model.VeiculoCredencialView Entity { get; set; }
        public IMOD.CredenciamentoDeskTop.Views.Model.VeiculoCredencialView EntityTmp { get; set; }


        public ObservableCollection<VeiculoCredencialView> EntityObserver { get; set; }
        public ObservableCollection<AutorizacaoView> Autorizacao { get; set; }


        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; set; } = true;

        public bool IsEnablePrint { get; set; } = true;

        public bool IsEnableFixo { get; set; } = false;

        #endregion

        #region Inicializacao

        public VeiculosCredenciaisViewModel()
        {
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(false, true, true, false, false);
            EntityObserver = new ObservableCollection<VeiculoCredencialView>();
            IsEnablePrint = false;
            IsEnableFixo = false;

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

        public void AtualizarVinculoVeiculoEmpresa(VeiculoView _entity)
        {
            if (_entity == null ) { return;}

            var lista1 = _VeiculoEmpresaService.Listar(_entity.EquipamentoVeiculoId, true);
            var lista2 = Mapper.Map<List<VeiculoEmpresa>>(lista1.OrderByDescending(n => n.VeiculoEmpresaId).Where(n => n.Ativo.Equals(true)).ToList());
          
            VeiculosEmpresas.Clear();
            
            lista2.ForEach(n =>
            {
                n.EmpresaNome.Trim();
                n.Descricao.Trim();
                VeiculosEmpresas.Add(n);
            });

        }

        public void AtualizarDados(VeiculoView entity)
        {
            _VeiculoView = entity ?? throw new ArgumentNullException(nameof(entity));
            ////Obter dados
            var list1 = _service.ListarView(entity.EquipamentoVeiculoId, null, null, null, null).ToList();
            var list2 = Mapper.Map<List<VeiculoCredencialView>>(list1.OrderByDescending(n => n.VeiculoCredencialId));
            EntityObserver = new ObservableCollection<VeiculoCredencialView>();
            list2.ForEach(n =>
            {
                EntityObserver.Add(n);
            });
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

                if (Entity == null)
                {
                    return;
                }
                if (Validar()) return;

                var n1 = Mapper.Map<VeiculoCredencial>(Entity);

                n1.CredencialMotivoId = Entity.CredencialMotivoId;
                n1.CredencialStatusId = Entity.CredencialStatusId;
                n1.FormatoCredencialId = Entity.FormatoCredencialId;
                n1.LayoutCrachaId = Entity.LayoutCrachaId;
                n1.TecnologiaCredencialId = Entity.TecnologiaCredencialId;
                n1.TipoCredencialId = Entity.TipoCredencialId;

                _service.Criar(n1);

                var list1 = _service.ListarView(_VeiculoView.EquipamentoVeiculoId, null, null, null, null).ToList();
                var list2 = Mapper.Map<List<VeiculoCredencialView>>(list1.OrderByDescending(n => n.VeiculoCredencialId));
                EntityObserver = new ObservableCollection<VeiculoCredencialView>();
                list2.ForEach(n =>
                {
                    EntityObserver.Add(n);
                });

                IsEnableLstView = true;
                IsEnablePrint = true;
                IsEnableFixo = false;
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
            Entity = new VeiculoCredencialView();
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
            IsEnablePrint = false;
            IsEnableFixo = false;

            //var lst6 = _VeiculoEmpresaService.Listar(Entity.VeiculoId);
            //VeiculosEmpresas = new List<VeiculoEmpresa>();

            //VeiculosEmpresas.Where(x => x.VeiculoId == Entity.VeiculoId);
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

                var n1 = Mapper.Map<VeiculoCredencial>(Entity);
                _service.Alterar(n1);
                IsEnableLstView = true;
                IsEnablePrint = true;
                IsEnableFixo = false;
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
                Entity = EntityTmp;
                IsEnableLstView = true;
                
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                IsEnableFixo = false;
                

                if (Entity != null)
                {
                    //if (Entity.VeiculoCredencialId == 0)
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
                if (Entity == null)
                {
                    return;
                }

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes)
                {
                    return;
                }

                var n1 = Mapper.Map<VeiculoCredencial>(Entity);
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
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
            //try
            //{
            //    if (_VeiculoView == null) return;

            //    var pesquisa = NomePesquisa;

            //    var num = PesquisarPor;

            //    //Por nome
            //    if (num.Key == 1)
            //    {
            //        var l1 = _service.Listar(_VeiculoView.EmpresaId, $"%{pesquisa}%");
            //        PopularObserver(l1);
            //    }
            //    //Por CPF
            //    if (num.Key == 2)
            //    {
            //        var l1 = _service.Listar(_VeiculoView.EmpresaId, null, $"%{pesquisa}%", null, null, null);
            //        PopularObserver(l1);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Utils.TraceException(ex);
            //}
        }

        private void PopularObserver(ICollection<VeiculoCredencial> list)
        {
            try
            {
                var list2 = Mapper.Map<List<VeiculoCredencialView>>(list.OrderBy(n => n.VeiculoCredencialId));
                EntityObserver = new ObservableCollection<VeiculoCredencialView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
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
        public ICommand PrepareSalvarCommand => new CommandBase(PrepareSalvar, true);

        /// <summary>
        ///     Remover
        /// </summary>
        public ICommand PrepareRemoverCommand => new CommandBase(PrepareRemover, true);

        /// <summary>
        ///     Pesquisar
        /// </summary>
        public ICommand PesquisarCommand => new CommandBase(Pesquisar, true);

        #endregion

        private void CarregaUI()
        {
            //CarregaColecaoEmpresas();
            //CarregaColecaoAreasAcessos();
            //CarregaColecaoCredenciaisStatus();
            //CarregaColecaoTiposCredenciais();
            //CarregaColecaoTecnologiasCredenciais();
            //CarregaColeçãoFormatosCredenciais();
            //CarregaColecaoCredenciaisMotivos();

            //CarregaColecaoVeiculosPrivilegios();
        }

        /// <summary>
        /// Imprimir Credencial
        /// </summary>
        public void OnImprimirAutorizacao()
        {
            try
            {
                if (Entity.Validade == null || !Entity.Ativa || Entity.LayoutCrachaId == 0)
                {
                    WpfHelp.PopupBox("Não foi possível imprimir esta credencial!", 3);
                    return;
                }
                var list1 = _service.ListarAutorizacaoView(Entity.VeiculoCredencialId);
                var list2 = Mapper.Map<List<AutorizacaoView>>(list1);
                var observer = new ObservableCollection<AutorizacaoView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Autorizacao = observer;

                var layoutCracha = _auxiliaresService.LayoutCrachaService.BuscarPelaChave(Entity.LayoutCrachaId);

                string _ArquivoRPT = Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;
                _ArquivoRPT = Path.ChangeExtension(_ArquivoRPT, ".rpt");
                byte[] arrayFile = Convert.FromBase64String(layoutCracha.LayoutRpt);
                File.WriteAllBytes(_ArquivoRPT, arrayFile);
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(_ArquivoRPT);

                reportDocument.SetDataSource(Autorizacao);

                PopupAutorizacao _popupCredencial = new PopupAutorizacao(reportDocument);
                _popupCredencial.ShowDialog();

                bool _result = _popupCredencial.Result;

                if (_result)
                {
                    _veiculoCredencialImpressao.VeiculoCredencialId = Entity.VeiculoCredencialId;
                    _veiculoCredencialImpressao.DataImpressao = DateTime.Now;
                    if (Entity.IsencaoCobranca)
                    {
                        _veiculoCredencialImpressao.Cobrar = false;
                    }
                    else
                    {
                        _veiculoCredencialImpressao.Cobrar = true;
                    }

                    _ImpressaoService.Criar(_veiculoCredencialImpressao);
                    WpfHelp.PopupBox("Impressão Efetuada com Sucesso!", 1);
                    Entity.Impressa = true;
                     

                    _sc.Vincular(Entity.VeiculoNome.Trim(), Entity.Cnpj.Trim(), Entity.Cnpj.Trim(),
                        Entity.EmpresaNome.Trim(), Entity.Colete.Trim(), Entity.Cargo.Trim(),
                        Entity.Fc.ToString().Trim(), Entity.NumeroCredencial.Trim(),
                        Entity.FormatoCredencialDescricao.Trim(), Entity.Validade.ToString(),
                        layoutCracha.LayoutCrachaGuid, Entity.VeiculoFoto.ConverterBase64StringToBitmap());
                }
                File.Delete(_ArquivoRPT);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion
         

        #region  Metodos
        private void ListarDadosAuxiliares()
        {
            var lst0 = _auxiliaresService.CredencialStatusService.Listar();
            CredencialStatus = new List<CredencialStatus>();
            CredencialStatus.AddRange(lst0);
            
            var lst1 = _auxiliaresService.CredencialMotivoService.Listar();
            CredencialMotivo = new List<CredencialMotivo>();
            CredencialMotivo.AddRange(lst1);
            
            var lst2 = _auxiliaresService.FormatoCredencialService.Listar();
            FormatoCredencial = new List<FormatoCredencial>();
            FormatoCredencial.AddRange(lst2);
            
            var lst3 = _auxiliaresService.TipoCredencialService.Listar();
            TipoCredencial = new List<TipoCredencial>();
            TipoCredencial.AddRange(lst3);

            var lst5 = _auxiliaresService.TecnologiaCredencialService.Listar();
            TecnologiasCredenciais = new List<TecnologiaCredencial>();
            TecnologiasCredenciais.AddRange(lst5);

            var lst6 = _VeiculoEmpresaService.Listar(null, true).OrderByDescending(n => n.VeiculoEmpresaId).Where(n => n.Ativo.Equals(true)).ToList();
            VeiculosEmpresas = new List<VeiculoEmpresa>();
            VeiculosEmpresas.AddRange(lst6);

            var lst7 = _auxiliaresService.AreaAcessoService.Listar();
            VeiculoPrivilegio = new List<AreaAcesso>();
            VeiculoPrivilegio.AddRange(lst7);

        }
        public void CarregaColecaoLayoutsCrachas(int _empresaId)
        {
            try
            {
                EmpresaLayoutCracha = new List<EmpresaLayoutCracha>();
                var service = new EmpresaLayoutCrachaService();
                var list1 = service.ListarLayoutCrachaPorEmpresaView(_empresaId);
                var list2 = Mapper.Map<List<EmpresaLayoutCracha>>(list1);
                EmpresaLayoutCracha = list2;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoCredenciaisMotivos(int _idStatusCredencial)
        {
            try
            {
                var lst1 = _auxiliaresService.CredencialMotivoService.Listar(null, null, _idStatusCredencial);

                if (CredencialMotivo != null && CredencialMotivo.Count() > 0) CredencialMotivo.Clear();
                CredencialMotivo = new List<CredencialMotivo>();
                CredencialMotivo.AddRange(lst1);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        #endregion

        private void PrepareSalvar()
        {
            if (Validar()) return;
            Comportamento.PrepareSalvar();
        }

        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                WpfHelp.PopupBox("Selecione um item da lista", 1);
                return;
            }

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
            return hasErros;
        }
         



    }
}