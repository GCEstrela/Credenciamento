// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region

using Genetec.Sdk.Workspace.Pages;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.Domain.EntitiesCustom;
using System.Configuration;
using Page = Genetec.Sdk.Workspace.Pages.Page;

#endregion

namespace IMOD.CredenciamentoDeskTop.Modulo
{
    [Page(typeof(ModuloDescritor))]
    public class ModuloPage : Page
    {
        
        #region  Metodos

        private MenuPrincipalView _view  = new MenuPrincipalView();
       
        public ModuloPage()
        {
            Configuration config = null;
            string exeConfigPath = this.GetType().Assembly.Location;
            config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
            string myValue = GetAppSetting(config, "Licenca");

            EstrelaEncryparDecrypitar.Variavel.key = "CREDENCIAMENTO2019";
            EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
            string LicencaDecryptada = ESTRELA_EMCRYPTAR.EstrelaDecrypt(myValue);
            if (UsuarioLogado.sdiLicenca != LicencaDecryptada)
            {
                UsuarioLogado.LicencaValida = false;
                WpfHelp.PopupBox("A Licença do credenciamento não é válida para este Security Center!", 1);
                this.View = new AcessoNegado();
                return;
            }
            this.View = _view; 
        }
        string GetAppSetting(Configuration config, string key)
        {
            KeyValueConfigurationElement element = config.AppSettings.Settings[key];
            if (element != null)
            {
                string value = element.Value;
                if (!string.IsNullOrEmpty(value))
                    return value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Deserializes the data contained by the specified byte array.
        /// </summary>
        /// <param name="data">A byte array that contains the data.</param>
        protected override void Deserialize(byte[] data)
        {
        }

        /// <summary>
        /// Initialize the page.
        /// </summary>
        /// <remarks>At this step, the <see cref="Genetec.Sdk.Workspace.Workspace"/> is available.</remarks>
        protected override void Initialize()
        { 
            _view.Initialize(Workspace);
        }

        /// <summary>
        /// Serializes the data to a byte array.
        /// </summary>
        /// <returns>A byte array that contains the data.</returns>
        protected override byte[] Serialize()
        {
            return null;
        }


        #endregion
    }
}