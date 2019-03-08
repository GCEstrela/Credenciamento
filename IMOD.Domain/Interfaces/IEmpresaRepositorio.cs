// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IEmpresaRepositorio : IRepositorioBaseAdoNet<Empresa>
    {
        /// <summary>
        /// Buscar empresa por CNPJ
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        Empresa BuscarEmpresaPorCnpj(string cnpj);
        /// <summary>
        /// Buscar empresa por Sigla
        /// </summary>
        /// <param name="sigla"></param>
        /// <returns></returns>
        Empresa BuscarEmpresaPorSigla(string sigla);
        /// <summary>
        /// Listar Pendencias
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        ICollection<EmpresaPendenciaView> ListarPendencias(int empresaId = 0);

        /// <summary>
        /// Listar Credenciais Empresa por tipo
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        ICollection<EmpresaTipoCredencialView> ListarTipoCredenciaisEmpresa(int empresaId = 0);

    }
}