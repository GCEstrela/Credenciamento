using AutoMapper;
using IMOD.Domain.EntitiesCustom;
using IMOD.ValidacaoCredencialWeb.Models;

namespace IMOD.ValidacaoCredencialWeb
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(
            m =>
            {
                m.CreateMap<AutorizacaoView, AutorizacaoViewModel>().ReverseMap(); 
                m.CreateMap<CredencialView, CredencialViewModel>().ReverseMap();
            });
        }
    }
}