// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace IMOD.Application.Service
{
    public class DadosAuxiliaresFacadeService : IDadosAuxiliaresFacade
    {
        private readonly ITiposAcessoService _acessoService = new TipoAcessoService();
        private readonly ITipoCobrancaService _cobrancaService = new TipoCobrancaService();
        private readonly IEstadoService _serviceEstado = new EstadoService();
        private readonly IMunicipioService _serviceMunicipio = new MunicipioService();
        private readonly IStatusService _statusService = new StatusService();
        private readonly ICursoService _cursosService = new CursoService();

        #region  Metodos


        /// <summary>
        ///     Listar Estados
        /// </summary>
        /// <returns></returns>
        public ICollection<Estados> ListarEstadosFederacao()
        {
            return _serviceEstado.Listar();
        }

        /// <summary>
        ///     Listar Municipios
        /// </summary>
        /// <returns></returns>
        public ICollection<Municipio> ListarMunicipios()
        {
            return _serviceMunicipio.Listar();
        }

        /// <summary>
        ///     Buscar municipios por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        public EstadoView BuscarEstadoMunicipiosPorUf(string uf)
        {
            return _serviceEstado.BuscarEstadoMunicipiosPorUf (uf);
        }

        /// <summary>
        ///     Listar Status
        /// </summary>
        /// <returns></returns>
        public ICollection<Status> ListarStatus()
        {
            return _statusService.Listar();
        }

        /// <summary>
        ///     Listar Tipos de Acessos
        /// </summary>
        /// <returns></returns>
        public ICollection<TipoAcesso> ListarTiposAcessos()
        {
            return _acessoService.Listar();
        }
        /// <summary>
        ///     Listar Tipos de Cursos
        /// </summary>
        /// <returns></returns>
        public ICollection<Curso> ListarCursos()
        {
            return _cursosService.Listar();
        }
        /// <summary>
        ///     Listar Tipo de Cobrança
        /// </summary>
        /// <returns></returns>
        public ICollection<TipoCobranca> ListarTiposCobranca()
        {
            return _cobrancaService.Listar();
        }

        public ITipoEquipamentoService EquipamentoService()
        {
            return  new TipoEquipamentoService();
        }

        #endregion
    }
}