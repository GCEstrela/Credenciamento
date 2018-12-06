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
        ///  Estados da Federação
        /// </summary>
        /// <returns></returns>
        IEstadoService EstadoService { get; }

        /// <summary>
        ///    Municipios
        /// </summary>
        /// <returns></returns>
        IMunicipioService MunicipioService { get; }
        /// <summary>
        /// Tipos de Equipamentos
        /// </summary>
        /// <returns></returns>
        ITipoEquipamentoService TipoEquipamentoService { get; }

        /// <summary>
        /// Anexos de Colaboradores
        /// </summary>
        /// <returns></returns>
        IColaboradorAnexoService ColaboradorAnexoService { get; }

        /// <summary>
        /// Relatorios
        /// </summary>
        /// <returns></returns>
        IRelatorioService RelatorioService { get; }

        /// <summary>
        /// Relatórios Gerenciais
        /// </summary>
        /// <returns></returns>
        IRelatorioGerencialService RelatorioGerencialService { get; }

        /// <summary>
        /// Modelos de Crachá
        /// </summary>
        /// <returns></returns>
        ILayoutCrachaService LayoutCrachaService { get; }

        /// <summary>
        /// Tipos de Atividade
        /// </summary>
        /// <returns></returns>
        ITipoAtividadeService TipoAtividadeService { get; }

        /// <summary>
        /// Tipos de Cobrança
        /// </summary>
        /// <returns></returns>
        ITipoCobrancaService TipoCobrancaService { get; }

        /// <summary>
        /// Tipos de Acesso
        /// </summary>
        /// <returns></returns>
        ITiposAcessoService TiposAcessoService { get; }

        /// <summary>
        /// Tipos de Status
        /// </summary>
        /// <returns></returns>
        IStatusService TipoStatusService { get; }

        /// <summary>
        /// Cursos
        /// </summary>
        /// <returns></returns>
        ICursoService CursoService { get; }

        /// <summary>
        /// Àreas de acesso
        /// </summary>
        /// <returns></returns>
        IAreaAcessoService AreaAcessoService { get; }
        
        #endregion
    }
}