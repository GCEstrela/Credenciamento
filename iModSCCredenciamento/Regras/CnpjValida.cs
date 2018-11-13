using iModSCCredenciamento.Funcoes;
using System.Globalization;
using System.Windows.Controls;

namespace iModSCCredenciamento.Regras
{
    public class CnpjValida : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(false, "O CNPJ é requerido");
            string str = value.ToString();
            var data = str.RetirarCaracteresEspeciais().Replace(" ", "");
            var valid = data.IsValidCnpj();
            if (!valid) return new ValidationResult(false, "CNPJ inválido");

            return ValidationResult.ValidResult;
        }
    }
}

    
