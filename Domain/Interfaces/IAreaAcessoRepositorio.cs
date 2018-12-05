using IMOD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.Interfaces
{
    public interface IAreaAcessoRepositorio : IRepositorioBaseAdoNet<AreaAcesso>
    {
        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        AreaAcesso BuscarPelaChave(int id);

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        ICollection<AreaAcesso> Listar(params object[] objects);
    }
}
