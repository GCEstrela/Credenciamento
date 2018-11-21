// ***********************************************************************
// Project: Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IRepositorioBaseAdoNet<TEntity> where TEntity : class
    {
        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        void Criar(TEntity entity);

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        TEntity BuscarPelaChave(int id);

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        ICollection<TEntity> Listar(params object[] objects);

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        void Alterar(TEntity entity);

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        void Remover(TEntity entity);

        #endregion
    }
}