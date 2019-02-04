// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IEmpresaContratoRepositorio : IRepositorioBaseAdoNet<EmpresaContrato>
    {
        #region  Metodos

        /// <summary>
        ///     Listar contratos por número
        /// </summary>
        /// <param name="numContrato"></param>
        /// <returns></returns>
        ICollection<EmpresaContrato> ListarPorNumeroContrato(string numContrato);

        /// <summary>
        /// Buscar numero do contrato
        /// </summary>
        /// <param name="numContrato"></param>
        /// <returns></returns>
        EmpresaContrato BuscarContrato(string numContrato);

        /// <summary>
        ///     Listar contratos por descrição
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        ICollection<EmpresaContrato> ListarPorDescricao(string desc);

        /// <summary>
        ///     Listar contratos por empresa
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        ICollection<EmpresaContrato> ListarPorEmpresa(int empresaId);

        #endregion
    }
}