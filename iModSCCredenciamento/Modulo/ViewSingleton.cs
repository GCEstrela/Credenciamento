// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
// ***********************************************************************

#region

using iModSCCredenciamento.Views;

#endregion

namespace iModSCCredenciamento.Modulo
{
    /// <summary>
    ///     Retorna objetos singleton
    /// </summary>
    public class ViewSingleton
    {
        private static EmpresaView _empresaView;
        private static ColaboradorView _colaboradorView;

        static ViewSingleton()
        {
            _empresaView = new EmpresaView();
            _colaboradorView= new ColaboradorView();
        }

        /// <summary>
        /// Obtem uma instância de objeto
        /// </summary>
        public  EmpresaView ObterInstanciaEmpresa { get { return _empresaView; } }
        /// <summary>
        /// Obtem uma instância de objeto
        /// </summary>
        public ColaboradorView ObterInstanciaColaborador { get { return _colaboradorView; } }


    }
}