// ***********************************************************************
// Project: UnitTestImod
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IMOD.Application.Service;
using IMOD.Domain.Entities;
using IMOD.Infra.Repositorios;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace UnitTestImod
{
    [TestClass]
    public class UnitTest1
    {
        private Empresa _empresa;
        private EmpresaSignatario _empresaSignatario;
        private EmpresaContrato _empresaContrato;
        private EmpresaAnexo _empresaAnexo;


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
                Cpf = "277.854.155-10",
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
                TelefoneResp = "711554212",
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

        }


        #region  Metodos


        [Priority(1)]
        [TestMethod]
        [Description("Objetivo cadastrar dados de empresa e seus relacionamentos")]
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
            for (int i = 0; i < 5; i++)
            {
                _empresaSignatario.EmpresaId = _empresa.EmpresaId;
                service.SignatarioService.Criar(_empresaSignatario);
            }

            #endregion

            #region Contrato

            //Cadastrar 5 contratos
            for (int i = 0; i < 5; i++)
            {
                _empresaContrato.EmpresaId = _empresa.EmpresaId;
                service.ContratoService.Criar(_empresaContrato);
            }

            #endregion

            #region Anexos

            //Cadastrar 5 anexos
            for (int i = 0; i < 5; i++)
            {
                _empresaAnexo.EmpresaId = _empresa.EmpresaId;
                service.AnexoService.Criar(_empresaAnexo);
            }

            #endregion


        }


        [TestMethod]
        public void EmpresaCriar_Alterar_Listar_Remover_com_Sucesso()
        {

            Random randNum = new Random();

            var repositorio = new EmpresaRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new Empresa
                {
                    Nome = "RAZÃO SOCIAL Ltda " + i,
                    Apelido = "NOME FANTASIA " + i,
                    Sigla = "EMP",
                    Cnpj = randNum.Next().ToString(),
                    Cep = "44009-730",
                    Endereco = "AVENIDA " + (i + 1) + "º PARQUE",
                    Numero = "229",
                    Complemento = "EDIFÍCIO EMPIRE STATE",
                    Bairro = "NOVO HORIZONTE",
                    MunicipioId = i,
                    EstadoId = i,
                    Telefone = "(73) 3934-411" + i,
                    Email1 = "teste1@teste.com.br",
                    Contato1 = "JOÃO",
                    Telefone1 = "(73) 7569-411" + i,
                    Celular1 = "(73) 98115-987" + i,
                    Email2 = "teste2@teste.com.br",
                    Contato2 = "MARIA",
                    Telefone2 = "(73) 3409-411" + i,
                    Celular2 = "(73) 90984-987" + i,
                    Obs = "Observações...",
                    Responsavel = "JOÃO & MARIA",
                    Logo =
                        "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",
                    InsEst = "97345358",
                    InsMun = "538496500172",
                    Excluida = i,
                    Pendente11 = true,
                    Pendente12 = true,
                    Pendente13 = true,
                    Pendente14 = true,
                    Pendente15 = true,
                    Pendente16 = true,
                    Pendente17 = true
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Sigla = "AAA";
                d1.InsEst = "000(EDIT)";
                d1.Numero = "000(EDIT)";

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().EmpresaId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Nome;
                if (DescFirst == null)
                {
                    return;
                }

                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%", null, null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().EmpresaId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);

        }

        [TestMethod]
        public void EmpresaContrato_insere_listar_arquivo_dados_filestream_com_sucesso()
        {
            var repositorio = new EmpresaContratoRepositorio();
            //Obter array de bytes do arquivo teste
            var arrayByte = File.ReadAllBytes("Arquivos/contrato.pdf");
            //Transformar numa string Base64
            var strBase64 = Convert.ToBase64String(arrayByte);
            for (var i = 0; i < 3; i++)
            {
                var d1 = new EmpresaContrato
                {
                    EmpresaId = i,
                    NumeroContrato = "10" + i,
                    Descricao = "#CONTRATO# " + (i + 1),
                    Emissao = DateTime.Now,
                    Validade = DateTime.Now,
                    Terceirizada = "3",
                    Contratante = "#CONTRATANTE# " + (i + 1),
                    IsencaoCobranca = "I",
                    TipoCobrancaId = i,
                    CobrancaEmpresaId = i,
                    StatusId = 1,
                    Arquivo = strBase64,
                    TipoAcessoId = 1,
                    NomeArquivo = "contrato.pdf",
                    ArquivoBlob = arrayByte
                };

                //CREATE
                repositorio.Criar(d1); //Criar dados
                d1.Descricao = (i + 1) + "#CONTRATO EDIT#." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().EmpresaContratoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }

                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(null, null, "%" + DescFirst + "%", null, null, null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().EmpresaContratoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);

        }

        [TestMethod]
        public void ColabororadorCredencial_Alterar_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();

            var id = repositorio.Listar().ToList().LastOrDefault().ColaboradorCredencialId;

            repositorio.Alterar(new ColaboradorCredencial
            {
                ColaboradorCredencialId = id,
                Fc = 420 * id,
                Emissao = DateTime.Now,
                Validade = DateTime.Now,
                NumeroCredencial = (id + 1) + "#CrED#" + (id + 1),
                Baixa = DateTime.Now
            });
        }

        [TestMethod]
        public void ColabororadorCredencial_Buscar_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();
            var d1 = repositorio.Listar().FirstOrDefault();
            var d2 = repositorio.BuscarPelaChave(d1.ColaboradorCredencialId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void ColabororadorCredencial_ListarColaboradorCredeniasView_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();

            var id1 = repositorio.Listar().ToList().FirstOrDefault().ColaboradorCredencialId;
            var id2 = repositorio.Listar().ToList().FirstOrDefault().CredencialStatusId;
            var id3 = repositorio.Listar().ToList().FirstOrDefault().FormatoCredencialId;

            //ColaboradorCredencialID
            var d2 = repositorio.Listar(id1, 0, 0);
            //CredencialStatusID
            var d3 = repositorio.Listar(0, id2, 0);
            //FormatoCredencialID
            var d4 = repositorio.Listar(0, 0, id3);

            Assert.IsNotNull(d2);
            Assert.IsNotNull(d3);
            Assert.IsNotNull(d4);
        }

        [TestMethod]
        public void ColabororadorAnexo_Buscar_Criar_Alterar_com_sucesso()
        {
            var repositorio = new ColaboradorAnexoRepositorio();

            var d0 = repositorio.Listar().FirstOrDefault();
            var d1 = repositorio.BuscarPelaChave(d0.ColaboradorAnexoId);

            var list = repositorio.Listar().ToList();
            var DescFirst = list.FirstOrDefault().NomeArquivo;

            var list0 = repositorio.Listar(0, "%" + DescFirst + "%").ToList();
            var list1 = repositorio.Listar(d0.ColaboradorId, null).ToList();


            d1.NomeArquivo = "Novo Nome arquivo";
            repositorio.Alterar(d1);
            d1.Descricao = "Descrição Alterada";
            repositorio.Criar(d1);
        }

        [TestMethod]
        public void Cadastrar_ColaboradorEanexos_com_sucesso()
        {
            var repositorio = new ColaboradorAnexoRepositorio();

            var d1 = repositorio.Listar().FirstOrDefault();
            if (d1 == null) return;

            d1.NomeArquivo = "Unit Teste";
            d1.Descricao = "Descricao";
            d1.Arquivo = "Arquivo";


            repositorio.Criar(d1);
        }

        [TestMethod]
        public void ColaboradorCredencialimpresssao_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialimpresssaoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new ColaboradorCredencialimpresssao
                {
                    Cobrar = true,
                    ColaboradorCredencialId = 1 + i,
                    DataImpressao = DateTime.Now
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Cobrar = false;

                //UPDATE
                repositorio.Alterar(d1);
            }
            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().CredencialImpressaoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().ColaboradorCredencialId;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(DescFirst, null, 0).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().CredencialImpressaoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void ColaboradorCursos_com_sucesso()
        {
            var repositorio = new ColaboradorCursoRepositorio();

            for (var i = 0; i < 4; i++)
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
                //CREATE
                repositorio.Criar(d1);

                d1.Arquivo = "Arquivo" + i + " alterado!";
                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().ColaboradorCursoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().NomeArquivo;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(0, 0, "%" + DescFirst + "%", null, null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().ColaboradorCursoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void CredencialMotivo_com_sucesso()
        {
            var repositorio = new CredencialMotivoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new CredencialMotivo
                {
                    CredencialMotivoId = i,
                    Descricao = "Descrição Motivo " + i,
                    Tipo = 1 + i
                };

                //CREATE
                repositorio.Criar(d1);
                d1.Descricao = "Descrição Motivo " + i + "  alterado" + i;

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().CredencialMotivoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(null, "%" + DescFirst + "%", null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().CredencialMotivoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void CredencialStatusRepositorio_com_sucesso()
        {
            var repositorio = new CredencialStatusRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new CredencialStatus
                {
                    CredencialStatusId = i,
                    Descricao = "Credencial status " + i
                };
                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = "Credencial status alterado" + i;

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().CredencialStatusId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(0, "%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().CredencialStatusId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void CursoRepositorio_com_sucesso()
        {
            var repositorio = new CursoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new Curso
                {
                    CursoId = i,
                    Descricao = "Descrição curso " + i
                };
                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = "Descrição curso " + i + " alterado";
                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().CursoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(0, "%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().CursoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void EmpresaAreaAcessoRepositorio_com_sucesso()
        {
            var repositorio = new EmpresaAreaAcessoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new EmpresaAreaAcesso
                {
                    AreaAcessoId = i + 1,
                    EmpresaId = i + 1,
                };
                //CREATE
                repositorio.Criar(d1);

                d1.EmpresaAreaAcessoId = 2;

                //UPDATE
                repositorio.Alterar(d1);
            }

            var list0 = repositorio.Listar();
            var frst = list0.FirstOrDefault();
            //EmpresaAreaAcessoID
            var d2 = repositorio.Listar(frst.EmpresaAreaAcessoId, 0, 0);
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

            for (var i = 0; i < 4; i++)
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
                //CREATE
                repositorio.Criar(d1);

                d1.NumeroContrato = "Contrato Alterado para 99" + i;

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().EmpresaContratoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(null, null, "%" + DescFirst + "%", null, null, null, null, null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().EmpresaContratoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void EmpresaEquipamentoRepositorio_com_sucesso()
        {
            var repositorio = new EmpresaEquipamentoRepositorio();

            for (var i = 0; i < 4; i++)
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

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = "Descrição alterada" + i;

                //UPDATE
                repositorio.Alterar(d1);
            }


            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().EmpresaEquipamentoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%", null, null, null, null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().EmpresaEquipamentoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void EmpresaLayoutCrachaRepositorio_com_sucesso()
        {
            var repositorio = new EmpresaLayoutCrachaRepositorio();
            for (var i = 0; i < 3; i++)
            {
                var d1 = new EmpresaLayoutCracha
                {
                    EmpresaId = 1,
                    EmpresaLayoutCrachaId = i,
                    LayoutCrachaId = 1
                };

                //CREATE
                repositorio.Criar(d1);

                d1.LayoutCrachaId = 2;

                //UPDATE
                repositorio.Alterar(d1);
            }

            var id1 = repositorio.Listar().ToList().FirstOrDefault().EmpresaLayoutCrachaId;
            var id2 = repositorio.Listar().ToList().FirstOrDefault().EmpresaId;
            var id3 = repositorio.Listar().ToList().FirstOrDefault().LayoutCrachaId;


            var list = repositorio.Listar();

            //EmpresaLayoutCrachaID
            var d2 = repositorio.Listar(id1, 0, 0);
            //EmpresaID
            var d3 = repositorio.Listar(0, id2, 0);
            //LayoutCrachaID
            var d4 = repositorio.Listar(0, 0, id3);

            Assert.IsNotNull(d2);
            Assert.IsNotNull(d3);
            Assert.IsNotNull(d4);
        }

        [TestMethod]
        public void EmpresaSeguroRepositorio_com_sucesso()
        {
            var repositorio = new EmpresaSeguroRepositorio();

            for (var i = 0; i < 4; i++)
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

                //CREATE
                repositorio.Criar(d1);

                d1.NomeSeguradora = "Nome Seguradora alterado" + i;

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().EmpresaSeguroId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().NomeSeguradora;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%", null, null, null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().EmpresaSeguroId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void LayoutCrachaRepositorio_com_sucesso()
        {
            var repositorio = new LayoutCrachaRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new LayoutCracha
                {
                    Nome = "Crachá" + i,
                    LayoutCrachaGuid = "#",
                    Valor = i,
                    LayoutRpt = ""
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Nome = "Crachá Alterado" + i;
                d1.LayoutRpt = "##";

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().LayoutCrachaId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Nome;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%", null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().LayoutCrachaId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void PendenciaRepositorio_com_sucesso()
        {
            var repositorio = new PendenciaRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new Pendencia
                {
                    PendenciaId = i,
                    TipoPendenciaId = 1,
                    Descricao = "Descrição Pendencias",
                    DataLimite = DateTime.Now,
                    Impeditivo = true,
                    ColaboradorId = 1,
                    EmpresaId = i,
                    VeiculoId = i
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = "Alteração Descrição Pendencias" + i;

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().PendenciaId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%", null, null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().PendenciaId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void ColabororadorEnpresa_Buscar_Criar_Alterar_com_sucesso()
        {
            var repositorio = new ColaboradorEmpresaRepositorio();

            var d1 = repositorio.Listar().FirstOrDefault();
            if (d1 == null) return;

            for (var i = 0; i < 4; i++)
            {
                var entity = repositorio.BuscarPelaChave(d1.ColaboradorEmpresaId);
                entity.Cargo = "Cargo " + i;

                //UPDATE
                repositorio.Alterar(d1);
                entity.Matricula = "0000" + i;

                //CREATE
                repositorio.Criar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().ColaboradorEmpresaId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Cargo;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(null, null, "%" + DescFirst + "%", null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().ColaboradorEmpresaId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);

        }

        [TestMethod]
        public void Colabororador_Criar_Listar_com_sucesso()
        {
            var repositorio = new ColaboradorRepositorio();

            //Criar Colaborador
            var cpf = "69885625898";
            var nome = "Valnei Filho";

            #region Cadastrar Colaborador

            repositorio.Criar(new Colaborador
            {
                Nome = nome,
                Apelido = "Marinpietri",
                DataNascimento = DateTime.Today.AddYears(-26),
                NomePai = "Valnei",
                NomeMae = "Veronice",
                Nacionalidade = "Brasil",
                Foto = null,
                EstadoCivil = "Solteiro",
                Cpf = cpf,
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
            });

            #endregion


            var list1 = repositorio.Listar(0, ""); //Listar todos
            Assert.IsNotNull(list1);
            var list2 = repositorio.Listar(); //Listar todos
            Assert.IsNotNull(list2);
            var list3 = repositorio.Listar(null); //Listar todos
            Assert.IsNotNull(list3);

            var CpfFirst = list1.FirstOrDefault().Cpf;

            var list4 = repositorio.Listar(null, CpfFirst, null); //Listar por cpf
            Assert.IsNotNull(list4);

            var NomeFirst = list1.FirstOrDefault().Nome;

            var list5 = repositorio.Listar(null, null, NomeFirst); //Listar por nome
            Assert.IsNotNull(list5);
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
            var repositorio = new ColaboradorRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var colaborador = new Colaborador
                {
                    Nome = "Colaborador " + (i + 1),
                    Apelido = "Collab " + (i + 1),
                    DataNascimento = DateTime.Today.AddYears(-26),
                    NomePai = "NomePai " + (i + 1),
                    NomeMae = "NomeMae " + (i + 1),
                    Nacionalidade = "Nacionalidade" + (i + 1),
                    Foto = null,
                    EstadoCivil = "SOLTEIRO",
                    Cpf = "818.033.740-55",
                    Rg = "44.644.119-3",
                    RgEmissao = DateTime.Today.AddYears(-10),
                    RgOrgLocal = "SSP",
                    RgOrgUf = "BA",
                    Passaporte = "PJ8830202",
                    PassaporteValidade = DateTime.Today.AddYears(3),
                    Rne = "RN4493",
                    TelefoneFixo = "(71) 3581-4913",
                    TelefoneCelular = "(71) 98879-2442",
                    Email = "collaborador" + (i + 1) + "@email.com.br",
                    ContatoEmergencia = "ContatoEmergencia " + (i + 1),
                    TelefoneEmergencia = "TelefoneEmergencia " + (i + 1),
                    Cep = "41925-580",
                    Endereco = "Rua Baixa da Paz",
                    Numero = "947",
                    Complemento = "AP01",
                    Bairro = "Santa Cruz",
                    MunicipioId = 1,
                    EstadoId = 1,
                    Motorista = true,
                    CnhCategoria = "AB",
                    Cnh = i + "0000" + i,
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

                repositorio.Criar(colaborador);
            }
        }

        [TestMethod]
        public void Veiculo_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new Veiculo
                {
                    Descricao = "# VEÍCULO #" + i + "#",
                    Placa_Identificador = i + "XK-" + i + "00" + i,
                    Frota = "A" + i,
                    Patrimonio = "PPT-" + i + "00" + i,
                    Marca = "Hyundai",
                    Modelo = i + "H Starex SVX 2.6 85cv Diesel",
                    Tipo = "VEICULO",
                    Cor = "Cinza",
                    Ano = "201" + i,
                    EstadoId = 1,
                    MunicipioId = 1,
                    Serie_Chassi = "",
                    CombustivelId = 1,
                    Altura = i + ".669mm",
                    Comprimento = i + ".152mm",
                    Largura = i + ".768mm",
                    TipoEquipamentoVeiculoId = 1,
                    Renavam = i + "8671439987" + i,
                    Foto = "",
                    Excluida = 1,
                    StatusId = 1,
                    TipoAcessoId = 1,
                    DescricaoAnexo = "ANEXO " + i,
                    NomeArquivoAnexo = "ANEXO " + i,
                    ArquivoAnexo = "ANEXO " + i,
                    Pendente31 = false,
                    Pendente32 = false,
                    Pendente33 = false,
                    Pendente34 = false
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = i + "VEÍCULO #100" + i + "#";
                d1.Cor = i + "#PRETO#" + i;

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = repositorio.Listar().ToList().FirstOrDefault().EquipamentoVeiculoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = repositorio.Listar().ToList().FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().EquipamentoVeiculoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);

        }

        [TestMethod]
        public void VeiculoSeguros_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoSeguroRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new VeiculoSeguro
                {
                    NomeSeguradora = "SEGURADORA " + (i + 1),
                    NumeroApolice = i + "#APÓLICE# " + (i + 1),
                    ValorCobertura = i * 1000,
                    VeiculoId = 1,
                    Arquivo = i + "#ARQUIVO# " + (i + 1),
                    NomeArquivo = i + "#ARQUIVO# " + (i + 1),
                    Emissao = DateTime.Now,
                    Validade = DateTime.Today.AddYears(i + 1)
                };

                //CREATE
                repositorio.Criar(d1);

                d1.NomeSeguradora = i + "#SEGURADORA EDIT#(" + (i + 1) + ")";
                d1.ValorCobertura = 420 * i;

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            //Listar Filtrando parâmetros
            var IdFirst = repositorio.Listar().ToList().FirstOrDefault().VeiculoSeguroId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = repositorio.Listar().ToList().FirstOrDefault().NomeSeguradora;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(0, "%" + DescFirst + "%", null).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().VeiculoSeguroId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void VeiculoEquipTipoServico_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoEquipTipoServicoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new VeiculoEquipTipoServico
                {
                    EquipamentoVeiculoTipoServicoId = 1,
                    EquipamentoVeiculoId = i + 1,
                    TipoServicoId = i + 1
                };

                //CREATE
                repositorio.Criar(d1);

                d1.EquipamentoVeiculoId = (i + 1) * 2;
                d1.TipoServicoId = (i + 1) * 2;

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().EquipamentoVeiculoTipoServicoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = repositorio.Listar().ToList().FirstOrDefault().TipoServicoId;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro
                var list2 = repositorio.Listar(0, DescFirst).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = repositorio.Listar().ToList().LastOrDefault().EquipamentoVeiculoTipoServicoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void VeiculoEmpresa_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoEmpresaRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new VeiculoEmpresa
                {
                    VeiculoId = 1,
                    EmpresaId = 1,
                    EmpresaContratoId = 1,
                    Cargo = "CARGO " + i,
                    Matricula = "MATRICULA " + i,
                    Ativo = true
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Cargo = i + " CARGO EDIT";
                d1.Matricula = i + " MATRICULA EDIT";

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().VeiculoEmpresaId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = repositorio.Listar().ToList().FirstOrDefault().Cargo;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(0, "%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }
            //DELETE
            var IdLast = list0.LastOrDefault().VeiculoEmpresaId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void VeiculoCredencial_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoCredencialRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new VeiculoCredencial
                {
                    VeiculoEmpresaId = 1,
                    TecnologiaCredencialId = 1,
                    TipoCredencialId = 1,
                    LayoutCrachaId = 1,
                    FormatoCredencialId = 1,
                    NumeroCredencial = i + "#CREDENCIAL#" + i,
                    Fc = 111,
                    Emissao = DateTime.Now,
                    Validade = DateTime.Now,
                    CredencialStatusId = 1,
                    CardHolderGuid = "",
                    CredencialGuid = "",
                    VeiculoPrivilegio1Id = 1,
                    VeiculoPrivilegio2Id = 1,
                    Ativa = true,
                    Colete = "Cl.1",
                    CredencialmotivoId = 1,
                    Baixa = DateTime.Now,
                    Impressa = true
                };

                //CREATE
                repositorio.Criar(d1);

                d1.NumeroCredencial = i + "#CARGO#" + i * 2;
                d1.Colete = i + "l.1";

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().VeiculoCredencialId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = repositorio.Listar().ToList().FirstOrDefault().NumeroCredencial;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(null, null, null, null, "%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = list0.LastOrDefault().VeiculoCredencialId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void VeiculoCredencialimpressao_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoCredencialimpressaoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new VeiculoCredencialimpressao
                {
                    VeiculoCredencialId = i + 100,
                    DataImpressao = DateTime.Now,
                    Cobrar = true
                };

                //CREATE
                repositorio.Criar(d1);

                d1.DataImpressao = DateTime.Today.AddDays(i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }


            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = repositorio.Listar().ToList().FirstOrDefault().CredencialImpressaoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = repositorio.Listar().ToList().FirstOrDefault().VeiculoCredencialId;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(DescFirst).ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = list0.LastOrDefault().CredencialImpressaoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void VeiculoAnexo_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoAnexoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new VeiculoAnexo
                {
                    VeiculoId = i + 100,
                    Descricao = i + " ANEXO" + i,
                    NomeArquivo = i + " ANEXO" + i,
                    Arquivo = i + "#arquivo#" + i + ".file"
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = i + "#Anexo Alterado.#" + i;

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().VeiculoAnexoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar(0, "%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            var IdLast = list0.LastOrDefault().VeiculoAnexoId;
            var list3 = repositorio.BuscarPelaChave(IdLast);
            Assert.IsNotNull(list3);
            repositorio.Remover(list3);
        }

        [TestMethod]
        public void TipoServico_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoServicoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new TipoServico
                {
                    Descricao = "#NOVO TIPO SERVIÇO#"
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = (i + 1) + "#TIPO SERVIÇO EDIT#." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().TipoServicoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            for (var j = 0; j < 4; j++)
            {
                var IdLast = repositorio.Listar().ToList().LastOrDefault().TipoServicoId;
                var list3 = repositorio.BuscarPelaChave(IdLast);
                Assert.IsNotNull(list3);
                repositorio.Remover(list3);
            }

        }

        [TestMethod]
        public void TipoEquipamento_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoEquipamentoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new TipoEquipamento
                {
                    Descricao = "TIPO EQUIPAMENTO " + (i + 1)
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = (i + 1) + "#TIPO EQUIPAMENTO EDIT#." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().TipoEquipamentoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            for (var j = 0; j < 4; j++)
            {
                var IdLast = repositorio.Listar().ToList().LastOrDefault().TipoEquipamentoId;
                var list3 = repositorio.BuscarPelaChave(IdLast);
                Assert.IsNotNull(list3);
                repositorio.Remover(list3);
            }
        }

        [TestMethod]
        public void TipoCombustivel_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoCombustivelRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new TipoCombustivel
                {
                    Descricao = "TIPO COMBUSTÍVEL " + (i + 1)
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = (i + 1) + "TIPO COMBUSTÍVEL EDIT." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().TipoCombustivelId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            for (var j = 0; j < 4; j++)
            {
                var IdLast = repositorio.Listar().ToList().LastOrDefault().TipoCombustivelId;
                var list3 = repositorio.BuscarPelaChave(IdLast);
                Assert.IsNotNull(list3);
                repositorio.Remover(list3);
            }
        }

        [TestMethod]
        public void TipoCobranca_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoCobrancaRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new TipoCobranca
                {
                    Descricao = "TIPO COBRANÇA "
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = (i + 1) + "TIPO COBRANÇA EDIT." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().TipoCobrancaId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            for (var j = 0; j < 4; j++)
            {
                var IdLast = repositorio.Listar().ToList().LastOrDefault().TipoCobrancaId;
                var list3 = repositorio.BuscarPelaChave(IdLast);
                Assert.IsNotNull(list3);
                repositorio.Remover(list3);
            }
        }

        [TestMethod]
        public void TipoAtividade_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoAtividadeRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new TipoAtividade
                {
                    Descricao = "TIPO ATIVIDADE "
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = (i + 1) + "#TIPO ATIVIDADE EDIT#." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().TipoAtividadeId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            for (var j = 0; j < 4; j++)
            {
                var IdLast = repositorio.Listar().ToList().LastOrDefault().TipoAtividadeId;
                var list3 = repositorio.BuscarPelaChave(IdLast);
                Assert.IsNotNull(list3);
                repositorio.Remover(list3);
            }
        }

        [TestMethod]
        public void TipoAcesso_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoAcessoRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new TipoAcesso
                {
                    Descricao = "TIPO ACESSO."
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = (i + 1) + "#TIPO ACESSO EDIT#." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().TipoAcessoId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            for (var j = 0; j < 4; j++)
            {
                var IdLast = repositorio.Listar().ToList().LastOrDefault().TipoAcessoId;
                var list3 = repositorio.BuscarPelaChave(IdLast);
                Assert.IsNotNull(list3);
                repositorio.Remover(list3);
            }
        }

        [TestMethod]
        public void TecnologiaCredencial_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TecnologiaCredencialRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new TecnologiaCredencial
                {
                    Descricao = "TECNOLOGIA CREDENCIAL." + (i + 1)
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = (i + 1) + "#TECNOLOGIA CREDENCIAL EDIT#." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().TecnologiaCredencialId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            for (var j = 0; j < 4; j++)
            {
                var IdLast = repositorio.Listar().ToList().LastOrDefault().TecnologiaCredencialId;
                var list3 = repositorio.BuscarPelaChave(IdLast);
                Assert.IsNotNull(list3);
                repositorio.Remover(list3);
            }
        }

        [TestMethod]
        public void Status_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new StatusRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new Status
                {
                    Descricao = "Status " + (i + 1)
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Descricao = (i + 1) + "#Status EDIT#." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().StatusId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Descricao;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            for (var j = 0; j < 4; j++)
            {
                var IdLast = repositorio.Listar().ToList().LastOrDefault().StatusId;
                var list3 = repositorio.BuscarPelaChave(IdLast);
                Assert.IsNotNull(list3);
                repositorio.Remover(list3);
            }
        }

        [TestMethod]
        public void Relatorios_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new RelatoriosRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new Relatorios
                {
                    Nome = "Relatorio " + (i + 1),
                    NomeArquivoRpt = "file.rpt",
                    ArquivoRpt = "file.txt",
                    Ativo = true
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Nome = (i + 1) + "#Relatorio EDIT#." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().RelatorioId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Nome;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            for (var j = 0; j < 4; j++)
            {
                var IdLast = repositorio.Listar().ToList().LastOrDefault().RelatorioId;
                var list3 = repositorio.BuscarPelaChave(IdLast);
                Assert.IsNotNull(list3);
                repositorio.Remover(list3);
            }
        }

        [TestMethod]
        public void RelatoriosGerenciais_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new RelatoriosGerenciaisRepositorio();

            for (var i = 0; i < 4; i++)
            {
                var d1 = new RelatoriosGerenciais
                {
                    Nome = "Relatorio Gerencial" + (i + 1),
                    NomeArquivoRpt = "file.rpt",
                    ArquivoRpt = "file.txt",
                    Ativo = true
                };

                //CREATE
                repositorio.Criar(d1);

                d1.Nome = (i + 1) + "#Relatorio Gerencial#." + (i + 1);

                //UPDATE
                repositorio.Alterar(d1);
            }

            //READ
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            var IdFirst = list0.FirstOrDefault().RelatorioId;
            if (IdFirst == null)
            {
                return;
            }
            else
            {
                //Listar pela chave
                var list1 = repositorio.BuscarPelaChave(IdFirst);
                Assert.IsNotNull(list1);

                var DescFirst = list0.FirstOrDefault().Nome;
                if (DescFirst == null)
                {
                    return;
                }
                //Listar Filtrando parâmetro(s)
                var list2 = repositorio.Listar("%" + DescFirst + "%").ToList();
                Assert.IsNotNull(list2);
            }

            //DELETE
            for (var j = 0; j < 4; j++)
            {
                var IdLast = repositorio.Listar().ToList().LastOrDefault().RelatorioId;
                var list3 = repositorio.BuscarPelaChave(IdLast);
                Assert.IsNotNull(list3);
                repositorio.Remover(list3);
            }
        }

        #endregion
    }
}