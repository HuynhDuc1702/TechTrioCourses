using CourseAPI.DTOs.Request;
using CourseAPI.DTOs.Response;
using static CourseAPI.DTOs.Response.CourseResponse;

namespace CourseAPI.Services.Interfaces
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
