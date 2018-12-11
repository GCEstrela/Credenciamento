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
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using Colaborador = IMOD.Domain.Entities.Colaborador;

#endregion


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
                iModSCCredenciamentoIcon = new BitmapImage(new Uri(@"pack://application:,,,/iModSCCredenciamento;Component/Resources/Cracha.png", UriKind.RelativeOrAbsolute));
                Global.AbreConfig();
            }

            catch (Exception)
            {

            }
        }

        #endregion

        #region Constants

        public static readonly Guid iModSCCredenciamentoId = new Guid("{2ACE4CD0-7E9C-FAFA-B8A6-24FD71D6DD59}");

        public static IEngine engine;

        private static readonly BitmapImage iModSCCredenciamentoIcon;

        private readonly List<Task> m_tasks = new List<Task>();

        #endregion

        #region Public Methods

        public override void Load()
        {
            engine = Workspace.Sdk;
            SubscribeToSdkEvents(engine);
            SubscribeToWorkspaceEvents();
        }

        public override void Unload()
        {
            if (Workspace != null)
            {
                UnregisterTaskExtensions();

                UnsubscribeFromWorkspaceEvents();

                UnsubscribeFromSdkEvents(Workspace.Sdk);
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
            }
        }

        private void OnLogonFailed(object sender, LogonFailedEventArgs e)
        {
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
                Workspace.Tasks.Unregister(task);
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
            var taskGroup = new TaskGroup(iModSCCredenciamentoId, Guid.Empty, "Módulo de Credenciamento", iModSCCredenciamentoIcon, 1500);
            taskGroup.Initialize(Workspace);
            m_tasks.Add(taskGroup);

            Task task = new CreatePageTask<PagePrincipal.PagePrincipal>(true);
            task.Initialize(Workspace);
            m_tasks.Add(task);

            foreach (var pageExtension in m_tasks)
            {
                Workspace.Tasks.Register(pageExtension);
            }
        }

        #endregion

        #region Event Handlers

        private void OnLoggedOn(object sender, LoggedOnEventArgs e)
        {
            try
            {
                var agenda = engine.GetEntity(new Guid("00000000-0000-0000-0000-000000000006")) as Schedule;
                var systemConfiguration = engine.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
                var service = systemConfiguration.CustomFieldService;
                Global._instancia = service.GetValue<string>("Instância", agenda.Guid);
                Global._bancoDados = service.GetValue<string>("Banco de Dados", agenda.Guid);
                Global._usuario = service.GetValue<string>("Usuário", agenda.Guid);
                Global._senha = service.GetValue<string>("Senha", agenda.Guid);

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
            var entity = engine.GetEntity(e.SourceGuid);
            if (entity != null)
            {
            }
        }

        private void OnEntitiesInvalidated(object sender, EntitiesInvalidatedEventArgs e)
        {

        }

        private void OnEntitiesAdded(object sender, EntitiesAddedEventArgs e)
        {

        }

        private void OnEntitiesRemoved(object sender, EntitiesRemovedEventArgs e)
        {

        }

        private void OnWorkspaceInitialized(object sender, InitializedEventArgs e)
        {
        }

        #endregion
    }

    public class AutoMapperConfig
    {
        #region  Metodos

        public static void RegisterMappings()
        {
            Mapper.Initialize(
                    m =>
                    {
                        //  .ForMember(n=>n.CNPJ,opt=>opt.MapFrom(n=>n.Cnpj.FormatarCnpj()))
                        m.CreateMap<Colaborador, ClasseColaboradores.Colaborador>()
                        .ForMember(k => k.CPF, opt => opt.MapFrom(k => k.Cpf.FormatarCpf())).ReverseMap();
                        m.CreateMap<ColaboradorCurso, ClasseColaboradoresCursos.ColaboradorCurso>().ReverseMap();
                        m.CreateMap<ClasseColaboradoresCredenciais, ClasseColaboradoresCredenciais.ColaboradorCredencial>().ReverseMap();
                        m.CreateMap<ColaboradoresCredenciaisView, ClasseColaboradoresCredenciais.ColaboradorCredencial>().ReverseMap();
                        m.CreateMap<ColaboradorEmpresa, ClasseColaboradoresEmpresas.ColaboradorEmpresa>().ReverseMap();
                        m.CreateMap<Empresa, ClasseEmpresas.Empresa>()
                       .ForMember(k => k.CNPJ, opt => opt.MapFrom(k => k.Cnpj.FormatarCnpj())).ReverseMap();
                        m.CreateMap<LayoutCracha, ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>().ReverseMap();

                        m.CreateMap<Estados, ClasseEstados.Estado>().ReverseMap();
                        m.CreateMap<Municipio, ClasseMunicipios.Municipio>().ReverseMap();
                        m.CreateMap<EmpresaSignatario, ClasseEmpresasSignatarios.EmpresaSignatario>().ReverseMap();
                        m.CreateMap<EmpresaContrato, ClasseEmpresasContratos.EmpresaContrato>().ReverseMap();
                        m.CreateMap<TipoAtividade, ClasseTiposAtividades.TipoAtividade>().ReverseMap();
                        m.CreateMap<TipoEquipamento, ClasseTiposEquipamento.TipoEquipamento>().ReverseMap();
                        m.CreateMap<EmpresaTipoAtividade, ClasseEmpresasTiposAtividades.EmpresaTipoAtividade>().ReverseMap();
                        m.CreateMap<AreaAcesso, ClasseAreasAcessos.AreaAcesso>().ReverseMap();
                        m.CreateMap<LayoutCracha, ClasseLayoutsCrachas.LayoutCracha>().ReverseMap();
                        m.CreateMap<EmpresaLayoutCrachaView, ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>().ReverseMap();


                    });
        }

        #endregion
    }
}