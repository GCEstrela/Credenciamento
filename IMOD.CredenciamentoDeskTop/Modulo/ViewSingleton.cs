// ***********************************************************************
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
        private static readonly EmpresaView _empresaView;
        private static readonly ColaboradorView _colaboradorView;
        private static readonly VeiculoView _veiculoView;
        private static readonly ConfiguracoesView _configuracoesView;
        private static readonly RelatoriosView _relatoriosView;
        private static readonly TermosView _termosView;

        #region  Propriedades

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public EmpresaView EmpresaView
        {
            get { return _empresaView; }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public ColaboradorView ColaboradorView
        {
            get { return _colaboradorView; }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public VeiculoView VeiculoView
        {
            get { return _veiculoView; }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public ConfiguracoesView ConfiguracoesView
        {
            get { return _configuracoesView; }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public RelatoriosView RelatoriosView
        {
            get { return _relatoriosView; }
        }

        /// <summary>
        ///     Obtem uma instância de objeto
        /// </summary>
        public TermosView TermosView
        {
            get { return _termosView; }
        }

        #endregion

        static ViewSingleton()
        {
            _empresaView = new EmpresaView();
            _colaboradorView = new ColaboradorView();
            _veiculoView = new VeiculoView();
            _configuracoesView = new ConfiguracoesView();
            _relatoriosView = new RelatoriosView();
            _termosView = new TermosView();
        }
    }
}