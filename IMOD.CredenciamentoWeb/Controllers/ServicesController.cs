 
#region

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using AutoMapper;

#endregion

namespace IMOD.CredenciamentoWeb.Controllers
{
    //[System.Web.Mvc.RoutePrefix("credenciamento/api/v1")]
    public class ServicesController : ApiController
    {
        #region Propriedades

        private Version VersaoServico => new Version(1, 0);

        #endregion
        
        [HttpGet]
        [Route("credenciamento/api/v1/verificar/{id}")]
        public HttpResponseMessage Verificar(string  id)
        {
            try
            {
                //if (!dados.DadosGrafico.Any())
                //    throw new FalhaValidacaoException("Sem dados gráficos");

                ////Validar licença so software
                //var licModel = new LicencaModel();
                //var licOuterXml = licModel.BuscarPelaChave(dados.IdLicenca);
                ////Validar Licença
                //var chavePublica = ConfiguracaoModel.ChavePublica;
                //var diasToleracia = ConfiguracaoModel.DiasParaExpirarValidade;
                //var diasPrxValidacao = ConfiguracaoModel.DiasParaProximaValidacao;
                //var xDoc = new XmlDocument();
                //xDoc.LoadXml(licOuterXml);
                //var lic = licModel.Validar(xDoc, chavePublica, diasToleracia, diasPrxValidacao, new LicencaSoftware());
                //if (!lic.EValidoDataExpira) throw new Exception("Licença de software vencida.");
                ////Cadastrar dados
                //var grafModel = new GraficoModel();

                //grafModel.Criar(dados.DadosGrafico, dados.IdLicenca, dados.TipoGrafico);

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                        $"Não foi possível inserir dados. {ex.Message}");
            }
        }
        //[Route("verificar")]
        //[HttpGet]
        //public HttpResponseMessage PostVerificarCredencial()
        //{
        //    try
        //    {
        //        //if (!dados.DadosGrafico.Any())
        //        //    throw new FalhaValidacaoException("Sem dados gráficos");

        //        ////Validar licença so software
        //        //var licModel = new LicencaModel();
        //        //var licOuterXml = licModel.BuscarPelaChave(dados.IdLicenca);
        //        ////Validar Licença
        //        //var chavePublica = ConfiguracaoModel.ChavePublica;
        //        //var diasToleracia = ConfiguracaoModel.DiasParaExpirarValidade;
        //        //var diasPrxValidacao = ConfiguracaoModel.DiasParaProximaValidacao;
        //        //var xDoc = new XmlDocument();
        //        //xDoc.LoadXml(licOuterXml);
        //        //var lic = licModel.Validar(xDoc, chavePublica, diasToleracia, diasPrxValidacao, new LicencaSoftware());
        //        //if (!lic.EValidoDataExpira) throw new Exception("Licença de software vencida.");
        //        ////Cadastrar dados
        //        //var grafModel = new GraficoModel();

        //        //grafModel.Criar(dados.DadosGrafico, dados.IdLicenca, dados.TipoGrafico);

        //        return Request.CreateResponse(HttpStatusCode.Created);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest,
        //                $"Não foi possível inserir dados. {ex.Message}");
        //    }
        //}
        ////[Authorize(Users = "User,Adm")]
        //[Route("CarregarDadosGraficos")]
        //[HttpPost]
        //public HttpResponseMessage PostCarregarDadosGraficos([FromBody] DadosGraficos dados)
        //{
        //    try
        //    {
        //        if (!dados.DadosGrafico.Any())
        //            throw new FalhaValidacaoException("Sem dados gráficos");

        //        //Validar licença so software
        //        var licModel = new LicencaModel();
        //        var licOuterXml = licModel.BuscarPelaChave(dados.IdLicenca);
        //        //Validar Licença
        //        var chavePublica = ConfiguracaoModel.ChavePublica;
        //        var diasToleracia = ConfiguracaoModel.DiasParaExpirarValidade;
        //        var diasPrxValidacao = ConfiguracaoModel.DiasParaProximaValidacao;
        //        var xDoc = new XmlDocument();
        //        xDoc.LoadXml(licOuterXml);
        //        var lic = licModel.Validar(xDoc, chavePublica, diasToleracia, diasPrxValidacao, new LicencaSoftware());
        //        if (!lic.EValidoDataExpira) throw new Exception("Licença de software vencida.");
        //        //Cadastrar dados
        //        var grafModel = new GraficoModel();

        //        grafModel.Criar(dados.DadosGrafico, dados.IdLicenca, dados.TipoGrafico);

        //        return Request.CreateResponse(HttpStatusCode.Created);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest,
        //                $"Não foi possível inserir dados. {ex.Message}");
        //    }
        //}

        

        ///// <summary>
        /////     Adicionar licença de software
        ///// </summary>
        ///// <param name="error"></param>
        ///// <returns></returns>
        ////[Authorize(Users = "User,Adm")]
        //[Route("postErroException")]
        //[HttpPost]
        //public HttpResponseMessage PostErroException([FromBody] ErrorTrace error)
        //{
        //    try
        //    {
        //        Utils.TraceException(error);
        //        return Request.CreateResponse(HttpStatusCode.Created);
        //    }
        //    catch (Exception)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest,
        //                "Não foi possivel coletar dados de erro.");
        //    }
        //}

        ///// <summary>
        /////     Adicionar licença de software
        ///// </summary>
        ///// <param name="licencaViewModel">Dados da licença</param>
        ///// <returns></returns>
        ////[Authorize(Users = "Adm")]
        //[Route("postAdicionarLicenca")]
        //[HttpPost]
        //public HttpResponseMessage PostAdicionarLicenca([FromBody] DadosLicencaViewModel licencaViewModel)
        //{
        //    try
        //    {
        //        var chavePrivada = ConfiguracaoModel.ChavePrivada;
        //        var licenca = new LicencaModel();
        //        var xmlLicenca = Mapper.Map<DadosLicenca>(licencaViewModel);

        //        var result = licenca.Adicionar(licencaViewModel.IdUser, xmlLicenca, chavePrivada,
        //                new LicencaSoftware());
        //        Request.Version = VersaoServico;
        //        return Request.CreateResponse(HttpStatusCode.Created, result);
        //    }
        //    catch (Exception)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest,
        //                "Não foi possivel adicionar licença do software.");
        //    }
        //}

        //[HttpPost]
        //[Route("postRenovarLicenca")]
        //public HttpResponseMessage PostRenovarLicenca([FromBody] RenovarLicenca renovarLicenca)
        //{
        //    try
        //    {
        //        var chavePrivada = ConfiguracaoModel.ChavePrivada;
        //        var licenca = new LicencaModel();
        //        var result = licenca.Renovar(renovarLicenca.IdLicenca, chavePrivada, renovarLicenca.DataTerminoRenovacao,
        //                new LicencaSoftware());
        //        Request.Version = VersaoServico;
        //        return Request.CreateResponse(HttpStatusCode.Created, result);
        //    }
        //    catch (Exception)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest,
        //                "Não foi possivel renovar licença do software.");
        //    }
        //}

        ///// <summary>
        /////     Vlidar arquivo XML contendo dados da licença de software
        ///// </summary>
        ///// <param name="xDoc">Documento Xml</param>
        ///// <returns></returns>
        ////[Authorize(Users = "User,Adm")]
        //[Route("postValidarLicenca")]
        //[HttpPost]
        //public HttpResponseMessage PostValidarLicenca([FromBody] XmlDocument xDoc)
        //{
        //    try
        //    {
        //        var result = ValidarLicenca(xDoc);
        //        Request.Version = VersaoServico;
        //        return Request.CreateResponse(HttpStatusCode.OK, result, new JsonMediaTypeFormatter
        //        {
        //            Indent = true,
        //            SupportedEncodings = {Encoding.UTF8}
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
        //    }
        //}

        ///// <summary>
        /////     Validar licença
        ///// </summary>
        ///// <param name="xDoc"></param>
        ///// <returns></returns>
        //private LicencaResposta ValidarLicenca(XmlDocument xDoc)
        //{
        //    var chavePublica = ConfiguracaoModel.ChavePublica;
        //    var diasToleracia = ConfiguracaoModel.DiasParaExpirarValidade;
        //    var diasPrxValidacao = ConfiguracaoModel.DiasParaProximaValidacao;
        //    //Quantidade em dias para a proxima validação do software
        //    var licenca = new LicencaModel();

        //    return licenca.Validar(xDoc, chavePublica, diasToleracia, diasPrxValidacao, new LicencaSoftware());
        //}

        ///// <summary>
        /////     Obter licença de software
        ///// </summary>
        ///// <param name="idLicenca">Dados da licença</param>
        ///// <returns></returns>
        ////[Authorize(Users = "User,Adm")]
        //[Route("getObterLicenca/{idLicenca}")]
        //[HttpGet]
        //public HttpResponseMessage GetObterLicenca(int idLicenca)
        //{
        //    try
        //    {
        //        var licenca = new LicencaModel();

        //        var dados = licenca.BuscarPelaChave(idLicenca);
        //        Request.Version = VersaoServico;
        //        return Request.CreateResponse(HttpStatusCode.OK, dados, new XmlMediaTypeFormatter
        //        {
        //            Indent = true,
        //            UseXmlSerializer = true,
        //            SupportedEncodings = {Encoding.UTF8}
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest,
        //                "Não foi possivel obter a licença do software.");
        //    }
        //}
    }
}

//[XmlRoot(ElementName = "Aluno")]
//public class Aluno
//{
//    #region Propriedades

//    public string Nome { get; set; }
//    public int Idade { get; set; }

//    #endregion
//}
//Retornar em formato XML
//[HttpGet]
//[Route("Recuperar/{id}")]
//public HttpResponseMessage Recuperar(int id)
//{
//    // //var artista = this.repositorio.Buscar(id);
//    //if (artista == null)
//    var dados = Request.CreateErrorResponse(
//            HttpStatusCode.BadRequest,
//            new HttpError("Artista não encontrado"));

//    return dados;
//    //else
//    // return Request.CreateResponse(HttpStatusCode.OK, artista);
//}
/*
 using (var client = new HttpClient())
{
using (var request = new HttpRequestMessage(HttpMethod.Get, Endereco))
{
request.Headers.Add("Accept", "application/xml");
using (var response = await client.SendAsync(request))
Console.WriteLine(response.Content.ReadAsStringAsync().Result);
}
}
 */
//[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
//public class ApiDocumentationAttribute : Attribute
//{
//    public ApiDocumentationAttribute(string message)
//    {
//        this.Message = message;
//    }
//    public string Message { get; private set; }
//}
//[ApiDocumentation("Retorna todos os clientes.")]
//public IEnumerable<Cliente> Get()
//{
//    //...
//}

//*****************************************

//[HttpGet]
//public HttpResponseMessage Recuperar(int id)
//{
//    var artista = this.repositorio.Buscar(id);
//    if (artista == null)
//        return Request.CreateErrorResponse(
//        HttpStatusCode.NotFound,
//        new HttpError("Artista não encontrado"));
//    else
//        return Request.CreateResponse(HttpStatusCode.OK, artista);
//}

//Global.asx
//GlobalConfiguration
// .Configuration
// .Filters
// .Add(new ExceptionTranslatorAttribute());

//public class ExceptionTranslatorAttribute : ExceptionFilterAttribute
//{
//    public override void OnException(HttpActionExecutedContext ctx)
//    {
//        var errorDetails = new ErrorDetails();
//        var statusCode = HttpStatusCode.InternalServerError;
//        if (ctx.Exception is HttpException)
//        {
//            var httpEx = (HttpException)ctx.Exception;
//            errorDetails.Message = httpEx.Message;
//            statusCode = (HttpStatusCode)httpEx.GetHttpCode();
//        }
//        else
//        {
//            errorDetails.Message = "** Internal Server Error **";
//        }
//        ctx.Result =
//        new HttpResponseMessage<ErrorDetails>(errorDetails, statusCode);
//    }
//}