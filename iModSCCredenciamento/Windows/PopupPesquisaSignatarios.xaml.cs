using System;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Interação lógica para PopupPesquisaSignatarios.xam
    /// </summary>
    public partial class PopupPesquisaSignatarios : Window
    {
        public PopupPesquisaSignatarios()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
            Nome_tb.Focus();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        public event EventHandler EfetuarProcura;
        private string _criterio;
        public string Criterio
        {
            get { return _criterio; }
            set
            {
                _criterio = value;
            }
        }

        private void Procurar_bt_Click(object sender, RoutedEventArgs e)
        {
            Criterio = Nome_tb.Text;
            EfetuarProcura(this, new EventArgs());
            DialogResult = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
