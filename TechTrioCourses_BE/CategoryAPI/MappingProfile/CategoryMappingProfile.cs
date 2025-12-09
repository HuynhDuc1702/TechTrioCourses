using AutoMapper;
using CategoryAPI.DTOs.Request;
using CategoryAPI.DTOs.Response;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CategoryAPI.MappingProfile
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CreateCategoryRequest, Models.Category>();
            CreateMap<UpdateCategoryRequest, Models.Category>()
.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Models.Category, CategoryResponse>();

        }
    }
}
