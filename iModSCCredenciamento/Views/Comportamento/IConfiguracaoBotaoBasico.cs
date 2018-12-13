using System.Windows;

namespace iModSCCredenciamento.Views.Comportamento
{
    /// <summary>
    /// Configura comporamento do menu CRUD padrão
    /// </summary>
   public interface IConfiguracaoBotaoBasico
    {
        /// <summary>
        /// Editar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnEditar_Click(object sender, RoutedEventArgs e);

        /// <summary>
        /// Adicionar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAdicionar_Click(object sender, RoutedEventArgs e);

        /// <summary>
        /// Excluir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnExcluir_Click(object sender, RoutedEventArgs e);

        /// <summary>
        /// Pesquisar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPesquisar_Click(object sender, RoutedEventArgs e);

        /// <summary>
        /// Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCancelar_Click(object sender, RoutedEventArgs e);
        /// <summary>
        /// Salvar Edição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSalvarEditar(object sender, RoutedEventArgs e);

        /// <summary>
        /// Salvar Adição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSalvarAdicionar(object sender, RoutedEventArgs e);
    }
}
