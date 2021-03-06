﻿// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  02 - 08 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
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
        private string _identificador;
        private string _identificacao1;
        private string _identificacao2;

        #region  Propriedades
        public bool Ativo { get; set; }
        public string IdentificadorCardHolderGuid { get; set; }
        public string IdentificadorCredencialGuid { get; set; }
        public string IdentificadorLayoutCrachaGuid { get; set; }
        public int FacilityCode { get; set; }
        public  Image Foto { get; set; }
        public bool  grupoAlterado { get; set; }
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
            get { return _nome.Truncate (50); }
            set { _nome = value; }
        }

        public string Apelido
        {
            get { return _apelido.Truncate(20); }
            set { _apelido = value; }
        }

        public string Cpf
        {
            get { return _cpf.Truncate(11); }
            set { _cpf = value; }
        }

        public string Cnpj
        {
            get { return _cnpj.Truncate(14); }
            set { _cnpj = value; }
        }

        /// <summary>
        /// Um identificador qualificado 
        /// </summary>
        public string Identificador
        {
            get { return _identificador.Truncate(20); ; }
            set { _identificador = value; }
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
        public string Identificacao1
        {
            get { return _identificacao1; }
            set { _identificacao1 = value; }
        }
        public string Identificacao2
        {
            get { return _identificacao2; }
            set { _identificacao2 = value; }
        }

        /// <summary>
        /// Data de validade da crendencial
        /// </summary>
        public DateTime Validade { get; set; }
        /// <summary>
        /// Formato da credencial
        /// </summary>
        public string FormatoCredencial { get; set; }
        public int TecnologiaCredencialId { get; set; }
        public int FormatoCredencialId { get; set; }
        public int Fc { get; set; }
        public bool Regras { get; set; }
        public string GrupoPadrao { get; set; }
        public List<Guid> listadeGrupos { get; set; }
        public List<Guid> ListaGrupos { get; set; }
        #endregion

    }
}