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
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Modules;
using Genetec.Sdk.Workspace.Tasks;


using IMOD.Infra.Servicos.Entities;


namespace IMOD.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private IColaboradorCredencialService _serviceColaborador = new ColaboradorCredencialService();
        private IEmpresaContratosService _service = new EmpresaContratoService();
        private Timer _timer;

        IEngine _sdk = Main.Engine;

        //static IEngine _sdk = Main.Engine;

        //private Genetec.Sdk.IEngine _sdk = new Engine();

        //Obter das configuraçoes do sistema
        private const int diasAlerta = 0;
        private const int diasAlerta1 = 5;
        private const int diasAlerta2 = 15;
        private const int diasAlerta3 = 30;


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
                ///_sdk.BeginLogOn("172.16.190.108", "admin", "");

                CriarLog("serviço rodando:" + DateTime.Now);                

                var colaboradorContratos = _serviceColaborador.Listar(null, null, null, null, null, null, true).ToList();
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
                        //O _SDK está vindo nulo
                        //_serviceColaborador.RemoverRegrasCardHolder(new CredencialGenetecService(_sdk), entity);

                    }

                    string texto = "Impressa.:" + ec.Impressa + " Status.: " + ec.Ativa + " " + ec.ColaboradorNome + ((ec.Validade < DateTime.Now) ? " Vencido em : " : " Válido até : ") + ec.Validade;
                    CriarLog(string.Format("Contrato: {0}", texto));
                }
                );

                var empresaContratos = _service.Listar().Where(ec => ec.StatusId == 0).ToList();
                //var empresaContratos = _service.Listar().ToList();
                empresaContratos.ForEach(ec =>
                    {
                    string texto = ec.Descricao + ((ec.Validade < DateTime.Now) ? " - Vencido em : " : " - Válido até : ") + ec.Validade;
                    CriarLog(string.Format("Contrato: {0}", texto));
                        int dias = ec.Validade.CompareTo(DateTime.Now.Date);
                        switch (dias)
                        {
                            case diasAlerta:
                                CriarLog("Disparar alerta de vencendo hoje");
                                break;
                            case diasAlerta1:
                                CriarLog("Disparar alerta de vencendo em 5 dias");
                                break;
                            case diasAlerta2:
                                CriarLog("Disparar alerta de vencendo em 15 dias");
                                break;
                            case diasAlerta3:
                                CriarLog("Disparar alerta de vencendo em 30 dias");
                                break;
                            default:
                                break;
                        }
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
