using iModSCCredenciamento.Funcoes; 
using System.Globalization; 
using System.Windows.Controls;

namespace iModSCCredenciamento.Regras
{
    public class CpfValida : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(false,"O CPF é requerido");
            string str = value.ToString();
            var data = str.RetirarCaracteresEspeciais().Replace(" ","");
            var valid = data.IsValidCpf();
            if (!valid) return new ValidationResult(false,"CPF inválido");

            return ValidationResult.ValidResult;
        }
    }
}
