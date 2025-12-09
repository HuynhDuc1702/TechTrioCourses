using AutoMapper;
using QuizAPI.DTOs.Request.Question;
using QuizAPI.DTOs.Request.Quiz;
using QuizAPI.DTOs.Request.QuizzeResult;
using QuizAPI.DTOs.Request.QuestionChoice;
using QuizAPI.DTOs.Request.QuestionAnswer;
using QuizAPI.DTOs.Request.UserSelectedChoice;
using QuizAPI.DTOs.Response.Question;
using QuizAPI.DTOs.Response.Quiz;
using QuizAPI.DTOs.Response.QuizzeResult;
using QuizAPI.DTOs.Response.QuestionChoice;
using QuizAPI.DTOs.Response.QuestionAnswer;
using QuizAPI.DTOs.Response.UserSelectedChoice;
using QuizAPI.Models;

namespace QuizAPI.MappingProfile
{
    public class QuizzesMappingProfile : Profile
    {
        public QuizzesMappingProfile()
        {
            // Question mappings
            CreateMap<Question, QuestionResponse>();
            CreateMap<CreateQuestionRequest, Question>();
            CreateMap<UpdateQuestionRequest, Question>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Quiz mappings
            CreateMap<Quiz, QuizResponse>();
            CreateMap<CreateQuizRequest, Quiz>();
            CreateMap<UpdateQuizRequest, Quiz>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // QuizzeResult mappings
            CreateMap<QuizzeResult, QuizzeResultResponse>();
            CreateMap<CreateQuizzeResultRequest, QuizzeResult>();
            CreateMap<UpdateQuizzeResultRequest, QuizzeResult>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // QuestionChoice mappings
            CreateMap<Models.QuestionChoice, QuestionChoiceResponse>();
            CreateMap<CreateQuestionChoiceRequest, Models.QuestionChoice>();
            CreateMap<UpdateQuestionChoiceRequest, Models.QuestionChoice>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // QuestionAnswer mappings
            CreateMap<Models.QuestionAnswer, QuestionAnswerResponse>();
            CreateMap<CreateQuestionAnswerRequest, Models.QuestionAnswer>();
            CreateMap<UpdateQuestionAnswerRequest, Models.QuestionAnswer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // UserSelectedChoice mappings
            CreateMap<Models.UserSelectedChoice, UserSelectedChoiceResponse>();
            CreateMap<CreateUserSelectedChoiceRequest, Models.UserSelectedChoice>();
            CreateMap<UpdateUserSelectedChoiceRequest, Models.UserSelectedChoice>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
