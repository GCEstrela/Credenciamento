using System;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupPesquisaColaboradoresCredenciais.xaml
    /// </summary>
    public partial class PopupPesquisaColaboradoresCredenciais : Window
    {
        public PopupPesquisaColaboradoresCredenciais()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
            Empresa_tb.Focus();
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
            //((EmpresaViewModel)this.DataContext).ExecutarPesquisaCommand.Execute(null);
            string _status = "0";

            if (Todas_rb.IsChecked == true)
            {
                _status = "0";
            }
            else if (Ativa_rb.IsChecked == true)
            {
                _status = "1";
            }
            else if (Cancelada_rb.IsChecked == true)
            {
                _status = "2";
            }
            else if (Destruida_rb.IsChecked == true)
            {
                _status = "3";
            }
            else if (NaoDevolvida_rb.IsChecked == true)
            {
                _status = "4";
            }

            string _tipoCredencialID = "0";

            if (Tipo_cb.Text == "")
            {
                _tipoCredencialID = "0";
            }
            else if (Tipo_cb.Text == "Permanente")
            {
                _tipoCredencialID = "1";
            }
            else if (Tipo_cb.Text == "Temporária")
            {
                _tipoCredencialID = "2";
            }

            Criterio = Empresa_tb.Text + (char)(20) + _tipoCredencialID + (char)(20) + _status;
            EfetuarProcura(this, new EventArgs());
            DialogResult = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
