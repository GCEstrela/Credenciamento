// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IEmpresaService : IEmpresaRepositorio
    {
        #region  Propriedades

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }

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

        IEmpresaTipoAtividadeService Atividade { get; }

        /// <summary>
        ///     Empresa Layout Crachá serviços
        /// </summary>
        IEmpresaLayoutCrachaService CrachaService { get; }

        /// <summary>
        ///     Empresa Area Acesso Serviços
        /// </summary>
        IEmpresaAreaAcessoService AreaAcessoService { get; }

        /// <summary>
        ///     Veículo Empresa Serviços
        /// </summary>
        IVeiculoEmpresaService VeiculoService { get; }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Verificar se existe CNPJ cadastrado
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        bool ExisteCnpj(string cnpj);

        /// <summary>
        ///     Criar um empresa com contrato básico
        ///     <para>Um contrato básico será criada automaticamente com base nos dados de configuracao</para>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dataValidade">Data de validade</param>
        /// <param name="numContrato">Numero do contrato</param>
        /// <param name="status">Status do cdontrato</param>
        void CriarContrato(Empresa entity, DateTime dataValidade, string numContrato, Status status,ConfiguraSistema configuraSistema);
        bool ExisteSigla(string sigla);

        #endregion
    }
}