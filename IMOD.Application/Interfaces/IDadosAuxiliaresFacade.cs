// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 10 - 2018
// ***********************************************************************

#region

#endregion

using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IDadosAuxiliaresFacade
    {
        #region  Propriedades

        /// <summary>
        ///     Estados da Federação
        /// </summary>
        /// <returns></returns>
        IEstadoService EstadoService { get; }

        /// <summary>
        ///     Municipios
        /// </summary>
        /// <returns></returns>
        IMunicipioService MunicipioService { get; }

        /// <summary>
        ///     Tipos de Equipamentos
        /// </summary>
        /// <returns></returns>
        ITipoEquipamentoService TipoEquipamentoService { get; }

        /// <summary>
        ///     Anexos de Colaboradores
        /// </summary>
        /// <returns></returns>
        IColaboradorAnexoService ColaboradorAnexoService { get; }

        /// <summary>
        ///     Relatorios
        /// </summary>
        /// <returns></returns>
        IRelatorioService RelatorioService { get; }

        /// <summary>
        ///     Relatórios Gerenciais
        /// </summary>
        /// <returns></returns>
        IRelatorioGerencialService RelatorioGerencialService { get; }

        /// <summary>
        ///     Modelos de Crachá
        /// </summary>
        /// <returns></returns>
        ILayoutCrachaService LayoutCrachaService { get; }

        /// <summary>
        ///     Tipos de Atividade
        /// </summary>
        /// <returns></returns>
        ITipoAtividadeService TipoAtividadeService { get; }

        /// <summary>
        ///     Tipos de Cobrança
        /// </summary>
        /// <returns></returns>
        ITipoCobrancaService TipoCobrancaService { get; }

        /// <summary>
        ///     Tipos de Acesso
        /// </summary>
        /// <returns></returns>
        ITiposAcessoService TiposAcessoService { get; }

        /// <summary>
        ///     Tipos de Status
        /// </summary>
        /// <returns></returns>
        IStatusService StatusService { get; }

        /// <summary>
        ///     Cursos
        /// </summary>
        /// <returns></returns>
        ICursoService CursoService { get; }

        /// <summary>
        ///     Àreas de acesso
        /// </summary>
        /// <returns></returns>
        IAreaAcessoService AreaAcessoService { get; }

        /// <summary>
        ///     Motivos Credenciais
        /// </summary>
        ICredencialMotivoService CredencialMotivoService { get; }

        /// <summary>
        ///     Status Credenciais
        /// </summary>
        ICredencialStatusService CredencialStatusService { get; }

        /// <summary>
        ///     Tecnologias Credenciais
        /// </summary>
        ITecnologiaCredencialService TecnologiaCredencialService { get; }

        /// <summary>
        ///     Tipo Credenciais
        /// </summary>
        ITipoCredencialService TipoCredencialService { get; }

        /// <summary>
        ///     Formato Credenciais
        /// </summary>
        IFormatoCredencialService FormatoCredencialService { get; }

        /// <summary>
        ///     Tipos Combustiveis
        /// </summary>
        ITipoCombustivelService TipoCombustivelService { get; }

        /// <summary>
        /// Tipos de Serviço
        /// </summary>
        ITipoServicoRepositorio TipoServico { get; }

        #endregion
    }
}