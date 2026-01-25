"use client";

import { useEffect, useMemo, useState } from "react";
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

  const quizId = params.quizId as string;
  const courseId = params.courseId as string;
  const resultId = params.resultId as string;

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
    questions: quizDetail.questions.map(q => ({
      ...q,
      userAnswer:
        localAnswers.get(q.questionId) ??
        serverAnswers.get(q.questionId),
    })),
  };

  setQuizViewModel(vm);
}, [quizDetail, userQuizResult]);

 const saveQuizProgress = async (isFinalSubmission: boolean) => {
  if (!quizViewModel || !userQuizResult) return;

  await userQuizzeResultsAPI.submitUserQuizzeResult(resultId, {
    resultId,
    userquizId: userQuizResult.userQuizId,
    durationSeconds: userQuizResult.durationSeconds,
    isFinalSubmission,
    answers: quizViewModel.questions.map(q => ({
      questionId: q.questionId,
      questionType: q.questionType,
      selectedChoiceIds: q.userAnswer?.selectedChoiceIds,
      inputAnswer: q.userAnswer?.textAnswer,
    })),
  });
};

  const handleSubmitQuiz = async () => {
    if (!quizViewModel) return;
    try {
      setLoading(true);
      await saveQuizProgress(true);

      
    } catch (err) {
      setError(err instanceof Error ? err.message : "An unknown error occurred");
    }
  }
 
   const handleManualSaveQuiz = async () => {
    if (!quizViewModel) return;
    try {
      setLoading(true);
     
      await saveQuizProgress(false);

      
    } catch (err) {
      setError(err instanceof Error ? err.message : "An unknown error occurred");
    }
  }
// Auto-save every 5 minutes
useEffect(() => {
  if (!quizViewModel) return;

  const interval = setInterval(() => {
    console.log("⏱ Auto-saving quiz progress...");
    saveQuizProgress(false);
  }, 5 * 60 * 1000); 

  return () => clearInterval(interval);
}, [quizViewModel]);


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
  const toggleMultipleChoiceAnswer = (questionId: string, choiceId: string) => {
    setQuizViewModel(prev => {
      if (!prev) return prev;
      return {
        ...prev,
        questions: prev.questions.map(q=>{
          if(q.questionId !== questionId) return q;

          const selectedChoices = q.userAnswer?.selectedChoiceIds || [];

          const next= selectedChoices.includes(choiceId)? selectedChoices
          .filter(id=>id!==choiceId):[...selectedChoices, choiceId];
          return {
            ...q,
            userAnswer:{
              questionId,
               selectedChoiceIds: next}}
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
        
        questions: prev.questions.map(q=>{
          if(q.questionId !== questionId) return q;
          return {
            ...q,
            userAnswer:{
              questionId,
              textAnswer: text}}
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
        questions: prev.questions.map(q=>{
          if(q.questionId !== questionId) return q;
          return {
            ...q,
            userAnswer:{
              questionId,
               selectedChoiceIds: [choiceId]}}
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
      {quizViewModel.questions.map((q, index) => (
        <div key={q.questionId}>
          <h3>Question {index + 1}: {q.questionText}</h3>
          {renderQuestion(q)}
        </div>
      ))}
    </div>
  );
}
