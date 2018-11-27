using Microsoft.Win32;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.ViewModels;
using System;
using System.Collections.Generic;
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
using iModSCCredenciamento.Views.Comportamento;
using UserControl = System.Windows.Controls.UserControl;


namespace iModSCCredenciamento.Views
{

    public partial class EmpresaView : UserControl,IConfiguracaoBotaoBasico
    {
        #region Inicializacao
        private bool _cnpjVerificar = false;
        public EmpresaView()
        {
            InitializeComponent();
            this.DataContext = new EmpresaViewModel();
            //this.PreviewKeyDown += (ss, ee) =>
            //{
            //    if (ee.Key == Key.Escape)
            //    {
            //        //System.Windows.Forms.MessageBox.Show(ee.Key.ToString());
            //        Global._escape = true;
            //    }
            //};
        }

        #endregion

        #region Comando dos Botoes
        private void SelecionarLogo_bt_Click(object sender, RoutedEventArgs e)
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
                    //Logo_im.Source = _img;
                    ((ClasseEmpresas.Empresa)ListaEmpresas_lv.SelectedItem).Logo = _imgstr; //Conversores.IMGtoSTR(new BitmapImage(new Uri(arquivoLogo.FileName)));
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

        private void Pesquisar_bt_Click(object sender, RoutedEventArgs e)
        {
            //Editando = false; Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //Botoes_Pesquisar_sp.Visibility = Visibility.Visible;
            ((EmpresaViewModel)this.DataContext).OnPesquisarCommand();
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            //Signatarios_ti.Visibility = Visibility.Hidden;
            //Contrato_ti.Visibility = Visibility.Hidden;
            //Anexos_ti.Visibility = Visibility.Hidden;

            //Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //Botoes_Editar_sp.Visibility = Visibility.Visible;
            //ListaEmpresas_lv.IsHitTestVisible = false;
            //Geral_sp.IsHitTestVisible = true;
            //((EmpresaViewModel)this.DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            //Signatarios_ti.Visibility = Visibility.Hidden;
            //Contrato_ti.Visibility = Visibility.Hidden;
            //Anexos_ti.Visibility = Visibility.Hidden;
            //Caracteristicas_gb.Visibility = Visibility.Hidden;

            //Botoes_Principais_sp.Visibility = Visibility.Hidden;
            //Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            //Geral_sp.IsHitTestVisible = true;
            //Geral_bt.Visibility = Visibility.Hidden;
            //((EmpresaViewModel)this.DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
           //Botoes_Principais_sp.Visibility = Visibility.Visible;
           // ((EmpresaViewModel)this.DataContext).OnExcluirCommand();
        }

        private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            //Botoes_Principais_sp.Visibility = Visibility.Visible;
            ////Criterios_tb.Text = PesquisaCodigo_tb.Text + (char)(20) + PesquisaNome_tb.Text + (char)(20) + PesquisaCNPJ_tb.Text;
            //Botoes_Principais_sp.Visibility = Visibility.Hidden;
            ////((EmpresaViewModel)this.DataContext).ExecutarPesquisaCommand();
        }

        private void CancelarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            //Botoes_Principais_sp.Visibility = Visibility.Visible;
            //Botoes_Pesquisar_sp.Visibility = Visibility.Hidden;
            //Geral_sp.IsHitTestVisible = false;
        }

        private void CancelarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            //Botoes_Principais_sp.Visibility = Visibility.Visible;
            //Botoes_Editar_sp.Visibility = Visibility.Hidden;
            //ListaEmpresas_lv.IsHitTestVisible = true;

            //((EmpresaViewModel)this.DataContext).OnCancelarEdicaoCommand();
            //Geral_sp.IsHitTestVisible = false;

            //Signatarios_ti.Visibility = Visibility.Visible;
            //Contrato_ti.Visibility = Visibility.Visible;
            //Anexos_ti.Visibility = Visibility.Visible;
            //Caracteristicas_gb.Visibility = Visibility.Visible;
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //if (CNPJ_tb.Text.Length == 0)
            //{
            //    Global.PopupBox("Insira o CNPJ!", 4);
            //    CNPJ_tb.Focus();
            //    return;
            //}
            //if (Nome_tb.Text.Length == 0)
            //{
            //    Global.PopupBox("Insira a Razão Social!", 4);
            //    Nome_tb.Focus();
            //    return;
            //}
            //try
            //{
            //    Check();
            //    if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            //    {
            //        return;
            //    }
            //    if (_cnpjVerificar)
            //    {
            //        if (((EmpresaViewModel)this.DataContext).ConsultaCNPJ(CNPJ_tb.Text))
            //        {
            //            //if (System.Windows.Forms.MessageBox.Show("CNPJ já cadastrado, confirma alteração do registro?", "Informação", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            //            //{
            //            //    return;
            //            //}
            //            if (Global.PopupBox("CNPJ já cadastrado, impossível alteração!", 4))
            //            {
            //                return;
            //            }

            //        }
            //    }
                //Botoes_Principais_sp.Visibility = Visibility.Visible;
                //////Optional - first test if the DataContext is not a MyViewModel
                ////if (!(DataContext is EmpresaViewModel)) return;
                //////Optional - check the CanExecute
                ////if (!((EmpresaViewModel)this.DataContext).Botao5Command.CanExecute(null)) return;
                //Execute the command
                //((EmpresaViewModel)this.DataContext).OnSalvarEdicaoCommand();
                //Botoes_Editar_sp.Visibility = Visibility.Hidden;
                //ListaEmpresas_lv.IsHitTestVisible = true;
                //Geral_sp.IsHitTestVisible = false;

                //Signatarios_ti.Visibility = Visibility.Visible;
                //Contrato_ti.Visibility = Visibility.Visible;
                //Anexos_ti.Visibility = Visibility.Visible;
                //Caracteristicas_gb.Visibility = Visibility.Visible;
            //}
            //catch (Exception ex)
            //{

            //    Global.PopupBox(ex.Message, 4);
            //}


        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            //Botoes_Principais_sp.Visibility = Visibility.Visible;
            //((EmpresaViewModel)this.DataContext).OnCancelarAdicaoCommand();
            //Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            //Geral_sp.IsHitTestVisible = false;

            //Signatarios_ti.Visibility = Visibility.Visible;
            //Contrato_ti.Visibility = Visibility.Visible;
            //Anexos_ti.Visibility = Visibility.Visible;
            //Caracteristicas_gb.Visibility = Visibility.Visible;
            //Geral_bt.Visibility = Visibility.Visible;
        }
        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            //try
            //{
            //    if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
            //    {
            //        return;
            //    }
            //    var model = (EmpresaViewModel)this.DataContext;
            //    var entity = model.EmpresaSelecionada;
            //    model.ValidarAdicao(entity);

            //    Botoes_Principais_sp.Visibility = Visibility.Visible;
            //    ((EmpresaViewModel)this.DataContext).OnSalvarAdicaoCommand();
            //    Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            //    Geral_sp.IsHitTestVisible = false;
            //    Signatarios_ti.Visibility = Visibility.Visible;
            //    Contrato_ti.Visibility = Visibility.Visible;
            //    Anexos_ti.Visibility = Visibility.Visible;
            //    Caracteristicas_gb.Visibility = Visibility.Visible;
            //    Geral_bt.Visibility = Visibility.Visible;
            //}
            //catch (Exception ex)
            //{
            //    Global.PopupBox(ex.Message, 4);
            //}


        }
        private void Check()
        {
           // var cnpjAnterior = Global._cnpjEdicao.RetirarCaracteresEspeciais();
           // var cnpjAtual = CNPJ_tb.Text.RetirarCaracteresEspeciais();
           // if (!cnpjAtual.IsValidCnpj()) { throw new InvalidOperationException("CNPJ inválido!"); }
           
           // //if (string.IsNullOrWhiteSpace(cnpjAnterior)) //Então a operação é de adição, logo verificar se ha CNPJ apenas no ação de salvar...
           //if (cnpjAnterior == "00.000.000/0000-00") //Então a operação é de adição, logo verificar se ha CNPJ apenas no ação de salvar...
           //{
           //     var c1 = ((EmpresaViewModel)this.DataContext).ConsultaCNPJ(cnpjAtual);
           //     if (c1) throw new InvalidOperationException("CNPJ já cadastrado, impossível inclusão!");
           // }
           // else if (cnpjAnterior.CompareTo(cnpjAtual) != 0 && !string.IsNullOrWhiteSpace(cnpjAnterior))
           // {
           //     //Então verificar se há cnpj exisitente
           //     //Verificar se existe
           //     var c1 = ((EmpresaViewModel)this.DataContext).ConsultaCNPJ(cnpjAtual);
           //     if (c1) throw new InvalidOperationException("CNPJ já cadastrado, impossível Edição!");
           // }
        }
        private void OnConsultarCnpj_LostFocus(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Check();
            //    var cnpjAtual = CNPJ_tb.Text.RetirarCaracteresEspeciais();
            //    CNPJ_tb.Text = cnpjAtual.FormatarCnpj();
            //}
            //catch (Exception ex)
            //{

            //    Global.PopupBox(ex.Message, 4);
            //}



        }
        //private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        //{
        //    //if (CNPJ_tb.Text.Length == 0)
        //    //{
        //    //    Global.PopupBox("Insira o CNPJ!", 4);
        //    //    CNPJ_tb.Focus();
        //    //    return;
        //    //}
        //    //if (Nome_tb.Text.Length == 0)
        //    //{
        //    //    Global.PopupBox("Insira a Razão Social!", 4);
        //    //    Nome_tb.Focus();
        //    //    return;
        //    //}


        //    if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
        //    {
        //        return;
        //    }

        //    if (((EmpresaViewModel)this.DataContext).ConsultaCNPJ(CNPJ_tb.Text))
        //    {
        //        //if (System.Windows.Forms.MessageBox.Show("CNPJ já cadastrado, confirma alteração do registro?", "Informação", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
        //        //{
        //        //    return;
        //        //}
        //        if (Global.PopupBox("CNPJ já cadastrado, impossível inclusão!", 4))
        //        {
        //            return;
        //        }

        //    }
        //    Botoes_Principais_sp.Visibility = Visibility.Visible;
        //    ((EmpresaViewModel)this.DataContext).OnSalvarAdicaoCommand();
        //    Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
        //    Geral_sp.IsHitTestVisible = false;

        //    Signatarios_ti.Visibility = Visibility.Visible;
        //    Contrato_ti.Visibility = Visibility.Visible;
        //    Anexos_ti.Visibility = Visibility.Visible;
        //    Caracteristicas_gb.Visibility = Visibility.Visible;
        //    Geral_bt.Visibility = Visibility.Visible;
        //}

        private void IncluirAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            //if (TipoAtividade_cb.Text != "" & TipoAtividade_cb.Text != "N/D")
            //{

            //    ((EmpresaViewModel)this.DataContext).OnInserirAtividadeCommand(TipoAtividade_cb.SelectedValue.ToString(), TipoAtividade_cb.Text);
            //    //TipoAtividade_cb.SelectedIndex = 0;
            //    TipoAtividade_cb.Text = "";
            //}

        }

        private void ExcluirAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresaViewModel)this.DataContext).OnExcluirAtividadeCommand();
        }

        private void IncluirAcesso_bt_Click(object sender, RoutedEventArgs e)
        {
            //if (AreaAcesso_cb.Text != "" & AreaAcesso_cb.Text != "N/D")
            //{

            //    ((EmpresaViewModel)this.DataContext).OnInserirAcessoCommand(AreaAcesso_cb.SelectedItem);
            //    //((EmpresaViewModel)this.DataContext).OnInserirAcessoCommand(AreaAcesso_cb.SelectedValue.ToString(), AreaAcesso_cb.Text);
            //    //AreaAcesso_cb.SelectedIndex = 0;
            //    AreaAcesso_cb.Text = "";
            //}

        }

        private void ExcluirAcesso_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresaViewModel)this.DataContext).OnExcluirAcessoCommand();
        }

        private void IncluirCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            //if (Cracha_cb.Text != "" & Cracha_cb.Text != "N/D")
            //{

            //    ((EmpresaViewModel)this.DataContext).OnInserirCrachaCommand(Convert.ToInt32(Cracha_cb.SelectedValue), Cracha_cb.Text);
            //    //Cracha_cb.SelectedIndex = 0;
            //    Cracha_cb.Text = "";
            //}
        }

        private void ExcluirCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresaViewModel)this.DataContext).OnExcluirCrachaCommand();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresaViewModel)this.DataContext).OnAbrirPendencias( sender,  e);
        }
        #endregion

        #region Metodos Privados
        private void OnTabSelected(object sender, RoutedEventArgs e)
        {
            //Thickness marginThickness = ListaEmpresas_sp.Margin;
            //ListaEmpresas_sp.Margin = new Thickness(marginThickness.Left, marginThickness.Top, 170, marginThickness.Bottom);
            //Botoes_ca.Visibility = Visibility.Visible;
        }

        private void OnTabUnSelected(object sender, RoutedEventArgs e)
        {
            //Thickness marginThickness = ListaEmpresas_sp.Margin;
            //ListaEmpresas_sp.Margin = new Thickness(marginThickness.Left, marginThickness.Top, 0, marginThickness.Bottom);
            //Botoes_ca.Visibility = Visibility.Hidden;
        }

        #endregion

 

        private void CNPJ_tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //CNPJ_tb.Text = CNPJ_tb.Text.FormatarCnpj();

            //Global.CheckField(sender,true,"","CNPJ");
            //if (Global._cnpjEdicao == null)
            //{
            //    return;
            //}
            //if (Global._cnpjEdicao.ToString() != CNPJ_tb.Text.ToString().Trim())
            //{
            //    if (Global._cnpjEdicao.Length != 0)
            //    {
            //        _cnpjVerificar = true;
            //    }
            //    else
            //    {
            //        _cnpjVerificar = false;
            //    }

            //}
            //else
            //{
            //    _cnpjVerificar = false;
            //}
        }

        private void ListaEmpresas_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (ListaEmpresas_lv.SelectedIndex == -1)
            //{
            //    TabGeral_tc.IsEnabled = false;
            //    Editar_bt.IsEnabled = false;
            //    Excluir_bt.IsEnabled = false;
            //}
            //else
            //{
            //    TabGeral_tc.IsEnabled = true;
            //    Editar_bt.IsEnabled = true;
            //    Excluir_bt.IsEnabled = true;
            //}
        }

        object _removed;
        private void TabGeral_tc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //    if (!ListaEmpresas_sp.IsEnabled)
            //    {
            //        if (((object[])e.AddedItems)[0] != _removed)
            //        {
            //            if (e.RemovedItems.Count > 0)
            //            {
            //                Dispatcher.BeginInvoke((Action)(() => TabGeral_tc.SelectedItem = ((object[])e.RemovedItems)[0]));
            //                _removed = ((object[])e.RemovedItems)[0];
            //            }
            //        }
            //    }

            //}
            //catch
            //{

            //}
        }

        private void CNPJ_tb_LostFocus(object sender, RoutedEventArgs e)
        {

            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //((EmpresaViewModel)this.DataContext).CarregaColecoesIniciais();
        }





        //private void CNPJ_tb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    if (Global._cnpjEdicao.ToString() != CNPJ_tb.Text.ToString().Trim())
        //    {
        //        if (Global._cnpjEdicao.Length != 0)
        //        {
        //            _cnpjVerificar = true;
        //        }
        //        else
        //        {
        //            _cnpjVerificar = false;
        //        }

        //    }
        //    else
        //    {
        //        _cnpjVerificar = false;
        //    }
        //}
        /// <summary>
        /// Editar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEditar_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Adicionar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Excluir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnExcluir_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCancelar_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Salvar Edição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSalvarEditar(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Salvar Adição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSalvarAdicionar(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }

}
