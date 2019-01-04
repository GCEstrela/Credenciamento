// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using AutoMapper;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels.Commands;
using iModSCCredenciamento.ViewModels.Comportamento;
using iModSCCredenciamento.Views.Model;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresasAnexosViewModel : ViewModelBase, IComportamento
    {
        private readonly IEmpresaAnexoService _service = new EmpresaAnexoService();
        private EmpresaView _empresaView;

        #region  Propriedades

        public EmpresaAnexoView Entity { get; set; }
        public ObservableCollection<EmpresaAnexoView> EntityObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        public EmpresasAnexosViewModel()
        {
            Comportamento = new ComportamentoBasico (true, true, true, true, true);
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

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
                var n1 = Mapper.Map<EmpresaAnexo> (Entity);
                n1.EmpresaId = _empresaView.EmpresaId;
                _service.Criar (n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<EmpresaAnexoView> (n1);
                EntityObserver.Insert (0, n2);
                IsEnableLstView = true;
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
            Entity = new EmpresaAnexoView();
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
                var n1 = Mapper.Map<EmpresaAnexo> (Entity);
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

                var n1 = Mapper.Map<EmpresaAnexo> (Entity);
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
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
        }

        public void AtualizarDadosAnexo(EmpresaView entity)
        {
            if (entity == null) throw new ArgumentNullException (nameof (entity));
            _empresaView = entity;
            //Obter dados
            var list1 = _service.Listar (entity.EmpresaId);
            var list2 = Mapper.Map<List<EmpresaAnexoView>> (list1);
            EntityObserver = new ObservableCollection<EmpresaAnexoView>();
            list2.ForEach (n => { EntityObserver.Add (n); });
        }

        #endregion

        //{
        //private string CriaXmlImagem(int empresaAnexoID)

        //#region Metodos privados

        //#endregion

        //#region Data Access
        //#endregion
        //}
        //    }
        //        Utils.TraceException(ex);
        //    {
        //    catch (Exception ex)

        //    }
        //        AnexoSelecionado = topList;

        //        var topList = observer.FirstOrDefault();

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
        public ICommand PrepareSalvarCommand => new CommandBase (Comportamento.PrepareSalvar, true);

        /// <summary>
        ///     Remover
        /// </summary>
        public ICommand PrepareRemoverCommand => new CommandBase (PrepareRemover, true);

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        public void Validar()
        {
            throw new NotImplementedException();
        }

        #endregion

        //        var list1 = _service.Listar(empresaID, _descricao, null, null, null, null);
        //        if (!string.IsNullOrWhiteSpace(_descricao)) _descricao = $"%{_descricao}%";
        //    {
        //    try
        //{
        //private void CarregaColecaoAnexos(int? empresaID = null, string _descricao = null)

        //#region Carregamento das Colecoes

        //#region Contrutores
        //public ObservableCollection<EmpresaAnexoView> Anexos
        //{
        //    get
        //    {
        //        return _Anexos;
        //    }

        //    set
        //    {
        //        if (_Anexos != value)
        //        {
        //            _Anexos = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        //public EmpresaAnexoView AnexoSelecionado
        //{
        //    get
        //    {
        //        return _AnexoSelecionado;
        //    }
        //    set
        //    {
        //        _AnexoSelecionado = value;
        //        base.OnPropertyChanged("SelectedItem");
        //        if (AnexoSelecionado != null)
        //        {
        //            //OnEmpresaSelecionada();
        //        }

        //    }
        //}

        //public int EmpresaSelecionadaID
        //{
        //    get
        //    {
        //        return _EmpresaSelecionadaID;
        //    }
        //    set
        //    {
        //        _EmpresaSelecionadaID = value;
        //        base.OnPropertyChanged();
        //        if (EmpresaSelecionadaID != null)
        //        {
        //            //OnEmpresaSelecionada();
        //        }

        //    }
        //}

        //public int SelectedIndex
        //{
        //    get
        //    {
        //        return _selectedIndex;
        //    }
        //    set
        //    {
        //        _selectedIndex = value;
        //        OnPropertyChanged("SelectedIndex");
        //    }
        //}

        //public bool HabilitaEdicao
        //{
        //    get
        //    {
        //        return _HabilitaEdicao;
        //    }
        //    set
        //    {
        //        _HabilitaEdicao = value;
        //        base.OnPropertyChanged();
        //    }
        //}

        //public string Criterios
        //{
        //    get
        //    {
        //        return _Criterios;
        //    }
        //    set
        //    {
        //        _Criterios = value;
        //        base.OnPropertyChanged();
        //    }
        //}
        //#endregion

        #region Comandos dos Botoes

        //public void OnAtualizaCommand(object empresaID)
        //{
        //    EmpresaSelecionadaID = Convert.ToInt32(empresaID);
        //    Thread CarregaColecaoAnexos_thr = new Thread(() => CarregaColecaoAnexos(Convert.ToInt32(empresaID)));
        //    CarregaColecaoAnexos_thr.Start();
        //}

        //public void OnBuscarArquivoCommand()
        //{
        //    try
        //    {
        //        var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
        //        var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
        //        if (arq == null) return;
        //        _anexoTemp.NomeAnexo = arq.Nome;
        //        _anexoTemp.Anexo = arq.FormatoBase64;
        //        if (Anexos != null)
        //            Anexos[0].NomeAnexo = arq.Nome;

        //    }
        //    catch (Exception ex)
        //    {
        //        WpfHelp.Mbox(ex.Message);
        //        Utils.TraceException(ex);
        //    }

        //}
        //public void OnAbrirArquivoCommand()
        //{
        //    try
        //    {
        //        var arquivoStr = AnexoSelecionado.Anexo;
        //        var nomeArquivo = AnexoSelecionado.NomeAnexo;
        //        var arrBytes = Convert.FromBase64String(arquivoStr);
        //        WpfHelp.DownloadArquivoDialog(nomeArquivo, arrBytes);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //public void OnEditarCommand()
        //{
        //    try
        //    {
        //        //BuscaBadges();
        //       //_anexoTemp = AnexoSelecionado.CriaCopia(AnexoSelecionado);
        //        _selectedIndexTemp = SelectedIndex;
        //        HabilitaEdicao = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //public void OnCancelarEdicaoCommand()
        //{
        //    try
        //    {
        //        Anexos[_selectedIndexTemp] = _anexoTemp;
        //        SelectedIndex = _selectedIndexTemp;
        //        HabilitaEdicao = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //public void OnSalvarEdicaoCommand()
        //{
        //    try
        //    {
        //        HabilitaEdicao = false;

        //        var entity = AnexoSelecionado;
        //        var entityConv = Mapper.Map<EmpresaAnexo>(entity);

        //        _service.Alterar(entityConv);

        //        Thread CarregaColecaoAnexosSignatarios_thr = new Thread(() => CarregaColecaoAnexos(AnexoSelecionado.EmpresaId));
        //        CarregaColecaoAnexosSignatarios_thr.Start();

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //public void OnAdicionarCommand()
        //{
        //    try
        //    {
        //        foreach (var x in Anexos)
        //        {
        //            _AnexosTemp.Add(x);
        //        }

        //        _selectedIndexTemp = SelectedIndex;
        //        Anexos.Clear();

        //        _anexoTemp = new EmpresaAnexoView();
        //        _anexoTemp.EmpresaId = EmpresaSelecionadaID;
        //        Anexos.Add(_anexoTemp);
        //        SelectedIndex = 0;
        //        HabilitaEdicao = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }

        //}
        //public void OnSalvarAdicaoCommand()
        //{
        //    try
        //    {
        //        HabilitaEdicao = false;

        //        ObservableCollection<EmpresaAnexoView> _EmpresasAnexosPro = new ObservableCollection<EmpresaAnexoView>();
        //        EmpresaAnexoView _ClasseEmpresasAnexosTemp = new EmpresaAnexoView();

        //        _EmpresasAnexosPro.Add(AnexoSelecionado);
        //        //_ClasseEmpresasAnexosTemp.EmpresasAnexos = _EmpresasAnexosPro;

        //        var entity = AnexoSelecionado;
        //        var entityConv = Mapper.Map<EmpresaAnexo>(entity);

        //        _service.Criar(entityConv);

        //        Thread CarregaColecaoAnexosSignatarios_thr = new Thread(() => CarregaColecaoAnexos(AnexoSelecionado.EmpresaId));
        //        CarregaColecaoAnexosSignatarios_thr.Start();

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //        throw;
        //    }
        //}
        //public void OnCancelarAdicaoCommand()
        //{
        //    try
        //    {
        //        Anexos = null;
        //        Anexos = new ObservableCollection<EmpresaAnexoView>(_AnexosTemp);
        //        SelectedIndex = _selectedIndexTemp;
        //        _AnexosTemp.Clear();
        //        HabilitaEdicao = false;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //public void OnExcluirCommand()
        //{
        //    try
        //    {
        //        if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
        //        {
        //            if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
        //            {
        //                var emp = _service.BuscarPelaChave(AnexoSelecionado.EmpresaAnexoId);
        //                _service.Remover(emp);
        //                Anexos.Remove(AnexoSelecionado);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}
        //public void OnPesquisarCommand()
        //{
        //    try
        //    {
        //        popupPesquisaEmpresasAnexos = new PopupPesquisaEmpresasAnexos();
        //        popupPesquisaEmpresasAnexos.EfetuarProcura += On_EfetuarProcura;
        //        popupPesquisaEmpresasAnexos.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //public void On_EfetuarProcura(object sender, EventArgs e)
        //{
        //    object vetor = popupPesquisaEmpresasAnexos.Criterio.Split((char)(20));
        //    int _empresaID = EmpresaSelecionadaID;
        //    string _descricao = ((string[])vetor)[0];
        //    CarregaColecaoAnexos(_empresaID, _descricao);
        //    SelectedIndex = 0;
        //}

        #endregion

        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseArquivosImagens = _xmlDocument.CreateElement("ClasseArquivosImagens");
        //        _xmlDocument.AppendChild(_ClasseArquivosImagens);

        //        XmlNode _ArquivosImagens = _xmlDocument.CreateElement("ArquivosImagens");
        //        _ClasseArquivosImagens.AppendChild(_ArquivosImagens);

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        SqlCommand SQCMDXML = new SqlCommand("Select * From EmpresasAnexos Where EmpresaAnexoID = " + empresaAnexoID + "", _Con);
        //        SqlDataReader SQDR_XML;
        //        SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
        //        while (SQDR_XML.Read())
        //        {
        //            XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
        //            _ArquivosImagens.AppendChild(_ArquivoImagem);

        //            //XmlNode _ArquivoImagemID = _xmlDocument.CreateElement("ArquivoImagemID");
        //            //_ArquivoImagemID.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["EmpresaSeguroID"].ToString())));
        //            //_ArquivoImagem.AppendChild(_ArquivoImagemID);

        //            XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
        //            _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Anexo"].ToString())));
        //            _ArquivoImagem.AppendChild(_Arquivo);

        //        }
        //        SQDR_XML.Close();

        //        _Con.Close();
        //        return _xmlDocument.InnerXml;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void CriaXmlImagem ex: " + ex);
        //        return null;
        //    }
        //}
        //#endregion
    }
}