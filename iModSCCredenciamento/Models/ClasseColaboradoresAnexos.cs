﻿using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseColaboradoresAnexos
    {
        public ObservableCollection<ColaboradorAnexo> ColaboradoresAnexos { get; set; }
        public class ColaboradorAnexo
        {

            public int ColaboradorAnexoID { get; set; }
            public int ColaboradorID { get; set; }
            public string Descricao { get; set; }
            public string NomeArquivo { get; set; }
            public string Arquivo { get; set; }         

            public ColaboradorAnexo CriaCopia(ColaboradorAnexo _ColaboradoresAnexos)
            {
                return (ColaboradorAnexo)_ColaboradoresAnexos.MemberwiseClone();
            }
        }
    }
}
