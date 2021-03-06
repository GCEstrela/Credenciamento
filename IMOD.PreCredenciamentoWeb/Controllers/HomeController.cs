﻿using IMOD.PreCredenciamentoWeb.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    //[HandleError]
    public class HomeController : Controller
    {
        //[Authorize]
        public ActionResult Index()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            ViewBag.User = SessionUsuario.EmpresaLogada.Nome;
            return View(SessionUsuario.EmpresaLogada);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult LinksUteis()
        {
            ViewBag.Links = "Links Uteis.";
            return View();
        }
    }
}