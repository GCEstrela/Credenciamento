using System;
using System.Windows;
using System.Windows.Controls;

namespace iModSCCredenciamento.Views.Comportamento
{
    public   class ConfiguracaoBotaoBasico
    {

        private enum Estado
        {
            Editar = 0,
            Adicionar = 1,
            Cancelar = 2,
            Excluir = 3,
            Pesquisar = 4
        }

        public string EstadoView { get; private set; }
        private RoutedEventHandler _onSalvarAdicao;
        private RoutedEventHandler _onSalvarEdicao;
        private IConfiguracaoBotaoBasico _configuracaoView;
        private Button _btn_Adicionar;
        private Button _btn_Editar;
        private Button _btn_Cancelar;
        private Button _btn_Excluir;
        private Button _btn_Pesquisar;
        private Button _btn_Salvar;              
        
        public ConfiguracaoBotaoBasico(Button btn_Pesquisar, Button btn_Adicionar,
              Button btn_Editar, Button btn_Excluir,
              Button btn_Salvar,  Button btn_Cancelar, IConfiguracaoBotaoBasico configuracaoView)
        {
            if (configuracaoView == null) throw new ArgumentNullException("A instância do objeto da View deve ser informado");
            _btn_Pesquisar = btn_Pesquisar;
            _btn_Adicionar = btn_Adicionar;
            _btn_Cancelar = btn_Cancelar;
            _btn_Excluir = btn_Excluir;
            _btn_Salvar = btn_Salvar;
            _btn_Editar = btn_Editar;
            _onSalvarEdicao = configuracaoView.OnSalvarEditar;
            _onSalvarAdicao = configuracaoView.OnSalvarAdicionar;
            _configuracaoView = configuracaoView;
            //----------------------------------------
            _btn_Editar.Click += OnAlterar_Click;
            _btn_Adicionar.Click += OnNovo_Click;
            //----------------------------------------
            btn_Pesquisar.Click += configuracaoView.OnPesquisar_Click;
            btn_Adicionar.Click += configuracaoView.OnAdicionar_Click;
            btn_Editar.Click += configuracaoView.OnEditar_Click;
            btn_Excluir.Click += configuracaoView.OnExcluir_Click;
            btn_Cancelar.Click += configuracaoView.OnCancelar_Click;

        }

        /// <summary>
        ///     Controla o estado do formulario ao clicar no botão Novo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNovo_Click(object sender, EventArgs e)
        {
            EstadoAdicionar();

            _btn_Salvar.Click -= _onSalvarAdicao;
            _btn_Salvar.Click -= _onSalvarEdicao;
            //----------------------------------------
            _btn_Salvar.Click += _onSalvarAdicao;//Delegate Event 
            //----------------------------------------
        }

        /// <summary>
        ///     Controla o estado do formulario ao clicar no botão Alterar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAlterar_Click(object sender, EventArgs e)
        {
            EstadoEditar();

            _btn_Salvar.Click -= _onSalvarAdicao;
            _btn_Salvar.Click -= _onSalvarEdicao;
            //----------------------------------------
            _btn_Salvar.Click += _onSalvarEdicao;//Delegate Event 
            //----------------------------------------
        }
        /// <summary>
        /// Adicionar estado ao formulario
        /// </summary>
        /// <param name="IsEnablePesquisar">True, habilitar botão Pesquisar</param>
        /// <param name="IsEnableAdicionar">True, habilitar botão Adicionar</param>
        /// <param name="IsEnableEditar">True, habilitar botão Editar</param>
        /// <param name="IsEnableExcluir">True, habilitar botão Excluir</param>
        /// <param name="IsEnableSalvar">True, habilitar botão Salvar</param>
        /// <param name="IsEnableCancelar">True, habilitar botão Cancelar </param>
        public void AdicionarEstado(bool IsEnablePesquisar, bool IsEnableAdicionar, bool IsEnableEditar, bool IsEnableExcluir, bool IsEnableSalvar, bool IsEnableCancelar)
        {          
            _btn_Pesquisar.IsEnabled = IsEnablePesquisar;
            _btn_Adicionar.IsEnabled = IsEnableAdicionar;
            _btn_Cancelar.IsEnabled = IsEnableCancelar;
            _btn_Excluir.IsEnabled = IsEnableExcluir;
            _btn_Salvar.IsEnabled = IsEnableSalvar;
            _btn_Editar.IsEnabled = IsEnableEditar;

        }

        public void EstadoAdicionar()
        {
            AdicionarEstado(true, true, false, false, true, true); 
        }

        public void EstadoEditar()
        {
            AdicionarEstado(true, false, true, false, true, true); 
        }
        
        public void EstadoPronto()
        {
           AdicionarEstado(true, true, true,true, true, true);
        }

        public void EstadoCustomizado()
        {

        }
        public void EstadoExcluir()
        {
            AdicionarEstado(true, false, false, true, true, true);
            EstadoView = Estado.Excluir.ToString();
        }

        public void EstadoCancelar()
        {
            AdicionarEstado(true, true, true, true, false, true);
            EstadoView = Estado.Cancelar.ToString();
        }   

        public void EstadoPesquisar()
        {
            AdicionarEstado(true, true, true, true, false, true);
            EstadoView = Estado.Pesquisar.ToString();
        }
        public void EstadoSalvar()
        {
            AdicionarEstado(true, true, true, true, false, true); 
        }












    }
}
