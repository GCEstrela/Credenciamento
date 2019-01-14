using System.Linq;
using System.Web.Mvc;
using DSBR.ServicesWeb.Models;

namespace DSBR.ServicesWeb.Controllers
{
    public class ChartsController : Controller
    {
        // GET: Charts
        public  JsonResult  ProdutosMaisVendidos()
        {
            //Obter Id do usuario logado
            var id = 1;
            var idLicenca = 108;
            //OBter Grafico
            var tipografico = 1;
            var graficoModel = new GraficoModel();
            var d1 = graficoModel.ListarAsNoTracking()
                    .FirstOrDefault(n => n.IdLicenca == idLicenca & n.TipoGrafico == tipografico);
            if (d1 == null) return null;
            return  Json(d1.Content, JsonRequestBehavior.AllowGet);
        }

        // GET: Charts
        public JsonResult VendasMensal()
        {
            //Obter Id do usuario logado
            var id = 1;
            var idLicenca = 108;
            //OBter Grafico
            var tipografico = 2;
            var graficoModel = new GraficoModel();
            var d1 = graficoModel.ListarAsNoTracking()
                    .FirstOrDefault(n => n.IdLicenca == idLicenca & n.TipoGrafico == tipografico);
            if (d1 == null) return null;
            return Json(d1.Content, JsonRequestBehavior.AllowGet);
        }
    }
}