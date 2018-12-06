// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IDadosAuxiliaresFacade
    {
        #region  Metodos

        /// <summary>
        ///     CRUD Completo Estados
        /// </summary>
        /// <returns></returns>
        IEstadoService EstadoService { get; }

        /// <summary>
        ///     CRUD Completo Municipios
        /// </summary>
        /// <returns></returns>
        IMunicipioService MunicipioService { get; }
        /// <summary>
        /// CRUD Completo Tipo de Equipamento
        /// </summary>
        /// <returns></returns>
        ITipoEquipamentoService TipoEquipamentoService { get; }

        /// <summary>
        /// CRUD Completo ColaboradorAnexo
        /// </summary>
        /// <returns></returns>
        IColaboradorAnexoService ColaboradorAnexoService { get; }

        /// <summary>
        /// CRUD Completo Relatorio
        /// </summary>
        /// <returns></returns>
        IRelatorioService RelatorioService { get; }

        /// <summary>
        /// CRUD Completo RelatorioGerencial
        /// </summary>
        /// <returns></returns>
        IRelatorioGerencialService RelatorioGerencialService { get; }

        /// <summary>
        /// CRUD Completo LayoutCracha
        /// </summary>
        /// <returns></returns>
        ILayoutCrachaService LayoutCrachaService { get; }

        /// <summary>
        /// CRUD Completo TipoAtividade
        /// </summary>
        /// <returns></returns>
        ITipoAtividadeService TipoAtividadeService { get; }

        /// <summary>
        /// CRUD Completo TipoCobranca
        /// </summary>
        /// <returns></returns>
        ITipoCobrancaService TipoCobrancaService { get; }

        /// <summary>
        /// CRUD Completo TiposAcesso
        /// </summary>
        /// <returns></returns>
        ITiposAcessoService TiposAcessoService { get; }

        /// <summary>
        /// CRUD Completo TipoStatus
        /// </summary>
        /// <returns></returns>
        IStatusService TipoStatusService { get; }

        /// <summary>
        /// CRUD Completo Curso
        /// </summary>
        /// <returns></returns>
        ICursoService CursoService { get; }

        /// <summary>
        /// CRUD Completo AreaAcesso
        /// </summary>
        /// <returns></returns>
        IAreaAcessoService AreaAcessoService { get; }

        /// <summary>
        /// CRUD Completo EmpresaLayoutCracha
        /// </summary>
        /// <returns></returns>
        IEmpresaLayoutCrachaService EmpresaLayoutCrachaService { get; }

        /// <summary>
        /// CRUD Completo Empresa Tipo Atividade 
        /// </summary>
        IEmpresaTipoAtividadeService EmpresaTipoAtividadeService { get; }

        /// <summary>
        /// CRUD Completo CredenciaisMotivosService 
        /// </summary>
        ICredencialMotivoService CredencialMotivoService { get; }

        /// <summary>
        /// CRUD Completo CredenciaisStatusService
        /// </summary>
        ICredencialStatusService CredencialStatusService { get; }

        /// <summary>
        /// CRUD Completo Tecnologias Credenciais
        /// </summary>
        ITecnologiaCredencialService TecnologiaCredencialService { get; }

        /// <summary>
        /// CRUD Completo Tipo Credenciais
        /// </summary>
        ITipoCredencialService TipoCredencialService { get; }

        /// <summary>
        /// CRUD Completo Tipo Credenciais
        /// </summary>
        IFormatoCredencialService FormatoCredencialService { get; }

        #endregion
    }
}