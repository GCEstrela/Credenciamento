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
    public class AreaAcessoService : IAreaAcessoService
    {
        #region Variaveis Globais

        private readonly IAreaAcessoRepositorio _repositorio = new AreaAcessoRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        public void Criar(AreaAcesso entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AreaAcesso BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<AreaAcesso> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        public void Alterar(AreaAcesso entity)
        {
            _repositorio.Alterar(entity);
        }

        public void Remover(AreaAcesso entity)
        {
            _repositorio.Remover(entity);
        }

        #endregion
    }
}
