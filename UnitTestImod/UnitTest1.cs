// ***********************************************************************
// Project: UnitTestImod
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Linq;
using IMOD.Domain.Entities;
using IMOD.Infra.Repositorios;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace UnitTestImod
{
    [TestClass]
    public class UnitTest1
    {
        #region  Metodos

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
            var d2 = repositorio.BuscarPelaChave(d1.ColaboradorCredencialId);
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
        public void ColabororadorAnexo_Buscar_Criar_Alterar_com_sucesso()
        {
            var repositorio = new ColaboradorAnexoRepositorio();

            var d0 = repositorio.Listar().FirstOrDefault();
            var d1 = repositorio.BuscarPelaChave(d0.ColaboradorAnexoId);

            var list0 = repositorio.Listar(0, "%arq%").ToList();
            var list1 = repositorio.Listar(0, d0.ColaboradorId).ToList();


            d1.NomeArquivo = "Novo Nome arquivo";
            repositorio.Alterar(d1);
            d1.Descricao = "Descrição Alterada";
            repositorio.Criar(d1);
        }

        [TestMethod]
        public void Cadastrar_ColaboradorEanexos_com_sucesso()
        {
            var repositorio = new ColaboradorRepositorio();
            var d1 = repositorio.Listar().FirstOrDefault();
            if (d1 == null) return;
            d1.Nome = "Alexandre Unit Teste";

            var anexos = new List<ColaboradorAnexo>();
            anexos.Add(new ColaboradorAnexo
            {
                NomeArquivo = "Arquivo Unit Test",
                Descricao = "Descricao",
                Arquivo = "Arquivo"
            });

            repositorio.CriarAnexos(d1, anexos);
        }

        [TestMethod]
        public void ColaboradorCredencialimpresssao_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialimpresssaoRepositorio();
            for (var i = 0; i < 3; i++)
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
            for (var i = 0; i < 3; i++)
            {
                var d1 = new ColaboradorCurso
                {
                    Arquivo = "Arquivo" + i,
                    ColaboradorId = i,
                    Controlado = true,
                    CursoId = i,
                    NomeArquivo = "file" + i + ".arq",
                    Validade = DateTime.Today.AddYears(i),
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
            for (var i = 0; i < 3; i++)
            {
                var d1 = new CredencialMotivo
                {
                    CredencialMotivoId = i,
                    Descricao = "Descrição Motivo " + i,
                    Tipo = 1 + i
                };
                repositorio.Criar(d1);

                d1.Descricao = "Descrição Motivo " + i + "  alterado" + i;
                repositorio.Alterar(d1);
            }
            var list0 = repositorio.Listar();
            var list1 = repositorio.Listar(0, "%ADA%", 0).ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void CredencialStatusRepositorio_com_sucesso()
        {
            var repositorio = new CredencialStatusRepositorio();
            for (var i = 0; i < 3; i++)
            {
                var d1 = new CredencialStatus
                {
                    CredencialStatusId = i,
                    Descricao = "Credencial status " + i
                };
                repositorio.Criar(d1);

                d1.Descricao = "Credencial status alterado" + i;
                repositorio.Alterar(d1);
            }
            var list = repositorio.Listar(); //CursoRepositorio
        }

        [TestMethod]
        public void CursoRepositorio_com_sucesso()
        {
            var repositorio = new CursoRepositorio();
            for (var i = 0; i < 3; i++)
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
            for (var i = 0; i < 3; i++)
            {
                var d1 = new EmpresaAreaAcesso
                {
                    AreaAcessoId = i + 1,
                    EmpresaId = i + 1,
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
            for (var i = 0; i < 3; i++)
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
            for (var i = 0; i < 3; i++)
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
            for (var i = 0; i < 3; i++)
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
            for (var i = 0; i < 3; i++)
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
            for (var i = 0; i < 3; i++)
            {
                var d1 = new LayoutCracha
                {
                    LayoutCrachaId = i,
                    Nome = "Nome do cracha" + i,
                    LayoutCrachaGuid = "",
                    Valor = i
                };
                repositorio.Criar(d1);

                d1.Nome = "Nome do cracha alterado" + i;
                repositorio.Alterar(d1);
            }
            var list0 = repositorio.Listar();
            var list1 = repositorio.Listar("%#%", null).ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void PendenciaRepositorio_com_sucesso()
        {
            var repositorio = new PendenciaRepositorio();
            for (var i = 0; i < 3; i++)
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
                repositorio.Criar(d1);

                d1.Descricao = "Aleração Descrição Pendencias" + i;
                repositorio.Alterar(d1);
            }
            var list0 = repositorio.Listar();
            var list1 = repositorio.Listar("%!%", null, null).ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);

        }

        [TestMethod]
        public void ColabororadorEnpresa_Buscar_Criar_Alterar_com_sucesso()
        {
            var repositorio = new ColaboradorEmpresaRepositorio();
            for (var i = 0; i < 3; i++)
            {
                var d1 = repositorio.BuscarPelaChave(5);
                d1.Cargo = "Cargo " + i;

                repositorio.Alterar(d1);
                d1.Matricula = "0000" + i;

                repositorio.Criar(d1);
            }

            var list0 = repositorio.Listar();
            var list1 = repositorio.Listar(1, null, null).ToList();
            var list2 = repositorio.Listar(null, "%X%", null).ToList();
            var list3 = repositorio.Listar(null, null, "%-%").ToList();

            Assert.IsNotNull(list0);
            Assert.IsNotNull(list1);

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
            var list4 = repositorio.Listar(null, cpf, null); //Listar por cpf
            Assert.IsNotNull(list4);
            var list5 = repositorio.Listar(null, null, "Suzuki"); //Listar por nome
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

            for (var i = 0; i < 3; i++)
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

            for (var i = 0; i < 3; i++)
            {
                var d1 = new Veiculo
                {
                    Descricao = "VEÍCULO (" + i + ")",
                    Placa_Identificador = "HSH-" + i + "00" + i,
                    Frota = "A" + i,
                    Patrimonio = "PAT-" + i + "00" + i,
                    Marca = "Renault",
                    Modelo = "Hi-Flex 1.6 16V 5p",
                    Tipo = "UTILITARIO " + i,
                    Cor = "Amarelo",
                    Ano = "201" + i,
                    EstadoID = 1,
                    MunicipioID = 1,
                    Serie_Chassi = "",
                    CombustivelID = 1,
                    Altura = "1.669 mm",
                    Comprimento = "4.152 mm",
                    Largura = "1.768 mm",
                    TipoEquipamentoVeiculoID = 1,
                    Renavam = "9773970473" + i,
                    Foto = "",
                    Excluida = 1,
                    StatusID = 1,
                    TipoAcessoID = 1,
                    DescricaoAnexo = "ANEXO " + i,
                    NomeArquivoAnexo = "ANEXO " + i,
                    ArquivoAnexo = "ANEXO " + i,
                    Pendente31 = false,
                    Pendente32 = false,
                    Pendente33 = false,
                    Pendente34 = false
                };

                repositorio.Criar(d1);

                d1.Descricao = "VEÍCULO ALTERADO(" + i + 1 + ")";
                d1.Cor = "6LACK";

                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);
            //Listar Filtrando parâmetro(s)
            var list1 = repositorio.Listar(0, "%CULO%" + "%").ToList();
            Assert.IsNotNull(list1);
            //Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
        }

        [TestMethod]
        public void VeiculoSeguros_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoSeguroRepositorio();

            for (var i = 0; i < 2; i++)
            {
                var d1 = new VeiculoSeguro
                {
                    NomeSeguradora = "SEGURADORA " + (i + 1),
                    NumeroApolice = "APÓLICE " + (i + 1),
                    ValorCobertura = i * 1000,
                    VeiculoId = 1,
                    Arquivo = "ARQUIVO " + (i + 1),
                    NomeArquivo = "ARQUIVO " + (i + 1),
                    Emissao = DateTime.Now,
                    Validade = DateTime.Today.AddYears(i + 1)
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.NomeSeguradora = "SEGURADORA ALTERADO(" + (i + 1) + ")";
                d1.ValorCobertura = 420;

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);
            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%0" + "%", 0).ToList();
            Assert.IsNotNull(list1);
            //Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var veiculoseguro = repositorio.BuscarPelaChave(primeiroDaLIsta.VeiculoSeguroId);
            Assert.IsNotNull(veiculoseguro);
        }

        [TestMethod]
        public void VeiculoEquipTipoServico_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoEquipTipoServicoRepositorio();

            for (var i = 0; i < 2; i++)
            {
                var d1 = new VeiculoEquipTipoServico
                {
                    VeiculoTipoServicoId = 1,
                    VeiculoId = i + 1,
                    TipoServicoId = i + 1
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.VeiculoId = (i + 1) * 2;
                d1.TipoServicoId = (i + 1) * 2;

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);
            //Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var veiculoequiptipservico = repositorio.BuscarPelaChave(primeiroDaLIsta.VeiculoTipoServicoId);
            Assert.IsNotNull(veiculoequiptipservico);
        }

        [TestMethod]
        public void VeiculoEmpresa_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoEmpresaRepositorio();

            for (var i = 0; i < 2; i++)
            {
                var d1 = new VeiculoEmpresa
                {
                    VeiculoId = 1,
                    EmpresaId = 1,
                    EmpresaContratoId = 1,
                    Cargo = "CARGO 1",
                    Matricula = "MATRICULA1",
                    Ativo = true
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Cargo = "CARGO EDITED";
                d1.Matricula = "MATRICULA EDITED";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%CAR%", null).ToList();
            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void VeiculoCredencial_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoCredencialRepositorio();

            for (var i = 0; i < 2; i++)
            {
                var d1 = new VeiculoCredencial
                {
                    VeiculoEmpresaId = 1,
                    TecnologiaCredencialId = 1,
                    TipoCredencialId = 1,
                    LayoutCrachaId = 1,
                    FormatoCredencialId = 1,
                    NumeroCredencial = "CREDENCIAL " + i,
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

                //Criar registro
                repositorio.Criar(d1);

                d1.NumeroCredencial = "CARGO " + i * 2;
                d1.Colete = "Cl.1";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%4%", 0).ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.VeiculoCredencialId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void VeiculoCredencialimpressao_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoCredencialimpressaoRepositorio();

            for (var i = 0; i < 3; i++)
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
            if (primeiroDaLIsta == null) return;
            var d3 = repositorio.BuscarPelaChave(primeiroDaLIsta.CredencialImpressaoId);
            Assert.IsNotNull(d3);
        }

        [TestMethod]
        public void VeiculoAnexo_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoAnexoRepositorio();

            for (var i = 0; i < 3; i++)
            {
                var d1 = new VeiculoAnexo
                {
                    VeiculoId = i + 100,
                    Descricao = "ANEXO" + i,
                    NomeArquivo = "ANEXO" + i,
                    Arquivo = "arquivo" + i + ".file"
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Descricao = "Anexo Alterado.";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%XI%").ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d3 = repositorio.BuscarPelaChave(primeiroDaLIsta.VeiculoAnexoId);
            Assert.IsNotNull(d3);
        }

        [TestMethod]
        public void TipoServico_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoServicoRepositorio();

            for (var i = 0; i < 3; i++)
            {
                var d1 = new TipoServico
                {
                    Descricao = "TIPO SERVIÇO " + (i + 1)
                };

                //Criar registro
                repositorio.Criar(d1);

                d1.Descricao = "TIPO SERVIÇO Alterado.";

                //Alterar registro
                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();
            Assert.IsNotNull(list0);

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%Ç%").ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d3 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoServicoId);
            Assert.IsNotNull(d3);
        }

        [TestMethod]
        public void TipoEquipamento_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoEquipamentoRepositorio();

            for (var i = 0; i < 3; i++)
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

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%O%").ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoEquipamentoId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void TipoCombustivel_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoCombustivelRepositorio();

            for (var i = 0; i < 3; i++)
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

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%O%").ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoCombustivelId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void TipoCobranca_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoCobrancaRepositorio();

            for (var i = 0; i < 3; i++)
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

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%Ç%").ToList();
            Assert.IsNotNull(list0);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoCobrancaId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void TipoAtividade_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoAtividadeRepositorio();

            for (var i = 0; i < 3; i++)
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

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%Ç%").ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoAtividadeId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void TipoAcesso_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TipoAcessoRepositorio();

            for (var i = 0; i < 3; i++)
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

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%/%").ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TipoAcessoId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void TecnologiaCredencial_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new TecnologiaCredencialRepositorio();

            for (var i = 0; i < 3; i++)
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

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%a%").ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.TecnologiaCredencialId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void Status_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new StatusRepositorio();

            for (var i = 0; i < 3; i++)
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

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%a%").ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.StatusId);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void Relatorios_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new RelatoriosRepositorio();

            for (var i = 0; i < 3; i++)
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
            //var list0 = repositorio.Listar(null).ToList();
            //Assert.IsNotNull(list0);

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(0, "%A%").ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            //var primeiroDaLIsta = list0.FirstOrDefault();
            //if (primeiroDaLIsta == null) return;
            //var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.RelatorioId);
            //Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void RelatoriosGerenciais_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new RelatoriosGerenciaisRepositorio();

            for (var i = 0; i < 3; i++)
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

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%a%").ToList();
            Assert.IsNotNull(list1);

            ////Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var d2 = repositorio.BuscarPelaChave(primeiroDaLIsta.RelatorioId);
            Assert.IsNotNull(d2);
        }

        #endregion
    }
}