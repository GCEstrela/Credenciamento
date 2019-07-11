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
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Funcoes;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels.Commands;
using IMOD.CredenciamentoDeskTop.ViewModels.Comportamento;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class ColaboradorViewModel : ViewModelBase, IComportamento, IAtualizarDados
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IColaboradorService _service = new ColaboradorService();
        private readonly IColaboradorEmpresaService _serviceColaboradorEmpresa = new ColaboradorEmpresaService();
        private readonly IColaboradorCredencialService _serviceColaboradorCredencial = new ColaboradorCredencialService();
        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();
        private ConfiguraSistema _configuraSistema;

        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;
        public bool IsEnablePreCadastro { get; set; } = false;
        public bool IsEnablePreCadastroCredenciamento { get; set; } = true;
        public string IsEnablePreCadastroColor { get; set; } = "#FFD0D0D0";
        #region  Propriedades
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }
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

        /// <summary>
        ///     True, empresa possui pendência de codigo 21
        /// </summary>
        public bool Pendencia21 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 22
        /// </summary>
        public bool Pendencia22 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 23
        /// </summary>
        public bool Pendencia23 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 24
        /// </summary>
        public bool Pendencia24 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 25
        /// </summary>
        public bool Pendencia25 { get; set; }

        public bool HabilitaCommandPincipal { get; set; } = true;
        /// <summary>
        ///     Resolução da webCam
        /// </summary>
        public int IsResolucao
        {
            get
            {
                return _configuraSistema.imagemResolucao;
            }
        }
        /// <summary>
        ///     Resolução da webCam
        /// </summary>
        public int IsTamanhoImagem
        {
            get
            {
                return _configuraSistema.imagemTamanho;
            }
        }
        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }


        public bool IsEnableTabItemGeral { get; set; }
        public bool IsEnableTabItemEmpresas { get; set; }
        public bool IsEnableTabItemCursos { get; set; }
        public bool IsEnableTabItemAnexos { get; set; }
        public bool IsEnableTabItemCredenciais { get; set; }

        /// <summary>
        ///     Seleciona o indice da tabcontrol desejada
        /// </summary>
        public short SelectedTabIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        public ColaboradorView Entity { get; set; }
        public ObservableCollection<ColaboradorView> EntityObserver { get; set; }

        /// <summary>
        ///     Estados
        /// </summary>
        public List<Estados> Estados { get; set; }

        public Estados Estado { get; set; }

        /// <summary>
        ///     Municipios
        /// </summary>
        public List<Municipio> Municipios { get; set; }

        /// <summary>
        ///     Dados de municipio armazenadas em memoria
        /// </summary>
        public List<Municipio> _municipios { get; set; }

        #endregion

        public ColaboradorViewModel()
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                ItensDePesquisaConfigura();
                ListarDadosAuxiliares();
                Comportamento = new ComportamentoBasico(false, true, true, false, false);
                EntityObserver = new ObservableCollection<ColaboradorView>();
                Comportamento.SalvarAdicao += OnSalvarAdicao;
                Comportamento.SalvarEdicao += OnSalvarEdicao;
                Comportamento.Remover += OnRemover;
                Comportamento.Cancelar += OnCancelar;
                PropertyChanged += OnEntityChanged;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
                Utils.TraceException(ex);
            }            
        }

        #region  Metodos

        /// <summary>
        ///     Habilita controles
        /// </summary>
        /// <param name="isEnableTabItem"></param>
        /// <param name="isEnableLstView"></param>
        private void HabilitaControle(bool isEnableTabItem, bool isEnableLstView)
        {
            HabilitaControleTabControls(isEnableLstView, isEnableTabItem, isEnableTabItem, isEnableTabItem, isEnableTabItem, isEnableTabItem); 
        }

        public void HabilitaControleTabControls(bool lstViewSuperior = true, bool isItemGeral = true, bool isItemEmpresas = false, bool isItemCursos = false, bool isItemAnexos = false, bool isItemCredenciais = false)
        {
            IsEnableLstView = lstViewSuperior;

            IsEnableTabItemGeral = isItemGeral;
            IsEnableTabItemEmpresas = isItemEmpresas;
            IsEnableTabItemCursos = isItemCursos;
            IsEnableTabItemAnexos = isItemAnexos;
            IsEnableTabItemCredenciais = isItemCredenciais;
            Comportamento.IsEnableCriar = lstViewSuperior;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Entity")

            {
                var enableControls = Entity != null;
                Comportamento.IsEnableEditar = enableControls; 
                HabilitaControleTabControls(true, enableControls, enableControls, enableControls, enableControls, enableControls);
            }
        
            if (e.PropertyName == "SelectedTabIndex")
            {
                //Habilita/Desabilita botoes principais...
                HabilitaCommandPincipal = SelectedTabIndex == 0; 
            }
                
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add (new KeyValuePair<int, string> (1, "CPF"));
            ListaPesquisa.Add (new KeyValuePair<int, string> (2, "Nome"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(3, "Empresa"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(4, "Colete"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(5, "Todos os Colaboradores"));
            PesquisarPor = ListaPesquisa[1]; //Pesquisa Default
        }

        private void PopularObserver(ICollection<Colaborador> list)
        {
            try
            {
                

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (!IsEnablePreCadastro)
                {
                    var list2 = Mapper.Map<List<ColaboradorView>>(list.ToList().OrderBy(c => c.Nome));
                    //var list2 = Mapper.Map<List<ColaboradorView>>(list.ToList().OrderBy(c => c.Nome).Take(100));
                    //var list2 = Mapper.Map<List<ColaboradorView>>(list.ToList());
                    EntityObserver = new ObservableCollection<ColaboradorView>();
                    list2.ForEach(n => { EntityObserver.Add(n); });
                    
                    //Havendo registros, selecione o primeiro
                    //if (EntityObserver.Any()) SelectListViewIndex = 0;
                }
                else
                {
                    var list2 = Mapper.Map<List<ColaboradorView>>(list.ToList().OrderBy(c => c.Nome));
                    EntityObserver = new ObservableCollection<ColaboradorView>();
                    list2.ForEach(n => { EntityObserver.Add(n); });
                    //Havendo registros, selecione o primeiro
                    //if (EntityObserver.Any()) SelectListViewIndex = 0;
                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
            }

            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
                Utils.TraceException (ex);
            }
        }
        public void carregaFoto(ImageSource minhaFoto)
        {
            

            //WriteableBitmap wbmp = new WriteableBitmap((BitmapSource)minhaFoto);
            //MemoryStream ms = new MemoryStream();
            ////wbmp.s(ms, wbmp.PixelWidth, wbmp.PixelHeight, 0, 100);
            ////return ms.ToArray();

            //var width = wbmp.PixelWidth;
            //var height = wbmp.PixelHeight;
            //var stride = width * ((wbmp.Format.BitsPerPixel + 7) / 8);

            //var bitmapData = new byte[height * stride];

            //wbmp.CopyPixels(bitmapData, stride, 0);


            //Entity.Foto = bitmapData.ToString();
        }
        

        private void Pesquisar()
        {
            try
            {
                var pesquisa = NomePesquisa;

                var num = PesquisarPor;

                //Todos
                if (num.Key == 5)
                {
                    
                    //var l1 = _service.Listar();
                    var l1 = _service.Listar(null, null, null, IsEnablePreCadastro);
                    PopularObserver(l1);

                }
                if (num.Key == 4)
                {

                    if (string.IsNullOrWhiteSpace(pesquisa)) return;
                    var l1 = _service.Listar(null, null, null, IsEnablePreCadastro);
                    var l2 = _serviceColaboradorCredencial.Listar(null, null, null, null, null, null, null, null, null, $"%{pesquisa}%");
                    var l3 = l2.Select(c => c.ColaboradorId).ToList<int>();
                    var l4 = l1.Where(c => l3.Contains(c.ColaboradorId)).ToList();

                    PopularObserver(l4);

                }
                if (num.Key == 3)
                {

                    if (string.IsNullOrWhiteSpace(pesquisa)) return;                    
                    var l1 = _service.Listar(null, null, null,IsEnablePreCadastro);
                    var l2 = _serviceColaboradorEmpresa.Listar(null, null, null, null, $"%{pesquisa}%", null);                    
                    var l3 = l2.Select(c => c.ColaboradorId ).ToList<int>();
                    var l4 =  l1.Where(c => l3.Contains(c.ColaboradorId)).ToList();
                                       
                    PopularObserver(l4);

                }

                //Por nome
                if (num.Key == 2)
                {
                    if (string.IsNullOrWhiteSpace (pesquisa)) return;
                    var l1 = _service.Listar (null, null, $"%{pesquisa}%", IsEnablePreCadastro);
                    
                    PopularObserver (l1);
                    
                }
                //Por cpf
                if (num.Key == 1)
                {
                    if (string.IsNullOrWhiteSpace (pesquisa)) return;
                    var l1 = _service.Listar (null, $"%{pesquisa.RetirarCaracteresEspeciais()}%", null,IsEnablePreCadastro);
                    PopularObserver (l1);
                    
                }

                IsEnableLstView = true; 
            }             
                catch (Exception ex)
            {
                WpfHelp.PopupBox(ex.Message, 1);
                Utils.TraceException (ex);
            }
        }
         
        public void BucarFoto(int colaborador)
        {
            try
            {
                if (Entity.Foto != null) return;
                var listaFoto = _service.BuscarPelaChave(colaborador);

                if (listaFoto!=null)
                    Entity.Foto = listaFoto.Foto;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }
        public void ListarDadosAuxiliares()
        {
            var lst3 = _auxiliaresService.EstadoService.Listar();
            Estados = Mapper.Map<List<Estados>> (lst3);
            Municipios = new List<Municipio>();
            _municipios = new List<Municipio>();

            _configuraSistema = ObterConfiguracao();
            //if (!_configuraSistema.Colete) //Se Cole não for automático false
            //{
                
            //}

        }

        /// <summary>
        ///     Atualizar dados de pendências
        /// </summary>
        public void AtualizarDadosPendencias()
        {
            if (Entity == null) return;

            var pendencia = _service.Pendencia.ListarPorColaborador (Entity.ColaboradorId).ToList();
            //Set valores
            SetPendenciaFalse();
            //Buscar pendências referente aos códigos: 21;22;23;24;25
            Pendencia21 = pendencia.Any (n => n.CodPendencia == 21 & n.Ativo);
            Pendencia22 = pendencia.Any (n => n.CodPendencia == 22 & n.Ativo);
            Pendencia23 = pendencia.Any (n => n.CodPendencia == 23 & n.Ativo);
            Pendencia24 = pendencia.Any (n => n.CodPendencia == 24 & n.Ativo);
            Pendencia25 = pendencia.Any (n => n.CodPendencia == 25 & n.Ativo);
        }

        private void SetPendenciaFalse()
        {
            Pendencia21 = false;
            Pendencia22 = false;
            Pendencia23 = false;
            Pendencia24 = false;
            Pendencia25 = false;
        }

        #endregion

        #region Regras de Negócio

        public bool ExisteCpf()
        {
            if (Entity == null) return false;
            var cpf = Entity.Cpf.RetirarCaracteresEspeciais();

            //Verificar dados antes de salvar uma criação
            if (_prepareCriarCommandAcionado)
                if (_service.ExisteCpf (cpf)) return true;
            //Verificar dados antes de salvar uma alteraçao
            if (!_prepareAlterarCommandAcionado) return false;
            var n1 = _service.BuscarPelaChave (Entity.ColaboradorId);
            if (n1 == null) return false;
            //Comparar o CNPJ antes e o depois
            //Verificar se há cnpj exisitente
            return string.Compare (n1.Cpf.RetirarCaracteresEspeciais(),
                cpf, StringComparison.Ordinal) != 0 && _service.ExisteCpf (cpf);
        }

        /// <summary>
        ///     Verificar se dados válidos
        ///     <para>True, inválido</para>
        /// </summary>
        /// <returns></returns>
        private bool EInValidoCpf()
        {
            if (Entity == null) return false;
            var cpf = Entity.Cpf.RetirarCaracteresEspeciais();
            if (!Utils.IsValidCpf (cpf)) return true;
            return false;
        }

        /// <summary>
        ///     Validar Regras de Negócio
        ///     True, regra de negócio violada
        /// </summary>
        /// <returns></returns>
        public bool Validar()
        {
            if (Entity == null) return true;
            if (Entity.Cpf != null & Entity.Cpf != "")
            {
                Entity.Validate();
                var hasErros = Entity.HasErrors;
                if (hasErros) return true;
                //Verificar valiade de cpf
                if (EInValidoCpf())
                {
                    Entity.SetMessageErro ("Cpf", "CPF inválido");
                    return true;
                }
                //Verificar existência de CPF
                if (ExisteCpf())
                {
                    Entity.SetMessageErro ("Cpf", "CPF já existe");
                    return true;
                }
                return Entity.HasErrors;
            }
            else
            {
                return false;
            }
            
        }

        #endregion

        #region Commands

        public ComportamentoBasico Comportamento { get; set; }

        /// <summary>
        ///     Listar Municipios
        /// </summary>
        /// <param name="uf"></param>
        public void ListarMunicipios(string uf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace (uf)) return;
                if (Estado == null) return;

                //Verificar se há municipios já carregados...
                var l1 = _municipios.Where (n => n.Uf == uf);
                Municipios.Clear();
                //Nao havendo municipios... obter do repositorio
                if (!l1.Any())
                {
                    var l2 = _auxiliaresService.MunicipioService.Listar (null, uf);
                    _municipios.AddRange (Mapper.Map<List<Municipio>> (l2));
                }

                var municipios = _municipios.Where (n => n.Uf == uf).ToList();
                Municipios = municipios;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }
        
        /// <summary>
        ///     Pré-Cadastro
        /// </summary>
        public ICommand PrepareIportarCommand => new CommandBase(PrepareImportar, true);
        private void PrepareImportar()
        {
            try
            {
                if (Entity == null) return;
                
                if (Validar()) return;

                var n1 = Mapper.Map<Colaborador>(Entity);
                
                n1.Pendente21 = true;
                n1.Pendente22 = true;
                n1.Pendente23 = true;
                n1.Pendente24 = true;
                n1.Pendente25 = true;
                #region Criar Pendências

                var pendencia = new Pendencia();
                pendencia.ColaboradorId = Entity.ColaboradorId;
                //--------------------------
                pendencia.CodPendencia = 22;
                pendencia.Impeditivo = true;
                Pendencia.CriarPendenciaSistema(pendencia);
                
                //--------------------------
                pendencia.CodPendencia = 23;
                pendencia.Impeditivo = true;
                Pendencia.CriarPendenciaSistema(pendencia);
                //--------------------------
                pendencia.CodPendencia = 24;
                pendencia.Impeditivo = true;
                Pendencia.CriarPendenciaSistema(pendencia);
                #endregion

                n1.Precadastro = false;
                _service.Alterar(n1);
                EntityObserver.RemoveAt(SelectListViewIndex);
                HabilitaControle(true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }
        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase (PrepareCriar, true);

        private void PrepareCriar()
        {
            Entity = new ColaboradorView();
            _prepareCriarCommandAcionado = true;
            Comportamento.PrepareCriar();
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            HabilitaControle (false, false);
            CloneObservable();
            SetPendenciaFalse();
        }

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

        #region Salva Dados

        private void PrepareSalvar()
        {
            if (Validar()) return;
            Comportamento.PrepareSalvar();
        }
         
        private List<ColaboradorView> _entityObserverCloned = new List<ColaboradorView>();
        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                WpfHelp.PopupBox ("Selecione um item da lista", 1);
                return;
            }

            Comportamento.PrepareAlterar();
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            HabilitaControle (false, false);

            CloneObservable();
        }
        /// <summary>
        /// Clone Observable
        /// </summary>
        private void CloneObservable()
        {
            _entityObserverCloned.Clear();
            EntityObserver.ToList().ForEach (n => { _entityObserverCloned.Add ((ColaboradorView) n.Clone()); });
        }

        private void PrepareRemover()
        {
            if (Entity == null) return;

            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = false;
            Comportamento.PrepareRemover();
            HabilitaControle (true, true);
        }

        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;
                if (Validar()) return;

                var n1 = Mapper.Map<Colaborador> (Entity);
                _service.Criar (n1);
                var n2 = Mapper.Map<ColaboradorView> (n1);
                EntityObserver.Insert (0, n2);
                HabilitaControle (true, true);
                SelectListViewIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;
                
                    if (Validar()) return;
                
                var n1 = Mapper.Map<Colaborador> (Entity);
                _service.Alterar (n1);
                HabilitaControle (true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            try
            {
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                if (Entity != null) Entity.ClearMessageErro();
                Entity = null;
                EntityObserver.Clear();
                EntityObserver = new ObservableCollection<ColaboradorView>(_entityObserverCloned);
                HabilitaControle((Entity != null), true);

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
                if (Entity == null) return;
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;

                var n1 = Mapper.Map<Colaborador> (Entity);

                //So remove de o colaborador for Precadastro=True
                if (n1.Precadastro)
                {
                    _service.Remover(n1);
                    //Retirar empresa da coleção
                    EntityObserver.Remove(Entity);

                }
                HabilitaControle (true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
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
        #endregion
    }
}