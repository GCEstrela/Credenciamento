﻿// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 21 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IColaboradorService : IColaboradorRepositorio
    {
        #region  Metodos
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }

        /// <summary>
        ///     Obter uma lista de colaboradores e suas credenciais
        /// </summary>
        /// <param name="statusCredencialId"></param>
        /// <param name="colaboradorId">Identificador do colaborador</param>
        /// <param name="colaboradorCredencialId">Identiicador credencial</param>
        /// <param name="nomeEmpresa">Nome da empresa</param>
        /// <param name="tipoCredencialId">Identificador da credencial</param>
        /// <returns></returns>
        ICollection<ColaboradoresCredenciaisView> ListarColaboradores(int colaboradorCredencialId, string nomeEmpresa = "", int tipoCredencialId = 0, int statusCredencialId = 0, int colaboradorId = 0);
        /// <summary>
        /// Serviços de credenciais
        /// </summary>
        IColaboradorCredencialService Credencial { get; }

        /// <summary>
        /// Serviços de Empresas
        /// </summary>
        IColaboradorEmpresaService Empresa { get; }

        /// <summary>
        /// Serviços de Cursos
        /// </summary>
        IColaboradorCursoService Curso { get; }

        /// <summary>
        /// Serviços de Anexos
        /// </summary>
        IColaboradorAnexoService Anexo { get; }

        /// <summary>
        /// Verificar se existe CPF cadastrado
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        bool ExisteCpf(string cpf);

        #endregion
    }
}