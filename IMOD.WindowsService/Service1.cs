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
using IMOD.Infra;
using IMOD.Domain.Enums;
using System.Threading;
using IMOD.Domain.Entities;
using Microsoft.Expression.Encoder;
using IMOD.Infra.Servicos;
using Genetec.Sdk;
using IMOD.Infra.Servicos.Entities;

namespace IMOD.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private IColaboradorCredencialService _serviceColaborador = new ColaboradorCredencialService();
        private IEmpresaContratosService _service = new EmpresaContratoService();
        private Timer _timer;
        static IEngine _sdk = Main.Engine;


        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(MetodoRealizaFuncao, null, 0, 120000);
        }

        /// <summary>
        /// Veifica os contratos da empresa que estão vencidos e alerta
        /// Verifica se a credencial do colaborador expirou e remove do genetec o cardholder
        /// </summary>
        /// <param name="state"></param>
        internal void MetodoRealizaFuncao(object state)
        {
            try
            {
                //CriarLog("serviço rodando:" + DateTime.Now);
                //var empresaContratos = _service.Listar().Where(ec => ec.StatusId == 0).ToList();

                var colaboradorContratos = _serviceColaborador.Listar(null, null, null, null, null, null, null).ToList();
                colaboradorContratos.ForEach(ec =>
                {
                    if (ec.Validade < DateTime.Now)
                    {
                        ec.Ativa = false;
                        ec.CredencialStatusId = (int)StatusCredencial.INATIVA;
                        ec.CredencialMotivoId = (int)Inativo.EXPIRADA;
                        _serviceColaborador.Alterar(ec);
                        CardHolderEntity entity = new CardHolderEntity();
                        entity.IdentificadorCardHolderGuid = ec.CardHolderGuid;
                        _serviceColaborador.RemoverRegrasCardHolder(new CredencialGenetecService(_sdk), entity);

                    }

                    string texto = "Impressa.:" + ec.Impressa + " Status.: " + ec.Ativa + " " + ec.ColaboradorNome + ((ec.Validade < DateTime.Now) ? " Vencido em : " : " Válido até : ") + ec.Validade;
                    CriarLog(string.Format("Contrato: {0}", texto));
                }
                );

                //var empresaContratos = _service.Listar().ToList();
                //empresaContratos.ForEach(ec =>
                //    {
                //        string texto = ec.Descricao + ((ec.Validade < DateTime.Now) ? " Vencido em : " : " Válido até : ") + ec.Validade;                        
                //        CriarLog(string.Format("Contrato: {0}" , texto));
                //        //Console.WriteLine(string.Format("Contrato: {0}", texto));
                //    }
                //);
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
