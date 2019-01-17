// ***********************************************************************
// Project: UnitTestImod
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
// ***********************************************************************

#region

using IMOD.Application.Service;
using IMOD.Domain.Entities;
using IMOD.Infra.Repositorios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

#endregion

namespace UnitTestImod
{
    [TestClass]
    public class UnitTest1
    {
        private AreaAcesso _areaAcesso;
        private Colaborador _colaborador;
        private ColaboradorCredencial _colaboradorCredencial;
        private ColaboradorEmpresa _colaboradorEmpresa;
        private ColaboradorCurso _colaboradorCurso;
        private ColaboradorAnexo _colaboradorAnexo;
        private CredencialStatus _credencialStatus;
        private CredencialMotivo _credencialMotivo;
        private Curso _curso;
        private Empresa _empresa; 
        private Estados _estados;
        private EmpresaAnexo _empresaAnexo;
        private EmpresaContrato _empresaContrato;
        private EmpresaLayoutCracha _empresaLayoutCracha;
        private EmpresaSignatario _empresaSignatario;
        private EmpresaTipoAtividade _empresaTipoAtividade;
        private FormatoCredencial _formatoCredencial;
        private LayoutCracha _layoutCracha;
        private Municipio _municipio;
        private Pendencia _pendencia;
        private Status _status;
        private TecnologiaCredencial _tecnologiaCredencial;
        private TipoCobranca _tipoCobrancas;
        private TipoAcesso _tipoAcesso;
        private TipoAtividade _tipoAtividade;
        private TipoCredencial _tipoCredencial;
        private Veiculo _veiculo;
        private VeiculoAnexo _veiculoAnexo;
        private VeiculoCredencial _veiculoCredencial;
        private VeiculoEmpresa _veiculoEmpresa;
        private VeiculoSeguro _veiculoSeguro;

        #region  Metodos

        [TestInitialize]
        public void Inicializa()
        {
            #region AreaAcesso

            _areaAcesso = new AreaAcesso
            {
                Descricao = "Nova Área de Acesso",
                Identificacao = "AREA1"
            };
            #endregion

            #region Colaborador

            _colaborador =
                new Colaborador
                {
                    Nome = "Colaborador",
                    Apelido = "Marinpietri",
                    DataNascimento = DateTime.Today.AddYears(-26),
                    Cpf = "65024121490",
                    NomePai = "Valnei",
                    NomeMae = "Veronice",
                    Nacionalidade = "Brasil",
                    Foto = Convert.ToBase64String(File.ReadAllBytes("Arquivos/bean.png")),
                    EstadoCivil = "Solteiro",
                    Rg = "44.644.119-3",
                    RgEmissao = DateTime.Today.AddYears(-10),
                    RgOrgLocal = "SSP",
                    RgOrgUf = "BA",
                    Passaporte = "PJ8830202",
                    PassaporteValidade = DateTime.Today.AddYears(3),
                    Rne = "RN4493",
                    TelefoneFixo = "(71) 3581-4913",
                    TelefoneCelular = "(71) 98879-2442",
                    Email = "grupoestrela@grupoestrela.com.br",
                    ContatoEmergencia = "523696565",
                    TelefoneEmergencia = "56233363",
                    Cep = "41925-580",
                    Endereco = "Rua Baixa da Paz",
                    Numero = "947",
                    Complemento = "AP01",
                    Bairro = "Santa Cruz",
                    MunicipioId = 1,
                    EstadoId = 1,
                    Motorista = true,
                    CnhCategoria = "AB",
                    Cnh = "563232",
                    CnhValidade = DateTime.Now,
                    CnhEmissor = "DETRAN",
                    Cnhuf = "BA",
                    Bagagem = "Não",
                    DataEmissao = DateTime.Today.AddYears(-1),
                    DataValidade = DateTime.Today.AddYears(2),
                    Excluida = 0,
                    StatusId = 1,
                    TipoAcessoId = 1,
                    Pendente21 = false,
                    Pendente22 = false,
                    Pendente23 = false,
                    Pendente24 = false,
                    Pendente25 = false
                };

            #endregion

            #region Colaborador Anexo

            _colaboradorAnexo =
                new ColaboradorAnexo()
                {
                    NomeArquivo = "Anexo 1",
                    Arquivo = "Anex1.arq",
                    Descricao = "DESCRIÇÃO"
                };

            #endregion

            #region Colaborador Curso

            _colaboradorCurso =
                new ColaboradorCurso()
                {
                    NomeArquivo = "File 1",
                    Arquivo = "Arquivo1.arq",
                    Controlado = true
                };

            #endregion

            #region Colaborador Credencial

            _colaboradorCredencial =
                new ColaboradorCredencial
                {
                    NumeroCredencial = "00000000",
                    Fc = 101,
                    Emissao = DateTime.Now,
                    Validade = DateTime.Now,
                    CardHolderGuid = "000000",
                    CredencialGuid = "000000",
                    Ativa = true,
                    Colete = "0000",
                    Baixa = DateTime.Now,
                    Impressa = true
                };

            #endregion

            #region Colaborador Empresa

            _colaboradorEmpresa =
                new ColaboradorEmpresa
                {
                    Cargo = "CARGO",
                    Matricula = "MATRICULA",
                    Ativo = true
                };

            #endregion

            #region Credencial Motivo

            _credencialMotivo =
                new CredencialMotivo()
                {
                    Descricao = "Credencial Motivo 1"
                };

            #endregion

            #region Credencial Status

            _credencialStatus =
                new CredencialStatus()
                {
                    Descricao = "Credencial Status 1"
                };

            #endregion

            #region Curso
            _curso =
                new Curso()
                {
                    Descricao = "Curso 1"
                };

            #endregion

            #region Empresa

            _empresa = new Empresa
            {
                Nome = "Empresa Unit Test ",
                Apelido = "Apelido",
                Bairro = "Bairro",
                Celular1 = "71 988569541",
                Celular2 = "71 988569541",
                Cep = "40000",
                Complemento = "Complemento",
                Cnpj = "21877574000112",
                Email1 = "email1@email.com",
                Email2 = "email2@email.com",
                Contato1 = "contato1",
                Contato2 = "contato2",
                Endereco = "Endereco",
                Telefone1 = "30225487",
                Telefone2 = "30225487",
                Telefone = "719966652",
                Numero = "1",
                InsEst = "IE",
                InsMun = "IM",
                Obs = "Obs",
                Pendente11 = false,
                Pendente12 = true,
                Pendente13 = true,
                Pendente14 = false,
                Pendente15 = false,
                Pendente16 = false,
                Pendente17 = true,
                Logo = Convert.ToBase64String(File.ReadAllBytes("Arquivos/logo.png")),
                Excluida = 0,
                Responsavel = "Responsavel",
                Sigla = "Sigla",
                EstadoId = 1,
                MunicipioId = 1
            };

            #endregion

            #region Empresa Anexo

            _empresaAnexo = new EmpresaAnexo
            {
                Descricao = "Descricao",
                Anexo = Convert.ToBase64String(File.ReadAllBytes("Arquivos/contrato.pdf")),
                NomeAnexo = "Nome Anexo"
            };

            #endregion

            #region Empresa Contrato

            _empresaContrato = new EmpresaContrato
            {
                Arquivo = Convert.ToBase64String(File.ReadAllBytes("Arquivos/contrato.pdf")),
                Bairro = "Bairro",
                Cep = "40000",
                CelularResp = "719985623",
                Complemento = "Complemento",
                Descricao = "Descricao",
                Endereco = "Endereco",
                Contratante = "Contrato",
                IsencaoCobranca = false,
                EmailResp = "email@email",
                Emissao = DateTime.Now,
                Numero = "21",
                NomeResp = "Nome responsavel",
                NomeArquivo = "Nome arquivo",
                TelefoneResp = "71554212",
                NumeroContrato = "1124122",
                Terceirizada = false,
                EstadoId = 1,
                MunicipioId = 1,
                Validade = DateTime.Now.AddDays(10)
            };

            #endregion

            #region Layout Cracha

            _layoutCracha =
                new LayoutCracha()
                {
                    Nome = "Layout Cracha 1"
                };

            #endregion

            #region Empresa Layout Cracha

            _empresaLayoutCracha = new EmpresaLayoutCracha
            {
                LayoutCrachaId = _layoutCracha.LayoutCrachaId,
                EmpresaId = _empresa.EmpresaId
            };

            #endregion

            #region Empresa Signatário

            _empresaSignatario = new EmpresaSignatario
            {
                Nome = "Nome",
                Cpf = "277.854.55-10",
                Celular = "7188455562",
                Email = "email@email.com",
                Principal = true,
                Telefone = "719985665"
            };

            #endregion

            #region Empresa Tipo Atividade

            _empresaTipoAtividade = new EmpresaTipoAtividade
            {
                EmpresaId = _empresa.EmpresaId
            };

            #endregion

            #region Estados

            _estados =
                new Estados()
                {
                    Nome = "Bahia",
                    Uf = "BA",
                    EstadoId = 1
                };

            #endregion

            #region Formato Credencial

            _formatoCredencial =
                new FormatoCredencial()
                {
                    Descricao = "Formato de Credencial 1"
                };

            #endregion

            #region Municipios

            _municipio =
                new Municipio()
                {
                    Nome = "Salvador",
                    Uf = "BA",
                    MunicipioId = 1
                };

            #endregion

            #region Pendencia

            _pendencia = new Pendencia
            {
                Descricao = "Pendência 1",
                DataLimite = DateTime.Now,
                Impeditivo = false
            };


            #endregion

            #region Status

            _status =
                new Status()
                {
                    Descricao = "Status 1"
                };

            #endregion

            #region Tecnologia Credencial

            _tecnologiaCredencial =
                new TecnologiaCredencial()
                {
                    Descricao = "Método de Autenticação 1"
                };

            #endregion

            #region Tipo Acesso

            _tipoAcesso = new TipoAcesso
            {
                Descricao = "Novo Tipo de Acesso"
            };

            #endregion

            #region Tipo Atividade

            _tipoAtividade = new TipoAtividade
            {
                Descricao = "Novo Tipo de Atividade"
            };

            #endregion

            #region Tipos Cobranças

            _tipoCobrancas =
                new TipoCobranca()
                {
                    Descricao = "Novo Tipo de Cobrança"
                };

            #endregion

            #region Tipo Credencial

            _tipoCredencial =
                new TipoCredencial()
                {
                    Descricao = "Tipo Credencial 1"
                };

            #endregion

            #region Veiculo

            _veiculo = new Veiculo
            {
                Descricao = "Veiculo Unit Test ",
                EstadoId = 1,
                MunicipioId = 1,
                StatusId = 1,
                ArquivoAnexo = Convert.ToBase64String(File.ReadAllBytes("Arquivos/contrato.pdf")),
                Altura = "a",
                Ano = "2018",
                Cor = "Verde",
                Comprimento = "10",
                Foto = Convert.ToBase64String(File.ReadAllBytes("Arquivos/logo.png")),
                Excluida = 1,
                Frota = "Frota",
                Marca = "Marca",
                NomeArquivoAnexo = "Nome arquivo Anexo",
                Modelo = "Modelo A",
                Patrimonio = "Patrimonio",
                Renavam = "00025410",
                SerieChassi = "445445454",
                Largura = "25",
                PlacaIdentificador = "asd58455",
                Tipo = "Tipo",
                DescricaoAnexo = "Descricao Anexo",
                Pendente31 = true,
                Pendente32 = true,
                Pendente33 = false,
                Pendente34 = false
            };

            #endregion

            #region Veiculo Anexo

            _veiculoAnexo = new VeiculoAnexo
            {
                Descricao = "Descricao",
                Arquivo = Convert.ToBase64String(File.ReadAllBytes("Arquivos/contrato.pdf")),
                NomeArquivo = "Nome arquivo"
            };

            #endregion

            #region Veiculo Credencial

            _veiculoCredencial = new VeiculoCredencial
            {
                NumeroCredencial = "#####",
                Emissao = DateTime.Today,
                Validade = DateTime.Now,
                CardHolderGuid = "000000",
                CredencialGuid = "000000",
                Ativa = true,
                Fc = 111,
                Colete = "C1",
                Baixa = DateTime.Now,
                Impressa = false
            };


            #endregion

            #region Veiculo Empresa

            _veiculoEmpresa = new VeiculoEmpresa
            {
                Ativo = false,
                Cargo = "Personal Driver Car",
                Matricula = "102542"
            };

            #endregion

            #region Veiculo Seguro

            _veiculoSeguro = new VeiculoSeguro
            {
                NomeSeguradora = "Seguradora",
                NumeroApolice = "0001",
                ValorCobertura = 562.2m,
                Arquivo = Convert.ToBase64String(File.ReadAllBytes("Arquivos/contrato.pdf")),
                NomeArquivo = "Nome arquivo",
                Emissao = DateTime.Now.Date
            };

            #endregion

        }

        [Priority(3)]
        [TestMethod]
        [Description("Objetivo: Cadastrar/ler/alterar/remover dados de Colaboradores e seus relacionamentos")]
        public void Colaborador_Cadastro_geral_com_Sucesso()
        {
            #region Serviços

            var serviceAuxiliares = new DadosAuxiliaresFacadeService();
            var serviceColaborador = new ColaboradorService();
            var serviceCusrso = new CursoService();
            var serviceEmpresa = new EmpresaService();
            var serviceColaboradorCredencial = new ColaboradorCredencialService();

            #endregion

            #region Lista de CPFs

            var cpfArray = new[]
            {
                "64846162486",
                "32222216699",
                "21361852763",
                "44212987279",
                "15139389592",
                "73984076614",
                "02411705166",
                "71467846490",
                "60474202143",
                "15728331572",
                "94570447201",
                "12439592749",
                "17954615197",
                "73864171873",
                "27673022168",
                "11578341914",
                "88734745190",
                "63858065536",
                "63846216577",
                "61535105070",
                "41872772005",
                "93138702155",
                "96856687109",
                "74467731113",
                "95130557360",
                "11164314670",
                "38584211209",
                "39478667122",
                "31578551153"
            };

            #endregion

            #region CRUD Colaborador (Geral)

            //Cadastrar Colaboradores
            int k = 0;
            foreach (var item in cpfArray)
            {
                _colaborador.Cpf = item;
                _colaborador.Nome = $"Colaborador ({_colaborador.ColaboradorId})";

                //Já existindo colaborador, então remove e cria novo, evitando duplicidade de CPF
                var d0 = serviceColaborador.ObterPorCpf(_colaborador.Cpf);
                if (d0 != null)
                {
                    serviceColaborador.Remover(d0);
                    serviceColaborador.Criar(d0);
                    _colaborador = d0;
                }
                else
                {
                    serviceColaborador.Criar(_colaborador);
                }
                k++;
            }

            //Alterar 1 Colaborador
            var d1 = serviceColaborador.BuscarPelaChave(_colaborador.ColaboradorId);

            d1.Nome = "Colaborador UPDATED";
            d1.Foto = Convert.ToBase64String(File.ReadAllBytes("Arquivos/bean.png"));

            serviceColaborador.Alterar(d1);

            //Listar 1 Colaborador
            var firstColaborador = serviceColaborador.Listar().FirstOrDefault();

            var list1 = serviceColaborador.Listar(0, null, "%" + firstColaborador.Nome + "%").ToList();
            Assert.IsNotNull(list1);

            var b0 = serviceColaborador.BuscarPelaChave(firstColaborador.ColaboradorId);
            Assert.IsNotNull(b0);

            

            #endregion

            #region CRUD Empresas Vínculos (ColaboradoresEmpresas)

            //Cria nova empresa
            //Já existindo empresa, então nao cadastrar novamente, pois não é possivel haver 2 CNPJ iguais
            var dd0 = serviceEmpresa.BuscarEmpresaPorCnpj(_empresa.Cnpj);
            if (dd0 != null)
            {
                //Remove pendência de empresa (caso exista)
                var pendencia = serviceEmpresa.Pendencia.ListarPorEmpresa(dd0.EmpresaId).FirstOrDefault();
                if (pendencia != null)
                {
                    serviceEmpresa.Pendencia.Remover(pendencia);
                }
                //Remove empresa
                serviceEmpresa.Remover(dd0);
                serviceEmpresa.Criar(dd0);
                _empresa = dd0;
            }
            else
            {
                serviceEmpresa.Criar(_empresa);
            }

            //Cria 1 Contrato vinculado a empresa criada
            _empresaContrato.EmpresaId = _empresa.EmpresaId;
            serviceEmpresa.ContratoService.Criar(_empresaContrato);

            //Cadastrar 1 vínculos de empresa e contrato
            _colaboradorEmpresa.EmpresaId = _empresa.EmpresaId;
            _colaboradorEmpresa.EmpresaContratoId = _empresaContrato.EmpresaContratoId;
            _colaboradorEmpresa.ColaboradorId = _colaborador.ColaboradorId;
            serviceColaborador.Empresa.Criar(_colaboradorEmpresa);

            //Alterar 1 Empresas Vínculos
            var dd1 = serviceColaborador.Empresa.BuscarPelaChave(_colaboradorEmpresa.ColaboradorEmpresaId);
            dd1.Cargo = "Cargo #";
            dd1.Matricula = "Matrícula ###";
            dd1.Ativo = true;
            serviceColaborador.Empresa.Alterar(dd1);

            //Listar Empresas Vínculos 
            string FirstMatricula = serviceColaborador.Empresa.Listar().FirstOrDefault().Matricula;

            var list2 = serviceColaborador.Empresa.Listar(0, null, null, "%" + FirstMatricula + "%").ToList();
            Assert.IsNotNull(list2);

            int Id = serviceColaborador.Empresa.Listar().LastOrDefault().ColaboradorEmpresaId;

            var b1 = serviceColaborador.Empresa.BuscarPelaChave(Id);
            Assert.IsNotNull(b1); 

            #endregion

            #region CRUD Treinamentos e Certificações (ColaboradoresCursos) 

            //Lista um curso existente
            _curso = serviceCusrso.Listar().LastOrDefault();

            //Vincular Curso a Colaborador
            _colaboradorCurso.ColaboradorId = _colaborador.ColaboradorId;
            _colaboradorCurso.CursoId = _curso.CursoId;
            serviceColaborador.Curso.Criar(_colaboradorCurso);

            //Alterar 1 registros ColaboradorCurso
            int ccId = serviceColaborador.Curso.Listar().LastOrDefault().ColaboradorCursoId;
            var cc = serviceColaborador.Curso.BuscarPelaChave(ccId);

            cc.NomeArquivo = $"Nome Arquivo ({ccId})";
            cc.Arquivo = $"Arquivo.arq ({ccId})";
            cc.Controlado = false;

            serviceColaborador.Curso.Alterar(cc);

            //Listar ColaboradoresCursos
            string nomeArquivo = serviceColaborador.Curso.Listar().FirstOrDefault().NomeArquivo;
            var list3 = serviceColaborador.Curso.Listar(0, 0, "%" + nomeArquivo + "%", null, null).ToList();
            Assert.IsNotNull(list3);

            int cursoId = serviceColaborador.Curso.Listar().LastOrDefault().ColaboradorCursoId;
            var b2 = serviceColaborador.Curso.BuscarPelaChave(cursoId);
            Assert.IsNotNull(b2); 

            #endregion

            #region CRUD Anexos

            //Cadastrar 1 Anexo
            _colaboradorAnexo.ColaboradorId = _colaborador.ColaboradorId;
            serviceColaborador.Anexo.Criar(_colaboradorAnexo);

            //Alterar 1 Anexo
            int lastId = serviceColaborador.Anexo.Listar().LastOrDefault().ColaboradorAnexoId;
            var dd2 = serviceColaborador.Anexo.BuscarPelaChave(lastId);
            dd2.NomeArquivo = "Anexo ##";
            dd2.Descricao = "Descrição ###";

            serviceColaborador.Anexo.Alterar(dd2);

            //Listar Anexos 
            string FirstDesc = serviceColaborador.Anexo.Listar().FirstOrDefault().NomeArquivo;

            var list4 = serviceColaborador.Anexo.Listar(0, "%" + FirstDesc + "%").ToList();
            Assert.IsNotNull(list4);

            int key = serviceColaborador.Anexo.Listar().LastOrDefault().ColaboradorAnexoId;

            var b3 = serviceColaborador.Anexo.BuscarPelaChave(key);
            Assert.IsNotNull(b3); 

            #endregion

            #region CRUD Credenciais (Colaboradores)

            #region Tabelas Auxiliares

            //Lista uma Área de Acesso existente
            _areaAcesso = serviceAuxiliares.AreaAcessoService.Listar().LastOrDefault();

            //Lista um Tipo de Acesso existente
            _tipoAcesso = serviceAuxiliares.TiposAcessoService.Listar().FirstOrDefault();

            //Lista uma Tipo de Cobrança existente
            _tipoCobrancas = serviceAuxiliares.TipoCobrancaService.Listar().LastOrDefault();

            //Lista um Status existente
            _status = serviceAuxiliares.StatusService.Listar().LastOrDefault();

            //Lista um Tecnologia de Credencial existente
            _tecnologiaCredencial = serviceAuxiliares.TecnologiaCredencialService.Listar().LastOrDefault();

            //Lista um Tipo de Credencial existente
            _tipoCredencial = serviceAuxiliares.TipoCredencialService.Listar().LastOrDefault();

            //Lista um Layout de Crachá existente 
            _layoutCracha = serviceAuxiliares.LayoutCrachaService.Listar().LastOrDefault();

            //Lista um Formato de Credencial existente 
            _formatoCredencial = serviceAuxiliares.FormatoCredencialService.Listar().LastOrDefault();

            //Lista um Status de Credencial existente 
            _credencialStatus = serviceAuxiliares.CredencialStatusService.Listar().FirstOrDefault();

            //Lista um Motivo de Credencial existente
            _credencialMotivo = serviceAuxiliares.CredencialMotivoService.Listar().FirstOrDefault();

            #endregion

            #region CRUD de Credencial (ColaboradoresCredenciais)

            //Cria 1 registro em EmpresasContratos
            _empresaContrato.EmpresaId = _empresa.EmpresaId;
            _empresaContrato.TipoCobrancaId = _tipoCobrancas.TipoCobrancaId;
            _empresaContrato.EstadoId = _estados.EstadoId;
            _empresaContrato.MunicipioId = _municipio.MunicipioId;
            _empresaContrato.StatusId = _status.StatusId;
            _empresaContrato.TipoAcessoId = _tipoAcesso.TipoAcessoId;

            serviceEmpresa.ContratoService.Criar(_empresaContrato);

            //Cria 1 registro em ColaboradoresEmpresas
            _colaboradorEmpresa.ColaboradorId = _colaborador.ColaboradorId;
            _colaboradorEmpresa.EmpresaId = _empresa.EmpresaId;
            _colaboradorEmpresa.EmpresaContratoId = _empresaContrato.EmpresaContratoId;

            serviceColaborador.Empresa.Criar(_colaboradorEmpresa);

            //Cria 1 credencial (ColaboradoresCredenciais)
            _colaboradorCredencial.ColaboradorEmpresaId = _colaboradorEmpresa.ColaboradorEmpresaId;
            _colaboradorCredencial.TecnologiaCredencialId = _tecnologiaCredencial.TecnologiaCredencialId;
            _colaboradorCredencial.TipoCredencialId = _tipoCredencial.TipoCredencialId;
            _colaboradorCredencial.LayoutCrachaId = _layoutCracha.LayoutCrachaId;
            _colaboradorCredencial.FormatoCredencialId = _formatoCredencial.FormatoCredencialId;
            _colaboradorCredencial.CredencialStatusId = _credencialStatus.CredencialStatusId;
            _colaboradorCredencial.CredencialStatusId = _credencialStatus.CredencialStatusId;
            _colaboradorCredencial.ColaboradorPrivilegio1Id = _areaAcesso.AreaAcessoId;
            _colaboradorCredencial.ColaboradorPrivilegio2Id = _areaAcesso.AreaAcessoId;
            _colaboradorCredencial.CredencialMotivoId = _credencialMotivo.CredencialMotivoId;

            serviceColaboradorCredencial.Criar(_colaboradorCredencial);

            //Altera 1 credencial
            _colaboradorCredencial.Ativa = true;
            _colaboradorCredencial.Validade = DateTime.Today.AddYears(1);
            serviceColaboradorCredencial.Alterar(_colaboradorCredencial);
            var d52 = serviceColaboradorCredencial.BuscarPelaChave(_colaboradorCredencial.ColaboradorCredencialId);
            Assert.IsNotNull(d52);

            #endregion


            #endregion

        }

        [Priority(2)]
        [TestMethod]
        [Description("Objetivo: Cadastrar/ler/alterar/remover dados de Veículos e seus relacionamentos")]
        public void Veiculo_Cadastro_geral_com_Sucesso()
        {
            #region Serviços

            var serviceVeiculo = new VeiculoService();
            var serviceEmpresa = new EmpresaService();
            var serviceVeiculoCredencial = new VeiculoCredencialService();
            var serviceAuxiliares = new DadosAuxiliaresFacadeService();

            #endregion

            #region CRUD Veículo (Aba Geral) 

            //Cadastrar 1 Veículo
            _veiculo.Descricao = "Veículo ##";
            _veiculo.Ano = "200#";
            _veiculo.Altura = "111m";
            _veiculo.CombustivelId = 1;
            _veiculo.StatusId = 1;
            _veiculo.TipoAcessoId = 1;


            serviceVeiculo.Criar(_veiculo);

            //Alterar 1 Veículo
            var d1 = serviceVeiculo.BuscarPelaChave(_veiculo.EquipamentoVeiculoId);
            d1.Descricao = $"Veículo ###";
            d1.Ano = DateTime.Now.AddYears(1).ToString();
            d1.Foto = Convert.ToBase64String(File.ReadAllBytes("Arquivos/car-equip.jpg"));

            serviceVeiculo.Alterar(d1);

            //Listar Veículos
            var firstVeiculo = serviceVeiculo.Listar().FirstOrDefault();

            var list1 = serviceVeiculo.Listar("%" + firstVeiculo.Descricao + "%", null).ToList();
            Assert.IsNotNull(list1);
            var list11 = serviceVeiculo.BuscarPelaChave(firstVeiculo.EquipamentoVeiculoId);
            Assert.IsNotNull(list11); 

            #endregion

            #region CRUD Empresas Vínculos (VeiculosEmpresas)

            //Cadastrar 1 Empresa
            //Já existindo Empresa, então remove e cria novo, evitando duplicidade de CNPJ
            var dd1 = serviceEmpresa.BuscarEmpresaPorCnpj(_empresa.Cnpj);
            if (dd1 != null)
            {
                //Remove vínculo VeiculoEmpresa (caso exista)
                var ddd1 = serviceEmpresa.VeiculoService.Listar().FirstOrDefault(n => n.EmpresaId == _empresa.EmpresaId);
                if (ddd1 != null)
                {
                    serviceVeiculo.Empresa.Remover(ddd1);
                }
                serviceEmpresa.Remover(dd1);
                serviceEmpresa.Criar(dd1);
                _empresa = dd1;
            }
            else
            {
                serviceEmpresa.Criar(_empresa);
            }

            //Cadastrar 1 Empresa Contrato
            _empresaContrato.EmpresaId = _empresa.EmpresaId;
            serviceEmpresa.ContratoService.Criar(_empresaContrato);

            //Cadastrar 1 Empresas Vínculos
            _veiculoEmpresa.VeiculoId = _veiculo.EquipamentoVeiculoId;
            _veiculoEmpresa.EmpresaId = _empresa.EmpresaId;
            _veiculoEmpresa.EmpresaContratoId = _empresaContrato.EmpresaContratoId;
            serviceVeiculo.Empresa.Criar(_veiculoEmpresa);

            //Alterar 1 Empresas Vínculos
            var dd0 = serviceVeiculo.Empresa.BuscarPelaChave(_veiculoEmpresa.VeiculoEmpresaId);

            dd0.Cargo = "Personal Driver Car ###";
            dd0.Matricula = "####";
            dd0.Ativo = true;

            serviceVeiculo.Empresa.Alterar(dd0);

            //Listar Empresas Vínculos 
            string FirstMatricula = serviceVeiculo.Empresa.Listar().FirstOrDefault().Matricula;

            var list2 = serviceVeiculo.Empresa.Listar(0, null, "%" + FirstMatricula + "%").ToList();
            Assert.IsNotNull(list2);

            int Id = serviceVeiculo.Empresa.Listar().LastOrDefault().VeiculoEmpresaId;

            var b1 = serviceVeiculo.Empresa.BuscarPelaChave(Id);
            Assert.IsNotNull(b1); 

            #endregion

            #region CRUD Seguros (VeiculosSeguros) 

            //Cadastrar e Vincular Seguro a Veiculo
            _veiculoSeguro.NomeSeguradora = "Seguradora ###";
            _veiculoSeguro.VeiculoId = _veiculo.EquipamentoVeiculoId;
            serviceVeiculo.Seguro.Criar(_veiculoSeguro);

            //Alterar 1 registro VeiculosSeguros

            int ccId = serviceVeiculo.Seguro.Listar().LastOrDefault().VeiculoSeguroId;
            var cc = serviceVeiculo.Seguro.BuscarPelaChave(ccId);

            cc.NomeSeguradora = $"Nova Seguradora ({ccId})";
            cc.NumeroApolice = $"New Arquivo {ccId}";
            cc.Validade = DateTime.Today;

            serviceVeiculo.Seguro.Alterar(cc);

            //Listar VeiculosSeguros
            string nomeSeguradora = serviceVeiculo.Seguro.Listar().FirstOrDefault().NomeSeguradora;
            var list3 = serviceVeiculo.Seguro.Listar(0, "%" + nomeSeguradora + "%", null).ToList();
            Assert.IsNotNull(list3);

            int cursoId = serviceVeiculo.Seguro.Listar().LastOrDefault().VeiculoSeguroId;
            var b2 = serviceVeiculo.Seguro.BuscarPelaChave(cursoId);
            Assert.IsNotNull(b2);
             

            #endregion

            #region CRUD Anexos

            //Cadastrar Anexos
            _veiculoAnexo.VeiculoId = _veiculo.EquipamentoVeiculoId;
            serviceVeiculo.Anexo.Criar(_veiculoAnexo);

            //Alterar Anexo
            var d0 = serviceVeiculo.Anexo.BuscarPelaChave(_veiculoAnexo.VeiculoAnexoId);
            d0.NomeArquivo = "Anexo ##";
            d0.Descricao = "Descrição ###";
            serviceVeiculo.Anexo.Alterar(d0);

            //Listar Anexos
            string FirstDesc = serviceVeiculo.Anexo.Listar().FirstOrDefault().Descricao;

            var list4 = serviceVeiculo.Anexo.Listar(0, "%" + FirstDesc + "%").ToList();
            Assert.IsNotNull(list4);

            int key = serviceVeiculo.Anexo.Listar().LastOrDefault().VeiculoAnexoId;

            var b3 = serviceVeiculo.Anexo.BuscarPelaChave(key);
            Assert.IsNotNull(b3);
             

            #endregion

            #region CRUD Credenciais (Veículos)

            #region Tabelas Auxiliares

            //Lista 1 Área de Acesso existente
            _areaAcesso = serviceAuxiliares.AreaAcessoService.Listar().FirstOrDefault();

            //Lista 1 Tipo de Acesso existente
            _tipoCobrancas = serviceAuxiliares.TipoCobrancaService.Listar().FirstOrDefault();

            //Lista 1 Status existente
            _status = serviceAuxiliares.StatusService.Listar().FirstOrDefault();

            //Lista 1 Tipo de Acesso existente
            _tipoAcesso = serviceAuxiliares.TiposAcessoService.Listar().FirstOrDefault();

            //Lista 1 Tecnologia de Credencial
            _tecnologiaCredencial = serviceAuxiliares.TecnologiaCredencialService.Listar().FirstOrDefault();

            //Lista 1 Tipo de Credencial
            _tipoCredencial = serviceAuxiliares.TipoCredencialService.Listar().FirstOrDefault();

            //Lista 1 Layout de Crachá
            _layoutCracha = serviceAuxiliares.LayoutCrachaService.Listar().FirstOrDefault();

            //Lista 1 Formato de Credencial
            _formatoCredencial = serviceAuxiliares.FormatoCredencialService.Listar().FirstOrDefault();

            //Lista 1 Status de Credencial
            _credencialStatus = serviceAuxiliares.CredencialStatusService.Listar().FirstOrDefault();

            //Lista 1 Motivo de Credencial 
            _credencialMotivo = serviceAuxiliares.CredencialMotivoService.Listar().FirstOrDefault();

            #endregion

            #region CRUD de Credencial (VeiculosCredenciais)

            //Atualiza 1 Empresa
            int idStr = serviceEmpresa.Listar().LastOrDefault().EmpresaId + 1;
            _empresa.Nome = "EMPRESA #" + idStr  + "#";
            serviceEmpresa.Alterar(_empresa);


            //Atualiza 1 EmpresaContrato
            _empresaContrato.EmpresaId = _empresa.EmpresaId;
            _empresaContrato.TipoCobrancaId = _tipoCobrancas.TipoCobrancaId;
            _empresaContrato.EstadoId = _estados.EstadoId;
            _empresaContrato.MunicipioId = _municipio.MunicipioId;
            _empresaContrato.StatusId = _status.StatusId;
            _empresaContrato.TipoAcessoId = _tipoAcesso.TipoAcessoId;

            serviceEmpresa.ContratoService.Alterar(_empresaContrato);

            //Atualiza 1 VeiculoEmpresa
            _veiculoEmpresa.VeiculoId = _veiculo.EquipamentoVeiculoId;
            _veiculoEmpresa.EmpresaId = _empresa.EmpresaId;
            _veiculoEmpresa.EmpresaContratoId = _empresaContrato.EmpresaContratoId;

            serviceVeiculo.Empresa.Alterar(_veiculoEmpresa);

            //Cadastrar 1 Credencial de Veículo (ColaboradorCredencial)
            _veiculoCredencial.VeiculoEmpresaId = _veiculoEmpresa.VeiculoEmpresaId;
            _veiculoCredencial.TecnologiaCredencialId = _tecnologiaCredencial.TecnologiaCredencialId;
            _veiculoCredencial.TipoCredencialId = _tipoCredencial.TipoCredencialId;
            _veiculoCredencial.LayoutCrachaId = _layoutCracha.LayoutCrachaId;
            _veiculoCredencial.FormatoCredencialId = _formatoCredencial.FormatoCredencialId;
            _veiculoCredencial.CredencialStatusId = _credencialStatus.CredencialStatusId;
            _veiculoCredencial.CredencialStatusId = _credencialStatus.CredencialStatusId;
            _veiculoCredencial.VeiculoPrivilegio1Id = _areaAcesso.AreaAcessoId;
            _veiculoCredencial.VeiculoPrivilegio2Id = _areaAcesso.AreaAcessoId;
            _veiculoCredencial.CredencialMotivoId = _credencialMotivo.CredencialMotivoId;

            serviceVeiculoCredencial.Criar(_veiculoCredencial);

            //Alterar 1 Credencial de Veículo (ColaboradorCredencial)
            _colaboradorCredencial.Ativa = true;
            _colaboradorCredencial.Validade = DateTime.Today.AddYears(1);

            serviceVeiculoCredencial.Alterar(_veiculoCredencial);


            //Listar 1 Credencial de Veículo (ColaboradorCredencial)
            var ccList = serviceVeiculoCredencial.Listar().FirstOrDefault();
            Assert.IsNotNull(ccList); 

            #endregion

            #endregion
        }

        [Priority(1)]
        [TestMethod]
        [Description("Objetivo cadastrar dados de empresa e seus relacionamentos")]
        public void Empresa_Cadastro_geral_com_Sucesso()
        {
            #region Serviços

            var empresaService = new EmpresaService();
            var serviceAuxiliar = new DadosAuxiliaresFacadeService();

            #endregion

            #region Lista de CNPJs

            var cnpjArray = new[]
            {
                "75676952000129",
                "44162050000140",
                "67275285000144",
                "75248517000101",
                "85579598000178",
                "16493080000102",
                "23490647000144",
                "14755140000184",
                "52241603000189",
                "60204850000133",
                "25156508000104",
                "30352250000151",
                "20785569000117",
                "68843837000136",
                "16010543000120",
                "04963765000188",
                "94198143000101",
                "26969709000193",
                "26969709000193",
                "82535365000185",
                "40307948000109",
                "96227160000147",
                "52419723000123",
                "07715262000181",
                "17047555000191",
                "66068275000175",
                "02456590000197",
                "30444996000195",
                "65911683000184"
            };

            #endregion

            #region Cadastro de Empresa

            //Cadastrar Empresas
            int k = 0;
            foreach (var item in cnpjArray)
            {
                _empresa.Cnpj = item;
                _empresa.Nome = $"Empresa ({_empresa.EmpresaId})";

                //Já existindo empresa, então nao cadastrar novamente, pois não é possivel haver 2 CNPJ iguais
                var d0 = empresaService.BuscarEmpresaPorCnpj(_empresa.Cnpj);

                if (d0 != null)
                {
                    //Remove pendência de empresa (caso exista)
                    var pendencia = empresaService.Pendencia.ListarPorEmpresa(d0.EmpresaId).FirstOrDefault();
                    if (pendencia != null)
                    {
                        empresaService.Pendencia.Remover(pendencia);
                    }
                    //Remove empresa
                    empresaService.Remover(d0);
                    empresaService.Criar(d0);
                    _empresa = d0;
                }
                else
                {
                    empresaService.Criar(_empresa);
                }
                k++;
            }

            //Cria TipoAtividade
            serviceAuxiliar.TipoAtividadeService.Criar(_tipoAtividade);
            _empresaTipoAtividade.TipoAtividadeId = _tipoAtividade.TipoAtividadeId;
            _empresaTipoAtividade.EmpresaId = _empresa.EmpresaId;

            //Cria EmpresaTipoAtividade
            empresaService.Atividade.Criar(_empresaTipoAtividade);

            var aux = new DadosAuxiliaresFacadeService();

            aux.LayoutCrachaService.Criar(_layoutCracha);
            _empresaLayoutCracha.LayoutCrachaId = _layoutCracha.LayoutCrachaId;
            _empresaLayoutCracha.EmpresaId = _empresa.EmpresaId;

            //Cria EmpresaLayoutCracha
            empresaService.CrachaService.Criar(_empresaLayoutCracha);

            #endregion

            #region Cadastro de Signatários

            //Cadastrar 1 Signatario
            _empresaSignatario.EmpresaId = _empresa.EmpresaId;
            empresaService.SignatarioService.Criar(_empresaSignatario);

            #endregion

            #region Anexos

            //Cadastrar 1 anexo
            _empresaAnexo.EmpresaId = _empresa.EmpresaId;
            empresaService.AnexoService.Criar(_empresaAnexo);

            #endregion

            #region Cadastrar Pendencias

            empresaService.Pendencia.Criar(new Pendencia
            {
                EmpresaId = _empresa.EmpresaId,
                Descricao = "At vero eos et accusamus et iusto odio dignissimos ducimus " +
                            "qui blanditiis praesentium voluptatum deleniti atque corrupti " +
                            "quos dolores et quas molestias excepturi sint occaecati cupiditate" +
                            " non provident, similique sunt in culpa qui officia deserunt" +
                            " mollitia animi, id est laborum et dolorum fuga.",
                DataLimite = DateTime.Now.Date,
                Impeditivo = true,
                CodPendencia = 24
            });

            #endregion

            #region Listar Pendencias

            var listP = empresaService.ListarPendencias(_empresa.EmpresaId).FirstOrDefault();
            if (listP == null) throw new Exception("Uma pendência foi cadastrada, mas não foi possível retorna informção");
            var pend = listP.Pendencias;
            Assert.IsTrue(!string.IsNullOrWhiteSpace(pend));

            #endregion

            #region Empresa Area Acesso

            //Lista 1 Área de Acesso existente
            _areaAcesso = serviceAuxiliar.AreaAcessoService.Listar().FirstOrDefault();

            //Cria 1 Empresa Area Acesso 
            var empresaAreaAcesso = new EmpresaAreaAcesso
            {
                AreaAcessoId = _areaAcesso.AreaAcessoId,
                EmpresaId = _empresa.EmpresaId
            };
            empresaService.AreaAcessoService.Criar(empresaAreaAcesso);

            //Altera Empresa Area Acesso
            var d1 = empresaService.AreaAcessoService.BuscarPelaChave(empresaAreaAcesso.EmpresaAreaAcessoId);
            empresaService.AreaAcessoService.Alterar(d1);
            var list = empresaService.AreaAcessoService.Listar().LastOrDefault();
            Assert.IsNotNull(list); 

            #endregion

            #region Empresa Layout Crachá

            //Lista 1 Layout Crachá existente
            _layoutCracha = serviceAuxiliar.LayoutCrachaService.Listar().FirstOrDefault();

            //Cria 1 Empresa Layout Crachá
            _empresaLayoutCracha.LayoutCrachaId = _layoutCracha.LayoutCrachaId;
            _empresaLayoutCracha.EmpresaId = _empresa.EmpresaId;
            empresaService.CrachaService.Criar(_empresaLayoutCracha);

            //Altera Empresa Layout Crachá
            var d4 = empresaService.CrachaService.BuscarPelaChave(_empresaLayoutCracha.EmpresaLayoutCrachaId);
            empresaService.CrachaService.Alterar(d4);
            var last = empresaService.CrachaService.Listar().LastOrDefault();
            Assert.IsNotNull(last);  


            #endregion

            #region Empresa Contrato

            //Lista 1 Tipo de Cobrança existente
            _tipoCobrancas = serviceAuxiliar.TipoCobrancaService.Listar().FirstOrDefault();

            //Lista 1 Status existente
            _status = serviceAuxiliar.StatusService.Listar().FirstOrDefault();

            //Lista 1 Tipo de Acesso existente
            _tipoAcesso = serviceAuxiliar.TiposAcessoService.Listar().FirstOrDefault();

            //Cadastrar 1 Contrato
            _empresaContrato.EmpresaId = _empresa.EmpresaId;
            empresaService.ContratoService.Criar(_empresaContrato);

            var n1 = empresaService.ContratoService.BuscarPelaChave(_empresaContrato.EmpresaContratoId);
            Assert.IsNotNull(n1);
            var l1 = empresaService.ContratoService.ListarPorEmpresa(_empresa.EmpresaId);
            Assert.IsNotNull(l1);
            var l2 = empresaService.ContratoService.ListarPorDescricao(_empresaContrato.Descricao);
            Assert.IsNotNull(l2);
            var l3 = empresaService.ContratoService.ListarPorNumeroContrato(_empresaContrato.NumeroContrato);
            Assert.IsNotNull(l3);

            //Atualiza Empresa Contrato
            _empresaContrato.TipoCobrancaId = _tipoCobrancas.TipoCobrancaId;
            _empresaContrato.StatusId = _status.StatusId;
            _empresaContrato.TipoAcessoId = _tipoAcesso.TipoAcessoId;
            _empresaContrato.EmpresaId = _empresa.EmpresaId;
            empresaService.ContratoService.Alterar(_empresaContrato);

            var list1 = empresaService.ContratoService.Listar().LastOrDefault();
            Assert.IsNotNull(list1); 

            #endregion

        }


        [TestMethod]
        [Description("Objetivo: Buscar dados de última Credencial de Colaborador criada")]
        public void ColabororadorCredencial_Buscar_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();

            var d1 = repositorio.Listar().LastOrDefault();

            if (d1 == null)
            {
                repositorio.Criar(_colaboradorCredencial);
            }

            var d2 = repositorio.BuscarPelaChave(_colaboradorCredencial.ColaboradorCredencialId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        [Description("Objetivo: Listar dados de Credencial de  Colaborador, usando filtros")]
        public void ColabororadorCredencial_ListarColaboradorCredeniasView_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();

            var ccid = repositorio.Listar().FirstOrDefault().ColaboradorCredencialId;
            if (ccid == 0)
            {
                repositorio.Criar(_colaboradorCredencial);
            }

            var csid = repositorio.Listar().FirstOrDefault().CredencialStatusId;
            if (csid == 0)
            {
                repositorio.Criar(_colaboradorCredencial);
            }

            var fcid = repositorio.Listar().FirstOrDefault().FormatoCredencialId;
            if (fcid == 0)
            {
                repositorio.Criar(_colaboradorCredencial);
            }

            //ColaboradorCredencialID
            var d2 = repositorio.Listar(ccid, 0, 0);
            //CredencialStatusID
            var d3 = repositorio.Listar(0, csid, 0);
            //FormatoCredencialID
            var d4 = repositorio.Listar(0, 0, fcid);

            Assert.IsNotNull(d2);
            Assert.IsNotNull(d3);
            Assert.IsNotNull(d4);
        }

        [TestMethod]
        [Description("Objetivo: Criar e listar dados de impressões de credenciais de colaboradores")]
        public void ColaboradorCredencialimpresssao_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialimpresssaoRepositorio();
            for (var i = 0; i < 6; i++)
            {
                var d1 = new ColaboradorCredencialimpresssao
                {
                    Cobrar = true,
                    ColaboradorCredencialId = 1 + i,
                    DataImpressao = DateTime.Now
                };
                repositorio.Criar(d1);

                d1.Cobrar = false;
                repositorio.Alterar(d1);
            }
            var list = repositorio.Listar();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        [Description("Objetivo: Criar e listar dados de Colaboradores e Cursos")]
        public void ColaboradorCursos_com_sucesso()
        {
            var colaboradorCursoRepositorio = new ColaboradorCursoRepositorio();
            var cursoRepositorio = new CursoRepositorio();
            var colaboradorRepositorio = new ColaboradorRepositorio(); 

            //Criar Curso
            cursoRepositorio.Criar(_curso);

            //Já existindo colaborador, então remove e cria novo, evitando duplicidade de CPF
            var d0 = colaboradorRepositorio.ObterPorCpf(_colaborador.Cpf);
            if (d0 != null)
            {
                colaboradorRepositorio.Remover(d0);
                colaboradorRepositorio.Criar(d0);
                _colaborador = d0;
            }
            else
            {
                colaboradorRepositorio.Criar(_colaborador);
            }

            for (var i = 0; i < 5; i++)
            {
                _colaboradorCurso.CursoId = _curso.CursoId;
                _colaboradorCurso.ColaboradorId = _colaborador.ColaboradorId;
                colaboradorCursoRepositorio.Criar(_colaboradorCurso);

                _colaboradorCurso.NomeArquivo = "File #" + i;
                _colaboradorCurso.Arquivo = "Arquivo" + i + " alterado!";
                colaboradorCursoRepositorio.Alterar(_colaboradorCurso);
            }

            string filtro = colaboradorCursoRepositorio.Listar().LastOrDefault().NomeArquivo;

            var list0 = colaboradorCursoRepositorio.Listar(0, 0, filtro, null, 0).ToList();
            Assert.IsNotNull(list0);
            var list1 = colaboradorCursoRepositorio.Listar();
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        [Description("Objetivo: Listar Credenciais Status, incluindo filtro")]
        public void CredencialStatusRepositorio_com_sucesso()
        {
            var repositorio = new CredencialStatusRepositorio();

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            var list = repositorio.Listar();
            Assert.IsNotNull(list);
            var list1 = repositorio.Listar(0, filtro).ToList();
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        [Description("Objetivo: Criar e Listar Cursos, incluindo filtro")]
        public void CursoRepositorio_com_sucesso()
        {
            var repositorio = new CursoRepositorio();
            for (var i = 0; i < 6; i++)
            {
                var d1 = new Curso
                {
                    CursoId = i,
                    Descricao = "Descrição curso " + i
                };
                repositorio.Criar(d1);

                d1.Descricao = "Descrição curso " + i + " alterado";
                repositorio.Alterar(d1);
            }

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            var list0 = repositorio.Listar();
            Assert.IsNotNull(list0);
            var list1 = repositorio.Listar(0, filtro).ToList();
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        [Description("Objetivo: Criar e Listar Layout Crachá, incluindo filtro")]
        public void LayoutCrachaRepositorio_com_sucesso()
        {
            var repositorio = new LayoutCrachaRepositorio();

            var list0 = repositorio.Listar();
            if (list0 == null)
            {
                repositorio.Criar(_layoutCracha);
                list0 = repositorio.Listar();
            }
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Nome;

            var list1 = repositorio.Listar(filtro, null);
            if (list1 == null)
            {
                repositorio.Criar(_layoutCracha);
                list1 = repositorio.Listar(filtro, null).ToList();
            }
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        [Description("Objetivo: Criar e Listar e Remover registro de pendência, incluindo filtro")]
        public void PendenciaRepositorio_com_sucesso()
        {
            var repositorio = new PendenciaRepositorio();

            var empresaRepositorio = new EmpresaRepositorio();


            var emp = empresaRepositorio.Listar().LastOrDefault();

            if (emp == null)
            {
                empresaRepositorio.Criar(_empresa);
            }

            _pendencia.EmpresaId = emp.EmpresaId;
            _pendencia.CodPendencia = 12;
            _pendencia.Descricao = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                                   "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                                   "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip";

            for (int i = 0; i < 2; i++)
            {
                repositorio.Criar(_pendencia);
            }


            var list0 = repositorio.Listar();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            var list1 = repositorio.Listar(filtro, null, null).ToList();
            Assert.IsNotNull(list1);

            repositorio.Remover(_pendencia);
        }

        [TestMethod]
        [Description("Objetivo: Criar e Listar registro de Colaborador, incluindo filtro")]
        public void Colabororador_Criar_Listar_com_sucesso()
        {
            var service = new ColaboradorService();

            #region Cadastrar Colaborador

            _colaborador.Cpf = "483.578.550-91";
            _colaborador.Nome = "Colaborador Nome";
            var d1 = service.ObterPorCpf(_colaborador.Cpf);
            if (d1 == null)
            {
                service.Criar(_colaborador);
            }

            #endregion

            var l1 = service.Listar(); //Listar todos
            Assert.IsNotNull(l1);

            string filtro = service.Listar().LastOrDefault().Nome;

            var l2 = service.Listar(null, null, filtro); //Listar por nome
            Assert.IsNotNull(l2);
        }

        [TestMethod]
        [Description("Objetivo: Criar registro de Colaborador Credencial")]
        public void ColabororadorCredencial_Cadastrar_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();

            var colaborador = new ColaboradorCredencial
            {
                Ativa = true,
                Baixa = DateTime.Now,
                CardHolderGuid = "000000",
                ColaboradorEmpresaId = 1,
                ColaboradorPrivilegio1Id = 0,
                ColaboradorPrivilegio2Id = 0,
                Colete = "Colete",
                CredencialGuid = "Credencial Guid",
                CredencialMotivoId = 1,
                Emissao = DateTime.Now,
                Fc = 0,
                Impressa = false,
                NumeroCredencial = "Numero Credencial",
                TipoCredencialId = 1,
                TecnologiaCredencialId = 2,
                CredencialStatusId = 1,
                FormatoCredencialId = 1,
                LayoutCrachaId = 2
            };

            repositorio.Criar(colaborador);
        }

        [TestMethod]
        [Description("Objetivo: Buscar filtrando por CPF e Criar registro de Colaborador, caso não exista")]
        public void Colabororador_Cadastrar_com_sucesso()
        {
            #region Lista de CPFs

            var cpfArray = new[]
            {
                "64846162486",
                "32222216699",
                "21361852763",
                "44212987279",
                "15139389592",
                "73984076614",
                "02411705166",
                "71467846490",
                "60474202143",
                "15728331572",
                "94570447201",
                "12439592749",
                "17954615197",
                "73864171873",
                "27673022168",
                "11578341914",
                "88734745190",
                "63858065536",
                "63846216577",
                "61535105070",
                "41872772005",
                "93138702155",
                "96856687109",
                "74467731113",
                "95130557360",
                "11164314670",
                "38584211209",
                "39478667122",
                "31578551153"
            };

            #endregion

            var service = new ColaboradorService();

            int i = 0;
            foreach (var item in cpfArray)
            {
                _colaborador.Cpf = item;
                _colaborador.Nome = $"Colaborador ({_colaborador.ColaboradorId})";

                //Já existindo colaborador, então remove e cria novo, evitando duplicidade de CPF
                var d1 = service.ObterPorCpf(_colaborador.Cpf);
                if (d1 != null)
                {
                    service.Remover(d1);
                    service.Criar(d1);
                    _colaborador = d1;
                }
                else
                {
                    service.Criar(_colaborador);
                }
                i++;
            }
        }

        [TestMethod]
        [Description("Objetivo: Criar registro de Colaborador Credencial")]
        public void VeiculoAnexo_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var service = new VeiculoService();

            _veiculo.CombustivelId = 1;
            _veiculo.StatusId = 1;
            _veiculo.TipoAcessoId = 1;
            service.Criar(_veiculo);

            _veiculoAnexo.VeiculoId = _veiculo.EquipamentoVeiculoId;

            for (var i = 0; i < 5; i++)
            {
                //criar
                service.Anexo.Criar(_veiculoAnexo);
                //update
                _veiculoAnexo.Descricao = "Descricao Alterada";
                service.Anexo.Alterar(_veiculoAnexo);
            }

            var list0 = service.Anexo.Listar().ToList();
            Assert.IsNotNull(list0);

            var b1 = service.Anexo.BuscarPelaChave(_veiculoAnexo.VeiculoAnexoId);
            Assert.IsNotNull(b1);

            var b2 = service.Anexo.Listar(0, "%" + _veiculoAnexo.Descricao + "%");
            Assert.IsNotNull(b2);

            //Remover
            service.Anexo.Remover(b1);
            var b4 = service.Anexo.BuscarPelaChave(b1.VeiculoAnexoId);
            Assert.IsNull(b4);
        }

        [TestMethod]
        [Description("Objetivo: Criar e listar dados de impressões de credenciais de veículos")]
        public void VeiculoCredencialimpressao_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoCredencialimpressaoRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new VeiculoCredencialimpressao
                {
                    VeiculoCredencialId = i + 100,
                    DataImpressao = DateTime.Now,
                    Cobrar = true
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.DataImpressao = DateTime.Today.AddDays(i + 1);
                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(1).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null)
            {
                return;
            }

            var d3 = repositorio.BuscarPelaChave(primeiroDaLIsta.CredencialImpressaoId);
            Assert.IsNotNull(d3);
        }

        [TestMethod]
        [Description("Objetivo: Cadastrar/listar/alterar/remover dados de Tipos de Serviços")]
        public void TipoServico_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoServicoRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new TipoServico
                {
                    Descricao = "Tipo de Serviço " + i
                };
                repositorio.Criar(d1);

                d1.Descricao = "Service Type " + i;
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(filtro).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var ultimoDaLIsta = list0.LastOrDefault();

            var d3 = repositorio.BuscarPelaChave(ultimoDaLIsta.TipoServicoId);
            Assert.IsNotNull(d3);

            //Remover
            repositorio.Remover(d3);
            var d4 = repositorio.BuscarPelaChave(d3.TipoServicoId);
            Assert.IsNull(d4);
        }

        [TestMethod]
        [Description("Objetivo: Cadastrar/listar/alterar/remover dados de Tipos de Equipamento")]
        public void TipoEquipamento_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoEquipamentoRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new TipoEquipamento
                {
                    Descricao = "TIPO EQUIPAMENTO " + (i + 1)
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Descricao = "TIPO EQUIPAMENTO Alterado.";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(filtro).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var ultimoDaLIsta = list0.LastOrDefault();
            var d2 = repositorio.BuscarPelaChave(ultimoDaLIsta.TipoEquipamentoId);
            Assert.IsNotNull(d2);

            //Remover
            repositorio.Remover(d2);
            var d3 = repositorio.BuscarPelaChave(d2.TipoEquipamentoId);
            Assert.IsNull(d3);
        }

        [TestMethod]
        [Description("Objetivo: Cadastrar/listar/alterar/remover dados de Tipos de Combustível")]
        public void TipoCombustivel_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoCombustivelRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new TipoCombustivel
                {
                    Descricao = "TIPO COMBUSTÍVEL " + (i + 1)
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Descricao = "TIPO COMBUSTÍVEL Alterado.";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(filtro).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var ultimoDaLIsta = list0.LastOrDefault();

            var d2 = repositorio.BuscarPelaChave(ultimoDaLIsta.TipoCombustivelId);
            Assert.IsNotNull(d2);

            //Remover
            repositorio.Remover(d2);
            var d3 = repositorio.BuscarPelaChave(d2.TipoCombustivelId);
            Assert.IsNull(d3);
        }

        [TestMethod]
        [Description("Objetivo: Cadastrar/listar/alterar/remover dados de Tipos de Cobrança")]
        public void TipoCobranca_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoCobrancaRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new TipoCobranca
                {
                    Descricao = "TIPO COBRANÇA " + (i + 1)
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Descricao = "TIPO COBRANÇA Alterado.";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(filtro).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var ultimoDalista = list0.LastOrDefault();

            var d2 = repositorio.BuscarPelaChave(ultimoDalista.TipoCobrancaId);
            Assert.IsNotNull(d2);

            //Remover
            repositorio.Remover(d2);
            var d3 = repositorio.BuscarPelaChave(d2.TipoCobrancaId);
            Assert.IsNull(d3);
        }

        [TestMethod]
        [Description("Objetivo: Cadastrar/listar/alterar/remover dados de Tipos de Atividade")]
        public void TipoAtividade_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoAtividadeRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new TipoAtividade
                {
                    Descricao = "TIPO ATIVIDADE " + (i + 1)
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Descricao = "TIPO ATIVIDADE Alterado.";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(filtro).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var ultimoDalista = list0.LastOrDefault();

            var d2 = repositorio.BuscarPelaChave(ultimoDalista.TipoAtividadeId);
            Assert.IsNotNull(d2);

            //Remover
            repositorio.Remover(d2);
            var d3 = repositorio.BuscarPelaChave(d2.TipoAtividadeId);
            Assert.IsNull(d3);
        }

        [TestMethod]
        [Description("Objetivo: Cadastrar/listar/alterar/remover dados de Tipos de Acesso")]
        public void TipoAcesso_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoAcessoRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new TipoAcesso
                {
                    Descricao = "TIPO ACESSO " + (i + 1)
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Descricao = "TIPO ACESSO Alterado.";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(filtro).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var ultimoDalista = list0.LastOrDefault();

            var d2 = repositorio.BuscarPelaChave(ultimoDalista.TipoAcessoId);
            Assert.IsNotNull(d2);

            //Remover
            repositorio.Remover(d2);
            var d3 = repositorio.BuscarPelaChave(d2.TipoAcessoId);
            Assert.IsNull(d3);
        }

        [TestMethod]
        [Description("Objetivo: Cadastrar/listar/alterar/remover dados de Tecnologia de Credencial")]
        public void TecnologiaCredencial_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TecnologiaCredencialRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new TecnologiaCredencial
                {
                    Descricao = "TECNOLOGIA CREDENCIAL " + (i + 1)
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Descricao = "TECNOLOGIA CREDENCIAL Alterado.";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(filtro).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var ultimoDalista = list0.LastOrDefault();

            var d2 = repositorio.BuscarPelaChave(ultimoDalista.TecnologiaCredencialId);
            Assert.IsNotNull(d2);

            //Remover
            repositorio.Remover(d2);
            var d3 = repositorio.BuscarPelaChave(d2.TecnologiaCredencialId);
            Assert.IsNull(d3);
        }

        [TestMethod]
        [Description("Objetivo: Cadastrar/listar/alterar/remover dados de Status")]
        public void Status_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new StatusRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new Status
                {
                    Descricao = "Status " + (i + 1)
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Descricao = "Status Alterado.";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Descricao;

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(filtro).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var ultimoDalista = list0.LastOrDefault();

            var d2 = repositorio.BuscarPelaChave(ultimoDalista.StatusId);
            Assert.IsNotNull(d2);

            //Remover
            repositorio.Remover(d2);
            var d3 = repositorio.BuscarPelaChave(d2.StatusId);
            Assert.IsNull(d3);
        }

        [TestMethod]
        [Description("Objetivo: Cadastrar/listar/alterar/remover dados de Relatórios")]
        public void Relatorios_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new RelatoriosRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new Relatorios
                {
                    Nome = "Relatorio " + (i + 1),
                    NomeArquivoRpt = "file.rpt",
                    ArquivoRpt = "file.txt",
                    Ativo = true
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Nome = "Relatorio Alterado." + i * 2;

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar(null).ToList();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Nome;

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(filtro).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var ultimoDalista = list0.LastOrDefault();
            var d2 = repositorio.BuscarPelaChave(ultimoDalista.RelatorioId);
            Assert.IsNotNull(d2);

            //Remover
            repositorio.Remover(d2);
            var d3 = repositorio.BuscarPelaChave(d2.RelatorioId);
            Assert.IsNull(d3);
        }


        [TestMethod]
        [Description("Objetivo: Cadastrar/listar/alterar/remover dados de Relatórios Gerenciais")]
        public void RelatoriosGerenciais_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new RelatoriosGerenciaisRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new RelatoriosGerenciais
                {
                    Nome = "Relatorio Gerencial" + (i + 1),
                    NomeArquivoRpt = "file.rpt",
                    ArquivoRpt = "file.txt",
                    Ativo = true
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Nome = "Relatorio Gerencial Alterado." + i * 2;

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            string filtro = repositorio.Listar().LastOrDefault().Nome;

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(filtro).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var ultimoDalista = list0.LastOrDefault();

            var d2 = repositorio.BuscarPelaChave(ultimoDalista.RelatorioId);
            Assert.IsNotNull(d2);

            //Remover
            repositorio.Remover(d2);
            var d3 = repositorio.BuscarPelaChave(d2.RelatorioId);
            Assert.IsNull(d3);
        }

        #endregion

    }

}



