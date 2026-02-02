using AutoMapper;
using CategoryAPI.Application.DTOs.Request;
using CategoryAPI.Application.DTOs.Response;
using CategoryAPI.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CategoryAPI.Application.MappingProfile
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>()
.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Category, CategoryResponse>();

        }
    }
}
