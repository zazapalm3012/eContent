using AutoMapper;
using eContentApp.Application.DTOs.Category;
using eContentApp.Application.DTOs.Post;
using eContentApp.Domain.Entities;

namespace eContentApp.Application.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Post, PostListDto>();
            CreateMap<Post, PostDetailDto>();
            CreateMap<CreatePostDto, Post>()
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Status is handled manually in service
                .ForMember(dest => dest.Categories, opt => opt.Ignore()); // Categories are handled manually in service
            CreateMap<UpdatePostDto, Post>()
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Status is handled manually in service
                .ForMember(dest => dest.Categories, opt => opt.Ignore()); // Categories are handled manually in service
        }
    }
}