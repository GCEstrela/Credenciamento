using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using DSBR.ServicesWeb.Models;

namespace IMOD.CredenciamentoWeb.Controllers
{
    public class PagamentoController : Controller
    {

        //public ActionResult ListarPagamentos(int t)
        //{
        //    return View();
        //}

        /// <summary>
        /// Listar minha fatura
        /// </summary>
        /// <returns></returns>
        [HandleError]
        public ActionResult MinhaFatura()
        {
            try
            {
                var usuarioModel = new UsuarioModel();
                var usuario = usuarioModel.ObterUsuario(User);
                var faturas = new PagamentoModel();
                var model = faturas.ListarFatura().Where(n => n.IdUser == usuario.IdUser).ToList();
                var model1 = Mapper.Map<IEnumerable<FaturasViewModel>>(model);
                return View("MinhasFaturas", model1);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        /// <summary>
        /// Listar pagamentos
        /// </summary>
        /// <param name="idLic">Id licença</param>
        /// <returns></returns>
        [HandleError]
        public ActionResult ListarFatura(string idLic)
        {
            try
            {
                var idLicenca = int.Parse(Utils.DecryptAes(idLic));

                var fat = new PagamentoModel();
                var model = fat.ListarFatura().Where(n => n.IdLicenca == idLicenca);
                var model1 = Mapper.Map<IEnumerable<FaturasViewModel>>(model);
                return View("ListarFaturas", model1); 

            }
            catch (Exception)
            {

                throw new Exception("Erro ao obter licença de software");
            }
        }

        [HandleError]
        public ActionResult CalcularParcelasAjax(PagamentoViewModel comprarViewModel)
        {

           try
           { 
                var list2 = PagamentoModel.CalcularParcelas(comprarViewModel);
                return View("ListarCalculos", list2);
           }
             catch (Exception ex)
             { 
                throw new Exception(ex.Message);
                 
            }
            
        }
         
    }
}