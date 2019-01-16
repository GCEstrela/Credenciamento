using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

namespace IMOD.Application.Service
{
    public class ColaboradorCredencialImpressaoService : IColaboradorCredencialImpressaoService
    {
        #region Variaveis Globais

        private readonly IColaboradorCredencialimpresssaoRepositorio _repositorio = new ColaboradorCredencialimpresssaoRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Criar(ColaboradorCredencialimpresssao entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ColaboradorCredencialimpresssao BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<ColaboradorCredencialimpresssao> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public void Alterar(ColaboradorCredencialimpresssao entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Remover
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public void Remover(ColaboradorCredencialimpresssao entity)
        {
            _repositorio.Remover(entity);
        }

        #endregion
    }
}
