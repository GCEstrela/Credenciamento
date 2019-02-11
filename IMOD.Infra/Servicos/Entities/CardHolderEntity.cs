// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  02 - 08 - 2019
// ***********************************************************************

#region

using System;
using System.Drawing;
using IMOD.CrossCutting;

#endregion

namespace IMOD.Infra.Servicos.Entities
{
    public class CardHolderEntity
    {
        private string _apelido;
        private string _cnpj;
        private string _cpf;
        private string _empresa;
        private string _matricula;
        private string _nome;
        private string _cargo;
        private string _numeroCredencial;

        #region  Propriedades

        public string IdentificadorCardHolderGuid { get; set; }
        public string IdentificadorCredencialGuid { get; set; }
        public string IdentificadorLayoutCrahaGuid { get; set; }
        public int FacilityCode { get; set; }
        public  Image Foto { get; set; }
        /// <summary>
        /// Numero do cartão da credencial
        /// </summary>
        public string NumeroCredencial
        {
            get { return _numeroCredencial.Truncate(10); }
            set { _numeroCredencial = value; }
        }

        public string Cargo
        {
            get { return _cargo.Truncate(10); }
            set { _cargo = value; }
        }

        public string Nome
        {
            get { return _nome.Truncate (20); }
            set { _nome = value; }
        }

        public string Apelido
        {
            get { return _apelido.Truncate (20); }
            set { _apelido = value; }
        }

        public string Cpf
        {
            get { return _cpf.Truncate (11); }
            set { _cpf = value; }
        }

        public string Cnpj
        {
            get { return _cnpj.Truncate (14); }
            set { _cnpj = value; }
        }

        public string Matricula
        {
            get { return _matricula.Truncate (15); }
            set { _matricula = value; }
        }

        public string Empresa
        {
            get { return _empresa.Truncate (30); }
            set { _empresa = value; }
        }

        public DateTime Validade { get; set; }
        public string FormatoCredencial { get; set; }

        #endregion
         
    }
}