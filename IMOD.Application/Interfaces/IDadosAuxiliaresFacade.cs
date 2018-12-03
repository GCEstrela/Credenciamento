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
        ///     Listar Estados
        /// </summary>
        /// <returns></returns>
        ICollection<Estados> ListarEstadosFederacao();

        /// <summary>
        ///     Listar Municipios
        /// </summary>
        /// <returns></returns>
        ICollection<Municipio> ListarMunicipios();

        /// <summary>
        ///     Buscar municipios por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        EstadoView BuscarEstadoMunicipiosPorUf(string uf);

        /// <summary>
        ///     Listar Status
        /// </summary>
        /// <returns></returns>
        ICollection<Status> ListarStatus();

        /// <summary>
        ///     Listar Tipos de Acessos
        /// </summary>
        /// <returns></returns>
        ICollection<TipoAcesso> ListarTiposAcessos();

        /// <summary>
        ///     Listar Tipo de Cobrança
        /// </summary>
        /// <returns></returns>
        ICollection<TipoCobranca> ListarTiposCobranca();

        /// <summary>
        /// Criar, Alterar, Listar e Remover Tipo de Equipamento
        /// </summary>
        /// <returns></returns>
        ITipoEquipamentoService TipoEquipamentoService();

        /// <summary>
        /// CRUD Completo ColaboradorAnexo
        /// </summary>
        /// <returns></returns>
        IColaboradorAnexoService ColaboradorAnexoService();

        /// <summary>
        /// CRUD Completo Relatorio
        /// </summary>
        /// <returns></returns>
        IRelatorioService RelatorioService();


        /// <summary>
        /// CRUD Completo RelatorioGerencial
        /// </summary>
        /// <returns></returns>
        IRelatorioGerencialService RelatorioGerencialService();

        /// <summary>
        /// CRUD Completo LayoutCracha
        /// </summary>
        /// <returns></returns>
        ILayoutCrachaService LayoutCrachaService();

        /// <summary>
        /// CRUD Completo TipoAtividade
        /// </summary>
        /// <returns></returns>
        ITipoAtividadeService TipoAtividadeService();

        /// <summary>
        /// CRUD Completo TipoCobranca
        /// </summary>
        /// <returns></returns>
        ITipoCobrancaService TipoCobrancaService();

        /// <summary>
        /// CRUD Completo TiposAcesso
        /// </summary>
        /// <returns></returns>
        ITiposAcessoService TiposAcessoService();

        /// <summary>
        /// CRUD Completo TipoStatus
        /// </summary>
        /// <returns></returns>
        IStatusService TipoStatusService();

        /// <summary>
        /// CRUD Completo Curso
        /// </summary>
        /// <returns></returns>
        ICursoService CursoService();

        /// <summary>
        /// CRUD Completo AreaAcesso
        /// </summary>
        /// <returns></returns>
        IAreaAcessoService AreaAcessoService();

        #endregion
    }
}