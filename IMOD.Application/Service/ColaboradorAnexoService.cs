// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System.Collections.Generic;
using System.Linq;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class ColaboradorAnexoService : IColaboradorAnexoService
    {
        private readonly IColaboradorAnexoRepositorio _repositorio;

        #region  Propriedades

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }

        #endregion

        public ColaboradorAnexoService()
        {
            _repositorio = new ColaboradorAnexoRepositorio();
        }

        #region  Metodos

        /// <summary>
        ///     Listar anexo por nome
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> ListarPorNome(string nomeArquivo)
        {
            return _repositorio.Listar ("%" + nomeArquivo + "%", 0);
        }

        /// <summary>
        ///     Listar anexo por colaborador
        /// </summary>
        /// <param name="colaboradorId"></param>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> ListarPorColaborador(int colaboradorId)
        {
            return _repositorio.Listar ("", colaboradorId);
        }

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(ColaboradorAnexo entity)
        {
            _repositorio.Criar (entity);
            #region Retirar pendencias de sistema
            var pendencia = Pendencia.ListarPorColaborador(entity.ColaboradorId)
                 .FirstOrDefault(n => n.PendenciaSistema & n.CodPendencia == 24);
            if (pendencia == null) return;
            Pendencia.Remover(pendencia);
            #endregion
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorAnexo BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave (id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> Listar(params object[] objects)
        {
            return _repositorio.Listar (objects);
        }

        /// <summary>
        ///     ListarComAnexo
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> ListarComAnexo(params object[] objects)
        {
            return _repositorio.ListarComAnexo(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorAnexo entity)
        {
            _repositorio.Alterar (entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(ColaboradorAnexo entity)
        {
            _repositorio.Remover (entity);
        }

        #endregion
    }
}