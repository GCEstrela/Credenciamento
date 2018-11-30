using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseTecnologiasCredenciais
    {
        public ObservableCollection<TecnologiaCredencial> TecnologiasCredenciais { get; set; }
        public class TecnologiaCredencial
        {
            public int TecnologiaCredencialID { get; set; }
            public string Descricao { get; set; }

        }
    }
}
