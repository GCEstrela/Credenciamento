﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class PendenciaView:ValidacaoModel
    {
        #region  Propriedades
        public int PendenciaId { get; set; }
        /// <summary>
        ///     Códigos válidos
        ///     <para>21 - Dados da ficha Geral</para>
        ///     <para>22 - Dados da ficha Vinculos</para>
        ///     <para>23 - Dados da ficha Treinamentos Certificados</para>
        ///     <para>24 - Dados da ficha Anexo</para>
        ///     <para>25 - Dados da ficha Credenciais</para>
        ///     <para>12 - Dados da ficha Representantes</para>
        ///     <para>13 - Dados da ficha Contato</para>
        ///     <para>19 - Dados da ficha Seguro</para>
        ///     <para>14 - Dados da ficha Contratos</para>
        /// </summary>
        public int CodPendencia { get; set; }
        public string CodPendenciaDescricao { get; set; }
        [Required(ErrorMessage = "O Motivo da pendência é requerido.")]
        public string Descricao { get; set; }
        public DateTime? DataLimite { get; set; }
        public bool Impeditivo { get; set; }
        public int? ColaboradorId { get; set; }
        public int? EmpresaId { get; set; }
        public int? VeiculoId { get; set; }
        public bool PendenciaSistema { get; set; }
        public bool Ativo { get; set; }

        #endregion
    }
}