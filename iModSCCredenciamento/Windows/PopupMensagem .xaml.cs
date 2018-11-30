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
using System.Windows.Shapes;

namespace iModSCCredenciamento.Windows
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
