// ***********************************************************************
// Project: UnitTestImod
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
// ***********************************************************************

#region

using System;
using System.IO;
using System.Linq;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Infra.Repositorios;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace UnitTestImod
{
    [TestClass]
    public class UnitTest1
    {
        private Colaborador _colaborador;
        private ColaboradorCredencial _colaboradorCredencial;
        private ColaboradorEmpresa _colaboradorEmpresa;
        private ColaboradorCurso _colaboradorCurso;
        private ColaboradorAnexo _colaboradorAnexo;
        private Curso _curso;
        private Empresa _empresa;
        private EmpresaAnexo _empresaAnexo;
        private EmpresaContrato _empresaContrato;
        private EmpresaSignatario _empresaSignatario;
        private Veiculo _veiculo;
        private VeiculoAnexo _veiculoAnexo;
        private VeiculoEmpresa _veiculoEmpresa;
        private VeiculoSeguro _veiculoSeguro;

        #region  Metodos

        [TestInitialize]
        public void Inicializa()
        {
            #region Dados de uma empresa

            _empresa = new Empresa
            {
                Nome = "Empresa Unit Test",
                Apelido = "Apelido",
                Bairro = "Bairro",
                Celular1 = "71 988569541",
                Celular2 = "71 988569541",
                Cep = "40000",
                Complemento = "Complemento",
                Cnpj = "21.877.574/0001-12",
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

            #region Representante Signatário

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
                EmailResp = "email@email",
                Emissao = DateTime.Now,
                Numero = "21",
                NomeResp = "Nome responsavel",
                NomeArquivo = "Nome arquivo",
                TelefoneResp = "71554212",
                NumeroContrato = "1124122",
                Terceirizada = "Terceirizada",
                EstadoId = 1,
                MunicipioId = 1,
                Validade = DateTime.Now.AddDays(10)
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

            #region Veiculo

            _veiculo = new Veiculo
            {
                Descricao = "Veiculo Unit Test",
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
                Serie_Chassi = "445445454",
                Largura = "25",
                Placa_Identificador = "asd58455",
                Tipo = "Tipo",
                DescricaoAnexo = "Descricao Anexo",
                Pendente31 = true,
                Pendente32 = true,
                Pendente33 = false,
                Pendente34 = false
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

            #region Veiculo Anexo

            _veiculoAnexo = new VeiculoAnexo
            {
                Descricao = "Descricao",
                Arquivo = Convert.ToBase64String(File.ReadAllBytes("Arquivos/contrato.pdf")),
                NomeArquivo = "Nome arquivo"
            };

            #endregion

            #region Veiculo Empresa

            _veiculoEmpresa = new VeiculoEmpresa
            {
                Ativo = true,
                Cargo = "cargo",
                Matricula = "002542"
            };

            #endregion

            #region Colaborador

            _colaborador =
                new Colaborador
                {
                    Apelido = "Marinpietri",
                    DataNascimento = DateTime.Today.AddYears(-26),
                    NomePai = "Valnei",
                    NomeMae = "Veronice",
                    Nacionalidade = "Brasil",
                    Foto = null,
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
                    CnhEmissor = "",
                    Cnhuf = "",
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

            #region Colaborador Credencial

            _colaboradorCredencial =
                new ColaboradorCredencial
                {
                    NumeroCredencial = "000000",
                    Fc = 101,
                    Emissao = DateTime.Now,
                    Validade = DateTime.Now.AddDays(365),
                    CardHolderGuid = "000000",
                    CredencialGuid = "000000",
                    Ativa = true,
                    Colete = "000000",
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

            #region Colaborador Curso

            _colaboradorCurso =
                new ColaboradorCurso()
                {
                    NomeArquivo = "File 1",
                    Arquivo = "Arquivo1.arq",
                    Controlado = true
                };

            #endregion

            _colaboradorAnexo =
                new ColaboradorAnexo()
                {
                    NomeArquivo = "Anexo 1",
                    Arquivo = "Anex1.arq",
                    Descricao = "DESCRIÇÃO"
                };

            #endregion

            #region Curso
            _curso =
                new Curso()
                {
                    Descricao = "Curso 1"
                };

            #endregion


        }

        [Priority(3)]
        [TestMethod]
        [Description("Objetivo cadastrar/ler/alterar/remover dados de Colaboradores e seus relacionamentos")]
        public void Colaborador_Cadastro_geral_com_Sucesso()
        {
            var service = new ColaboradorService();

            #region CRUD Colaborador

            //Cadastrar 5 Colaboradores
            for (var i = 0; i < 5; i++)
            {
                _colaborador.Cpf = Utils.GerarCpf();
                _colaborador.Nome = $"Colaborador ({i})";
                var d0 = service.ObterPorCpf(_colaborador.Cpf);
                if (d0 == null)
                    service.Criar(_colaborador);
            }

            //Alterar 2 Colaboradores
            for (var j = 0; j < 2; j++)
            {
                int lastId = service.Listar().LastOrDefault().ColaboradorId - j;
                var d1 = service.BuscarPelaChave(lastId);

                d1.Nome = $"Colaborador {j * 100}";
                d1.Cpf = Utils.GerarCpf();

                service.Alterar(d1);
            }

            //Listar Colaboradores
            string FirstNome = service.Listar().FirstOrDefault().Nome;

            var list1 = service.Listar(0, null, "%" + FirstNome + "%").ToList();
            Assert.IsNotNull(list1);

            var b0 = service.BuscarPelaChave(service.Listar().LastOrDefault().ColaboradorId);
            Assert.IsNotNull(b0);

            //Remover 1 Colaborador
            service.Remover(b0);
            var d2 = service.BuscarPelaChave(b0.ColaboradorId);
            Assert.IsNull(d2);


            #endregion

            #region CRUD Empresas Vínculos (ColaboradoresEmpresas)

            //Cadastrar 5 Empresas Vínculos
            for (var j = 0; j < 5; j++)
            {
                _colaboradorEmpresa.ColaboradorId = _colaborador.ColaboradorId;
                service.Empresa.Criar(_colaboradorEmpresa);
            }

            //Alterar 2 Empresas Vínculos
            for (var j = 0; j < 2; j++)
            {
                int lastId = service.Empresa.Listar().LastOrDefault().ColaboradorEmpresaId - j;

                var d0 = service.Empresa.BuscarPelaChave(lastId);

                d0.Cargo = $"Cargo ({j})"; ;
                d0.Matricula = $"Matrícula ({j})"; ;
                d0.Ativo = false;

                service.Empresa.Alterar(d0);
            }

            //Listar Empresas Vínculos 
            string FirstMatricula = service.Empresa.Listar().FirstOrDefault().Matricula;

            var list2 = service.Empresa.Listar(0, null, null, "%" + FirstMatricula + "%").ToList();
            Assert.IsNotNull(list2);

            int Id = service.Empresa.Listar().LastOrDefault().ColaboradorEmpresaId;

            var b1 = service.Empresa.BuscarPelaChave(Id);
            Assert.IsNotNull(b1);

            //Remover 1 Empresa Vínculo
            service.Empresa.Remover(b1);
            var d3 = service.Empresa.BuscarPelaChave(b1.ColaboradorEmpresaId);
            Assert.IsNull(d3);

            #endregion

            #region CRUD Treinamentos e Certificações (ColaboradoresCursos) 

            //Cadastrar 5 Cursos
            for (var j = 0; j < 5; j++)
            {
                _colaboradorCurso.ColaboradorId = _colaborador.ColaboradorId;
                _colaboradorCurso.CursoId = _curso.CursoId;
                service.Curso.Criar(_colaboradorCurso);
            }

            //Alterar 2 Cursos
            for (var j = 0; j < 2; j++)
            {
                int lastId = service.Curso.Listar().LastOrDefault().ColaboradorCursoId - j;

                var d0 = service.Curso.BuscarPelaChave(lastId);

                d0.NomeArquivo = $"Nome Arquivo ({j * 100})";
                d0.Arquivo = $"Arquivo.arq ({j * 100})";
                d0.Controlado = false;

                service.Curso.Alterar(d0);
            }

            //Listar Cursos 
            string nomeArquivo = service.Curso.Listar().FirstOrDefault().NomeArquivo;

            var list3 = service.Curso.Listar(0, 0, "%" + nomeArquivo + "%", null, null).ToList();
            Assert.IsNotNull(list3);

            int cursoId = service.Curso.Listar().LastOrDefault().ColaboradorCursoId;

            var b2 = service.Curso.BuscarPelaChave(cursoId);
            Assert.IsNotNull(b2);

            //Remover 1 Cursos
            service.Curso.Remover(b2);
            var d4 = service.Curso.BuscarPelaChave(b2.ColaboradorCursoId);
            Assert.IsNull(d4);

            #endregion

            #region CRUD Anexos

            //Cadastrar 5 Anexos
            for (var j = 0; j < 5; j++)
            {
                _colaboradorAnexo.ColaboradorId = _colaborador.ColaboradorId;
                service.Anexo.Criar(_colaboradorAnexo);
            }

            //Alterar 2 Anexo
            for (var j = 0; j < 2; j++)
            {
                int lastId = service.Anexo.Listar().LastOrDefault().ColaboradorAnexoId - j;

                var d0 = service.Anexo.BuscarPelaChave(lastId);

                d0.NomeArquivo = $"Anexo ({j})";
                d0.Descricao = $"Descrição ({j})";

                service.Anexo.Alterar(d0);
            }

            //Listar Anexo 
            string FirstDesc = service.Anexo.Listar().FirstOrDefault().NomeArquivo;

            var list4 = service.Anexo.Listar(0, "%" + FirstDesc + "%").ToList();
            Assert.IsNotNull(list4);

            int key = service.Anexo.Listar().LastOrDefault().ColaboradorAnexoId;

            var b3 = service.Anexo.BuscarPelaChave(key);
            Assert.IsNotNull(b3);

            //Remover 1 Anexo
            service.Anexo.Remover(b3);
            var d5 = service.Anexo.BuscarPelaChave(b1.ColaboradorEmpresaId);
            Assert.IsNull(d5);

            #endregion

            //TODO: Continuar TestUnit1 (Mihai)
            #region CRUD Credenciais



            #endregion

        }


        [Priority(2)]
        [TestMethod]
        [Description("Objetivo cadastrar dados de Veículos e seus relacionamentos")]
        public void Veiculo_Cadastro_geral_com_Sucesso()
        {
            #region Cadastrar Dados Auxiliares

            var serviceAuxiliar = new DadosAuxiliaresFacadeService();
            serviceAuxiliar.TiposAcessoService.Criar(new TipoAcesso
            {
                Descricao = "Acesso 2"
            });
            _veiculo.TipoAcessoId = serviceAuxiliar.TiposAcessoService.Listar().FirstOrDefault().TipoAcessoId;
            _veiculo.CombustivelId = serviceAuxiliar.TipoCombustivelService.Listar().FirstOrDefault().TipoCombustivelId;
            _veiculo.EquipamentoVeiculoId = serviceAuxiliar.TipoEquipamentoService.Listar().FirstOrDefault().TipoEquipamentoId;

            #endregion

            #region Cadastrar Veiculo

            var service = new VeiculoService();
            service.Criar(_veiculo);
            var d1 = service.BuscarPelaChave(_veiculo.EquipamentoVeiculoId);
            Assert.IsNotNull(d1);
            service.Remover(d1);
            var d2 = service.BuscarPelaChave(d1.EquipamentoVeiculoId);
            Assert.IsNull(d2);
            var l1 = service.Listar("%" + d1.Descricao + "%", null);
            Assert.IsNotNull(l1);

            #endregion
        }

        [Priority(1)]
        [TestMethod]
        [Description("Objetivo cadastrar dados de Empresas e seus relacionamentos")]
        public void Empresa_Cadastro_geral_com_Sucesso()
        {
            var service = new EmpresaService();

            #region Cadastrar Caracterisitcas

            var serviceAuxiliar = new DadosAuxiliaresFacadeService();
            //Tipo de Atividade
            serviceAuxiliar.TipoAtividadeService.Criar(new TipoAtividade
            {
                Descricao = "Descricao"
            });
            //Modelo de crachá
            serviceAuxiliar.LayoutCrachaService.Criar(new LayoutCracha
            {
                Nome = "Modelo 1",
                Valor = 54,
                LayoutCrachaGuid = "0098787667676666"
            });

            #endregion

            #region Cadastro de Empresa

            //Já existindo empresa, então nao cadastrar novamente, pois não é possivel haver 2 CNPJ iguais
            var empresa = service.BuscarEmpresaPorCnpj(_empresa.Cnpj);
            if (empresa == null)
            {
                service.Criar(_empresa);
            }
            else
            {
                //Então use esse registro, caso exista
                _empresa = empresa;
            }

            service.TipoAtividadeService.Criar(new EmpresaTipoAtividade
            {
                Descricao = "Atividade 1",
                EmpresaId = _empresa.EmpresaId,
                TipoAtividadeId = serviceAuxiliar.TipoAtividadeService.Listar().FirstOrDefault().TipoAtividadeId
            });

            service.CrachaService.Criar(new EmpresaLayoutCracha
            {
                Nome = "Nome",
                EmpresaId = _empresa.EmpresaId,
                LayoutCrachaId = serviceAuxiliar.LayoutCrachaService.Listar().FirstOrDefault().LayoutCrachaId
            });

            #endregion

            #region Cadastro de Signatários

            //Cadastrar 5 Signatario
            for (var i = 0; i < 5; i++)
            {
                _empresaSignatario.EmpresaId = _empresa.EmpresaId;
                service.SignatarioService.Criar(_empresaSignatario);
            }

            #endregion

            #region Contrato

            //Cadastrar 5 contratos
            for (var i = 0; i < 5; i++)
            {
                _empresaContrato.EmpresaId = _empresa.EmpresaId;
                service.ContratoService.Criar(_empresaContrato);
                var n1 = service.ContratoService.BuscarPelaChave(_empresaContrato.EmpresaContratoId);
                var l1 = service.ContratoService.ListarPorEmpresa(_empresa.EmpresaId);
                var l2 = service.ContratoService.ListarPorDescricao("Des");
                var l3 = service.ContratoService.ListarPorNumeroContrato("1124122");
                Assert.IsNotNull(l1);
                Assert.IsNotNull(l2);
                Assert.IsNotNull(l3);
                Assert.IsNotNull(n1);
            }

            #endregion

            #region Anexos

            //Cadastrar 5 anexos
            for (var i = 0; i < 5; i++)
            {
                _empresaAnexo.EmpresaId = _empresa.EmpresaId;
                service.AnexoService.Criar(_empresaAnexo);
            }

            #endregion

            #region Cadastrar Pendencias

            service.Pendencia.Criar(new Pendencia
            {
                EmpresaId = _empresa.EmpresaId,
                Descricao = "Teste Unitário",
                DataLimite = DateTime.Now.Date,
                Impeditivo = true,
                TipoPendenciaId = 24
            });

            #endregion

            #region Listar Pendencias

            var listP = service.ListarPendencias(_empresa.EmpresaId).FirstOrDefault();
            if (listP == null) throw new Exception("Uma pendência foi cadastrada, mas não foi possível retorna informção");
            var pend = listP.Pendencias;
            Assert.IsTrue(!string.IsNullOrWhiteSpace(pend));

            #endregion

            #region Veiculos Vinculados

            var veicserv = new VeiculoService();
            veicserv.Criar(_veiculo);
            var v1 = veicserv.BuscarPelaChave(_veiculo.EquipamentoVeiculoId);
            _veiculoEmpresa.VeiculoId = v1.EquipamentoVeiculoId;
            _veiculoEmpresa.EmpresaId = _empresa.EmpresaId;
            _veiculoEmpresa.EmpresaContratoId = _empresaContrato.EmpresaContratoId;

            for (var i = 0; i < 5; i++)
            {
                veicserv.Veiculo.Criar(_veiculoEmpresa);
            }

            var list0 = veicserv.Veiculo.Listar();
            Assert.IsNotNull(list0);

            var b1 = veicserv.Veiculo.BuscarPelaChave(_veiculoEmpresa.VeiculoEmpresaId);
            Assert.IsNotNull(b1);

            var b2 = veicserv.Veiculo.Listar(0, 0, "%" + _veiculoEmpresa.Matricula + "%");
            Assert.IsNotNull(b2);

            //Remover
            veicserv.Veiculo.Remover(b1);
            var b4 = veicserv.Veiculo.BuscarPelaChave(b1.VeiculoEmpresaId);
            Assert.IsNull(b4);

            #endregion
        }

        [TestMethod]
        public void EmpresaContrato_insere_listar_arquivo_dados_filestream_com_sucesso()
        {
            var repositorio = new EmpresaContratoRepositorio();
            //Obter array de bytes do arquivo teste
            var arrayByte = File.ReadAllBytes("Arquivos/contrato.pdf");
            //Transformar numa string Base64
            var strBase64 = Convert.ToBase64String(arrayByte);

            for (var i = 0; i < 6; i++)
            {
                var d1 = new EmpresaContrato
                {
                    EmpresaId = i,
                    NumeroContrato = "10" + i,
                    Descricao = "Teste de Unidade " + (i + 1),
                    Emissao = DateTime.Now,
                    Validade = DateTime.Now,
                    Terceirizada = "3",
                    Contratante = "CONTRATANTE " + (i + 1),
                    IsencaoCobranca = "I",
                    TipoCobrancaId = i,
                    CobrancaEmpresaId = i,
                    StatusId = 1,
                    Arquivo = strBase64,
                    TipoAcessoId = 1,
                    NomeArquivo = "contrato.pdf",
                    ArquivoBlob = arrayByte
                };
                repositorio.Criar(d1); //Criar dados
                var d2 = repositorio.BuscarPelaChave(d1.EmpresaContratoId);
                Assert.IsNotNull(d2.ArquivoBlob);
            }

            //ler dados
        }

        [TestMethod]
        public void ColabororadorCredencial_Alterar_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();
            repositorio.Alterar(new ColaboradorCredencial
            {
                ColaboradorCredencialId = 1,
                Fc = 420,
                Emissao = DateTime.Now,
                Validade = DateTime.Now,
                NumeroCredencial = "CR3D3NC1@L @LT3R@D@",
                Baixa = DateTime.Now
            });
        }

        [TestMethod]
        public void ColabororadorCredencial_Buscar_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();

            var d1 = repositorio.Listar().FirstOrDefault();

            if (d1 == null) repositorio.Criar(_colaboradorCredencial);

            var d2 = repositorio.BuscarPelaChave(_colaboradorCredencial.ColaboradorCredencialId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void ColabororadorCredencial_ListarColaboradorCredeniasView_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();

            //ColaboradorCredencialID
            var d2 = repositorio.Listar(1, 0, 0);
            //CredencialStatusID
            var d3 = repositorio.Listar(0, 2, 0);
            //FormatoCredencialID
            var d4 = repositorio.Listar(0, 0, 7);

            Assert.IsNotNull(d2);
            Assert.IsNotNull(d3);
            Assert.IsNotNull(d4);
        }

        [TestMethod]
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
        public void ColaboradorCursos_com_sucesso()
        {
            var repositorio = new ColaboradorCursoRepositorio();
            for (var i = 0; i < 6; i++)
            {
                var d1 = new ColaboradorCurso
                {
                    Arquivo = "Arquivo" + i,
                    ColaboradorId = i,
                    Controlado = true,
                    CursoId = i,
                    NomeArquivo = "file" + i + ".arq",
                    Validade = DateTime.Today.AddYears(i)
                };
                repositorio.Criar(d1);

                d1.Arquivo = "Arquivo" + i + " alterado!";
                repositorio.Alterar(d1);
            }
            var list0 = repositorio.Listar(0, 0, "%2%", null, 0).ToList();
            var list1 = repositorio.Listar();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void CredencialMotivo_com_sucesso()
        {
            var repositorio = new CredencialMotivoRepositorio();

            //for (var i = 0; i < 6; i++)
            //{
            //    var d1 = new CredencialMotivo
            //    {
            //        CredencialMotivoId = i,
            //        Descricao = "Descrição Motivo " + i,
            //        Tipo = 1 + i
            //    };
            //    repositorio.Criar(d1);

            //    d1.Descricao = "Descrição Motivo " + i + "  alterado" + i;
            //    repositorio.Alterar(d1);
            //}

            var list0 = repositorio.Listar();
            var list1 = repositorio.Listar(0, "%ADA%", 0).ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void CredencialStatusRepositorio_com_sucesso()
        {
            var repositorio = new CredencialStatusRepositorio();

            //for (var i = 0; i < 6; i++)
            //{
            //    var d1 = new CredencialStatus
            //    {
            //        CredencialStatusId = i,
            //        Descricao = "Credencial status " + i
            //    };
            //    repositorio.Criar(d1);

            //    d1.Descricao = "Credencial status alterado" + i;
            //    repositorio.Alterar(d1);
            //}

            var list = repositorio.Listar(); //CursoRepositorio
        }

        [TestMethod]
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
            var list0 = repositorio.Listar();
            var list1 = repositorio.Listar(0, "%AVSEC%").ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void EmpresaAreaAcessoRepositorio_com_sucesso()
        {
            var repositorio = new EmpresaAreaAcessoRepositorio();

            for (var i = 0; i < 6; i++)
            {
                var d1 = new EmpresaAreaAcesso
                {
                    AreaAcessoId = i + 1,
                    EmpresaId = i + 1
                };
                repositorio.Criar(d1);

                d1.EmpresaAreaAcessoId = 2;
                repositorio.Alterar(d1);
            }

            var list0 = repositorio.Listar();
            var frst = list0.FirstOrDefault();
            //EmpresaAreaAcessoID
            var d2 = repositorio.Listar(frst.EmpresaAreaAcessoId, 5, 1);
            //EmpresaID
            var d3 = repositorio.Listar(0, frst.EmpresaId, 0);
            //AreaAcessoID
            var d4 = repositorio.Listar(0, 0, frst.AreaAcessoId);

            Assert.IsNotNull(d2);
            Assert.IsNotNull(d3);
            Assert.IsNotNull(d4);
        }

        [TestMethod]
        public void EmpresaContratoRepositorio_com_sucesso()
        {
            var repositorio = new EmpresaContratoRepositorio();
            for (var i = 0; i < 6; i++)
            {
                var d1 = new EmpresaContrato
                {
                    EmpresaId = i,
                    NumeroContrato = "10" + i,
                    Descricao = "CONTRATO " + (i + 1),
                    Emissao = DateTime.Now,
                    Validade = DateTime.Now,
                    Terceirizada = "3",
                    Contratante = "CONTRATANTE " + (i + 1),
                    IsencaoCobranca = "I",
                    TipoCobrancaId = i,
                    CobrancaEmpresaId = i,
                    Cep = "427900-" + i + "00",
                    Endereco = "END. " + (i + 1),
                    Complemento = "COMPL. " + (i + 1),
                    Numero = "# " + (i + 1),
                    Bairro = "BAIRRO " + (i + 1),
                    MunicipioId = i,
                    EstadoId = i,
                    NomeResp = "RESP. " + (i + 1),
                    TelefoneResp = "(71) 4477-000" + i,
                    CelularResp = "(71) 99283-000" + i,
                    EmailResp = "email" + i + "@email.com",
                    StatusId = 1,
                    Arquivo = "ARQUIVOS " + (i + 1),
                    TipoAcessoId = 1,
                    NomeArquivo = "arquivo.exe"
                };
                repositorio.Criar(d1);

                d1.NumeroContrato = "Contrato Alterado para 99" + i;
                repositorio.Alterar(d1);
            }
            var list0 = repositorio.Listar();

            var list1 = repositorio.Listar(null, "%CONTR%", null, null, null, null, null).ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void EmpresaEquipamentoRepositorio_com_sucesso()
        {
            var repositorio = new EmpresaEquipamentoRepositorio();
            for (var i = 0; i < 6; i++)
            {
                var d1 = new EmpresaEquipamento
                {
                    EmpresaEquipamentoId = i,
                    EmpresaId = 1,
                    Descricao = "Descrição " + i,
                    Marca = "Marca " + i,
                    Modelo = "Modelo" + i,
                    Ano = "2018",
                    Patrimonio = "",
                    Seguro = "",
                    ApoliceSeguro = "",
                    ApoliceValor = "",
                    ApoliceVigencia = "",
                    DataEmissao = DateTime.Now,
                    DataValidade = DateTime.Now,
                    Excluido = "",
                    TipoEquipamentoId = i,
                    StatusId = i,
                    TipoAcessoId = i
                };
                repositorio.Criar(d1);

                d1.Descricao = "Descrição alterada" + i;
                repositorio.Alterar(d1);
            }
            var list0 = repositorio.Listar();
            var list1 = repositorio.Listar("%1%", null, null, null, null).ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void EmpresaLayoutCrachaRepositorio_com_sucesso()
        {
            var repositorio = new EmpresaLayoutCrachaRepositorio();
            for (var i = 0; i < 6; i++)
            {
                var d1 = new EmpresaLayoutCracha
                {
                    EmpresaId = 1,
                    EmpresaLayoutCrachaId = i,
                    LayoutCrachaId = 1
                };
                repositorio.Criar(d1);

                d1.LayoutCrachaId = 2;
                repositorio.Alterar(d1);
            }
            var list = repositorio.Listar();

            //EmpresaLayoutCrachaID
            var d2 = repositorio.Listar(1, 0, 0);
            //EmpresaID
            var d3 = repositorio.Listar(0, 1, 0);
            //LayoutCrachaID
            var d4 = repositorio.Listar(0, 0, 2);

            Assert.IsNotNull(d2);
            Assert.IsNotNull(d3);
            Assert.IsNotNull(d4);
        }

        [TestMethod]
        public void EmpresaSeguroRepositorio_com_sucesso()
        {
            var repositorio = new EmpresaSeguroRepositorio();
            for (var i = 0; i < 6; i++)
            {
                var d1 = new EmpresaSeguro
                {
                    EmpresaSeguroId = i,
                    NomeSeguradora = "NomeSeguradora" + i,
                    NumeroApolice = "5888" + i,
                    ValorCobertura = "100" + i,
                    EmpresaId = 1,
                    Arquivo = "Arquivo" + i,
                    NomeArquivo = "",
                    Emissao = DateTime.Now,
                    Validade = DateTime.Now
                };
                repositorio.Criar(d1);

                d1.NomeSeguradora = "NomeSeguradora alterado" + i;
                repositorio.Alterar(d1);
            }
            var list0 = repositorio.Listar();
            var list1 = repositorio.Listar("%2%", null, null, null).ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void LayoutCrachaRepositorio_com_sucesso()
        {
            var repositorio = new LayoutCrachaRepositorio();
            //for (var i = 0; i < 6; i++)
            //{
            //    var d1 = new LayoutCracha
            //    {
            //        LayoutCrachaId = i,
            //        Nome = "Nome do cracha" + i,
            //        LayoutCrachaGuid = "",
            //        Valor = i
            //    };
            //    repositorio.Criar(d1);

            //    d1.Nome = "Nome do cracha alterado" + i;
            //    repositorio.Alterar(d1);
            //}
            var list0 = repositorio.Listar();
            var list1 = repositorio.Listar("%#%", null).ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void PendenciaRepositorio_com_sucesso()
        {
            var repositorio = new PendenciaRepositorio();
            //for (var i = 0; i < 6; i++)
            //{
            //    var d1 = new Pendencia
            //    {
            //        PendenciaId = i,
            //        TipoPendenciaId = 1,
            //        Descricao = "Descrição Pendencias",
            //        DataLimite = DateTime.Now,
            //        Impeditivo = true,
            //        ColaboradorId = 1,
            //        EmpresaId = i,
            //        VeiculoId = i
            //    };
            //    repositorio.Criar(d1);

            //    d1.Descricao = "Aleração Descrição Pendencias" + i;
            //    repositorio.Alterar(d1);
            //}
            var list0 = repositorio.Listar();
            var list1 = repositorio.Listar("%!%", null, null).ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void Colabororador_Criar_Listar_com_sucesso()
        {
            var service = new ColaboradorService();

            #region Cadastrar Colaborador

            _colaborador.Cpf = "483.578.550-91";
            _colaborador.Nome = "Colaborador Nome";
            var d1 = service.ObterPorCpf(_colaborador.Cpf);
            if (d1 == null)
                service.Criar(_colaborador);

            #endregion

            var l1 = service.Listar(); //Listar todos
            Assert.IsNotNull(l1);
            var l2 = service.Listar(null, null, "Suzuki"); //Listar por nome
            Assert.IsNotNull(l2);
        }

        [TestMethod]
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
        public void Colabororador_Cadastrar_com_sucesso()
        {
            var service = new ColaboradorService();

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

            int i = 0;
            foreach (var item in cpfArray)
            {
                _colaborador.Cpf = item;
                _colaborador.Nome = $"Colaborador ({i})";
                var d1 = service.ObterPorCpf(item);
                if (d1 == null)
                    service.Criar(_colaborador);
                i++;
            }

            //TODO: Implementar acesso a dados [Minhai]...
        }

        [TestMethod]
        public void VeiculoAnexo_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var service = new VeiculoService();
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


        //    var list0 = repositorio.Listar(0, "%arq%").ToList();
        //    var d1 = repositorio.BuscarPelaChave(d0.ColaboradorAnexoId);

        //    var d0 = repositorio.Listar().FirstOrDefault();
        //    var repositorio = new ColaboradorAnexoRepositorio();
        //{
        //public void ColabororadorAnexo_Buscar_Criar_Alterar_com_sucesso()

        //[TestMethod]
        //}
        //    Assert.IsNull(b4);
        //    var b4 = service.Seguro.BuscarPelaChave(b1.VeiculoSeguroId);
        //    service.Seguro.Remover(b1);
        //    //Remover
        //    Assert.IsNotNull(b3);

        //    var b3 = service.Seguro.Listar(0, "%" + _veiculoSeguro.NomeSeguradora + "%", null);
        //    Assert.IsNotNull(b1);

        //    var b1 = service.Seguro.BuscarPelaChave(_veiculoSeguro.VeiculoSeguroId);
        //    Assert.IsNotNull(list0);

        //    var list0 = service.Seguro.Listar().ToList();
        //    }
        //        service.Seguro.Alterar(_veiculoSeguro);
        //        _veiculoSeguro.NomeSeguradora = "Alterado";
        //        //update
        //        service.Anexo.Criar(_veiculoAnexo);
        //        //criar
        //    {
        //    for (var i = 0; i < 5; i++)
        //    _veiculoAnexo.VeiculoId = _veiculo.EquipamentoVeiculoId;
        //    service.Criar(_veiculo);
        //    var service = new VeiculoService();
        //{
        //public void VeiculoSeguros_Cadastrar_Alterar_Listar_Remover_com_sucesso()

        //[TestMethod]
        //    var list1 = repositorio.Listar(0, d0.ColaboradorId).ToList();

        //    d1.NomeArquivo = "Novo Nome arquivo";
        //    repositorio.Alterar(d1);
        //    d1.Descricao = "Descrição Alterada";
        //    repositorio.Criar(d1);
        //}
        //[TestMethod]
        //public void VeiculoCredencialimpressao_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new VeiculoCredencialimpressaoRepositorio();

        //    for (var i = 0; i < 6; i++)
        //    {
        //        var d1 = new VeiculoCredencialimpressao
        //        {
        //            VeiculoCredencialId = i + 100,
        //            DataImpressao = DateTime.Now,
        //            Cobrar = true
        //        };

        //        //Criar registro
        //        repositorio.Criar(d1);

        //        d1.DataImpressao = DateTime.Today.AddDays(i + 1);

        //        //Alterar registro
        //        repositorio.Alterar(d1);
        //    }

        //    //Listar todos
        //    var list0 = repositorio.Listar().ToList();
        //    Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar(1).ToList();
        //    Assert.IsNotNull(list1);

        //    ////Listar pela chave
        //    var primeiroDaLIsta = list0.FirstOrDefault();
        //    if (primeiroDaLIsta == null) return;
        //    var d3 = repositorio.BuscarPelaChave(primeiroDaLIsta.CredencialImpressaoId);
        //    Assert.IsNotNull(d3);
        //}

        //[TestMethod]
        //public void TipoServico_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new TipoServicoRepositorio();

        //    //for (var i = 0; i < 6; i++)
        //    //{
        //    //    var d1 = new TipoServico
        //    //    {
        //    //        Descricao = "TIPO SERVIÇO " + (i + 1)
        //    //    };

        //    //    //Criar registro
        //    //    repositorio.Criar(d1);

        //    //    d1.Descricao = "TIPO SERVIÇO Alterado.";

        //    //    //Alterar registro
        //    //    repositorio.Alterar(d1);
        //    //}

        //    //Listar todos
        //    var list0 = repositorio.Listar().ToList();
        //    Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar("%Ç%").ToList();
        //    Assert.IsNotNull(list1);

        //    ////Listar pela chave
        //    var primeiroDaLIsta = list0.FirstOrDefault();
        //    if (primeiroDaLIsta == null) return;
        //    var d3 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoServicoId);
        //    Assert.IsNotNull(d3);
        //}

        //[TestMethod]
        //public void TipoEquipamento_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new TipoEquipamentoRepositorio();

        //    //for (var i = 0; i < 6; i++)
        //    //{
        //    //    var d1 = new TipoEquipamento
        //    //    {
        //    //        Descricao = "TIPO EQUIPAMENTO " + (i + 1)
        //    //    };

        //    //    //Criar registro
        //    //    repositorio.Criar(d1);

        //    //    d1.Descricao = "TIPO EQUIPAMENTO Alterado.";

        //    //    //Alterar registro
        //    //    repositorio.Alterar(d1);
        //    //}

        //    //Listar todos
        //    var list0 = repositorio.Listar().ToList();
        //    Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar("%O%").ToList();
        //    Assert.IsNotNull(list1);

        //    ////Listar pela chave
        //    var primeiroDaLIsta = list0.FirstOrDefault();
        //    if (primeiroDaLIsta == null) return;
        //    var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoEquipamentoId);
        //    Assert.IsNotNull(d2);
        //}

        //[TestMethod]
        //public void TipoCombustivel_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new TipoCombustivelRepositorio();

        //    //for (var i = 0; i < 6; i++)
        //    //{
        //    //    var d1 = new TipoCombustivel
        //    //    {
        //    //        Descricao = "TIPO COMBUSTÍVEL " + (i + 1)
        //    //    };

        //    //    //Criar registro
        //    //    repositorio.Criar(d1);

        //    //    d1.Descricao = "TIPO COMBUSTÍVEL Alterado.";

        //    //    //Alterar registro
        //    //    repositorio.Alterar(d1);
        //    //}

        //    //Listar todos
        //    var list0 = repositorio.Listar().ToList();
        //    Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar("%O%").ToList();
        //    Assert.IsNotNull(list1);

        //    ////Listar pela chave
        //    var primeiroDaLIsta = list0.FirstOrDefault();
        //    if (primeiroDaLIsta == null) return;
        //    var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoCombustivelId);
        //    Assert.IsNotNull(d2);
        //}

        //[TestMethod]
        //public void TipoCobranca_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new TipoCobrancaRepositorio();

        //    //for (var i = 0; i < 6; i++)
        //    //{
        //    //    var d1 = new TipoCobranca
        //    //    {
        //    //        Descricao = "TIPO COBRANÇA " + (i + 1)
        //    //    };

        //    //    //Criar registro
        //    //    repositorio.Criar(d1);

        //    //    d1.Descricao = "TIPO COBRANÇA Alterado.";

        //    //    //Alterar registro
        //    //    repositorio.Alterar(d1);
        //    //}

        //    //Listar todos
        //    var list0 = repositorio.Listar().ToList();
        //    Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar("%Ç%").ToList();
        //    Assert.IsNotNull(list0);

        //    ////Listar pela chave
        //    var primeiroDaLIsta = list0.FirstOrDefault();
        //    if (primeiroDaLIsta == null) return;
        //    var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoCobrancaId);
        //    Assert.IsNotNull(d2);
        //}

        //[TestMethod]
        //public void TipoAtividade_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new TipoAtividadeRepositorio();

        //    //for (var i = 0; i < 6; i++)
        //    //{
        //    //    var d1 = new TipoAtividade
        //    //    {
        //    //        Descricao = "TIPO ATIVIDADE " + (i + 1)
        //    //    };

        //    //    //Criar registro
        //    //    repositorio.Criar(d1);

        //    //    d1.Descricao = "TIPO ATIVIDADE Alterado.";

        //    //    //Alterar registro
        //    //    repositorio.Alterar(d1);
        //    //}

        //    //Listar todos
        //    var list0 = repositorio.Listar().ToList();
        //    Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar("%Ç%").ToList();
        //    Assert.IsNotNull(list1);

        //    ////Listar pela chave
        //    var primeiroDaLIsta = list0.FirstOrDefault();
        //    if (primeiroDaLIsta == null) return;
        //    var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoAtividadeId);
        //    Assert.IsNotNull(d2);
        //}

        //[TestMethod]
        //public void TipoAcesso_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new TipoAcessoRepositorio();

        //    //for (var i = 0; i < 6; i++)
        //    //{
        //    //    var d1 = new TipoAcesso
        //    //    {
        //    //        Descricao = "TIPO ACESSO " + (i + 1)
        //    //    };

        //    //    //Criar registro
        //    //    repositorio.Criar(d1);

        //    //    d1.Descricao = "TIPO ACESSO Alterado.";

        //    //    //Alterar registro
        //    //    repositorio.Alterar(d1);
        //    //}

        //    //Listar todos
        //    var list0 = repositorio.Listar().ToList();
        //    Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar("%/%").ToList();
        //    Assert.IsNotNull(list1);

        //    ////Listar pela chave
        //    var primeiroDaLIsta = list0.FirstOrDefault();
        //    if (primeiroDaLIsta == null) return;
        //    var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoAcessoId);
        //    Assert.IsNotNull(d2);
        //}

        //[TestMethod]
        //public void TecnologiaCredencial_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new TecnologiaCredencialRepositorio();

        //    //for (var i = 0; i < 6; i++)
        //    //{
        //    //    var d1 = new TecnologiaCredencial
        //    //    {
        //    //        Descricao = "TECNOLOGIA CREDENCIAL " + (i + 1)
        //    //    };

        //    //    //Criar registro
        //    //    repositorio.Criar(d1);

        //    //    d1.Descricao = "TECNOLOGIA CREDENCIAL Alterado.";

        //    //    //Alterar registro
        //    //    repositorio.Alterar(d1);
        //    //}

        //    //Listar todos
        //    var list0 = repositorio.Listar().ToList();
        //    Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar("%a%").ToList();
        //    Assert.IsNotNull(list1);

        //    ////Listar pela chave
        //    var primeiroDaLIsta = list0.FirstOrDefault();
        //    if (primeiroDaLIsta == null) return;
        //    var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TecnologiaCredencialId);
        //    Assert.IsNotNull(d2);
        //}

        //[TestMethod]
        //public void Status_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new StatusRepositorio();

        //    //for (var i = 0; i < 6; i++)
        //    //{
        //    //    var d1 = new Status
        //    //    {
        //    //        Descricao = "Status " + (i + 1)
        //    //    };

        //    //    //Criar registro
        //    //    repositorio.Criar(d1);

        //    //    d1.Descricao = "Status Alterado.";

        //    //    //Alterar registro
        //    //    repositorio.Alterar(d1);
        //    //}

        //    //Listar todos
        //    var list0 = repositorio.Listar().ToList();
        //    Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar("%a%").ToList();
        //    Assert.IsNotNull(list1);

        //    ////Listar pela chave
        //    var primeiroDaLIsta = list0.FirstOrDefault();
        //    if (primeiroDaLIsta == null) return;
        //    var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.StatusId);
        //    Assert.IsNotNull(d2);
        //}

        //[TestMethod]
        //public void Relatorios_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new RelatoriosRepositorio();

        //    //for (var i = 0; i < 6; i++)
        //    //{
        //    //    var d1 = new Relatorios
        //    //    {
        //    //        Nome = "Relatorio " + (i + 1),
        //    //        NomeArquivoRpt = "file.rpt",
        //    //        ArquivoRpt = "file.txt",
        //    //        Ativo = true
        //    //    };

        //    //    //Criar registro
        //    //    repositorio.Criar(d1);

        //    //    d1.Nome = "Relatorio Alterado." + i * 2;

        //    //    //Alterar registro
        //    //    repositorio.Alterar(d1);
        //    //}

        //    //Listar todos
        //    //var list0 = repositorio.Listar(null).ToList();
        //    //Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar(0, "%A%").ToList();
        //    Assert.IsNotNull(list1);

        //    ////Listar pela chave
        //    //var primeiroDaLIsta = list0.FirstOrDefault();
        //    //if (primeiroDaLIsta == null) return;
        //    //var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.RelatorioId);
        //    //Assert.IsNotNull(d2);
        //}

        //[TestMethod]
        //public void RelatoriosGerenciais_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        //{
        //    var repositorio = new RelatoriosGerenciaisRepositorio();

        //    //for (var i = 0; i < 6; i++)
        //    //{
        //    //    var d1 = new RelatoriosGerenciais
        //    //    {
        //    //        Nome = "Relatorio Gerencial" + (i + 1),
        //    //        NomeArquivoRpt = "file.rpt",
        //    //        ArquivoRpt = "file.txt",
        //    //        Ativo = true
        //    //    };

        //    //    //Criar registro
        //    //    repositorio.Criar(d1);

        //    //    d1.Nome = "Relatorio Gerencial Alterado." + i * 2;

        //    //    //Alterar registro
        //    //    repositorio.Alterar(d1);
        //    //}

        //    //Listar todos
        //    var list0 = repositorio.Listar().ToList();
        //    Assert.IsNotNull(list0);

        //    //Listar Filtrando parâmetros
        //    var list1 = repositorio.Listar("%a%").ToList();
        //    Assert.IsNotNull(list1);

        //    ////Listar pela chave
        //    var primeiroDaLIsta = list0.FirstOrDefault();
        //    if (primeiroDaLIsta == null) return;
        //    var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.RelatorioId);
        //    Assert.IsNotNull(d2);
        //}
    }
}