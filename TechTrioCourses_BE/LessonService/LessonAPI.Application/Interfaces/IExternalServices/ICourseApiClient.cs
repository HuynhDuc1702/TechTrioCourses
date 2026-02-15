using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrioCourses.Shared.Dtos.Course;

namespace LessonAPI.Application.Interfaces.IExternalServices
{
    public interface ICourseApiClient
    {
        Task<CourseResponse?> GetCourseByIdAsync(Guid courseId);
       
    }
}
