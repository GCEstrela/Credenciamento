using System;
using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
   public class ClasseFormatosCredenciais
    {
        public ObservableCollection<FormatoCredencial> FormatosCredenciais = new ObservableCollection<FormatoCredencial>();

        public class FormatoCredencial
        {
            public int FormatoCredencialID { get; set; }

            public string Descricao { get; set; }

            public Guid? FormatIDGUID { get; set; }
        }
    }
}
