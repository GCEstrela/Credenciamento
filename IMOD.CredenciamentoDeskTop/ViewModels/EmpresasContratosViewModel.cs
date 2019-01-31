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
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class EmpresasContratosViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaContratosService _service = new EmpresaContratoService();
        private EmpresaView _empresaView;
        private EmpresaViewModel _viewModelParent;

        #region  Propriedades

        /// <summary>
        ///     Lista de municipios
        /// </summary>
        public List<Municipio> Municipios { get; private set; }

        public Estados Estado { get; set; }

        /// <summary>
        ///     Dados de municipio armazendas em memoria
        /// </summary>
        public List<Municipio> _municipios { get; set; }

        /// <summary>
        ///     Lista de estados
        /// </summary>
        public List<Estados> Estados { get; private set; }

        /// <summary>
        ///     Lista de sattus
        /// </summary>
        public List<Status> Status { get; private set; }

        /// <summary>
        ///     Lista de tipos de cobrança
        /// </summary>
        public List<TipoCobranca> TiposCobranca { get; private set; }

        /// <summary>
        ///     Lista de tipos de acessos
        /// </summary>
        public List<TipoAcesso> ListaTipoAcessos { get; private set; }
        public ObservableCollection<EmpresaContratoView> EntityObserver { get; set; }
        public EmpresaContratoView Entity { get; set; }

        EmpresaContratoView EntidadeTMP = new EmpresaContratoView();

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        public EmpresasContratosViewModel()
        {
            ListarDadosAuxiliares();
            ItensDePesquisaConfigura();
            Comportamento = new ComportamentoBasico(false, true, true, false, false);
            EntityObserver = new ObservableCollection<EmpresaContratoView>();
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
        /// <summary>
        ///     Carregar dados auxiliares em memória
        /// </summary>
        private void ListarDadosAuxiliares()
        {
            //Estados
            var lst1 = _auxiliaresService.EstadoService.Listar();
            Estados = new List<Estados>();
            Estados.AddRange(lst1);
            //Status
            var lst2 = _auxiliaresService.StatusService.Listar();
            Status = new List<Status>();
            Status.AddRange(lst2);
            //Tipos Cobrança
            var lst3 = _auxiliaresService.TipoCobrancaService.Listar();
            TiposCobranca = new List<TipoCobranca>();
            TiposCobranca.AddRange(lst3);
            //Tipo de Acesso
            var lst4 = _auxiliaresService.TiposAcessoService.Listar();
            ListaTipoAcessos = new List<TipoAcesso>();
            ListaTipoAcessos.AddRange(lst4);
        }

        /// <summary>
        ///     Listar Municipios
        /// </summary>
        /// <param name="uf"></param>
        public void ListarMunicipios(string uf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uf)) return;

                if (Municipios == null) Municipios = new List<Municipio>();                

                if (_municipios == null) _municipios = new List<Municipio>();              

                if (Estado == null) return;                 

                //Verificar se há municipios já carregados...
                var l1 = _municipios.Where(n => n.Uf == uf);
                Municipios.Clear();
                //Nao havendo municipios... obter do repositorio
                if (!l1.Any())
                {
                    var l2 = _auxiliaresService.MunicipioService.Listar(null, uf);
                    _municipios.AddRange(Mapper.Map<List<Municipio>>(l2));
                }

                var municipios = _municipios.Where(n => n.Uf == uf).ToList();
                Municipios = municipios;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void AtualizarDados(EmpresaView entity, EmpresaViewModel viewModelParent)
        {
          
            _empresaView = entity ?? throw new ArgumentNullException(nameof(entity));
            _viewModelParent = viewModelParent;
            //Obter dados
            var list1 = _service.Listar(entity.EmpresaId, null, null, null, null, null, null);
            var list2 = Mapper.Map<List<EmpresaContratoView>>(list1.OrderByDescending(n => n.EmpresaContratoId));
            EntityObserver = new ObservableCollection<EmpresaContratoView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "Nome"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Todos"));
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

                var n1 = Mapper.Map<EmpresaContrato>(Entity);
                n1.EmpresaId = _empresaView.EmpresaId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<EmpresaContratoView>(n1);
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

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            EntidadeTMP = Entity;
            Entity = new EmpresaContratoView();
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

                var n1 = Mapper.Map<EmpresaContrato>(Entity);
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
                if (Entity != null)
                {
                    Entity.ClearMessageErro();
                    Entity = EntidadeTMP;
                    
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
                if (Entity == null) return;

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                
                var n1 = Mapper.Map<EmpresaContrato>(Entity);
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
            if (Entity == null)
            {
                WpfHelp.PopupBox("Selecione um Item na Lista de Contratos", 1);
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
                var pesquisa = NomePesquisa;
                var num = PesquisarPor;


                //Por nome
                if (num.Key == 1)
                {
                    //Obet itens do observer
                    //var l1 = _service.Listar (Entity.EmpresaId,null, $"%{pesquisa}%");
                    if (string.IsNullOrWhiteSpace(pesquisa)) return;

                    var l1 = EntityObserver.Where(n => n.Descricao
                   .ToLower()
                   .Contains(pesquisa.ToLower())).ToList();
                    EntityObserver = new ObservableCollection<EmpresaContratoView>();
                    l1.ForEach(n => { EntityObserver.Add(n); });
           
                }

                if (num.Key == 2)
                {
                    //Obet itens do observer
                    var l1 = _service.Listar(Entity.EmpresaId);
                    PopularObserver(l1);
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PopularObserver(ICollection<EmpresaContrato> list)
        {
            try
            {
                var list2 = Mapper.Map<List<EmpresaContratoView>>(list);
                EntityObserver = new ObservableCollection<EmpresaContratoView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
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
            if (Entity == null) return true;
            //Verificar valiade de Contrato
            if (EInValidocontrato())
            {
                Entity.SetMessageErro("NumeroContrato", "Nº do Contrato inválido");
                return true;
            }

            //Verificar valiade de Descricao do Contrato
            if (EInValidoDescricao())
            {
                Entity.SetMessageErro("Descricao", "Descrição do Contrato inválido");
                return true;
            }
            //Verificar valiade de Data de Emissão
            if (EInValidoEmissao())
            {
                Entity.SetMessageErro("Emissao", "Emissao do Contrato inválido");
                return true;
            }
            //Verificar valiade de Data de Emissão
            if (EInValidoValidade())
            {
                Entity.SetMessageErro("Validade", "Validade do Contrato inválido");
                return true;
            }
            var hasErros = Entity.HasErrors;
             
            return hasErros;

        }

        #endregion
        #region Regras de Negócio
        /// <summary>
        ///     Verificar se dados válidos
        /// <para>True, inválido</para>
        /// </summary>
        /// <returns></returns>
        //private bool ExisteContrato()
        //{
        //    //if (Entity == null) return false;
        //    //var cpf = Entity.NumeroContrato.RetirarCaracteresEspeciais();

        //    ////Verificar dados antes de salvar uma criação
        //    //if (_prepareCriarCommandAcionado)
        //    //    if (_service.ExisteCpf(cpf)) return true;
        //    ////Verificar dados antes de salvar uma alteraçao
        //    //if (!_prepareAlterarCommandAcionado) return false;
        //    //var n1 = _service.BuscarPelaChave(Entity.ColaboradorId);
        //    //if (n1 == null) return false;
        //    ////Comparar o CNPJ antes e o depois
        //    ////Verificar se há cnpj exisitente
        //    //return string.Compare(n1.Cpf.RetirarCaracteresEspeciais(),
        //    //    cpf, StringComparison.Ordinal) != 0 && _service.ExisteCpf(cpf);
        //}
        private bool EInValidocontrato()
        {
            if (Entity == null) return false;
            var contrato = Entity.NumeroContrato.RetirarCaracteresEspeciais();
            if (contrato == "") return true;
            return false;
        }
        private bool EInValidoDescricao()
        {
            if (Entity == null) return false;
            var descricao = Entity.Descricao.RetirarCaracteresEspeciais();
            if (descricao == "") return true;
            return false;
        }
        private bool EInValidoEmissao()
        {
            if (Entity == null) return false;
            var emissao = Entity.Emissao.ToString();
            if (CheckDate(emissao)) return true;
            return false;
        }
        private bool EInValidoValidade()
        {
            if (Entity == null) return false;
            var validade = Entity.Validade.ToString();
            if (CheckDate(validade)) return true;
            return false;
        }
        protected bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return false;
            }
            catch
            {
                return true;
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
       
    }
}

