// ***********************************************************************
// Project: IMOD.ImodApp
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
// ***********************************************************************

#region

using System.Windows;
using iModSCCredenciamento.Mapeamento;

#endregion

namespace IMOD.ImodApp
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AutoMapperConfig.RegisterMappings();
        }
    }
}