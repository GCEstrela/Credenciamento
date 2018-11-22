using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.ViewModels;
using iModSCCredenciamento.Windows;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;
namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interação lógica para VeiculoView.xam
    /// </summary>
    public partial class VeiculoView : UserControl
    {
        public VeiculoView()
        {
            InitializeComponent();
            this.DataContext = new VeiculoViewModel();

        }
        #region Comando dos Botoes
        private void SelecionarFoto_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if (openFileDialog.ShowDialog() == true)
                {
                    BitmapImage _img = new BitmapImage(new Uri(openFileDialog.FileName));

                    string _imgstr = Conversores.IMGtoSTR(_img);

                    var fileLength = new FileInfo(openFileDialog.FileName).Length; //limitar o tamanho futuro

                    Foto_im.Source = _img;
                    ((ClasseVeiculos.Veiculo)ListaVeiculos_lv.SelectedItem).Foto = _imgstr; //Conversores.IMGtoSTR(new BitmapImage(new Uri(arquivoLogo.FileName)));
                    //ListaEmpresas_lv.Items.Refresh();

                    //BindingExpression be = BindingOperations.GetBindingExpression(Logo_im, Image.SourceProperty);
                    //be.UpdateTarget();
                    //_imgstr = null;
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void BuscarArquivoAnexo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculoViewModel)this.DataContext).OnBuscarArquivoCommand();
            //Arquivo_tb.Text = ((VeiculoViewModel)this.DataContext).Veiculos[0].NomeArquivoAnexo;
        }

        private void AbrirArquivoAnexo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculoViewModel)this.DataContext).OnAbrirArquivoCommand();
        }
        private void CapturarFoto_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                PopupWebCam _PopupWebCam = new PopupWebCam();
                _PopupWebCam.ShowDialog();

                BitmapSource _img = _PopupWebCam.Captura;

                if (_img != null)
                {
                    string _imgstr = Conversores.IMGtoSTR(_img);
                    Foto_im.Source = _img;
                    ((ClasseVeiculos.Veiculo)ListaVeiculos_lv.SelectedItem).Foto = _imgstr;

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void BuscarApoliceArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculoViewModel)this.DataContext).OnBuscarArquivoCommand();
        }

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculoViewModel)this.DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            VinculoEmpresa_ti.Visibility = Visibility.Hidden;
            Seguros_ti.Visibility = Visibility.Hidden;
            Credenciais_ti.Visibility = Visibility.Hidden;

            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaVeiculos_lv.IsHitTestVisible = false;
            Geral_sp.IsHitTestVisible = true;
            ((VeiculoViewModel)this.DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            VinculoEmpresa_ti.Visibility = Visibility.Hidden;
            Seguros_ti.Visibility = Visibility.Hidden;
            Credenciais_ti.Visibility = Visibility.Hidden;

            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Geral_sp.IsHitTestVisible = true;
            Geral_bt.Visibility = Visibility.Hidden;
            ((VeiculoViewModel)this.DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculoViewModel)this.DataContext).OnExcluirCommand();
        }

        private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            //Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //((EmpresasVeiculosViewModel)this.DataContext).ExecutarPesquisaCommand();
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
            ListaVeiculos_lv.IsHitTestVisible = true;
            Geral_sp.IsHitTestVisible = false;
            ((VeiculoViewModel)this.DataContext).OnCancelarEdicaoCommand();

            VinculoEmpresa_ti.Visibility = Visibility.Visible;
            Seguros_ti.Visibility = Visibility.Visible;
            Credenciais_ti.Visibility = Visibility.Visible;
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            if (Placa_tb.Text.Length == 0)
            {
                Global.PopupBox("Insira a Placa!", 4);
                Placa_tb.Focus();
                return;
            }

            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }

            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculoViewModel)this.DataContext).SalvarEdicao();
            //((VeiculoViewModel)this.DataContext).OnSalvarEdicaoCommandAsync();
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaVeiculos_lv.IsHitTestVisible = true;
            Geral_sp.IsHitTestVisible = false;

            VinculoEmpresa_ti.Visibility = Visibility.Visible;
            Seguros_ti.Visibility = Visibility.Visible;
            Credenciais_ti.Visibility = Visibility.Visible;
        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculoViewModel)this.DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Geral_sp.IsHitTestVisible = false;

            VinculoEmpresa_ti.Visibility = Visibility.Visible;
            Seguros_ti.Visibility = Visibility.Visible;
            Credenciais_ti.Visibility = Visibility.Visible;
            Geral_bt.Visibility = Visibility.Visible;

        }

        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            if (Placa_tb.Text.Length == 0)
            {
                Global.PopupBox("Insira a Placa!", 4);
                Placa_tb.Focus();
                return;
            }

            if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            {
                return;
            }

            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((VeiculoViewModel)this.DataContext).SalvarAdicao();
            //((VeiculoViewModel)this.DataContext).OnSalvarAdicaoCommandAsync();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Geral_sp.IsHitTestVisible = false;

            VinculoEmpresa_ti.Visibility = Visibility.Visible;
            Seguros_ti.Visibility = Visibility.Visible;
            Credenciais_ti.Visibility = Visibility.Visible;
            Geral_bt.Visibility = Visibility.Visible;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((VeiculoViewModel)this.DataContext).OnAbrirPendencias(sender, e);
        }
        #endregion

        #region Metodos Privados
        private void OnTabSelected(object sender, RoutedEventArgs e)
        {
            Thickness marginThickness = ListaVeiculos_lv.Margin;
            ListaVeiculos_sp.Margin = new Thickness(marginThickness.Left, marginThickness.Top, 170, marginThickness.Bottom);
            Botoes_ca.Visibility = Visibility.Visible;
            ListaVeiculos_lv.Focus();
        }

        private void OnTabUnSelected(object sender, RoutedEventArgs e)
        {
            Thickness marginThickness = ListaVeiculos_lv.Margin;
            ListaVeiculos_sp.Margin = new Thickness(marginThickness.Left, marginThickness.Top, 0, marginThickness.Bottom);
            Botoes_ca.Visibility = Visibility.Hidden;
            ListaVeiculos_lv.Focus();
        }


        #endregion

        #region Testes

        #endregion


        private void ListaVeiculos_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaVeiculos_lv.SelectedIndex == -1)
            {
                TabGeral_tc.IsEnabled = false;
                Editar_bt.IsEnabled = false;
                Excluir_bt.IsEnabled = false;

            }
            else
            {
                TabGeral_tc.IsEnabled = true;
                Editar_bt.IsEnabled = true;
                Excluir_bt.IsEnabled = true;
                //AbrirArquivoAnexo_bt.IsHitTestVisible = true;
            }
        }

        object _removed;

        private void TabGeral_tc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!ListaVeiculos_sp.IsEnabled)
                {
                    if (((object[])e.AddedItems)[0] != _removed)
                    {
                        if (e.RemovedItems.Count > 0)
                        {
                            Dispatcher.BeginInvoke((Action)(() => TabGeral_tc.SelectedItem = ((object[])e.RemovedItems)[0]));
                            _removed = ((object[])e.RemovedItems)[0];
                        }
                    }
                }

            }
            catch
            {

            }
        }
    }
}
