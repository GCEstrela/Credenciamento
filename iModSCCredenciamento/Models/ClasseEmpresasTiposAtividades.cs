using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseEmpresasTiposAtividades
    {

        public ObservableCollection<EmpresaTipoAtividade> EmpresasTiposAtividades { get; set; }
        public class EmpresaTipoAtividade
        {
            public int EmpresaTipoAtividadeID { get; set; }
            public int EmpresaID { get; set; }
            public int TipoAtividadeID { get; set; }
            public string Descricao { get; set; }

            public EmpresaTipoAtividade CriaCopia(EmpresaTipoAtividade empresaTiposAtividades)
            {
                return (EmpresaTipoAtividade)empresaTiposAtividades.MemberwiseClone();
            }

        }
    }
}
