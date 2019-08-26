// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 09 - 2019
// ***********************************************************************

#region

using System;
using System.Globalization;
using System.Reflection;
using IMOD.Application.Interfaces;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Ado;
using IMOD.Infra.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class ConfiguracaoService : IConfiguracaoService
    {
        private readonly IConfiguracaoRepositorio _repositorio;
        private readonly IInfoDataBase _infoData;

        public ConfiguracaoService()
        {
            _repositorio = new ConfiguracaoRepositorio();
            _infoData = new ConfiguracaoRepositorio();
        }

        #region  Metodos

        public static string ObterVersaoSoftware(Assembly assembly)
        {
            try
            {
                var v = assembly.GetName().Version;
                return string.Format(CultureInfo.InvariantCulture, @"Versão {0}.{1}.{2}", v.Major, v.Minor,
                    v.Revision);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Obter informações do banco de dados
        /// </summary>
        public DataBaseInfo ObterInformacaoBancoDeDados { get { return _infoData.ObterInformacaoBancoDeDados; } }
    }
}