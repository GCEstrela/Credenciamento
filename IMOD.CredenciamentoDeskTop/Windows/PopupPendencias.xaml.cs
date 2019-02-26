// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.Enums;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    ///     Lógica interna para PopPendencias.xaml
    /// </summary>
    public partial class PopupPendencias : Window
    {
        private  PopupPendenciasViewModel _viewModel;
        public PopupPendencias()
        {
            InitializeComponent();
        }

        #region  Metodos

        /// <summary>
        ///     Inicializar
        /// </summary>
        /// <param name="codPendencia">Cóodigo da pendencia</param>
        /// <param name="identificador">Identificador (IdEmpresa,IdColaborador ou IdVeiculo</param>
        /// <param name="tipoPendencia">Classe de Pendencia</param>
        public void Inicializa(int codPendencia, int identificador, PendenciaTipo tipoPendencia)
        {
            _viewModel = new PopupPendenciasViewModel (codPendencia, identificador, tipoPendencia);
            DataContext = _viewModel;
        }



        #endregion

        private void OnFormatDateLimite_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDateDataLimite.Text;
                txtDateDataLimite.Text = str.FormatarData();
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("DataLimite", "Data inválida");
            }
           
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}