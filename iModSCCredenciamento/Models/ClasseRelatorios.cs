using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseRelatorios
    {
        public ObservableCollection<Relatorio> Relatorios { get; set; }
        public class Relatorio
        {
            public int RelatorioID { get; set; }
            public string Nome { get; set; }
            public string NomeArquivoRPT { get; set; }
            public string ArquivoRPT { get; set; }
            public bool Ativo { get; set; }

            public Relatorio CriaCopia(Relatorio relatorio)
            {
                return (Relatorio)relatorio.MemberwiseClone();
            }
        }
    }
}
