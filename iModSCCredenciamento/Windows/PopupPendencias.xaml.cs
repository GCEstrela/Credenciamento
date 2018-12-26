// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System.Windows;
using System.Windows.Input;
using iModSCCredenciamento.Enums;
using iModSCCredenciamento.ViewModels;

#endregion

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    ///     Lógica interna para PopPendencias.xaml
    /// </summary>
    public partial class PopupPendencias : Window
    {
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
            DataContext = new PopupPendenciasViewModel (codPendencia, identificador, tipoPendencia);
        }

       

        #endregion
    }
}