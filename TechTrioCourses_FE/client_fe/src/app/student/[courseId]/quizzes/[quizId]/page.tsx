"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { quizAPI, QuizResponse } from "@/services/quizAPI";

import Link from "next/link";
import { useAuth } from "@/contexts/AuthContext";
import {
  userQuizAPI,
  UserQuizResponse,
  UserQuizStatus,
} from "@/services/userAPI";
import {
  UserQuizzeResultResponse,
  userQuizzeResultsAPI,
} from "@/services/UserAPI/userQuizzeResultAPI";


export default function QuizzDetailPage() {
  const params = useParams();
  const router = useRouter();
  const [quiz, setQuiz] = useState<QuizResponse | null>(null);
  const [userQuiz, setUserQuiz] = useState<UserQuizResponse | null>(null);
  const [latestUserQuizResult, setLatestUserQuizResult] = useState<UserQuizzeResultResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const { user } = useAuth();

  const quizId = params.quizId as string;
  const courseId = params.courseId as string;

  useEffect(() => {
    const fetchQuiz = async () => {
      try {

        const data = await quizAPI.getQuizById(quizId);
        setQuiz(data);

      } catch (err) {
        setError(err instanceof Error ? err.message : "An unknown error occurred");
      } finally {
        setLoading(false);
      }
    };

    if (params.quizId) {
      fetchQuiz();
      if (user) {
        checkStarted();
      }

    }

  }, [params.quizId, user]);


  const checkStarted = async () => {
    if (!user) {
     
      setUserQuiz(null);
      return;
    }
    try {
      const userQuizData = await userQuizAPI.getUserQuizByUserAndQuiz(quizId);
      setUserQuiz(userQuizData);

      if (userQuizData) {
        const latestResult = await userQuizzeResultsAPI.getLatestUserQuizzeResultByUserQuizId(userQuizData.id);
        setLatestUserQuizResult(latestResult);
      }

    } catch (err: any) {
      if (err?.response?.status === 404) {
     
      setUserQuiz(null);
      setLatestUserQuizResult(null);
      return;
    }
     setError("Failed to load quiz progress");
    }
  };
  const getStatusBadge = () => {
    if (!userQuiz) return null;

    const statusConfig = {
      [UserQuizStatus.In_progress]: {
        bg: "bg-yellow-100",
        text: "text-yellow-700",
        label: "In Progress"
      },
      [UserQuizStatus.Passed]: {
        bg: "bg-green-100",
        text: "text-green-700",
        label: "Passed"
      },
      [UserQuizStatus.Failed]: {
        bg: "bg-red-100",
        text: "text-red-700",
        label: "Failed"
      },
      [UserQuizStatus.Not_Started]: {
        bg: "bg-gray-100",
        text: "text-gray-700",
        label: "Not Started"
      }
    };

    const config = statusConfig[userQuiz.status];
    return (
      <span className={`inline-flex items-center px-4 py-2 rounded-full text-sm font-medium ${config.bg} ${config.text}`}>
        {config.label}
      </span>
    );
  };

  const renderActionButton = () => {
    if (!userQuiz) {
      return (
        <button
          onClick={handleStartQuiz}
          className="w-full md:w-auto bg-gradient-to-r from-blue-500 to-indigo-600
           text-white font-bold py-3 px-8 rounded-lg
           hover:from-blue-600 hover:to-indigo-700
           transition-all duration-300 shadow-lg hover:shadow-xl"
        >
          Start Quiz
        </button>
      );
    }
    switch (userQuiz?.status) {
      case UserQuizStatus.In_progress:
        return (
          <button
            onClick={handleContinueQuiz}
            className="w-full md:w-auto bg-gradient-to-r from-yellow-500 to-orange-600
             text-white font-bold py-3 px-8 rounded-lg
             hover:from-yellow-600 hover:to-orange-700
             transition-all duration-300 shadow-lg hover:shadow-xl"
          >
            Continue Quiz
          </button>
        );
      case UserQuizStatus.Passed:
      case UserQuizStatus.Failed:
        
          return (
            <button
              onClick={handleRetakeQuiz}
              className="w-full md:w-auto bg-gradient-to-r from-purple-500 to-indigo-600
             text-white font-bold py-3 px-8 rounded-lg
             hover:from-purple-600 hover:to-indigo-700
             transition-all duration-300 shadow-lg hover:shadow-xl"
            >
              Retake Quiz
            </button>
          );
        
      default:
        return null;
    }

  };
  const handleStartQuiz = async () => {

    if (!user?.userId) {
      alert('Please log in to start the quiz');
      router.push('/auth/login');
      return;
    }
    try {

      const createdUserQuiz = await userQuizAPI.createUserQuiz({
        userId: user.userId,
        quizId: quizId,
        courseId: quiz?.courseId || '',
      });
     const createdUserQuizzeResult = await userQuizzeResultsAPI.createUserQuizzeResult({
        userId: user.userId,
        courseId: quiz?.courseId || '',
        quizId: quizId,
        userQuizId: createdUserQuiz.id,
      });
      router.push(`/student/quizzes/${quizId}/attempt/${createdUserQuizzeResult.id}`);

    } catch (err) {
      alert('Failed to start quiz ');
    }
  }
  const handleRetakeQuiz = async () => {

    if (!user?.userId) {
      alert('Please log in to start the quiz');
      router.push('/auth/login');
      return;
    }
    try {

      await userQuizAPI.retakeUserQuiz(userQuiz!.id);
        
      
      const createdUserQuizzeResult = await userQuizzeResultsAPI.createUserQuizzeResult({
        userId: user.userId,
        courseId: quiz?.courseId || '',
        quizId: quizId,
        userQuizId: userQuiz!.id,

      });
      router.push(`/student/quizzes/${quizId}/attempt/${createdUserQuizzeResult.id}`);

    } catch (err) {
      alert('Failed to start quiz ');
    }
  }
  const handleContinueQuiz = async () => {

    if (!user?.userId) {
      alert('Please log in to start the quiz');
      router.push('/auth/login');
      return;
    }
    router.push(`/student/quizzes/${quizId}/attempt/${latestUserQuizResult?.id}`);
  }

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
        <div className="text-center">
          <div className="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mb-4"></div>
          <div className="text-xl text-gray-700">Loading quiz details...</div>
        </div>
      </div>
    );
  }

 if (error) {
  return (
    <div className="flex justify-center items-center min-h-screen">
      <p className="text-red-700 font-semibold">Error: {error}</p>
    </div>
  );
}

if (!quiz) {
  return (
    <div className="flex justify-center items-center min-h-screen">
      <p className="text-red-700 font-semibold">Quiz not found</p>
      <Link
        href={`/student/${courseId}`}
        className="mt-4 inline-block text-indigo-600 underline"
      >
        Back to Course
      </Link>
    </div>
  );
}


  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50">
      <div className="container mx-auto px-4 py-12">
        <Link
          href={`/student/${courseId}`}
          className="inline-flex items-center text-indigo-600 hover:text-indigo-800 mb-6 transition-colors"
        >
          <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
          </svg>
          Back to Courses
        </Link>

        <div className="bg-white rounded-2xl shadow-2xl overflow-hidden">
          <div className="bg-gradient-to-r from-blue-500 to-indigo-600 h-3"></div>

          <div className="p-8 md:p-12">
            //Quiz Section
            <div className="mb-6">
              <h1 className="text-4xl md:text-5xl font-bold mb-4 text-gray-800">
                {quiz.name}
              </h1>

              <div className="flex flex-wrap gap-3 mb-6">
                <span className="inline-flex items-center px-4 py-2 rounded-full text-sm font-medium bg-indigo-100 text-indigo-700">
                  {quiz.description || "No description provided"}
                </span>
                {getStatusBadge()}
              </div>
            </div>

            {userQuiz ? (
              <div className="bg-gray-50 rounded-lg p-6 mb-8">
                <h3 className="text-lg font-semibold mb-4 text-gray-800">Your Progress</h3>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                  <div className="bg-white p-4 rounded-lg shadow-sm">
                    <p className="text-sm text-gray-600">Attempts</p>
                    <p className="text-2xl font-bold text-indigo-600">{userQuiz.attemptCount || 0}</p>
                  </div>
                  <div className="bg-white p-4 rounded-lg shadow-sm">
                    <p className="text-sm text-gray-600">Best Score</p>
                    <p className="text-2xl font-bold text-green-600">{userQuiz.bestScore || 0}</p>
                  </div>
                  <div className="bg-white p-4 rounded-lg shadow-sm">
                    <p className="text-sm text-gray-600">Last Attempt</p>
                    <p className="text-sm font-medium text-gray-700">
                      {userQuiz.lastAttemptAt
                        ? new Date(userQuiz.lastAttemptAt).toLocaleDateString()
                        : "N/A"}
                    </p>
                  </div>
                </div>
              </div>
            ):(
              <div className="bg-gray-50 rounded-lg p-6 mb-8">
                <h3 className="text-lg font-semibold mb-4 text-gray-800">Your Progress</h3>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                  <div className="bg-white p-4 rounded-lg shadow-sm">
                    <p className="text-sm text-gray-600">Attempts</p>
                    <p className="text-2xl font-bold text-indigo-600">0</p>
                  </div>
                  <div className="bg-white p-4 rounded-lg shadow-sm">
                    <p className="text-sm text-gray-600">Best Score</p>
                    <p className="text-2xl font-bold text-green-600">0</p>
                  </div>
                  <div className="bg-white p-4 rounded-lg shadow-sm">
                    <p className="text-sm text-gray-600">Last Attempt</p>
                    <p className="text-sm font-medium text-gray-700">
                      N/A
                    </p>
                  </div>
                </div>
              </div>
            )}



            {latestUserQuizResult ?.completedAt? (
              <div className="border-t border-gray-200 pt-6 mb-8">
                <h3 className="text-lg font-semibold mb-2 text-gray-800">Latest Result</h3>
                <p className="text-gray-600">
                  Score: <span className="font-bold text-indigo-600">{latestUserQuizResult.score}</span> |
                  Completed: {new Date(latestUserQuizResult.completedAt).toLocaleString()}
                </p>
              </div>
            ) : (
              <div className="border-t border-gray-200 pt-6 mb-8">
                <h3 className="text-lg font-semibold mb-2 text-gray-800">Latest Result</h3>
                <p className="text-gray-600">
                  Score: <span className="font-bold text-indigo-600">0</span> |
                  Completed: N/A
                </p>
              </div>
            )}

            <div className="mt-8">
              {renderActionButton()}
            </div>




          </div>
        </div>
      </div>
    </div>
  );
}
