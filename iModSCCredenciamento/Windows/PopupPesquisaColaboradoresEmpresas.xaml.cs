﻿using System;
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
using System.Windows.Shapes;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupPesquisaColaboradoresEmpresas.xaml
    /// </summary>
    public partial class PopupPesquisaColaboradoresEmpresas : Window
    {
        public PopupPesquisaColaboradoresEmpresas()
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
            string _ativo = "2";

            if (Todas_rb.IsChecked == true)
            {
                _ativo = "2";
            }
            else if (Ativo_rb.IsChecked == true)
            {
                _ativo = "1";
            }
            else if (Inativo_rb.IsChecked == true)
            {
                _ativo = "0";
            }

            Criterio = Matricula_tb.Text + (char)(20) + EmpresaRazaoSocial_tb.Text + (char)(20) + Cargo_tb.Text + (char)(20) + _ativo;
            EfetuarProcura(this, new EventArgs());
            this.DialogResult = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
