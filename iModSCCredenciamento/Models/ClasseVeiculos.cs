using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseVeiculos
    {
        public ObservableCollection<Veiculo> Veiculos { get; set; }

        public class Veiculo
        {


            public int VeiculoID { get; set; }
            public string Descricao { get; set; }
            public string Placa_Identificador { get; set; }
            public string Frota { get; set; }
            public string Patrimonio { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string Tipo { get; set; }
            public string Cor { get; set; }
            public string Ano { get; set; }
            //public string Renavam { get; set; }
            public int EstadoID { get; set; }
            public int MunicipioID { get; set; }
            public string Serie_Chassi { get; set; }
            public int CombustivelID { get; set; }
            public string Altura { get; set; }
            public string Comprimento { get; set; }
            public string Largura { get; set; }

            public string Foto { get; set; }
            public int Excluida { get; set; }
            public int StatusID { get; set; }
            public int TipoAcessoID { get; set; }
            public string DescricaoAnexo { get; set; }
            public string NomeArquivoAnexo { get; set; }
            public string ArquivoAnexo { get; set; }
            public bool Pendente { get; set; }
            public bool Pendente31 { get; set; }
            public bool Pendente32 { get; set; }
            public bool Pendente33 { get; set; }
            public bool Pendente34 { get; set; }

            public Veiculo CriaCopia(Veiculo veiculos)
            {
                return (Veiculo)veiculos.MemberwiseClone();
            }
        }
    }
}
