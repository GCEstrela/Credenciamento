// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  13 - 11 - 2019
// ***********************************************************************

#region

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    /// Interação lógica para EmpresasSegurosView.xam
    /// </summary>
    public partial class EmpresasSegurosView : UserControl
    {
        private readonly EmpresasSegurosViewModel _viewModel;
        #region Inicializacao
        public EmpresasSegurosView()
        {
            InitializeComponent();
            _viewModel = new EmpresasSegurosViewModel();
            DataContext = _viewModel;
            
        }
        #endregion

        #region  Metodos
        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModelParent"></param>
        public void AtualizarDados(Model.EmpresaView entity, EmpresaViewModel viewModelParent)
        {
            //if (entity == null) return;
            _viewModel.AtualizarDados(entity, viewModelParent);
        }
        #endregion
    }
}
