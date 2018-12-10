using System;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Interação lógica para PopupPesquisaEquipamentos.xam
    /// </summary>
    public partial class PopupPesquisaEquipamentos : Window
    {
        public PopupPesquisaEquipamentos()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
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
            Criterio = Descricao_tb.Text + (char)(20);
            EfetuarProcura(this, new EventArgs());
            DialogResult = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
