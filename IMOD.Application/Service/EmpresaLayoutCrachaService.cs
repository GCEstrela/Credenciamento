using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

namespace IMOD.Application.Service
{
    public class EmpresaLayoutCrachaService : IEmpresaLayoutCrachaRepositorio
    {
        #region Variaveis Globais

        private readonly IEmpresaLayoutCrachaRepositorio _repositorio = new EmpresaLayoutCrachaRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmpresaLayoutCracha BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<EmpresaLayoutCracha> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        #endregion
    }
}
