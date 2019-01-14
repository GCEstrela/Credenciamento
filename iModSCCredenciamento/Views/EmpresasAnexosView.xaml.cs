// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels;
using IMOD.CrossCutting;

#endregion

namespace iModSCCredenciamento.Views
{
    /// <summary>
    ///     Interação lógica para EmpresasAnexosView.xam
    /// </summary>
    public partial class EmpresasAnexosView : UserControl
    {
        private readonly EmpresasAnexosViewModel _viewModel;

        public EmpresasAnexosView()
        {
            InitializeComponent();
            _viewModel = new EmpresasAnexosViewModel();
            DataContext = _viewModel;
        }

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.EmpresaView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDadosAnexo(entity);
        }

        /// <summary>
        ///     UpLoad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _viewModel.Entity.Anexo = arq.FormatoBase64;
                _viewModel.Entity.NomeAnexo = arq.Nome;
                txtNomeAnexo.Text = arq.Nome;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///     Downlaod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var arrayByes = Convert.FromBase64String(_viewModel.Entity.Anexo);
                WpfHelp.AbrirArquivoPdf(_viewModel.Entity.NomeAnexo, arrayByes);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        #endregion
    }
}