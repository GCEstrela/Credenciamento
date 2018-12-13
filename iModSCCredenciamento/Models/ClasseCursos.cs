using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseCursos
    {
        public ObservableCollection<Curso> Cursos { get; set; }
        public class Curso
        {
            public int CursoID { get; set; }
            public string Descricao { get; set; }

        }

    }
}
