// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Windows;
using System.Windows.Controls;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels;
using IMOD.CrossCutting;

#endregion

namespace iModSCCredenciamento.Views
{
    /// <summary>
    ///     Interação lógica para EmpresasSegnatariosView.xam
    /// </summary>
    public partial class EmpresasSignatariosView : UserControl
    {
        private readonly EmpresasSignatariosViewModel _viewModel;

        public EmpresasSignatariosView()
        {
            InitializeComponent();
            _viewModel = new EmpresasSignatariosViewModel();
            DataContext = _viewModel;
        }

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.EmpresaView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados(entity);
        }

        /// <summary>
        ///     UpLoad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _viewModel.Entity.Assinatura = arq.FormatoBase64;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///     Downlaod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAbrirArquivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var arquivoStr = _viewModel.Entity.Assinatura;
                Global.PopupPDF(arquivoStr);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void FormatCnpj_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.Entity == null) return;
                txtCpf.Text = _viewModel.Entity.Cpf.FormatarCpf();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox($"Não foi realizar a operação solicitada\n{ex.Message}", 3);
            }
        }

        #endregion
    }
}

//private void CPF_tb_LostFocus(object sender, RoutedEventArgs e)
//{
//    //CPF_tb.Text = CPF_tb.Text.FormatarCpf();
//    //Global.CheckField(sender, true, "", "CPF");
//}

//static int _empresaIDFisrt;
//public int EmpresaSelecionadaIDView
//{
//    get { return (int)GetValue(EmpresaSelecionadaIDViewProperty); }
//    set { SetValue(EmpresaSelecionadaIDViewProperty, value); }
//}

///// <summary>
///// Empresa
///// </summary>
//public EmpresaView Empresa
//{
//    get { return (EmpresaView)GetValue(EmpresaProperty); }
//    set { SetValue(EmpresaProperty, value); }
//}

//public static readonly DependencyProperty
//    EmpresaProperty = DependencyProperty.Register("Empresa",
//        typeof(object),
//        typeof(EmpresasSignatariosView), new PropertyMetadata(0, PropertyChanged));

//private void ListaSegnatarios_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
//{
//    //if (ListaSegnatarios_lv.SelectedIndex == -1)
//    //{
//    //    Linha0_sp.IsEnabled = false;
//    //    Editar_bt.IsEnabled = false;
//    //}
//    //else
//    //{
//    //    Linha0_sp.IsEnabled = true;
//    //    Editar_bt.IsEnabled = true;
//    //    AbrirArquivo_bt.IsHitTestVisible = true;
//    //}
//}

//public static readonly DependencyProperty EmpresaSelecionadaIDViewProperty =
//    DependencyProperty.Register("EmpresaSelecionadaIDView", typeof(int), typeof(EmpresasSignatariosView), new PropertyMetadata(0, PropertyChanged));
//private static void PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
//{
//    var entity = source.GetValue (EmpresaProperty);

//    // _viewModel.ObterDados (Empresa);
//    //int _empresaID = Convert.ToInt32(e.NewValue);
//    //if (_empresaID != _empresaIDFisrt && _empresaID != 0)
//    //{
//    //    ((EmpresasSignatariosViewModel)((FrameworkElement)source).DataContext).OnAtualizaCommand(_empresaID);
//    //_empresaIDFisrt = _empresaID;
//    // }
//}

//public bool Editando
//{
//    get { return (bool)GetValue(EditandoProperty); }
//    set { SetValue(EditandoProperty, value); }
//}

//public static readonly DependencyProperty EditandoProperty =
//    DependencyProperty.Register("Editando", typeof(bool), typeof(EmpresasSignatariosView), new FrameworkPropertyMetadata(true,
//                                                   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EditandoPropertyChanged));

//private static void EditandoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//{
//    //throw new NotImplementedException();
//}

//#region Comando dos Botoes
////private void SelecionarAssinatura_bt_Click(object sender, RoutedEventArgs e)
////{
////    try
////    {

////        Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
////        openFileDialog.Multiselect = false;
////        openFileDialog.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";

////        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
////        if (openFileDialog.ShowDialog() == true)
////        {
////            BitmapImage _img = new BitmapImage(new Uri(openFileDialog.FileName));

////            string _imgstr = Conversores.IMGtoSTR(_img);
////            Assinatura_im.Source = _img;
////            ((ClasseEmpresasSignatarios.EmpresaSignatario)ListaSegnatarios_lv.SelectedItem).Assinatura = _imgstr; 

////        }

////    }
////    catch (Exception ex)
////    {

////    }
////}

//private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
//{
//    // ((EmpresasSignatariosViewModel)DataContext).OnPesquisarCommand();
//}

//private void Editar_bt_Click(object sender, RoutedEventArgs e)
//{
//    //Editando = false;
//    //Botoes_Principais_sp.Visibility = Visibility.Hidden;
//    //Botoes_Editar_sp.Visibility = Visibility.Visible;
//    //ListaSegnatarios_lv.IsHitTestVisible = false;
//    //Global.SetReadonly(Linha0_sp, false);
//    //((EmpresasSignatariosViewModel)DataContext).OnEditarCommand();
//}

//private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
//{
//    //Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
//    //Botoes_Adicionar_sp.Visibility = Visibility.Visible;
//    //Global.SetReadonly(Linha0_sp, false);
//    //((EmpresasSignatariosViewModel)DataContext).OnAdicionarCommand();
//}

//private void Excluir_bt_Click(object sender, RoutedEventArgs e)
//{
//    //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
//    //((EmpresasSignatariosViewModel)DataContext).OnExcluirCommand();
//}

//private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
//{
//    //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
//    ////Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
//    //Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
//    ////((EmpresasContratosViewModel)this.DataContext).ExecutarPesquisaCommand();
//}

//private void CancelarPesquisa_bt_Click(object sender, RoutedEventArgs e)
//{
//    //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
//    //Botoes_Pesquisar_sp.Visibility = Visibility.Hidden;

//}

//private void CancelarEdicao_bt_Click(object sender, RoutedEventArgs e)
//{
//    //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
//    //Botoes_Editar_sp.Visibility = Visibility.Hidden;
//    //ListaSegnatarios_lv.IsHitTestVisible = true;
//    //Global.SetReadonly(Linha0_sp, true);
//    //((EmpresasSignatariosViewModel)DataContext).OnCancelarEdicaoCommand();
//}

//private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
//{

//    ////if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Signatário", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//    ////{
//    ////    return;
//    ////}
//    //if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
//    //{
//    //    return;
//    //}

//    //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
//    //((EmpresasSignatariosViewModel)DataContext).OnSalvarEdicaoCommand();
//    //Botoes_Editar_sp.Visibility = Visibility.Hidden;
//    //ListaSegnatarios_lv.IsHitTestVisible = true;
//    //Global.SetReadonly(Linha0_sp, true);
//}

//private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
//{
//    //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
//    //((EmpresasSignatariosViewModel)DataContext).OnCancelarAdicaoCommand();
//    //Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
//    //Global.SetReadonly(Linha0_sp, true);
//}

//private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
//{

//    ////if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja salvar?", "Salvar Signatário", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//    ////{
//    ////    return;
//    ////}
//    //if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
//    //{
//    //    return;
//    //}

//    //Editando = true;Botoes_Principais_sp.Visibility = Visibility.Visible;
//    //((EmpresasSignatariosViewModel)DataContext).OnSalvarAdicaoCommand();
//    //Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
//    //Global.SetReadonly(Linha0_sp, true);
//}

//#endregion