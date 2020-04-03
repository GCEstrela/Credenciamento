// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region
using Genetec.Sdk;
using Genetec.Sdk.Workspace.Pages;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.Views;
using IMOD.Domain.Constantes;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Page = Genetec.Sdk.Workspace.Pages.Page;

#endregion

namespace IMOD.CredenciamentoDeskTop.Modulo
{
    [Page(typeof(ModuloDescritor))]
    public class ModuloPage : Page
    {
        
        #region  Metodos
        private ConfiguraSistema _viewConfig = new ConfiguraSistema();
        private MenuPrincipalView _view  = new MenuPrincipalView();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private ConfiguraSistema _configuraSistema;
        private static ConfiguracoesView _configuracoesView;
        public ModuloPage()
        {

            //Configuration config = null;
            //string exeConfigPath = this.GetType().Assembly.Location;
            //config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
            //string myValue = GetAppSetting(config, "Licenca");
            string myValue="";
            _configuraSistema = ObterConfiguracao();
            
            if (!string.IsNullOrEmpty(_configuraSistema.Licenca))
            {
                myValue = _configuraSistema.Licenca.Trim();
            }
            EstrelaEncryparDecrypitar.Variavel.key = Constante.CRIPTO_KEY;
            EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
            string[] Decryptada = ESTRELA_EMCRYPTAR.EstrelaDecrypt(myValue).Split('<');
            string LicencaDecryptada = Decryptada[0];
            
            if (Decryptada.Length > 1)
            {

                DateTime DataExpiracaoLicencaDecryptada = Convert.ToDateTime(Decryptada[1]);
                Double expiracao = DataExpiracaoLicencaDecryptada.Subtract(DateTime.Now.Date).Days;
                if (expiracao < 15 && expiracao > 0)
                {
                    WpfHelp.PopupBox(string.Format("Sua licença vai expirar em {0} dias", expiracao), 1);

                }
                else if (expiracao <= 0)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    UsuarioLogado.LicencaValida = false;                    
                    this.View = new AcessoNegado("Licença válida até " + Decryptada[1] + ". Licença Expirada...");
                    //this.View = _configuracoesView;
                    return;
                }

            }
                
            if (UsuarioLogado.sdiLicenca != LicencaDecryptada)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                UsuarioLogado.LicencaValida = false;
                //WpfHelp.PopupBox("A Licença do credenciamento não é válida para este Security Center!", 1);
                this.View = new AcessoNegado("A Licença do credenciamento não é válida para este Security Center!");

                return;
            }
            
            //if (_configuraSistema.DBVersao == null) return;
            //if (_configuraSistema.DBVersao.Trim() != ConfiguracaoService.ObterVersaoSoftware(Assembly.GetExecutingAssembly()).Split()[1])
            //{
            //    WpfHelp.PopupBox("Versão do credenciamento " + ConfiguracaoService.ObterVersaoSoftware(Assembly.GetExecutingAssembly()).Split()[1].ToString() + " diferente da versão do banco " + _configuraSistema.DBVersao, 1);
            //    //this.View = new AcessoNegado("Versão do credenciamento " + ConfiguracaoService.ObterVersaoSoftware(Assembly.GetExecutingAssembly()).Split()[1].ToString() + " diferente da versão do banco " + _configuraSistema.DBVersao);
            //    //return;
            //}

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
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
        ///     Obtem configuração de sistema
        /// </summary>
        /// <returns></returns>
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresService.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
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