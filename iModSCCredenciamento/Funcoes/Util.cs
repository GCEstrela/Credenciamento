using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace iModSCCredenciamento.Funcoes
{


    /// <summary>
    ///     Contém Helpers para auxiliar ou extender funcionalidades
    ///     <para>
    ///         Não é permitido uso de controles de interface gráfica com o usuário
    ///     </para>
    /// </summary>
    public static class Utils
    {
        private static readonly string _caminhoErrorLog = Path.Combine(Environment.CurrentDirectory, "ErrorLog");
        private static readonly string _nomeArqErrorLog = "IMOD_ErrorLog";

        #region Diversos

        private static void Valnei()
        {

        }

        /// <summary>
        /// Format Currency
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatCurrency(this string str)
        {
            try
            {
                var num = decimal.Parse(str);
                if (string.IsNullOrWhiteSpace(str.RetirarCaracteresEspeciais())) return "";
                var currency = Convert.ToDecimal(str);
                return string.Format("{0:N}", currency);
            }
            catch (Exception)
            {
                return "0";
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
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        ///     Retirar zeros a esquerda de uma string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RetirarZerosEsquerda(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "" : value.TrimStart('0');
        }

        /// <summary>
        /// Converte uma cadeia de caracteres para UTF8
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TransformarEncodingUtf8(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            byte[] bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(input);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///     Converter uma string em inteiro
        /// </summary>
        /// <param name="num">string</param>
        /// <returns>Retorna 0 caso não seja possivel converter</returns>
        public static int ConvertStringToInt(this string num)
        {
            try
            {
                return Convert.ToInt32(num);
            }
            catch (Exception)
            {
                return 0;
            }
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
        ///     Detecta na String caracteres especiais
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool TemCaracterEspecial(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            var r = new Regex("[^0-9a-zA-Z\\s]+",
                    RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.IsMatch(str);
        }

        /// <summary>
        ///     Valida CNPJ
        /// </summary>
        /// <param name="cnpj">recebe uma cadeia de caracteres</param>
        /// <returns>retorna um booleano</returns>
        public static bool IsValidCnpj(this string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;
            var valida = true;
            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("/", "").Replace(".", "").Replace("-", "");

            if (cnpj.Length == 14)
            {
                var verifica = cnpj.Substring(12);
                var tempCnpj = cnpj.Substring(0, 12);
                var soma = 0;

                for (var i = 0; i < 12; i++)
                    soma = soma + int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

                var resto = soma % 11;
                resto = resto < 2 ? resto = 0 : resto = 11 - resto;
                ;
                var digito = resto.ToString();

                tempCnpj = tempCnpj + digito;

                soma = 0;
                for (var i = 0; i < 13; i++)
                    soma = soma + int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

                resto = soma % 11;
                resto = resto < 2 ? resto = 0 : resto = 11 - resto;
                ;
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
        public static bool IsValidCpf(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;
            var retorno = false;
            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

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
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (var i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;

            // calcula segundo digito
            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
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
        ///     Retorna uma instancia de objeto Rijndael
        /// </summary>
        /// <returns></returns>
        private static Rijndael InstanceRijndael()
        {
            var IV = "UPGHJWHBSOO2RTHG";
            var sharedSecret = "UJHSSWIYH29N3DGT";
            var aesAlg = Rijndael.Create();
            aesAlg.Key = Encoding.ASCII.GetBytes(sharedSecret); //key.GetBytes(aesAlg.KeySize / 8); 
            aesAlg.IV = Encoding.ASCII.GetBytes(IV);
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
            if (string.IsNullOrEmpty(text)) return "";

            string outStr = null; // Encrypted string to return
            RijndaelManaged aesAlg = null; // RijndaelManaged object used to encrypt the data.

            try
            {
                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();

                var oRijndael = InstanceRijndael();
                //Create a decryptor to perform the stream transform.
                var encryptor = aesAlg.CreateEncryptor(oRijndael.Key, oRijndael.IV);
                // Create the streams used for encryption.
                // using (MemoryStream msEncrypt = new MemoryStream())
                var msEncrypt = new MemoryStream();
                // {
                //sing (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                //
                var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(text);
                }
                //
                outStr = ArrayBytesToHexString(msEncrypt.ToArray()); //Convert.ToBase64String(msEncrypt.ToArray());
                //}
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
            if (string.IsNullOrEmpty(cipherText)) return "";

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string text;

            try
            {
                // Create the streams used for decryption.                
                var bytes = HexStringToArrayBytes(cipherText);
                using (var msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    aesAlg = new RijndaelManaged();
                    var oRijndael = InstanceRijndael();
                    // Create a decrytor to perform the stream transform.
                    var decryptor = aesAlg.CreateDecryptor(oRijndael.Key, oRijndael.IV);
                    //ICryptoTransform decryptor = aesAlg.CreateDecryptor();
                    //using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    // {
                    var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                    var srDecrypt = new StreamReader(csDecrypt);
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
            var qtdeBytesEncriptados = conteudo.Length / 2;
            var arrayConteudoEncriptado = new byte[qtdeBytesEncriptados];
            for (var i = 0; i < qtdeBytesEncriptados; i++)
                arrayConteudoEncriptado[i] = Convert.ToByte(conteudo.Substring(i * 2, 2), 16);
            return arrayConteudoEncriptado;
        }

        private static string ArrayBytesToHexString(byte[] conteudo)
        {
            try
            {
                var arrayHex = Array.ConvertAll(conteudo, b => b.ToString("X2"));
                return string.Concat(arrayHex);
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
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
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
            // try
            //{
            if (pbyteImage.Length != 0)
            {
                var imgConverter = new ImageConverter();
                return imgConverter.ConvertFrom(pbyteImage) as Image;
            }
            return null;
            // }
            // catch (Exception ex)
            // {
            //Mbox("Falha ao carregar imagem\n" + ex.Message, MessageBoxIcon.Error);
            //return null;
            //}
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
            // if (picBox.Image != null)
            // {
            img.Save(ms, ImageFormat.Png);
            return ms.ToArray();
            // }
            // else { return null; }
        }

        /// <summary>
        ///     Executa calculo matematico simples a partir de uma string
        /// </summary>
        /// <param name="equacao"></param>
        /// <returns></returns>
        public static decimal EvalJavaScript(string equacao)
        {
            if (string.IsNullOrWhiteSpace(equacao)) return 0;
            var scriptType = Type.GetTypeFromCLSID(Guid.Parse("0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC"));
            dynamic obj = Activator.CreateInstance(scriptType, false);
            obj.Language = "javascript";
            //equacao = "a=3; 2*a+32-Math.sin(6)"; exemplos
            //equacao = "5 * 5 + Math.sqrt(4);";
            return obj.Eval(equacao);
        }

        /// <summary>
        ///     Retira caracteres especias da string
        /// </summary>
        /// <param name="str">Uma string</param>
        /// <returns></returns>
        public static string RetirarCaracteresEspeciais(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return "";
            var novastring = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return novastring.Replace(str, string.Empty);
        }

        /// <summary>
        /// Formatar string para CNPJ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatarCnpj(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return "";
            var str2 = str.RetirarCaracteresEspeciais();
            if (string.IsNullOrWhiteSpace(str2)) return "";
            return Convert.ToUInt64(str2).ToString(@"00\.000\.000\/0000\-00");
        }
        /// <summary>
        /// Formatar string para CNPJ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatarCpf(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return "";
            var str2 = str.RetirarCaracteresEspeciais();
            if (string.IsNullOrWhiteSpace(str2)) return "";
            return Convert.ToUInt64(str2).ToString(@"000\.000\.000\-00");
        }

        /// <summary>
        ///     Criar pasta no caminho indicador
        /// </summary>
        /// <param name="caminho">caminho do arquivo</param>
        public static bool CriarPasta(string caminho)
        {
            //Tentar criar diretorio
            Directory.CreateDirectory(caminho);
            return true;
        }

        /// <summary>
        ///     Criar pasta se não existir
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <returns></returns>
        public static void CriarPastaSeNaoExistir(string caminho)
        {
            if (!ExisteDiretorio(caminho)) CriarPasta(caminho);
        }

        /// <summary>
        ///     Verificar se o diretorio existe
        /// </summary>
        /// <param name="caminho">caminho do arquivo</param>
        /// <returns></returns>
        public static bool ExisteDiretorio(string caminho)
        {
            // Determinar se o diretorio existe
            if (Directory.Exists(caminho))
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
            var diretorio = new DirectoryInfo(caminho);
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
            var c1 = Path.Combine(caminho, nomeArquivo);
            File.Delete(c1);
        }

        /// <summary>
        ///     Escrever arquivo, assincronamente
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <param name="conteudo">Conteudo</param>
        public static Task EscreverArquivoAsync(string caminho, string nomeArquivo, string conteudo)
        {
            if (caminho == null)
                throw new ArgumentNullException(nameof(caminho), "O caminho do arquivo deve ser informado");
            if (nomeArquivo == null)
                throw new ArgumentNullException(nameof(nomeArquivo), "O nome do arquivo deve ser informado");
            if (conteudo == null)
                throw new ArgumentNullException(nameof(conteudo), "O conteúdo do arquivo deve ser informado");
            return Task.Factory.StartNew(() =>
            {
                CriarPastaSeNaoExistir(caminho);
                var encodedText = Encoding.Unicode.GetBytes(conteudo);
                var c1 = Path.Combine(caminho, nomeArquivo); //Combina caminho do arquivo como nome do arquivo
                //Thread.Sleep(10000);//usado para testes
                using (
                        var sourceStream = new FileStream(c1, FileMode.Append, FileAccess.Write, FileShare.None, 4096,
                                true))
                {
                    sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                }
            });
        }

        /// <summary>
        ///     Escrever arquivo
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <param name="conteudo">Conteudo</param>
        public static void EscreverArquivo(string caminho, string nomeArquivo, string conteudo)
        {
            CriarPastaSeNaoExistir(caminho);
            var encodedText = Encoding.Unicode.GetBytes(conteudo);
            var c1 = Path.Combine(caminho, nomeArquivo); //Combina caminho do arquivo como nome do arquivo
            using (var sourceStream = new FileStream(c1, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true))
            {
                sourceStream.Write(encodedText, 0, encodedText.Length);
            }
        }

        /// <summary>
        ///     Ler arquivo, assincronamente
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        public static async Task<string> LerArquivoAsync(string caminho, string nomeArquivo)
        {
            var c1 = Path.Combine(caminho, nomeArquivo); //Combina caminho do arquivo como nome do arquivo

            if (File.Exists(c1) == false)
                throw new FileNotFoundException("Arquivo não encontrado");
            //Lendo arquivo
            using (var sourceStream = new FileStream(c1, FileMode.Open, FileAccess.Read, FileShare.Read,
                    4096, true))
            {
                var sb = new StringBuilder();

                var buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    var text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                return sb.ToString();
            }
        }

        /// <summary>
        ///     Ler arquivo
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        public static string LerArquivo(string caminho, string nomeArquivo)
        {
            var c1 = Path.Combine(caminho, nomeArquivo); //Combina caminho do arquivo como nome do arquivo

            if (File.Exists(c1) == false)
                throw new FileNotFoundException("Arquivo não encontrado");
            //Lendo arquivo
            using (var sourceStream = new FileStream(c1, FileMode.Open, FileAccess.Read, FileShare.Read,
                    4096, true))
            {
                var sb = new StringBuilder();

                var buffer = new byte[0x1000];
                int numRead;
                while ((numRead = sourceStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    var text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);

                }

                return sb.ToString();
            }
        }

        /// <summary>
        ///     Ler arquivo
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        public static byte[] LerArquivoStream(string caminho, string nomeArquivo)
        {
            var c1 = Path.Combine(caminho, nomeArquivo); //Combina caminho do arquivo como nome do arquivo

            if (File.Exists(c1) == false)
                throw new FileNotFoundException("Arquivo não encontrado");
            //Lendo arquivo
            byte[] stream;
            using (var sourceStream = new FileStream(c1, FileMode.Open, FileAccess.Read, FileShare.Read,
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
            var result = Uri.TryCreate(nomeservico, UriKind.Absolute, out uriResult) &&
                         (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!result) throw new InvalidOperationException("Url Inválida");
            return uriResult.ToString();
        }

        /// <summary>
        ///     Formatar 1ª letra da palavra em maiúscula
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string PrimeiroCaracterMaiuscula(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            var array = str.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (var i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        /// <summary>
        /// Permitir apenas números
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsTextAllowed(string text)
        {
            var regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
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
            var ser = new XmlSerializer(typeof(T));

            using (var memory = new MemoryStream())
            {
                using (TextReader tr = new StreamReader(memory, Encoding.UTF8))
                {
                    ser.Serialize(memory, objeto);
                    memory.Position = 0;
                    xml = XElement.Load(tr);
                    xml.Attributes()
                            .Where(x => x.Name.LocalName.Equals("xsd") || x.Name.LocalName.Equals("xsi"))
                            .Remove();
                }
            }
            return XElement.Parse(xml.ToString()).ToString(SaveOptions.DisableFormatting);
        }

        /// <summary>
        ///     Deserializa a classe a partir de uma String contendo a estrutura XML daquela classe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T XmlStringParaClasse<T>(string input) where T : class
        {
            var ser = new XmlSerializer(typeof(T));

            using (var sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
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
            if (!File.Exists(arquivo))
                throw new FileNotFoundException("Arquivo " + arquivo + " não encontrado!");

            var serializador = new XmlSerializer(typeof(T));
            var stream = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            try
            {
                return (T)serializador.Deserialize(stream);
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
            var dir = Path.GetDirectoryName(caminho);
            if (dir != null && !Directory.Exists(dir))
                throw new DirectoryNotFoundException("Diretório " + dir + " não encontrado!");

            var xml = ClasseParaXmlString(objeto);
            try
            {
                //Combina o caminho do arquivo e o nome do arquivo
                var caminhonovo = Path.Combine(caminho, nomearquivo);

                var stw = new StreamWriter(caminhonovo);
                stw.WriteLine(xml);
                stw.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível criar o arquivo " + caminho + "\nRazão:" + ex.Message);
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
            var xmlDoc = XDocument.Load(arquivoXml);
            var xmlString = (from d in xmlDoc.Descendants()
                             where d.Name.LocalName == nomeDoNode
                             select d).FirstOrDefault();

            if (xmlString == null)
                throw new Exception($"Nenhum objeto {nomeDoNode} encontrado no arquivo {arquivoXml}!");
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
            var xmlDoc = XDocument.Parse(s);
            var xmlString = (from d in xmlDoc.Descendants()
                             where d.Name.LocalName == nomeDoNode
                             select d).FirstOrDefault();

            if (xmlString == null)
                throw new Exception(string.Format("Nenhum objeto {0} encontrado no xml!", nomeDoNode));
            return xmlString.ToString();
        }

        #endregion

    }

    public static class EnumExt
    {
        #region  Métodos        

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
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
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

        #endregion
    }



}
