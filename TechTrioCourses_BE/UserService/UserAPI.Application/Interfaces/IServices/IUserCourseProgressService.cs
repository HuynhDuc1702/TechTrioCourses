namespace UserAPI.Application.Interfaces.IServices
{
    public interface IUserCourseProgressService
    {
        Task<object> RecaculateCourseProgress(Guid courseId, Guid userId);
       
    }
}
