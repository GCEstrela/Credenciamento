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

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Interação lógica para PopUpGrupos.xam
    /// </summary>
    public partial class PopUpGrupos : Window
    {
        public PopUpGrupos()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void Procurar_bt_Click(object sender, RoutedEventArgs e)
        {
            ICollection<Guid> guids = TCHG.SelectedItems;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
