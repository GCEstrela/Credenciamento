using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Application.Service
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepositorio _repositorio = new CursoRepositorio();

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(Curso entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Curso BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<Curso> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(Curso entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(Curso entity)
        {
            _repositorio.Remover(entity);
        }

        #endregion
    }
}
