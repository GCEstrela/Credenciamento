using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseCredenciaisStatus
    {
        public ObservableCollection<CredencialStatus> CredenciaisStatus { get; set; }
        public class CredencialStatus
        {
            public int CredencialStatusID { get; set; }
            public string Descricao { get; set; }
        }
    }
}
