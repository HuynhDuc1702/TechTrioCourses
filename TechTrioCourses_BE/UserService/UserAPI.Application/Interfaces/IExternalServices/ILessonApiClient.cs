namespace UserAPI.Application.Interfaces.IExternalServices
{
    public interface ILessonApiClient
    {

        Task<int> GetLessonCountByCourseIdAsync(Guid courseId);
       
    }
}
