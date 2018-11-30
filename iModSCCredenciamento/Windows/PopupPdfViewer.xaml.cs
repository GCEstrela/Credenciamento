using iModSCCredenciamento.Funcoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica interna para PopupPdfViewer.xaml
    /// </summary>
    public partial class PopupPdfViewer : Window
    {
        private string _ArquivoPDF = "";
        public PopupPdfViewer(string _PDF)
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;

            _ArquivoPDF = _PDF;
            byte[] buffer = Conversores.StringToPDF(_ArquivoPDF);
            _ArquivoPDF = System.IO.Path.GetRandomFileName();
            _ArquivoPDF = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoPDF;
            _ArquivoPDF = System.IO.Path.ChangeExtension(_ArquivoPDF, ".pdf");
            System.IO.File.WriteAllBytes(_ArquivoPDF, buffer);

            pdfWebViewer.Navigate(new Uri(_ArquivoPDF));

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            pdfWebViewer.Dispose();

                Thread.Sleep(500);

            System.IO.File.Delete(_ArquivoPDF);
            this.Close();
        }
    }
}
