// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IPendenciaRepositorio : IRepositorioBaseAdoNet<Pendencia>
    {
        #region  Metodos

        /// <summary>
        ///     Desativar Pendência
        /// </summary>
        /// <param name="entity"></param>
        void Desativar(Pendencia entity);

        /// <summary>
        ///     Listar Pendencia por Empresa
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        ICollection<Pendencia> ListarPorEmpresa(int empresaId);

        /// <summary>
        ///     Listar Pendencia por Colaborador
        /// </summary>
        /// <param name="colaboradorId"></param>
        /// <returns></returns>
        ICollection<Pendencia> ListarPorColaborador(int colaboradorId);

        /// <summary>
        ///     Listar Pendencia por Veiculo
        /// </summary>
        /// <param name="veiculoId"></param>
        /// <returns></returns>
        ICollection<Pendencia> ListarPorVeiculo(int veiculoId);

        /// <summary>
        ///     Criar pendência de sistema
        /// </summary>
        /// <param name="entity"></param>
        void CriarPendenciaSistema(Pendencia entity);

        /// <summary>
        ///     Alterar pendência de sistema
        /// </summary>
        /// <param name="entity"></param>
        void AlterarPendenciaSistema(Pendencia entity);

        #endregion
    }
}