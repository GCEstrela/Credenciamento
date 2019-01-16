// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Mihainuan de Sá Silva
// Date:  12 - 16 - 2018
// ***********************************************************************

using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;
using System.Collections.Generic;

namespace IMOD.Application.Service
{
    public class ColaboradorCredencialService : IColaboradorCredencialService
    {
        #region Variaveis Globais

        private readonly IColaboradorCredencialRepositorio _repositorio = new ColaboradorCredencialRepositorio();

        #endregion

        #region Construtores

        public ITecnologiaCredencialService TecnologiaCredencial { get { return new TecnologiaCredencialService(); } }

        public ITipoCredencialService TipoCredencial { get { return new TipoCredencialService(); } }

        public ILayoutCrachaService LayoutCracha { get { return new LayoutCrachaService(); } }

        public IFormatoCredencialService FormatoCredencial { get { return new FormatoCredencialService(); } }

        public ICredencialStatusService CredencialStatus { get { return new CredencialStatusService(); } }

        public ICredencialMotivoService CredencialMotivo { get { return new CredencialMotivoService(); } }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorCredencial entity)
        {
            _repositorio.Alterar(entity);
        }

        public ColaboradoresCredenciaisView BuscarCredencialPelaChave(int colaboradorCredencialId)
        {
            return _repositorio.BuscarCredencialPelaChave(colaboradorCredencialId);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ColaboradorCredencial BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(ColaboradorCredencial entity)
        {
            _repositorio.Criar(entity);
        }


        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<ColaboradorCredencial> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        /// Listar Colaboradores e suas credenciais
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public ICollection<ColaboradoresCredenciaisView> ListarView(params object[] o)
        {
            return _repositorio.ListarView(o);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(ColaboradorCredencial entity)
        {
            _repositorio.Remover(entity);
        }

        #endregion


    }
}
