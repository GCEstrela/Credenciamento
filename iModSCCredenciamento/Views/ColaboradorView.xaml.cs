using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.ViewModels;
using iModSCCredenciamento.Windows;
using IMOD.CrossCutting;
using Microsoft.Win32;
using System.Data;
//using IMOD.Application.Service;

namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interação lógica para ColaboradorView.xam
    /// </summary>
    public partial class ColaboradorView : UserControl
    {
        DataRowView CompRow;
        int SComp;
        long CompID;
        public ColaboradorView()
        {
            InitializeComponent();
            DataContext = new ColaboradorViewModel();
            
        }



        #region Comando dos Botoes
        private void SelecionarFoto_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Imagem files (*.jpg)|*.jpg|All Files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if (openFileDialog.ShowDialog() == true)
                {
                    long tamanho = new FileInfo(openFileDialog.FileName).Length;
                    if (tamanho > 200)
                    {
                        MessageBox.Show("Tamanho ( " + tamanho + " ) inválido, só é permitido arquivo com o máximo de 200");
                        return;
                    }

                    BitmapImage _img = new BitmapImage(new Uri(openFileDialog.FileName));

                    string _imgstr = Conversores.IMGtoSTR(_img);

                    var fileLength = new FileInfo(openFileDialog.FileName).Length; //limitar o tamanho futuro

                    Foto_im.Source = _img;
                    ((ClasseColaboradores.Colaborador)ListaColaboradores_lv.SelectedItem).Foto = _imgstr; //Conversores.IMGtoSTR(new BitmapImage(new Uri(arquivoLogo.FileName)));
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
                    ((ClasseColaboradores.Colaborador)ListaColaboradores_lv.SelectedItem).Foto = _imgstr;

                }
            }
            catch (Exception ex)
            {

            }
        }


        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            ((ColaboradorViewModel)DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            VinculoEmpresa_ti.Visibility = Visibility.Hidden;
            Cursos_ti.Visibility = Visibility.Hidden;
            Anexos_ti.Visibility = Visibility.Hidden;
            Credenciais_ti.Visibility = Visibility.Hidden;

            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaColaboradores_lv.IsHitTestVisible = false;
            Geral_sp.IsHitTestVisible = true;
            ((ColaboradorViewModel)DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            VinculoEmpresa_ti.Visibility = Visibility.Hidden;
            Cursos_ti.Visibility = Visibility.Hidden;
            Anexos_ti.Visibility = Visibility.Hidden;
            Credenciais_ti.Visibility = Visibility.Hidden;

            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Geral_sp.IsHitTestVisible = true;
            Geral_bt.Visibility = Visibility.Hidden;
            ((ColaboradorViewModel)DataContext).OnAdicionarCommand();

        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((ColaboradorViewModel)DataContext).OnExcluirCommand();
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
            ListaColaboradores_lv.IsHitTestVisible = true;
            Geral_sp.IsHitTestVisible = false;
            ((ColaboradorViewModel)DataContext).OnCancelarEdicaoCommand();

            VinculoEmpresa_ti.Visibility = Visibility.Visible;
            Cursos_ti.Visibility = Visibility.Visible;
            Anexos_ti.Visibility = Visibility.Visible;
            Credenciais_ti.Visibility = Visibility.Visible;
        }

        private void OnSalvarEdicao2_Click(object sender, RoutedEventArgs e)
        {
            //if (CPF_tb.Text.Length == 0)
            //{
            //    Global.PopupBox("Insira o CPF!", 4);
            //    CPF_tb.Focus();
            //    return;
            //}
            //if (Nome_tb.Text.Length == 0)
            //{
            //    Global.PopupBox("Insira um Nome!", 4);
            //    Nome_tb.Focus();
            //    return;
            //}

            try
            {
                CheckCPF();

                if (Validation.GetHasError(CPF_tb))
                {
                    Global.PopupBox("Corrija o Campo CPF!", 4);
                    CPF_tb.Focus();
                    return;
                }
                var model = ((ColaboradorViewModel)DataContext);
                var entity = model.ColaboradorSelecionado;
                model.ValidarEdicao(entity);

                if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
                {
                    return;
                }

                Botoes_Principais_sp.Visibility = Visibility.Visible;
                model.SalvarEdicao();
                //model.OnSalvarEdicaoCommand2();
                //((ColaboradorViewModel)this.DataContext).OnSalvarEdicaoCommandAsync();
                Botoes_Editar_sp.Visibility = Visibility.Hidden;
                ListaColaboradores_lv.IsHitTestVisible = true;
                Geral_sp.IsHitTestVisible = false;

                VinculoEmpresa_ti.Visibility = Visibility.Visible;
                Cursos_ti.Visibility = Visibility.Visible;
                Anexos_ti.Visibility = Visibility.Visible;
                Credenciais_ti.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {

                Global.PopupBox(ex.Message, 4);
            }


        }
        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            //if (CPF_tb.Text.Length == 0)
            //{
            //    Global.PopupBox("Insira o CPF!", 4);
            //    CPF_tb.Focus();
            //    return;
            //}
            //if (Nome_tb.Text.Length == 0)
            //{
            //    Global.PopupBox("Insira um Nome!", 4);
            //    Nome_tb.Focus();
            //    return;
            //}

            try
            {
                CheckCPF();

                if (Validation.GetHasError(CPF_tb))
                {
                    Global.PopupBox("Corrija o Campo CPF!", 4);
                    CPF_tb.Focus();
                    return;
                }
                var model = ((ColaboradorViewModel)DataContext);
                var entity = model.ColaboradorSelecionado;
                model.ValidarEdicao(entity);

                if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
                {
                    return;
                }

                Botoes_Principais_sp.Visibility = Visibility.Visible;
                model.SalvarEdicao();
                //((ColaboradorViewModel)this.DataContext).OnSalvarEdicaoCommandAsync();
                Botoes_Editar_sp.Visibility = Visibility.Hidden;
                ListaColaboradores_lv.IsHitTestVisible = true;
                Geral_sp.IsHitTestVisible = false;

                VinculoEmpresa_ti.Visibility = Visibility.Visible;
                Cursos_ti.Visibility = Visibility.Visible;
                Anexos_ti.Visibility = Visibility.Visible;
                Credenciais_ti.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {

                Global.PopupBox(ex.Message, 4);
            }


        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((ColaboradorViewModel)DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Geral_sp.IsHitTestVisible = false;

            VinculoEmpresa_ti.Visibility = Visibility.Visible;
            Cursos_ti.Visibility = Visibility.Visible;
            Anexos_ti.Visibility = Visibility.Visible;
            Credenciais_ti.Visibility = Visibility.Visible;
            Geral_bt.Visibility = Visibility.Visible;

        }

        //private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        //{

        //    if (CPF_tb.Text.Length == 0)
        //    {
        //        Global.PopupBox("Insira o CPF!", 4);
        //        CPF_tb.Focus();
        //        return;
        //    }
        //    if (Nome_tb.Text.Length == 0)
        //    {
        //        Global.PopupBox("Insira um Nome!", 4);
        //        Nome_tb.Focus();
        //        return;
        //    }
        //    if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
        //    {
        //        return;
        //    }

        //    Botoes_Principais_sp.Visibility = Visibility.Visible;
        //    ((ColaboradorViewModel)this.DataContext).SalvarAdicao();
        //    //((ColaboradorViewModel)this.DataContext).OnSalvarAdicaoCommandAsync();
        //    Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
        //    Geral_sp.IsHitTestVisible = false;

        //    VinculoEmpresa_ti.Visibility = Visibility.Visible;
        //    Cursos_ti.Visibility = Visibility.Visible;
        //    Anexos_ti.Visibility = Visibility.Visible;
        //    Credenciais_ti.Visibility = Visibility.Visible;
        //    Geral_bt.Visibility = Visibility.Visible;
        //}
        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            #region Opcao 1
            try
            {
                var model = ((ColaboradorViewModel)DataContext);
                var entity = model.ColaboradorSelecionado;
                model.ValidarAdicao(entity);

                Botoes_Principais_sp.Visibility = Visibility.Visible;
                model.SalvarAdicao();
                //model.SalvarAdicao2();
                Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
                Geral_sp.IsHitTestVisible = false;
                VinculoEmpresa_ti.Visibility = Visibility.Visible;
                Cursos_ti.Visibility = Visibility.Visible;
                Anexos_ti.Visibility = Visibility.Visible;
                Credenciais_ti.Visibility = Visibility.Visible;
                Geral_bt.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Global.PopupBox(ex.Message, 4);
            }
            #endregion

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((ColaboradorViewModel)DataContext).OnAbrirPendencias(sender, e);
        }
        #endregion

        #region Metodos Privados
        private void OnTabSelected(object sender, RoutedEventArgs e)
        {
            Thickness marginThickness = ListaColaboradores_lv.Margin;
            ListaColaboradores_sp.Margin = new Thickness(marginThickness.Left, marginThickness.Top, 170, marginThickness.Bottom);
            Botoes_ca.Visibility = Visibility.Visible;
            ListaColaboradores_lv.Focus();
        }

        private void OnTabUnSelected(object sender, RoutedEventArgs e)
        {
            Thickness marginThickness = ListaColaboradores_lv.Margin;
            ListaColaboradores_sp.Margin = new Thickness(marginThickness.Left, marginThickness.Top, 0, marginThickness.Bottom);
            Botoes_ca.Visibility = Visibility.Hidden;
            ListaColaboradores_lv.Focus();
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        #endregion

        #region Testes

        #endregion

        private void selecionar(object sender, RoutedEventArgs e)
        {
            ListaColaboradores_lv.Items.Refresh();

            BindingExpression be1 = BindingOperations.GetBindingExpression(Apelido_tb, TextBox.TextProperty);
            if (be1 != null)
            {
                be1.UpdateSource();
            }
        }

        private void CNHValidade_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender, false);
        }

        private void DataNascimento_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender, false);
        }

        private void PassaporteValidade_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender, false);
        }

        private void RGEmissao_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Global.CheckField(sender, false);
        }

        //private void CPF_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    CPF_tb.Text = CPF_tb.Text.FormatarCpf();
        //    Global.CheckField(sender,true,"","CPF");
        //}
        private void CheckCPF()
        {

            var cpfAnterior = Global.CpfEdicao.RetirarCaracteresEspeciais();
            var cpfjAtual = CPF_tb.Text.RetirarCaracteresEspeciais();

            var model = (ColaboradorViewModel)DataContext;

            if (!Utils.IsValidCpf(cpfjAtual)) { throw new InvalidOperationException("CPF inválido!"); }

            if (cpfAnterior == "000.000.000-00") //Então a operação é de adição, logo verificar se ha CNPJ apenas no ação de salvar...
            {
                var c1 = ((ColaboradorViewModel)DataContext).ConsultarCpf(cpfjAtual);
                //if (c1) Global.PopupBox("CPF já cadastrado, impossível inclusão!", 4);
                if (c1) throw new InvalidOperationException("CPF já cadastrado, impossível inclusão!");
            }
            else if (cpfAnterior.CompareTo(cpfjAtual) != 0 && !string.IsNullOrWhiteSpace(cpfAnterior))
            {
                //Então verificar se há cnpj exisitente
                //Verificar se existe
                var c1 = ((ColaboradorViewModel)DataContext).ConsultarCpf(cpfjAtual);
                if (c1) throw new InvalidOperationException("CPF já cadastrado, impossível edição!");
                //if (c1) Global.PopupBox("CPF já cadastrado, impossível edição!", 4);
            }
        }

        private void OnConsultarCpf_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckCPF();
                var cpfjAtual = CPF_tb.Text.RetirarCaracteresEspeciais();
                CPF_tb.Text = cpfjAtual.FormatarCpf();
            }
            catch (Exception ex)
            {


                Global.PopupBox(ex.Message, 4);
            }

        }

        private void ListaColaboradores_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaColaboradores_lv.SelectedIndex == -1)
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
            }
        }

        object _removed;

        private void TabGeral_tc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!ListaColaboradores_sp.IsEnabled)
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

        private void Motorista_cb_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void OnSalvarEdicao_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Estado_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        //private void VinculoEmpresa_ti_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    Dispatcher.BeginInvoke((Action)(() => TabGeral_tc.SelectedItem = VinculoEmpresa_ti));

        //}
    }
}
