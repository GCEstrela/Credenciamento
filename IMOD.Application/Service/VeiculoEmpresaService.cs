// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
// ***********************************************************************

#region

using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace IMOD.Application.Service
{
    public class VeiculoEmpresaService : IVeiculoEmpresaService
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }

        #region Variaveis Globais

        private readonly IVeiculoEmpresaRepositorio _repositorio = new VeiculoEmpresaRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoEmpresa entity)
        {
            try
            {
                _repositorio.Criar(entity);
                #region Retirar pendencias de sistema
                var pendencia = Pendencia.ListarPorVeiculo(entity.VeiculoId)
                    .FirstOrDefault(n => n.PendenciaSistema & n.CodPendencia == 22);
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
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoEmpresa BuscarPelaChave(int id)
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
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoEmpresa> Listar(params object[] objects)
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
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoEmpresa entity)
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
        /// <param name="entity"></param>
        public void Remover(VeiculoEmpresa entity)
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

        public ICollection<VeiculoEmpresa> ListarEmpresas(params object[] objects)
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

        public ICollection<VeiculoEmpresaView> ListarContratoView(params object[] o)
        {
            try
            {
                return _repositorio.ListarContratoView(o);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion
    }
}