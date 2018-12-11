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
    public class ColaboradorEmpresaService : IColaboradorEmpresaService
    {


        #region Variaveis Globais

        private readonly IColaboradorEmpresaRepositorio _repositorio = new ColaboradorEmpresaRepositorio();

        #endregion

        #region Construtor

        #endregion
        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(ColaboradorEmpresa entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorEmpresa BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorEmpresa> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorEmpresa entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(ColaboradorEmpresa entity)
        {
            _repositorio.Remover(entity);
        }

        /// <summary>
        ///     Listar View
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorEmpresa> ListarView(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }
        #endregion
    }
}
