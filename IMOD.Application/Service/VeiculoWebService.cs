// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 10 - 2018
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
    public class VeiculoWebService : IVeiculoWebService
    {
        #region Variaveis Globais

        private readonly IVeiculoWebRepositorio _repositorio = new VeiculoWebRepositorio();

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
        public IVeiculoempresaWebService Empresa
        {
            get { return new VeiculoEmpresaWebService(); }
        }

        /// <summary>
        ///     Seguros
        /// </summary>
        public IVeiculoSeguroWebService Seguro
        {
            get { return new VeiculoSeguroWebService(); }
        }

        /// <summary>
        ///     Anexos
        /// </summary>
        public IVeiculoAnexoWebService Anexo
        {
            get { return new VeiculoAnexoWebService(); }
        }

        /// <summary>
        ///     Observações
        /// </summary>
        public IVeiculoObservacaoService Observacao
        {
            get { return new VeiculoObservacaoService(); }
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
        public void Criar(VeiculoWeb entity)
        {
            _repositorio.Criar (entity);
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

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoWeb BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave (id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoWeb> Listar(params object[] objects)
        {
            return _repositorio.Listar (objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoWeb entity)
        {
            _repositorio.Alterar (entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(VeiculoWeb entity)
        {
            _repositorio.Remover (entity);
        }

        #endregion
    }
}