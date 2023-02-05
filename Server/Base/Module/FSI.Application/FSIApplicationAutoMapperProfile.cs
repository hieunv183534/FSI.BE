using AutoMapper;
using FSI.Application.Contracts.Test.DTO;
using FSI.Domain.Test;

namespace FSI
{
    public class FSIApplicationAutoMapperProfile : Profile
    {
        public FSIApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Test, TestDto>().ReverseMap();
            CreateMap<Test, CreateTestDto>().ReverseMap();

        }
    }
}
