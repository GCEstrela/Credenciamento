using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IMOD.PreCredenciamentoWeb.Util
{
    public class ConverterArquivo
    {
        public static string Base64(HttpPostedFileBase file)
        {
            byte[] bufferArquivo;

            var arquivoStream = file.InputStream;

            using (MemoryStream ms = new MemoryStream())
            {
                arquivoStream.CopyTo(ms);
                bufferArquivo = ms.ToArray();
            }

            var arquivoBase64 = Convert.ToBase64String(bufferArquivo);

            return arquivoBase64;
        }
    }
}