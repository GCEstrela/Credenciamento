using System.Windows;

namespace iModSCCredenciamento
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AutoMapperConfig.RegisterMappings();
            InitializeComponent();
          
        }
    }


}
