// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IPendenciaRepositorio : IRepositorioBaseAdoNet<Pendencia>
    {
        /// <summary>
        /// Listar Pendencia por Empresa
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        ICollection<Pendencia> ListarPorEmpresa(int empresaId);

        /// <summary>
        /// Listar Pendencia por Colaborador
        /// </summary>
        /// <param name="colaboradorId"></param> 
        /// <returns></returns>
        ICollection<Pendencia> ListarPorColaborador(int colaboradorId);

        /// <summary>
        /// Listar Pendencia por Veiculo
        /// </summary>
        /// <param name="veiculoId"></param> 
        /// <returns></returns>
        ICollection<Pendencia> ListarPorVeiculo(int veiculoId);







    }
}