// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 10 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class VeiculoService : IVeiculoService
    {
        #region Variaveis Globais

        private readonly IVeiculoRepositorio _repositorio = new VeiculoRepositorio();

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
        ///     Serviços de credenciais
        /// </summary>
        public IVeiculoCredencialService Credencial
        {
            get { return new VeiculoCredencialService(); }
        }

        /// <summary>
        ///     Serviços de Empresas
        ///     Author: Mihai
        /// </summary>
        public IVeiculoEmpresaService Empresa
        {
            get { return new VeiculoEmpresaService(); }
        }

        /// <summary>
        ///     Seguros
        /// </summary>
        public IVeiculoSeguroService Seguro
        {
            get { return new VeiculoSeguroService(); }
        }

        /// <summary>
        ///     Anexos
        /// </summary>
        public IVeiculoAnexoService Anexo
        {
            get { return new VeiculoAnexoService(); }
        }

        /// <summary>
        ///     Serviços Equipamentos
        /// </summary>
        public IEquipamentoVeiculoTipoServicoService Equipamento
        {
            get { return new EquipamentoVeiculoTipoServicoService(); }
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(Veiculo entity)
        {
            try
            {
                _repositorio.Criar(entity);
                //Criar pendências
                #region Criar Pendências

                var pendencia = new Pendencia();
                pendencia.VeiculoId = entity.EquipamentoVeiculoId;
                //--------------------------
                pendencia.CodPendencia = 22;
                Pendencia.CriarPendenciaSistema(pendencia);
                //--------------------------
                pendencia.CodPendencia = 19;
                Pendencia.CriarPendenciaSistema(pendencia);
                //--------------------------
                pendencia.CodPendencia = 24;
                Pendencia.CriarPendenciaSistema(pendencia);
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
        public Veiculo BuscarPelaChave(int id)
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
        public ICollection<Veiculo> Listar(params object[] objects)
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
        public void Alterar(Veiculo entity)
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
        public void Remover(Veiculo entity)
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