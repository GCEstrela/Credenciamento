// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.Views;
using IMOD.CrossCutting;
using System;

#endregion

namespace IMOD.CredenciamentoDeskTop.Modulo
{
    /// <summary>
    ///     Retorna objetos singleton
    /// </summary>
    public class ViewSingleton
    {
        private static EmpresaView _empresaView;
        private static ColaboradorView _colaboradorView;
        private static VeiculoView _veiculoView;
        private static ConfiguracoesView _configuracoesView;
        private static RelatoriosView _relatoriosView;
        private static TermosView _termosView;
        private static EquipamentosView _equipamentosView;

        #region  Propriedades

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public EmpresaView EmpresaView
        {

            get { return _empresaView ?? (_empresaView = new EmpresaView()); }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public ColaboradorView ColaboradorView
        {
            get
            {
                try
                {
                    return _colaboradorView ?? (_colaboradorView = new ColaboradorView());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public VeiculoView VeiculoView
        {
            get
            {
                try
                {
                    return _veiculoView ?? (_veiculoView = new VeiculoView());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public ConfiguracoesView ConfiguracoesView
        {
            get
            {
                try
                {
                    return _configuracoesView ?? (_configuracoesView = new ConfiguracoesView());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public RelatoriosView RelatoriosView
        {
            get
            {
                try
                {
                    return _relatoriosView ?? (_relatoriosView = new RelatoriosView());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public TermosView TermosView
        {
            get
            {
                try
                {
                    return _termosView ?? (_termosView = new TermosView());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public EquipamentosView EquipamentosView
        {
            get
            {
                try
                {
                    return _equipamentosView ?? (_equipamentosView = new EquipamentosView());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion
    }
}