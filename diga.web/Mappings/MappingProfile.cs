using AutoMapper;
using diga.bl.Models;
using diga.web.Models.Texts;

namespace diga.web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Texts, TextDto>()
                .ForMember(d => d.En, o => o.MapFrom(s => s.En))
                .ForMember(d => d.Es, o => o.MapFrom(s => s.Es))
                .ForMember(d => d.Pt, o => o.MapFrom(s => s.Pt))
                .ForMember(d => d.IsHtml, o => o.MapFrom(s => s.IsHtml))
                .ForMember(d => d.TextId, o => o.MapFrom(s => s.TextId));
        }
    }
}
