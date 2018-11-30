using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Application.Service
{
    public class ColaboradorEmpresaService : IColaboradorEmpresaService
    {

        private readonly IColaboradorEmpresaRepositorio _repositorio = new ColaboradorEmpresaRepositorio();
        public void Alterar(ColaboradorEmpresa entity)
        {
            _repositorio.Alterar(entity);
        }

        public ColaboradorEmpresa BuscarPelaChave(int id)
        {
            throw new NotImplementedException();
        }

        public void Criar(ColaboradorEmpresa entity)
        {
            _repositorio.Criar(entity);
        }

        public ICollection<ColaboradorEmpresa> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        public void Remover(ColaboradorEmpresa entity)
        {
            _repositorio.Remover(entity);
        }
    }
}
