// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 18 - 2018
// ***********************************************************************

#region

using System.Windows;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels.Comportamento
{
    public class ComportamentoBasico : ViewModelBase
    {
        private Acao _salvar;

        #region  Propriedades


        public bool IsEnableEditar { get; set; } = true;
        public bool IsEnableCriar { get; set; } = true;
        public bool isEnableRemover { get; set; } = true;
        public bool isEnableSalvar { get; set; }
        public bool isEnableCancelar { get; set; } = true;
        public bool isEnableLstView { get; set; } = true;

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="btnEditarEHabilitado">Propriedade do botão Editar</param>
        /// <param name="btnCriarEHabilitado">Propriedade do botão  Criar</param>
        /// <param name="btnExcluirEHabilitado">Propriedade do botão Remover</param>
        /// <param name="btnSalvarEHabilitado">Propriedade do botão  Salvar</param>
        /// <param name="cancelarHabilitado">Propriedade do botão Cancelar</param>
        public ComportamentoBasico(bool btnEditarEHabilitado,
            bool btnCriarEHabilitado, bool btnExcluirEHabilitado,
            bool btnSalvarEHabilitado, bool cancelarHabilitado)
        {
            IsEnableEditar = btnEditarEHabilitado;
            IsEnableCriar = btnCriarEHabilitado;
            isEnableRemover = btnExcluirEHabilitado;
            isEnableSalvar = btnSalvarEHabilitado;
            isEnableCancelar = cancelarHabilitado;

        }

        #region  Metodos

        public event RoutedEventHandler Remover;
        public event RoutedEventHandler SalvarAdicao;
        public event RoutedEventHandler SalvarEdicao;
        public event RoutedEventHandler Cancelar;

        /// <summary>
        ///     Prepara botões para comportamento de criação
        /// </summary>
        public void PrepareCriar()
        {
            AdicionarEstado(false, false, false, true, true, false);

            _salvar = Acao.SalvarAdicao;
        }

        /// <summary>
        /// </summary>
        public void PrepareAlterar()
        {
            AdicionarEstado(false, false, false, true, true, false);
            _salvar = Acao.SalvarEditar;
        }

        public void PrepareCancelar()
        {
            AdicionarEstado(true, true, true, false, false, true);
            _salvar = Acao.Cancelar;
            OnCancelar(new RoutedEventArgs());
        }

        public void PrepareRemover()
        {
            AdicionarEstado(true, true, true, false, false, true);
            _salvar = Acao.Remover;
            OnRemover(new RoutedEventArgs());
        }

        public void PrepareSalvar()
        {
            if (_salvar == Acao.SalvarEditar)
                OnSalvarEdicao(new RoutedEventArgs());
            if (_salvar == Acao.SalvarAdicao)
                OnSalvarAdicao(new RoutedEventArgs());

            AdicionarEstado(true, true, true, false, false, true);
        }

        /// <summary>
        ///     Adicionar estado ao formulario
        /// </summary>
        /// <param name="isEnableCriar">True, habilitar botão Adicionar</param>
        /// <param name="isEnableEditar">True, habilitar botão Editar</param>
        /// <param name="isEnableRemover">True, habilitar botão Excluir</param>
        /// <param name="isEnableSalvar">True, habilitar botão Salvar</param>
        /// <param name="isEnableCancelar">True, habilitar botão Cancelar </param>
        private void AdicionarEstado(bool isEnableCriar, bool isEnableEditar, bool isEnableRemover,
            bool isEnableSalvar, bool isEnableCancelar, bool isEnableLstView)
        {
            this.IsEnableCriar = isEnableCriar;
            this.isEnableCancelar = isEnableCancelar;
            this.isEnableRemover = isEnableRemover;
            this.isEnableSalvar = isEnableSalvar;
            this.IsEnableEditar = isEnableEditar;
            this.isEnableLstView = isEnableLstView;
        }

        protected virtual void OnRemover(RoutedEventArgs e)
        {
            Remover?.Invoke(this, e);
        }

        protected virtual void OnSalvarAdicao(RoutedEventArgs e)
        {
            SalvarAdicao?.Invoke(this, e);
        }

        protected virtual void OnSalvarEdicao(RoutedEventArgs e)
        {
            SalvarEdicao?.Invoke(this, e);
        }

        protected virtual void OnCancelar(RoutedEventArgs e)
        {
            Cancelar?.Invoke(this, e);
        }

        #endregion

        private enum Acao
        {
            SalvarEditar,
            SalvarAdicao,
            Remover,
            Cancelar
        }
    }
}