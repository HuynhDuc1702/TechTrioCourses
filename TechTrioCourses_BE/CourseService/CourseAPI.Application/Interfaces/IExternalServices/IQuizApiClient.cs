using CourseAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrioCourses.Shared.Dtos.Quiz;
namespace CourseAPI.Application.Interfaces.IExternalServices
{
    public interface IQuizApiClient
    {

        Task<List<QuizResponse>> GetQuizzesByCourseIdAsync(Guid courseId);
        Task PopulateQuizCountsAsync(List<CourseResponse> courses);
        Task<Dictionary<Guid, int>> GetAllQuizCountsAsync();
    }
}
