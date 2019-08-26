using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

namespace IMOD.Application.Service
{
    public class CredencialStatusService : ICredencialStatusService
    {
        private readonly ICredencialStatusRepositorio _repositorio = new CredencialStatusRepositorio();

        /// <summary>
        ///     Criar CredencialStatus 
        /// </summary>
        /// <returns></returns>
        public void Criar(CredencialStatus entity)
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
        ///     Buscar CredencialStatus pela chave
        /// </summary>
        /// <returns></returns>
        public CredencialStatus BuscarPelaChave(int id)
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
        ///     Listar CredencialStatus
        /// </summary>
        /// <returns></returns>
        public ICollection<CredencialStatus> Listar(params object[] objects)
        {
            try
            {
                return _repositorio.Listar(objects);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
            
        }
        /// <summary>
        ///     Alterar CredencialStatus
        /// </summary>
        /// <returns></returns>
        public void Alterar(CredencialStatus entity)
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
        ///     Remover CredencialStatus
        /// </summary>
        /// <returns></returns>
        public void Remover(CredencialStatus entity)
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

