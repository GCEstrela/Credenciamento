using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMOD.CredenciamentoWeb.Controllers
{
    public class CheckController : Controller
    {
        // GET: CheckCredential
        public ActionResult Credencial(string guid)
        {
            return View();
        }
    }
}