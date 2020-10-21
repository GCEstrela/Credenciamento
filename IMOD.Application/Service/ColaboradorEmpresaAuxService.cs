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
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class ColaboradorEmpresaAuxService : IColaboradorEmpresaAuxService
    {
        #region Variaveis Globais

        private readonly IColaboradorEmpresaAuxRepositorio _repositorio = new ColaboradorEmpresaAuxRepositorio();

        #endregion

        #region  Propriedades

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(ColaboradorEmpresa entity)
        {
            _repositorio.Criar (entity);

            #region Retirar pendencias de sistema
            var pendencia = Pendencia.ListarPorColaborador(entity.ColaboradorId)
                .FirstOrDefault(n => n.PendenciaSistema & n.CodPendencia==22);
            if (pendencia == null) return;
            Pendencia.Remover(pendencia);
            #endregion


        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorEmpresa BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave (id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorEmpresa> Listar(params object[] objects)
        {
            return _repositorio.Listar (objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorEmpresa entity)
        {
            _repositorio.Alterar (entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(ColaboradorEmpresa entity)
        {
            _repositorio.Remover (entity);
        }

        /// <summary>
        ///     Listar View
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorEmpresa> ListarView(params object[] objects)
        {
            return _repositorio.ListarView (objects);
        }
        /// <summary>
        /// Criar numero de matricula
        /// </summary>
        /// <param name="entity"></param>
        public void CriarNumeroMatricula(ColaboradorEmpresa entity)
        {
            _repositorio.CriarNumeroMatricula(entity);
        }

        public ICollection<RelatorioColaboradorEmpresaView> ListarColaboradorEmpresaView(params object[] o)
        {
           return  _repositorio.ListarColaboradorEmpresaView(o);
        }

        public ICollection<RelatorioColaboradorEmpresaView> ListarColaboradorValidadeCursosEmpresaView(params object[] o)
        {
            return _repositorio.ListarColaboradorValidadeCursosEmpresaView(o);
        }

        ICollection<ColaboradorEmpresa> IColaboradorEmpresaAuxRepositorio.BuscarListaIntegracao(int nomedaTabela)
        {
            return _repositorio.BuscarListaIntegracao(nomedaTabela);
        }
        #endregion
    }
}