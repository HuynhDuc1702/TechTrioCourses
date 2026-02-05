using CourseAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TechTrioCourses.Shared.Dtos.User;

namespace CourseAPI.Application.Interfaces.IExternalServices
{
    public interface IUserApiClient
    {
        Task<UserResponse?> GetUserByIdAsync(Guid id);
        Task PopulateCreatorNameAsync(List<CourseResponse> courses);
        Task<Dictionary<Guid, string>> GetCreatorNamesAsync(IEnumerable<Guid> categoryIds);
    }
}
