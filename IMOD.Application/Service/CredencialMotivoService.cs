#region

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
            _repositorio.Criar(entity);
        }
        /// <summary>
        ///     Buscar CredencialMotivo pela chave
        /// </summary>
        /// <returns></returns>
        public CredencialMotivo BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }
        /// <summary>
        ///     Listar CredencialMotivo
        /// </summary>
        /// <returns></returns>
        public ICollection<CredencialMotivo> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }
        /// <summary>
        ///     Alterar CredencialMotivo
        /// </summary>
        /// <returns></returns>
        public void Alterar(CredencialMotivo entity)
        {
            _repositorio.Alterar(entity);
        }
        /// <summary>
        ///     Remover CredencialMotivo
        /// </summary>
        /// <returns></returns>
        public void Remover(CredencialMotivo entity)
        {
            _repositorio.Remover(entity);
        }
    }
}
