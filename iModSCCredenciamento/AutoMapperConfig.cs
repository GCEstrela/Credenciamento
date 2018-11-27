// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region

using AutoMapper;
using iModSCCredenciamento.Models;

#endregion

namespace iModSCCredenciamento
{
    public class AutoMapperConfig
    {
        #region  Metodos

        public static void RegisterMappings()
        {
            Mapper.Initialize (
                m => { m.CreateMap<Colaborador, ClasseColaboradores.Colaborador>().ReverseMap(); });
        }

        #endregion
    }
}