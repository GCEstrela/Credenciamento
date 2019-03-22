// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 03 - 2019
// ***********************************************************************

#region

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
            _repositorio.Alterar(entity);
        }

        public ConfiguraSistema BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        public void Criar(ConfiguraSistema entity)
        {
            _repositorio.Criar(entity);
        }

        public ICollection<ConfiguraSistema> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        public void Remover(ConfiguraSistema entity)
        {
            _repositorio.Remover(entity);
        }
    }
}
