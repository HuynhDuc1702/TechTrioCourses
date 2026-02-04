   CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
/* ===========================
   QUIZZES TABLE
   =========================== */
CREATE TABLE quizzes (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    course_id UUID NOT NULL, -- external reference

    name VARCHAR(200) NOT NULL,
    description TEXT,

    total_marks FLOAT NOT NULL,
    status SMALLINT NOT NULL DEFAULT 0,
    duration_minutes FLOAT NOT NULL DEFAULT 0,

    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);

CREATE INDEX idx_quizzes_course ON quizzes(course_id);



/* ===========================
   QUESTIONS TABLE
   =========================== */
CREATE TABLE questions (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    user_id UUID NOT NULL,    -- external (user service)
    course_id UUID NOT NULL,  -- external (course service)

    question_text TEXT NOT NULL,
    question_type SMALLINT NOT NULL, 
    status SMALLINT NOT NULL,
    points FLOAT NOT NULL,

    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);

CREATE INDEX idx_questions_course ON questions(course_id);
CREATE INDEX idx_questions_user ON questions(user_id);


/* ===========================
   QUESTION Choices TABLE 
   =========================== */
CREATE TABLE question_choices (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    question_id UUID NOT NULL,
    choice_text TEXT NOT NULL,
    is_correct BOOLEAN NOT NULL DEFAULT FALSE,

    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),

    CONSTRAINT fk_question_choices_question
        FOREIGN KEY (question_id)
        REFERENCES questions(id)
        ON DELETE CASCADE
);

CREATE INDEX idx_question_choices_question ON question_choices(question_id);


CREATE TABLE quiz_questions (
    quiz_id UUID NOT NULL,
    question_id UUID NOT NULL,

    question_order INT,
    override_points FLOAT,

    PRIMARY KEY (quiz_id, question_id),

    CONSTRAINT fk_qq_quiz
        FOREIGN KEY (quiz_id)
        REFERENCES quizzes(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_qq_question
        FOREIGN KEY (question_id)
        REFERENCES questions(id)
        ON DELETE CASCADE
);

CREATE INDEX idx_quiz_questions_quiz ON quiz_questions(quiz_id);

/* ===========================
   QUESTION Answer TABLE 
   =========================== */
CREATE TABLE question_answers (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    question_id UUID NOT NULL,
    answer_text TEXT NOT NULL,

    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),

    CONSTRAINT fk_question_answers_question
        FOREIGN KEY (question_id)
        REFERENCES questions(id)
        ON DELETE CASCADE
);

CREATE INDEX idx_question_answers_question ON question_answers(question_id);


