using System;
using System.Collections.Generic;
using System.Globalization;
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
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.ViewModels;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopPendencias.xaml
    /// </summary>
    public partial class PopupPendencias : Window
    {
        int _cadastro;
        int _entidadeID;
        int _tag;
        public PopupPendencias(int Cadastro, object Tag = null,int ID = 0, string Nome="")
        {
            InitializeComponent();

            Pendencias_gb.Header = "Detalhes da Pendência (" +  Nome + ")";

            this.DataContext = new PopupPendenciasViewModel();
            this.PreviewKeyDown += (ss, ee) =>
            {
                if (ee.Key == Key.Escape)
                {
                    
                    this.Close();
                }
            };

            _cadastro = Cadastro;
            _entidadeID = ID;

            if (Tag != null)
            {
                Tipo_cb.IsEnabled = false;
                _tag = Convert.ToInt32(Tag);

            }
            else
            {
                _tag = 0;
            }

            MouseDown += Window_MouseDown;
            ((PopupPendenciasViewModel)this.DataContext).OnAtualizaCommand( Cadastro, _tag,  ID );

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        #region Comando dos Botoes

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((PopupPendenciasViewModel)this.DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            Linha0_sp.IsEnabled = true;
            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaPendencias_lv.IsHitTestVisible = false;
            ((PopupPendenciasViewModel)this.DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            Linha0_sp.IsEnabled = true;
            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            ((PopupPendenciasViewModel)this.DataContext).OnAdicionarCommand(_cadastro, _tag, _entidadeID);
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((PopupPendenciasViewModel)this.DataContext).OnExcluirCommand(_cadastro, _tag, _entidadeID);
            if (ListaPendencias_lv.Items.Count == 0) { this.Close(); }
        }

        private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            //Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //((EmpresasColaboradorsViewModel)this.DataContext).ExecutarPesquisaCommand();
        }

        private void CancelarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Pesquisar_sp.Visibility = Visibility.Hidden;

        }

        private void CancelarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaPendencias_lv.IsHitTestVisible = true;
            ((PopupPendenciasViewModel)this.DataContext).OnCancelarEdicaoCommand();
            Linha0_sp.IsEnabled = false;
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((PopupPendenciasViewModel)this.DataContext).OnSalvarEdicaoCommand();
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaPendencias_lv.IsHitTestVisible = true;
            Linha0_sp.IsEnabled = false;
        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((PopupPendenciasViewModel)this.DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Linha0_sp.IsEnabled = false;
        }

        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((PopupPendenciasViewModel)this.DataContext).OnSalvarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Linha0_sp.IsEnabled = false;
            this.Close();
        }

        #endregion

        private void ListaPendencias_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ListaPendencias_lv.SelectedIndex == -1)
            {
                Editar_bt.IsEnabled = false;
                Excluir_bt.IsEnabled = false;
            }
            else
            {
                Editar_bt.IsEnabled = true;
                Excluir_bt.IsEnabled = true;
            }

        }


    }
}
