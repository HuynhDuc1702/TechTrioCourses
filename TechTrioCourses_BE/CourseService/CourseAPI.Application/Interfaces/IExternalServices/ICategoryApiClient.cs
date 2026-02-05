using CourseAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrioCourses.Shared.Dtos.Category;

namespace CourseAPI.Application.Interfaces.IExternalServices
{
    public interface ICategoryApiClient
    {
        Task<CategoryResponse?> GetCategoryByIdAsync(Guid id);
        Task PopulateCategoryNameAsync(List<CourseResponse> courses);
        Task<Dictionary<Guid, string>> GetCategoryNamesAsync(IEnumerable<Guid> categoryIds);
    }
}
