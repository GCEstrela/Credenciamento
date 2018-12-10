using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Genetec.Sdk.Workspace.Pages;
using iModSCCredenciamento.Funcoes;

namespace iModSCCredenciamento.PagePrincipal
{

    [Page(typeof(PagePrincipalPageDescriptor))]
    public class PagePrincipal : Page
    {
        #region Constants

        private readonly PagePrincipalView m_view = new PagePrincipalView();

        #endregion

        #region Constructors

        public PagePrincipal()
        {
            View = m_view;
        }

        #endregion

        #region Protected Methods

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
            m_view.Initialize(Workspace);
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
    public class PagePrincipalPageDescriptor : PageDescriptor
    {

        #region Constructors

        public PagePrincipalPageDescriptor()
        {
            m_icon = new BitmapImage(new Uri(@"pack://application:,,,/iModSCCredenciamento;Component/Resources/Cracha.ico", UriKind.RelativeOrAbsolute));
            m_thumbnail = new BitmapImage(new Uri(@"pack://application:,,,/iModSCCredenciamento;Component/Resources/Cracha.png", UriKind.RelativeOrAbsolute));
        }

        #endregion

        #region Constants

        public const string Privilege = "{175AD7DC-A9A9-FAFA-A19E-367AF9547DC9}";

        private readonly ImageSource m_icon;

        private readonly ImageSource m_thumbnail;

        #endregion

        #region Properties

        public override Guid CategoryId
        {
            get { return Main.iModSCCredenciamentoId; }
        }

        public override string Description
        {
            get
            {
                return "Esta tarefa abre o módulo de Credenciamento";
            }
        }

        public override string Name
        {
            get
            {
                return "Módulo Credenciamento";
            }
        }

        public override Guid Type
        {
            get
            {
                return new Guid("{A72B3A40-FAFA-486C-BA75-EA008553D8E4}");
            }
        }
        public override ImageSource Icon
        {
            get { return m_icon; }
        }
        public override ImageSource Thumbnail
        {
            get { return m_thumbnail; }
        }

        #endregion

        #region Public Methods

        public override bool HasPrivilege()
        {
            if (m_sdk.IsConnected)
            {
                Global.Privilegio_Salvar = m_sdk.SecurityManager.IsPrivilegeGranted(new Guid("{D1EE90DF-88CC-4ABF-A92E-1B0F57F8CF79}"));
                return m_sdk.SecurityManager.IsPrivilegeGranted(new Guid(Privilege));
            }

            return false;
        }

        #endregion
    }
}
