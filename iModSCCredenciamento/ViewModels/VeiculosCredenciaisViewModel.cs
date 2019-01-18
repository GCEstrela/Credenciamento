// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels.Commands;
using iModSCCredenciamento.ViewModels.Comportamento;
using iModSCCredenciamento.Views.Model;
using iModSCCredenciamento.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
//using IMOD.Domain.EntitiesCustom;
//using VeiculosCredenciaisView = IMOD.Domain.EntitiesCustom.VeiculosCredenciaisView;
//using EmpresaView = iModSCCredenciamento.Views.Model.EmpresaView;
using VeiculoEmpresaView = iModSCCredenciamento.Views.Model.VeiculoEmpresaView;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class VeiculosCredenciaisViewModel : ViewModelBase, IComportamento
    {
        //private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        //private readonly IVeiculoService _service = new VeiculoService();
        private readonly IVeiculoCredencialService _service = new VeiculoCredencialService();
        //private readonly IVeiculoCredencialImpressaoService _ImpressaoService = new VeiculoCredencialImpressaoService();


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

        public VeiculosCredenciaisView Entity { get; set; }
        public ObservableCollection<VeiculosCredenciaisView> EntityObserver { get; set; }
        public ObservableCollection<iModSCCredenciamento.Views.Model.AutorizacaoView> Autorizacao { get; set; }


        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; set; } = true;

        #endregion 
        #region Inicializacao

        public VeiculosCredenciaisViewModel()
        {
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            EntityObserver = new ObservableCollection<VeiculosCredenciaisView>();

            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

        //TODO: AtualizarVinculo
        public void AtualizarVinculo(VeiculoView entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var lista1 = _VeiculoEmpresaService.Listar(entity.EquipamentoVeiculoId);
            var lista2 = Mapper.Map<List<VeiculoEmpresa>>(lista1.OrderByDescending(n => n.VeiculoEmpresaId).ToList());

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
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _VeiculoView = entity;
            ////Obter dados
            var list1 = _service.ListarView(entity.EquipamentoVeiculoId, null, null, null, null).ToList();
            var list2 = Mapper.Map<List<VeiculosCredenciaisView>>(list1.OrderByDescending(n => n.VeiculoCredencialId));
            EntityObserver = new ObservableCollection<VeiculosCredenciaisView>();
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

                var n1 = Mapper.Map<VeiculoCredencial>(Entity);

                n1.CredencialMotivoId = Entity.CredencialMotivoId;
                n1.CredencialStatusId = Entity.CredencialStatusId;
                n1.FormatoCredencialId = Entity.FormatoCredencialId;
                n1.LayoutCrachaId = Entity.LayoutCrachaId;
                n1.TecnologiaCredencialId = Entity.TecnologiaCredencialId;
                n1.TipoCredencialId = Entity.TipoCredencialId;

                _service.Criar(n1);
                //////Adicionar no inicio da lista um item a coleção
                //var n2 = Mapper.Map<VeiculoCredencialView>(n1);
                //EntityObserver = new ObservableCollection<VeiculosCredenciaisView>();
                //Entity.VeiculoCredencialId = n1.VeiculoCredencialId;

                var list1 = _service.ListarView(_VeiculoView.EquipamentoVeiculoId, null, null, null, null).ToList();
                var list2 = Mapper.Map<List<VeiculosCredenciaisView>>(list1.OrderByDescending(n => n.VeiculoCredencialId));
                EntityObserver = new ObservableCollection<VeiculosCredenciaisView>();
                list2.ForEach(n =>
                {
                    EntityObserver.Add(n);
                });

                //var n2 = _service.BuscarCredencialPelaChave(Entity.VeiculoCredencialId);
                //EntityObserver.Insert(0, n2);
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
            //Entity = new VeiculoCredencialView();
            Entity = new VeiculosCredenciaisView();
            Comportamento.PrepareCriar();
            IsEnableLstView = false;

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

                var n1 = Mapper.Map<VeiculoCredencial>(Entity);
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
        ///     Acionado antes de alterar
        /// </summary>
        private void PrepareAlterar()
        {
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
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
                var list2 = Mapper.Map<List<VeiculosCredenciaisView>>(list.OrderBy(n => n.VeiculoCredencialId));
                EntityObserver = new ObservableCollection<VeiculosCredenciaisView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
                //Empresas = observer;
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        public void Validar()
        {
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
                var list2 = Mapper.Map<List<iModSCCredenciamento.Views.Model.AutorizacaoView>>(list1);
                var observer = new ObservableCollection<iModSCCredenciamento.Views.Model.AutorizacaoView>();
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

                    BitmapImage _foto = Conversores.STRtoIMG(Entity.VeiculoFoto) as BitmapImage;

                    _sc.Vincular(Entity.VeiculoNome.Trim(), Entity.Cnpj.Trim(), Entity.Cnpj.Trim(),
                        Entity.EmpresaNome.Trim(), Entity.Colete.Trim(), Entity.Cargo.Trim(),
                        Entity.Fc.ToString().Trim(), Entity.NumeroCredencial.Trim(),
                        Entity.FormatoCredencialDescricao.Trim(), Entity.Validade.ToString(),
                        layoutCracha.LayoutCrachaGuid, Conversores.BitmapImageToBitmap(_foto));
                }
                File.Delete(_ArquivoRPT);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

        #region Variaveis Privadas

        //public List<VeiculoEmpresaView> VeiculoEmpresa { get; private set; }



        #endregion

        #region  Metodos
        private void ListarDadosAuxiliares()
        {//ok
            var lst0 = _auxiliaresService.CredencialStatusService.Listar();
            CredencialStatus = new List<CredencialStatus>();
            CredencialStatus.AddRange(lst0);
            //ok
            var lst1 = _auxiliaresService.CredencialMotivoService.Listar();
            CredencialMotivo = new List<CredencialMotivo>();
            CredencialMotivo.AddRange(lst1);
            //ok
            var lst2 = _auxiliaresService.FormatoCredencialService.Listar();
            FormatoCredencial = new List<FormatoCredencial>();
            FormatoCredencial.AddRange(lst2);
            //ok
            var lst3 = _auxiliaresService.TipoCredencialService.Listar();
            TipoCredencial = new List<TipoCredencial>();
            TipoCredencial.AddRange(lst3);

            //var lst4 = _empresaLayoutCracha.Listar();
            //EmpresaLayoutCracha = new List<EmpresaLayoutCracha>();
            //EmpresaLayoutCracha.AddRange(lst4);

            var lst5 = _auxiliaresService.TecnologiaCredencialService.Listar();
            TecnologiasCredenciais = new List<TecnologiaCredencial>();
            TecnologiasCredenciais.AddRange(lst5);

            var lst6 = _VeiculoEmpresaService.Listar();
            VeiculosEmpresas = new List<VeiculoEmpresa>();
            VeiculosEmpresas.AddRange(lst6);

            //var lst6 = _VeiculoEmpresaService.ListarView();
            //VeiculosEmpresas = new List<IMOD.Domain.EntitiesCustom.VeiculoEmpresaView>();
            //VeiculosEmpresas.AddRange(lst6);

            var lst7 = _auxiliaresService.AreaAcessoService.Listar();
            VeiculoPrivilegio = new List<AreaAcesso>();
            VeiculoPrivilegio.AddRange(lst7);

        }
        public void CarregaColecaoLayoutsCrachas(int _empresaID)
        {
            try
            {
                EmpresaLayoutCracha = new List<EmpresaLayoutCracha>();
                var service = new EmpresaLayoutCrachaService();
                var list1 = service.ListarLayoutCrachaPorEmpresaView(_empresaID);
                var list2 = Mapper.Map<List<EmpresaLayoutCracha>>(list1);
                EmpresaLayoutCracha = list2;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        #endregion





    }
}