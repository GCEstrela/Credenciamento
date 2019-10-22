using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.Domain.Entities;
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
        IConfiguraSistemaService configuracaoService = new ConfiguraSistemaService();
        public static EmpresaViewModel EmpresaLogada
        {
            get
            {                
                return (EmpresaViewModel)HttpContext.Current.Session["EmpresaLogada"];
            }
        }

        public static ConfiguraSistema ConfigSistema
        {
            get
            {
                return (ConfiguraSistema)HttpContext.Current.Session["ConfigSistema"];
            }
        }

    }
}