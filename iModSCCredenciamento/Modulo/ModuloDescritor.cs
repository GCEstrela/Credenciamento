// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Genetec.Sdk.Workspace.Pages;
using iModSCCredenciamento.Funcoes;

#endregion

namespace iModSCCredenciamento.Modulo
{
    public class ModuloDescritor : PageDescriptor
    {
        #region Constants

        public const string Privilege = "{175AD7DC-A9A9-FAFA-A19E-367AF9547DC9}";

        #endregion

        #region  Metodos

        public override bool HasPrivilege()
        {
            if (!m_sdk.IsConnected) return false;
               return m_sdk.SecurityManager.IsPrivilegeGranted (new Guid (Privilege));
        }

        #endregion

        #region Properties

        public override Guid CategoryId => Main.ImodCredencialGuid;

        public override string Description => "Cadastro de dados para o credenciamento de pessoal";

        public override string Name => "Credenciamento";

        public override Guid Type => new Guid ("{A72B3A40-FAFA-486C-BA75-EA008553D8E4}");

        public override ImageSource Icon => new BitmapImage (new Uri (@"pack://application:,,,/iModSCCredenciamento;Component/Resources/Cracha.ico", UriKind.RelativeOrAbsolute));

        public override ImageSource Thumbnail => new BitmapImage (new Uri (@"pack://application:,,,/iModSCCredenciamento;Component/Resources/Cracha.png", UriKind.RelativeOrAbsolute));

        #endregion
    }
}