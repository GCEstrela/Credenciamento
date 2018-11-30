using iModSCCredenciamento.Funcoes;
using System.Globalization;
using System.Windows.Controls;

namespace iModSCCredenciamento.Regras
{
    public class RazaoSocial : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(false, "A razão social é requerida");
            string str = value.ToString();
            if(string.IsNullOrWhiteSpace(str)) return new ValidationResult(false, "A razão social é requerida");
            return ValidationResult.ValidResult;
        }
    }
}