using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Application.Service
{
    public class ColaboradorCredencialService : IColaboradorCredencialService
    {
        private readonly IColaboradorCredencialRepositorio _repositorio = new ColaboradorCredencialRepositorio();
        public void Alterar(ColaboradorCredencial entity)
        {
            _repositorio.Alterar(entity);
        }

        public ColaboradorCredencial BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        public void Criar(ColaboradorCredencial entity)
        {
            _repositorio.Criar(entity);
        }

        public ICollection<ColaboradorCredencial> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        public ICollection<ColaboradoresCredenciaisView> ListarView(params object[] o)
        {
            return _repositorio.ListarView(o);
        }

        public void Remover(ColaboradorCredencial entity)
        {
            _repositorio.Remover(entity);
        }
    }
}
