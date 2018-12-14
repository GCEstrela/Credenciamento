using System;
using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseColaboradores
    {
        public ObservableCollection<Colaborador> Colaboradores { get; set; }
        public class Colaborador
        {
            public int ColaboradorID { get; set; }
            public string Nome { get; set; }
            public string Apelido { get; set; }
            public DateTime? DataNascimento { get; set; }
            public string NomePai { get; set; }
            public string NomeMae { get; set; }
            public string Nacionalidade { get; set; }
            public string Foto { get; set; }
            public string EstadoCivil { get; set; }
            public string CPF { get; set; }
            public string RG { get; set; }
            public DateTime? RGEmissao { get; set; }
            public string RGOrgLocal { get; set; }
            public string RGOrgUF { get; set; }
            public string Passaporte { get; set; }
            public DateTime? PassaporteValidade { get; set; }
            public string RNE { get; set; }
            public string TelefoneFixo { get; set; }
            public string TelefoneCelular { get; set; }
            public string Email { get; set; }
            public string ContatoEmergencia { get; set; }
            public string TelefoneEmergencia { get; set; }
            public string Cep { get; set; }
            public string Endereco { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public int MunicipioID { get; set; }
            public int EstadoID { get; set; }
            public bool Motorista { get; set; }
            public string CNHCategoria { get; set; }
            public string CNH { get; set; }
            public DateTime? CNHValidade { get; set; }
            public string CNHEmissor { get; set; }
            public string CNHUF { get; set; }
            public string Bagagem { get; set; }
            public DateTime? DataEmissao { get; set; }
            public DateTime? DataValidade { get; set; }
            public int Excluida { get; set; }
            public int StatusID { get; set; }
            public int TipoAcessoID { get; set; }
            public bool Pendente { get; set; }
            public bool Pendente21 { get; set; }
            public bool Pendente22 { get; set; }
            public bool Pendente23 { get; set; }
            public bool Pendente24 { get; set; }
            public bool Pendente25 { get; set; }
            public bool Pendente26 { get; set; }
            public bool Pendente27 { get; set; }

            public Colaborador CriaCopia(Colaborador colaborador)
            {
                return (Colaborador)colaborador.MemberwiseClone();
            }
        }
    }
}
