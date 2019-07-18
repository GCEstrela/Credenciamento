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
        ///     Obter dados da credencial pelo numero da credencial
        /// </summary>
        /// <param name="numCredencial"></param>
        /// <returns></returns>
        ColaboradorCredencial ObterCredencialPeloNumeroCredencial(string numCredencial);
        /// <summary>
        ///     Obter dados da credencial pelo numero da credencial
        /// </summary>
        /// <param name="numColete"></param>
        /// <returns></returns>
        ColaboradorCredencial ObterNumeroColete(int colaboradorid, string numColete);
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
        ///     Obter credencial
        /// </summary>
        /// <param name="colaboradorCredencialId"></param>
        /// <returns></returns>
        CredencialView ObterCredencialView(int colaboradorCredencialId);

        /// <summary>
        ///     Obtem a data de validade de uma credencial
        ///     <para>
        ///         Verificar se o contrato é temporário ou permanente,
        ///         sendo permanente, então vale obter a menor data entre
        ///         um curso controlado e uma data de validade do contrato, caso contrario, será concedido prazo de 90 dias a
        ///         partir da data atual
        ///     </para>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="colaboradorId"></param>
        /// <param name="numContrato"></param>
        /// <param name="credencialRepositorio"></param>
        /// <returns></returns>
        DateTime? ObterDataValidadeCredencial(
            ColaboradorCredencial entity,
            int colaboradorId,
            string numContrato,
            ITipoCredencialRepositorio credencialRepositorio);

        /// <summary>
        ///     Obtem a data de validade de uma credencial
        ///     <para>
        ///         Verificar se o contrato é temporário ou permanente,
        ///         sendo permanente, então vale obter a menor data entre
        ///         um curso controlado e uma data de validade do contrato, caso contrario, será concedido prazo de 90 dias a
        ///         partir da data atual
        ///     </para>
        /// </summary>
        /// <param name="tipoCredencialId"></param>
        /// <param name="colaboradorId"></param>
        /// <param name="numContrato"></param>
        /// <param name="credencialRepositorio"></param>
        /// <returns></returns>
        DateTime? ObterDataValidadeCredencial(int tipoCredencialId, int colaboradorId, string numContrato, ITipoCredencialRepositorio credencialRepositorio);

        /// <summary>
        ///     Criar registro credencial obtendo a data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        /// <param name="credencialRepositorio"></param>
        void Criar(ColaboradorCredencial entity, int colaboradorId, ITipoCredencialRepositorio credencialRepositorio);

        /// <summary>
        ///     Alterar registro credencial obtendo a data de validade da credencial
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="colaboradorId"></param>
        /// <param name="credencialRepositorio"></param>
        void Alterar(ColaboradorCredencial entity, int colaboradorId, ITipoCredencialRepositorio credencialRepositorio);

        /// <summary>
        ///     Listar contratos
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<ColaboradorEmpresaView> ListarContratos(params object[] o);

        /// <summary>
        ///    Listar Colaboradores credenciais - concedidas
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        List<ColaboradoresCredenciaisView> ListarColaboradorCredencialConcedidasView(FiltroReportColaboradoresCredenciais entity);


        /// <summary>
        ///    Listar Colaboradores credenciais - vias adicionais
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        List<ColaboradoresCredenciaisView> ListarColaboradorCredencialViaAdicionaisView(FiltroReportColaboradoresCredenciais entity);

        /// <summary>
        ///    Listar Colaboradores credenciais - inválidas
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        List<ColaboradoresCredenciaisView> ListarColaboradorCredencialInvalidasView(FiltroReportColaboradoresCredenciais entity);


        /// <summary>
        ///    Listar Colaboradores credenciais - impressoes 
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        List<ColaboradoresCredenciaisView> ListarColaboradorCredencialImpressoesView(FiltroReportColaboradoresCredenciais entity);


        /// <summary>
        ///    Listar Colaboradores credenciais - permanentes ativos por área
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        List<ColaboradoresCredenciaisView> ListarColaboradorCredencialPermanentePorAreaView(FiltroReportColaboradoresCredenciais entity);

        /// <summary>
        ///    Listar Colaboradores credenciais - destruídas
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        List<ColaboradoresCredenciaisView> ListarColaboradorCredencialDestruidasView(FiltroReportColaboradoresCredenciais entity);

        /// <summary>
        ///    Listar Colaboradores credenciais - extraviadas
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        List<ColaboradoresCredenciaisView> ListarColaboradorCredencialExtraviadasView(FiltroReportColaboradoresCredenciais entity);

        #endregion
    }
}