using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
   public class ClasseCredenciaisMotivos
    {
        public ObservableCollection<CredencialMotivo> CredenciaisMotivos { get; set; }
        public class CredencialMotivo
        {
            public int CredencialMotivoID { get; set; }
            public string Descricao { get; set; }
            public int Tipo { get; set; }
        }
    }
}
