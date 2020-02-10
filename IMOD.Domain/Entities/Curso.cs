// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class Curso
    {
        #region  Propriedades

        public int CursoId { get; set; }
        public string Descricao { get; set; }
        public bool Cracha { get; set; }
        public bool Ativo { get; set; }
        public bool Habilitado { get; set; }


        public override bool Equals(object obj)
        {
            var curso = obj as Curso;
            return curso != null &&
                   CursoId == curso.CursoId;
        }

        public override int GetHashCode()
        {
            return 1406555770 + CursoId.GetHashCode();
        }

        public Curso(int cursoId)
        {
            CursoId = cursoId;
        }

        public Curso()
        {
        }




        #endregion


    }
}