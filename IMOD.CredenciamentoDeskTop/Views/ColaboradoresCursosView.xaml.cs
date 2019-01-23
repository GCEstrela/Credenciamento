using System;
using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    /// Interação lógica para ColaboradoresCursosView.xam
    /// </summary>
    public partial class ColaboradoresCursosView : UserControl
    {
        private readonly ColaboradoresCursosViewModel _viewModel;
        #region Inicializacao
        public ColaboradoresCursosView()
        {
            InitializeComponent();
            //DataContext = new ColaboradoresCursosViewModel();
            _viewModel = new ColaboradoresCursosViewModel();
            DataContext = _viewModel;
        }
        #endregion

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCurso.SelectionChanged += OnSelecionaCurso_SelectionChanged;
        }
        private void OnSelecionaCurso_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (_viewModel.Cursos == null) return;
            //_viewModel.ListarContratos(_viewModel.);
        }
        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.ColaboradorView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados(entity);
        }

        private void OnUpLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _viewModel.Entity.Arquivo = arq.FormatoBase64;
                _viewModel.Entity.NomeArquivo = arq.Nome;
                NomeArquivo_tb.Text = arq.Nome;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        private void OnAbrirArquivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var arrayByes = Convert.FromBase64String(_viewModel.Entity.Arquivo);
                WpfHelp.AbrirArquivoPdf(_viewModel.Entity.NomeArquivo, arrayByes);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
    }
}
