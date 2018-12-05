// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace IMOD.Application.Service
{
    public class DadosAuxiliaresFacadeService : IDadosAuxiliaresFacade
    {
        private readonly ITiposAcessoService _acessoService = new TipoAcessoService();
        private readonly ITipoCobrancaService _cobrancaService = new TipoCobrancaService();
        private readonly IEstadoService _serviceEstado = new EstadoService();
        private readonly IMunicipioService _serviceMunicipio = new MunicipioService();
        private readonly IStatusService _statusService = new StatusService();

        #region  Metodos

        /// <summary>
        ///     CRUD Completo Estados
        /// </summary>
        /// <returns></returns>
        public IEstadoService EstadoService
        {
            get { return new EstadoService(); }
        }
        /// <summary>
        ///     CRUD Completo Municipios
        /// </summary>
        /// <returns></returns>
        public IMunicipioService MunicipioService
        {
            get { return new MunicipioService(); }
        }
        /// <summary>
        ///  CRUD Completo Tipo de Equipamento
        /// </summary>
        /// <returns></returns>
        public ITipoEquipamentoService TipoEquipamentoService
        {
            get { return new TipoEquipamentoService(); }
        }

        /// <summary>
        /// CRUD Completo ColaboradorAnexo
        /// </summary>
        /// <returns></returns>
        public IColaboradorAnexoService ColaboradorAnexoService
        {
            get { return new ColaboradorAnexoService(); }
        }
        /// <summary>
        /// CRUD Completo Relatorio
        /// </summary>
        /// <returns></returns>
        public IRelatorioService RelatorioService
        {
            get { return new RelatorioService(); }
        }
        /// <summary>
        /// CRUD Completo RelatorioGerencial
        /// </summary>
        /// <returns></returns>
        public IRelatorioGerencialService RelatorioGerencialService
        {
            get { return new RelatorioGerencialService(); }
        }
        /// <summary>
        /// CRUD Completo LayoutCracha
        /// </summary>
        /// <returns></returns>
        public ILayoutCrachaService LayoutCrachaService
        {
            get { return new LayoutCrachaService(); }
        }
        /// <summary>
        /// CRUD Completo TipoAtividade
        /// </summary>
        /// <returns></returns>
        public ITipoAtividadeService TipoAtividadeService
        {
            get { return new TipoAtividadeService(); }
        }
        /// <summary>
        /// CRUD Completo TipoCobranca
        /// </summary>
        /// <returns></returns>
        public ITipoCobrancaService TipoCobrancaService
        {
            get { return new TipoCobrancaService(); }
        }
        /// <summary>
        /// CRUD Completo TiposAcesso
        /// </summary>
        /// <returns></returns>
        public ITiposAcessoService TiposAcessoService
        {
            get { return new TipoAcessoService(); }
        }
        /// <summary>
        /// CRUD Completo TipoStatus
        /// </summary>
        /// <returns></returns>
        public IStatusService TipoStatusService
        {
            get { return new StatusService(); }
        }
        /// <summary>
        /// CRUD Completo Curso
        /// </summary>
        /// <returns></returns>
        public ICursoService CursoService
        {
            get { return new CursoService(); }
        }
        /// <summary>
        /// CRUD Completo AreaAcesso
        /// </summary>
        /// <returns></returns>
        public IAreaAcessoService AreaAcessoService
        {
            get { return new AreaAcessoService(); }
        }
        /// <summary>
        /// CRUD Completo EmpresaLayoutCracha
        /// </summary>
        /// <returns></returns>
        public IEmpresaLayoutCrachaService EmpresaLayoutCrachaService
        {
            get { return new EmpresaLayoutCrachaService(); }
        }
        /// <summary>
        /// CRUD Completo Empresa Tipo Atividade 
        /// </summary>
        public IEmpresaTipoAtividadeService EmpresaTipoAtividadeService
        {
            get { return new EmpresaTipoAtividadeService(); }
        }

        #endregion
    }
}