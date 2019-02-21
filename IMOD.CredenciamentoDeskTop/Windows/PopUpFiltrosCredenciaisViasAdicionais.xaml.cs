using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Interaction logic for PopUpFiltrosCredenciaisViasAdicionais.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisViasAdicionais : Window
    {
        public PopUpFiltrosCredenciaisViasAdicionais()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciais(1);//Carregar os motivos do status 1 - ativo 
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
            int tipo = 0;
            IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView motivoCredencialSelecionado = null;
            string dataIni = dp_dataInicial.Text; 
            string dataFim = dp_dataFinal.Text; 

            if (lstMotivoCredencial.SelectedItem != null)
            {
                motivoCredencialSelecionado = (IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView)lstMotivoCredencial.SelectedItem;
                tipo = ((IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView)lstMotivoCredencial.SelectedItem).CredencialMotivoId;
            }

            ((RelatoriosViewModel)DataContext).OnFiltroCredencialViasAdicionaisCommand(tipo, dataIni, dataFim);

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
