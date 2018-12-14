using System;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupPesquisaVeiculosCredenciais.xaml
    /// </summary>
    public partial class PopupPesquisaVeiculosCredenciais : Window
    {
        public PopupPesquisaVeiculosCredenciais()
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
            //((EmpresaViewModel)this.DataContext).ExecutarPesquisaCommand.Execute(null);
            int _ativo;

            if (Todas_rb.IsChecked == true)
            {
                _ativo = 2;
            }
            else if (Ativo_rb.IsChecked == true)
            {
                _ativo = 1;
            }
            else if (Inativo_rb.IsChecked == true)
            {
                _ativo = 0;
            }

            Criterio = Matricula_tb.Text + (char)(20) + EmpresaRazaoSocial_tb.Text + (char)(20) + Cargo_tb.Text + (char)(20) + Inativo_rb;
            EfetuarProcura(this, new EventArgs());
            DialogResult = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
