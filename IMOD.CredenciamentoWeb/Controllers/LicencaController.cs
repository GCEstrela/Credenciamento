//  *********************************************************************************************************
// Empresa: DSBR - Empresa de Desenvolvimento de Sistemas
// Sistema: Automação Comercial
// Projeto: DSBR.ServicesWeb
// Autores: 
// Valnei Filho e-mail: vbatistas@devsysbrasil.com.br;
// Vagner Marcelo e-mail: vmarcelo@devsysbrasil.com.br
// Data Criação:01/09/2018
// Todos os direitos reservados
//  *********************************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using DSBR.ServicesWeb.Helpers;
using DSBR.ServicesWeb.Models;

#endregion

namespace IMOD.CredenciamentoWeb.Controllers
{
    [HandleError]
    public class LicencaController : Controller
    {
        /// <summary>
        ///     Action não encontrada
        /// </summary>
        /// <param name="actionName"></param>
        protected override void HandleUnknownAction(string actionName)
        {
            try
            {
                View(actionName).ExecuteResult(ControllerContext);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = HtmlHelpers.MensagenErroFormat("Não foi possível identificar a ação solicitada.", true);
                View("Error").ExecuteResult(ControllerContext);
            }
        }
         

        public PartialViewResult Visualizar(string idLic)
        {
            try
            {
                var idLicenca = int.Parse(Utils.DecryptAes(idLic));
                var licModel = new LicencaModel();
                var content = licModel.BuscarPelaChave(idLicenca);
                var model = licModel.ConvertXmlString(content);
                var model1 = Mapper.Map<DadosLicencaViewModel>(model);
                return PartialView("Visualizar", model1);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao obter licença de software");
            }
            
        }


        [HttpGet]
        public ActionResult Comprar(string idLic)
        {
           
            var list = HtmlHelpers.SelectListFor<ClassePgto>();
            
            var model = new PagamentoViewModel {DataInicioParcela = DateTime.Now.AddDays(1),
                NumeroParcela = 1,IdLicencaCript = idLic,IntervaloParcela = 30,
            QtdDiasPrimeriParcela = 30};//adicionar um ano a partir da data corrente
            ViewBag.ListFormPgto = new SelectList(list, "Value", "Text");
            return View("Comprar",model);
        }

        [HandleError]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Comprar(PagamentoViewModel comprarViewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(comprarViewModel.FormaPagamento))
                    throw new Exception("Informe uma forma de pagamento");
                if (string.IsNullOrWhiteSpace(comprarViewModel.IdLicencaCript))
                    throw new Exception("Uma licença deve estar associada ao pagamento");

                var list2 = PagamentoModel.CalcularParcelas(comprarViewModel).ToList();
                comprarViewModel.IdLicenca = int.Parse(Utils.DecryptAes(comprarViewModel.IdLicencaCript));//Descriptografaar identificador licenca
                var pagModel = new PagamentoModel();
                var usuarioModel = new UsuarioModel();
                var user = usuarioModel.ObterUsuario(User);
                pagModel.Criar(user.Nome, comprarViewModel, list2);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
                 
        }

        [HandleError]
        public ActionResult MinhaLicenca()
        {
            try
            {
                var usuarioModel = new UsuarioModel();
                var model = usuarioModel.ObterUsuario(User);


                return View("MinhasLicencas", model.Licencas);
            }
            catch (Exception ex)
            {
                throw new  Exception(ex.Message);
            }
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public void Criar(DadosLicencaViewModel dadosLicenca)
        {
            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Informe todos os campos requerido *");

                var chavePrivada = ConfiguracaoModel.ChavePrivada;
                var licenca = new LicencaModel();
                var xmlLicenca = Mapper.Map<DadosLicenca>(dadosLicenca);

                licenca.Adicionar(dadosLicenca.IdUser, xmlLicenca, chavePrivada, new LicencaSoftware());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Download de arquivo de licença
        /// </summary>
        /// <param name="idLic">Id licença</param>
        /// <returns></returns>
        [HandleError]
        public FileResult Download(string idLic)
        {
            try
            {
                //throw new Exception("Esta licença não existe");
                //Exceto usuario master, o usuario deve acessar apenas as suas licenças
                var idLicenca = int.Parse(Utils.DecryptAes(idLic));
                var user = (ClaimsIdentity)User.Identity;
                var key = user.FindFirst("id");
                var usuarioModel = new UsuarioModel();
                var id = int.Parse(key.Value);
                //Somente o usuario master pode baixar todas as licenças
                var usuarioLogado = usuarioModel.Listar().FirstOrDefault(n => n.IdUser == id);

                if (usuarioLogado == null) throw new Exception("Usuário inválido");
                if (usuarioLogado.Perfil != "Adm")
                {
                    var exists = usuarioLogado.Licenses.Any(n => n.IdLicenca == idLicenca);
                    if (!exists) throw new Exception("Esta licença não existe");
                }

                var licenca = new LicencaModel();
                var dados = licenca.BuscarPelaChave(idLicenca);
                var byteArray = Encoding.ASCII.GetBytes(dados);
                var stream = new MemoryStream(byteArray);

                return File(stream, "text/xml", "Licenca.lic");
            }
            catch (Exception)
            {
                    
                throw new Exception("Erro ao obter licença de software");
            }
            
             
            
        }

        /// <summary>
        /// Listar faturas
        /// </summary>
        /// <param name="idLic">Id licença</param>
        /// <returns></returns>
        public ActionResult ListarFaturas(string idLic)
        {
            try
            {
                var idLicenca = idLic;
                //int.Parse(Utils.DecryptAes(idLic));
                //var fat = new PagamentoModel();
                //var model = fat.ListarFatura().Where(n => n.IdLicenca == idLicenca);
                //return View("Listar", model);
                return RedirectToAction("ListarFatura", "Pagamento",new {idLic= idLicenca });

            }
            catch (Exception)
            {

                throw new Exception("Erro ao obter licença de software");
            }
        }

        /// <summary>
        ///     Adicionar licença de software
        /// </summary>
        /// <returns></returns>
        //[Authorize(Users = "Adm")] 
        [HttpGet]
        public ActionResult Criar()
        {
            
                //Obter a lista
                ListarUsuarios();
                return View("Novo");
            
        }

        private void ListarUsuarios()
        {
            var userModel = new UsuarioModel();
            var list = userModel.ListarAsNoTracking().Where(n => n.Perfil != "Adm").ToList();
            ViewBag.Usuarios = new SelectList(list, "IdUser", "Nome");
        }
        
        public ActionResult Listar()
        {
            var lic = new LicencaModel();
            var model = lic.ListarAsNoTracking().ToList();

            var listModel = new List<DadosLicenca>();
            model.ForEach(n =>
            {
                var d1 = lic.ConvertXmlString(n.Content);
                listModel.Add(d1);
            });

            return View("Listar", listModel);
        }

        /// <summary>
        /// Aletrar data da licença
        /// </summary>
        /// <param name="idLic"></param>
        /// <returns></returns>
        [HandleError]
        public PartialViewResult AlterarDataValidadeLicenca(string idLic)
        {
            try
            {
                var idLicenca = int.Parse(Utils.DecryptAes(idLic));
                var licModel = new LicencaModel();
                var content = licModel.BuscarPelaChave(idLicenca);
                var model = licModel.ConvertXmlString(content);
                var model1 = Mapper.Map<DadosLicencaViewModel>(model);
                return PartialView("AlterarValidadeLicenca", model1);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao obter licença de software");
            }
            
        }

        [HttpPost]
        [HandleError]
        public void Renovar(DadosLicencaViewModel licenca)
        {
            try
            {
                var chavePrivada = ConfiguracaoModel.ChavePrivada;
                var licModel = new LicencaModel();
                if(licenca.DataRenovacao==null) throw new Exception("A data da renovação é requerida.");
                licModel.Renovar(licenca.IdLicenca, chavePrivada, (DateTime)licenca.DataRenovacao,new LicencaSoftware());
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





    }
}