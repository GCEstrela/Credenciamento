using Common.Logging;
using Genetec.Sdk;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.Domain.Enums;
using IMOD.Infra.Servicos;
using IMOD.Infra.Servicos.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using System.Net.Mail;
using System.Net;
using System.Threading;

namespace Meu_Servico.Service
{

    class WinService
    {
        private ICredencialService _serviceGenetec;
        private IEmpresaService _serviceEmpresa = new EmpresaService();
        private IColaboradorCredencialService _serviceColaborador = new ColaboradorCredencialService();
        private IEmpresaContratosService _service = new EmpresaContratoService();
        
        private IEngine _sdk;

        public ILog Log { get; private set; }
        private Boolean logado;
        private const int diasAlerta = 0;
        private const int diasAlerta1 = 5;
        private const int diasAlerta2 = 15;
        private const int diasAlerta3 = 30;

        public WinService(ILog logger)
        {

            // IocModule.cs needs to be updated in case new paramteres are added to this constructor

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            Log = logger;

        }

        public bool Start(HostControl hostControl)
        {
            Genetec.Sdk.Engine _sdk = new Genetec.Sdk.Engine();
            Logon_SC_th(_sdk);

            _sdk.LoggedOn += _sdk_LoggedOn;
            _sdk.LoggedOff += _sdk_LoggedOff;
            _sdk.LogonFailed += _sdk_LogonFailed;
            _sdk.EventReceived += _sdk_EventReceived;
            _sdk.EntityInvalidated += _sdk_EntityInvalidated;



            _serviceGenetec = new CredencialGenetecService(_sdk);
            MetodoRealizaFuncao(true, _sdk);

            //Log.Info($"{nameof(Service.WinService)} Start command received.");

            //TODO: Implement your service start routine.
            return true;

        }
        private void Logon_SC_th(Genetec.Sdk.Engine _sdk)
        {
            //_logando = true;
            try
            {
                //_sdk.ClientCertificate = "y+BiIiYO5VxBax6/HNi7/ZcXWuvlnEemfaMhoQS1RMkfOGvEBWdUV7zQN272yHVG"
                _sdk.ClientCertificate = "KxsD11z743Hf5Gq9mv3+5ekxzemlCiUXkTFY5ba1NOGcLCmGstt2n0zYE9NsNimv";
                if (_sdk.IsConnected)
                {
                    _sdk.BeginLogOff();
                    Thread.Sleep(1000);
                    if (!_sdk.IsConnected)
                    {
                        _sdk.LogOn("172.16.190.108", "Admin", "");
                    }
                }
                else
                {
                    _sdk.LogOn("172.16.190.108", "Admin", "");

                }
            }
            catch (Exception ex)
            {
                
            }
            //_logando = false;
        }
        private void _sdk_LoggedOn(object sender, LoggedOnEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                
            }

        }
        private void _sdk_LoggedOff(object sender, LoggedOffEventArgs e)
        {
            //logon_SC.Abort()
            logado = false;

            try
            {
            }
            catch (Exception ex)
            {
                
            }
        }
        private void _sdk_LogonFailed(object sender, LogonFailedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
               
            }

        }
        private void _sdk_EventReceived(object sender, EventReceivedEventArgs e)
        {
            try
            {
                

            }
            catch (Exception ex)
            {
               
            }
        }
        private void _sdk_EntityInvalidated(object sender, EntityInvalidatedEventArgs e)
        {

            try
            {
                

            }
            catch (Exception ex)
            {

            }
        }
        private void MetodoRealizaFuncao(object state,Engine m_sdk )
        {
            try
            {

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
                        if (!string.IsNullOrEmpty(ec.CredencialGuid))
                        {
                            CardHolderEntity entity = new CardHolderEntity();
                            entity.IdentificadorCardHolderGuid = ec.CardHolderGuid;
                            entity.IdentificadorCredencialGuid = ec.CredencialGuid;
                            entity.Nome = ec.ColaboradorNome;                            
                            
                            
                            //O _SDK está vindo nulo
                            _serviceGenetec.AlterarStatusCredencial(entity);
                            
                        }

                        //var n1 = _serviceColaborador.BuscarCredencialPelaChave(ec.ColaboradorCredencialId);
                        //_serviceColaborador.RemoverRegrasCardHolder(new CredencialGenetecService(m_sdk), new ColaboradorService(), n1);
                       
                    }

                    string texto = "Impressa.:" + ec.Impressa + " Status.: " + ec.Ativa + " " + ec.ColaboradorNome + ((ec.Validade < DateTime.Now) ? " Vencido em : " : " Válido até : ") + ec.Validade;
                    CriarLog(string.Format("Contrato: {0}", texto));
                }
                );


                //CardHolderEntity entity2 = new CardHolderEntity();

                string messa;
                var empresaContratos = _service.Listar().Where(ec => ec.StatusId == 0).ToList();
                //var empresaContratos = _service.Listar().ToList();
                empresaContratos.ForEach(ec =>
                {
                    
                    string texto = ec.Descricao + ((ec.Validade < DateTime.Now) ? " - Vencido em : " : " - Válido até : ") + ec.Validade;
                    CriarLog(string.Format("Contrato: {0}", texto));
                    int dias = ec.Validade.Subtract(DateTime.Now.Date).Days;                    
                    switch (dias)
                    {
                        
                        case diasAlerta:
                            //CriarLog("Disparar alerta de vencendo hoje");
                            var empresa = _serviceEmpresa.BuscarPelaChave(ec.EmpresaId);
                            empresa.PraVencer = 0;
                            _serviceEmpresa.Alterar(empresa);
                            messa="Disparar alerta de vencendo hoje";
                            sendMessage(messa, ec.EmailResp);
                            //_serviceGenetec.DisparaAlarme(messa,8);
                            break;
                        case diasAlerta1:
                            //CriarLog("Disparar alerta de vencendo em 5 dias");
                            empresa = _serviceEmpresa.BuscarPelaChave(ec.EmpresaId);
                            empresa.PraVencer = 5;
                            _serviceEmpresa.Alterar(empresa);
                            messa = "Disparar alerta de vencendo em 5 dias";
                            sendMessage(messa, ec.EmailResp);
                            //_serviceGenetec.DisparaAlarme(messa, 8);
                            break;
                        case diasAlerta2:
                            //CriarLog("Disparar alerta de vencendo em 15 dias");
                            empresa = _serviceEmpresa.BuscarPelaChave(ec.EmpresaId);
                            empresa.PraVencer = 15;
                            _serviceEmpresa.Alterar(empresa);
                            messa = "Disparar alerta de vencendo em 15 dias";
                            sendMessage(messa, ec.EmailResp);
                            //_serviceGenetec.DisparaAlarme(messa, 8);
                            break;
                        case diasAlerta3:
                            //CriarLog("Disparar alerta de vencendo em 30 dias");
                            empresa = _serviceEmpresa.BuscarPelaChave(ec.EmpresaId);
                            empresa.PraVencer = 30;
                            _serviceEmpresa.Alterar(empresa);
                            messa = "Disparar alerta de vencendo em 30 dias";
                            sendMessage(messa, ec.EmailResp);
                           // _serviceGenetec.DisparaAlarme(messa, 8);
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
        protected void sendMessage(string msg,string email)
        {
           
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("renato.maximo@gmail.com");
            //mail.To.Add(email); // para
            mail.To.Add(email); // para
            mail.Subject = "Inativação"; // assunto
            mail.Body = msg; // mensagem

            // em caso de anexos
            //mail.Attachments.Add(new Attachment(@"C:\teste.txt"));
            //Tendo o objeto mail configurado, o próximo passo é criar um cliente Smtp e enviar o e-mail.
            using (var smtp = new SmtpClient("smtp.gmail.com"))
            {
                smtp.EnableSsl = false; // GMail requer SSL
                smtp.Port = 587;       // porta para SSL
                //smtp.Port = 465;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas

                // seu usuário e senha para autenticação
                smtp.Credentials = new NetworkCredential("renato.maximo@gmail.com", "maximo2552");

                try
                {
                    smtp.Send(mail);
                }
                catch (System.Exception erro)
                {
                    //trata erro
                }
                finally
                {
                    mail = null;
                }
            }
        }
        private void CriarLog(object state)
        {
            StreamWriter vWriter = new StreamWriter(@"C:\Tmp\Logs\log.txt", true);
            vWriter.WriteLine(state.ToString());
            vWriter.Flush();
            vWriter.Close();
        }
        public bool Stop(HostControl hostControl)
        {

            Log.Trace($"{nameof(Service.WinService)} Stop command received.");

            //TODO: Implement your service stop routine.
            return true;

        }
        public bool Pause(HostControl hostControl)
        {

            Log.Trace($"{nameof(Service.WinService)} Pause command received.");

            //TODO: Implement your service start routine.
            return true;

        }
        public bool Continue(HostControl hostControl)
        {

            Log.Trace($"{nameof(Service.WinService)} Continue command received.");

            //TODO: Implement your service stop routine.
            return true;

        }
        public bool Shutdown(HostControl hostControl)
        {

            Log.Trace($"{nameof(Service.WinService)} Shutdown command received.");

            //TODO: Implement your service stop routine.
            return true;

        }

    }
}
