// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region
 
using Genetec.Sdk.Workspace.Pages; 

#endregion

namespace iModSCCredenciamento.Modulo
{
    [Page(typeof(ModuloDescritor))]
    public class ModuloPage : Page
    {
        
        #region  Metodos

        private MenuPrincipalView _view;
        public ModuloPage()
        {
            _view= new MenuPrincipalView();
            this.View = _view;

        }
        /// <summary>
        /// Deserializes the data contained by the specified byte array.
        /// </summary>
        /// <param name="data">A byte array that contains the data.</param>
        protected override void Deserialize(byte[] data)
        {
        }

        /// <summary>
        /// Initialize the page.
        /// </summary>
        /// <remarks>At this step, the <see cref="Genetec.Sdk.Workspace.Workspace"/> is available.</remarks>
        protected override void Initialize()
        { 
            _view.Initialize(Workspace);
            
        }

        /// <summary>
        /// Serializes the data to a byte array.
        /// </summary>
        /// <returns>A byte array that contains the data.</returns>
        protected override byte[] Serialize()
        {
            return null;
        }


        #endregion
    }
}