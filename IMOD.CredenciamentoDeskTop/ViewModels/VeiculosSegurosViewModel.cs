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

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    class VeiculosSegurosViewModel : ViewModelBase, IComportamento
    {

        #region  Propriedades

        private readonly IVeiculoSeguroService _service = new VeiculoSeguroService();
        private VeiculoView _veiculoView;
        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;


        public VeiculoSeguroView Entity { get; set; }
        public VeiculoSeguroView EntityTmp { get; set; }

        public ObservableCollection<VeiculoSeguroView> EntityObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        #region Inicializacao

        public void AtualizarDados(VeiculoView entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _veiculoView = entity;
            //Obter dados

            var list1 = _service.Listar(entity.EquipamentoVeiculoId, null, null);
            var list2 = Mapper.Map<List<VeiculoSeguroView>>(list1.OrderByDescending(n => n.VeiculoSeguroId));
            var observer = new ObservableCollection<VeiculoSeguro>();
            //var ppppp = new ObservableCollection<VeiculoSeguroView>();
            EntityObserver = new ObservableCollection<VeiculoSeguroView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
        }

        public VeiculosSegurosViewModel()
        {
            ItensDePesquisaConfigura();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #endregion

        #region Metódos


        /// <summary>
        ///     Relação dos itens de pesquisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "Seguro"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Apólice"));
            PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
        }


        private void PrepareRemover()
        {
            if (Entity == null) return;

            IsEnableLstView = true;
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = false;
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

                var n1 = Mapper.Map<VeiculoSeguro>(Entity);
                n1.VeiculoId = _veiculoView.EquipamentoVeiculoId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<VeiculoSeguroView>(n1);
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
            EntityTmp = Entity;
            Entity = new VeiculoSeguroView();
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = true;
            Comportamento.PrepareCriar();
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
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
                var n1 = Mapper.Map<VeiculoSeguro>(Entity);
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
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                Entity = EntityTmp;

                if (Entity != null)
                {
                    //if (Entity.VeiculoSeguroId == 0)
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

                var n1 = Mapper.Map<VeiculoSeguro>(Entity);
                _service.Remover(n1);
                //Retirar empresa da coleção
                EntityObserver.Remove(Entity);
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        private void PrepareSalvar()
        {
            if (!ErroValidacao)
                Comportamento.PrepareSalvar();
        }
        private void PrepareAlterar()
        {
            if (Entity == null) return;

            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
        }

        /// <summary>
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                if (_veiculoView == null)
                {
                    return;
                }

                var pesquisa = NomePesquisa;

                var num = PesquisarPor;

                //Por Seguro
                if (num.Key == 1)
                {
                    var l1 = _service.Listar(_veiculoView.EquipamentoVeiculoId, $"%{pesquisa}%", null);
                    PopularObserver(l1);
                }
                //Por Seguro
                if (num.Key == 2)
                {
                    var l1 = _service.Listar(_veiculoView.EquipamentoVeiculoId, null, $"%{pesquisa}%");
                    PopularObserver(l1);
                }

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PopularObserver(ICollection<VeiculoSeguro> list)
        {
            try
            {
                var list2 = Mapper.Map<List<VeiculoSeguroView>>(list.OrderBy(n => n.NomeArquivo));
                EntityObserver = new ObservableCollection<VeiculoSeguroView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
                //Empresas = observer;
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

        /// <summary>
        /// Erro de validação
        /// True, Erro de validação
        /// </summary>
        public bool ErroValidacao { get { return Validar(); } }
    }
    

}
