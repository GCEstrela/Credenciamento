// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 03 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion


namespace IMOD.Application.Service
{
    public class ConfiguraSistemaService : IConfiguraSistemaService
    {
        #region Variaveis Globais

        private readonly IConfiguraSistemaRepositorio _repositorio = new ConfiguraSistemaRepositorio();

        #endregion
        public void Alterar(ConfiguraSistema entity)
        {
            try
            {
                _repositorio.Alterar(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ConfiguraSistema BuscarPelaChave(int id)
        {
            try
            {
                return _repositorio.BuscarPelaChave(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Criar(ConfiguraSistema entity)
        {
            try
            {
                _repositorio.Criar(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ICollection<ConfiguraSistema> Listar(params object[] objects)
        {
            try
            {
                return _repositorio.Listar(objects);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Remover(ConfiguraSistema entity)
        {
            try
            {
                _repositorio.Remover(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
