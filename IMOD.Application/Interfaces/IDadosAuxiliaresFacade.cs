// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IDadosAuxiliaresFacade
    {
        #region  Metodos
 

        /// <summary>
        ///     Listar Estados
        /// </summary>
        /// <returns></returns>
        ICollection<Estados> ListarEstadosFederacao();

        /// <summary>
        ///     Listar Municipios
        /// </summary>
        /// <returns></returns>
        ICollection<Municipio> ListarMunicipios();

        /// <summary>
        ///     Buscar municipios por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        EstadoView BuscarEstadoMunicipiosPorUf(string uf);

        /// <summary>
        ///     Listar Status
        /// </summary>
        /// <returns></returns>
        ICollection<Status> ListarStatus();

        /// <summary>
        ///     Listar Tipos de Acessos
        /// </summary>
        /// <returns></returns>
        ICollection<TipoAcesso> ListarTiposAcessos();

        /// <summary>
        ///     Listar Tipo de Cobrança
        /// </summary>
        /// <returns></returns>
        ICollection<TipoCobranca> ListarTiposCobranca();

        ITipoEquipamentoService EquipamentoService();

        #endregion
    }
}