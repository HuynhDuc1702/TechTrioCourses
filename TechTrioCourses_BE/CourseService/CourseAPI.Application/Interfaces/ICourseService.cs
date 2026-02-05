using CourseAPI.Application.DTOs.Request;
using CourseAPI.Application.DTOs.Response;
using static CourseAPI.Application.DTOs.Response.CourseResponse;

namespace CourseAPI.Application.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponse>> GetAllCoursesAsync();
      
        Task<CourseResponse?> GetCourseByIdAsync(Guid id);
        
        Task<CourseResponse> CreateCourseAsync(CreateCourseRequest request);
        Task<CourseResponse?> UpdateCourseAsync(Guid id, UpdateCourseRequest request);
        Task<bool> DeleteCourseAsync(Guid id);
        Task<bool> DisableCourseAsync(Guid id);
        Task<bool> ArchiveCourseAsync(Guid id);
    }
}
