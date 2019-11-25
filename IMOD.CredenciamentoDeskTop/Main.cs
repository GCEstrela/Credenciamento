// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Modules;
using Genetec.Sdk.Workspace.Tasks;
using IMOD.CredenciamentoDeskTop.Mapeamento;
using IMOD.CredenciamentoDeskTop.Modulo;
using IMOD.CrossCutting;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace IMOD.CredenciamentoDeskTop
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

        public static Engine Engine;

        #endregion

        #region Public Methods

        public override void Load()
        {
            Engine = (Engine)Workspace.Sdk;            

            SubscribeToSdkEvents (Engine);
            SubscribeToWorkspaceEvents();
           
        }
        private void _sdk_EntityInvalidated(object sender, EntityInvalidatedEventArgs e)
        {
            Entity entity = Engine.GetEntity(e.EntityGuid);
            if (entity != null)
            {
                if (entity.EntityType == EntityType.Credential)
                {
                    Credential _credential = Engine.GetEntity(entity.Guid) as Credential;
                    var state = _credential.State.ToString();
                    var data = _credential.ExpirationDate.ToString();
                    var messa = string.Format("{0}, {1} was modified {2}\r\n", entity.Name, state, data, e.IsLocalUpdate ? "locally" : "remotely");



                }
                else
                {
                    Cardholder _cardholder = Engine.GetEntity(entity.Guid) as Cardholder;
                    var state = _cardholder.State.ToString();
                    var data = _cardholder.ExpirationDate.ToString();
                    var messa = string.Format("{0}, {1} was modified {2}\r\n", entity.Name, state, data, e.IsLocalUpdate ? "locally" : "remotely");


                }
                //var ttt =entity.RunningState.ToString();

            }
        }
        private void UnregisterTaskExtensions()
        {
            // Register them to the workspace
            foreach (var task in _tasks)
            {
                Workspace.Tasks.Unregister (task);
            }

            _tasks.Clear();
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

        private readonly List<Task> _tasks = new List<Task>();

        private void RegisterTaskExtensions()
        {
            var image = new BitmapImage (new Uri (@"pack://application:,,,/IMOD.CredenciamentoDeskTop;Component/Resources/Cracha.png", UriKind.RelativeOrAbsolute));
            var taskGroup = new TaskGroup (ImodCredencialGuid, Guid.Empty, "Credenciamento", image, 1500);
            taskGroup.Initialize (Workspace); //Inicialiar grupo 
            _tasks.Add (taskGroup);
            //Adicionar modulo ao Genetec  
            Task task = new CreatePageTask<ModuloPage> (true);
            //Inicialiar workspace
            task.Initialize (Workspace);
            _tasks.Add (task);

            foreach (var item in _tasks)
            {
                Workspace.Tasks.Register (item);
            }
        }

        #endregion

        #region Event Handlers

        private void OnLoggedOn(object sender, LoggedOnEventArgs e)
        {
            RegisterTaskExtensions();
            //=================== Obter dados do usuario logado ================
            var usuario = Engine.LoggedUser; 
            if (usuario == null) return;
            UsuarioLogado.Nome = usuario.Name;
            UsuarioLogado.Email = usuario.EmailAddress;
            UsuarioLogado.Adm = usuario.IsAdministrator;

            SystemConfiguration _systemConfiguration = (SystemConfiguration)Engine.GetEntity(SdkGuids.SystemConfiguration);
            UsuarioLogado.sdiLicenca = _systemConfiguration.License.SystemId;
            
            //===================================================================
        }

        private void OnWorkspaceInitialized(object sender, InitializedEventArgs e)
        {
        }

        #endregion
    }
}