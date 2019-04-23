using IMOD.PreCredenciamentoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace IMOD.PreCredenciamentoWeb.Util
{
    public class SessionUsuario
    {                        

        public static EmpresaViewModel EmpresaLogada
        {
            get
            {                
                return (EmpresaViewModel)HttpContext.Current.Session["EmpresaLogada"];
            }
        }

    }
}