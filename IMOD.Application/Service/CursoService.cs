using System;
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
            try
            {
                _repositorio.Criar(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        ///     Buscar Cursos pela chave
        /// </summary>
        /// <returns></returns>
        public Curso BuscarPelaChave(int id)
        {
            try
            {
                return _repositorio.BuscarPelaChave(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        ///     Listar Cursos
        /// </summary>
        /// <returns></returns>
        public ICollection<Curso> Listar(params object[] objects)
        {
            try
            {
                return _repositorio.Listar(objects);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        ///     Alterar Cursos
        /// </summary>
        /// <returns></returns>
        public void Alterar(Curso entity)
        {
            try
            {
                _repositorio.Alterar(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        ///     Remover Cursos
        /// </summary>
        /// <returns></returns>
        public void Remover(Curso entity)
        {
            try
            {
                _repositorio.Remover(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
