using System.Web.Mvc;

namespace IMOD.CredenciamentoWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Index()
        {
            return View();
        }


        //public ActionResult Grid()
        //{
        //    return View("Grid");
        //}

        //public ActionResult Tabela()
        //{
        //    return View("Tables");
        //}



    }
}