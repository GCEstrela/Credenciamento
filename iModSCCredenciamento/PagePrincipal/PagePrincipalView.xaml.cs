using Genetec.Sdk.Workspace;
using iModSCCredenciamento.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iModSCCredenciamento.PagePrincipal
{
    /// <summary>
    /// Interação lógica para PagePrincipal.xam
    /// </summary>
    public partial class PagePrincipalView : UserControl
    {

        #region Constructors
        public PagePrincipalView()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
       // protected Workspace Workspace { get; private set; }
        public static Workspace Workspace { get; private set; }
        Object EmpresaViewDataContext = new EmpresaView();
        Object ColaboradorViewDataContext = new ColaboradorView();
        Object VeiculoViewDataContext = new VeiculoView();
        Object ConfiguracoesViewDataContext = new ConfiguracoesView();
        Object RelatoriosViewDataContext = new RelatoriosView();
        Object TermosViewDataContext = new TermosView();
        #endregion

        #region Public Methods

        public void Initialize(Workspace workspace)
        {
            if (workspace == null)
                throw new ArgumentNullException("workspace");

            Workspace = workspace;

        }

        #endregion

        #region Event Handlers
        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            switch (((System.Windows.FrameworkElement)sender).Name)
            {
                case "Empresas_bt":
                    DataContext = EmpresaViewDataContext;

                    break;

                case "Colaboradores_bt":
                    DataContext = ColaboradorViewDataContext;
                    break;

                case "Veiculos_bt":
                    DataContext = VeiculoViewDataContext;
                    break;

                case "Configuracoes_bt":
                    DataContext = ConfiguracoesViewDataContext;
                    break;

                case "Relatorios_bt":
                    DataContext = RelatoriosViewDataContext;
                    break;

                case "Termos_bt":
                    DataContext = TermosViewDataContext;
                    break;
            }

        }
        #endregion
    }
}
