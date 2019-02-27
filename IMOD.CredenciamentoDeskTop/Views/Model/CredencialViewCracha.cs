﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class CredencialViewCracha
    {
        public int ColaboradorCredencialID { get; set; }
        public string Colete { get; set; }
        public DateTime Emissao { get; set; }
        public DateTime Validade { get; set; }
        public string Matricula { get; set; }
        public string Cargo { get; set; }
        public string EmpresaNome { get; set; }
        public string EmpresaApelido { get; set; }
        public string CNPJ { get; set; }
        public string Sigla { get; set; }
        public string Logo { get; set; }
        public string ColaboradorNome { get; set; }
        public string ColaboradorApelido { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string RGOrgLocal { get; set; }
        public string RGOrgUF { get; set; }
        public string TelefoneEmergencia { get; set; }
        public string Foto { get; set; }
        public byte [] Foto2 { get; set; }
        public byte [] Logo2 { get; set; }
        public string Identificacao1 { get; set; }
        public string Identificacao2 { get; set; }
        public string CNHCategoria { get; set; }
        public string LayoutRPT { get; set; }
    }
}