using System;
using System.Collections.Generic;
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
            var d1 = repositorio.BuscarPelaChave(9);
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
        public void ColabororadorCredencial_Remove_com_sucesso()
        {


        }

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
        public void Colabororador_Buscar_Criar_Alterar_com_sucesso()
        {
            var repositorio = new ColaboradorRepositorio();
            var d1 = repositorio.BuscarPelaChave(9);
            d1.Nome = "Valnei Batista Filho";
            repositorio.Alterar(d1);
            d1.Nome = "José Dirceu";
            int key;
            repositorio.Criar(d1);
        }

        [TestMethod]
        public void ColabororadorCredencial_Cadastrar_com_sucesso()
        {
            var repositorio = new ColaboradorCredencialRepositorio();
            int key;
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

    }
}
