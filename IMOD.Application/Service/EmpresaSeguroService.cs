// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class EmpresaSeguroService : IEmpresaSeguroRepositorio
    {
        #region Variaveis Globais

        private readonly IEmpresaSeguroRepositorio _repositorio = new EmpresaSeguroRepositorio();

        #endregion
        

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(EmpresaSeguro entity)
        {
            _repositorio.Criar (entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmpresaSeguro BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave (id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<EmpresaSeguro> Listar(params object[] objects)
        {
            return _repositorio.Listar (objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(EmpresaSeguro entity)
        {
            _repositorio.Alterar (entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(EmpresaSeguro entity)
        {
            _repositorio.Remover (entity);
        }

        #endregion
    }
}