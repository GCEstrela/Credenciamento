using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace iModSCCredenciamento.Funcoes
{
    //[ValueConversion(typeof(string), typeof(bool))]
    public class Check1Converter : IMultiValueConverter // IValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string _parameter = ((string)parameter);

                if (_parameter== "Imprimir")
                {
                    DateTime? _Validade = (DateTime?)values[0];
                    string _CredencialStatusDescricao = (string)values[1];
                    int _LayoutCrachaID = (int)values[2];
                    int _FormatoCredencialID = (int)values[3];
                    string _NumeroCredencial = values[4].ToString().Trim();
                    string _Descricao = (string)values[5];
                    bool _Ativa = (bool)values[6];

                    if (_Validade == null || _CredencialStatusDescricao != "ATIVA" || _LayoutCrachaID == 0 || _Descricao != "ATIVA" || !_Ativa)
                    {
                        return false;
                    }

                    return true;
                }

                if (_parameter == "Salvar")
                {
                    int _CredencialStatusID = (int)values[0];
                    int _ColaboradorEmpresaID = (int)values[1];
                    int _TipoCredencialID = (int)values[2];
                    int _ColaboradorPrivilegio1ID = (int)values[3];
                    int _ColaboradorPrivilegio2ID = (int)values[4];
                    int _LayoutCrachaID = (int)values[5];
                    int _CredencialMotivoID = (int)values[6];

                    if ((_CredencialStatusID == 0 || _ColaboradorEmpresaID == 0 || _TipoCredencialID == 0 || _LayoutCrachaID == 0 || _CredencialMotivoID == 0) ||
                        (_ColaboradorPrivilegio1ID ==0 && _ColaboradorPrivilegio2ID == 0))
                    {
                        return false;
                    }

                    return true;
                }

                if (_parameter == "DadosAutenticacaoCredencial")
                {
                    if (values[0] == null)
                    {
                        return true;
                    }

                    Guid _CredencialGuid = (Guid)values[0];
                    bool _CredencialAtiva = (bool)values[1];
                    if ((_CredencialGuid == null || _CredencialGuid == new Guid("00000000-0000-0000-0000-000000000000")) && _CredencialAtiva)
                    {
                        return true;
                    }

                    return false;
                }

                if ((string)parameter == "StatusCredencial_tb")
                {
                    string _Descricao = (string)values[0];
                    bool _CredencialAtiva = (bool)values[1];

                    if (_CredencialAtiva && _Descricao == "ATIVA")
                    {
                        return Visibility.Visible;
                    }

                    if (_CredencialAtiva && _Descricao != "ATIVA")
                    {
                        return Visibility.Collapsed;
                    }

                    if (!_CredencialAtiva && _Descricao != "ATIVA")
                    {
                        return Visibility.Visible;
                    }

                    if (!_CredencialAtiva && _Descricao == "ATIVA")
                    {
                        return Visibility.Collapsed;
                    }

                }


                else if ((string)parameter == "Impressa")
                {
                    bool _Impressa = (bool)values[0];
                    int _StatusCredencialID = (int)values[1];
                    BitmapImage _CardPrinterAzul = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/CardPrinterAzul.png"));
                    BitmapImage _CardPrinterAmarelo = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/CardPrinterAmarelo.png"));
                    BitmapImage _CardPrinterVermelho = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/CardPrinterVermelho.png"));

                    if (_Impressa && _StatusCredencialID ==1)
                    {
                        return _CardPrinterAzul;
                    }

                    if (_Impressa && _StatusCredencialID != 1)
                    {
                        return _CardPrinterVermelho;
                    }

                    if (!_Impressa && _StatusCredencialID != 1)
                    {
                        return _CardPrinterVermelho;
                    }

                    if (!_Impressa && _StatusCredencialID == 1)
                    {
                        return _CardPrinterAmarelo;
                    }

                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    string _string = (string)value;
        //    if (_string.Trim() == "")
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    return null;
        //}

    }
}
