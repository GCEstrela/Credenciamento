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
            _repositorio.Criar(entity);
        }
        /// <summary>
        ///     Buscar CredencialStatus pela chave
        /// </summary>
        /// <returns></returns>
        public CredencialStatus BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }
        /// <summary>
        ///     Listar CredencialStatus
        /// </summary>
        /// <returns></returns>
        public ICollection<CredencialStatus> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }
        /// <summary>
        ///     Alterar CredencialStatus
        /// </summary>
        /// <returns></returns>
        public void Alterar(CredencialStatus entity)
        {
            _repositorio.Alterar(entity);
        }
        /// <summary>
        ///     Remover CredencialStatus
        /// </summary>
        /// <returns></returns>
        public void Remover(CredencialStatus entity)
        {
            _repositorio.Remover(entity);
        }
    }
}

