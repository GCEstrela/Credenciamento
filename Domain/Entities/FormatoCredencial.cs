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
    public class FormatoCredencial
    {
        public int FormatoCredencialId { get; set; }
        public string Descricao { get; set; }
        public Guid? FormatIdguid { get; set; }
    }
}