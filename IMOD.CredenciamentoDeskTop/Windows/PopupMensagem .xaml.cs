using System.Windows;
using System.Windows.Input;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para popupPesquisaSeguro.xaml
    /// </summary>
    public partial class PopupMensagem : Window
    {
 
        public PopupMensagem(string Texto)
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
            Mensagem_lb.Content = Texto;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }



    }
}
