using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IMOD.PreCredenciamentoWeb.Helper
{
    public static class CriptografiaHelper
    {
        const string chaveCriptografia = "PO4ESTREL@FOR";

        /// <summary>
        /// Método para codificar os dados da querystring
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Codificar(string param)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(param);
            using (Aes encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(chaveCriptografia, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    param = Convert.ToBase64String(ms.ToArray());
                }
            }
            return param;
        }

        /// <summary>
        /// Método para decodificar os dados da querystring
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Decodificar(string param)
        {
            param = param.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(param);
            using (Aes encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(chaveCriptografia, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    param = Encoding.Unicode.GetString(ms.ToArray()); 
                }
            }

            return param;
        }



    }
}