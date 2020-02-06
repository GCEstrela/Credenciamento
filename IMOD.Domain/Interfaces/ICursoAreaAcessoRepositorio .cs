using System.Collections.Generic;
using IMOD.Domain.Entities;

namespace IMOD.Domain.Interfaces
{
    public interface ICursoAreaAcessoRepositorio : IRepositorioBaseAdoNet<CursosAreasAcessos>
    {
        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        CursosAreasAcessos BuscarPelaChave(int id);

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        ICollection<CursosAreasAcessos> Listar(params object[] objects);
    }
}
