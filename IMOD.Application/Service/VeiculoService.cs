// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
// ***********************************************************************

#region

using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;
using System.Collections.Generic;

#endregion

namespace IMOD.Application.Service
{
    public class VeiculoService : IVeiculoService
    {
        #region Variaveis Globais

        private readonly IVeiculoRepositorio _repositorio = new VeiculoRepositorio();
        private readonly IVeiculoCredencialRepositorio _repositorioCredencial = new VeiculoCredencialRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(Veiculo entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Veiculo BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<Veiculo> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(Veiculo entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(Veiculo entity)
        {
            _repositorio.Remover(entity);
        }

        #endregion

        /// <summary>
        /// Credencial X Veiculo Service
        /// </summary>
        public IVeiculoCredencialService Credencial => new VeiculoCredencialService();

        /// <summary>
        /// Empresa X Veiculo Service
        /// </summary>
        public IVeiculoEmpresaService Empresa { get { return new VeiculoEmpresaService(); } }

        /// <summary>
        /// Seguros X Veiculo Service
        /// </summary>
        public IVeiculoSeguroService Seguro { get { return new VeiculoSeguroService(); } }

        /// <summary>
        /// Anexo X Veiculo Serice
        /// </summary>
        public IVeiculoAnexoService Anexo { get { return new VeiculoAnexoService(); } }

        /// <summary>
        /// Veiculo X Empresa Service
        /// </summary>
        public IVeiculoEmpresaService Veiculo { get { return new VeiculoEmpresaService(); } }
    }
}