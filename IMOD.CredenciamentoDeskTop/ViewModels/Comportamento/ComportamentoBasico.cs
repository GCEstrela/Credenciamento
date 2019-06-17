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
        private bool seguntatentativa = false;
        #region  Propriedades


        public bool IsEnableEditar { get; set; } = true;
        public bool IsEnableCriar { get; set; } = true;
        public bool isEnableRemover { get; set; } = true;
        public bool isEnableSalvar { get; set; }
        public bool isEnableCancelar { get; set; } = true;
        public bool isEnableLstView { get; set; } = true;
        public bool isEnableBotoes { get; set; } = true;

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
            isEnableLstView = false;
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
            try
            {
                AdicionarEstado(false, false, false, true, true, false);

                _salvar = Acao.SalvarAdicao;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///     Prepara botões para comportamento de criação
        /// </summary>
        public void PrepareCriarSegundaTentativa()
        {
            try
            {
                seguntatentativa = true;
                AdicionarEstado(false, false, false, true, true, false);

                _salvar = Acao.SalvarAdicao;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// </summary>
        public void PrepareAlterar()
        {
            try
            {
                AdicionarEstado(false, false, false, true, true, false);
                _salvar = Acao.SalvarEditar;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void PrepareCancelar()
        {
            AdicionarEstado(true, true, true, false, false, true);
            _salvar = Acao.Cancelar;
            OnCancelar(new RoutedEventArgs());
        }

        public void PrepareRemover()
        {
            try
            {
                AdicionarEstado(true, true, true, false, false, true);
                _salvar = Acao.Remover;
                OnRemover(new RoutedEventArgs());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void PrepareSalvar()
        {
            try
            {
                if (_salvar == Acao.SalvarEditar)
                    OnSalvarEdicao(new RoutedEventArgs());
                if (_salvar == Acao.SalvarAdicao)
                    OnSalvarAdicao(new RoutedEventArgs());
                if (!seguntatentativa)
                {
                    AdicionarEstado(true, true, true, false, false, true);
                    seguntatentativa = false;
                }
                else
                {
                    AdicionarEstado(false, false, false, true, true, false);
                    seguntatentativa = false;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
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
            try
            {
                Remover?.Invoke(this, e);
            }
            catch (System.Exception ex)
            {
                //throw ex;
            }
        }
        protected virtual void OnSalvarAdicao(RoutedEventArgs e)
        {
            try
            {
                SalvarAdicao?.Invoke(this, e);
            }
            catch (System.Exception ex)
            {
                //throw ex;
            }
        }

        protected virtual void OnSalvarEdicao(RoutedEventArgs e)
        {
            try
            {
                SalvarEdicao?.Invoke(this, e);
            }
            catch (System.Exception ex)
            {
                //throw ex;
            }
        }

        protected virtual void OnCancelar(RoutedEventArgs e)
        {
            try
            {
                Cancelar?.Invoke(this, e);
            }
            catch (System.Exception ex)
            {
                //throw ex;
            }
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