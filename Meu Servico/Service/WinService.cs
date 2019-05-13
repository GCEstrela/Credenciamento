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
using System.Collections;
using IMOD.Domain.Entities;
using Genetec.Sdk.Entities;

namespace Meu_Servico.Service
{
   
    class WinService
    {
        private ICredencialService _serviceGenetec;
        private IEmpresaService _serviceEmpresa = new EmpresaService();
        private IColaboradorCredencialService _serviceColaborador = new ColaboradorCredencialService();
        private IEmpresaContratosService _serviceContrato = new EmpresaContratoService();
        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();
        private ConfiguraSistema _configuraSistema;
        private IEngine _sdk;

        private System.Timers.Timer timer;

        public ILog Log { get; private set; }

        private Boolean logado;
        private const int diasAlerta = 10;
        private const int diasAlerta0 = 0;
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
        //var MyTimer = new Timer(RunTask, AutoEvent, 1000, 2000);
        
        public bool Start(HostControl hostControl)
        {
            //Genetec.Sdk.Engine _sdk = new Genetec.Sdk.Engine();
            //Logon_SC_th(_sdk);

            //_sdk.LoggedOn += _sdk_LoggedOn;
            //_sdk.LoggedOff += _sdk_LoggedOff;
            //_sdk.LogonFailed += _sdk_LogonFailed;
            //_sdk.EventReceived += _sdk_EventReceived;
            //_sdk.EntityInvalidated += _sdk_EntityInvalidated;

            this.timer = new System.Timers.Timer(40000D);  // 40000 milliseconds = 40 seconds
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            this.timer.Start();

            return true;

        }
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
            Genetec.Sdk.Engine _sdk = new Genetec.Sdk.Engine();
            Logon_SC_th(_sdk);
            //_sdk.LoggedOn += _sdk_LoggedOn;
            //_sdk.LoggedOff += _sdk_LoggedOff;
            //_sdk.LogonFailed += _sdk_LogonFailed;
            //_sdk.EventReceived += _sdk_EventReceived;
            //_sdk.EntityInvalidated += _sdk_EntityInvalidated;

            _serviceGenetec = new CredencialGenetecService(_sdk);
            MetodoRealizaFuncao(true, _sdk);

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
            catch (Exception)
            {
                
            }
            //_logando = false;
        }
        private void _sdk_LoggedOn(object sender, LoggedOnEventArgs e)
        {
            try
            {

            }
            catch (Exception)
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
            catch (Exception)
            {
                
            }
        }
        private void _sdk_LogonFailed(object sender, LogonFailedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception)
            {
               
            }

        }
        private void _sdk_EventReceived(object sender, EventReceivedEventArgs e)
        {
            try
            {
                

            }
            catch (Exception)
            {
               
            }
        }
        private void _sdk_EntityInvalidated(object sender, EntityInvalidatedEventArgs e)
        {

            try
            {
                

            }
            catch (Exception)
            {

            }
        }
        private void MetodoRealizaFuncao(object state,Engine m_sdk )
        {
            try
            {
                ////_serviceGenetec.DisparaAlarme("teste", 8);
                CriarLog("serviço rodando:" + DateTime.Now);
                string messa;
                var colaboradorContratos = _serviceColaborador.Listar(null, null, null, null, null, null, true).ToList();
                colaboradorContratos.ForEach(ec =>
                {
                    DateTime validadeCredencial = (DateTime)ec.Validade;
                    //string texto = ((ec.Validade < DateTime.Now) ? " - Vencido em : " : " - Válido até : ") + ec.Validade;
                    //CriarLog(string.Format("Contrato: {0}", texto));
                    int dias = validadeCredencial.Subtract(DateTime.Now.Date).Days;
                    switch (dias)
                    {
                        case diasAlerta:
                            messa = "A credencial do colaborador.: " + ec.ColaboradorNome + " vencendo hoje";
                            //_serviceGenetec.DisparaAlarme(messa, 8);

                            _configuraSistema = ObterConfiguracao();
                            if (_configuraSistema.Email != null)
                            {
                                if (ec.Email != null)
                                    sendMessage(messa, _configuraSistema.Email.Trim(), _configuraSistema.SMTP.Trim(), _configuraSistema.EmailUsuario.Trim(), _configuraSistema.EmailSenha.Trim(), ec.Email.Trim());
                            }
                            break;

                        case diasAlerta1:
                            messa = "A credencial do colaborador.: " + ec.ColaboradorNome + " vencerá em " + diasAlerta1 + " dias.";
                            //_serviceGenetec.DisparaAlarme(messa, 8);

                            _configuraSistema = ObterConfiguracao();
                            if (_configuraSistema.Email != null)
                            {
                                if (ec.Email != null)
                                    sendMessage(messa, _configuraSistema.Email.Trim(), _configuraSistema.SMTP.Trim(), _configuraSistema.EmailUsuario.Trim(), _configuraSistema.EmailSenha.Trim(), ec.Email.Trim());
                            }
                            break;

                        case diasAlerta2:
                            messa = "A credencial do colaborador.: " + ec.ColaboradorNome + " vencerá em " + diasAlerta2 + " dias.";
                            //_serviceGenetec.DisparaAlarme(messa, 8);

                            _configuraSistema = ObterConfiguracao();
                            if (_configuraSistema.Email != null)
                            {
                                if (ec.Email != null)
                                    sendMessage(messa, _configuraSistema.Email.Trim(), _configuraSistema.SMTP.Trim(), _configuraSistema.EmailUsuario.Trim(), _configuraSistema.EmailSenha.Trim(), ec.Email.Trim());
                            }
                            break;

                        case diasAlerta3:
                            messa = "A credencial do colaborador.: " + ec.ColaboradorNome + " vencerá em " + diasAlerta3 + " dias.";
                            //_serviceGenetec.DisparaAlarme(messa, 8);

                            _configuraSistema = ObterConfiguracao();
                            if (_configuraSistema.Email != null)
                            {
                                if (ec.Email != null)
                                    sendMessage(messa, _configuraSistema.Email.Trim(), _configuraSistema.SMTP.Trim(), _configuraSistema.EmailUsuario.Trim(), _configuraSistema.EmailSenha.Trim(), ec.Email.Trim());
                            }
                            break;

                        default:
                            break;
                    }

                    
                    if (ec.Validade < DateTime.Now)
                    {
                        ec.Ativa = false;
                        ec.CredencialStatusId = (int)StatusCredencial.INATIVA;
                        ec.CredencialMotivoId = (int)Inativo.EXPIRADA;
                        ec.CredencialStatusId = 2;
                        _serviceColaborador.Alterar(ec);
                        if (!string.IsNullOrEmpty(ec.CredencialGuid))
                        {
                            CardHolderEntity entity = new CardHolderEntity();
                            entity.IdentificadorCardHolderGuid = ec.CardHolderGuid;
                            entity.IdentificadorCredencialGuid = ec.CredencialGuid;
                            entity.Nome = ec.ColaboradorNome;


                            ////O _SDK está vindo nulo
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
                Hashtable empresaContrato = new Hashtable();
                Hashtable empresaContratoEmail = new Hashtable();

                //var empresaContratos = _serviceContrato.Listar().Where(ec =>  ec.StatusId == 0).OrderByDescending(ec => ec.Validade).ToList();                
                var empresaContratos = _serviceContrato.Listar().Where(ec => ec.StatusId == 0).OrderByDescending(ec => ec.EmpresaId).ToList();
                //var empresaContratos = _service.Listar().ToList();
                empresaContratos.ForEach(ec =>
                {
                    
                    string texto = ec.Descricao + ((ec.Validade < DateTime.Now) ? " - Vencido em : " : " - Válido até : ") + ec.Validade;
                    CriarLog(string.Format("Contrato: {0}", texto));
                    int dias = ec.Validade.Subtract(DateTime.Now.Date).Days;                    
                    switch (dias)
                    {
                        case diasAlerta:
                            var colaboradorContrao = _serviceColaborador.Listar(null, null, null, null, null, null, null, ec.NumeroContrato).ToList();
                            colaboradorContrao.ForEach(cd =>
                            {
                                cd.Ativa = false;
                                _serviceColaborador.Alterar(cd);
                            }
                            );
                            break;
                        case diasAlerta0:
                            //CriarLog("Disparar alerta de vencendo hoje");
                            // Verifica se a Hashtable contém esta chave
                            if (!empresaContrato.ContainsKey(ec.EmpresaId))
                            {
                                empresaContrato[ec.EmpresaId] = diasAlerta0;
                            }

                            var empresa = _serviceEmpresa.BuscarPelaChave(ec.EmpresaId);
                            empresa.PraVencer = diasAlerta0;                            
                            _serviceEmpresa.Alterar(empresa);

                            var contrato = _serviceContrato.BuscarPelaChave(ec.EmpresaContratoId);
                            contrato.PraVencer = diasAlerta0;
                            contrato.StatusId = 1;
                            _serviceContrato.Alterar(contrato);


                            
                            _configuraSistema = ObterConfiguracao();
                            if (_configuraSistema.Email != null)
                            {
                                if (empresa.Email1 != null)
                                {
                                    var messa1 = new MeuValor() { Valor1 = "O Contrato Nº.: " + ec.NumeroContrato + " da empresa " + empresa.Nome + " vencendo hoje", Valor2 = empresa.Email1.Trim() };
                                    empresaContratoEmail[ec.EmpresaId] = messa1;
                                }
                                    //sendMessage(messa, _configuraSistema.Email.Trim(), _configuraSistema.SMTP.Trim(), _configuraSistema.EmailUsuario.Trim(), _configuraSistema.EmailSenha.Trim(), empresa.Email1.Trim());

                            }
                            ////_serviceGenetec.GerarEvento(messa,8);
                            ////_serviceGenetec.GerarEvento("8000",null,messa);
                            break;
                        case diasAlerta1:
                            //CriarLog("Disparar alerta de vencendo em 5 dias");
                            // Verifica se a Hashtable contém esta chave
                            if (!empresaContrato.ContainsKey(ec.EmpresaId))
                            {
                                empresaContrato[ec.EmpresaId] = diasAlerta1;
                            }
                            
                            empresa = _serviceEmpresa.BuscarPelaChave(ec.EmpresaId);
                            empresa.PraVencer = diasAlerta1;
                            _serviceEmpresa.Alterar(empresa);

                            contrato = _serviceContrato.BuscarPelaChave(ec.EmpresaContratoId);
                            contrato.PraVencer = diasAlerta1;
                            contrato.StatusId = 1;
                            _serviceContrato.Alterar(contrato);


                            messa = "O Contrato Nº.: " + ec.NumeroContrato + " da empresa " + empresa.Nome + " vencendo em 5 dias";
                            empresaContratoEmail[ec.EmpresaId] = messa;

                            //_configuraSistema = ObterConfiguracao();
                            //if (_configuraSistema.Email != null)
                            //{
                            //    if (empresa.Email1 != null)
                            //        sendMessage(messa, _configuraSistema.Email.Trim(), _configuraSistema.SMTP.Trim(), _configuraSistema.EmailUsuario.Trim(), _configuraSistema.EmailSenha.Trim(), empresa.Email1.Trim());
                            //}
                            ////_serviceGenetec.DisparaAlarme(messa, 8);
                            ////_serviceGenetec.GerarEvento("8005", null, messa);
                            break;
                        case  diasAlerta2:
                            //CriarLog("Disparar alerta de vencendo em 15 dias");
                            // Verifica se a Hashtable contém esta chave
                            if (!empresaContrato.ContainsKey(ec.EmpresaId))
                            {
                                empresaContrato[ec.EmpresaId] = diasAlerta2;
                            }
                            
                            empresa = _serviceEmpresa.BuscarPelaChave(ec.EmpresaId);
                            empresa.PraVencer = diasAlerta2;
                            _serviceEmpresa.Alterar(empresa);

                            contrato = _serviceContrato.BuscarPelaChave(ec.EmpresaContratoId);
                            contrato.PraVencer = diasAlerta2;
                            contrato.StatusId = 1;
                            _serviceContrato.Alterar(contrato);


                            messa = "O Contrato Nº.: " + ec.NumeroContrato + " da empresa " + empresa.Nome + " vencendo em 15 dias";
                            empresaContratoEmail[ec.EmpresaId] = messa;

                            //_configuraSistema = ObterConfiguracao();
                            //if (_configuraSistema.Email != null)
                            //{
                            //    if (empresa.Email1 != null)
                            //        sendMessage(messa, _configuraSistema.Email.Trim(), _configuraSistema.SMTP.Trim(), _configuraSistema.EmailUsuario.Trim(), _configuraSistema.EmailSenha.Trim(), empresa.Email1.Trim());
                            //}
                            ////_serviceGenetec.DisparaAlarme(messa, 8);
                            ////_serviceGenetec.GerarEvento("8015", null, messa);
                            break;
                        case diasAlerta3:
                            //CriarLog("Disparar alerta de vencendo em 30 dias");
                            // Verifica se a Hashtable contém esta chave
                            if (!empresaContrato.ContainsKey(ec.EmpresaId))
                            {
                                empresaContrato[ec.EmpresaId] = diasAlerta3;
                            }
                            
                            empresa = _serviceEmpresa.BuscarPelaChave(ec.EmpresaId);
                            empresa.PraVencer = diasAlerta3;
                            _serviceEmpresa.Alterar(empresa);

                            contrato = _serviceContrato.BuscarPelaChave(ec.EmpresaContratoId);
                            contrato.PraVencer = diasAlerta3;
                            contrato.StatusId = 1;
                            _serviceContrato.Alterar(contrato);

                            messa = "O Contrato Nº.: " + ec.NumeroContrato + " da empresa " + empresa.Nome +  " vencendo em 30 dias";
                            empresaContratoEmail[ec.EmpresaId] = messa;

                            //_configuraSistema = ObterConfiguracao();
                            //if (_configuraSistema.Email != null)
                            //{
                            //    if (empresa.Email1 != null)
                            //        sendMessage(messa, _configuraSistema.Email.Trim(), _configuraSistema.SMTP.Trim(), _configuraSistema.EmailUsuario.Trim(), _configuraSistema.EmailSenha.Trim(), empresa.Email1.Trim());
                            //}
                            //// _serviceGenetec.DisparaAlarme(messa, 8);
                            ////_serviceGenetec.GerarEvento("8030", null, messa);
                            break;
                        default:
                            break;

                    }

                }

                );
                                    //_configuraSistema = ObterConfiguracao();
                    //if (_configuraSistema.Email != null)
                    //{
                        
                    //        //sendMessage(messa, _configuraSistema.Email.Trim(), _configuraSistema.SMTP.Trim(), _configuraSistema.EmailUsuario.Trim(), _configuraSistema.EmailSenha.Trim(), empresa.Email1.Trim());
                    //}
                    foreach (string element in empresaContratoEmail)
                    {
                       
                        

                    }
                
            }
            catch (Exception ex)
            {
                CriarLog(ex.Message);
            }
        }
        public class MeuValor
        {
            public object Valor1 { get; set; }
            public object Valor2 { get; set; }
        }
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresServiceConfiguraSistema.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }
        protected void sendMessage(string msg, string emailOrigem,string Emailsmtp,string usuario,string senha, string emailDestino)
        {

            if (emailDestino == null || emailDestino == "") return;

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(emailOrigem);
            mail.To.Add(emailOrigem); // para
            mail.To.Add(emailDestino); // para
            mail.Subject = "Inativação de Contrato(s)"; // assunto 
            mail.Body = msg; // mensagem
           
            //em caso de anexos
            //mail.Attachments.Add(new Attachment(@"C:\teste.txt"));
            //Tendo o objeto mail configurado, o próximo passo é criar um cliente Smtp e enviar o e-mail.
            using (var smtp = new SmtpClient(Emailsmtp))
            {
                smtp.EnableSsl = false; // GMail requer SSL
                smtp.Port = 587;       // porta para SSL
                //smtp.Port = 465;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas

                // seu usuário e senha para autenticação
                smtp.Credentials = new NetworkCredential(usuario, senha);

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
            try
            {
                //StreamWriter vWriter = new StreamWriter(@"C:\Tmp\Logs\log.txt", true);
                //vWriter.WriteLine(state.ToString());
                //vWriter.Flush();
                //vWriter.Close();
            }
            catch (Exception ex)
            {
                CriarLog(ex.Message);
            }

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
