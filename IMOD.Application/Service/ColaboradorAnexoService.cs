// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Linq;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class ColaboradorAnexoService : IColaboradorAnexoService
    {
        private readonly IColaboradorAnexoRepositorio _repositorio;

        #region  Propriedades

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }

        #endregion

        public ColaboradorAnexoService()
        {
            try
            {
                _repositorio = new ColaboradorAnexoRepositorio();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region  Metodos

        /// <summary>
        ///     Listar anexo por nome
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> ListarPorNome(string nomeArquivo)
        {
            try
            {
                return _repositorio.Listar("%" + nomeArquivo + "%", 0);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Listar anexo por colaborador
        /// </summary>
        /// <param name="colaboradorId"></param>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> ListarPorColaborador(int colaboradorId)
        {
            try
            {
                return _repositorio.Listar("", colaboradorId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(ColaboradorAnexo entity)
        {
            try
            {
                _repositorio.Criar(entity);
                #region Retirar pendencias de sistema
                var pendencia = Pendencia.ListarPorColaborador(entity.ColaboradorId)
                     .FirstOrDefault(n => n.PendenciaSistema & n.CodPendencia == 24);
                if (pendencia == null) return;
                Pendencia.Remover(pendencia);
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorAnexo BuscarPelaChave(int id)
        {
            try
            {
                return _repositorio.BuscarPelaChave(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> Listar(params object[] objects)
        {
            try
            {
                return _repositorio.Listar(objects);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     ListarComAnexo
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> ListarComAnexo(params object[] objects)
        {
            try
            {
                return _repositorio.ListarComAnexo(objects);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorAnexo entity)
        {
            try
            {
                _repositorio.Alterar(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(ColaboradorAnexo entity)
        {
            try
            {
                _repositorio.Remover(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }
}