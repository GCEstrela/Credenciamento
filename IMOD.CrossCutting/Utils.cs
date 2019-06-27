// ***********************************************************************
// Project: IMOD.CrossCutting
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;
using IMOD.CrossCutting.Entities;
using Newtonsoft.Json;

#endregion

namespace IMOD.CrossCutting
{
    /// <summary>
    ///     Contém Helpers para auxiliar ou extender funcionalidades
    ///     <para>
    ///         Não é permitido uso de controles de interface gráfica com o usuário
    ///         Para fazê-lo usar a classe DevSysbr.App.WinHelp
    ///     </para>
    /// </summary>
    public static class Utils
    {
        //Salvar dados de log no caminho de arquivos temporários
        private static readonly string CaminhoLog = Path.Combine (Path.GetTempPath(), "Credenciamento\\Log");
        private static readonly string NomeArqErrorLog = "Error.log";

        #region  Metodos

        /// <summary>
        ///     Lista de impressoras instaladas
        /// </summary>
        /// <returns></returns>
        public static PrinterSettings.StringCollection ListarImpressoras()
        {
            try
            {
                return PrinterSettings.InstalledPrinters;
            }
            catch (Exception ex)
            {
                TraceException (ex, "Não foi possível listar impressoras instaladas");
                throw new Exception ("Não foi possível listar impressoras instaladas");
            }
        }

        /// <summary>
        ///     Converte uma string em Base64 para uma imagem
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static Bitmap ConverterBase64StringToBitmap(this string base64String)
        {
            if (string.IsNullOrWhiteSpace (base64String)) return null;
            Bitmap bmpReturn = null;
            var byteBuffer = Convert.FromBase64String (base64String);
            var memoryStream = new MemoryStream (byteBuffer);
            memoryStream.Position = 0;
            bmpReturn = (Bitmap) Image.FromStream (memoryStream);
            memoryStream.Close();
            return bmpReturn;
        }

        /// <summary>
        ///     Enviar email
        /// </summary>
        /// <param name="email">E-mail</param>
        /// <param name="nomeUsuario">Nome do usuário</param>
        /// <returns></returns>
        public static void EnviarEmail(Email email, string nomeUsuario)
        {
            try
            {
                var smtpClient = new SmtpClient();
                var basicCredential = new NetworkCredential (email.Usuario, email.Senha);

                smtpClient.EnableSsl = email.UsarSsl;
                smtpClient.Host = email.ServidorEmail;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                smtpClient.Port = Convert.ToInt32 (email.Porta);

                smtpClient.Timeout = string.IsNullOrEmpty (email.TimeOut) ? 100000 : Convert.ToInt32 (email.TimeOut);

                var fromAddress = new MailAddress (email.EmailRemetente, email.NomeRemetente);
                var vMessage = new MailMessage();
                vMessage.From = fromAddress;
                vMessage.IsBodyHtml = true;
                vMessage.Subject = email.Assunto;
                foreach (var destinatario in email.EmailDestinatario)
                {
                    if (string.IsNullOrEmpty (destinatario.Trim()))
                        continue;
                    vMessage.To.Add (destinatario);
                }

                if (vMessage.To.Count == 0)
                    throw new Exception ("Informar um destinatário");

                email.Mensagem = email.Mensagem.Replace ("\n", "<br />");
                vMessage.Body = email.Mensagem;
                //Anexos
                if (email.Anexos != null)
                    for (var i = 0; i < email.Anexos.Count; i++)
                    {
                        Stream stream = new MemoryStream (email.Anexos[i]);
                        stream.Position = 0;
                        vMessage.Attachments.Add (new Attachment (stream, "Anexo" + i, MediaTypeNames.Application.Octet));
                    }

                smtpClient.Send (vMessage);
            }
            catch (Exception ex)
            {
                TraceException (ex);
                throw new Exception ("Uma falha ocorreu ao enviar email", ex);
            }
        }

        /// <summary>
        ///     Trunca string
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="maxLength">Quantidade máxima de caracteres para exibição</param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty (value)) return value;
            return value.Length <= maxLength ? value : value.Substring (0, maxLength);
        }

        /// <summary>
        ///     Retirar zeros a esquerda de uma string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RetirarZerosEsquerda(this string value)
        {
            return string.IsNullOrWhiteSpace (value) ? "" : value.TrimStart ('0');
        }

        /// <summary>
        ///     Converte uma cadeia de caracteres para UTF8
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertParaEncodingUtf8(this string input)
        {
            if (string.IsNullOrEmpty (input))
                return string.Empty;
            var bytes = Encoding.GetEncoding ("iso-8859-8").GetBytes (input);
            return Encoding.UTF8.GetString (bytes);
        }

        /// <summary>
        ///     Converter uma string em inteiro
        /// </summary>
        /// <param name="num">string</param>
        /// <returns>Retorna 0 caso não seja possivel converter</returns>
        public static int ConvertStringParaInt(this string num)
        {
            try
            {
                return Convert.ToInt32 (num);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        ///     Converter uma string vazia em null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertParaVazioToNull(this string str)
        {
            return string.IsNullOrWhiteSpace (str) ? null : str.Trim();
        }

        /// <summary>
        ///     Verifica de as datas estão dentro do intervalo
        /// </summary>
        /// <param name="entrada">Entrada</param>
        /// <param name="date1">Data 1</param>
        /// <param name="date2">Data 2</param>
        /// <returns></returns>
        public static bool Entre(DateTime entrada, DateTime date1, DateTime date2)
        {
            return entrada >= date1 && entrada <= date2;
        }

        /// <summary>
        ///     Detecta na string caracteres especiais
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool TemCaracterEspecial(this string str)
        {
            if (string.IsNullOrWhiteSpace (str)) return false;
            var r = new Regex ("[^0-9a-zA-Z\\s]+",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.IsMatch (str);
        }

        /// <summary>
        ///     Valida CNPJ
        /// </summary>
        /// <param name="cnpj">recebe uma cadeia de caracteres</param>
        /// <returns>retorna um booleano</returns>
        public static bool IsValidCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace (cnpj))
                return false;

            if (cnpj.All (char.IsLetter))
                return false;

            var valida = true;
            var multiplicador1 = new[] {5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};
            var multiplicador2 = new[] {6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace ("/", "").Replace (".", "").Replace ("-", "");

            if (cnpj.Length == 14)
            {
                var verifica = cnpj.Substring (12);
                var tempCnpj = cnpj.Substring (0, 12);
                var soma = 0;

                for (var i = 0; i < 12; i++)
                    soma = soma + int.Parse (tempCnpj[i].ToString())*multiplicador1[i];

                var resto = soma%11;
                resto = resto < 2 ? 0 : 11 - resto;

                var digito = resto.ToString();

                tempCnpj = tempCnpj + digito;

                soma = 0;
                for (var i = 0; i < 13; i++)
                    soma = soma + int.Parse (tempCnpj[i].ToString())*multiplicador2[i];

                resto = soma%11;
                resto = resto < 2 ? 0 : 11 - resto;

                digito = digito + resto;

                if (digito != verifica)
                    valida = false;

                // verifica valores fixos
                if (cnpj == "00000000000000" || cnpj == "1111111111111" ||
                    cnpj == "22222222222222" || cnpj == "3333333333333" ||
                    cnpj == "44444444444444" || cnpj == "5555555555555" ||
                    cnpj == "66666666666666" || cnpj == "7777777777777" ||
                    cnpj == "88888888888888" || cnpj == "9999999999999")
                    valida = false;
            }
            else
            {
                valida = false;
            }
            return valida;
        }

        /// <summary>
        ///     Método valida CPF
        /// </summary>
        /// <param name="cpf">Um Cpf sem caractteres especiais</param>
        /// <returns></returns>
        public static bool IsValidCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace (cpf))
                return false;

            if (cpf.All (char.IsLetter))
                return false;

            bool retorno;
            var multiplicador1 = new[] {10, 9, 8, 7, 6, 5, 4, 3, 2};
            var multiplicador2 = new[] {11, 10, 9, 8, 7, 6, 5, 4, 3, 2};
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace (".", "").Replace ("-", "");

            // verifica valores fixos
            if (cpf == "00000000000" || cpf == "11111111111" ||
                cpf == "22222222222" || cpf == "33333333333" ||
                cpf == "44444444444" || cpf == "55555555555" ||
                cpf == "66666666666" || cpf == "77777777777" ||
                cpf == "88888888888" || cpf == "99999999999")
                return false;

            if (cpf.Length != 11)
                return false;

            // calcula primeiro digito
            tempCpf = cpf.Substring (0, 9);
            soma = 0;

            for (var i = 0; i < 9; i++)
                soma += int.Parse (tempCpf[i].ToString())*multiplicador1[i];

            resto = soma%11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;

            // calcula segundo digito
            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += int.Parse (tempCpf[i].ToString())*multiplicador2[i];

            resto = soma%11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;

            if (cpf == tempCpf)
                retorno = true;
            else
                retorno = false;

            return retorno;
        }

        /// <summary>
        ///     Detecta na string caracteres
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool TemCaracteres(this string str)
        {
            if (string.IsNullOrWhiteSpace (str)) return false;
            var r = new Regex ("[a-zA-Z\\s]+",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.IsMatch (str);
        }

        /// <summary>
        ///     Retorna uma instancia de objeto Rijndael
        /// </summary>
        /// <returns></returns>
        private static Rijndael InstanceRijndael()
        {
            var IV = "UPGHJWHBSOO2RTHGA";
            var sharedSecret = "UJHSSWIYH29N3DGTA";
            var aesAlg = Rijndael.Create();
            aesAlg.Key = Encoding.ASCII.GetBytes (sharedSecret); //key.GetBytes(aesAlg.KeySize / 8); 
            aesAlg.IV = Encoding.ASCII.GetBytes (IV);
            aesAlg.Padding = PaddingMode.PKCS7;

            return aesAlg;
        }

        /// <summary>
        ///     Encripta mensagem
        ///     <para>Algoritmo simétrico de criptografia</para>
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <returns>text encrypted</returns>
        public static string EncryptAes(string text)
        {
            if (string.IsNullOrEmpty (text)) return "";

            string outStr = null; // Encrypted string to return
            RijndaelManaged aesAlg = null; // RijndaelManaged object used to encrypt the data.

            try
            {
                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();

                var oRijndael = InstanceRijndael();
                //Create a decryptor to perform the stream transform.
                var encryptor = aesAlg.CreateEncryptor (oRijndael.Key, oRijndael.IV);
                // Create the streams used for encryption.
                // using (MemoryStream msEncrypt = new MemoryStream())
                var msEncrypt = new MemoryStream();
                // {
                //sing (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                //
                var csEncrypt = new CryptoStream (msEncrypt, encryptor, CryptoStreamMode.Write);

                using (var swEncrypt = new StreamWriter (csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write (text);
                }
                //
                outStr = ArrayBytesToHexString (msEncrypt.ToArray()); //Convert.ToBase64String(msEncrypt.ToArray());
                //}
            }
            catch (ArgumentException ex)
            {
                TraceException (ex);
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        ///     Decripta mensagem
        ///     <para>Algoritmo simétrico de criptografia</para>
        /// </summary>
        /// <param name="cipherText">an string enccrypt</param>
        /// <returns>an string decrypt</returns>
        public static string DecryptAes(string cipherText)
        {
            if (string.IsNullOrEmpty (cipherText)) return "";

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string text;

            try
            {
                // Create the streams used for decryption.                
                var bytes = HexStringToArrayBytes (cipherText);
                using (var msDecrypt = new MemoryStream (bytes))
                {
                    // Create a RijndaelManaged object
                    aesAlg = new RijndaelManaged();
                    var oRijndael = InstanceRijndael();
                    // Create a decrytor to perform the stream transform.
                    var decryptor = aesAlg.CreateDecryptor (oRijndael.Key, oRijndael.IV);
                    //ICryptoTransform decryptor = aesAlg.CreateDecryptor();
                    //using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    // {
                    var csDecrypt = new CryptoStream (msDecrypt, decryptor, CryptoStreamMode.Read);
                    var srDecrypt = new StreamReader (csDecrypt);
                    //using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    // {
                    // Read the decrypted bytes from the decrypting stream
                    // and place them in a string.
                    text = srDecrypt.ReadToEnd();
                    // }
                    //
                }
            }
            catch (InvalidOperationException ex)
            {
                TraceException (ex, "Não foi possivel criptografar senha");
                return "";
            }

            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return text;
        }

        private static byte[] HexStringToArrayBytes(string conteudo)
        {
            var qtdeBytesEncriptados = conteudo.Length/2;
            var arrayConteudoEncriptado = new byte[qtdeBytesEncriptados];
            for (var i = 0; i < qtdeBytesEncriptados; i++)
                arrayConteudoEncriptado[i] = Convert.ToByte (conteudo.Substring (i*2, 2), 16);
            return arrayConteudoEncriptado;
        }

        private static string ArrayBytesToHexString(byte[] conteudo)
        {
            try
            {
                var arrayHex = Array.ConvertAll (conteudo, b => b.ToString ("X2"));
                return string.Concat (arrayHex);
            }
            catch (ArgumentNullException)
            {
                return "";
            }
        }

        /// <summary>
        ///     Redimensina uma imagem
        /// </summary>
        /// <param name="image">Uma imagem</param>
        /// <param name="maxWidth">Tamanho horizontal</param>
        /// <param name="maxHeight">Tamanho vertical</param>
        /// <returns>Imagem redimensionada</returns>
        public static Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            if (image == null) return null;
            var ratioX = (double) maxWidth/image.Width;
            var ratioY = (double) maxHeight/image.Height;
            var ratio = Math.Min (ratioX, ratioY);

            var newWidth = (int) (image.Width*ratio);
            var newHeight = (int) (image.Height*ratio);

            var newImage = new Bitmap (newWidth, newHeight);
            Graphics.FromImage (newImage).DrawImage (image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        /// <summary>
        ///     R   etorma uma imagem a partir de um array de bytes
        /// </summary>
        /// <param name="pbyteImage">Um array de byte</param>
        /// <returns>Uma Imagem</returns>
        public static Image ObterImageDeArrayBytes(byte[] pbyteImage)
        {
            if (pbyteImage == null) return null;

            if (pbyteImage.Length != 0)
            {
                var imgConverter = new ImageConverter();
                return imgConverter.ConvertFrom (pbyteImage) as Image;
            }
            return null;
        }

        /// <summary>
        ///     Retorna um array de byte a partir de uma imagem
        /// </summary>
        /// <param name="img">Imagem</param>
        /// <returns>Um array de bytes</returns>
        public static byte[] ObterArrayDeUmaImagem(Image img)
        {
            if (img == null) return null;
            var ms = new MemoryStream();

            img.Save (ms, ImageFormat.Png);
            return ms.ToArray();
        }

        /// <summary>
        ///     Executa calculo matematico simples a partir de uma string
        /// </summary>
        /// <param name="equacao"></param>
        /// <returns></returns>
        public static decimal EvalJavaScript(string equacao)
        {
            if (string.IsNullOrWhiteSpace (equacao)) return 0;
            var scriptType = Type.GetTypeFromCLSID (Guid.Parse ("0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC"));
            dynamic obj = Activator.CreateInstance (scriptType, false);
            obj.Language = "javascript";
            //equacao = "a=3; 2*a+32-Math.sin(6)"; exemplos
            //equacao = "5 * 5 + Math.sqrt(4);";
            return obj.Eval (equacao);
        }

        /// <summary>
        ///     Retira caracteres especias da string
        /// </summary>
        /// <param name="str">Uma string</param>
        /// <returns></returns>
        public static string RetirarCaracteresEspeciais(this string str)
        {
            if (string.IsNullOrWhiteSpace (str)) return "";
            var novastring = new Regex ("(?:[^a-z0-9 ]|(?<=['\"])s)",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return novastring.Replace (str, string.Empty);
        }

        /// <summary>
        ///     Criar pasta no caminho indicador
        /// </summary>
        /// <param name="caminho">caminho do arquivo</param>
        public static bool CriarPasta(string caminho)
        {
            //Tentar criar diretorio
            Directory.CreateDirectory (caminho);
            return true;
        }

        /// <summary>
        ///     Criar pasta se não existir
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <returns></returns>
        public static void CriarPastaSeNaoExistir(string caminho)
        {
            if (!ExisteDiretorio (caminho)) CriarPasta (caminho);
        }

        /// <summary>
        ///     Verificar se o diretorio existe
        /// </summary>
        /// <param name="caminho">caminho do arquivo</param>
        /// <returns></returns>
        public static bool ExisteDiretorio(string caminho)
        {
            // Determinar se o diretorio existe
            if (Directory.Exists (caminho))
                return true;
            return false;
        }

        /// <summary>
        ///     Deletar arquivos presentes da pasta
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        public static void DeletarArquivosDaPasta(string caminho)
        {
            //Deletar todos os arquivos se houver.... 
            var diretorio = new DirectoryInfo (caminho);
            foreach (var file in diretorio.GetFiles())
                file.Delete();
        }

        /// <summary>
        ///     Deletar arquivo
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        public static void DelatarArquivo(string caminho, string nomeArquivo)
        {
            var c1 = Path.Combine (caminho, nomeArquivo);
            File.Delete (c1);
        }

        /// <summary>
        ///     Escrever arquivo em modo stream, assincronamente
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <param name="conteudo">Conteudo</param>
        public static void EscreverArquivo(string caminho, string nomeArquivo, string conteudo)
        {
            if (caminho == null)
                throw new ArgumentNullException (nameof (caminho), "O caminho do arquivo deve ser informado");
            if (nomeArquivo == null)
                throw new ArgumentNullException (nameof (nomeArquivo), "O nome do arquivo deve ser informado");
            if (conteudo == null)
                throw new ArgumentNullException (nameof (conteudo), "O conteúdo do arquivo deve ser informado");

            CriarPastaSeNaoExistir (caminho);
            var encodedText = Encoding.UTF8.GetBytes (conteudo);
            var c1 = Path.Combine (caminho, nomeArquivo); //Combina caminho do arquivo como nome do arquivo
            using (var sourceStream = new FileStream (c1, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true))
            {
                sourceStream.Write (encodedText, 0, encodedText.Length);
                sourceStream.Close();
            }
        }

        /// <summary>
        ///     Ler arquivo
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        public static string LerArquivo(string caminho, string nomeArquivo)
        {
            var c1 = Path.Combine (caminho, nomeArquivo); //Combina caminho do arquivo como nome do arquivo

            if (File.Exists (c1) == false)
                throw new FileNotFoundException ("Arquivo não encontrado");
            //Lendo arquivo
            string str;
            using (var sourceStream = new StreamReader (c1, Encoding.UTF8))
            {
                str = sourceStream.ReadToEnd();
                sourceStream.Close();
            }
            return str;
        }

        /// <summary>
        ///     Ler arquivo
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        public static byte[] LerArquivoStream(string caminho, string nomeArquivo)
        {
            var c1 = Path.Combine (caminho, nomeArquivo); //Combina caminho do arquivo como nome do arquivo

            if (File.Exists (c1) == false)
                throw new FileNotFoundException ("Arquivo não encontrado");
            //Lendo arquivo
            byte[] stream;
            using (var sourceStream = new FileStream (c1, FileMode.Open, FileAccess.Read, FileShare.Read,
                4096, true))
            {
                stream = new byte[sourceStream.Length];
            }

            return stream;
        }

        /// <summary>
        ///     Tratar nome da URL
        /// </summary>
        /// <param name="nomeservico">Nome do serviço</param>
        /// <returns></returns>
        public static string TratarNomeUrl(string nomeservico)
        {
            Uri uriResult;
            var result = Uri.TryCreate (nomeservico, UriKind.Absolute, out uriResult) &&
                         (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!result) throw new InvalidOperationException ("Url Inválida");
            return uriResult.ToString();
        }

        /// <summary>
        ///     Formatar 1ª letra da palavra em maiúscula
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string PrimeiroCaracterMaiuscula(this string str)
        {
            if (string.IsNullOrEmpty (str)) return "";
            var array = str.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower (array[0]))
                {
                    array[0] = char.ToUpper (array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (var i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower (array[i]))
                    {
                        array[i] = char.ToUpper (array[i]);
                    }
                }
            }
            return new string (array);
        }

        /// <summary>
        ///     Testar conexão com internet
        /// </summary>
        /// <returns></returns>
        public static bool TestarConexaoInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead ("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Validacao

        /// <summary>
        ///     Valida campos com base nas retrições impostas em seu objeto Vo
        ///     <para>Por exemeplo: A classe XVo decora seus atributos com [Required=""]</para>
        /// </summary>
        /// <param name="o">Um objeto Vo</param>
        /// <returns></returns>
        public static List<ValidationResult> ValidarCampos(params object[] o)
        {
            try
            {
                var errors = new List<ValidationResult>();
                foreach (var item in o)
                {
                    //Caso o objeto seja nullo,
                    //então continue o laço, sem contudo executar as proximas rotinas
                    if (item == null) continue;
                    var contexto = new ValidationContext (item, null, null);
                    Validator.TryValidateObject (item, contexto, errors, true);
                }
                return errors;
            }
            catch (Exception ex)
            {
                TraceException (ex);
                return null;
            }
        }

        /// <summary>
        ///     Verifica se o modelo de dados é válido
        /// </summary>
        /// <param name="o">Um objeto Vo</param>
        /// <returns></returns>
        public static bool ModelStateIsvalid(params object[] o)
        {
            //var list = ValidarCampos(o);
            //Caso nao houver campos pendentes de validacao, entao o modelo é valido, true
            //return list.Count == 0;
            return ModelStateIsvalid (null, o);
        }

        public static bool ModelStateIsvalid(List<string> msgErro, params object[] o)
        {
            var list = ValidarCampos (o);
            if (list.Count == 0) return list.Count == 0;
            if (msgErro != null)
            {
                var erros = list.Select (m => m.ErrorMessage);
                msgErro.AddRange (erros);
            }
            //Caso nao houver campos pendentes de validacao, entao o modelo é valido, true
            return list.Count == 0;
        }

        #endregion

        #region Trace Exception

        /// <summary>
        ///     Registra erros do sistema
        /// </summary>
        /// <param name="ex"></param>
        public static void TraceException(Exception ex)
        {
            TraceException (ex, "");
        }

        /// <summary>
        ///     Registra erros do sistema
        /// </summary>
        /// <param name="ex">Um Exception</param>
        /// <param name="msg">Uma mensagem adicional</param>
        public static void TraceException(Exception ex, string msg)

        {
            var str = new StringBuilder();
            var error = new ErrorTrace
            {
                Data = DateTime.Now.ToString ("g"),
                Detalhe = ex.GetType().Name,
                Mensagem = ex.Message,
                StackTrace = ex.StackTrace
            };

            var content = JsonSerialize (error);
            str.Append (content);
            str.Append ("?");
            str.Append (Environment.NewLine);

            EscreverArquivo (CaminhoLog, NomeArqErrorLog, str.ToString());
        }

        /// <summary>
        ///     Obter lista de erros gravados
        /// </summary>
        /// <returns></returns>
        public static ICollection<ErrorTrace> ListarTraceExceptions()
        {
            try
            {
                var lst = new List<ErrorTrace>();
                var c1 = LerArquivo (CaminhoLog, NomeArqErrorLog);
                var d1 = c1.Split (new[] {"?"}, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in d1)
                {
                    var str = item.Replace (Environment.NewLine, string.Empty);
                    if (!string.IsNullOrWhiteSpace (str))
                        lst.Add (JsonConvert.DeserializeObject<ErrorTrace> (str, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            Formatting = Formatting.Indented
                        }));
                }
                return lst;
            }
            catch (Exception ex)
            {
                //Limpar arquivos de erros para poder obter dados da proxima vez...
                //DelatarArquivo(_caminhoErrorLog,_nomeArqErrorLog);
                TraceException (ex);
                //Deserializa
                return new List<ErrorTrace>();
            }
        }

        /// <summary>
        ///     Deletar arquivo de Log
        /// </summary>
        public static void DeletarArquivoLog()
        {
            DelatarArquivo (CaminhoLog, NomeArqErrorLog);
        }

        /// <summary>
        ///     Gerar numero de CPF Válido
        /// </summary>
        /// <returns></returns>
        public static string GerarCpf()
        {
            int soma = 0, resto = 0;
            var multiplicador1 = new int[9] {10, 9, 8, 7, 6, 5, 4, 3, 2};
            var multiplicador2 = new int[10] {11, 10, 9, 8, 7, 6, 5, 4, 3, 2};

            var rnd = new Random();
            var semente = rnd.Next (100000000, 999999999).ToString();

            for (var i = 0; i < 9; i++)
                soma += int.Parse (semente[i].ToString())*multiplicador1[i];

            resto = soma%11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            semente = semente + resto;
            soma = 0;

            for (var i = 0; i < 10; i++)
                soma += int.Parse (semente[i].ToString())*multiplicador2[i];

            resto = soma%11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            semente = semente + resto;
            return semente;
        }

        /// <summary>
        ///     Gerar numero de CNPJ Válido
        /// </summary>
        /// <returns></returns>
        public static string GerarCnpj()
        {
            // Gerar 8 números aleatórios de 0 a 9 e 4 números (0001), e depois calcular eles
            // Colocar eles na maskedTextBox

            var rnd = new Random();
            var n1 = rnd.Next (0, 10);
            var n2 = rnd.Next (0, 10);
            var n3 = rnd.Next (0, 10);
            var n4 = rnd.Next (0, 10);
            var n5 = rnd.Next (0, 10);
            var n6 = rnd.Next (0, 10);
            var n7 = rnd.Next (0, 10);
            var n8 = rnd.Next (0, 10);
            var n9 = 0;
            var n10 = 0;
            var n11 = 0;
            var n12 = 1;

            var Soma1 = n1*5 + n2*4 + n3*3 + n4*2 + n5*9 + n6*8 + n7*7 + n8*6 + n9*5 + n10*4 + n11*3 + n12*2;

            var DV1 = Soma1%11;

            if (DV1 < 2)
            {
                DV1 = 0;
            }
            else
            {
                DV1 = 11 - DV1;
            }

            var Soma2 = n1*6 + n2*5 + n3*4 + n4*3 + n5*2 + n6*9 + n7*8 + n8*7 + n9*6 + n10*5 + n11*4 + n12*3 + DV1*2;

            var DV2 = Soma2%11;

            if (DV2 < 2)
            {
                DV2 = 0;
            }
            else
            {
                DV2 = 11 - DV2;
            }

            return n1.ToString() + n2 + n3 + n4 + n5 + n6 + n7 + n8 + n9 + n10 + n11 + n12 + DV1 + DV2;
        }

        #endregion

        #region Funções XML

        /// <summary>
        ///     Serializa a classe passada para uma string no form
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static string ClasseParaXmlString<T>(T objeto)
        {
            XElement xml;
            var ser = new XmlSerializer (typeof(T));

            using (var memory = new MemoryStream())
            {
                using (TextReader tr = new StreamReader (memory, Encoding.UTF8))
                {
                    ser.Serialize (memory, objeto);
                    memory.Position = 0;
                    xml = XElement.Load (tr);
                    xml.Attributes()
                        .Where (x => x.Name.LocalName.Equals ("xsd") || x.Name.LocalName.Equals ("xsi"))
                        .Remove();
                }
            }
            return XElement.Parse (xml.ToString()).ToString (SaveOptions.DisableFormatting);
        }

        /// <summary>
        ///     Deserializa a classe a partir de uma String contendo a estrutura XML daquela classe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T XmlStringParaClasse<T>(string input) where T : class
        {
            var ser = new XmlSerializer (typeof(T));

            using (var sr = new StringReader (input))
            {
                return (T) ser.Deserialize (sr);
            }
        }

        /// <summary>
        ///     Carrega o objeto da classe com dados do arquivo XML (Deserializa a classe). Atenção o XML deve ter a mesma
        ///     estrutura da classe
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="arquivo">Arquivo XML</param>
        /// <returns>Retorna a classe</returns>
        public static T ArquivoXmlParaClasse<T>(string arquivo) where T : class
        {
            if (!File.Exists (arquivo))
                throw new FileNotFoundException ("Arquivo " + arquivo + " não encontrado!");

            var serializador = new XmlSerializer (typeof(T));
            var stream = new FileStream (arquivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            try
            {
                return (T) serializador.Deserialize (stream);
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        ///     Copia a estrutura e os dados da classe passada para um arquivo XML (Serializa a classe). Use try catch para tratar
        ///     a possível exceção "DirectoryNotFoundException"
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="objeto">Objeto da Classe</param>
        /// <param name="caminho">caminho para salvar o arquivo XML</param>
        /// <param name="nomearquivo">Nome do arquivo</param>
        public static void ClasseParaArquivoXml<T>(T objeto, string caminho, string nomearquivo)
        {
            var dir = Path.GetDirectoryName (caminho);
            if (dir != null && !Directory.Exists (dir))
                throw new DirectoryNotFoundException ("Diretório " + dir + " não encontrado!");

            var xml = ClasseParaXmlString (objeto);
            try
            {
                //Combina o caminho do arquivo e o nome do arquivo
                var caminhonovo = Path.Combine (caminho, nomearquivo);

                var stw = new StreamWriter (caminhonovo);
                stw.WriteLine (xml);
                stw.Close();
            }
            catch (Exception ex)
            {
                throw new Exception ("Não foi possível criar o arquivo " + caminho + "\nRazão:" + ex.Message);
            }
        }

        /// <summary>
        ///     Obtém um node XML no formato string de um arquivo XML. Util por exemplo, para extrair uma NFe de um XML contendo um
        ///     nfeproc, enviNFe, etc.
        /// </summary>
        /// <param name="nomeDoNode"></param>
        /// <param name="arquivoXml"></param>
        /// <returns>Retorna a string contendo o node XML cujo nome foi passado no parâmetro nomeDoNode</returns>
        public static string ObterNodeDeArquivoXml(string nomeDoNode, string arquivoXml)
        {
            var xmlDoc = XDocument.Load (arquivoXml);
            var xmlString = (from d in xmlDoc.Descendants()
                where d.Name.LocalName == nomeDoNode
                select d).FirstOrDefault();

            if (xmlString == null)
                throw new Exception ($"Nenhum objeto {nomeDoNode} encontrado no arquivo {arquivoXml}!");
            return xmlString.ToString();
        }

        /// <summary>
        ///     Obtém um node XML no formato string de um arquivo XML. Util por exemplo, para extrair uma NFe de um XML contendo um
        ///     nfeproc, enviNFe, etc.
        /// </summary>
        /// <param name="nomeDoNode"></param>
        /// <param name="stringXml"></param>
        /// <returns>Retorna a string contendo o node XML cujo nome foi passado no parâmetro nomeDoNode</returns>
        public static string ObterNodeDeStringXml(string nomeDoNode, string stringXml)
        {
            var s = stringXml;
            var xmlDoc = XDocument.Parse (s);
            var xmlString = (from d in xmlDoc.Descendants()
                where d.Name.LocalName == nomeDoNode
                select d).FirstOrDefault();

            if (xmlString == null)
                throw new Exception (string.Format ("Nenhum objeto {0} encontrado no xml!", nomeDoNode));
            return xmlString.ToString();
        }

        #endregion

        #region Json

        /// <summary>
        ///     Converte objeto em JSON
        /// </summary>
        /// <param name="entity">Entidade de dominio</param>
        /// <returns></returns>
        public static string JsonSerialize(object entity)
        {
            var jsonconverter = JsonConvert.SerializeObject (entity, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            });
            return jsonconverter;
        }

        /// <summary>
        ///     Converte JSON em objeto
        /// </summary>
        /// <typeparam name="T">Tipo a ser convertido</typeparam>
        /// <param name="json">String Json</param>
        /// <returns></returns>
        public static object JsonDeserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T> (json);
        }

        #endregion
    }

    public static class EnumExt
    {
        #region  Métodos

        /// <summary>
        ///     Tipos de Juros
        /// </summary>
        public enum Juros
        {
            [Description("Nenhum")] Nenhum = 0,
            [Description("Simples")] Simples = 1,
            [Description("Compostos")] Compostos = 2
        }

        /// <summary>
        ///     Função de extensão de Enums.
        ///     Obtém um atributo associado ao Enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ObterAtributo<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember (value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes (typeof(T), false);
            return (T) attributes[0];
        }

        /// <summary>
        ///     Função de extensão de Enums.
        ///     Obtém a descrição definida no atributo [Description("xx")] para o Enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Descricao(this Enum value)
        {
            var attribute = value.ObterAtributo<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        ///     Formatar Moeda
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatarMoeda(this string str)
        {
            try
            {
                var num = decimal.Parse (str);
                if (string.IsNullOrWhiteSpace (str.RetirarCaracteresEspeciais()))
                {
                    return "";
                }

                var currency = Convert.ToDecimal (str);
                return string.Format ("{0:N}", currency);
            }
            catch (Exception)
            {
                return "0";
            }
        }

        /// <summary>
        ///     Permitir apenas números
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool ENumero(string text)
        {
            var regex = new Regex ("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch (text);
        }

        /// <summary>
        ///     Formatar string para CEP
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormataCep(this string cep)
        {
            try
            {
                return Convert.ToUInt64 (cep).ToString (@"00000\-000");
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        ///     Formatar string para CNPJ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatarCnpj(this string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace (str))
                {
                    return "";
                }

                var str2 = str.RetirarCaracteresEspeciais();
                if (string.IsNullOrWhiteSpace (str2))
                {
                    return "";
                }

                return Convert.ToUInt64 (str2).ToString (@"00\.000\.000\/0000\-00");
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        ///     Formatar data
        ///     <para>dd/mm/aaaa</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatarData(this string str)
        {
            try
            { 
                if (string.IsNullOrWhiteSpace (str)) return "";
                str = str.RetirarCaracteresEspeciais();
                if (str.Length != 8) throw new Exception();
 
                    var dia = str.Substring(0, 2);
                    var mes = str.Substring(2, 2);
                    var ano = str.Substring(4, 4);

                if (!dia.All (char.IsNumber)) throw new Exception();
                if (!mes.All (char.IsNumber)) throw new Exception();
                if (!ano.All (char.IsNumber)) throw new Exception();

                var data = new DateTime(int.Parse(ano),int.Parse(mes),int.Parse(dia));
                return $"{data:dd/MM/yyyy}";
                 
            }
            catch (Exception)
            {
                throw new Exception("Data inválida");
            }
        }

        /// <summary>
        ///     Formatar string para CNPJ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatarCpf(this string str)
        {
            try
            {
                //var isNumeric = int.TryParse(str, out int n);
                //bool b2 = Microsoft.VisualBasic.Information.IsNumeric("1aa");
                if (str == null) return "";
                if (string.IsNullOrWhiteSpace(str)) return "";

                var str2 = str.RetirarCaracteresEspeciais();
                if (string.IsNullOrWhiteSpace(str2)) return "";

                return Convert.ToUInt64(str2).ToString(@"000\.000\.000\-00");

            }
            catch (Exception)
            {
                return str;
                //throw new Exception("Data inválida");
            }
        }

        #endregion
    }
}