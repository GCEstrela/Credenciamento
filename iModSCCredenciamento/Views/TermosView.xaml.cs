using iModSCCredenciamento.Windows;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interação lógica para TermosView.xam
    /// </summary>
    public partial class TermosView : UserControl
    {
        private PopupFiltrosTermos PopupFiltrosTermos;
        public TermosView()
        {
            InitializeComponent();
        }

        private void ButtonTermoConcessaoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //12_TermoConcessaoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(12,1);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonTermoIndeferimentoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //16_TermoIndeferimentoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(16,5);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonTermoCancelamentoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //14_TermoCancelamentoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(14,2);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonTermoDestruicaoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //18_TermoDestruicaoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(18,3);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonTermoViasAdicionaisCredenciaisClick(object sender, RoutedEventArgs e)
        {
            PopupFiltrosTermos = new PopupFiltrosTermos(20,1);
            PopupFiltrosTermos.ShowDialog();
        }
    }
}
