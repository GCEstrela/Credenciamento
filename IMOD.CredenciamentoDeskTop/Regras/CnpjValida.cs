using System.Globalization;
using System.Windows.Controls;
using IMOD.CrossCutting;

namespace IMOD.CredenciamentoDeskTop.Regras
{
    public class CnpjValida : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(false, "O CNPJ é requerido");
            string str = value.ToString();
            var d1 = str.RetirarCaracteresEspeciais().Replace(" ", "");
            var valid = Utils.IsValidCnpj (d1); 
            if (!valid) return new ValidationResult(false, "CNPJ inválido");

            return ValidationResult.ValidResult;
        }
    }
}

    
