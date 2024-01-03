using AutoMapper;
using MyBlog.Model;
using MyBlog.Model.DTO;

namespace MyBlog.Utility._AutoMapper
{
    public class CustomAutoMapperProfile : Profile
    {
        public CustomAutoMapperProfile()
        {
            base.CreateMap<WriterInfo, WriterDTO>();
            base.CreateMap<BlogNews,BlogNewsDTO>()
                .ForMember(dest => dest.TypeName, sourse => sourse.MapFrom(src => src.TypeInfo.Name))
                .ForMember(dest => dest.WriterName, sourse => sourse.MapFrom(src => src.WriteInfo.Name));

        }
    }
}
