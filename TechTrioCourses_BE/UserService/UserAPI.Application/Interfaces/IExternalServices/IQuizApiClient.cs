namespace UserAPI.Application.Interfaces.IExternalServices
{
    public interface IQuizApiClient
    {

        Task<int> GetQuizCountByCourseIdAsync(Guid courseId);
        
    }
}
