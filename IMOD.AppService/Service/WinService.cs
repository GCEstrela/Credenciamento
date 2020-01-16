using Common.Logging;
using Genetec.Sdk;
using Genetec.Sdk.Credentials;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Activation;
using Genetec.Sdk.Entities.CustomEvents;
using Genetec.Sdk.Entities.CustomFields;
using Genetec.Sdk.Events;
using Genetec.Sdk.Queries;
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
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace IMOD.Service.Service
{

    class WinService
    {
        private ICredencialService _serviceGenetec;
        private IEmpresaService _serviceEmpresa = new EmpresaService();
        private IColaboradorCredencialService _serviceColaborador = new ColaboradorCredencialService();
        //private readonly IColaboradorCredencialService _serviceCredencialPendencia = new ColaboradorCredencialService();
        private IVeiculoCredencialService _serviceVeiculo = new VeiculoCredencialService();
        private IEmpresaContratosService _serviceContrato = new EmpresaContratoService();
        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();
        private ConfiguraSistema _configuraSistema;
        private IEngine _sdk;
        //private Engine _sdk = new Engine();

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
        //private readonly IEngine _sdk;
        public bool Start(HostControl hostControl)
        {
            try
            {
                //_sdk = new Genetec.Sdk.Engine();
                //_sdk = sdk;
                Genetec.Sdk.Engine _sdk = new Genetec.Sdk.Engine();
                Logon_SC_th(_sdk);
                CriarLog("Serviço Iniciado...: " + DateTime.Now);
                _serviceGenetec = new CredencialGenetecService(_sdk);
                MetodoRealizaFuncao(true, _sdk);
                CriarLog("Serviço Finalizado...: " + DateTime.Now);
                Environment.Exit(1);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

        }
        private void Logon_SC_th(Genetec.Sdk.Engine _sdk)
        {
            //_logando = true;
            try
            {
                //////////////////////////////////////
                //string returnValue = null;
                //string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //XmlDocument xmlDoc = new XmlDocument();
                //////////////////////
                ////string nodestring = "";
                //string instancia = "";
                //string banco = "";
                //string usuario = "";
                //string senha = "";
                //string complemento = "";

                //string Certificado = "";
                //string Diretorio = "";
                //string UsuarioSC = "";
                //string SenhaSC = "";

                //// cria a consulta
                //var prodsWEB = from p in XElement.Load(path + "\\ConexaoWEB.xml").Elements("Produto")
                //            where p.Element("ProdutoNome").Value == "SERVICO"
                //               select new
                //            {
                //                instancia = p.Element("InstanciaSQL").Value,
                //                banco = p.Element("Banco").Value,
                //                usuarioDB = p.Element("UsuarioDB").Value,
                //                SenhaDB = p.Element("SenhaDB").Value,
                //                complemento = p.Element("Complemento").Value,

                //                Certificado = p.Element("Certificado").Value,
                //                Diretorio = p.Element("Diretorio").Value,
                //                UsuarioSC = p.Element("UsuarioSC").Value,
                //                SenhaSC = p.Element("SenhaSC").Value,
                //            };

                //// Executa a consulta
                //foreach (var produto in prodsWEB)
                //{
                //    //lbProdutos.Items.Add(produto.NomeProduto);
                //    instancia = produto.instancia;
                //    banco = produto.banco;
                //    usuario = produto.usuarioDB;
                //    senha = produto.SenhaDB;
                //    complemento = produto.complemento;

                //    Certificado = produto.Certificado;
                //    Diretorio = produto.Diretorio;
                //    UsuarioSC = produto.UsuarioSC;
                //    SenhaSC = produto.SenhaSC;
                //}


                ////Conexao no Temp
                //xmlDoc.Load(path + "\\Conexao.xml");

                //XmlNode nodestring = xmlDoc.SelectSingleNode("StringConexao");
                //XmlNode instancia = nodestring.SelectSingleNode("InstanciaSQL");
                //XmlNode banco = nodestring.SelectSingleNode("Banco");
                //XmlNode usuario = nodestring.SelectSingleNode("UsuarioDB");
                //XmlNode senha = nodestring.SelectSingleNode("SenhaDB");
                //string senhaDecryptada = senha.InnerXml;

                //EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
                //EstrelaEncryparDecrypitar.Variavel.key = "CREDENCIAMENTO2019";
                //string senhaDecryptada = ESTRELA_EMCRYPTAR.EstrelaDecrypt(senha);

                //XmlNode complemento = nodestring.SelectSingleNode("Complemento");
                //returnValue = "Data Source=" + instancia + ";Initial Catalog=" + banco + ";User ID=" + usuario + ";Password=" + senhaDecryptada + ";" + complemento;
                //UsuarioLogado.InstanciaSQL = returnValue;

                //return returnValue;

                //////////////////////////////////
                string strintsql = ConfigurationManager.AppSettings["Conexao"].ToString();
                string certificado = ConfigurationManager.AppSettings["Certificado"].ToString();
                string diretorio = ConfigurationManager.AppSettings["Diretorio"];
                string usuariosc = ConfigurationManager.AppSettings["UsuarioSC"];
                string senhasc = ConfigurationManager.AppSettings["SenhaSC"];

                //string certificado = Certificado.ToString();
                //string diretorio = Diretorio.ToString();
                //string usuariosc = UsuarioSC.ToString();
                //string senhasc = SenhaSC.ToString();

                _sdk.ClientCertificate = certificado;
                if (_sdk.IsConnected)
                {
                    _sdk.LogOff();
                    Thread.Sleep(500);
                    if (!_sdk.IsConnected)
                    {
                        _sdk.LogOn(diretorio, usuariosc, senhasc);
                    }
                }
                else
                {
                    _sdk.LogOff();
                    Thread.Sleep(500);
                    _sdk.LogOn(diretorio, usuariosc, senhasc);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
        Hashtable empresaContrato = new Hashtable();
        private void MetodoRealizaFuncao(object state, Engine m_sdk)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                ////_serviceGenetec.DisparaAlarme("teste", 8);
                CriarLog("-----------------------------------------------");
                CriarLog("Iniciando Validade dos Colaboradores...");
                try
                {
                    var colaboradorContratos = _serviceColaborador.Listar(null, null, null, null, null, null, true).ToList();
                    colaboradorContratos.ForEach(ec =>
                    {
                        string messa = "";
                        var empresasEmail = _serviceEmpresa.Listar(null, null, null, null, null, null, ec.ColaboradorEmpresaId).FirstOrDefault();
                        DateTime validadeCredencial = (DateTime)ec.Validade;
                        //string texto = ((ec.Validade < DateTime.Now) ? " - Vencido em : " : " - Válido até : ") + ec.Validade;
                        //CriarLog(string.Format("Contrato: {0}", texto));
                        int dias = validadeCredencial.Subtract(DateTime.Now.Date).Days;
                        switch (dias)
                        {
                            case diasAlerta:
                                messa = "A credencial do colaborador.: " + ec.ColaboradorNome + " vencendo hoje";
                                //_serviceGenetec.DisparaAlarme(messa, 8);
                                CriarLog(messa);
                                break;

                            case diasAlerta1:
                                messa = "A credencial do colaborador.: " + ec.ColaboradorNome + " vencerá em " + diasAlerta1 + " dias.";
                                //_serviceGenetec.DisparaAlarme(messa, 8);

                                break;

                            case diasAlerta2:
                                messa = "A credencial do colaborador.: " + ec.ColaboradorNome + " vencerá em " + diasAlerta2 + " dias.";
                                //_serviceGenetec.DisparaAlarme(messa, 8);

                                break;

                            case diasAlerta3:
                                messa = "A credencial do colaborador.: " + ec.ColaboradorNome + " vencerá em " + diasAlerta3 + " dias.";
                                //_serviceGenetec.DisparaAlarme(messa, 8);

                                break;

                            default:
                                break;
                        }

                        _configuraSistema = ObterConfiguracao();
                        if (_configuraSistema.Email != null)
                        {
                            //if (empresasEmail != null && empresasEmail.Email1 !=null)
                            //sendMessage(messa, _configuraSistema.Email.Trim(), _configuraSistema.SMTP.Trim(), _configuraSistema.EmailUsuario.Trim(), _configuraSistema.EmailSenha.Trim(), empresasEmail.Email1.Trim());
                        }

                        if (ec.Validade < DateTime.Now)
                        {
                            ec.Ativa = false;
                            ec.CredencialStatusId = (int)StatusCredencial.INATIVA;
                            ec.CredencialMotivoId = (int)Inativo.EXPIRADA;
                            ec.CredencialStatusId = 2;
                            _serviceColaborador.Alterar(ec);
                            //Em processo de alteração.
                            var pendencia = _serviceColaborador.ListarView(ec.ColaboradorCredencialId).FirstOrDefault();
                            _serviceColaborador.CriarPendenciaImpeditiva(pendencia);

                            if (!string.IsNullOrEmpty(ec.CredencialGuid))
                            {
                                CardHolderEntity entity = new CardHolderEntity();
                                entity.IdentificadorCardHolderGuid = ec.CardHolderGuid;
                                entity.IdentificadorCredencialGuid = ec.CredencialGuid;
                                entity.Nome = ec.ColaboradorNome;


                                ////O _SDK está vindo nulo
                                //_serviceGenetec.AlterarStatusCredencial(entity);

                            }

                            //var n1 = _serviceColaborador.BuscarCredencialPelaChave(ec.ColaboradorCredencialId);
                            //_serviceColaborador.RemoverRegrasCardHolder(new CredencialGenetecService(m_sdk), new ColaboradorService(), n1);

                        }

                        string texto = "Impressa.:" + ec.Impressa + " Status.: " + ec.Ativa + " " + ec.ColaboradorNome + ((ec.Validade < DateTime.Now) ? " Vencido em : " : " Válido até : ") + ec.Validade;
                        //CriarLog(dias + " Colaborador.: " + ec.ColaboradorNome + " Validade.: " + ec.Validade);
                    }
                    );
                }
                catch (Exception ex)
                {

                    //throw;
                }


                try
                {
                    CriarLog("-----------------------------------------------");
                    CriarLog("Iniciando Validade dos Veículos/Equipamentos...");
                    var veiculoContratos = _serviceVeiculo.Listar(null, null, null, null, null, true).ToList();
                    veiculoContratos.ForEach(ev =>
                    {
                        string messaveiculo = "";
                        var empresasEmail = _serviceEmpresa.Listar(null, null, null, null, null, null, ev.VeiculoEmpresaId).FirstOrDefault();
                        DateTime validadeCredencial = (DateTime)ev.Validade;
                        int dias = validadeCredencial.Subtract(DateTime.Now.Date).Days;
                        switch (dias)
                        {
                            case diasAlerta:
                                messaveiculo = "A ATIV do veiculo.: " + ev.IdentificacaoDescricao + " vencendo hoje";
                                CriarLog(messaveiculo);
                                break;

                            case diasAlerta1:
                                messaveiculo = "A ATIV do veiculo.: " + ev.IdentificacaoDescricao + " vencerá em " + diasAlerta1 + " dias.";

                                break;

                            case diasAlerta2:
                                messaveiculo = "A ATIV do veiculo.: " + ev.IdentificacaoDescricao + " vencerá em " + diasAlerta2 + " dias.";


                                break;

                            case diasAlerta3:
                                messaveiculo = "A ATIV do veiculo.: " + ev.IdentificacaoDescricao + " vencerá em " + diasAlerta3 + " dias.";

                                break;

                            default:
                                break;
                        }

                        _configuraSistema = ObterConfiguracao();
                        if (_configuraSistema.Email != null)
                        {
                        }

                        if (ev.Validade < DateTime.Now)
                        {
                            ev.Ativa = false;
                            ev.CredencialStatusId = (int)StatusCredencial.INATIVA;
                            ev.CredencialMotivoId = (int)Inativo.EXPIRADA;
                            ev.CredencialStatusId = 2;
                            _serviceVeiculo.Alterar(ev);
                            if (!string.IsNullOrEmpty(ev.CredencialGuid))
                            {

                            }

                        }

                        string texto = "Impressa.:" + ev.Impressa + " Status.: " + ev.Ativa + " " + ev.IdentificacaoDescricao + ((ev.Validade < DateTime.Now) ? " Vencido em : " : " Válido até : ") + ev.Validade;
                    }
                    );
                }
                catch (Exception)
                {

                }

                try
                {
                    CriarLog("-----------------------------------------------");
                    CriarLog("Iniciando Validade dos Contratos...");
                    Hashtable empresaContratoEmail = new Hashtable();
                    string nomeEmpresa = "";
                    string emailEmpresa = "";

                    var empresas = _serviceEmpresa.Listar().OrderByDescending(ec => ec.EmpresaId).ToList();
                    empresas.ForEach(e =>
                    {
                        emailEmpresa = e.Email1;
                        nomeEmpresa = e.Nome;

                        List<MessagemEmail> listMessagemEmail = new List<MessagemEmail>();
                        var empresaContratosAtivo = _serviceContrato.Listar().Where(ec => ec.StatusId == 0 && ec.EmpresaId == e.EmpresaId).ToList();
                        var AlartaList = new List<int>() { -1, 5, 15, 30 };
                            empresaContratosAtivo.ForEach(ec =>
                            {
                                int dias = ec.Validade.Subtract(DateTime.Now.Date).Days;

                                if (AlartaList.Contains(dias))
                                {
                                    AlterarDados(ec.EmpresaId, ec.EmpresaContratoId, diasAlerta0);

                                    var messa1 = new MessagemEmail() { Contrato = ec.NumeroContrato, Dias = dias, DescricaoDoContrato = ec.Descricao, EmailDestino = emailEmpresa };
                                    listMessagemEmail.Add(messa1);

                                    CriarLog("Dias.: " + dias + " Contrato.: " + ec.NumeroContrato + " Descrição.: " + ec.Descricao);
                                }
                                else
                                {
                                    AlterarDados(ec.EmpresaId, ec.EmpresaContratoId, dias);
                                }
                            }
                        );


                        listMessagemEmail = listMessagemEmail.OrderBy(m => m.Dias).ToList();

                        if (listMessagemEmail.Count > 0)
                        {
                            StringBuilder emailFraport = new StringBuilder();
                            emailFraport.AppendLine("Prezado usuário,");
                            emailFraport.AppendLine(string.Empty);
                            emailFraport.AppendLine(string.Format("Informamos que o(s) contrato(s) abaixo pertencente(s) a empresa {0} estão próximos do vencimento:", nomeEmpresa));
                            emailFraport.AppendLine(string.Empty);
                            foreach (MessagemEmail element in listMessagemEmail)
                            {
                                emailFraport.AppendLine(string.Format(" - Contrato: {0} - {1}. {2} dia(s) para o vencimento;", element.Contrato, element.DescricaoDoContrato, element.Dias));
                            }
                            emailFraport.AppendLine("");
                            emailFraport.AppendLine("Att:");
                            emailFraport.AppendLine("");
                            emailFraport.AppendLine("Alerta do Sistema de Credenciamento.");
                            emailFraport.AppendLine("Setor de Credenciamento " + _configuraSistema.NomeAeroporto);
                            if (emailEmpresa != "")
                            {
                                try
                                {
                                    sendMessage(emailFraport.ToString(), emailEmpresa);
                                }
                                catch (Exception ex)
                                {

                                    //throw;
                                }

                            }
                        }
                    }

                    );
                }
                catch (Exception ex)
                {

                    //throw;
                }

                Cursor.Current = Cursors.IBeam;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.IBeam;
                CriarLog(ex.Message);
            }
        }
        public class MessagemEmail
        {
            public string Contrato { get; set; }
            public object Dias { get; set; }
            public object DescricaoDoContrato { get; set; }
            public object EmailDestino { get; set; }
        }
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresServiceConfiguraSistema.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }

        public void AlterarDados(int empresaID, int empresacontratoID, int diasrestantes)
        {

            try
            {
                if (!empresaContrato.ContainsKey(empresaID))
                {
                    empresaContrato[empresaID] = diasrestantes;
                }

                var empresa = _serviceEmpresa.BuscarPelaChave(empresaID);
                empresa.PraVencer = diasrestantes;
                _serviceEmpresa.Alterar(empresa);

                var contrato = _serviceContrato.BuscarPelaChave(empresacontratoID);
                contrato.PraVencer = diasrestantes;
                contrato.StatusId = 0;

                if (diasrestantes == 0)
                {
                    contrato.StatusId = 1;
                }
                _serviceContrato.Alterar(contrato);
            }
            catch (System.Exception erro)
            {
                //trata erro
            }
        }
        protected void sendMessage(string msg, string emailDestino)
        {
            var emailOrigem = "";
            var Emailsmtp = "";
            var usuario = "";
            var senha = "";
            var emailInterno = "";
            _configuraSistema = ObterConfiguracao();
            if (_configuraSistema.EmailUsuario != null)
            {
                emailOrigem = _configuraSistema.EmailUsuario;
                Emailsmtp = _configuraSistema.SMTP;
                usuario = _configuraSistema.EmailUsuario;
                senha = _configuraSistema.EmailSenha;
                emailInterno = _configuraSistema.Email;
            }
            if (emailDestino == null || emailDestino == "") return;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailOrigem);

            //mail.To.Add(emailDestino); // para
            var variosEmail = emailDestino.Split(';');
            foreach (string element in variosEmail)
            {
                mail.To.Add(element);
            }
            variosEmail = emailInterno.Split(';');
            foreach (string element in variosEmail)
            {
                mail.To.Add(element);
            }

            mail.Subject = "Alerta de Vencimento de Contrato(s)"; // assunto 
            mail.Body = msg; // mensagem

            //em caso de anexos
            //mail.Attachments.Add(new Attachment(@"C:\teste.txt"));
            //Tendo o objeto mail configurado, o próximo passo é criar um cliente Smtp e enviar o e-mail.
            using (var smtp = new SmtpClient(Emailsmtp))
            {
                smtp.EnableSsl = _configuraSistema.EnableSsl; // GMail requer SSL
                smtp.Port = _configuraSistema.PortaSMTP;       // porta para SSL 587
                //smtp.Port = 465;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                smtp.UseDefaultCredentials = _configuraSistema.UseDefaultCredentials; // vamos utilizar credencias especificas

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
                StreamWriter vWriter = new StreamWriter(@"C:\Tmp\log.txt", true);
                vWriter.WriteLine(state.ToString());
                vWriter.Flush();
                vWriter.Close();
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
