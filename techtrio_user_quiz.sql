

/* ===========================
   USER SELECTED CHOICES TABLE 
   =========================== */
CREATE TABLE user_selected_choices (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    result_id UUID NOT NULL,      
    question_id UUID NOT NULL,
	choice_id  UUID NOT NULL, --(Choice id)
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    UNIQUE(result_id, question_id)
);
CREATE INDEX idx_user_selected_choices_result
ON user_selected_choices(result_id);

CREATE INDEX idx_user_selected_choices_question
ON user_selected_choices(question_id);

/* ===========================
 USER INPUT ANSWERS
   =========================== */
CREATE TABLE user_input_answers (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    result_id UUID NOT NULL,      
    question_id UUID NOT NULL,
    answer_text TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    UNIQUE(result_id, question_id)
);
CREATE INDEX idx_user_input_answers_result
ON user_input_answers(result_id);

CREATE INDEX idx_user_input_answers_question
ON user_input_answers(question_id);


/* ===========================
   QUIZ RESULTS TABLE
   =========================== */
CREATE TABLE quizze_results (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),  -- result_id
    user_id UUID NOT NULL,
    course_id UUID NOT NULL,
    quiz_id UUID NOT NULL,
    score FLOAT DEFAULT 0,
    status SMALLINT NOT NULL DEFAULT 0,           -- 0=pending, 1=in_progress, 2=completed
    attempt_number INT NOT NULL DEFAULT 1,
    started_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
    completed_at TIMESTAMPTZ,
    duration_seconds INT DEFAULT 0,
    metadata JSONB,
    updated_at TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);
CREATE INDEX idx_results_user ON quizze_results(user_id);
CREATE INDEX idx_results_course ON quizze_results(course_id);
CREATE INDEX idx_results_quiz ON quizze_results(quiz_id);
