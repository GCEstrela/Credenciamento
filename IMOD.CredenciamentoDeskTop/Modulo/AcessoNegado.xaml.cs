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
    }
}
