#region

using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class CredencialMotivoService : ICredencialMotivoService
    {

        private readonly ICredencialMotivoRepositorio _repositorio = new CredencialMotivoRepositorio();

        /// <summary>
        ///     Criar CredencialMotivo 
        /// </summary>
        /// <returns></returns>
        public void Criar(CredencialMotivo entity)
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
        ///     Buscar CredencialMotivo pela chave
        /// </summary>
        /// <returns></returns>
        public CredencialMotivo BuscarPelaChave(int id)
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
        ///     Listar CredencialMotivo
        /// </summary>
        /// <returns></returns>
        public ICollection<CredencialMotivo> Listar(params object[] objects)
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
        ///     Alterar CredencialMotivo
        /// </summary>
        /// <returns></returns>
        public void Alterar(CredencialMotivo entity)
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
        ///     Remover CredencialMotivo
        /// </summary>
        /// <returns></returns>
        public void Remover(CredencialMotivo entity)
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
