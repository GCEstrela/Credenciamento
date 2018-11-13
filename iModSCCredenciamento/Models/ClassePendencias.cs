using System;
using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    
    public class ClassePendencias
    {

        public ObservableCollection<Pendencia> Pendencias { get; set; }
        public class Pendencia
        {

            public int PendenciaID { get; set; }
            public int ColaboradorID { get; set; }
            public int EmpresaID { get; set; }
            public int VeiculoID { get; set; }
            public int TipoPendenciaID { get; set; }
            public string Descricao { get; set; }
            public DateTime? DataLimite { get; set; }
            public bool Impeditivo { get; set; }

            public Pendencia CriaCopia(Pendencia _Pendencias)
            {
                return (Pendencia)_Pendencias.MemberwiseClone();
            }
        }

    }
}
