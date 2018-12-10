using System.Collections.Generic;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

namespace IMOD.Application.Service
{
    public class EmpresaAreaAcessoService : IEmpresaAreaAcessoRepositorio
    {
        #region Variaveis Globais

        private readonly IEmpresaAreaAcessoRepositorio _repositorio = new EmpresaAreaAcessoRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmpresaAreaAcesso BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<EmpresaAreaAcesso> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        #endregion
    }
}
