using AutoMapper;
using Microsoft.Extensions.Logging;

using TechTrioCourses.Shared.Enums;
using UserAPI.Application.DTOs.Response.UserCourse;
using UserAPI.Application.Interfaces.IExternalServices;
using UserAPI.Application.Interfaces.IRepositories;
using UserAPI.Application.Interfaces.IServices;


namespace UserAPI.Application.Services
{
    public class UserCourseProgressService : IUserCourseProgressService
    {
        private readonly IUserCourseRepository _userCourseRepo;
        private readonly IUserLessonRepository _userLessonRepo;
        private readonly IUserQuizRepository _userQuizRepo;
        private readonly ILessonApiClient _lessonApiClient;
        private readonly IQuizApiClient _quizApiClient;
        private readonly ILogger<UserCourseProgressService> _logger;
        private readonly IMapper _mapper;

        public UserCourseProgressService(
            IUserCourseRepository userCourseRepo,
            IUserLessonRepository userLessonRepo,
            IUserQuizRepository userQuizRepo,
            IHttpClientFactory httpClientFactory,
            ILogger<UserCourseProgressService> logger,
            ILessonApiClient lessonAPIClient,
            IQuizApiClient quizApiClient,
            IMapper mapper)
        {
            _userCourseRepo = userCourseRepo;
            _userLessonRepo = userLessonRepo;
            _userQuizRepo = userQuizRepo;
            _lessonApiClient = lessonAPIClient;
            _quizApiClient = quizApiClient;

            _logger = logger;
            _mapper = mapper;
        }

        public async Task<object> RecaculateCourseProgress(Guid courseId, Guid userId)
        {
            _logger.LogInformation("RecaculateCourseProgress called with UserId: {UserId}, CourseId: {CourseId}", userId, courseId);


            var userCourse = await _userCourseRepo.GetByUserAndCourseAsync(userId, courseId);

            if (userCourse == null)
            {
                _logger.LogWarning("UserCourse not found for UserId: {UserId}, CourseId: {CourseId}", userId, courseId);
                return null;
            }


            var userLessons = await _userLessonRepo.GetByUserAndCourseAsync(userId, courseId);
            var userLessonsList = userLessons.ToList();

            int totalLessons = await _lessonApiClient.GetLessonCountByCourseIdAsync(courseId);
            int completedLessons = userLessonsList.Count(ul => ul.Status == UserLessonStatusEnum.Completed);



            var userQuiz = await _userQuizRepo.GetByUserAndCourseAsync(userId, courseId);
            var userQuizList = userQuiz.ToList();

            int totalQuiz = await _quizApiClient.GetQuizCountByCourseIdAsync(courseId);
            int passedQuiz = userQuizList.Count(uq => uq.Status == UserQuizStatusEnum.Passed);


            // Calculate progress and round to 2 decimal places
            if (totalLessons > 0 || totalQuiz > 0)
            {
                int totalCourse = totalQuiz + totalLessons;
                int totalUserCourse = completedLessons + passedQuiz;
                double rawProgress = (double)totalUserCourse / totalCourse * 100;
                userCourse.Progress = Math.Ceiling(rawProgress);
            }
            else
            {
                userCourse.Progress = 0;
            }

            await _userCourseRepo.UpdateUserCourseAsync(userCourse);

            _logger.LogInformation("Course progress recalculated for UserId: {UserId}, CourseId: {CourseId}, Progress: {Progress}%",
   userId, courseId, userCourse.Progress);

            return _mapper.Map<UserCourseResponse>(userCourse);
        }
    }


    
}
