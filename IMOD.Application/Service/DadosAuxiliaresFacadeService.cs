// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
// ***********************************************************************

#region

using System;
using IMOD.Application.Interfaces;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class DadosAuxiliaresFacadeService : IDadosAuxiliaresFacade
    {
        #region  Metodos

        /// <summary>
        ///      Estados
        /// </summary>
        /// <returns></returns>
        public IEstadoService EstadoService
        {
            get { return new EstadoService(); }
        }
        /// <summary>
        ///      Municipios
        /// </summary>
        /// <returns></returns>
        public IMunicipioService MunicipioService
        {
            get { return new MunicipioService(); }
        }
        /// <summary>
        ///   Tipo de Equipamento
        /// </summary>
        /// <returns></returns>
        public ITipoEquipamentoService TipoEquipamentoService
        {
            get { return new TipoEquipamentoService(); }
        }

        /// <summary>
        ///  ColaboradorAnexo
        /// </summary>
        /// <returns></returns>
        public IColaboradorAnexoService ColaboradorAnexoService
        {
            get { return new ColaboradorAnexoService(); }
        }
        /// <summary>
        ///  Relatorio
        /// </summary>
        /// <returns></returns>
        public IRelatorioService RelatorioService
        {
            get { return new RelatorioService(); }
        }
        /// <summary>
        ///  RelatorioGerencial
        /// </summary>
        /// <returns></returns>
        public IRelatorioGerencialService RelatorioGerencialService
        {
            get { return new RelatorioGerencialService(); }
        }
        /// <summary>
        ///  LayoutCracha
        /// </summary>
        /// <returns></returns>
        public ILayoutCrachaService LayoutCrachaService
        {
            get { return new LayoutCrachaService(); }
        }
        /// <summary>
        ///  TipoAtividade
        /// </summary>
        /// <returns></returns>
        public ITipoAtividadeService TipoAtividadeService
        {
            get { return new TipoAtividadeService(); }
        }
        /// <summary>
        ///  TipoCobranca
        /// </summary>
        /// <returns></returns>
        public ITipoCobrancaService TipoCobrancaService
        {
            get { return new TipoCobrancaService(); }
        }
        /// <summary>
        ///  TiposAcesso
        /// </summary>
        /// <returns></returns>
        public ITiposAcessoService TiposAcessoService
        {
            get { return new TipoAcessoService(); }
        }

        /// <summary>
        ///  Status
        /// </summary>
        /// <returns></returns>
        public IStatusService StatusService
        {
            get { return new StatusService(); }
        }

        /// <summary>
        ///  Curso
        /// </summary>
        /// <returns></returns>
        public ICursoService CursoService
        {
            get { return new CursoService(); }
        }
        /// <summary>
        ///  AreaAcesso
        /// </summary>
        /// <returns></returns>
        public IAreaAcessoService AreaAcessoService
        {
            get { return new AreaAcessoService(); }
        }


        /// <summary>
        ///  Credencial Motivo 
        /// </summary>
        public ICredencialMotivoService CredencialMotivoService
        {
            get { return new CredencialMotivoService(); }
        }
        /// <summary>
        ///  Credencial Motivo 
        /// </summary>
        public ICredencialStatusService CredencialStatusService
        {
            get { return new CredencialStatusService(); }
        }
        /// <summary>
        ///  TecnologiaCredencial 
        /// </summary>
        public ITecnologiaCredencialService TecnologiaCredencialService
        {
            get { return new TecnologiaCredencialService(); }
        }

        /// <summary>
        ///  Tipo Credencial 
        /// </summary>
        public ITipoCredencialService TipoCredencialService
        {
            get { return new TipoCredencialService(); }
        }

        /// <summary>
        ///  Tipo Credencial 
        /// </summary>
        public IFormatoCredencialService FormatoCredencialService
        {
            get { return new FormatoCredencialService(); }
        }

        /// <summary>
        ///     Tipos Serviços
        /// </summary>
        public ITipoServicoService TipoServicoService
        {
            get { return new TipoServicoService(); }
        }


        /// <summary>
        ///     Tipos Combustiveis
        /// </summary>
        public ITipoCombustivelService TipoCombustivelService { get { return new TipoCombustivelService(); } }

        public ITipoServicoRepositorio TipoServico
        {
            get { return new TipoServicoService(); }
        }
        /// <summary>
        ///     Tipos Representante
        /// </summary>
        public ITipoRepresentanteService TipoRepresentanteService { get { return new TipoRepresentanteService(); } }
        /// <summary>
        ///     Tipos Representante
        /// </summary>
        public ITipoRepresentanteRepositorio TipoRepresentante
        {
            get { return new TipoRepresentanteService(); }
        }
        


        public IConfiguraSistemaService ConfiguraSistemaService
        {
            get { return new ConfiguraSistemaService(); }
        }
        #endregion
    }
}