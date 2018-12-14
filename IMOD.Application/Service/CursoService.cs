using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

namespace IMOD.Application.Service
{
    public class CursoService : ICursoService
    {

        private readonly ICursoRepositorio _repositorio = new CursoRepositorio();

        /// <summary>
        ///     Criar Cursos 
        /// </summary>
        /// <returns></returns>
        public void Criar(Curso entity)
        {
            _repositorio.Criar(entity);
        }
        /// <summary>
        ///     Buscar Cursos pela chave
        /// </summary>
        /// <returns></returns>
        public Curso BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }
        /// <summary>
        ///     Listar Cursos
        /// </summary>
        /// <returns></returns>
        public ICollection<Curso> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }
        /// <summary>
        ///     Alterar Cursos
        /// </summary>
        /// <returns></returns>
        public void Alterar(Curso entity)
        {
            _repositorio.Alterar(entity);
        }
        /// <summary>
        ///     Remover Cursos
        /// </summary>
        /// <returns></returns>
        public void Remover(Curso entity)
        {
            _repositorio.Remover(entity);
        }
    }
}
