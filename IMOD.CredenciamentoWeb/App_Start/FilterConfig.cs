// ***********************************************************************
// Project: IMOD.CredenciamentoWeb
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 10 - 2019
// ***********************************************************************

#region

using System.Web.Mvc;

#endregion

namespace IMOD.CredenciamentoWeb
{
    public class FilterConfig
    {
        #region  Metodos

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add (new HandleErrorAttribute());
        }

        #endregion
    }
}