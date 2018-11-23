using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using IMOD.Domain.Entities;
using IMOD.Infra.Repositorios;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestImod
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ColabororadorCredencial_Alterar_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();
            repositorio.Alterar(new ColaboradorCredencial
            {
                ColaboradorCredencialId = 1,
                NumeroCredencial = "Valnei Filho"
            });

        }

        [TestMethod]
        public void ColabororadorCredencial_Buscar_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();
            var d1 = repositorio.BuscarPelaChave(12);
            Assert.IsNotNull(d1);

        }

        [TestMethod]
        public void ColabororadorCredencial_ListarColaboradorCredeniasView_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();
            var d2 = repositorio.Listar(0, 0, 0);
        }

        [TestMethod]
        public void ColabororadorAnexo_Buscar_Criar_Alterar_com_sucesso()
        {
            var repositorio = new ColaboradorAnexoRepositorio();
            var d1 = repositorio.BuscarPelaChave(3);
            d1.NomeArquivo = "Novo nome arquivo";
            repositorio.Alterar(d1);
            d1.Descricao = "Descricao Alterada";
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


        //[TestMethod]
        //public void VeiculoSeguroCadastrar_com_sucesso()
        //{

        //    var repositorio = new VeiculoSeguroRepositorio();

        //    for (int i = 0; i < 30; i++)
        //    {
        //        var d1 = new VeiculoSeguro
        //        {

        //            Arquivo = "",
        //            Emissao = DateTime.Now,
        //            NomeArquivo = "Nome arquivo",
        //            NumeroApolice = "Numero apolice",
        //            NomeSeguradora = "Nome seguro",
        //            Validade = DateTime.Now,

        //            ValorCobertura = 1

        //        };
        //        repositorio.Criar(d1);
        //    }

        //}

        [TestMethod]
        public void ColabororadorEnpresa_Buscar_Criar_Alterar_com_sucesso()
        {
            var repositorio = new ColaboradorEmpresaRepositorio();
            var d1 = repositorio.BuscarPelaChave(8);
            d1.Cargo = "Cargo X";
            repositorio.Alterar(d1);
            d1.Matricula = "0001";
            int key;
            repositorio.Criar(d1);
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
            
            var list1 = repositorio.Listar(0, "");//Listar todos
            Assert.IsNotNull(list1);
                var list2 = repositorio.Listar();//Listar todos
            Assert.IsNotNull(list2);
                var list3 = repositorio.Listar(null);//Listar todos
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

            for (int i = 0; i < 30; i++)
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
                    Pendente25 = false,
                };

                repositorio.Criar(colaborador);
            }
        }

        [TestMethod]
        public void Veiculo_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculosRepositorio();

            for (int i = 0; i < 5; i++)
            {
                var d1 = new Veiculos
                {
                    Descricao = "SANDERO (" + i + ")",
                    PlacaIdentificador = "HSH-" + i + "00" + i,
                    Frota = "A" + i,
                    Patrimonio = "PAT-" + i + "00" + i,
                    Marca = "Renault",
                    Modelo = "STEPWAY Hi-Flex 1.6 16V 5p",
                    Tipo = "UTILITARIO " + i,
                    Cor = "Amarelo",
                    Ano = "201" + i,
                    EstadoId = 1,
                    MunicipioId = 1,
                    SerieChassi = "",
                    CombustivelId = 1,
                    Altura = "1.669 mm",
                    Comprimento = "4.152 mm",
                    Largura = "1.768 mm",
                    TipoEquipamentoVeiculoId = 1,
                    Renavam = "9773970473" + i,
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

                repositorio.Criar(d1);

                d1.Descricao = "SANDERO ALTERADO(" + i + 1 + ")";
                d1.Cor = "White";

                repositorio.Alterar(d1);
            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();

            //Listar Filtrando parâmetro(s)
            var list1 = repositorio.Listar(0, "%STEP" + "%").ToList();

            //Listar filtrando resultado de lista (LINQ)
            //var list2 = list1.Where(n => n.Marca.Contains("au")).ToList();
            //var list3 = list2.Where(n => n.Patrimonio == "PAT-4004").ToList();

            //Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;

            var veiculo = repositorio.BuscarPelaChave(primeiroDaLIsta.EquipamentoVeiculoId);

            //Remover passando objeto como parâmetro
            repositorio.Remover(veiculo);

        }

        [TestMethod]
        public void VeiculoSeguros_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoSeguroRepositorio();

            for (int i = 0; i < 2; i++)
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

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar("%0" + "%", 0).ToList();

            //Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var veiculoseguro = repositorio.BuscarPelaChave(primeiroDaLIsta.VeiculoSeguroId);

            //Remover passando objeto como parâmetro
            repositorio.Remover(veiculoseguro);


        }

        [TestMethod]
        public void VeiculoEquipTipoServico_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoEquipTipoServicoRepositorio();

            for (int i = 0; i < 2; i++)
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

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(4, 0).ToList();

            //Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var veiculoequiptipservico = repositorio.BuscarPelaChave(primeiroDaLIsta.VeiculoId);

            //Remover passando objeto como parâmetro
            repositorio.Remover(veiculoequiptipservico);

        }

        [TestMethod]
        public void VeiculoEmpresa_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoEmpresaRepositorio();

            for (int i = 0; i < 2; i++)
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

                d1.Cargo = "CARGO 2";
                d1.Matricula = "MATRICULA1222";

                //Alterar registro
                repositorio.Alterar(d1);

            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(4, 0).ToList();

            //Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var veiculoempresa = repositorio.BuscarPelaChave(primeiroDaLIsta.VeiculoEmpresaId);

            //Remover passando objeto como parâmetro
            repositorio.Remover(veiculoempresa);

        }

        public void VeiculoCredencial_Cadastrar_Alterar_Listar_Remover_com_sucesso()
        {
            var repositorio = new VeiculoCredencialRepositorio();

            for (int i = 0; i < 2; i++)
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
                    CardHolderGuid = null,
                    CredencialGuid = null,
                    VeiculoPrivilegio1Id = 1,
                    VeiculoPrivilegio2Id = 1,
                    Ativa = true,
                    Colete = "COLETE " + i,
                    CredencialmotivoId = 1,
                    Baixa = DateTime.Now,
                    Impressa = true

                };

                //Criar registro
                repositorio.Criar(d1);

                d1.NumeroCredencial = "CARGO " + (i * 2);
                d1.Colete = "MATRICULA " + (i * 2);

                //Alterar registro
                repositorio.Alterar(d1);

            }

            //Listar todos
            var list0 = repositorio.Listar().ToList();

            //Listar Filtrando parâmetros
            var list1 = repositorio.Listar(4, 0).ToList();

            //Listar pela chave
            var primeiroDaLIsta = list0.FirstOrDefault();
            if (primeiroDaLIsta == null) return;
            var veiculocredencial = repositorio.BuscarPelaChave(primeiroDaLIsta.VeiculoCredencialId);

            //Remover passando objeto como parâmetro
            repositorio.Remover(veiculocredencial);

        }


    }
}
