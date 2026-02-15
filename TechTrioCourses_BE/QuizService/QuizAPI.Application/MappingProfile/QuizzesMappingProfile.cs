using AutoMapper;
using QuizAPI.Domain.Entities;
using QuizAPI.Application.DTOs.Request.QuestionAnswer;
using QuizAPI.Application.DTOs.Response.QuizQuestion;
using QuizAPI.Application.DTOs.Response.QuestionAnswer;
using QuizAPI.Application.DTOs.Request.QuizQuestion;
using QuizAPI.Application.DTOs.Request.Question;
using QuizAPI.Application.DTOs.Response.QuestionChoice;
using QuizAPI.Application.DTOs.Request.Quiz;
using QuizAPI.Application.DTOs.Request.QuestionChoice;
using QuizAPI.Application.DTOs.Response.Question;
using QuizAPI.Application.DTOs.Response.Quiz;

namespace QuizAPI.Application.MappingProfile
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

            // QuestionChoice mappings
            CreateMap<QuestionChoice, QuestionChoiceResponse>();
            CreateMap<CreateQuestionChoiceRequest, QuestionChoice>();
            CreateMap<UpdateQuestionChoiceRequest, QuestionChoice>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // QuestionAnswer mappings
            CreateMap<QuestionAnswer, QuestionAnswerResponse>();
            CreateMap<CreateQuestionAnswerRequest, QuestionAnswer>();
            CreateMap<UpdateQuestionAnswerRequest, QuestionAnswer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // QuizQuestion mappings
            CreateMap<QuizQuestion, QuizQuestionResponse>();
            CreateMap<CreateQuizQuestionRequest, QuizQuestion>();
            CreateMap<UpdateQuizQuestionRequest, QuizQuestion>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
