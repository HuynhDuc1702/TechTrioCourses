

namespace UserAPI.Services.Interfaces
{
    public interface IUserCourseProgress
    {
        Task<Object> RecaculateCourseProgress(Guid courseId, Guid userId);
       
    }
}
