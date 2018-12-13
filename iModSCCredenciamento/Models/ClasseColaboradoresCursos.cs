using System;
using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseColaboradoresCursos
    {

        public ObservableCollection<ColaboradorCurso> ColaboradoresCursos { get; set; }
        public class ColaboradorCurso
        {

            public int ColaboradorCursoID { get; set; }
            public int ColaboradorID { get; set; }
            public int CursoID { get; set; }
            public string CursoNome { get; set; }
            public string NomeArquivo { get; set; }
            public string Arquivo { get; set; }
            public DateTime? Validade { get; set; }
            public bool Controlado { get; set; }

            public ColaboradorCurso CriaCopia(ColaboradorCurso _ColaboradoresCursos)
            {
                return (ColaboradorCurso)_ColaboradoresCursos.MemberwiseClone();
            }
        }
    }
}
