using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
//using System.Windows.Forms;

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradoresAnexosViewModel : ViewModelBase
    {

        private IColaboradorAnexoService _colaboradorAnexoService;

        #region Inicializacao
        public ColaboradoresAnexosViewModel()
        {
            _colaboradorAnexoService = new ColaboradorAnexoService();
            CarregaUI();
        }
        private void CarregaUI()
        {
            CarregaColecaoColaboradoresAnexos();
        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo> _ColaboradoresAnexos;

        private ClasseColaboradoresAnexos.ColaboradorAnexo _ColaboradorAnexoSelecionado;

        private ClasseColaboradoresAnexos.ColaboradorAnexo _ColaboradorAnexoTemp = new ClasseColaboradoresAnexos.ColaboradorAnexo();

        private List<ClasseColaboradoresAnexos.ColaboradorAnexo> _ColaboradoresAnexosTemp = new List<ClasseColaboradoresAnexos.ColaboradorAnexo>();

        PopupPesquisaColaboradorAnexo popupPesquisaColaboradorAnexo;

        private int _selectedIndex;

        private int _ColaboradorAnexoSelecionadaID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo> ColaboradoresAnexos
        {
            get
            {
                return _ColaboradoresAnexos;
            }

            set
            {
                if (_ColaboradoresAnexos != value)
                {
                    _ColaboradoresAnexos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseColaboradoresAnexos.ColaboradorAnexo ColaboradorAnexoSelecionado
        {
            get
            {
                return _ColaboradorAnexoSelecionado;
            }
            set
            {
                _ColaboradorAnexoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (ColaboradorAnexoSelecionado != null)
                {
                    //OnEmpresaSelecionada();
                }

            }
        }

        public int ColaboradorAnexoSelecionadaID
        {
            get
            {
                return _ColaboradorAnexoSelecionadaID;
            }
            set
            {
                _ColaboradorAnexoSelecionadaID = value;
                base.OnPropertyChanged();
                if (ColaboradorAnexoSelecionadaID != null)
                {
                    //OnEmpresaSelecionada();
                }

            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public bool HabilitaEdicao
        {
            get
            {
                return _HabilitaEdicao;
            }
            set
            {
                _HabilitaEdicao = value;
                base.OnPropertyChanged();
            }
        }

        public string Criterios
        {
            get
            {
                return _Criterios;
            }
            set
            {
                _Criterios = value;
                base.OnPropertyChanged();
            }
        }
        #endregion

        #region Comandos dos Botoes
        public void OnAtualizaCommand(object _colaboradorAnexoID)
        {

            ColaboradorAnexoSelecionadaID = Convert.ToInt32(_colaboradorAnexoID);
            Thread CarregaColecaoColaboradoresAnexos_thr = new Thread(() => CarregaColecaoColaboradoresAnexos(Convert.ToInt32(_colaboradorAnexoID)));
            CarregaColecaoColaboradoresAnexos_thr.Start();

        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _ColaboradorAnexoTemp.NomeArquivo = arq.Nome;
                _ColaboradorAnexoTemp.Arquivo = arq.FormatoBase64;
                if (ColaboradoresAnexos != null)
                    ColaboradoresAnexos[0].NomeArquivo = arq.Nome;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnAbrirArquivoCommand()
        {
            try
            {
                var arquivoStr = ColaboradorAnexoSelecionado.Arquivo;
                var nomeArquivo = ColaboradorAnexoSelecionado.NomeArquivo;
                var arrBytes = Convert.FromBase64String(arquivoStr);
                WpfHelp.DownloadArquivoDialog(nomeArquivo, arrBytes);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnEditarCommand()
        {
            try
            {
                //BuscaBadges();
                _ColaboradorAnexoTemp = ColaboradorAnexoSelecionado.CriaCopia(ColaboradorAnexoSelecionado);
                _selectedIndexTemp = SelectedIndex;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnCancelarEdicaoCommand()
        {
            try
            {
                ColaboradoresAnexos[_selectedIndexTemp] = _ColaboradorAnexoTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;


                var entity = Mapper.Map<ColaboradorAnexo>(ColaboradorAnexoSelecionado);
                var repositorio = new ColaboradorAnexoService();
                repositorio.Alterar(entity);

                var id = entity.ColaboradorId;

                _ColaboradoresAnexosTemp = null;

                _ColaboradoresAnexosTemp.Clear();
                _ColaboradorAnexoTemp = null;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        public void OnSalvarAdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;
                XmlSerializer serializer = new XmlSerializer(typeof(ClasseColaboradoresAnexos));

                ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo> _ColaboradoresAnexosPro = new ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo>();
                ClasseColaboradoresAnexos _ClasseColaboradoresAnexosPro = new ClasseColaboradoresAnexos();
                _ColaboradoresAnexosPro.Add(ColaboradorAnexoSelecionado);
                _ClasseColaboradoresAnexosPro.ColaboradoresAnexos = _ColaboradoresAnexosPro;


                var entity = Mapper.Map<ColaboradorAnexo>(ColaboradorAnexoSelecionado);
                var repositorio = new ColaboradorAnexoService();
                repositorio.Criar(entity);
                var id = entity.ColaboradorId;


                Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoColaboradoresAnexos(id));
                CarregaColecaoSeguros_thr.Start();
                _ColaboradoresAnexosTemp.Add(ColaboradorAnexoSelecionado);
                ColaboradoresAnexos = null;
                ColaboradoresAnexos = new ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo>(_ColaboradoresAnexosTemp);
                SelectedIndex = _selectedIndexTemp;
                _ColaboradoresAnexosTemp.Clear();


                _ColaboradoresAnexosPro = null;

                _ColaboradoresAnexosPro.Clear();
                _ColaboradorAnexoTemp = null;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {

                foreach (var x in ColaboradoresAnexos)
                {
                    _ColaboradoresAnexosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                ColaboradoresAnexos.Clear();

                _ColaboradorAnexoTemp = new ClasseColaboradoresAnexos.ColaboradorAnexo();
                _ColaboradorAnexoTemp.ColaboradorID = ColaboradorAnexoSelecionadaID;
                ColaboradoresAnexos.Add(_ColaboradorAnexoTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                ColaboradoresAnexos = null;
                ColaboradoresAnexos = new ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo>(_ColaboradoresAnexosTemp);
                SelectedIndex = _selectedIndexTemp;
                _ColaboradoresAnexosTemp.Clear();
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void OnExcluirCommand()
        {
            try
            {

                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        var entity = Mapper.Map<ColaboradorAnexo>(ColaboradorAnexoSelecionado);
                        var repositorio = new ColaboradorAnexoService();
                        repositorio.Remover(entity);

                        ColaboradoresAnexos.Remove(ColaboradorAnexoSelecionado);
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }
        public void OnPesquisarCommand()
        {
            try
            {
                popupPesquisaColaboradorAnexo = new PopupPesquisaColaboradorAnexo();
                popupPesquisaColaboradorAnexo.EfetuarProcura += On_EfetuarProcura;
                popupPesquisaColaboradorAnexo.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaColaboradorAnexo.Criterio.Split((char)(20));
            int _colaboradorID = ColaboradorAnexoSelecionadaID;
            string _DescricaoAnexo = ((string[])vetor)[0];
            CarregaColecaoColaboradoresAnexos(_colaboradorID, _DescricaoAnexo);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        public void CarregaColecaoColaboradoresAnexos(int _colaboradorID = 0, string _nome = "")
        {
            try
            {
                var service = new ColaboradorAnexoService();
                if (!string.IsNullOrWhiteSpace(_nome)) _nome = $"%{_nome}%";
                var list1 = service.Listar(_colaboradorID, _nome);

                var list2 = Mapper.Map<List<ClasseColaboradoresAnexos.ColaboradorAnexo>>(list1);
                var observer = new ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                ColaboradoresAnexos = observer;

                //Hotfix auto-selecionar registro no topo da ListView
                var topList = observer.FirstOrDefault();
                ColaboradorAnexoSelecionado = topList;

                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

    }
}
