// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class EmpresaService : IEmpresaService
    {
        #region Variaveis Globais

        private readonly IEmpresaRepositorio _repositorio = new EmpresaRepositorio();

        #endregion

        #region  Propriedades

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }

        /// <summary>
        ///     Signatário serviços
        /// </summary>
        public IEmpresaSignatarioService SignatarioService
        {
            get { return new EmpresaSignatarioService(); }
        }

        /// <summary>
        ///     Contrato serviços
        /// </summary>
        public IEmpresaContratosService ContratoService
        {
            get { return new EmpresaContratoService(); }
        }

        /// <summary>
        ///     Anexo serviços
        /// </summary>
        public IEmpresaAnexoService AnexoService
        {
            get { return new EmpresaAnexoService(); }
        }

        public IEmpresaTipoAtividadeService Atividade
        {
            get { return new EmpresaTipoAtividadeService(); }
        }

        /// <summary>
        ///     Empresa Layout Crachá serviços
        /// </summary>
        public IEmpresaLayoutCrachaService CrachaService
        {
            get { return new EmpresaLayoutCrachaService(); }
        }

        /// <summary>
        ///     Empresa Area Acesso Serviços
        /// </summary>
        public IEmpresaAreaAcessoService AreaAcessoService
        {
            get { return new EmpresaAreaAcessoService(); }
        }

        /// <summary>
        ///     Veículo Empresa Serviços
        /// </summary>
        public IVeiculoEmpresaService VeiculoService
        {
            get { return new VeiculoEmpresaService(); }
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Verificar se existe CNPJ cadastrado
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public bool ExisteCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace (cnpj)) return false;
            var v1 = cnpj.TemCaracteres();
            if(v1)
                throw new InvalidOperationException("O CNPJ não está num formato válido.");
            //Verificar formato válido
            var v2 = Utils.IsValidCnpj (cnpj);
            if (!v2)
                throw new InvalidOperationException("CNPJ inválido.");


            var doc = cnpj.RetirarCaracteresEspeciais();
            var n1 = BuscarEmpresaPorCnpj (doc);
            return n1 != null;
        }

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(Empresa entity)
        {
            _repositorio.Criar (entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Empresa BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave (id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<Empresa> Listar(params object[] objects)
        {
            return _repositorio.Listar (objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(Empresa entity)
        {
            _repositorio.Alterar (entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(Empresa entity)
        {
            _repositorio.Remover (entity);
        }

        /// <summary>
        ///     Buscar empresa por CNPJ
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public Empresa BuscarEmpresaPorCnpj(string cnpj)
        {
            return _repositorio.BuscarEmpresaPorCnpj (cnpj);
        }

        /// <summary>
        ///     Listar Pendencias
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public ICollection<EmpresaPendenciaView> ListarPendencias(int empresaId = 0)
        {
            return _repositorio.ListarPendencias (empresaId);
        }

        /// <summary>
        /// Listar Credenciais Empresa por tipo
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public ICollection<EmpresaTipoCredencialView> ListarTipoCredenciaisEmpresa(int empresaId = 0)

        {
            return _repositorio.ListarTipoCredenciaisEmpresa(empresaId);
        }

        #endregion
    }
}