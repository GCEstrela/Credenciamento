// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region

using System;
using System.Windows.Media.Imaging;
using Genetec.Sdk;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Modules;
using Genetec.Sdk.Workspace.Tasks;
using iModSCCredenciamento.Mapeamento;
using iModSCCredenciamento.Modulo;
using IMOD.CrossCutting;

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
                //AutoMapper configuraçoes
                AutoMapperConfig.RegisterMappings();
            }

            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Constants

        public static readonly Guid ImodCredencialGuid = new Guid ("{2ACE4CD0-7E9C-FAFA-B8A6-24FD71D6DD59}");

        private static IEngine Engine;

        #endregion

        #region Public Methods

        public override void Load()
        {
            Engine = Workspace.Sdk;
            SubscribeToSdkEvents (Engine);
            SubscribeToWorkspaceEvents();
            RegisterTaskExtensions();
        }

        public override void Unload()
        {
            if (Workspace != null)
            {
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
            }
        }

        private void SubscribeToWorkspaceEvents()
        {
            if (Workspace != null)
            {
                Workspace.Initialized += OnWorkspaceInitialized;
            }
        }

        private void UnsubscribeFromSdkEvents(IEngine engine)
        {
            if (engine == null) return;
            engine.LoggedOn -= OnLoggedOn;
        }

        private void UnsubscribeFromWorkspaceEvents()
        {
            if (Workspace == null) return;
            Workspace.Initialized -= OnWorkspaceInitialized;
        }

        private void RegisterTaskExtensions()
        {
            //Adicionar grupo ao Genetec
            var image = new BitmapImage (new Uri (@"pack://application:,,,/iModSCCredenciamento;Component/Resources/Cracha.png", UriKind.RelativeOrAbsolute));
            var taskgrupo1 = new TaskGroup (ImodCredencialGuid, Guid.NewGuid(), "Credenciamento", image, 1500);
            taskgrupo1.Initialize (Workspace); //Inicialiar grupo 

            Workspace.Tasks.Register (taskgrupo1); //Adicionar grupo
            //Adicionar modulo ao Genetec  
            var taskModulo = new CreatePageTask<ModuloPage> (true);
            taskModulo.Initialize (Workspace); //Inicialiar modulo
            Workspace.Tasks.Register (taskModulo); //Adicionar modulo
        }

        #endregion

        #region Event Handlers

        private void OnLoggedOn(object sender, LoggedOnEventArgs e)
        {
            //try
            //{
            //    var agenda = Engine.GetEntity(new Guid("00000000-0000-0000-0000-000000000006")) as Schedule;
            //    var systemConfiguration = Engine.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
            //    var service = systemConfiguration.CustomFieldService;
            //    Global._instancia = service.GetValue<string>("Instância", agenda.Guid);
            //    Global._bancoDados = service.GetValue<string>("Banco de Dados", agenda.Guid);
            //    Global._usuario = service.GetValue<string>("Usuário", agenda.Guid);
            //    Global._senha = service.GetValue<string>("Senha", agenda.Guid);

            //    Global._connectionString = "Data Source=" + Global._instancia + "; Initial Catalog=" + Global._bancoDados + "; User ID=" + Global._usuario + "; Password=" + Global._senha +
            //                               "; Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";
            //}
            //catch (Exception ex)
            //{
            //    Global.AbreConfig();
            //}
            //// CarregaLayoutCracha();
            //RegisterTaskExtensions();
        }

        private void OnWorkspaceInitialized(object sender, InitializedEventArgs e)
        {
        }

        #endregion
    }
}