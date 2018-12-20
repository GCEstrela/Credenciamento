// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Xml;
using AutoMapper;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresasSignatariosViewModel : ViewModelBase
    {
        #region  Metodos

        #region Carregamento das Colecoes

        private void CarregaColecaoEmpresasSignatarios(int? empresaID = null, string nome = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";

                var list1 = _service.Listar(empresaID, nome, null, null, null, null,null);
                var list2 = Mapper.Map<List<ClasseEmpresasSignatarios.EmpresaSignatario>>(list1);

                var observer = new ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario>();
                list2.ForEach(n => { observer.Add(n); });

                Signatarios = observer;
                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Global.Log("Erro void CarregaColecaoEmpresasSignatarios ex: " + ex.Message);
            }
        }

        #endregion
 

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario> _Signatarios;

        private ClasseEmpresasSignatarios.EmpresaSignatario _SignatarioSelecionado;

        private ClasseEmpresasSignatarios.EmpresaSignatario _signatarioTemp = new ClasseEmpresasSignatarios.EmpresaSignatario();

        private readonly List<ClasseEmpresasSignatarios.EmpresaSignatario> _SignatarioTemp = new List<ClasseEmpresasSignatarios.EmpresaSignatario>();

        private readonly IEmpresaSignatarioService _service = new EmpresaSignatarioService();

        private PopupPesquisaContrato PopupPesquisaSignatarios;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        #endregion

        #region Contrutores

        public ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario> Signatarios
        {
            get { return _Signatarios; }

            set
            {
                if (_Signatarios != value)
                {
                    _Signatarios = value;
                    OnPropertyChanged();
                }
            }
        }

        public ClasseEmpresasSignatarios.EmpresaSignatario SignatarioSelecionado
        {
            get { return _SignatarioSelecionado; }
            set
            {
                _SignatarioSelecionado = value;
                OnPropertyChanged("SelectedItem");
                if (SignatarioSelecionado != null)
                {
                    OnSignatarioSelecionado();
                }
            }
        }

        private void OnSignatarioSelecionado()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public int EmpresaSelecionadaID
        {
            get { return _EmpresaSelecionadaID; }
            set
            {
                _EmpresaSelecionadaID = value;
                OnPropertyChanged();
                if (EmpresaSelecionadaID != null)
                {
                    //OnEmpresaSelecionada();
                }
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public bool HabilitaEdicao
        {
            get { return _HabilitaEdicao; }
            set
            {
                _HabilitaEdicao = value;
                OnPropertyChanged();
            }
        }

        public string Criterios
        {
            get { return _Criterios; }
            set
            {
                _Criterios = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Comandos dos Botoes

        public void OnAtualizaCommand(object empresaID)
        {
            EmpresaSelecionadaID = Convert.ToInt32(empresaID);
            var CarregaColecaoEmpresasSignatarios_thr = new Thread(() => CarregaColecaoEmpresasSignatarios(Convert.ToInt32(empresaID)));
            CarregaColecaoEmpresasSignatarios_thr.Start();
        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _signatarioTemp.Assinatura = arq.FormatoBase64;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        public void OnAbrirArquivoCommand()
        {
            try
            {
                var arquivoStr = SignatarioSelecionado.Assinatura;
                var nomeArquivo = "Ficha Cadastral";
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
                _signatarioTemp = SignatarioSelecionado.CriaCopia(SignatarioSelecionado);
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
                Signatarios[_selectedIndexTemp] = _signatarioTemp;
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

                var entity = SignatarioSelecionado;
                var entityConv = Mapper.Map<EmpresaSignatario>(entity);

                _service.Alterar(entityConv);

                var CarregaColecaoEmpresasSignatarios_thr = new Thread(() => CarregaColecaoEmpresasSignatarios(SignatarioSelecionado.EmpresaId, null));
                CarregaColecaoEmpresasSignatarios_thr.Start();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        //Global g = new Global();
        public void OnAdicionarCommand()
        {
            //if (g.iniciarFiljos = true)
            //    {
            //    return;
            //}
            try
            {
                foreach (var x in Signatarios)
                {
                    _SignatarioTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                Signatarios.Clear();

                _signatarioTemp = new ClasseEmpresasSignatarios.EmpresaSignatario();
                _signatarioTemp.EmpresaId = EmpresaSelecionadaID;
                Signatarios.Add(_signatarioTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;
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

                var entity = SignatarioSelecionado;
                var entityConv = Mapper.Map<EmpresaSignatario>(entity);

                _service.Criar(entityConv);

                var CarregaColecaoEmpresasSignatarios_thr = new Thread(() => CarregaColecaoEmpresasSignatarios(SignatarioSelecionado.EmpresaId));
                CarregaColecaoEmpresasSignatarios_thr.Start();
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
                Signatarios = null;
                Signatarios = new ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario>(_SignatarioTemp);
                SelectedIndex = _selectedIndexTemp;
                _SignatarioTemp.Clear();
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
                        var emp = _service.BuscarPelaChave(SignatarioSelecionado.EmpresaSignatarioID);
                        _service.Remover(emp);

                        Signatarios.Remove(SignatarioSelecionado);
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
                var popupPesquisaSegnatarios = new PopupPesquisaSignatarios();
                popupPesquisaSegnatarios.EfetuarProcura += On_EfetuarProcura;
                popupPesquisaSegnatarios.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            var _empresaID = EmpresaSelecionadaID;
            var _nome = ((PopupPesquisaSignatarios)sender).Criterio.Trim();
            CarregaColecaoEmpresasSignatarios(_empresaID, _nome);
            //CarregaColecaoContratos(_empresaID, _descricao, _numerocontrato);
            SelectedIndex = 0;
        }

        #endregion

        #region Data Access

        #endregion
    }
}