// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 21 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.Application.Service
{
    public class ColaboradorAnexoService : IColaboradorAnexoService
    {
        private readonly IColaboradorAnexoService _colaboradorAnexoService = new ColaboradorAnexoService();

        #region  Metodos

        /// <summary>
        ///     Listar anexo por nome
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> ListarPorNome(string nomeArquivo)
        {
            return _colaboradorAnexoService.Listar ("%" + nomeArquivo + "%", 0);
        }

        /// <summary>
        ///     Listar anexo por colaborador
        /// </summary>
        /// <param name="colaboradorId"></param>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> ListarPorColaborador(int colaboradorId)
        {
            return _colaboradorAnexoService.Listar ("", colaboradorId);
        }

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(ColaboradorAnexo entity)
        {
            _colaboradorAnexoService.Criar (entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorAnexo BuscarPelaChave(int id)
        {
            return _colaboradorAnexoService.BuscarPelaChave (id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> Listar(params object[] objects)
        {
            return _colaboradorAnexoService.Listar (objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorAnexo entity)
        {
            _colaboradorAnexoService.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(ColaboradorAnexo entity)
        {
            _colaboradorAnexoService.Remover(entity);
        }

        #endregion
    }
}