using System;
using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseVeiculosCredenciais
    {
        public ObservableCollection<VeiculoCredencial> VeiculosCredenciais { get; set; }

        public class VeiculoCredencial
        {

            public int VeiculoCredencialID { get; set; }
            public int VeiculoEmpresaID { get; set; }
            public int VeiculoID { get; set; }
            public string ContratoDescricao { get; set; }
            public string EmpresaNome { get; set; }
            public string VeiculoNome { get; set; }
            public int TecnologiaCredencialID { get; set; }
            public string TecnologiaCredencialDescricao { get; set; }
            public int TipoCredencialID { get; set; }
            public string TipoCredencialDescricao { get; set; }
            public int LayoutCrachaID { get; set; }
            public int EmpresaLayoutCrachaID { get; set; }
            public string LayoutCrachaNome { get; set; }
            public int FormatoCredencialID { get; set; }
            public string FormatoCredencialDescricao { get; set; }
            public string NumeroCredencial { get; set; }
            public int FC { get; set; }
            public DateTime? Emissao { get; set; }
            public DateTime? Validade { get; set; }
            public int CredencialStatusID { get; set; }
            public string CredencialStatusDescricao { get; set; }
            public string CredencialGuid { get; set; }
            public string CardHolderGuid { get; set; }
            public int VeiculoPrivilegio1ID { get; set; }
            public int VeiculoPrivilegio2ID { get; set; }
            public string PrivilegioDescricao1 { get; set; }
            public string PrivilegioDescricao2 { get; set; }
            public bool Ativa { get; set; }
            public string ColaboradorApelido { get; set; }
            public bool Motorista { get; set; }
            public string Placa { get; set; }
            public string VeiculoFoto { get; set; }
            public string EmpresaLogo { get; set; }
            public int EmpresaID { get; set; }
            public string EmpresaApelido { get; set; }
            public string CNPJ { get; set; }
            public string Cargo { get; set; }
            public string LayoutCrachaGUID { get; set; }
            public string FormatIDGUID { get; set; }
            public string Colete { get; set; }
            public string EmpresaSigla { get; set; }
            public int CredencialMotivoID { get; set; }
            public bool Impressa { get; set; }
            public DateTime? Baixa { get; set; }

            //public int VeiculoCredencialId { get; set; }
            //public int? VeiculoEmpresaId { get; set; }
            //public int? TecnologiaCredencialId { get; set; }
            //public int? TipoCredencialId { get; set; }
            //public int? LayoutCrachaId { get; set; }
            //public int? FormatoCredencialId { get; set; }
            //public string NumeroCredencial { get; set; }
            //public int? Fc { get; set; }
            //public DateTime? Emissao { get; set; }
            //public DateTime? Validade { get; set; }
            //public int? CredencialStatusId { get; set; }
            //public string CardHolderGuid { get; set; }
            //public string CredencialGuid { get; set; }
            //public int? VeiculoPrivilegio1Id { get; set; }
            //public int? VeiculoPrivilegio2Id { get; set; }
            //public bool Ativa { get; set; }
            //public string Colete { get; set; }
            //public int? CredencialmotivoId { get; set; }
            //public DateTime? Baixa { get; set; }
            //public bool Impressa { get; set; }

            public VeiculoCredencial CriaCopia(VeiculoCredencial _VeiculosCredenciais)
            {
                return (VeiculoCredencial)_VeiculosCredenciais.MemberwiseClone();
            }


        }
    }
}
