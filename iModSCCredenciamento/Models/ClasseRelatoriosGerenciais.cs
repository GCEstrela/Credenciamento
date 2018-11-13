using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseRelatoriosGerenciais
    {
        public ObservableCollection<RelatorioGerencial> RelatoriosGerenciais { get; set; }
        public class RelatorioGerencial
        {
            public int RelatorioID { get; set; }
            public string Nome { get; set; }
            public string NomeArquivoRPT { get; set; }
            public string ArquivoRPT { get; set; }
            public bool Ativo { get; set; }

            public RelatorioGerencial CriaCopia(RelatorioGerencial relatoriogerencial)
            {
                return (RelatorioGerencial)relatoriogerencial.MemberwiseClone();
            }
        }
    }
}
