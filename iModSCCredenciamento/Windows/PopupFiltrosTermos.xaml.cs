using iModSCCredenciamento.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Interaction logic for PopupFiltrosTermos.xaml
    /// </summary>
    public partial class PopupFiltrosTermos : Window
    {
        public int _termo, _status;
        public PopupFiltrosTermos(int termo, int status)
        {

            InitializeComponent();
            this.DataContext = new TermosViewModel();
            MouseDown += Window_MouseDown;
            _termo = termo;
            _status = status;

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
                periodo = 1;
            }
            else if (semana_rb.IsChecked.Value)
            {
                periodo = 2;
            }
            else if (mes_rb.IsChecked.Value)
            {
                periodo = 3;
            }
            else
            {
                periodo = 0;
            }

            if (dp_dataInicial.Text != "" && dp_dataFinal.Text != "")
            {
                DataIni = dp_dataInicial.Text;
                DataFim = dp_dataFinal.Text;
            }


                ((TermosViewModel)this.DataContext).OnFiltrosTermosCommand(_termo, _status, periodo, DataIni, DataFim);


            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
