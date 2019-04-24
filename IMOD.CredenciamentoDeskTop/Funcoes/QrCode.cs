using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.CredenciamentoDeskTop.Funcoes
{
    public class QrCode
    {


        public string GerarQrCode(String mensagem, String nomeImagem) 
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator(); 
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(mensagem, QRCodeGenerator.ECCLevel.Q);

            BitmapByteQRCode b = new BitmapByteQRCode();
            b.SetQRCodeData(qrCodeData); 

            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(8);

            var tempArea = Path.GetTempPath();

            qrCodeImage.Save(tempArea + nomeImagem, ImageFormat.Png); 

            return tempArea + nomeImagem;
        }

    }
}
