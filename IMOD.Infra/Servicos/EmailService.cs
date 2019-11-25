using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using IMOD.Infra.Entities;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Infra.Repositorios;

namespace IMOD.Infra.Servicos
{
    public static class EmailService
    {
        public static void EnviarEmail(Email email, string nomeUsuario)
        {
            try
            {
                EstrelaEncryparDecrypitar.Variavel.key = "CREDENCIAMENTO2019";
                EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
                email.Senha = ESTRELA_EMCRYPTAR.EstrelaDecrypt(email.Senha);

                var smtpClient = new SmtpClient();
                var basicCredential = new NetworkCredential(email.Usuario, email.Senha);

                smtpClient.EnableSsl = email.UsarSsl;
                smtpClient.Host = email.ServidorEmail;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                smtpClient.Port = Convert.ToInt32(email.Porta);

                smtpClient.Timeout = string.IsNullOrEmpty(email.TimeOut) ? 100000 : Convert.ToInt32(email.TimeOut);

                var fromAddress = new MailAddress(email.EmailRemetente, email.NomeRemetente);
                var vMessage = new MailMessage();
                vMessage.From = fromAddress;
                vMessage.IsBodyHtml = true;
                vMessage.Subject = email.Assunto;
                foreach (var destinatario in email.EmailDestinatario)
                {
                    if (string.IsNullOrEmpty(destinatario.Trim()))
                        continue;
                    vMessage.To.Add(destinatario);
                }

                if (vMessage.To.Count == 0)
                    throw new Exception("Informar um destinatário");

                email.Mensagem = email.Mensagem.Replace("\n", "<br />");
                vMessage.Body = email.Mensagem;
                //Anexos
                if (email.Anexos != null)
                    for (var i = 0; i < email.Anexos.Count; i++)
                    {
                        Stream stream = new MemoryStream(email.Anexos[i]);
                        stream.Position = 0;
                        vMessage.Attachments.Add(new Attachment(stream, "Anexo" + i, MediaTypeNames.Application.Octet));
                    }

                smtpClient.Send(vMessage);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw new Exception("Uma falha ocorreu ao enviar email", ex);
            }
        }

        public static void EnviarEmail(string destinatarios,string assunto, string mensagem, string nomeUsuario)
        {
            try
            {
                //Configuraçoes do serviço de email
                ConfiguraSistema configSistema = ObterConfiguracao();
                Email email = new Email();
                email.Assunto = assunto;
                email.Usuario = configSistema.EmailUsuario.Trim();
                email.Senha = configSistema.EmailSenha.Trim();
                email.ServidorEmail = configSistema.SMTP.Trim();
                email.Porta = configSistema.PortaSMTP.ToString();       // porta para SSL                                   
                email.UsarSsl = configSistema.EnableSsl; // GMail requer SSL         
                                       //email.UsarTls = true;
                email.UsarAutenticacao = false;
                email.EmailDestinatario = destinatarios.Split(';').ToList();
                email.EmailRemetente = configSistema.EmailUsuario;
                email.NomeRemetente = configSistema.NomeAeroporto;                                
                email.Mensagem = mensagem;

                var smtpClient = new SmtpClient();
                var basicCredential = new NetworkCredential(email.Usuario, email.Senha);

                smtpClient.EnableSsl = email.UsarSsl;
                smtpClient.Host = email.ServidorEmail;
                smtpClient.UseDefaultCredentials = configSistema.UseDefaultCredentials;
                smtpClient.Credentials = basicCredential;
                smtpClient.Port = Convert.ToInt32(email.Porta);
                smtpClient.Timeout = string.IsNullOrEmpty(email.TimeOut) ? 100000 : Convert.ToInt32(email.TimeOut);

                var fromAddress = new MailAddress(email.EmailRemetente, email.NomeRemetente);
                var vMessage = new MailMessage();
                vMessage.From = fromAddress;
                vMessage.IsBodyHtml = true;
                vMessage.Subject = email.Assunto;
                foreach (var destinatario in email.EmailDestinatario)
                {
                    if (string.IsNullOrEmpty(destinatario.Trim()))
                        continue;
                    vMessage.To.Add(destinatario);
                }

                if (vMessage.To.Count == 0)
                    throw new Exception("Informar um destinatário");

                email.Mensagem = email.Mensagem.Replace("\n", "<br />");
                vMessage.Body = email.Mensagem;
                //Anexos
                if (email.Anexos != null)
                    for (var i = 0; i < email.Anexos.Count; i++)
                    {
                        Stream stream = new MemoryStream(email.Anexos[i]);
                        stream.Position = 0;
                        vMessage.Attachments.Add(new Attachment(stream, "Anexo" + i, MediaTypeNames.Application.Octet));
                    }

                smtpClient.Send(vMessage);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw new Exception("Uma falha ocorreu ao enviar email", ex);
            }
        }

        private static ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = new ConfiguraSistemaRepositorio().Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }


    }
}
