using LessonAPI.DTOs.Request;
using LessonAPI.DTOs.Response;
using static LessonAPI.DTOs.Response.LessonResponse;

namespace LessonAPI.Services.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonResponse>> GetAllLessonsAsync();
        Task<IEnumerable<LessonResponse>> GetAllLessonsByCourseAsync(Guid courseId);
        Task<LessonResponse?> GetLessonByIdAsync(Guid id);
        Task<LessonResponse> CreateLessonAsync(CreateLessonRequest request);
        Task<LessonResponse?> UpdateLessonAsync(Guid id, UpdateLessonRequest request);
        Task<bool> DeleteLessonAsync(Guid id);
        Task<bool> DisableLessonAsync(Guid id);
        Task<bool> ArchiveLessonAsync(Guid id);
    }
}
