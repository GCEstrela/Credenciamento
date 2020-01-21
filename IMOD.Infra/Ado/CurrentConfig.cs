﻿// ***********************************************************************
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

            ////////////////////////////////////////
            string returnValue = null;
            ////////////////////////////////////////
            /// Para o Projeto Credenciamento e WinService
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ////////////////////////////////////////


            XmlDocument xmlDoc = new XmlDocument();
            ////////////////////////////////////////
            string instancia = "";
            string banco = "";
            string usuario = "";
            string senha = "";
            string complemento = "";

            ////////////////////////////////////////
            // cria a consulta
            if (!string.IsNullOrEmpty(UsuarioLogado.sdiLicenca))
            {
                path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var prods = from p in XElement.Load(path + "\\Conexao.xml").Elements("Produto")
                            where p.Element("SystemID").Value == UsuarioLogado.sdiLicenca   // Quando esta sendo usudo pelo Credenciamento
                            select new
                            {
                                instancia = p.Element("InstanciaSQL").Value,
                                banco = p.Element("Banco").Value,
                                usuarioDB = p.Element("UsuarioDB").Value,
                                SenhaDB = p.Element("SenhaDB").Value,
                                complemento = p.Element("Complemento").Value,
                            };

                //Executa a consulta
                foreach (var produto in prods)
                {
                    instancia = produto.instancia;
                    banco = produto.banco;
                    usuario = produto.usuarioDB;
                    senha = produto.SenhaDB;
                    complemento = produto.complemento;
                }
            }
            else
            {
                string processo = Process.GetCurrentProcess().ProcessName;
                List<string> ipList = new List<string>();
                foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    ipList.Add(ip.ToString());
                }


                if (processo.Equals("IMOD.Service"))
                {
                         
                    var prods = (from p in XElement.Load(path + "\\Conexao.xml").Elements("Produto")
                                 where  ipList.Contains(p.Element("ServerCarga").Value)     // Quando esta sendo usudo pelo WinSevice e Pre-Cadastro
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
                    path = System.Web.HttpContext.Current.Server.MapPath("~");
                   
                    var prods = (from p in XElement.Load(path + "\\Conexao.xml").Elements("Produto")
                                 where ipList.Contains(p.Element("ServerCarga").Value)    // Quando esta sendo usudo pelo WinSevice e Pre-Cadastro
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
            }
            ////////////////////////////////////////
            EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
            EstrelaEncryparDecrypitar.Variavel.key = "CREDENCIAMENTO2019";
            string senhaDecryptada = ESTRELA_EMCRYPTAR.EstrelaDecrypt(senha);
            ////////////////////////////////////////
            returnValue = "Data Source=" + instancia + ";Initial Catalog=" + banco + ";User ID=" + usuario + ";Password=" + senhaDecryptada + ";" + complemento;
            UsuarioLogado.InstanciaSQL = returnValue;
            ////////////////////////////////////////
            return returnValue;

        }

        #endregion
    }
}