using System;
using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Interaction logic for PopupFiltrosTermos.xaml
    /// </summary>
    public partial class PopupFiltrosTermos : Window
    {
        public int _termo, _status;
        public bool _tipo;
        public PopupFiltrosTermos(int termo, int status, bool tipo)
        {

            InitializeComponent();
            DataContext = new TermosViewModel();
            MouseDown += Window_MouseDown;
            _termo = termo;
            _status = status;
            _tipo = tipo;

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void button_ClickFiltrar(object sender, RoutedEventArgs e)
        {
            int periodo = 0;
            string DataIni = "01/01/1900";
            string DataFim = DateTime.Now.ToShortDateString();

            if (hoje_rb.IsChecked.Value)
            {
                periodo = 0;
            }
            else if (semana_rb.IsChecked.Value)
            {
                periodo = 7;
            }
            else if (mes_rb.IsChecked.Value)
            {
                periodo = 30;
            }
            else
            {
                periodo = 999;
            }

            if (dp_dataInicial.Text != "" && dp_dataFinal.Text != "")
            {
                DataIni = dp_dataInicial.Text;
                DataFim = dp_dataFinal.Text;
            }

            if (_tipo)
            {
                ((TermosViewModel)DataContext).OnFiltrosTermosCredenciaisCommand(_termo, _status, periodo, DataIni, DataFim);
            }
            else
            {
                ((TermosViewModel)DataContext).OnFiltrosTermosAutorizacoesCommand(_termo, _status, periodo, DataIni, DataFim);
            }

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
