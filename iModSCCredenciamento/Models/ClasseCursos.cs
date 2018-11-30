using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
