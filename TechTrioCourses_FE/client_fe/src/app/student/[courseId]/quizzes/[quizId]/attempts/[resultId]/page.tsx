"use client";

import { useEffect, useMemo, useRef, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import {
  quizAPI,
  AttemptQuizDetailResponseDto,
  AttemptQuizQuestionDto,
  QuestionTypeEnum
} from "@/services/quizAPI";

import Link from "next/link";
import { useAuth } from "@/contexts/AuthContext";
import {
  userQuizAPI,
  UserQuizResponse,
  UserQuizStatus,
} from "@/services/userAPI";
import {

  userQuizzeResultsAPI,
  UserQuizzeResultResumeResponse,
  UserQuizQuestionAnswerBase,
} from "@/services/UserAPI/userQuizzeResultAPI";

export interface QuizQuestionViewModel
  extends AttemptQuizQuestionDto {
  userAnswer?: UserQuizQuestionAnswerBase;
}

export interface AttemptQuizViewModel
  extends AttemptQuizDetailResponseDto {
    startedAt: string;
  questions: QuizQuestionViewModel[];
}

export default function QuizAttemptPage() {
  const params = useParams();
  const router = useRouter();

  const [userQuizResult, setUserQuizResult] = useState<UserQuizzeResultResumeResponse | null>(null);
  const [quizDetail, setQuizDetail] = useState<AttemptQuizDetailResponseDto | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [quizViewModel, setQuizViewModel] =
    useState<AttemptQuizViewModel | null>(null);
  const { user } = useAuth();
  const [error, setError] = useState<string | null>(null);
  const [remainingMs, setRemainingMs] = useState<number>(0);
  const endTimeRef = useRef<number | null>(null);
const minutes = Math.floor(remainingMs / 60000);
const seconds = Math.floor((remainingMs % 60000) / 1000);

  const quizId = params.quizId as string;
  const courseId = params.courseId as string;
  const resultId = params.resultId as string;
  const hasSubmittedRef = useRef(false);
  const STORAGE_KEY = `quiz_attempt_${resultId}`;

  useEffect(() => {
    const fetchQuizAttempt = async () => {
      try {
        setLoading(true);

        const [userQuiz, quizData] = await Promise.all([
          userQuizzeResultsAPI.resumeUserQuizzeResult(resultId),
          quizAPI.GetQuizDetailForAttempt(quizId),
        ]);

        setUserQuizResult(userQuiz);
        setQuizDetail(quizData);

      } catch (err) {
        setError(err instanceof Error ? err.message : "An unknown error occurred");
      } finally {
        setLoading(false);
      }
    };

    if (quizId && resultId) {
      fetchQuizAttempt();
    }
  }, [quizId, resultId]);
  // Merge quiz detail and user answers into view model 
  useEffect(() => {
    if (!quizDetail || !userQuizResult) return;

    const local = localStorage.getItem(STORAGE_KEY);
    let localAnswers = new Map<string, UserQuizQuestionAnswerBase>();

    if (local) {
      const parsed = JSON.parse(local);
      localAnswers = new Map(
        parsed.questions.map((q: any) => [q.questionId, q.userAnswer])
      );
    }

    const serverAnswers = new Map(
      userQuizResult.answers.map(a => [a.questionId, a])
    );

    const vm: AttemptQuizViewModel = {
      ...quizDetail,
      startedAt: userQuizResult.startedAt,
      questions: quizDetail.questions.map(q => ({
        ...q,
        userAnswer:
          localAnswers.get(q.questionId) ??
          serverAnswers.get(q.questionId),
      })),
    };

    setQuizViewModel(vm);
  }, [quizDetail, userQuizResult]);

  //Auto-save progress to localStorage
  useEffect(() => {
    if (!quizViewModel) return;

    const payload = {
      savedAt: new Date().toISOString(),
      questions: quizViewModel.questions.map(q => ({
        questionId: q.questionId,
        userAnswer: q.userAnswer,
      })),
    };

    localStorage.setItem(STORAGE_KEY, JSON.stringify(payload));
  }, [quizViewModel]);

  

  const saveQuizProgress = async (isFinalSubmission: boolean) => {
    if (!quizViewModel || !userQuizResult) return;

    try {
      const payload = {
        resultId,
        userquizId: userQuizResult.userQuizId,
        durationSeconds: userQuizResult.durationSeconds,
        IsFinalSubmisson: isFinalSubmission,  
        answers: quizViewModel.questions.map(q => ({
          questionId: q.questionId,
          questionType: q.questionType,
          SelectedChoices: q.userAnswer?.selectedChoiceIds, 
          textAnswer: q.userAnswer?.textAnswer,
        })),
      };

      console.log("Submitting quiz to backend...", {
        resultId,
        isFinalSubmission,
        questionCount: quizViewModel.questions.length
      });
      console.log("Full payload:", JSON.stringify(payload, null, 2));
      console.log("Answers detail:", payload.answers);

      const response = await userQuizzeResultsAPI.submitUserQuizzeResult(resultId, payload);

      console.log("Backend response:", response);
      return response;
    } catch (error) {
      console.error("Failed to submit quiz:", error);
      throw error; 
    }
  };




  const handleSubmitQuiz = async () => {
    if (!quizViewModel) return;
    if (hasSubmittedRef.current) return; 
    hasSubmittedRef.current = true;
    try {
      setLoading(true);
      setError(null);

      const result = await saveQuizProgress(true);

      if (result) {
        alert(`Quiz submitted! Score: ${result.score ?? 'Pending grading'}`);
        // Clear localStorage after successful submission
        localStorage.removeItem(STORAGE_KEY);
        router.push(`/student/${courseId}/quizzes/${quizId}/results/${result.resultId}`);
      }
    } catch (err) {
      console.error("Submit error:", err);
      setError(err instanceof Error ? err.message : "Failed to submit quiz");
      setLoading(false);
    }
  }

  const handleManualSaveQuiz = async () => {
    if (!quizViewModel) return;
    try {
      setLoading(true);
      setError(null);

      await saveQuizProgress(false);

      alert("Progress saved successfully!");
    } catch (err) {
      console.error("Save error:", err);
      setError(err instanceof Error ? err.message : "Failed to save progress");
    } finally {
      setLoading(false);
    }
  }
  // Auto-save every 5 minutes
  useEffect(() => {
    if (!quizViewModel) return;

    const interval = setInterval(() => {
      console.log("Auto-saving quiz progress...");
      saveQuizProgress(false);
    }, 5 * 60 * 1000);

    return () => clearInterval(interval);
  }, [quizViewModel]);


   // Timer
  useEffect(() => {
    if (!quizViewModel?.startedAt) return;

    const endTime= new Date(quizViewModel.startedAt).getTime() + quizViewModel.durationMinutes * 60 * 1000;

    const remainingTime = endTime - Date.now();

    if (remainingTime <= 0) {
      console.log("Time is already up, submitting quiz...");
      handleSubmitQuiz();
      return;
    }

    const timeout = setTimeout(() => {
      console.log("Time up, submitting quiz...");
      handleSubmitQuiz();
    }, remainingTime);

    return () => clearTimeout(timeout);
  },  [quizViewModel?.startedAt, quizViewModel?.durationMinutes]);

//Render timer for UI
  useEffect(() => {
    if (!quizViewModel?.startedAt) return;  
    endTimeRef.current= new Date(quizViewModel.startedAt).getTime() + 
    quizViewModel.durationMinutes * 60 * 1000;

    const updateRemainingTime = () => {
    
      const remaining = (endTimeRef.current ?? 0) - Date.now();
      setRemainingMs(prev => {
      const next = Math.max(0, remaining);
      return prev !== next ? next : prev;
    });

    if (remaining <= 0) {
      clearInterval(interval);
    }
    };
    updateRemainingTime();

    const interval = setInterval(updateRemainingTime, 1000);
    return () => clearInterval(interval);

  }, [quizViewModel?.startedAt, quizViewModel?.durationMinutes]);

  const renderQuestion = (q: QuizQuestionViewModel) => {
    switch (q.questionType) {
      case QuestionTypeEnum.MultipleChoice:
        return renderMultipleChoice(q)
      case QuestionTypeEnum.TrueFalse:
        return renderTrueFalse(q)
      case QuestionTypeEnum.ShortAnswer:
        return renderShortAnswer(q)
      default:
        return null
    }
  }


  const renderMultipleChoice = (q: QuizQuestionViewModel) => {
    return (
      <div>
        {q.choices?.map(c => (
          <label key={c.id} style={{ display: 'block' }}>
            <input
              type="checkbox"
              checked={
                q.userAnswer?.selectedChoiceIds?.includes(c.id) || false

              }
              onChange={() =>
                toggleMultipleChoiceAnswer(q.questionId, c.id)
              }
            />
            {c.choiceText}
          </label>
        ))}
      </div>
    )

  }
  const renderTimer = () => {
    return (
      <div>
        Time Remaining: {minutes}:{seconds.toString().padStart(2, '0')}
      </div>
    );
  }
  const toggleMultipleChoiceAnswer = (questionId: string, choiceId: string) => {
    setQuizViewModel(prev => {
      if (!prev) return prev;
      return {
        ...prev,
        questions: prev.questions.map(q => {
          if (q.questionId !== questionId) return q;

          const selectedChoices = q.userAnswer?.selectedChoiceIds || [];

          const next = selectedChoices.includes(choiceId) ? selectedChoices
            .filter(id => id !== choiceId) : [...selectedChoices, choiceId];

          console.log("Multiple choice toggled:", { questionId, choiceId, oldSelection: selectedChoices, newSelection: next });

          return {
            ...q,
            userAnswer: {
              questionId,
              selectedChoiceIds: next
            }
          }
        })
      }
    },
    )
  }
  const renderTrueFalse = (q: QuizQuestionViewModel) => {
    return (
      <div>
        {q.choices?.map(c => (
          <label key={c.id} style={{ display: 'block' }}>
            <input
              type="radio"
              name={q.questionId}
              checked={
                q.userAnswer?.selectedChoiceIds?.[0] === c.id

              }
              onChange={() =>
                toggleTrueFalseAnswer(q.questionId, c.id)
              }
            />
            {c.choiceText}
          </label>
        ))}
      </div>
    )


  }
  const renderShortAnswer = (q: QuizQuestionViewModel) => {
    return (
      <textarea
        value={q.userAnswer?.textAnswer ?? ""}
        onChange={e =>
          updateTextAnswer(q.questionId, e.target.value)
        }
      />

    )
  }
  const updateTextAnswer = (questionId: string, text: string) => {
    setQuizViewModel(prev => {
      if (!prev) return prev;
      return {
        ...prev,

        questions: prev.questions.map(q => {
          if (q.questionId !== questionId) return q;
          return {
            ...q,
            userAnswer: {
              questionId,
              textAnswer: text
            }
          }
        })
      }
    },
    )
  }

  const toggleTrueFalseAnswer = (questionId: string, choiceId: string) => {
    setQuizViewModel(prev => {
      if (!prev) return prev;
      return {
        ...prev,
        questions: prev.questions.map(q => {
          if (q.questionId !== questionId) return q;

          console.log("True/False selected:", { questionId, choiceId });

          return {
            ...q,
            userAnswer: {
              questionId,
              selectedChoiceIds: [choiceId]
            }
          }
        })
      }
    },
    )
  }
  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;
  if (!quizViewModel) return null;

  return (
    <div>
      <h1>{quizViewModel.name}</h1>
      <p>Time limit: {quizViewModel.durationMinutes} minutes</p>
      {renderTimer()}
      {quizViewModel.questions.map((q, index) => (
        <div key={q.questionId}>
          <h3>Question {index + 1}: {q.questionText}</h3>
          {renderQuestion(q)}
        </div>
      ))}
      <div style={{ marginTop: 24, display: "flex", gap: 12 }}>
        <button
          onClick={handleManualSaveQuiz}
          disabled={loading}
        >
          💾 Save progress
        </button>

        <button
          onClick={handleSubmitQuiz}
          disabled={loading}
          style={{ backgroundColor: "#e53935", color: "white" }}
        >
          🚀 Submit quiz
        </button>
      </div>
    </div>

  );
}
