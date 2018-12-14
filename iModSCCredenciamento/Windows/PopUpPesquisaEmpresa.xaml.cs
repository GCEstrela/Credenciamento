using System;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Interaction logic for PopUpPesquisaEmpresa.xaml
    /// </summary>
    public partial class PopUpPesquisaEmpresa : Window
    {
        public PopUpPesquisaEmpresa()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
            Codigo_tb.Focus();
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

            Criterio = Codigo_tb.Text + (char)(20) + RazaoSocial_tb.Text + (char)(20) + NomeFantasia_tb.Text + (char)(20) + CNPJ_tb.Text;
            EfetuarProcura(this, new EventArgs());
            DialogResult = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
