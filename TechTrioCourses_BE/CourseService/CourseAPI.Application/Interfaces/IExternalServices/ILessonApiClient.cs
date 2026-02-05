using CourseAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrioCourses.Shared.Dtos.Lesson;
namespace CourseAPI.Application.Interfaces.IExternalServices
{
    public interface ILessonApiClient
    {

        Task<List<LessonResponse>> GetLessonsByCourseIdAsync(Guid courseId);
        Task PopulateLessonCountsAsync(List<CourseResponse> courses);
       
        Task<Dictionary<Guid, int>> GetAllLessonCountsAsync();
    }
}
