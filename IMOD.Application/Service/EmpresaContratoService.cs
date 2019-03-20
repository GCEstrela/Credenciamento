// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Linq;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class EmpresaContratoService : IEmpresaContratosService
    {
        private readonly IEmpresaContratoRepositorio _repositorio = new EmpresaContratoRepositorio();
        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();
        private ConfiguraSistema _configuraSistema;

        #region  Propriedades

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }

        public IAreaAcessoService AreaAceesso
        {
            get { return new AreaAcessoService(); }
        }

        public ITiposAcessoService TipoAcesso
        {
            get { return new TipoAcessoService(); }
        }

        public IStatusService Status
        {
            get { return new StatusService(); }
        }

        public ITipoCobrancaService TipoCobranca
        {
            get { return new TipoCobrancaService(); }
        }

        public IEstadoService Estado
        {
            get { return new EstadoService(); }
        }

        public IMunicipioService Municipio
        {
            get { return new MunicipioService(); }
        }
        public bool IsEnableComboContrato { get; private set; } = true;
        #endregion
        #region  Metodos
        /// <summary>
        /// Obtem configuração de sistema
        /// </summary>
        /// <returns></returns>
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresServiceConfiguraSistema.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }
        /// <summary>
        ///     Alterar a data de validade de um contrato básico para a maior data de validade de um outro contrato
        /// </summary>
        /// <param name="entity"></param>
        private void AlterarMaiorDataValidadeContratoBasico(EmpresaContrato entity)
        {
            //if (!contrato)
            //{
            //    return; 
            //}
            //Verificar se há um contrato básico
            //Status ativo
            var status = Status.Listar().FirstOrDefault(n => n.CodigoStatus);
            if (status == null) throw new InvalidOperationException("Informe um status do ativo para continuar");
            var n1 = ListarPorEmpresa (entity.EmpresaId); //Contratos associado a empresa com status ativo
            if (!n1.Any (n => n.ContratoBasico)) return;
            var n2 = n1.FirstOrDefault (n => n.ContratoBasico);
            if (n2 == null) return;
            var result = DateTime.Compare (entity.Validade, n2.Validade);
            if (result <= 0) return;
            //Alterar para a maior data
            n2.Validade = entity.Validade;
            _repositorio.Alterar (n2);
        }

        /// <summary>
        ///     Alterar a data de validade de um contrato básico para a maior data de validade dentre todos os contratos
        ///     vinculados a empresa
        /// </summary>
        /// <param name="entity"></param>
        public void AlterarMaiorDataValidadeContratoBasicoEntreContratos(EmpresaContrato entity)
        {
            //Verificar se há um contrato básico
            //Status ativo
            var status = Status.Listar().FirstOrDefault (n => n.CodigoStatus);
            if(status==null) throw new InvalidOperationException("Informe um status do ativo para continuar");
            var n1 = ListarPorEmpresa(entity.EmpresaId); //Contratos associado a empresa com status ativo
            if (!n1.Any(n => n.ContratoBasico)) return;
            //Obtêm a maior data de validade dos contratos, (exceto o contrato básico)
            var dtValidadeMax = n1.Where(n => !n.ContratoBasico & entity.StatusId == status.StatusId).Max (n => n.Validade);
            var n2 = n1.FirstOrDefault(n => n.ContratoBasico);
            if (n2 == null) return;
            //Alterar para a maior data
            n2.Validade = dtValidadeMax;
            _repositorio.Alterar(n2);
        }

        /// <summary>
        ///     Criar um contrato básico
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dataValidade">Data de Validade</param>
        /// <param name="status">Status do contrato</param>
        public void CriarContratoBasico(Empresa entity, DateTime dataValidade, Status status)
        {
            var contrato = new EmpresaContrato();
            contrato.EmpresaId = entity.EmpresaId;
            contrato.Validade = dataValidade;
            contrato.Descricao = "Contrato Básico";
            contrato.StatusId = status.StatusId;
            contrato.NumeroContrato = "0";
            contrato.Emissao = contrato.Validade;
            contrato.ContratoBasico = true;
            Criar (contrato);
        }

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(EmpresaContrato entity)
        {
            _configuraSistema = ObterConfiguracao();
            if (_configuraSistema.Contrato)
            {
                AlterarMaiorDataValidadeContratoBasicoEntreContratos(entity);
            }
            _repositorio.Criar (entity);

            #region Retirar pendencias de sistema exceto para contratos do tipo básico

            if (!entity.ContratoBasico)
            {
                var pendencia = Pendencia.ListarPorEmpresa(entity.EmpresaId)
                    .FirstOrDefault(n => n.PendenciaSistema & (n.CodPendencia == 14));
                if (pendencia == null) return;
                Pendencia.Remover(pendencia);
            }
           

            #endregion
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public EmpresaContrato BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave (id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<EmpresaContrato> Listar(params object[] objects)
        {
            return _repositorio.Listar (objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(EmpresaContrato entity)
        {
           
            _repositorio.Alterar (entity);
            _configuraSistema = ObterConfiguracao();
            if (_configuraSistema.Contrato)
            {
                AlterarMaiorDataValidadeContratoBasicoEntreContratos(entity);
            }

        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(EmpresaContrato entity)
        {
            _repositorio.Remover (entity);
        }

        /// <summary>
        ///     Listar contratos por número
        /// </summary>
        /// <param name="numContrato"></param>
        /// <returns></returns>
        public ICollection<EmpresaContrato> ListarPorNumeroContrato(string numContrato)
        {
            return _repositorio.ListarPorNumeroContrato (numContrato);
        }

        /// <summary>
        ///     Buscar numero do contrato
        /// </summary>
        /// <param name="numContrato"></param>
        /// <returns></returns>
        public EmpresaContrato BuscarContrato(string numContrato)
        {
            return _repositorio.BuscarContrato (numContrato);
        }

        /// <summary>
        ///     Listar contratos por descrição
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public ICollection<EmpresaContrato> ListarPorDescricao(string desc)
        {
            return _repositorio.ListarPorDescricao (desc);
        }

        /// <summary>
        ///     Listar contratos por empresa
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public ICollection<EmpresaContrato> ListarPorEmpresa(int empresaId)
        {
            return _repositorio.ListarPorEmpresa (empresaId);
        }

        #endregion
    }
}