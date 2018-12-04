using iModSCCredenciamento.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace iModSCCredenciamento.Funcoes
{
    public class Global
    {
        public static string _cnpjEdicao = "";
        public static string CpfEdicao { get; set; }

        public static Boolean Privilegio_Salvar = true;
        //public static Boolean _escape = false;
        public static string _instancia;
        public static string _bancoDados;
        public static string _usuario;
        public static string _senha;

        //public static SqlConnection _Con = new SqlConnection("Data Source=WDPOA-006100\\SQLEXPRESS;Initial Catalog=iModCredenciamento;User ID=imod;Password=imod;Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True");
        //public static SqlConnection _Con = new SqlConnection("Data Source=LUIZHENRIQUE\\SQLEXPRESS;Initial Catalog=iModCredenciamento;User ID=imod;Password=imod;Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True");
        public static string _connectionString = "Data Source=172.16.190.108\\SQLEXPRESS;Initial Catalog=D_iModCredenciamento;User ID=imod;Password=imod;Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";

        public static void AbreConfig()
        {
            try
            {
                XmlNodeList oNoLista;
                XmlDocument xmlConfig = new XmlDocument();
                StreamReader sr = new StreamReader("iModSCCredenciamento_Config.xml");
                xmlConfig.Load(sr);
                sr.Close();
                sr.Dispose();

                oNoLista = xmlConfig.SelectNodes("/CONFIG/BancoDeDados");
                foreach (XmlNode oNo in oNoLista)
                {
                    _instancia = oNo.ChildNodes[0].InnerText.Trim();
                    _bancoDados = oNo.ChildNodes[1].InnerText.Trim();
                    _usuario = oNo.ChildNodes[2].InnerText.Trim();
                    _senha = oNo.ChildNodes[3].InnerText.Trim();

                }
                _connectionString = "Data Source=" + _instancia + ";Initial Catalog=" + _bancoDados +
                ";User ID=" + _usuario + ";Password=" + _senha +
                ";Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";
                //                _connectionString = "Data Source=" + oNo.ChildNodes[0].InnerText.Trim() + ";Initial Catalog=" + oNo.ChildNodes[1].InnerText.Trim() +
                //";User ID=" + oNo.ChildNodes[2].InnerText.Trim() + ";Password=" + oNo.ChildNodes[3].InnerText.Trim() +
                //";Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";


            }
            catch (Exception)
            {
                Log("Não foi possível carregar o arquivo iMod_Config.xml");
            }
        }

        public static void Log(string texto, int funcao = 2, string comando = "")
        {
            //0 - apenas arquivo de log
            //1 - apenas display
            //2 - arquivo de log e display

            object Lock = new object();
            lock (Lock)
            {
                string _eventolog;
                string _eventoDisplay;

                if (texto == "")
                {
                    _eventoDisplay = texto + Environment.NewLine;
                    _eventolog = "";
                }
                else
                {
                    _eventoDisplay = System.DateTime.Now + " " + texto + Environment.NewLine;
                    _eventolog = System.DateTime.Now + " " + texto.Replace("*", "") + Environment.NewLine;
                }



                if (funcao == 1 | funcao == 2)
                {
                    //If _Display_Pg.DisplayEventos Then
                    //    _Display_Pg.Texto = _eventoDisplay
                    //End If
                }

                if (funcao == 0 | funcao == 2)
                {
                    string _path2 = System.IO.Path.GetTempPath() + "ModuloCredenciamento_LogTemp";
                    if (!(Directory.Exists(_path2)))
                    {
                        Directory.CreateDirectory(_path2);
                    }

                    string arqlog = _path2 + "\\iMod_log_" + System.DateTime.Now.ToString("ddMMyy") + ".txt";

                    try
                    {
                        System.IO.File.AppendAllText(arqlog, _eventolog);

                    }
                    catch (Exception)
                    {
                        //MsgBox("Erro na Sub Log do modulo Global_mod:" & ex.Message)

                    }
                    finally
                    {
                    }
                }

                _eventoDisplay = null;
                _eventolog = null;

            }

        }

        public static void AbrirArquivoPDF(string _CaminhoArquivoPDF)
        {
            using (Process p = new Process())
            {
                p.StartInfo = new ProcessStartInfo(_CaminhoArquivoPDF);
                p.Start();
                p.WaitForExit();
                try
                {
                    File.Delete(_CaminhoArquivoPDF);
                }
                catch (Exception ex)
                {
                    Global.Log("Erro na void AbrirArquivoPDF  ex: " + ex);
                }
            }
        }

        /// <summary>
        /// Mensagem por Popup
        /// </summary>
        /// <param name="Texto">Mensagem a ser exibida</param>
        /// <param name="Icone">1-Informação, 2-Interrogação, 3-Exclamação, 4-Proibido</param>
        /// <returns></returns>
        public static bool PopupBox(string Texto, int Icone)
        {
            PopupBox _popupBox = new PopupBox(Texto, Icone);
            _popupBox.ShowDialog();
            return _popupBox.Result;
        }

        public static void PopupPDF(string _arquivoSTRPdf, bool dialog = true)
        {

            PopupPdfViewer _popupPDF = new PopupPdfViewer(_arquivoSTRPdf);
            if (dialog)
            {
                _popupPDF.ShowDialog();
            }
            else
            {
                _popupPDF.Show();
            }

        }

        public static bool CheckDate(String date, bool _proibeBranco = true)
        {

            try
            {

                if (date != "__/__/____")
                {
                    DateTime dt = DateTime.Parse(date);
                }
                else
                {
                    if (_proibeBranco)
                    {
                        //PopupBox("Data não pode ser em branco (pressione [Esc] para cancelar)", 1);

                        return false;
                    }
                }

                return true;
            }
            catch
            {
                //PopupBox("Data Inválida (pressione [Esc] para cancelar)", 1);

                return false;
            }
        }

        public static bool ValidaCNPJ(string cnpj)
        {
            try
            {
                int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int soma;
                int resto;
                string digito;
                string tempdigito;
                string tempCnpj;
                cnpj = cnpj.Trim();
                cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace("_", "");
                if (cnpj.Length != 14)
                {
                    //PopupBox("CNPJ Inválido (pressione [Esc] para cancelar)", 1);
                    return false;
                }

                tempCnpj = cnpj.Substring(0, 12);
                tempdigito = cnpj.Substring(12, 2);
                soma = 0;
                for (int i = 0; i < 12; i++)
                {
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
                }

                resto = (soma % 11);
                if (resto < 2)
                {
                    resto = 0;
                }
                else
                {
                    resto = 11 - resto;
                }

                digito = resto.ToString();
                tempCnpj = tempCnpj + digito;
                soma = 0;
                for (int i = 0; i < 13; i++)
                {
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
                }

                resto = (soma % 11);
                if (resto < 2)
                {
                    resto = 0;
                }
                else
                {
                    resto = 11 - resto;
                }

                digito = digito + resto.ToString();
                if (digito == tempdigito)
                {
                    return true;
                }
                else
                {
                    //PopupBox("CNPJ Inválido (pressione [Esc] para cancelar)", 1);
                    return false;
                }
                //return cnpj.EndsWith(digito);
            }

            catch (Exception)
            {
                //PopupBox("CNPJ Inválido (pressione [Esc] para cancelar)", 1);
                return false;
            }
        }

        public static bool ValidaCPF(string cpf)

        {
            try
            {
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                string tempCpf;

                string digito;
                string tempdigito;

                int soma;

                int resto;

                cpf = cpf.Trim();

                cpf = cpf.Replace(".", "").Replace("-", "").Replace("_", "");

                if (cpf.Length != 11)
                {
                    //PopupBox("CPF Inválido (pressione [Esc] para cancelar)", 1);
                    return false;
                }


                tempCpf = cpf.Substring(0, 9);
                tempdigito = cpf.Substring(9, 2);

                soma = 0;

                for (int i = 0; i < 9; i++)
                {
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
                }

                resto = soma % 11;

                if (resto < 2)
                {
                    resto = 0;
                }
                else
                {
                    resto = 11 - resto;
                }

                digito = resto.ToString();

                tempCpf = tempCpf + digito;

                soma = 0;

                for (int i = 0; i < 10; i++)
                {
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
                }

                resto = soma % 11;

                if (resto < 2)
                {
                    resto = 0;
                }
                else
                {
                    resto = 11 - resto;
                }

                digito = digito + resto.ToString();
                if (digito == tempdigito)
                {
                    return true;
                }
                else
                {
                    //PopupBox("CPF Inválido (pressione [Esc] para cancelar)", 1);
                    return false;
                }
                //return cpf.EndsWith(digito);
            }
            catch (Exception)
            {
                //PopupBox("CPF Inválido (pressione [Esc] para cancelar)", 1);
                return false;
            }
        }

        public static void CheckField(object _controle, bool _proibeBranco = true, string _tooltip = "", string _tipo = "")
        {
            string _controleType = _controle.GetType().ToString();


            if (_controleType == "System.Windows.Controls.TextBox")
            {
                System.Windows.Controls.TextBox _textBox = (System.Windows.Controls.TextBox)_controle;
                switch (_tipo)
                {

                    case ("00/00/0000"):
                        //if (!_escape)
                        //{
                        if (!CheckDate(_textBox.Text, _proibeBranco))
                        {
                            if (PopupBox("Data Inválida (pressione [Esc] para cancelar)", 1))
                            {
                                _textBox.Focus();
                            }
                            else
                            {
                                _textBox.Text = "";
                            }


                        }
                        //}
                        //else
                        //{
                        //    _textBox.Text = "";
                        //}
                        //_escape = false;

                        break;
                    case ("CPF"):

                        //if (!_escape)
                        //{
                        if (!ValidaCPF(_textBox.Text))
                        {
                            if (PopupBox("CPF Inválido (pressione [Esc] para cancelar)", 1))
                            {
                                _textBox.Focus();
                            }
                            else
                            {
                                _textBox.Text = "";
                            }
                        }
                        //}
                        //else
                        //{
                        //    _textBox.Text = "";
                        //}
                        //_escape = false;

                        break;
                    case ("CNPJ"):

                        if (!ValidaCNPJ(_textBox.Text))
                        {
                            if (PopupBox("CNPJ Inválido (pressione [Esc] para cancelar)", 1))
                            {
                                _textBox.Focus();
                            }
                            else
                            {
                                _textBox.Text = "";
                            }
                        }


                        break;
                }
            }
            else if (_controleType == "System.Windows.Controls.TextBox")
            {
                System.Windows.Controls.TextBox _textBox = (System.Windows.Controls.TextBox)_controle;
                if (_textBox.Text.Trim() == "")
                {
                    if (PopupBox("Campo [ " + _tooltip + " ]  em branco é Inválido", 1))
                    {
                        _textBox.Focus();
                    }


                    //if (!_escape)
                    //{
                    //    PopupBox("Campo [ " + _tooltip + " ]  em branco é Inválido", 1);
                    //    _textBox.Focus();
                    //}
                    //_escape = false;

                }
            }
            else if (_controleType == "")
            {

            }

        }

        public static void SetReadonly(object controls, bool Valor)
        {
            //System.Windows.DependencyObject control;

            switch (((System.Windows.DependencyObject)controls).DependencyObjectType.Name)
            {
                case "StackPanel":
                    controls = (System.Windows.Controls.StackPanel)controls;
                    break;
                case "GroupBox":
                    controls = (System.Windows.Controls.GroupBox)controls;
                    break;
            }
            DependencyObject control = (System.Windows.DependencyObject)controls;
            foreach (System.Windows.Controls.TextBox tb in FindVisualChildren<System.Windows.Controls.TextBox>(control))
            {
                tb.IsReadOnly = Valor;
            }
            foreach (System.Windows.Controls.ComboBox tb in FindVisualChildren<System.Windows.Controls.ComboBox>(control))
            {
                tb.IsHitTestVisible = !Valor;
            }
            foreach (System.Windows.Controls.CheckBox tb in FindVisualChildren<System.Windows.Controls.CheckBox>(control))
            {
                tb.IsHitTestVisible = !Valor;
            }
            foreach (System.Windows.Controls.Button tb in FindVisualChildren<System.Windows.Controls.Button>(control))
            {
                tb.IsHitTestVisible = !Valor;
            }
            foreach (CSharpControls.Wpf.ToggleSwitch tb in FindVisualChildren<CSharpControls.Wpf.ToggleSwitch>(control))
            {
                tb.IsHitTestVisible = !Valor;
            }

        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)

                {

                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)

                    {

                        yield return (T)child;

                    }



                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static int GerarID()
        {
            var now = DateTime.Now;
            var zeroDate = DateTime.MinValue.AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second).AddMilliseconds(now.Millisecond);
            int uniqueId = (int)(zeroDate.Ticks / 100000);
            return uniqueId;
        }

        //Tranporta os dados de uma Classe para a outra
        //Máximo (28/11/2018)
        public void TranportarDados(object obj_Origem, int indent, object obj_Destino)
        {
            try
            {
                if (obj_Origem == null) return;
                string indentString = new string(' ', indent);
                Type objType = obj_Origem.GetType();
                Type objType2 = obj_Destino.GetType();
                PropertyInfo[] properties = objType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                PropertyInfo[] properties2 = objType2.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetProperty);

                foreach (PropertyInfo property in properties)
                {
                    object propValue = property.GetValue(obj_Origem, null);
                    var nomeOrigem = property.Name.ToUpper();
                    foreach (PropertyInfo property2 in properties2)
                    {
                        var nomeDestino = property2.Name.ToUpper();
                        object propValue2 = property2.GetValue(obj_Destino, null);
                        if (nomeOrigem == nomeDestino)
                        {
                            property2.SetValue(obj_Destino, propValue);
                            break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
        }
        //public static Image ResizeImagem(Image imgFoto, int percentual)
        //{
        //    float nPorcentagem = ((float)percentual / 100);

        //    int fonteLargura = imgFoto.Width;     //armazena a largura original da imagem origem
        //    int fonteAltura = imgFoto.Height;   //armazena a altura original da imagem origem
        //    int origemX = 0;        //eixo x da imagem origem
        //    int origemY = 0;        //eixo y da imagem origem

        //    int destX = 0;          //eixo x da imagem destino
        //    int destY = 0;          //eixo y da imagem destino
        //                            //Calcula a altura e largura da imagem redimensionada
        //    int destWidth = (int)(fonteLargura * nPorcentagem);
        //    int destHeight = (int)(fonteAltura * nPorcentagem);

        //    //Cria um novo objeto bitmap
        //    Bitmap bmImagem = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
        //    //Define a resolu~ção do bitmap.
        //    bmImagem.SetResolution(imgFoto.HorizontalResolution, imgFoto.VerticalResolution);
        //    //Crima um objeto graphics e defina a qualidade
        //    Graphics grImagem = Graphics.FromImage(bmImagem);
        //    grImagem.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //    //Desenha a imge usando o método DrawImage() da classe grafica
        //    grImagem.DrawImage(imgFoto,
        //        new Rectangle(destX, destY, destWidth, destHeight),
        //        new Rectangle(origemX, origemY, fonteLargura, fonteAltura),
        //        GraphicsUnit.Pixel);

        //    grImagem.Dispose();  //libera o objeto grafico
        //    return bmImagem;
        //}
    }
}
