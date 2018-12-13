using System;
using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseEmpresasContratos
    {

        public ObservableCollection<EmpresaContrato> EmpresasContratos { get; set; }

        public class EmpresaContrato
        {

            public int EmpresaContratoID { get; set; }
            public int EmpresaID { get; set; }
            public string NumeroContrato { get; set; }
            public string Descricao { get; set; }
            public DateTime? Emissao { get; set; }
            public DateTime? Validade { get; set; }
            public string Terceirizada { get; set; }
            public string Contratante { get; set; }
            public string IsencaoCobranca { get; set; }
            public int TipoCobrancaID { get; set; }
            public int CobrancaEmpresaID { get; set; }
            public string CEP { get; set; }
            public string Endereco { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public int MunicipioID { get; set; }
            public int EstadoID { get; set; }
            public string NomeResp { get; set; }
            public string TelefoneResp { get; set; }
            public string CelularResp { get; set; }
            public string EmailResp { get; set; }
            public int StatusID { get; set; }
            public string NomeArquivo { get; set; }
            public string Arquivo { get; set; }
            public int TipoAcessoID { get; set; }

            public EmpresaContrato CriaCopia(EmpresaContrato _EmpresaContrato)
            {
                return (EmpresaContrato)_EmpresaContrato.MemberwiseClone();
            }
        }
    }
}
