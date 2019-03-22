// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  08 - 03 - 2019
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
    public class TipoRepresentanteService : ITipoRepresentanteService
    {
        #region Variaveis Globais

        private readonly ITipoRepresentanteRepositorio _repositorio = new TipoRepresentanteRepositorio();

        #endregion
        public void Alterar(TipoRepresentante entity)
        {
            _repositorio.Alterar(entity);
        }

        public TipoRepresentante BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        public void Criar(TipoRepresentante entity)
        {
            _repositorio.Criar(entity);
        }

        public ICollection<TipoRepresentante> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        public void Remover(TipoRepresentante entity)
        {
            _repositorio.Remover(entity);
        }
    }
}
