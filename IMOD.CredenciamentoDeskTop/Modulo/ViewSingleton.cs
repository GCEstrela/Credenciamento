﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 12 - 2018
// ***********************************************************************

#region

using IMOD.CredenciamentoDeskTop.Views;

#endregion

namespace IMOD.CredenciamentoDeskTop.Modulo
{
    /// <summary>
    ///     Retorna objetos singleton
    /// </summary>
    public class ViewSingleton
    {
        private static  EmpresaView _empresaView;
        private static  ColaboradorView _colaboradorView;
        private static  VeiculoView _veiculoView;
        private static  ConfiguracoesView _configuracoesView;
        private static  RelatoriosView _relatoriosView;
        private static  TermosView _termosView;

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
            get { return _colaboradorView ?? (_colaboradorView = new ColaboradorView()); }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public VeiculoView VeiculoView
        {
            get { return _veiculoView ?? (_veiculoView = new VeiculoView()); }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public ConfiguracoesView ConfiguracoesView
        {
            get { return _configuracoesView ?? (_configuracoesView = new ConfiguracoesView()); }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public RelatoriosView RelatoriosView
        {
            get { return _relatoriosView ?? (_relatoriosView = new RelatoriosView()); }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public TermosView TermosView
        {
            get { return _termosView ?? (_termosView = new TermosView()); }
        }

        #endregion
        
    }
}