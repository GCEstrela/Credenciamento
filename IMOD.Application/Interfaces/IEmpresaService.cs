// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 05 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Interfaces;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IEmpresaService : IEmpresaRepositorio
    {
        #region  Propriedades

        /// <summary>
        ///     Signatário serviços
        /// </summary>
        IEmpresaSignatarioService SignatarioService { get; }

        /// <summary>
        ///     Contrato serviços
        /// </summary>
        IEmpresaContratosService ContratoService { get; }

        /// <summary>
        ///     Anexo serviços
        /// </summary>
        IEmpresaAnexoService AnexoService { get; }

        /// <summary>
        /// Atividade serviços
        /// </summary>
        IEmpresaTipoAtividadeService TipoAtividadeService { get; }
        /// <summary>
        /// Crachá serviços
        /// </summary>
        IEmpresaLayoutCrachaService CrachaService { get; }


        #endregion
    }
}