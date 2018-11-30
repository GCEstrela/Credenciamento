using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseTiposCredenciais
    {
        public ObservableCollection<TipoCredencial> TiposCredenciais { get; set; }
        public class TipoCredencial
        {
            public int TipoCredencialID { get; set; }
            public string Descricao { get; set; }

        }
    }
}
