// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System.Collections.Generic;
using System.Linq;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class EmpresaContratoService : IEmpresaContratosService
    {
        private readonly IEmpresaContratoRepositorio _repositorio = new EmpresaContratoRepositorio();

        #region  Propriedades

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }

        public IAreaAcessoService AreaAceesso
        {
            get { return new AreaAcessoService(); }
        }

        public ITiposAcessoService TipoAcesso
        {
            get { return new TipoAcessoService(); }
        }

        public IStatusService Status
        {
            get { return new StatusService(); }
        }

        public ITipoCobrancaService TipoCobranca
        {
            get { return new TipoCobrancaService(); }
        }

        public IEstadoService Estado
        {
            get { return new EstadoService(); }
        }

        public IMunicipioService Municipio
        {
            get { return new MunicipioService(); }
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(EmpresaContrato entity)
        {
            _repositorio.Criar (entity);
            #region Retirar pendencias de sistema
            var pendencia = Pendencia.ListarPorEmpresa(entity.EmpresaId)
                .FirstOrDefault(n => n.PendenciaSistema & n.CodPendencia == 14);
            if (pendencia == null) return;
            Pendencia.Remover(pendencia);
            #endregion
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public EmpresaContrato BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave (id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<EmpresaContrato> Listar(params object[] objects)
        {
            return _repositorio.Listar (objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(EmpresaContrato entity)
        {
            _repositorio.Alterar (entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(EmpresaContrato entity)
        {
            _repositorio.Remover (entity);
        }

        /// <summary>
        ///     Listar contratos por número
        /// </summary>
        /// <param name="numContrato"></param>
        /// <returns></returns>
        public ICollection<EmpresaContrato> ListarPorNumeroContrato(string numContrato)
        {
            return _repositorio.ListarPorNumeroContrato (numContrato);
        }

        /// <summary>
        /// Buscar numero do contrato
        /// </summary>
        /// <param name="numContrato"></param>
        /// <returns></returns>
        public EmpresaContrato BuscarContrato(string numContrato)
        {
            return _repositorio.BuscarContrato (numContrato);
        }

        /// <summary>
        ///     Listar contratos por descrição
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public ICollection<EmpresaContrato> ListarPorDescricao(string desc)
        {
            return _repositorio.ListarPorDescricao (desc);
        }

        /// <summary>
        ///     Listar contratos por empresa
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public ICollection<EmpresaContrato> ListarPorEmpresa(int empresaId)
        {
            return _repositorio.ListarPorEmpresa (empresaId);
        }

        #endregion
    }
}