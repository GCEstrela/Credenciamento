using Genetec.Sdk.Workspace;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
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
using System.Windows.Threading;

namespace IMOD.CredenciamentoDeskTop.Modulo
{
    /// <summary>
    /// Interação lógica para AcessoNegado.xam
    /// </summary>
    public partial class AcessoNegado : UserControl
    {
        int left = 0; // this is the left int variable with the value 0
        int speed = 5; // this integer called speed will determine how fast the blue circle will go
        public AcessoNegado(string dataExpirada)
        {
            InitializeComponent();
            lbl_DataExpirada.Content = dataExpirada;
            //var timer = new DispatcherTimer(); // creating a new timer
            //timer.Interval = TimeSpan.FromMilliseconds(10); // this timer will trigger every 10 milliseconds
            //timer.Start(); // starting the timer
            //timer.Tick += _timer_Tick; // with each tick it will trigger this function
        }
        void _timer_Tick(object sender, EventArgs e)
        {
            // this is the timer tick function

            left += speed; // the left will increase by the speed

            BlueCircle.Margin = new Thickness(left, 0, 0, 90);
            // we are adding the left integer values to the blue circles thickness it will push the object towards the right

            if (left > 443)
            {
                // if left is greater than 443 then we reverse the speed
                speed = -5;
            }
            if (left < 2)
            {
                // if the left is less than 2 then we reverse the speed
                speed = 5;
            }
        }
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private void BntSalvarLicenca_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var cf = _auxiliaresService.ConfiguraSistemaService.Listar().FirstOrDefault();
                cf.Licenca = this.txtLicenca.Text.Trim();
                if (ValidarLicenca(cf.Licenca))
                {
                    _auxiliaresService.ConfiguraSistemaService.Alterar(cf);

                    //bntSalvarLicenca.Foreground = new SolidColorBrush(Colors.Green);
                    //WpfHelp.PopupBox("Licença para o credenciamento foi enviada com êxito!", 1);
                    this.lblResposta.Content = "Sua licença foi validada com sucesso. Feche esta aba e acesse novamente!";
                    this.lblResposta.Foreground = new SolidColorBrush(Colors.Green);
                    new MenuPrincipalView();
                }
                else
                {
                    //bntSalvarLicenca.Foreground = new SolidColorBrush(Colors.Red);
                    //WpfHelp.PopupBox("Licença para o credenciamento inválida !", 1);
                    this.lblResposta.Content = "Licença para o credenciamento inválida!";
                }
            }
            catch (Exception ex)
            {
                WpfHelp.MboxError(ex);
            }
        }

        private MenuPrincipalView _view = new MenuPrincipalView();
        private ConfiguraSistema _configuraSistema;
        private bool ValidarLicenca(string licenca)
        {
            try
            {
                string myValue = licenca;
                _configuraSistema = ObterConfiguracao();
                if (!string.IsNullOrEmpty(licenca))
                {
                    myValue = licenca.Trim();
                }
                EstrelaEncryparDecrypitar.Variavel.key = "CREDENCIAMENTO2019";
                EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
                string[] Decryptada = ESTRELA_EMCRYPTAR.EstrelaDecrypt(myValue).Split('<');
                string LicencaDecryptada = Decryptada[0];
                if (Decryptada.Length > 1)
                {
                    DateTime DataExpiracaoLicencaDecryptada = Convert.ToDateTime(Decryptada[1]);
                    Double expiracao = DataExpiracaoLicencaDecryptada.Subtract(DateTime.Now.Date).Days;
                    if (expiracao < 15 && expiracao > 0)
                    {
                        WpfHelp.PopupBox(string.Format("Sua licença vai expirar em {0} dias", expiracao), 1);

                    }
                    else if (expiracao <= 0)
                    {
                        UsuarioLogado.LicencaValida = false;
                        //this.View = _configuracoesView;
                        return false;
                    }
                }

                if (UsuarioLogado.sdiLicenca != LicencaDecryptada)
                {
                    return false;
                }

                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresService.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }
    }
}
