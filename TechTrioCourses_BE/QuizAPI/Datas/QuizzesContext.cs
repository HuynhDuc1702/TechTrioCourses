using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuizAPI.Enums;
using QuizAPI.Models;

namespace QuizAPI.Datas;

public partial class QuizzesContext : DbContext
{
    public QuizzesContext()
    {
    }

    public QuizzesContext(DbContextOptions<QuizzesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    public virtual DbSet<QuestionChoice> QuestionChoices { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("questions_pkey");

            entity.ToTable("questions");

            entity.HasIndex(e => e.CourseId, "idx_questions_course");

            entity.HasIndex(e => e.UserId, "idx_questions_user");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("created_at");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.QuestionText).HasColumnName("question_text");
            entity.Property(e => e.QuestionType)
             .HasConversion<short>()
            .HasColumnName("question_type");
            entity.Property(e => e.Status)
             .HasConversion<short>()
                .HasDefaultValue(QuestionStatusEnum.Hidden)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<QuestionAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_answers_pkey");

            entity.ToTable("question_answers");

            entity.HasIndex(e => e.QuestionId, "idx_question_answers_question");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.AnswerText).HasColumnName("answer_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("created_at");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionAnswers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("fk_question_answers_question");
        });

        modelBuilder.Entity<QuestionChoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_choices_pkey");

            entity.ToTable("question_choices");

            entity.HasIndex(e => e.QuestionId, "idx_question_choices_question");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.ChoiceText).HasColumnName("choice_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("created_at");
            entity.Property(e => e.IsCorrect)
                .HasDefaultValue(false)
                .HasColumnName("is_correct");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionChoices)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("fk_question_choices_question");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quizzes_pkey");

            entity.ToTable("quizzes");

            entity.HasIndex(e => e.CourseId, "idx_quizzes_course");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DurationMinutes).HasColumnName("duration_minutes");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                 .HasConversion<short>()
                .HasDefaultValue(QuizzStatusEnum.Hidden)
                .HasColumnName("status");
            entity.Property(e => e.TotalMarks).HasColumnName("total_marks");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(e => new { e.QuizId, e.QuestionId }).HasName("quiz_questions_pkey");

            entity.ToTable("quiz_questions");

            entity.HasIndex(e => e.QuizId, "idx_quiz_questions_quiz");

            entity.Property(e => e.QuizId).HasColumnName("quiz_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.OverridePoints).HasColumnName("override_points");
            entity.Property(e => e.QuestionOrder).HasColumnName("question_order");

            entity.HasOne(d => d.Question).WithMany(p => p.QuizQuestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("fk_qq_question");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizQuestions)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("fk_qq_quiz");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
