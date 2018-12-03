// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using AutoMapper;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Modules;
using Genetec.Sdk.Workspace.Tasks;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using Colaborador = IMOD.Domain.Entities.Colaborador;

#endregion

//using Colaborador = IMOD.Domain.Entities.Colaborador;

namespace iModSCCredenciamento
{
    public class Main : Module
    {
        #region Constructors

        static Main()
        {
            try
            {
                AutoMapperConfig.RegisterMappings();
                //iModSCCredenciamentoIcon = new BitmapImage(new Uri(@"iModSCCredenciamento.Resources.Cracha.png", UriKind.RelativeOrAbsolute));
                iModSCCredenciamentoIcon = new BitmapImage (new Uri (@"pack://application:,,,/iModSCCredenciamento;Component/Resources/Cracha.png", UriKind.RelativeOrAbsolute));
                Global.AbreConfig();
            }

            catch (Exception)
            {
            }
        }

        #endregion

        #region Constants

        public static readonly Guid iModSCCredenciamentoId = new Guid ("{2ACE4CD0-7E9C-FAFA-B8A6-24FD71D6DD59}");

        public static IEngine engine;

        private static readonly BitmapImage iModSCCredenciamentoIcon;

        private readonly List<Task> m_tasks = new List<Task>();

        #endregion

        #region Public Methods

        public override void Load()
        {
            engine = Workspace.Sdk;

            SubscribeToSdkEvents (engine);

            SubscribeToWorkspaceEvents();

            //RegisterTaskExtensions();
        }

        public override void Unload()
        {
            if (Workspace != null)
            {
                UnregisterTaskExtensions();

                UnsubscribeFromWorkspaceEvents();

                UnsubscribeFromSdkEvents (Workspace.Sdk);
            }
        }

        #endregion

        #region Private Methods - Modulo

        private void SubscribeToSdkEvents(IEngine engine)
        {
            if (engine != null)
            {
                engine.LoggedOn += OnLoggedOn;
                engine.LogonFailed += OnLogonFailed;

                //engine.EventReceived += OnEventReceived;
                //engine.EntitiesInvalidated += OnEntitiesInvalidated;
                //engine.EntitiesRemoved += OnEntitiesRemoved;
                //engine.EntitiesAdded += OnEntitiesAdded;
            }
        }

        private void OnLogonFailed(object sender, LogonFailedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void SubscribeToWorkspaceEvents()
        {
            if (Workspace != null)
            {
                Workspace.Initialized += OnWorkspaceInitialized;
            }
        }

        private void UnregisterTaskExtensions()
        {
            // Register them to the workspace
            foreach (var task in m_tasks)
            {
                Workspace.Tasks.Unregister (task);
            }

            m_tasks.Clear();
        }

        private void UnsubscribeFromSdkEvents(IEngine engine)
        {
            if (engine != null)
            {
                engine.LoggedOn -= OnLoggedOn;
            }
        }

        private void UnsubscribeFromWorkspaceEvents()
        {
            if (Workspace != null)
            {
                Workspace.Initialized -= OnWorkspaceInitialized;
            }
        }

        private void RegisterTaskExtensions()
        {
            var taskGroup = new TaskGroup (iModSCCredenciamentoId, Guid.Empty, "Módulo de Credenciamento", iModSCCredenciamentoIcon, 1500);
            taskGroup.Initialize (Workspace);
            m_tasks.Add (taskGroup);

            Task task = new CreatePageTask<PagePrincipal.PagePrincipal> (true);
            task.Initialize (Workspace);
            m_tasks.Add (task);

            foreach (var pageExtension in m_tasks)
            {
                Workspace.Tasks.Register (pageExtension);
            }
        }

        #endregion

        #region Event Handlers

        private void OnLoggedOn(object sender, LoggedOnEventArgs e)
        {
            try
            {
                var agenda = engine.GetEntity (new Guid ("00000000-0000-0000-0000-000000000006")) as Schedule;
                var systemConfiguration = engine.GetEntity (SdkGuids.SystemConfiguration) as SystemConfiguration;
                var service = systemConfiguration.CustomFieldService;
                Global._instancia = service.GetValue<string> ("Instância", agenda.Guid);
                Global._bancoDados = service.GetValue<string> ("Banco de Dados", agenda.Guid);
                Global._usuario = service.GetValue<string> ("Usuário", agenda.Guid);
                Global._senha = service.GetValue<string> ("Senha", agenda.Guid);

                Global._connectionString = "Data Source=" + Global._instancia + "; Initial Catalog=" + Global._bancoDados + "; User ID=" + Global._usuario + "; Password=" + Global._senha +
                                           "; Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";
            }
            catch (Exception ex)
            {
                Global.AbreConfig();
            }
            // CarregaLayoutCracha();
            RegisterTaskExtensions();
        }

        private void OnEventReceived(object sender, EventReceivedEventArgs e)
        {
            var entity = engine.GetEntity (e.SourceGuid);
            if (entity != null)
            {
            }
        }

        private void OnEntitiesInvalidated(object sender, EntitiesInvalidatedEventArgs e)
        {
            //foreach (Guid entityGuid in e.Entities.Select((info => info.EntityGuid)))
            //{
            //    Entity entity = engine.GetEntity(entityGuid);

            //    if (entity != null)
            //    {
            //        if (entity.EntityType == EntityType.Badge)
            //        {
            //            try
            //            {
            //                ClasseLayoutsCrachas.LayoutCracha layoutCracha = new ClasseLayoutsCrachas.LayoutCracha();
            //                layoutCracha.LayoutCrachaGUID = entity.Guid.ToString();
            //                layoutCracha.Nome = entity.Name;

            //                XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseLayoutsCrachas));

            //                ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> _LayoutsCrachasTemp = new ObservableCollection<ClasseLayoutsCrachas.LayoutCracha>();
            //                ClasseLayoutsCrachas _ClasseLayoutsCrachasTemp = new ClasseLayoutsCrachas();
            //                _LayoutsCrachasTemp.Add(layoutCracha);
            //                _ClasseLayoutsCrachasTemp.LayoutsCrachas = _LayoutsCrachasTemp;

            //                string xmlString;
            //                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
            //                {

            //                    using (XmlTextWriter xw = new XmlTextWriter(sw))
            //                    {
            //                        xw.Formatting = Formatting.Indented;
            //                        serializer.Serialize(xw, _ClasseLayoutsCrachasTemp);
            //                        xmlString = sw.ToString();
            //                    }

            //                }
            //                AtualizaLayoutCrachaBD(xmlString);

            //                _LayoutsCrachasTemp = null;
            //                layoutCracha = null;

            //                //EmpresasLayoutsCrachas.Add();
            //            }
            //            catch (Exception ex)
            //            {
            //            }

            //        }

            //    }

            //}
        }

        private void OnEntitiesAdded(object sender, EntitiesAddedEventArgs e)
        {
            //try
            //{
            //    foreach (Guid entityGuid in e.Entities.Select((info => info.EntityGuid)))
            //    {
            //        Entity entity = engine.GetEntity(entityGuid);

            //        if (entity != null)
            //        {
            //            if (entity.EntityType == EntityType.Badge)
            //            {
            //                try
            //                {
            //                    ClasseLayoutsCrachas.LayoutCracha layoutCracha = new ClasseLayoutsCrachas.LayoutCracha();
            //                    layoutCracha.LayoutCrachaGUID = entity.Guid.ToString();
            //                    layoutCracha.Nome = entity.Name;

            //                    XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseLayoutsCrachas));

            //                    ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> _LayoutsCrachasTemp = new ObservableCollection<ClasseLayoutsCrachas.LayoutCracha>();
            //                    ClasseLayoutsCrachas _ClasseLayoutsCrachasTemp = new ClasseLayoutsCrachas();
            //                    _LayoutsCrachasTemp.Add(layoutCracha);
            //                    _ClasseLayoutsCrachasTemp.LayoutsCrachas = _LayoutsCrachasTemp;

            //                    string xmlString;
            //                    using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
            //                    {

            //                        using (XmlTextWriter xw = new XmlTextWriter(sw))
            //                        {
            //                            xw.Formatting = Formatting.Indented;
            //                            serializer.Serialize(xw, _ClasseLayoutsCrachasTemp);
            //                            xmlString = sw.ToString();
            //                        }

            //                    }
            //                    AtualizaLayoutCrachaBD(xmlString);

            //                    _LayoutsCrachasTemp = null;
            //                    layoutCracha = null;

            //                    //EmpresasLayoutsCrachas.Add();
            //                }
            //                catch (Exception ex)
            //                {
            //                }

            //            }

            //        }

            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void OnEntitiesRemoved(object sender, EntitiesRemovedEventArgs e)
        {
            //foreach (EntityUpdateInfo info in e.Entities)
            //{

            //    if (info.EntityType == EntityType.Badge)
            //    {
            //        try
            //        {
            //            ClasseLayoutsCrachas.LayoutCracha layoutCracha = new ClasseLayoutsCrachas.LayoutCracha();
            //            layoutCracha.LayoutCrachaGUID = info.EntityGuid.ToString();
            //            layoutCracha.Nome = "";

            //            XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseLayoutsCrachas));

            //            ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> _LayoutsCrachasTemp = new ObservableCollection<ClasseLayoutsCrachas.LayoutCracha>();
            //            ClasseLayoutsCrachas _ClasseLayoutsCrachasTemp = new ClasseLayoutsCrachas();
            //            _LayoutsCrachasTemp.Add(layoutCracha);
            //            _ClasseLayoutsCrachasTemp.LayoutsCrachas = _LayoutsCrachasTemp;

            //            string xmlString;
            //            using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
            //            {

            //                using (XmlTextWriter xw = new XmlTextWriter(sw))
            //                {
            //                    xw.Formatting = Formatting.Indented;
            //                    serializer.Serialize(xw, _ClasseLayoutsCrachasTemp);
            //                    xmlString = sw.ToString();
            //                }

            //            }
            //            ExcluiLayoutCrachaBD(xmlString);

            //            _LayoutsCrachasTemp = null;
            //            layoutCracha = null;

            //            //EmpresasLayoutsCrachas.Add();
            //        }
            //        catch (Exception ex)
            //        {
            //        }

            //    }
            //}
        }

        private void OnWorkspaceInitialized(object sender, InitializedEventArgs e)
        {
        }

        #endregion

        #region Private Methods - Credenciamento

        private void CarregaLayoutCracha()
        {
            //try
            //{

            //    XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseLayoutsCrachas));

            //    ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> _LayoutsCrachasTemp = new ObservableCollection<ClasseLayoutsCrachas.LayoutCracha>();

            //    ClasseLayoutsCrachas _ClasseLayoutsCrachasTemp = new ClasseLayoutsCrachas();

            //    EntityConfigurationQuery query;

            //    QueryCompletedEventArgs result;

            //    query = engine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;

            //    query.EntityTypeFilter.Add(EntityType.Badge);

            //    query.NameSearchMode = StringSearchMode.StartsWith;

            //    result = query.Query();

            //    if (result.Success)
            //    {
            //        foreach (DataRow dr in result.Data.Rows)
            //        {
            //            Entity _Badge = engine.GetEntity((Guid)dr[0]) as Entity;

            //            ClasseLayoutsCrachas.LayoutCracha layoutCracha = new ClasseLayoutsCrachas.LayoutCracha();

            //            layoutCracha.LayoutCrachaGUID = _Badge.Guid.ToString();

            //            layoutCracha.Nome = _Badge.Name;

            //            _LayoutsCrachasTemp.Add(layoutCracha);
            //        }

            //    }

            //    _ClasseLayoutsCrachasTemp.LayoutsCrachas = _LayoutsCrachasTemp;

            //    string xmlString;
            //    using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
            //    {

            //        using (XmlTextWriter xw = new XmlTextWriter(sw))
            //        {
            //            xw.Formatting = Formatting.Indented;
            //            serializer.Serialize(xw, _ClasseLayoutsCrachasTemp);
            //            xmlString = sw.ToString();
            //        }

            //    }
            //    AtualizaLayoutCrachaBD(xmlString);

            //    _LayoutsCrachasTemp = null;
            //    //layoutCracha = null;

            //    //EmpresasLayoutsCrachas.Add();
            //}
            //catch (Exception ex)
            //{
            //}
        }

        private void AtualizaLayoutCrachaBD(string xmlString)
        {
            //try
            //{

            //    System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

            //    _xmlDoc.LoadXml(xmlString);

            //    ClasseLayoutsCrachas.LayoutCracha _LayoutCracha = new ClasseLayoutsCrachas.LayoutCracha();

            //    if (_xmlDoc.GetElementsByTagName("LayoutCrachaGUID").Count == 0)
            //    {
            //        return;
            //    }

            //    SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

            //    for (int x = 0; x < _xmlDoc.GetElementsByTagName("LayoutCrachaGUID").Count; x++)
            //    {

            //        string _strSql = "Select * from LayoutsCrachas where LayoutCrachaGUID='" + _xmlDoc.GetElementsByTagName("LayoutCrachaGUID")[x].InnerText + "'";
            //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
            //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
            //        if (_sqlreader.Read())
            //        {
            //            SqlCommand _sqlCmd;

            //                _LayoutCracha.Nome = _xmlDoc.GetElementsByTagName("Nome")[x].InnerText;

            //                _sqlCmd = new SqlCommand("Update LayoutsCrachas Set Nome='" + _LayoutCracha.Nome +
            //                    "' Where LayoutCrachaGUID='" + _xmlDoc.GetElementsByTagName("LayoutCrachaGUID")[x].InnerText + "'", _Con);

            //                _sqlCmd.ExecuteNonQuery();

            //        }
            //        else
            //        {
            //            SqlCommand _sqlCmd;

            //                _LayoutCracha.LayoutCrachaGUID = _xmlDoc.GetElementsByTagName("LayoutCrachaGUID")[x].InnerText;

            //                _LayoutCracha.Nome = _xmlDoc.GetElementsByTagName("Nome")[x].InnerText;

            //                _sqlCmd = new SqlCommand("Insert into LayoutsCrachas(LayoutCrachaGUID,Nome) values ('" + _LayoutCracha.LayoutCrachaGUID + "','" + _LayoutCracha.Nome + "')", _Con);

            //                _sqlCmd.ExecuteNonQuery();

            //        }

            //    }

            //    _Con.Close();

            //}
            //catch (Exception ex)
            //{
            //    Global.Log("Erro na void AtualizaLayoutCrachaBD ex: " + ex);

            //}
        }

        private void ExcluiLayoutCrachaBD(string xmlString)
        {
            //try
            //{

            //    System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

            //    _xmlDoc.LoadXml(xmlString);

            //    ClasseLayoutsCrachas.LayoutCracha _LayoutCracha = new ClasseLayoutsCrachas.LayoutCracha();

            //    if (_xmlDoc.GetElementsByTagName("LayoutCrachaGUID").Count == 0)
            //    {
            //        return;
            //    }

            //     SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
            //    SqlCommand _sqlCmd;
            //    _sqlCmd = new SqlCommand("Delete from LayoutsCrachas where LayoutCrachaGUID='" + _xmlDoc.GetElementsByTagName("LayoutCrachaGUID")[0].InnerText + "'", _Con);

            //    _sqlCmd.ExecuteNonQuery();

            //    _sqlCmd = new SqlCommand("Delete from EmpresasLayoutsCrachas where LayoutCrachaGUID='" + _xmlDoc.GetElementsByTagName("LayoutCrachaGUID")[0].InnerText + "'", _Con);

            //    _sqlCmd.ExecuteNonQuery();

            //    _Con.Close();
            //}
            //catch (Exception ex)
            //{
            //    Global.Log("Erro na void ExcluiLayoutCrachaBD ex: " + ex);

            //}
        }

        #endregion
    }

    public class AutoMapperConfig
    {
        #region  Metodos

        public static void RegisterMappings()
        {
            Mapper.Initialize (
                m =>
                {
                    //*******Valnei Filho
                    m.CreateMap<Colaborador, ClasseColaboradores.Colaborador>().ReverseMap();
                    m.CreateMap<Empresa, ClasseEmpresas.Empresa>()
                        .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                        .ReverseMap();
                    m.CreateMap<Estados, ClasseEstados.Estado>().ReverseMap();
                    m.CreateMap<Municipio, ClasseMunicipios.Municipio>().ReverseMap();
                    m.CreateMap<TipoAtividade, ClasseTiposAtividades.TipoAtividade>().ReverseMap();
                    m.CreateMap<TipoAcesso, ClasseTiposAcessos.TipoAcesso>().ReverseMap();
                    m.CreateMap<Status, ClasseStatus.Status>().ReverseMap();
                    m.CreateMap<EmpresaTipoAtividade, ClasseEmpresasTiposAtividades.EmpresaTipoAtividade>().ReverseMap();
                    m.CreateMap<AreaAcesso, ClasseAreasAcessos.AreaAcesso>().ReverseMap();
                    m.CreateMap<LayoutCracha, ClasseLayoutsCrachas.LayoutCracha>().ReverseMap();
                    m.CreateMap<EmpresaLayoutCrachaView, ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>().ReverseMap();
                    m.CreateMap<TipoCobranca, ClasseTiposCobrancas.TipoCobranca>().ReverseMap();
                    m.CreateMap<EmpresaContrato, ClasseEmpresasContratos.EmpresaContrato>().ReverseMap();
                    //*******Valnei Filho
                });
        }

        #endregion
    }
}