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
using iModSCCredenciamento.Helpers;
using IMOD.CrossCutting;
using UserControl = System.Windows.Controls.UserControl;


namespace iModSCCredenciamento.Views
{

    public partial class EmpresaView : UserControl
    {
        #region Inicializacao
        private bool _cnpjVerificar = false;
        public EmpresaView()
        {
            InitializeComponent();
            this.DataContext = new EmpresaViewModel();
        }

        #endregion

        #region Comando dos Botoes
        private void SelecionarLogo_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog (filtro);
                if (arq == null) return;
                ((ClasseEmpresas.Empresa) ListaEmpresas_lv.SelectedItem).Logo = arq.FormatoBase64;
                 BindingExpression be = BindingOperations.GetBindingExpression(Logo_im, Image.SourceProperty);
                 be.UpdateTarget();
            }
            catch (Exception ex)
            {
                    Utils.TraceException(ex);
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
            Signatarios_ti.Visibility = Visibility.Hidden;
            Contrato_ti.Visibility = Visibility.Hidden;
            Anexos_ti.Visibility = Visibility.Hidden;

            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Editar_sp.Visibility = Visibility.Visible;
            ListaEmpresas_lv.IsHitTestVisible = false;
            Geral_sp.IsHitTestVisible = true;
            ((EmpresaViewModel)this.DataContext).OnEditarCommand();
        }

        private void Adicionar_bt_Click(object sender, RoutedEventArgs e)
        {
            Signatarios_ti.Visibility = Visibility.Hidden;
            Contrato_ti.Visibility = Visibility.Hidden;
            Anexos_ti.Visibility = Visibility.Hidden;
            Caracteristicas_gb.Visibility = Visibility.Hidden;

            Botoes_Principais_sp.Visibility = Visibility.Hidden;
            Botoes_Adicionar_sp.Visibility = Visibility.Visible;
            Geral_sp.IsHitTestVisible = true;
            Geral_bt.Visibility = Visibility.Hidden;
            ((EmpresaViewModel)this.DataContext).OnAdicionarCommand();
        }

        private void Excluir_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresaViewModel)this.DataContext).OnExcluirCommand();
        }

        private void ExecutarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Principais_sp.Visibility = Visibility.Hidden;
        }

        private void CancelarPesquisa_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Pesquisar_sp.Visibility = Visibility.Hidden;
            Geral_sp.IsHitTestVisible = false;
        }

        private void CancelarEdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            Botoes_Editar_sp.Visibility = Visibility.Hidden;
            ListaEmpresas_lv.IsHitTestVisible = true;

            ((EmpresaViewModel)this.DataContext).OnCancelarEdicaoCommand();
            Geral_sp.IsHitTestVisible = false;

            Signatarios_ti.Visibility = Visibility.Visible;
            Contrato_ti.Visibility = Visibility.Visible;
            Anexos_ti.Visibility = Visibility.Visible;
            Caracteristicas_gb.Visibility = Visibility.Visible;
        }

        private void SalvarEdicao_bt_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                Check();
                if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
                {
                    return;
                }
                if (_cnpjVerificar)
                {
                    if (((EmpresaViewModel)this.DataContext).ConsultaCNPJ(CNPJ_tb.Text))
                    {
                        if (Global.PopupBox("CNPJ já cadastrado, impossível alteração!", 4))
                        {
                            return;
                        }

                    }
                }
                Botoes_Principais_sp.Visibility = Visibility.Visible;
                //Execute the command
                ((EmpresaViewModel)this.DataContext).OnSalvarEdicaoCommand();
                Botoes_Editar_sp.Visibility = Visibility.Hidden;
                ListaEmpresas_lv.IsHitTestVisible = true;
                Geral_sp.IsHitTestVisible = false;

                Signatarios_ti.Visibility = Visibility.Visible;
                Contrato_ti.Visibility = Visibility.Visible;
                Anexos_ti.Visibility = Visibility.Visible;
                Caracteristicas_gb.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {

                Global.PopupBox(ex.Message, 4);
            }


        }

        private void CancelarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {
            Botoes_Principais_sp.Visibility = Visibility.Visible;
            ((EmpresaViewModel)this.DataContext).OnCancelarAdicaoCommand();
            Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
            Geral_sp.IsHitTestVisible = false;

            Signatarios_ti.Visibility = Visibility.Visible;
            Contrato_ti.Visibility = Visibility.Visible;
            Anexos_ti.Visibility = Visibility.Visible;
            Caracteristicas_gb.Visibility = Visibility.Visible;
            Geral_bt.Visibility = Visibility.Visible;
        }
        private void SalvarAdicao_bt_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!Global.PopupBox("Tem certeza que deseja salvar?", 2))
                {
                    return;
                }
                var model = (EmpresaViewModel)this.DataContext;
                var entity = model.EmpresaSelecionada;
                model.ValidarAdicao(entity);

                Botoes_Principais_sp.Visibility = Visibility.Visible;
                ((EmpresaViewModel)this.DataContext).OnSalvarAdicaoCommand();
                Botoes_Adicionar_sp.Visibility = Visibility.Hidden;
                Geral_sp.IsHitTestVisible = false;
                Signatarios_ti.Visibility = Visibility.Visible;
                Contrato_ti.Visibility = Visibility.Visible;
                Anexos_ti.Visibility = Visibility.Visible;
                Caracteristicas_gb.Visibility = Visibility.Visible;
                Geral_bt.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Global.PopupBox(ex.Message, 4);
            }


        }
        private void Check()
        {
            var cnpjAnterior = Global._cnpjEdicao.RetirarCaracteresEspeciais();
            var cnpjAtual = CNPJ_tb.Text.RetirarCaracteresEspeciais();
            if (!Utils.IsValidCnpj(cnpjAtual)) { throw new InvalidOperationException("CNPJ inválido!"); }
            
            if (cnpjAnterior == "00.000.000/0000-00") //Então a operação é de adição, logo verificar se ha CNPJ apenas no ação de salvar...
            {
                var c1 = ((EmpresaViewModel)this.DataContext).ConsultaCNPJ(cnpjAtual);
                if (c1) throw new InvalidOperationException("CNPJ já cadastrado, impossível inclusão!");
            }
            else if (cnpjAnterior.CompareTo(cnpjAtual) != 0 && !string.IsNullOrWhiteSpace(cnpjAnterior))
            {
                //Então verificar se há cnpj exisitente
                //Verificar se existe
                var c1 = ((EmpresaViewModel)this.DataContext).ConsultaCNPJ(cnpjAtual);
                if (c1) throw new InvalidOperationException("CNPJ já cadastrado, impossível Edição!");
            }
        }

        private void OnConsultarCnpj_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Check();
                var cnpjAtual = CNPJ_tb.Text.RetirarCaracteresEspeciais();
                CNPJ_tb.Text = cnpjAtual.FormatarCnpj();
            }
            catch (Exception ex)
            {
                Global.PopupBox(ex.Message, 4);
            }



        }
       
        private void IncluirAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            if (TipoAtividade_cb.Text != "" & TipoAtividade_cb.Text != "N/D")
            {

                ((EmpresaViewModel)this.DataContext).OnInserirAtividadeCommand(TipoAtividade_cb.SelectedValue.ToString(), TipoAtividade_cb.Text);
                //TipoAtividade_cb.SelectedIndex = 0;
                TipoAtividade_cb.Text = "";

            }

        }

        private void ExcluirAtividade_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresaViewModel)this.DataContext).OnExcluirAtividadeCommand();
        }

        private void ExcluirAcesso_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresaViewModel)this.DataContext).OnExcluirAcessoCommand();
        }

        private void IncluirCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            if (Cracha_cb.Text != "" & Cracha_cb.Text != "N/D")
            {

                ((EmpresaViewModel)this.DataContext).OnInserirCrachaCommand(Convert.ToInt32(Cracha_cb.SelectedValue), Cracha_cb.Text);
                //Cracha_cb.SelectedIndex = 0;
                Cracha_cb.Text = "";
            }
        }

        private void ExcluirCracha_bt_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresaViewModel)this.DataContext).OnExcluirCrachaCommand();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((EmpresaViewModel)this.DataContext).OnAbrirPendencias(sender, e);
        }
        #endregion

        #region Metodos Privados
        private void OnTabSelected(object sender, RoutedEventArgs e)
        {
            Thickness marginThickness = ListaEmpresas_sp.Margin;
            ListaEmpresas_sp.Margin = new Thickness(marginThickness.Left, marginThickness.Top, 170, marginThickness.Bottom);
            Botoes_ca.Visibility = Visibility.Visible;
        }

        private void OnTabUnSelected(object sender, RoutedEventArgs e)
        {
            Thickness marginThickness = ListaEmpresas_sp.Margin;
            ListaEmpresas_sp.Margin = new Thickness(marginThickness.Left, marginThickness.Top, 0, marginThickness.Bottom);
            Botoes_ca.Visibility = Visibility.Hidden;
        }

        #endregion

        private void ListaEmpresas_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaEmpresas_lv.SelectedIndex == -1)
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
                if (!ListaEmpresas_sp.IsEnabled)
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //((EmpresaViewModel)this.DataContext).CarregaColecoesIniciais();
        }
    }

}
