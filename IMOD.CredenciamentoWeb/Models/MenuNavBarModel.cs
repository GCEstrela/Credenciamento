 
#region

using System;
using System.Collections.Generic;
using IMOD.CredenciamentoWeb.ViewModel;

#endregion

namespace IMOD.CredenciamentoWeb.Models
{
    public class MenuNavBarModel
    {
        #region Variaveis Globais

        /// <summary>
        ///     Commom item menu
        /// </summary>
        private readonly List<NavbarViewModel> _list = new List<NavbarViewModel>();

        #endregion

        public IEnumerable<NavbarViewModel> ObterMenuItem(string role)
        {
            switch (role)
            {
                case "adm":
                    return ObterMenuItemMaster();
                case "user":
                    return ObterMenuItemUser();
                default:
                    throw new ArgumentOutOfRangeException("Falha ao carregar Menu item");
            }
        }

        private IEnumerable<NavbarViewModel> ObterMenuItemMaster()
        {
            _list.Add(new NavbarViewModel
            {
                Id = 1,
                nameOption = "Usuários",
                imageClass = "",
                controller = "Usuario",
                action = "Listar",
                status = true,
                isParent = false
            });
            _list.Add(new NavbarViewModel
            {
                Id = 2,
                nameOption = "Credenciamento", 
                controller = "Licenca",
                imageClass = "",
                action = "Listar",
                status = true,
                isParent = false
            });
            //_list.Add(new NavbarViewModel
            //{
            //    Id = 4,
            //    nameOption = "Indicadores", 
            //    controller = "home",
            //    imageClass = "",
            //    action = "index",
            //    status = true,
            //    isParent = false
            //});

            //_list.Add(new NavbarViewModel
            //{
            //    Id = 4,
            //    nameOption = "Minha Fatura", 
            //    controller = "Pagamento",
            //    imageClass = "",
            //    action = "MinhaFatura",
            //    status = true,
            //    isParent = false
            //});

            //_list.Add(new NavbarViewModel
            //{
            //    Id = 4,
            //    nameOption = "Minha Licença", 
            //    controller = "Licenca",
            //    imageClass = "",
            //    action = "MinhaLicenca",
            //    status = true,
            //    isParent = false
            //});

            return _list;
        }

        private IEnumerable<NavbarViewModel> ObterMenuItemUser()
        {
            _list.Add(new NavbarViewModel
            {
                Id = 4,
                nameOption = "Indicadores",
                imageClass = "fa fa-bar-chart-o fa-fw",
                controller = "home",
                action = "index",
                status = true,
                isParent = false
            });
            return _list;
        }

        ///// </summary>
        ///// <para>Level granted to the user level full</para>
        ///// Level full

        ///// <summary>
        ///// <returns></returns>
        //public IEnumerable<Navbar> GetLevelFull()
        //{ 
        //    _list.AddRange(GetLevelOne());
        //    _list.Add(new Navbar { Id = 5, nameOption = "Tables", controller = "Home", action = "Tables", imageClass = "fa fa-table fa-fw", status = true, isParent = false, parentId = 0 });
        //    _list.Add(new Navbar { Id = 6, nameOption = "Forms", controller = "Home", action = "Forms", imageClass = "fa fa-edit fa-fw", status = true, isParent = false, parentId = 0 });
        //    _list.Add(new Navbar { Id = 7, nameOption = "UI Elements", imageClass = "fa fa-wrench fa-fw", status = true, isParent = true, parentId = 0 });
        //    _list.Add(new Navbar { Id = 8, nameOption = "Panels and Wells", controller = "Home", action = "Panels", status = true, isParent = false, parentId = 7 });
        //    _list.Add(new Navbar { Id = 9, nameOption = "Buttons", controller = "Home", action = "Buttons", status = true, isParent = false, parentId = 7 });
        //    _list.Add(new Navbar { Id = 10, nameOption = "Notifications", controller = "Home", action = "Notifications", status = true, isParent = false, parentId = 7 });
        //    _list.Add(new Navbar { Id = 11, nameOption = "Typography", controller = "Home", action = "Typography", status = true, isParent = false, parentId = 7 });
        //    _list.Add(new Navbar { Id = 12, nameOption = "Icons", controller = "Home", action = "Icons", status = true, isParent = false, parentId = 7 });
        //    _list.Add(new Navbar { Id = 13, nameOption = "Grid", controller = "Home", action = "Grid", status = true, isParent = false, parentId = 7 });
        //    _list.Add(new Navbar { Id = 14, nameOption = "Multi-Level Dropdown", imageClass = "fa fa-sitemap fa-fw", status = true, isParent = true, parentId = 0 });
        //    _list.Add(new Navbar { Id = 15, nameOption = "Second Level Item", status = true, isParent = false, parentId = 14 });
        //    _list.Add(new Navbar { Id = 16, nameOption = "Sample Pages", imageClass = "fa fa-files-o fa-fw", status = true, isParent = true, parentId = 0 });
        //    _list.Add(new Navbar { Id = 17, nameOption = "Blank Page", controller = "Home", action = "Blank", status = true, isParent = false, parentId = 16 });
        //    _list.Add(new Navbar { Id = 18, nameOption = "Login Page", controller = "Home", action = "Login", status = true, isParent = false, parentId = 16 });

        //    return _list;
        //}

        ///// <summary>
        ///// Level one
        ///// <para>Level granted to the user level one</para>
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Navbar> GetLevelOne()
        //{

        //    _list.Add(new Navbar { Id = 2, nameOption = "Painel de Indicadores", imageClass = "fa fa-bar-chart-o fa-fw", status = true, isParent = true, parentId = 0 });
        //    _list.Add(new Navbar { Id = 3, nameOption = "Basico", controller = "Home", action = "MainGraph", status = true, isParent = false, parentId = 2 });
        //    //_list.Add(new Navbar { Id = 4, nameOption = "Vendas por mês", controller = "Home", action = "MorrisCharts", status = true, isParent = false, parentId = 2 });
        //    return _list;
        //}

        ///// <summary>
        ///// Level one
        ///// <para>Level granted to the user level one</para>
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Navbar> GetLevelMaster()
        //{

        //    _list.Add(new Navbar { Id = 1, nameOption = "Administração", imageClass = "fa fa-bar-chart-o fa-fw", status = true, isParent = true, parentId = 0 });
        //    _list.Add(new Navbar { Id = 2, nameOption = "Usuários", controller = "Usuario", action = "Index", status = true, isParent = false, parentId = 1 });
        //    _list.Add(new Navbar { Id = 3, nameOption = "Minhas licenças", controller = "Licenca", action = "MinhasLicencas", status = true, isParent = false, parentId = 1 });
        //    _list.Add(new Navbar { Id = 4, nameOption = "Licenças", controller = "Licenca", action = "Licencas", status = true, isParent = false, parentId = 1 });
        //    return _list;

        //}

        ///// <summary>
        ///// Level two
        ///// <para>Level granted to the user level two</para>
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Navbar> GetLevelTwo()
        //{
        //    _list.AddRange(GetLevelOne());
        //    //add here more itens of the menu
        //    return _list;
        //}
    }
}