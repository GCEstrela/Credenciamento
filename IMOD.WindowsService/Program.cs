using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMOD.WindowsService
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        static void Main()
        {
#if (!DEBUG)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new Service1()
                };
                ServiceBase.Run(ServicesToRun);
            }
#else

            Service1 service = new Service1();
            Timer _timer;

            // Chamada do seu método para Debug.
             service.MetodoRealizaFuncao(null);
            // _timer = new Timer(service.MetodoRealizaFuncao, null, 0, 30000);
            // System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#endif
        }
    }
    
}
