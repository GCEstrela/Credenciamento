// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 21 - 2018
// ***********************************************************************

#region

using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;
using System.Collections.Generic;

#endregion

namespace IMOD.Application.Service
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly IColaboradorRepositorio _repositorio = new ColaboradorRepositorio();
        private readonly IColaboradorCredencialRepositorio _repositorioCredencial = new ColaboradorCredencialRepositorio();

        /// <summary>
        /// Credencial X Colaborador Service
        /// </summary>
        public IColaboradorCredencialService Credencial => new ColaboradorCredencialService();

        /// <summary>
        /// Empresa X Colaborador Service
        /// </summary>
        public IColaboradorEmpresaService Empresa { get { return new ColaboradorEmpresaService(); } }

        /// <summary>
        ///Curso X Colaborador Service
        /// </summary>
        public IColaboradorCursoService Curso { get { return new ColaboradorCursosService(); } }

        /// <summary>
        /// Anexo X Colaborador Serice
        /// </summary>
        public IColaboradorAnexoService Anexo { get { return new ColaboradorAnexoService(); } }

        #region  Metodos

        /// <summary>
        ///     Listar por credencial
        /// </summary>
        /// <param name="colaboradorCredencialId"></param>
        /// <returns></returns>
        public ICollection<Colaborador> ListarColaboradorPorCredencial(int colaboradorCredencialId)
        {
            return _repositorio.Listar(colaboradorCredencialId, "", "");
        }

        /// <summary>
        ///     Obter colaborador por CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public Colaborador ObterPorCpf(string cpf)
        {
            return _repositorio.ObterPorCpf(cpf);
        }

        /// <summary>
        ///     Listar por nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public ICollection<Colaborador> ListarColaboradorPorNome(string nome)
        {
            return _repositorio.Listar(0, "", "%" + nome + "%");
        }

        /// <summary>
        /// Verificar se existe colaborador
        /// </summary>
        /// <param name="idcolaborador"></param>
        /// <returns></returns>
        public bool ExisteColaborador(int idcolaborador)
        {
            var d1 = BuscarPelaChave(idcolaborador);
            return d1 != null;
        }



        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(Colaborador entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public Colaborador BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<Colaborador> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(Colaborador entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(Colaborador entity)
        {
            _repositorio.Remover(entity);
        }

        /// <summary>
        ///     Criar colaboradore e anexo
        /// </summary>
        /// <param name="colaborador">Colaborador</param>
        /// <param name="anexos">Anexos</param>
        public void CriarAnexos(Colaborador colaborador, IList<ColaboradorAnexo> anexos)
        {
            _repositorio.CriarAnexos(colaborador, anexos);
        }

        /// <summary>
        ///     Obter uma lista de colaboradores e suas credenciais
        /// </summary>
        /// <param name="statusCredencialId"></param>
        /// <param name="colaboradorId">Identificador do colaborador</param>
        /// <param name="colaboradorCredencialId">Identiicador credencial</param>
        /// <param name="nomeEmpresa">Nome da empresa</param>
        /// <param name="tipoCredencialId">Identificador da credencial</param>
        /// <returns></returns>
        public ICollection<ColaboradoresCredenciaisView> ListarColaboradores(int colaboradorCredencialId, string nomeEmpresa = "", int tipoCredencialId = 0, int statusCredencialId = 0, int colaboradorId = 0)
        {
            return _repositorioCredencial.ListarView(colaboradorCredencialId, nomeEmpresa, tipoCredencialId, statusCredencialId, colaboradorId);
        }



        /// <summary>
        /// Listar Colaborador por status
        /// </summary>
        /// <param name="idStatus"></param>
        /// <returns></returns>
        public ICollection<Colaborador> ListarPorStatus(int idStatus)
        {
            return _repositorio.ListarPorStatus(idStatus);
        }

        #endregion
    }
}