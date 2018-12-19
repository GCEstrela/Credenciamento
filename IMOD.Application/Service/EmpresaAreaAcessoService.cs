using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

namespace IMOD.Application.Service
{
    public class EmpresaAreaAcessoService : IEmpresaAreaAcessoService
    {
        #region Variaveis Globais

        private readonly IEmpresaAreaAcessoRepositorio _repositorio = new EmpresaAreaAcessoRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        public void Criar(EmpresaAreaAcesso entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmpresaAreaAcesso BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<EmpresaAreaAcesso> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        public void Alterar(EmpresaAreaAcesso entity)
        {
            _repositorio.Alterar(entity);
        }

        public void Remover(EmpresaAreaAcesso entity)
        {
            _repositorio.Remover(entity);
        }

        #endregion
    }
}
