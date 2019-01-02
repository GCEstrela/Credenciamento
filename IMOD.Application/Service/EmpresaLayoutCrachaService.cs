using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

namespace IMOD.Application.Service
{
    public class EmpresaLayoutCrachaService : IEmpresaLayoutCrachaService
    {
        #region Variaveis Globais

        private readonly IEmpresaLayoutCrachaRepositorio _repositorio = new EmpresaLayoutCrachaRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(EmpresaLayoutCracha entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmpresaLayoutCracha BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<EmpresaLayoutCracha> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(EmpresaLayoutCracha entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(EmpresaLayoutCracha entity)
        {
            _repositorio.Remover(entity);
        }

        #endregion

        /// <summary>
        /// Remover tipo de atividades por empresa
        /// </summary>
        /// <param name="empresaId"></param>
        public void RemoverPorEmpresa(int empresaId)
        {
            _repositorio.RemoverPorEmpresa(empresaId);
        }

        /// <summary>
        /// Listar layout Crachas
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public ICollection<EmpresaLayoutCrachaView> ListarLayoutCrachaView(params object[] objects)
        {
            return _repositorio.ListarLayoutCrachaView(objects);
        }

        /// <summary>
        /// Listar layout Cracha por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public ICollection<EmpresaLayoutCrachaView> ListarLayoutCrachaPorEmpresaView(int idEmpresa)
        {
            return _repositorio.ListarLayoutCrachaPorEmpresaView(idEmpresa);
        }
    }
}
