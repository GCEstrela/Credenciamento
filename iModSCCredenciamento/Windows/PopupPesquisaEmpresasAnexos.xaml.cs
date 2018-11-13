using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Controls.Primitives;
//using System.Windows.Controls.Ribbon;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Interação lógica para PopupPesquisaAnexosEmpresas.xam
    /// </summary>
    public partial class PopupPesquisaEmpresasAnexos : Window
    {
        public PopupPesquisaEmpresasAnexos()
        {
            InitializeComponent();
            //LimpaTextBoxes(0,this);
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

            //foreach (object c in Grid1.Children)

            //{

            //    if (c is TextBox)

            //    {

            //        ((TextBox)c).Text = "";

            //    }

            //}

            //LimpaTextBoxes(0, this);
            //((EmpresaViewModel)this.DataContext).ExecutarPesquisaCommand.Execute(null);

            Criterio = Descrícao_tb.Text;
            EfetuarProcura(this, new EventArgs());
            this.DialogResult = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}

