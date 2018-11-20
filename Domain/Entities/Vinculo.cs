// ***********************************************************************
// Project: Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;

#endregion

namespace IMOD.Domain.Entities
{
    public class Vinculo
    {
        public int VinculoId { get; set; }
        public int ColaboradorId { get; set; }
        public string ColaboradorNome { get; set; }
        public string ColaboradorApelido { get; set; }
        public bool Motorista { get; set; }
        public string Cpf { get; set; }
        public string ColaboradorFoto { get; set; }
        public int EmpresaId { get; set; }
        public string EmpresaNome { get; set; }
        public string EmpresaApelido { get; set; }
        public string Cnpj { get; set; }
        public string Cargo { get; set; }
        public string LayoutCrachaGuid { get; set; }
        public string FormatIdguid { get; set; }
        public string NumeroCredencial { get; set; }
        public int Fc { get; set; }
        public DateTime? Validade { get; set; }
        public Guid CredencialGuid { get; set; }
        public Guid CardHolderGuid { get; set; }
    }
}