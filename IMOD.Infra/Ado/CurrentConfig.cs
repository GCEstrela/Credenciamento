// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 20 - 2018
// ***********************************************************************

#region

#endregion

using IMOD.Infra.Properties;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Xml.Linq;
using System.Linq;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Entities;
using System.Web;
using System.Diagnostics;
using System.Net;
using System.Collections.Generic;
using IMOD.Domain.Constantes;

namespace IMOD.Infra.Ado
{

    /// <summary>
    ///     Obtem configurações correntes
    /// </summary>
    public static class CurrentConfig
    {
        #region  Propriedades

        /// <summary>
        ///     String de conexao com o banco de dados
        /// </summary>
        //public static string ConexaoString => Settings.Default.Credenciamento;
        public static string ConexaoString => GetConnectionString();

        // private readonly TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
        //private readonly MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
        //private const string key = "iModEstrela2016";
        public static string GetConnectionString()
        {            
            string returnValue = null;
            ////////////////////////////////////////
            /// Para o Projeto Credenciamento e WinService
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ////////////////////////////////////////                        
            string instancia = string.Empty;
            string banco = string.Empty;
            string usuario = string.Empty;
            string senha = string.Empty;
            string complemento = string.Empty;
            ////////////////////////////////////////
            ///Módulos que usam o SDK
            if (!string.IsNullOrEmpty(UsuarioLogado.sdiLicenca))
            {
                path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var prods = (from p in XElement.Load(path + "\\Conexao.xml").Elements("Produto")
                            where p.Element("SystemID").Value == UsuarioLogado.sdiLicenca   // Quando esta sendo usudo pelo Credenciamento
                            select new
                            {
                                instancia = p.Element("InstanciaSQL").Value,
                                banco = p.Element("Banco").Value,
                                usuarioDB = p.Element("UsuarioDB").Value,
                                SenhaDB = p.Element("SenhaDB").Value,
                                complemento = p.Element("Complemento").Value,
                            }).FirstOrDefault();
                
                if (prods != null)
                {
                    instancia = prods.instancia;
                    banco = prods.banco;
                    usuario = prods.usuarioDB;
                    senha = prods.SenhaDB;
                    complemento = prods.complemento;
                }
            }
            else
            {
                string processo = Process.GetCurrentProcess().ProcessName;
                List<string> ipList = new List<string>();
                string campoPesquisa = "ServerCarga";
                
                foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    ipList.Add(ip.ToString());
                }

                //verifica se o ip utilizado será o da carga ou do servidor web - IMOD.ImodApp
                if (processo.Equals("IMOD.ImodApp"))
                {

                }
                else if(!processo.Equals("IMOD.Service"))
                {
                    campoPesquisa = "ServerWeb";
                    path = HttpContext.Current.Server.MapPath("~");
                }
                

                var prods = (from p in XElement.Load(path + "\\Conexao.xml").Elements("Produto")
                             where ipList.Contains(p.Element(campoPesquisa).Value)     // Quando esta sendo usudo pelo WinSevice e Pre-Cadastro
                             select new
                             {
                                 instancia = p.Element("InstanciaSQL").Value,
                                 banco = p.Element("Banco").Value,
                                 usuarioDB = p.Element("UsuarioDB").Value,
                                 SenhaDB = p.Element("SenhaDB").Value,
                                 complemento = p.Element("Complemento").Value,
                             }).FirstOrDefault();
                if (prods == null)               
                {
                    prods = (from p in XElement.Load(path + "\\Conexao.xml").Elements("Produto")
                             where p.Element(campoPesquisa).Value == string.Empty     // Quando esta sendo usudo pelo WinSevice e Pre-Cadastro
                             select new
                             {
                                 instancia = p.Element("InstanciaSQL").Value,
                                 banco = p.Element("Banco").Value,
                                 usuarioDB = p.Element("UsuarioDB").Value,
                                 SenhaDB = p.Element("SenhaDB").Value,
                                 complemento = p.Element("Complemento").Value,
                             }).FirstOrDefault();
                }

                if (prods != null)
                {
                    instancia = prods.instancia;
                    banco = prods.banco;
                    usuario = prods.usuarioDB;
                    senha = prods.SenhaDB;
                    complemento = prods.complemento;
                }


            }
            ////////////////////////////////////////
            EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
            EstrelaEncryparDecrypitar.Variavel.key = Constante.CRIPTO_KEY;
            string senhaDecryptada = ESTRELA_EMCRYPTAR.EstrelaDecrypt(senha);
            ////////////////////////////////////////
            returnValue = @"Data Source=" + instancia + ";Initial Catalog=" + banco + ";User ID=" + usuario + ";Password=" + senhaDecryptada + ";" + complemento;
            //returnValue = @"Data Source=SCVIRTUAL\SQLEXPRESS;Initial Catalog=D_iModCredenciamento;User ID=imod;Password=imod;Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";
            //returnValue = @"Data Source=GCTEC04;Initial Catalog=D_iModCredenciamento;User ID=imod;Password=imod;Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";
            //Data Source=GCVIRTUAL\\SQLEXPRESS;Initial Catalog=D_iModCredenciamento;User ID=imod;Password=imod;Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True
            //Debug.Print(@returnValue);
            UsuarioLogado.InstanciaSQL = returnValue;
            ////////////////////////////////////////
            return returnValue;

        }

        #endregion
    }
}