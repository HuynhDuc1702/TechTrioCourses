-- Enable UUID generation
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";


/* ===========================
   ACCOUNTS TABLE (Account Service)
   =========================== */
CREATE TABLE accounts (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    email VARCHAR(150) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    status SMALLINT NOT NULL DEFAULT 1,   -- 0=disabled, 1=active, 2=banned
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);


/* ===========================
   USERS TABLE (Account Service)
   =========================== */
CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    account_id UUID UNIQUE NOT NULL,      -- no FK
    full_name VARCHAR(100) NOT NULL,
    avatar_url TEXT,
    role SMALLINT NOT NULL DEFAULT 1,     -- 0=student, 1=instructor, 2=admin
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);


/* ===========================
   CATEGORIES TABLE (Course Service)
   =========================== */
CREATE TABLE categories (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT
);


/* ===========================
   COURSES TABLE (Course Service)
   =========================== */
CREATE TABLE courses (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    title VARCHAR(200) NOT NULL,
    description TEXT,
    category_id UUID,      -- reference to categories by UUID
    creator_id UUID,       -- reference to users by UUID
    status SMALLINT NOT NULL DEFAULT 1,   -- 0=hidden, 1=published, 2=archived
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);

CREATE INDEX idx_courses_category ON courses(category_id);
CREATE INDEX idx_courses_creator ON courses(creator_id);


/* ===========================
   COMMENTS TABLE (Course Service)
   =========================== */
CREATE TABLE comments (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    course_id UUID NOT NULL,  -- reference to courses
    user_id UUID NOT NULL,    -- reference to users
    content TEXT NOT NULL,
    status SMALLINT NOT NULL DEFAULT 1,   -- 0=hidden, 1=visible, 2=flagged
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);

CREATE INDEX idx_comments_course ON comments(course_id);
CREATE INDEX idx_comments_user ON comments(user_id);


/* ===========================
   RATINGS TABLE (Course Service)
   =========================== */
CREATE TABLE ratings (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    course_id UUID NOT NULL,
    user_id UUID NOT NULL,
    rating_value Float CHECK (rating_value BETWEEN 1 AND 5),
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    UNIQUE(user_id, course_id)
);

CREATE INDEX idx_ratings_course ON ratings(course_id);
CREATE INDEX idx_ratings_user ON ratings(user_id);


/* ===========================
   LESSONS TABLE (Course Service)
   =========================== */
CREATE TABLE lessons (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    course_id UUID NOT NULL,
    title VARCHAR(200) NOT NULL,
    content TEXT,
    media_url TEXT,
    media_type VARCHAR(50),
    order_index INT DEFAULT 0,
	 status SMALLINT NOT NULL DEFAULT 0, 
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);

CREATE INDEX idx_lessons_course ON lessons(course_id);


CREATE TABLE quizze_results (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_id UUID NOT NULL,
    course_id UUID NOT NULL,
    quiz_id UUID NOT NULL,
    score FLOAT NOT NULL,
    completed_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);



/* ===========================
   QUESTIONS TABLE (Quiz Service)
   =========================== */
CREATE TABLE questions (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    quiz_id UUID NOT NULL,                         -- reference to quizzes
    question_text TEXT NOT NULL,
    question_type VARCHAR(50) NOT NULL,           -- "multiple_choice", "true_false", "short_answer"
    points Float NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);

-- Index for fast lookup of questions by quiz
CREATE INDEX idx_questions_quiz ON questions(quiz_id);


/* ===========================
   QUESTION CHOICES TABLE (Multiple Choice / True-False)
   =========================== */
CREATE TABLE question_choices (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    question_id UUID NOT NULL,                     -- reference to questions
    result_id UUID NOT NULL,                       -- reference to results (user attempt)
    option_text TEXT NOT NULL,
    is_correct BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);

-- Index for fast lookup of choices by question
CREATE INDEX idx_question_choices_question ON question_choices(question_id);


/* ===========================
   QUESTION ANSWERS TABLE (Short Answer / User Responses)
   =========================== */
CREATE TABLE question_answers (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    result_id UUID NOT NULL,                       -- reference to results
    question_id UUID NOT NULL,                     -- reference to questions                    
    answer_text TEXT NOT NULL,                          -- for short-answer questions
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    UNIQUE(result_id, question_id)
);

-- Indexes for fast lookup of answers
CREATE INDEX idx_question_answers_result ON question_answers(result_id);
CREATE INDEX idx_question_answers_question ON question_answers(question_id);


/* ===========================
   RESULTS TABLE (Result Service)
   =========================== */
CREATE TABLE quizze_results (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_id UUID NOT NULL,    -- reference to users
    course_id UUID NOT NULL,  -- reference to courses
    quiz_id UUID NOT NULL,    -- reference to quizzes
    score Float NOT NULL,
    completed_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);

CREATE INDEX idx_results_user ON results(user_id);
CREATE INDEX idx_results_course ON results(course_id);
CREATE INDEX idx_results_quiz ON results(quiz_id);





/* ===========================
   CERTIFICATES TABLE (Certificate Service)
   =========================== */
CREATE TABLE certificates (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    course_id UUID NOT NULL,
    user_id UUID NOT NULL,
    issued_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    score INT,
    status SMALLINT DEFAULT 1
);

CREATE INDEX idx_certificates_course ON certificates(course_id);
CREATE INDEX idx_certificates_user ON certificates(user_id);
