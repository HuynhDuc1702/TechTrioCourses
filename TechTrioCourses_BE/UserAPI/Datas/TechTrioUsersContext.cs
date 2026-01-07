using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UserAPI.Enums;
using UserAPI.Models;

namespace UserAPI.Datas;

public partial class TechTrioUsersContext : DbContext
{
    public TechTrioUsersContext()
    {
    }

    public TechTrioUsersContext(DbContextOptions<TechTrioUsersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserCourse> UserCourses { get; set; }
    public virtual DbSet<UserLesson> UserLessons { get; set; }
    public virtual DbSet<UserQuiz> UserQuizzes { get; set; }
    public virtual DbSet<UserQuizzeResult> UserQuizzeResults { get; set; }
    public virtual DbSet<UserInputAnswer> UserInputAnswers { get; set; }
    public virtual DbSet<UserSelectedChoice> UserSelectedChoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.AccountId, "users_account_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("created_at");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Role)
                .HasConversion<short>()
                .HasDefaultValue(UserRoleEnum.Student)
                .HasColumnName("role");
        });
        modelBuilder.Entity<UserCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_courses_pkey");

            entity.ToTable("user_courses");

            entity.HasIndex(e => e.CourseId, "idx_user_courses_course");

            entity.HasIndex(e => e.UserId, "idx_user_courses_user");

            entity.HasIndex(e => new { e.UserId, e.CourseId }, "user_courses_user_id_course_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.EnrolledAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("enrolled_at");
            entity.Property(e => e.Progress).HasColumnName("progress");
            entity.Property(e => e.Status)
                .HasConversion<short>()
                .HasDefaultValue(UserCourseStatus.In_progress)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });
        modelBuilder.Entity<UserLesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_lessons_pkey");

            entity.ToTable("user_lessons");

            entity.HasIndex(e => e.LessonId, "idx_user_lessons_lesson");

            entity.HasIndex(e => e.Status, "idx_user_lessons_status");

            entity.HasIndex(e => e.UserId, "idx_user_lessons_user");

            entity.HasIndex(e => new { e.UserId, e.LessonId }, "user_lessons_user_id_lesson_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.Status)
               .HasConversion<short>()
                .HasDefaultValue(UserLessonStatus.Not_Started)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });
        modelBuilder.Entity<UserQuizzeResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_quizze_results_pkey");

            entity.ToTable("user_quizze_results");

            entity.HasIndex(e => e.CourseId, "idx_results_course");

            entity.HasIndex(e => e.QuizId, "idx_results_quiz");

            entity.HasIndex(e => e.UserId, "idx_results_user");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.AttemptNumber)
                .HasDefaultValue(1)
                .HasColumnName("attempt_number");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.DurationSeconds)
                .HasDefaultValue(0)
                .HasColumnName("duration_seconds");
            entity.Property(e => e.Metadata)
                .HasColumnType("jsonb")
                .HasColumnName("metadata");
            entity.Property(e => e.QuizId).HasColumnName("quiz_id");
            entity.Property(e => e.Score)
                .HasDefaultValueSql("0")
                .HasColumnName("score");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("started_at");
            entity.Property(e => e.Status)
               .HasConversion<short>()
                .HasDefaultValue(UserQuizzeResultStatusEnum.In_progress)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });
        modelBuilder.Entity<UserInputAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_input_answers_pkey");

            entity.ToTable("user_input_answers");

            entity.HasIndex(e => e.QuestionId, "idx_user_input_answers_question");

            entity.HasIndex(e => e.ResultId, "idx_user_input_answers_result");

            entity.HasIndex(e => new { e.ResultId, e.QuestionId }, "user_input_answers_result_id_question_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.AnswerText).HasColumnName("answer_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("created_at");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.ResultId).HasColumnName("result_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");
        });
        modelBuilder.Entity<UserSelectedChoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_selected_choices_pkey");

            entity.ToTable("user_selected_choices");

            entity.HasIndex(e => e.QuestionId, "idx_user_selected_choices_question");

            entity.HasIndex(e => e.ResultId, "idx_user_selected_choices_result");

            entity.HasIndex(e => new { e.ResultId, e.QuestionId }, "user_selected_choices_result_id_question_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.ChoiceId).HasColumnName("choice_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("created_at");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.ResultId).HasColumnName("result_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<UserQuiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_quizzes_pkey");

            entity.ToTable("user_quizzes");

            entity.HasIndex(e => e.QuizId, "idx_user_quizzes_quiz");

            entity.HasIndex(e => e.Status, "idx_user_quizzes_status");

            entity.HasIndex(e => e.UserId, "idx_user_quizzes_user");

            entity.HasIndex(e => new { e.UserId, e.QuizId }, "uq_user_quizzes_user_quiz").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.AttemptCount)
                .HasDefaultValue(0)
                .HasColumnName("attempt_count");
            entity.Property(e => e.BestScore).HasColumnName("best_score");
            entity.Property(e => e.FirstAttemptAt).HasColumnName("first_attempt_at");
            entity.Property(e => e.LastAttemptAt).HasColumnName("last_attempt_at");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.PassedAt).HasColumnName("passed_at");
            entity.Property(e => e.QuizId).HasColumnName("quiz_id");
            entity.Property(e => e.Status)
                .HasConversion<short>()
                .HasDefaultValue(UserQuizzStatus.Not_Started)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
