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
    public interface IVeiculoCredencialRepositorio : IRepositorioBaseAdoNet<VeiculoCredencial>
    {
        #region  Metodos

        /// <summary>
        ///     Listar Veículos e suas credenciais
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<VeiculosCredenciaisView> ListarView(params object[] o);

        /// <summary>
        ///     Listar dados de Autorização de Veículo
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<AutorizacaoView> ListarAutorizacaoView(params object[] o);

        /// <summary>
        ///     Obter credencial
        /// </summary>
        /// <param name="veiculoCredencialId"></param>
        /// <returns></returns>
        AutorizacaoView ObterCredencialView(int veiculoCredencialId);

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
        /// <param name="equiapmentoVeiculoId"></param>
        /// <param name="numContrato"></param>
        /// <param name="credencialRepositorio"></param>
        /// <returns></returns>
        DateTime? ObterDataValidadeCredencial(
            VeiculoCredencial entity,
            int equiapmentoVeiculoId,
            string numContrato,
            ITipoCredencialRepositorio credencialRepositorio);

        /// <summary>
        ///     Criar registro credencial obtendo a data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="veiculoId">Identificador</param>
        /// <param name="credencialRepositorio"></param>
        void Criar(VeiculoCredencial entity, int veiculoId, ITipoCredencialRepositorio credencialRepositorio);

        /// <summary>
        ///     Alterar registro credencial obtendo a data de validade da credencial
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="veiculoId"></param>
        /// <param name="credencialRepositorio"></param>
        void Alterar(VeiculoCredencial entity, int veiculoId, ITipoCredencialRepositorio credencialRepositorio);

        /// <summary>
        ///     Obter dados da credencial
        /// </summary>
        /// <param name="veiculoCredencialId">Identificador</param>
        /// <returns></returns>
        VeiculosCredenciaisView BuscarCredencialPelaChave(int veiculoCredencialId);

        /// <summary>
        ///     Obter dados da credencial pelo numero da credencial
        /// </summary>
        /// <param name="numCredencial"></param>
        /// <returns></returns>
        VeiculoCredencial ObterCredencialPeloNumeroCredencial(string numCredencial);

        /// <summary>
        ///    Listar Colaboradores / credenciais concedidas
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        List<VeiculosCredenciaisView> ListarVeiculoCredencialConcedidasView(FiltroVeiculoCredencial entity);


        /// <summary>
        ///    Listar Colaboradores / credenciais vias adicionais
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        List<VeiculosCredenciaisView> ListarVeiculoCredencialViaAdicionaisView(FiltroVeiculoCredencial entity);

        /// <summary>
        ///    Listar Colaboradores / credenciais concedidas
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        List<VeiculosCredenciaisView> ListarVeiculoCredencialInvalidasView(FiltroVeiculoCredencial entity);
        #endregion
    }
}