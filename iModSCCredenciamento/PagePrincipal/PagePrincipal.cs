﻿using Genetec.Sdk.Workspace.Pages;
using iModSCCredenciamento.Funcoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using iModSCCredenciamento.Modulo;

namespace iModSCCredenciamento.PagePrincipal
{

    [Page(typeof(ModuloDescritor))]
    public class PagePrincipal : Page
    {
        #region Constants

        private readonly PagePrincipalView m_view = new PagePrincipalView();
        //private readonly MenuPrincipalView m_view = new MenuPrincipalView();

        #endregion

        #region Constructors

        public PagePrincipal()
        {
            View = m_view;
        }

        #endregion

        #region Protected Methods

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
            m_view.Initialize(Workspace);
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
