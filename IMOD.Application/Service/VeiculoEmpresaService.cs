﻿// ***********************************************************************
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
            _repositorio.Criar(entity);
            #region Retirar pendencias de sistema
            var pendencia = Pendencia.ListarPorVeiculo(entity.VeiculoId)
                .FirstOrDefault(n => n.PendenciaSistema & n.CodPendencia ==  22);
            if (pendencia == null) return;
            Pendencia.Remover(pendencia);
            #endregion
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoEmpresa BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoEmpresa> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoEmpresa entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(VeiculoEmpresa entity)
        {
            _repositorio.Remover(entity);
        }

        public ICollection<VeiculoEmpresa> ListarEmpresas(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        public ICollection<VeiculoEmpresaView> ListarContratoView(params object[] o)
        {
            return _repositorio.ListarContratoView(o);
        }

        /// <summary>
        /// Criar numero de matricula
        /// </summary>
        /// <param name="entity"></param>
        public void CriarNumeroMatricula(VeiculoEmpresa entity)
        {
            _repositorio.CriarNumeroMatricula(entity);
        }
        #endregion
    }
}