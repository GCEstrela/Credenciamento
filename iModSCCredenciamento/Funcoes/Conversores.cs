using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Drawing;
using iModSCCredenciamento.Funcoes;


namespace iModSCCredenciamento.Funcoes
{
    public class Conversores
    {
        public static BitmapSource STRtoIMG(string str)
        {
            try
            {
                System.IO.MemoryStream o = new System.IO.MemoryStream(Convert.FromBase64String(str));
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.StreamSource = o;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                str = null;
                return img;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        //Converte uma imagem para uma string Base64
        public static string IMGtoSTR(BitmapSource img)
        {
            try
            {
                MemoryStream o = new MemoryStream();
                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(img));
                png.Save(o);
                string str;
                str = Convert.ToBase64String(o.GetBuffer());
                o = null;
                png = null;
                return str;
            }
            catch (Exception ex)
            {
                return null;
            }

            
        }

        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            try
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                    enc.Save(outStream);
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                    return new Bitmap(bitmap);
                }
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public static string PDFtoString(string _arquivoPDF)
        {
            try
            {
                byte[] _pdfBytes = File.ReadAllBytes(_arquivoPDF);
                string _pdfBase64 = Convert.ToBase64String(_pdfBytes);
                return _pdfBase64.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static byte[] StringToPDF(string _arquivoSTR)
        {
            try
            {
                byte[] _bytes = Convert.FromBase64String(_arquivoSTR);
                //var _MemoryStream = new MemoryStream(_bytes);
                //return _MemoryStream;
                return _bytes;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
  

}
