using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using iModSCCredenciamento.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradoresEmpresasViewModel : ViewModelBase
    {
        //Global g = new Global();
        #region Inicializacao
        public ColaboradoresEmpresasViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();

        }

        private void CarregaUI()
        {
            CarregaColecaoEmpresas();
            //CarregaColeçãoFormatosCredenciais();

        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa> _ColaboradoresEmpresas;

        private ObservableCollection<ClasseVinculos.Vinculo> _Vinculos;

        private ObservableCollection<ClasseEmpresas.Empresa> _Empresas;

        private ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> _FormatosCredenciais;

        private ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha> _EmpresasLayoutsCrachas;

        private ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> _Contratos;

        private ClasseColaboradoresEmpresas.ColaboradorEmpresa _ColaboradorEmpresaSelecionado;

        private ClasseColaboradoresEmpresas.ColaboradorEmpresa _ColaboradorEmpresaTemp = new ClasseColaboradoresEmpresas.ColaboradorEmpresa();

        private List<ClasseColaboradoresEmpresas.ColaboradorEmpresa> _ColaboradoresEmpresasTemp = new List<ClasseColaboradoresEmpresas.ColaboradorEmpresa>();

        PopupPesquisaColaboradoresEmpresas popupPesquisaColaboradoresEmpresas;

        private int _selectedIndex;

        private int _ColaboradorSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        private string _Validade;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa> ColaboradoresEmpresas
        {
            get
            {
                return _ColaboradoresEmpresas;
            }

            set
            {
                if (_ColaboradoresEmpresas != value)
                {
                    _ColaboradoresEmpresas = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseVinculos.Vinculo> Vinculos
        {
            get
            {
                return _Vinculos;
            }

            set
            {
                if (_Vinculos != value)
                {
                    _Vinculos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha> EmpresasLayoutsCrachas
        {
            get
            {
                return _EmpresasLayoutsCrachas;
            }

            set
            {
                if (_EmpresasLayoutsCrachas != value)
                {
                    _EmpresasLayoutsCrachas = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseColaboradoresEmpresas.ColaboradorEmpresa ColaboradorEmpresaSelecionado
        {
            get
            {
                return this._ColaboradorEmpresaSelecionado;
            }
            set
            {
                this._ColaboradorEmpresaSelecionado = value;
                //base.OnPropertyChanged("SelectedItem");
                base.OnPropertyChanged();
                if (ColaboradorEmpresaSelecionado != null)
                {
                    //CarregaColeçãoEmpresasLayoutsCrachas(Convert.ToInt32(ColaboradorEmpresaSelecionado.EmpresaID));
                }

            }
        }

        public ObservableCollection<ClasseEmpresas.Empresa> Empresas

        {
            get
            {
                return _Empresas;
            }

            set
            {
                if (_Empresas != value)
                {
                    _Empresas = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> FormatosCredenciais

        {
            get
            {
                return _FormatosCredenciais;
            }

            set
            {
                if (_FormatosCredenciais != value)
                {
                    _FormatosCredenciais = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> Contratos
        {
            get
            {
                return _Contratos;
            }

            set
            {
                if (_Contratos != value)
                {
                    _Contratos = value;
                    OnPropertyChanged();

                }
            }
        }
        public int ColaboradorSelecionadaID
        {
            get
            {
                return this._ColaboradorSelecionadaID;

            }
            set
            {
                this._ColaboradorSelecionadaID = value;
                base.OnPropertyChanged();
                if (ColaboradorSelecionadaID != null)
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
                return this._HabilitaEdicao;
            }
            set
            {
                this._HabilitaEdicao = value;
                base.OnPropertyChanged();
            }
        }

        public string Criterios
        {
            get
            {
                return this._Criterios;
            }
            set
            {
                this._Criterios = value;
                base.OnPropertyChanged();
            }
        }

        public string Validade
        {
            get
            {
                return this._Validade;
            }
            set
            {
                this._Validade = value;
                base.OnPropertyChanged();
            }
        }

        //public string ComboEmpresaSelecionado
        //{
        //    get
        //    {
        //        return this._ComboEmpresaSelecionado;
        //    }
        //    set
        //    {
        //        this._ComboEmpresaSelecionado = value;
        //        base.OnPropertyChanged();
        //    }
        //}
        #endregion

        #region Comandos dos Botoes

        public void OnAtualizaCommand(object _ColaboradorID)
        {
            try
            {
                ColaboradorSelecionadaID = Convert.ToInt32(_ColaboradorID);
                Thread CarregaColecaoColaboradoresEmpresas_thr = new Thread(() => CarregaColecaoColaboradoresEmpresas(Convert.ToInt32(_ColaboradorID)));
                CarregaColecaoColaboradoresEmpresas_thr.Start();
                //CarregaColecaoColaboradoresEmpresas(Convert.ToInt32(_ColaboradorID));

            }
            catch (Exception ex)
            {

            }

        }
 
        public void OnEditarCommand()
        {
            try
            {
                //o preenchimento de _ColaboradoresEmpresasTemp é apenas para apoiar o metodo VerificaVinculo()
                _ColaboradoresEmpresasTemp.Clear();
                foreach (var x in ColaboradoresEmpresas)
                {
                    _ColaboradoresEmpresasTemp.Add(x);
                }

                _ColaboradorEmpresaTemp = ColaboradorEmpresaSelecionado.CriaCopia(ColaboradorEmpresaSelecionado);
                _selectedIndexTemp = SelectedIndex;

                //CarregaColecaoContratos(_ColaboradorEmpresaTemp.EmpresaID);

                HabilitaEdicao = true;
            }
            catch (Exception)
            {
            }
        }

        public void OnCancelarEdicaoCommand()
        {
            try
            {
                ColaboradoresEmpresas[_selectedIndexTemp] = _ColaboradorEmpresaTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {

            }
        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {

                HabilitaEdicao = false;
                

                //IMOD.Domain.Entities.ColaboradorEmpresa ColaboradorEmpresaEntity = new IMOD.Domain.Entities.ColaboradorEmpresa();
                //g.TranportarDados(ColaboradorEmpresaSelecionado, 1, ColaboradorEmpresaEntity);
                //var repositorio = new IMOD.Infra.Repositorios.ColaboradorEmpresaRepositorio();


                var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorEmpresa>(ColaboradorEmpresaSelecionado);
                var repositorio = new IMOD.Application.Service.ColaboradorEmpresaService();

                repositorio.Criar(entity);
                var id = entity.ColaboradorEmpresaId;



                ClasseColaboradoresEmpresas.ColaboradorEmpresa _ColaboradorEmpresaSelecionadoPro = new ClasseColaboradoresEmpresas.ColaboradorEmpresa();
                _ColaboradorEmpresaSelecionadoPro = ColaboradorEmpresaSelecionado;
                _ColaboradorEmpresaSelecionadoPro.ColaboradorEmpresaID = id;
                ColaboradoresEmpresas = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>(_ColaboradoresEmpresasTemp);
                ColaboradoresEmpresas.Add(_ColaboradorEmpresaSelecionadoPro);

                //Thread CarregaColecaoColaboradoresEmpresas_thr = new Thread(() => CarregaColecaoColaboradoresEmpresas(ColaboradorEmpresaSelecionado.ColaboradorID));
                //CarregaColecaoColaboradoresEmpresas_thr.Start();

            }
            catch (Exception ex)
            {
            }
        }

        public void OnSalvarEdicaoCommand()
        {
            try
            {
                //HabilitaEdicao = false;
                //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresEmpresas));

                //ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa> _ColaboradorEmpresaTemp = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>();
                //ClasseColaboradoresEmpresas _ClasseColaboradorerEmpresasTemp = new ClasseColaboradoresEmpresas();
                //_ColaboradorEmpresaTemp.Add(ColaboradorEmpresaSelecionado);
                //_ClasseColaboradorerEmpresasTemp.ColaboradoresEmpresas = _ColaboradorEmpresaTemp;


                //IMOD.Domain.Entities.ColaboradorEmpresa ColaboradorEmpresaEntity = new IMOD.Domain.Entities.ColaboradorEmpresa();
                //g.TranportarDados(ColaboradorEmpresaSelecionado, 1, ColaboradorEmpresaEntity);

                //var repositorio = new IMOD.Infra.Repositorios.ColaboradorEmpresaRepositorio();

                var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorEmpresa>(ColaboradorEmpresaSelecionado);
                var repositorio = new IMOD.Application.Service.ColaboradorEmpresaService();
                repositorio.Alterar(entity);

                var id = entity.ColaboradorEmpresaId;
                AtualizaCredenciais(id);

                //string xmlString;

                //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                //{

                //    using (XmlTextWriter xw = new XmlTextWriter(sw))
                //    {
                //        xw.Formatting = Formatting.Indented;
                //        serializer.Serialize(xw, _ClasseColaboradorerEmpresasTemp);
                //        xmlString = sw.ToString();
                //    }

                //}

                //InsereColaboradorEmpresaBD(xmlString);
                //if (!ColaboradorEmpresaSelecionado.Ativo)
                //{
                //    AtualizaCredenciais(ColaboradorEmpresaSelecionado.ColaboradorEmpresaID);
                //}




                // ColaboradoresEmpresas[_selectedIndexTemp] = ColaboradorEmpresaSelecionado;

                //Thread CarregaColecaoColaboradoresEmpresas_thr = new Thread(() => CarregaColecaoColaboradoresEmpresas(ColaboradorEmpresaSelecionado.ColaboradorID));
                //CarregaColecaoColaboradoresEmpresas_thr.Start();

                //_ClasseEmpresasSegurosTemp = null;

                //_SegurosTemp.Clear();
                //_seguroTemp = null;


            }
            catch (Exception ex)
            {
            }
        }


        public void OnAdicionarCommand()
        {
            try
            {
                _ColaboradoresEmpresasTemp.Clear();

                foreach (var x in ColaboradoresEmpresas)
                {
                    _ColaboradoresEmpresasTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                ColaboradoresEmpresas.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                CarregaColecaoEmpresas();
                _ColaboradorEmpresaTemp = new ClasseColaboradoresEmpresas.ColaboradorEmpresa();
                _ColaboradorEmpresaTemp.ColaboradorID = ColaboradorSelecionadaID;
                _ColaboradorEmpresaTemp.Matricula = string.Format("{0:#,##0}", Global.GerarID()) + "-" + String.Format("{0:yy}", DateTime.Now);
                _ColaboradorEmpresaTemp.Ativo = true;
                ColaboradoresEmpresas.Add(_ColaboradorEmpresaTemp);

                SelectedIndex = 0;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {

            }

        }
        //public void OnSelecionaEmpresaCommand(int _empresaID)
        //{
        //    CarregaColecaoContratos(_empresaID);
        //}


        public void OnCancelarAdicaoCommand()
        {
            try
            {
                ColaboradoresEmpresas = null;
                ColaboradoresEmpresas = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>(_ColaboradoresEmpresasTemp);
                SelectedIndex = _selectedIndexTemp;
                _ColaboradoresEmpresasTemp.Clear();
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
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
                        var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorEmpresa>(ColaboradorEmpresaSelecionado);
                        var repositorio = new IMOD.Application.Service.ColaboradorEmpresaService();
                        repositorio.Remover(entity);
                        
                        ColaboradoresEmpresas.Remove(ColaboradorEmpresaSelecionado);
                    }
                }

            }
            catch (Exception ex)
            {

            }

        }

        public void OnPesquisarCommand()
        {
            try
            {
                popupPesquisaColaboradoresEmpresas = new PopupPesquisaColaboradoresEmpresas();
                popupPesquisaColaboradoresEmpresas.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaColaboradoresEmpresas.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            try
            {
                object vetor = popupPesquisaColaboradoresEmpresas.Criterio.Split((char)(20));
                int _colaboradorID = ColaboradorSelecionadaID;
                string _matricula = ((string[])vetor)[0];
                string _empresaNome = ((string[])vetor)[1];
                string _cargo = ((string[])vetor)[2];
                int _ativo = Convert.ToInt32(((string[])vetor)[3]);

                CarregaColecaoColaboradoresEmpresas(_colaboradorID, _empresaNome,_cargo, _matricula, _ativo);
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {

            }

        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoColaboradoresEmpresas(int _colaboradorID, string _empresaNome = "", string _cargo = "", string _matricula = "", int _ativo = 2)
        {
            try
            {
                var service = new IMOD.Application.Service.ColaboradorEmpresaService();
                if (!string.IsNullOrWhiteSpace(_cargo)) _cargo = $"%{_cargo}%";
                if (!string.IsNullOrWhiteSpace(_matricula)) _matricula = $"%{_matricula}%";
                var list1 = service.Listar(_colaboradorID, _cargo, _matricula);

                var list2 = Mapper.Map<List<ClasseColaboradoresEmpresas.ColaboradorEmpresa>>(list1);

                var observer = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.ColaboradoresEmpresas = observer;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private void CarregaColecaoEmpresas(string _empresaID = "", string _nome = "", string _apelido = "", string _cNPJ = "", string _quantidaderegistro = "500")
        {
            try
            {
                
                var service = new IMOD.Application.Service.EmpresaService();
                if (!string.IsNullOrWhiteSpace(_nome)) _nome = $"%{_nome}%";
                if (!string.IsNullOrWhiteSpace(_apelido)) _apelido = $"%{_apelido}%";
                if (!string.IsNullOrWhiteSpace(_cNPJ)) _cNPJ = $"%{_cNPJ}%";

                var list1 = service.Listar(_empresaID, _nome, _apelido, _cNPJ);
                var list2 = Mapper.Map<List<ClasseEmpresas.Empresa>>(list1);

                var observer = new ObservableCollection<ClasseEmpresas.Empresa>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Empresas = observer;
                SelectedIndex = 0;


            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }    
        public void CarregaColecaoContratos(int empresaID = 0)
        {
            
            try
            {

                //var service = new IMOD.Application.Service.EmpresaContratoService();
                ////if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                ////if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                ////if (!string.IsNullOrWhiteSpace(cpf)) cpf = $"%{cpf}%";
                //var list1 = service.Listar();

                //var list2 = Mapper.Map<List<ClasseEmpresasContratos.EmpresaContrato>>(list1.OrderBy(n => n.EmpresaId));

                //var observer = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato>();
                //list2.ForEach(n =>
                //{
                //    observer.Add(n);
                //});

                //this.Contratos = observer;

                var service = new IMOD.Application.Service.EmpresaContratoService();
                var list1 = service.Listar(empresaID);
                var list2 = Mapper.Map<List<ClasseEmpresasContratos.EmpresaContrato>>(list1.OrderBy(n => n.EmpresaId));
                var observer = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Contratos = observer;
                SelectedIndex = 0;

                //SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access

        private DateTime validadeCursoContrato(int _colaborador = 0)
        {
            try
            {

                //DateTime _menorDataCurso = Convert.ToDateTime("01-01-2999");
                //DateTime _menorDataContrato = Convert.ToDateTime("01-01-2999");

                string _menorDataCurso = "";
                string _menorDataContrato = "";

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _strSql = "SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome,CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103) " +
                                 "as ValidadeCurso,DATEDIFF(DAY, GETDATE(), CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103)) AS Dias FROM dbo.Colaboradores " +
                                 "INNER JOIN dbo.ColaboradoresCursos ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresCursos.ColaboradorID where dbo.Colaboradores.Excluida = 0 And dbo.ColaboradoresCursos.Controlado = 1 And dbo.ColaboradoresCursos.ColaboradorID = " + _colaborador + " Order By Dias";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                if (_sqlreader.Read())
                {

                    //if (Convert.ToInt32(_sqlreader["Dias"]) < 30)
                    //{
                    //MessageBox.Show("Data de Vinculo!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _menorDataCurso = _sqlreader["ValidadeCurso"].ToString();
                    // break;
                    //}

                }
                _sqlreader.Close();


                //_strSql = "SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome, dbo.EmpresasContratos.EmpresaID, dbo.EmpresasContratos.NumeroContrato, " +
                //          "CONVERT(datetime,dbo.EmpresasContratos.Validade,103) as DataContrato, DATEDIFF ( DAY , GETDATE(),  CONVERT(datetime, dbo.EmpresasContratos.Validade,103))  AS Dias " +
                //          "FROM  dbo.EmpresasContratos INNER JOIN dbo.ColaboradoresEmpresas ON dbo.EmpresasContratos.EmpresaID = dbo.ColaboradoresEmpresas.EmpresaID INNER JOIN dbo.Colaboradores " +
                //          "ON dbo.ColaboradoresEmpresas.ColaboradorID = dbo.Colaboradores.ColaboradorID WHERE (dbo.Colaboradores.Excluida = 0) And dbo.Colaboradores.ColaboradorID = " + _colaborador + " Order By Dias";

                //_sqlcmd = new SqlCommand(_strSql, _Con);
                //_sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                //if (_sqlreader.Read())
                //{

                //    // if (Convert.ToInt32(_sqlreader["Dias"]) < 30)
                //    //{
                //    //MessageBox.Show("Data de Vinculo!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    _menorDataContrato = _sqlreader["DataContrato"].ToString();
                //    // break;
                //    //}

                //}
                //_sqlreader.Close();



                //if (Convert.ToDateTime(_menorDataCurso) < Convert.ToDateTime(_menorDataContrato))
                //{
                return Convert.ToDateTime(_menorDataCurso);
                //}
                //else if (Convert.ToDateTime(_menorDataCurso) > Convert.ToDateTime(_menorDataContrato))
                //{
                //    return Convert.ToDateTime(_menorDataContrato);
                //}

                //return DateTime.Now;

            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }
        #endregion

        #region Metodos Publicos
        public bool VerificaVinculo()
        {
            try
            {
                if( _ColaboradoresEmpresasTemp.Where(x =>
                (x.EmpresaContratoID == ColaboradorEmpresaSelecionado.EmpresaContratoID && x.Ativo)
                && (x.ColaboradorEmpresaID!= ColaboradorEmpresaSelecionado.ColaboradorEmpresaID)).Count() > 0)
                {
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        #endregion


        #region Metodos Privados

        private void AtualizaCredenciais(int colaboradorEmpresaID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;

                    _sqlCmd = new SqlCommand("Update ColaboradoresCredenciais Set " +
                            "CredencialStatusID=@v1" +
                            ",CredencialMotivoID=@v2" +
                            ",Baixa=@v3" +
                            ",Ativa=@v4" +
                            " Where ColaboradorEmpresaID = @v0 AND CredencialStatusID = 1", _Con);

                _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = colaboradorEmpresaID;
                _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = 2;
                _sqlCmd.Parameters.Add("@V2", SqlDbType.Int).Value = 8;
                _sqlCmd.Parameters.Add("@V3", SqlDbType.DateTime).Value = DateTime.Now;
                _sqlCmd.Parameters.Add("@V4", SqlDbType.Bit).Value = 0;

                _sqlCmd.ExecuteNonQuery();

                _Con.Close();

                //EFETUAR ATUALIZAÇÃO NO SC

            }
            catch (Exception ex)
            {

                
            }
        }

        #endregion

    }
}
