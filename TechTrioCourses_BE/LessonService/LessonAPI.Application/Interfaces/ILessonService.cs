using LessonAPI.Application.DTOs.Request;
using LessonAPI.Application.DTOs.Response;
using static LessonAPI.Application.DTOs.Response.LessonResponse;

namespace LessonAPI.Application.Interfaces
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
