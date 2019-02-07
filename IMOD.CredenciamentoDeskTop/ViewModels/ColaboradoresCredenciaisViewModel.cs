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
using IMOD.Domain.EntitiesCustom;
using CredencialView = IMOD.CredenciamentoDeskTop.Views.Model.CredencialView;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class ColaboradoresCredenciaisViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IColaboradorEmpresaService _colaboradorEmpresaService = new ColaboradorEmpresaService();
        private readonly IColaboradorCredencialService _service = new ColaboradorCredencialService();
        //private ColaboradorCredencialimpresssao _colaboradorCredencialImpressao = new ColaboradorCredencialimpresssao();
        //private readonly IColaboradorCredencialImpressaoService _ImpressaoService = new ColaboradorCredencialImpressaoService();


        private ColaboradorView _colaboradorView;

        #region  Propriedades

        public List<CredencialStatus> CredencialStatus { get; set; }
        public List<CredencialMotivo> CredencialMotivo { get; set; }
        public List<FormatoCredencial> FormatoCredencial { get; set; }
        public List<TipoCredencial> TipoCredencial { get; set; }
        public List<EmpresaLayoutCracha> EmpresaLayoutCracha { get; set; }
        public List<TecnologiaCredencial> TecnologiasCredenciais { get; set; }
        public List<ColaboradorEmpresa> ColaboradoresEmpresas { get; set; }
        public ColaboradorEmpresa ColaboradorEmpresa { get; set; }
        public List<AreaAcesso> ColaboradorPrivilegio { get; set; }
        public ColaboradoresCredenciaisView Entity { get; set; }
        public ObservableCollection<ColaboradoresCredenciaisView> EntityObserver { get; set; }
        public ObservableCollection<CredencialView> Credencial { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; set; } = true;

        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }

        #endregion

        #region  Metodos

        private void ListarDadosAuxiliares()
        {
            var lst0 = _auxiliaresService.CredencialStatusService.Listar();
            CredencialStatus = new List<CredencialStatus>();
            CredencialStatus.AddRange (lst0);

            var lst2 = _auxiliaresService.FormatoCredencialService.Listar();
            FormatoCredencial = new List<FormatoCredencial>();
            FormatoCredencial.AddRange (lst2);

            var lst3 = _auxiliaresService.TipoCredencialService.Listar();
            TipoCredencial = new List<TipoCredencial>();
            TipoCredencial.AddRange (lst3);

            var lst5 = _auxiliaresService.TecnologiaCredencialService.Listar();
            TecnologiasCredenciais = new List<TecnologiaCredencial>();
            TecnologiasCredenciais.AddRange (lst5);

            var lst6 = _colaboradorEmpresaService.Listar (null, true).OrderByDescending (n => n.ColaboradorEmpresaId).Where (n => n.Ativo.Equals (true)).ToList();
            ColaboradoresEmpresas = new List<ColaboradorEmpresa>();
            ColaboradoresEmpresas.AddRange (lst6);

            var lst7 = _auxiliaresService.AreaAcessoService.Listar();
            ColaboradorPrivilegio = new List<AreaAcesso>();
            ColaboradorPrivilegio.AddRange (lst7);
        }

        /// <summary>
        /// </summary>
        /// <param name="tipoCredencial"></param>
        public void ListarMotivos(string tipoCredencial)
        {
            var lst1 = _auxiliaresService.CredencialMotivoService.Listar (null, null, tipoCredencial);
            CredencialMotivo = new List<CredencialMotivo>();
            CredencialMotivo.AddRange (lst1);
        }

        /// <summary>
        /// </summary>
        /// <param name="empresaId"></param>
        public void ListarCracha(int empresaId)
        {
            try
            {
                EmpresaLayoutCracha = new List<EmpresaLayoutCracha>();
                var service = new EmpresaLayoutCrachaService();
                var list1 = service.ListarLayoutCrachaPorEmpresaView (empresaId);
                var list2 = Mapper.Map<List<EmpresaLayoutCracha>> (list1);
                EmpresaLayoutCracha = list2;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        //SCManager _sc = new SCManager();
        //TODO:Retirar a função de SCManager e colocar em infra esrutura

        #region Inicializacao

        public ColaboradoresCredenciaisViewModel()
        {
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico (false, true, true, false, false);
            EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
            PropertyChanged += OnEntityChanged;
        }

        #region  Metodos

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Entity") //habilitar botão alterar todas as vezes em que houver entidade diferente de null
                Comportamento.IsEnableEditar = true;
        }

        public void AtualizarVinculoColaboradorEmpresa(ColaboradorView entity)
        {
            if (entity == null) throw new ArgumentNullException (nameof (entity));

            var lista1 = _colaboradorEmpresaService.Listar (entity.ColaboradorId, true);
            var lista2 = Mapper.Map<List<ColaboradorEmpresa>> (lista1.OrderByDescending (n => n.ColaboradorEmpresaId).Where (n => n.Ativo.Equals (true)).ToList());

            ColaboradoresEmpresas.Clear();
            lista2.ForEach (n => { ColaboradoresEmpresas.Add (n); });
        }

        public void AtualizarDados(ColaboradorView entity)
        {
            if (entity == null) throw new ArgumentNullException (nameof (entity));
            _colaboradorView = entity;
            //Obter dados
            var list1 = _service.ListarView (null, null, null, null, entity.ColaboradorId).ToList();
            var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>> (list1.OrderByDescending (n => n.ColaboradorCredencialId));
            EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();

            list2.ForEach (n => { EntityObserver.Add (n); });
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add (new KeyValuePair<int, string> (1, "Nome"));
            ListaPesquisa.Add (new KeyValuePair<int, string> (2, "CPF"));
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

                var n1 = Mapper.Map<ColaboradorCredencial> (Entity);

                n1.CredencialMotivoId = Entity.CredencialMotivoId;
                n1.CredencialStatusId = Entity.CredencialStatusId;
                n1.FormatoCredencialId = Entity.FormatoCredencialId;
                n1.LayoutCrachaId = Entity.LayoutCrachaId;
                n1.TecnologiaCredencialId = Entity.TecnologiaCredencialId;
                n1.TipoCredencialId = Entity.TipoCredencialId;

                _service.Criar (n1, _colaboradorView.ColaboradorId);
                IsEnableLstView = true;
                SelectListViewIndex = 0;

                var list1 = _service.ListarView (null, null, null, null, _colaboradorView.ColaboradorId).ToList();
                var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>> (list1.OrderByDescending (n => n.ColaboradorCredencialId));
                EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
                list2.ForEach (n => { EntityObserver.Add (n); });
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            Entity = new ColaboradoresCredenciaisView
            {
                Ativa = true
            };
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

                var n1 = Mapper.Map<ColaboradorCredencial> (Entity);
                _service.Alterar (n1);
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
                Entity = null;
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

                var n1 = Mapper.Map<ColaboradorCredencial> (Entity);
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
            if (Entity == null)
            {
                WpfHelp.PopupBox ("Selecione um item da lista", 1);
                return;
            }
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
        }

        private void PrepareSalvar()
        {
            if (Validar()) return;
            Comportamento.PrepareSalvar();
        }

        /// <summary>
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
        }

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        public bool Validar()
        {
            if (Entity == null) return true;
            return false;
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
        public ICommand PrepareRemoverCommand => new CommandBase (PrepareRemover, true);

        /// <summary>
        ///     Pesquisar
        /// </summary>
        public ICommand PesquisarCommand => new CommandBase (Pesquisar, true);

        #endregion

        /// <summary>
        ///     Imprimir Credencial
        /// </summary>
        public void OnImprimirCredencial()
        {
            try
            {
                //if (Entity.Validade == null || !Entity.Ativa || Entity.LayoutCrachaId == 0)
                //{
                //    WpfHelp.PopupBox("Não foi possível imprimir esta credencial!", 3);
                //    return;
                //}
                //var list1 = _service.ListarCredencialView(Entity.ColaboradorCredencialId);
                //var list2 = Mapper.Map<List<CredencialView>>(list1);
                //var observer = new ObservableCollection<CredencialView>();
                //list2.ForEach(n =>
                //{
                //    observer.Add(n);
                //});

                //Credencial = observer;

                //var layoutCracha = _auxiliaresService.LayoutCrachaService.BuscarPelaChave(Entity.LayoutCrachaId);

                //string _ArquivoRPT = Path.GetRandomFileName();
                //_ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;
                //_ArquivoRPT = Path.ChangeExtension(_ArquivoRPT, ".rpt");
                //byte[] arrayFile = Convert.FromBase64String(layoutCracha.LayoutRpt);
                //File.WriteAllBytes(_ArquivoRPT, arrayFile);
                //ReportDocument reportDocument = new ReportDocument();
                //reportDocument.Load(_ArquivoRPT);

                //reportDocument.SetDataSource(Credencial);

                //PopupCredencial _popupCredencial = new PopupCredencial(reportDocument);
                //_popupCredencial.ShowDialog();

                //bool _result = _popupCredencial.Result;

                //if (_result)
                //{
                //    _colaboradorCredencialImpressao.ColaboradorCredencialId = Entity.ColaboradorCredencialId;
                //    _colaboradorCredencialImpressao.DataImpressao = DateTime.Now;
                //    if (Entity.IsencaoCobranca)
                //    {
                //        _colaboradorCredencialImpressao.Cobrar = false;
                //    }
                //    else
                //    {
                //        _colaboradorCredencialImpressao.Cobrar = true;
                //    }

                //    _ImpressaoService.Criar(_colaboradorCredencialImpressao);
                //    WpfHelp.PopupBox("Impressão Efetuada com Sucesso!", 1);
                //    Entity.Impressa = true;

                //    //Criar cardHolder
                //    string cardholderGuid = _sc.CardHolder(Entity.ColaboradorNome.Trim(), Entity.Cpf.Trim(), Entity.Cnpj.Trim(),
                //        Entity.EmpresaNome.Trim(), Entity.Matricula.Trim(), Entity.Cargo.Trim(),
                //        Entity.Fc.ToString().Trim(), Entity.NumeroCredencial.Trim(),
                //        Entity.FormatoCredencialDescricao.Trim(), Entity.Validade.ToString(),
                //        layoutCracha.LayoutCrachaGuid, Entity.ColaboradorFoto.ConverterBase64StringToBitmap());

                //    string credentialGuid = _sc.Credencial(layoutCracha.LayoutCrachaGuid, Entity.Fc.ToString().Trim(),
                //        Entity.NumeroCredencial.Trim(), Entity.FormatoCredencialDescricao.Trim(), cardholderGuid);

                //    if (Entity == null)
                //    {
                //        return;
                //    }
                //    var n1 = Mapper.Map<ColaboradorCredencial>(Entity);
                //    n1.CardHolderGuid = cardholderGuid.ToString();
                //    n1.CredencialGuid = credentialGuid.ToString();
                //    _service.Alterar(n1);

                //}
                //File.Delete(_ArquivoRPT);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion
    }
}