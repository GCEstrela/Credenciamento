using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
