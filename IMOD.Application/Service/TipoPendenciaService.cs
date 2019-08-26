using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

namespace IMOD.Application.Service
{
    public class TipoPendenciaService : ITipoPendenciaService
    {
        private readonly ITipoPendenciaRepositorio _repositorio = new TipoPendenciaRepositorio();

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<TipoPendencia> Listar()
        {
            try
            {
                return _repositorio.Listar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Obter Tipo Pendencia por codigo
        /// </summary>
        /// <param name="codPendencia"></param>
        /// <returns></returns>
        public TipoPendencia BuscarPorCodigo(string codPendencia)
        {
            try
            {
                return _repositorio.BuscarPorCodigo(codPendencia);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
