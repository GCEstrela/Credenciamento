// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
// ***********************************************************************

#region

using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace IMOD.Application.Service
{
    public class VeiculoEmpresaWebService : IVeiculoempresaWebService
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }

        #region Variaveis Globais

        private readonly IVeiculoEmpresaWebRepositorio _repositorio = new VeiculoEmpresaWebRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoEmpresaWeb entity)
        {
            _repositorio.Criar(entity);
            #region Retirar pendencias de sistema
            var pendencia = Pendencia.ListarPorVeiculo(entity.VeiculoId)
                .FirstOrDefault(n => n.PendenciaSistema & n.CodPendencia ==  22);
            if (pendencia == null) return;
            Pendencia.Remover(pendencia);
            #endregion
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoEmpresaWeb BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoEmpresaWeb> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoEmpresaWeb entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(VeiculoEmpresaWeb entity)
        {
            _repositorio.Remover(entity);
        }

        public ICollection<VeiculoEmpresaWeb> ListarEmpresas(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        /// Criar numero de matricula
        /// </summary>
        /// <param name="entity"></param>
        public void CriarNumeroMatricula(VeiculoEmpresaWeb entity)
        {
            _repositorio.CriarNumeroMatricula(entity);
        }
        #endregion
    }
}