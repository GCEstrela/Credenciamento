// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class CursoView
    {
        #region  Propriedades

        public int CursoId { get; set; }
        public string Descricao { get; set; }
        public bool Cracha { get; set; }
        public bool Ativo { get; set; }
        public bool Habilitado { get; set; }

        public override bool Equals(object obj)
        {
            var view = obj as CursoView;
            return view != null &&
                   CursoId == view.CursoId;
        }

        public override int GetHashCode()
        {
            return 1406555770 + CursoId.GetHashCode();
        }


        #endregion
    }
}