using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using IMOD.Application.Service;
using IMOD.Application.Interfaces;
using System.Threading;

namespace IMOD.WindowsService
{
    public partial class Service1 : ServiceBase
    {

        private IEmpresaContratosService _service = new EmpresaContratoService();
        private Timer _timer;


        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(MetodoRealizaFuncao, null, 0, 120000);            
        }

        internal void MetodoRealizaFuncao(object state)
        {
            try
            {
                CriarLog("serviço rodando:" + DateTime.Now);
                //var empresaContratos = _service.Listar().Where(ec => ec.StatusId == 0).ToList();
                var empresaContratos = _service.Listar().ToList();
                empresaContratos.ForEach(ec =>
                    {
                        string texto = ec.Descricao + ((ec.Validade < DateTime.Now) ? " Vencido em : " : " Válido até : ") + ec.Validade;                        
                        CriarLog(string.Format("Contrato: {0}" , texto));
                        Console.WriteLine(string.Format("Contrato: {0}", texto));
                    }
                );
            }
            catch (Exception ex)
            {
                CriarLog(ex.Message);
            }
        }

        protected override void OnStop()
        {
            CriarLog("Finalizado");
        }

        private void CriarLog(object state)
        {
            StreamWriter vWriter = new StreamWriter(@"C:\Tmp\Logs\log.txt", true);            
            vWriter.WriteLine(state.ToString());
            vWriter.Flush();
            vWriter.Close();
        }
    }
}
