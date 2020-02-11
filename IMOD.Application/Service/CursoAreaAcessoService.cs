using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

namespace IMOD.Application.Service
{
    public class CursoAreaAcessoService : ICursoAreaAcessoService
    {
        #region Variaveis Globais

        private readonly ICursoAreaAcessoRepositorio _repositorio = new CursoAreaAcessoRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Criar(CursosAreasAcessos entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CursosAreasAcessos BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<CursosAreasAcessos> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        public void Alterar(CursosAreasAcessos entity)
        {
            _repositorio.Alterar(entity);
        }

        public void Remover(CursosAreasAcessos entity)
        {
            _repositorio.Remover(entity);
        }

        #endregion
    }
}
