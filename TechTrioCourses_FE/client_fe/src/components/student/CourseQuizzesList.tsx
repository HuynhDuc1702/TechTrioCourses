"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { quizAPI, QuizResponse, QuizStatusEnum } from "@/services/quizAPI";
import {
    userQuizAPI,
    UserQuizResponse,
    UserQuizStatus,
} from "@/services/userAPI";
import { useAuth } from "@/contexts/AuthContext";
import Link from "next/link";

interface CourseQuizListProps {
    courseId: string;
}

interface QuizWithUserStatus extends QuizResponse {
    userQuizStatus: UserQuizStatus;
    bestScore?: number;
    attemptCount: number;
}

export default function CourseQuizList({ courseId }: CourseQuizListProps) {
    const router = useRouter();
    const { user } = useAuth();
    const [quizzes, setQuizzes] = useState<QuizWithUserStatus[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchQuizzesAndStatus = async () => {
            try {
                setLoading(true);
                // 1. Fetch all quizzes for this course
                const allQuizData = await quizAPI.getQuizzesByCourseId(courseId);
                const quizData = allQuizData.filter((quiz) => quiz.status === QuizStatusEnum.Published);

                // 2. Fetch User Quiz Statuses if user is logged in
                let userQuizzesData: UserQuizResponse[] = [];
                if (user?.userId) {
                    try {
                        userQuizzesData = await userQuizAPI.getUserQuizzesByUserAndCourse(
                            courseId
                        );
                    } catch (err) {
                        console.error("Failed to fetch user quiz statuses", err);
                        // Don't block the UI, just assume no progress
                    }
                }

                // 3. Merge Data
                const mergedQuizzes: QuizWithUserStatus[] = quizData.map((quiz) => {
                    const userQuiz = userQuizzesData.find(
                        (uq) => uq.quizId === quiz.id
                    );
                    return {
                        ...quiz,
                        userQuizStatus: userQuiz?.status || UserQuizStatus.Not_Started,
                        bestScore: userQuiz?.bestScore,
                        attemptCount: userQuiz?.attemptCount || 0,
                    };
                });

                setQuizzes(mergedQuizzes);
            } catch (err) {
                setError(
                    err instanceof Error ? err.message : "Failed to load quizzes"
                );
            } finally {
                setLoading(false);
            }
        };

        fetchQuizzesAndStatus();
    }, [user, courseId]);

    const handleQuizClick = (quizId: string) => {
        router.push(`/student/${courseId}/quizzes/${quizId}`);
    };

    const getStatusBadge = (status: UserQuizStatus, score?: number, total?: number) => {
        switch (status) {
            case UserQuizStatus.Passed:
                return (
                    <span className="inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold bg-green-100 text-green-700 border border-green-200">
                        <span className="w-1.5 h-1.5 rounded-full bg-green-500 mr-1.5"></span>
                        Passed {score !== undefined && total ? `(${Math.round((score / total) * 100)}%)` : ""}
                    </span>
                );
            case UserQuizStatus.Failed:
                return (
                    <span className="inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold bg-red-100 text-red-700 border border-red-200">
                        <span className="w-1.5 h-1.5 rounded-full bg-red-500 mr-1.5"></span>
                        Failed {score !== undefined && total ? `(${Math.round((score / total) * 100)}%)` : ""}
                    </span>
                );
            case UserQuizStatus.In_progress:
                return (
                    <span className="inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold bg-amber-100 text-amber-700 border border-amber-200">
                        <span className="w-1.5 h-1.5 rounded-full bg-amber-500 mr-1.5 animate-pulse"></span>
                        In Progress
                    </span>
                );
            default:
                return (
                    <span className="inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold bg-gray-100 text-gray-600 border border-gray-200">
                        <span className="w-1.5 h-1.5 rounded-full bg-gray-400 mr-1.5"></span>
                        Not Started
                    </span>
                );
        }
    };

    

    if (loading) {
        return (
            <div className="flex justify-center items-center py-16">
                <div className="text-center">
                    <div className="inline-block animate-spin rounded-full h-10 w-10 border-4 border-indigo-100 border-t-indigo-600 mb-4"></div>
                    <p className="text-gray-500 font-medium animate-pulse">Loading Quizzes...</p>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="bg-red-50 border-l-4 border-red-500 p-6 rounded-lg my-4 shadow-sm">
                <div className="flex items-center">
                    <svg className="h-6 w-6 text-red-500 mr-3" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                    </svg>
                    <p className="text-red-700 font-medium">Error: {error}</p>
                </div>
            </div>
        );
    }

    if (quizzes.length === 0) {
        return (
            <div className="text-center py-16 bg-white rounded-xl border border-dashed border-gray-300">
                <div className="bg-indigo-50 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                    <svg
                        className="h-8 w-8 text-indigo-400"
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth={2}
                            d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
                        />
                    </svg>
                </div>
                <h3 className="text-lg font-semibold text-gray-900">No quizzes available</h3>
                <p className="mt-2 text-sm text-gray-500 max-w-sm mx-auto">
                    It looks like there aren't any quizzes assigned to this course yet. Check back later!
                </p>
            </div>
        );
    }

    return (
        <div className="space-y-4">
            {quizzes.map((quiz) => (
                <div
                    key={quiz.id}
                    className="group bg-white border border-gray-200 rounded-xl p-5 hover:shadow-lg hover:border-indigo-300 transition-all duration-300 relative overflow-hidden"
                >
                    <div className="absolute top-0 right-0 p-4 opacity-10 group-hover:opacity-100 transition-opacity duration-300">
                        <svg className="w-24 h-24 text-indigo-100 transform rotate-12 -mr-8 -mt-8 pointer-events-none" fill="currentColor" viewBox="0 0 20 20">
                            <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM9.555 7.168A1 1 0 008 8v4a1 1 0 001.555.832l3-2a1 1 0 000-1.664l-3-2z" clipRule="evenodd" />
                        </svg>
                    </div>

                    <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 relative z-10">
                        <div className="flex-1 min-w-0">
                            <div className="flex items-center gap-3 mb-2">
                                <h3 className="text-xl font-bold text-gray-800 group-hover:text-indigo-600 transition-colors">
                                    {quiz.name}
                                </h3>
                                {getStatusBadge(quiz.userQuizStatus, quiz.bestScore, quiz.totalMarks)}
                            </div>

                            <p className="text-gray-600 text-sm mb-4 line-clamp-2">
                                {quiz.description || "No description provided."}
                            </p>

                            <div className="flex flex-wrap items-center gap-4 text-sm text-gray-500">
                                <div className="flex items-center bg-gray-50 px-3 py-1 rounded-md">
                                    <svg className="w-4 h-4 mr-2 text-indigo-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
                                    </svg>
                                    <span>{quiz.durationMinutes} mins</span>
                                </div>
                                <div className="flex items-center bg-gray-50 px-3 py-1 rounded-md">
                                    <svg className="w-4 h-4 mr-2 text-indigo-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                                    </svg>
                                    <span>{quiz.totalMarks} Points</span>
                                </div>
                                {quiz.attemptCount > 0 && (
                                    <div className="flex items-center bg-gray-50 px-3 py-1 rounded-md">
                                        <svg className="w-4 h-4 mr-2 text-indigo-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
                                        </svg>
                                        <span>{quiz.attemptCount} Attempt{quiz.attemptCount !== 1 ? 's' : ''}</span>
                                    </div>
                                )}
                            </div>
                        </div>

                        <div className="flex-shrink-0 pt-2 md:pt-0">
                            <button
                                onClick={(e) => {
                                    e.stopPropagation();
                                    handleQuizClick(quiz.id);
                                }}
                                className={`w-full md:w-auto px-6 py-2.5 rounded-lg font-semibold shadow-sm transition-all duration-200 flex items-center justify-center gap-2 ${quiz.userQuizStatus === UserQuizStatus.Passed
                                    ? "bg-white text-indigo-600 border border-indigo-200 hover:bg-indigo-50 hover:border-indigo-300"
                                    : "bg-indigo-600 text-white hover:bg-indigo-700 hover:shadow-indigo-200 hover:shadow-md"
                                    }`}
                            >
                                <span>View Quiz's detail</span>
                                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M14 5l7 7m0 0l-7 7m7-7H3" />
                                </svg>
                            </button>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    );
}
