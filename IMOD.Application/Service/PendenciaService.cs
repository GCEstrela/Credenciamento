// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class PendenciaService : IPendenciaService
    {
        #region Variaveis Globais

        private readonly IPendenciaRepositorio _repositorio = new PendenciaRepositorio();

        #endregion

        #region  Propriedades

        /// <summary>
        ///     Tipo
        /// </summary>
        public ITipoPendenciaService TipoPendenciaService
        {
            get { return new TipoPendenciaService(); }
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(Pendencia entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Pendencia BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<Pendencia> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(Pendencia entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(Pendencia entity)
        {
            _repositorio.Remover(entity);
        }

        /// <summary>
        ///     Listar Pendencia por Empresa
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public ICollection<Pendencia> ListarPorEmpresa(int empresaId)
        {
            return _repositorio.ListarPorEmpresa (empresaId);
        }

        /// <summary>
        /// Listar Pendencia por Colaborador
        /// </summary>
        /// <param name="colaboradorId"></param> 
        /// <returns></returns>
        public ICollection<Pendencia> ListarPorColaborador(int colaboradorId)
        {
            return _repositorio.ListarPorColaborador (colaboradorId);
        }

        /// <summary>
        /// Listar Pendencia por Veiculo
        /// </summary>
        /// <param name="veiculoId"></param> 
        /// <returns></returns>
        public ICollection<Pendencia> ListarPorVeiculo(int veiculoId)
        {
            return _repositorio.ListarPorVeiculo (veiculoId);
        }

        #endregion

        #region Construtor

        #endregion
    }
}