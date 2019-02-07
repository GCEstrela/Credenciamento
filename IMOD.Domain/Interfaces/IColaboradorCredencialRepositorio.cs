// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IColaboradorCredencialRepositorio : IRepositorioBaseAdoNet<ColaboradorCredencial>
    {
        #region  Metodos

        /// <summary>
        ///     Listar Colaboradores e suas credenciais
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<ColaboradoresCredenciaisView> ListarView(params object[] o);

        /// <summary>
        ///     Obter dados da credencial
        /// </summary>
        /// <param name="colaboradorCredencialId">Identificador</param>
        /// <returns></returns>
        ColaboradoresCredenciaisView BuscarCredencialPelaChave(int colaboradorCredencialId);

        /// <summary>
        ///     Listar dados de Credencial (Impressão)
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<CredencialView> ListarCredencialView(int id);

        /// <summary>
        ///     Obtém a menor data de entre um curso do tipo controlado e uma data de validade do contrato
        /// </summary>
        /// <param name="colaboradorId">Identificador do colaborador</param>
        /// <param name="numContrato">Número do contrato</param>
        /// <returns></returns>
        DateTime? ObterMenorData(int colaboradorId, string numContrato);

        /// <summary>
        ///     Criar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        void Criar(ColaboradorCredencial entity, int colaboradorId);

        /// <summary>
        ///  Alterar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="colaboradorId"></param>
        void Alterar(ColaboradorCredencial entity, int colaboradorId);

        /// <summary>
        ///     Listar contratos
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<ColaboradorEmpresaView> ListarContratos(params object[] o);

        #endregion
    }
}