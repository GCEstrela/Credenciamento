// ***********************************************************************
// Project: IMOD.CredenciamentoWeb
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 10 - 2019
// ***********************************************************************

#region



#endregion

using IMOD.CrossCutting;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.UI;
using AutoMapper;
using IMOD.CredenciamentoWeb.ViewModel;

namespace IMOD.CredenciamentoWeb.Models
{
    public class UsuarioModel //: UsuarioRepository
    {
        public ClaimsIdentity ObterIdentidade(Usuario user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Role, user.Perfil),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.IdUser.ToString())
            };

            var identity = new ClaimsIdentity(claims, "cookieAutentication");
            return identity;
        }
        /// <summary>
        ///     Login de Usuario
        /// </summary>
        /// <param name="login">Login do usuario</param>
        /// <param name="senha">Senha</param>
        /// <returns></returns>
        public bool Login(string login, string senha)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (senha == null) throw new ArgumentNullException(nameof(senha));
            try
            {
                //O login deve ser CaseInsensitive.
                var usuario = login.ToLower();
                 //TODO: Criar sistema de login
                //var oUsr = Listar().Single(n => n.Email.ToLower() == usuario);
                //Nao pode existir mais de dois usuarios com o mesmo nome
                //if (oUsr == null) return false;
                //var senha1 = Utils.DecryptAes(oUsr.Senha);

                //return string.Compare(senha1, senha, StringComparison.OrdinalIgnoreCase) == 0;
                return true;
            }
            catch (Exception ex)
            {
                //Utils.TraceException(ex);
                return false;
            }
        }

        public Usuario ObterUsuario(string email, string login)
        {
            //TODO:Acessar dados de usuário no repositório
            return new Usuario();
        }

        public void Alterar(Usuario entity)
        {
            //TODO:Acessar dados de usuário no repositório
             
        }

        public void Criar(Usuario entity)
        {
            //TODO:Acessar dados de usuário no repositório

        }

        public ICollection<Usuario> ListarAdministrador()
        {
            //TODO:Acessar dados de usuário no repositório
            return new List<Usuario>();
        }

        /// <summary>
        /// Buscar pela Chave
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public Usuario BuscarPelaChave(int idUser)
        {
            //TODO:Acessar dados de usuário no repositório
            return new Usuario();
        }

        ///// <summary>
        /////     Criar usuario comum
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public void CriarUsuarioComum(Usuario entity)
        //{
        //    entity.Perfil = "User";
        //    Criar(entity);
        //}

        ///// <summary>
        ///// Obter dados do usuario
        ///// </summary>
        ///// <param name="user"></param>
        ///// <returns></returns>
        //public UsuarioProfileViewModel ObterUsuario(IPrincipal user)
        //{
        //    if (!user.Identity.IsAuthenticated) throw new Exception();
        //    var userIdentity = (ClaimsIdentity)user.Identity;
        //    var key = userIdentity.FindFirst("id");
        //    var usuarioModel = new UsuarioModel();
        //    return usuarioModel.Conta(int.Parse(key.Value));

        //}

        ///// <summary>
        ///// Obter conta do usuário
        ///// </summary>
        ///// <param name="idUser"></param>
        ///// <returns></returns>
        //public UsuarioProfileViewModel Conta(int idUser)
        //{
        //    //Obter usuario logado
        //    var conta = Listar().FirstOrDefault(n => n.IdUser == idUser);
        //    if (conta == null) throw new Exception("Usuário não existente");
        //    var model = Mapper.Map<UsuarioProfileViewModel>(conta);
        //    var licModel = new LicencaModel();
        //    model.Senha = Utils.DecryptAes(conta.Senha); //Decripte senha
        //    model.Licencas = new List<DadosLicencaViewModel>();
        //    foreach (var item in conta.Licenses)
        //    {
        //        var d1 = licModel.ConvertXmlString(item.Content);

        //        model.Licencas.Add(new DadosLicencaViewModel
        //        {
        //            IdLicenca = d1.IdLicenca,
        //            DataRenovacao = d1.DataExpira,
        //            DataExpira = d1.DataExpira,
        //            IdUser = conta.IdUser,
        //            Cnpj = d1.Cnpj,
        //            LicenciadoPara = d1.LicenciadoPara,
        //            HardwareInfo = d1.HardwareInfo,
        //            NumUsuariosSimultaneos = d1.NumUsuariosSimultaneos
        //        });
        //    }

        //    return model;
        //}
    }
}