using System;
using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    [Serializable]
    public class ClasseEmpresas
    {
        public ObservableCollection<Empresa> Empresas { get; set; }

        public class Empresa
        {

            public int EmpresaID { get; set; }
            public string Nome { get; set; }
            public string Apelido { get; set; }
            public string CNPJ { get; set; }
            public string InsEst { get; set; }
            public string InsMun { get; set; }
            public string Responsavel { get; set; }
            public string CEP { get; set; }
            public string Endereco { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public int EstadoID { get; set; }
            public int MunicipioID { get; set; }
            public string Email1 { get; set; }
            public string Contato1 { get; set; }
            public string Telefone1 { get; set; }
            public string Celular1 { get; set; }
            public string Email2 { get; set; }
            public string Contato2 { get; set; }
            public string Telefone2 { get; set; }
            public string Celular2 { get; set; }
            public string Obs { get; set; }
            public string Logo { get; set; }
            public int Excluida { get; set; }
            public bool Pendente { get; set; }
            public bool Pendente11 { get; set; }
            public bool Pendente12 { get; set; }
            public bool Pendente13 { get; set; }
            public bool Pendente14 { get; set; }
            public bool Pendente15 { get; set; }
            public bool Pendente16 { get; set; }
            public bool Pendente17 { get; set; }
            public string Sigla { get; set; }
            public int TotalPermanente { get; set; }
            public int TotalTemporaria { get; set; }

            public Empresa CriaCopia(Empresa empresa)
            {
                return (Empresa)empresa.MemberwiseClone();
            }


        }




    }
}
