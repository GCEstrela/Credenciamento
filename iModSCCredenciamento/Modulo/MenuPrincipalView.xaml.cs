// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

using System;
using System.Windows;
using System.Windows.Media;
using Genetec.Sdk.Workspace;
using iModSCCredenciamento.Views;

namespace iModSCCredenciamento.Modulo
{
    /// <summary>
    ///     Interaction logic for MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipalView
    {

        public MenuPrincipalView()
        {
            InitializeComponent();
        }

        public static Workspace Workspace { get; private set; }
        public void Initialize(Workspace wrk)
        {
            if (wrk == null)
                throw new ArgumentNullException(nameof(wrk));
            Workspace = wrk;
            DataContext = null;//Iniciar sem conteudo na tela do frame
        }

        private void OpenEmpresa_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            //Empresas_bt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            //Colaboradores_bt.Background = Brushes.Transparent;
            //Veiculos_bt.Background = Brushes.Transparent;
            //Configuracoes_bt.Background = Brushes.Transparent;
            //Relatorios_bt.Background = Brushes.Transparent;
            //Termos_bt.Background = Brushes.Transparent;
            //ButtonClick(sender, new RoutedEventArgs());
            this.DataContext = new EmpresaView();
        }
    }
}