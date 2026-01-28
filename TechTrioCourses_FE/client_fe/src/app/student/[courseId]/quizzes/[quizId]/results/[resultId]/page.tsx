"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import {
    quizAPI,
    QuizDetailResponseDto,
    QuestionTypeEnum,
    QuizQuestionDetailDto
} from "@/services/quizAPI";
import {
    userQuizzeResultsAPI,
    UserQuizzeResultReviewResponse,
    UserQuizQuestionAnswerBase,
    UserQuizzeResultStatusEnum
} from "@/services/UserAPI/userQuizzeResultAPI";
import Link from "next/link";


interface QuestionReviewViewModel extends QuizQuestionDetailDto {
    userAnswer?: UserQuizQuestionAnswerBase;
}

export default function QuizResultPage() {
    const params = useParams();
    const router = useRouter();

    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const [quizDetail, setQuizDetail] = useState<QuizDetailResponseDto | null>(null);
    const [userResult, setUserResult] = useState<UserQuizzeResultReviewResponse | null>(null);

    const quizId = params.quizId as string;
    const courseId = params.courseId as string;
    const resultId = params.resultId as string;

    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);

                // Fetch both quiz details (with correct answers) and user's result
                const [quizData, resultData] = await Promise.all([
                    quizAPI.GetQuizDetailForReview(quizId),
                    userQuizzeResultsAPI.reviewUserQuizzeResult(resultId)
                ]);

                setQuizDetail(quizData);
                setUserResult(resultData);

            } catch (err) {
                console.error("Failed to load results:", err);
                setError("Failed to load quiz results. Please try again.");
            } finally {
                setLoading(false);
            }
        };

        if (quizId && resultId) {
            fetchData();
        }
    }, [quizId, resultId]);

    if (loading) return <div className="p-8 text-center">Loading results...</div>;
    if (error) return <div className="p-8 text-center text-red-500">{error}</div>;
    if (!quizDetail || !userResult) return null;


    // Helper to merge data for rendering
    const questions: QuestionReviewViewModel[] = quizDetail.questions.map(q => ({
        ...q,
        userAnswer: userResult.answers.find(a => a.questionId === q.questionId)
    }));

    
    const isPassed = userResult.status === UserQuizzeResultStatusEnum.Passed;
 
    const statusColor = isPassed ? "text-green-600" : (userResult.status === UserQuizzeResultStatusEnum.Failed ? "text-red-600" : "text-yellow-600");
    const statusText = isPassed ? "PASSED" : (userResult.status === UserQuizzeResultStatusEnum.Failed ? "FAILED" : "GRADING / IN PROGRESS");

    return (
        <div className="max-w-4xl mx-auto p-6 space-y-8">
            {/* Header Section */}
            <div className="bg-white shadow rounded-lg p-6 border-l-4 border-blue-500">
                <h1 className="text-2xl font-bold text-gray-800 mb-2">{quizDetail.name} - Results</h1>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mt-4">
                    <div className="p-4 bg-gray-50 rounded text-center">
                        <p className="text-sm text-gray-500">Score</p>
                        <p className="text-2xl font-bold">{userResult.score ?? 0} / {quizDetail.totalMarks}</p>
                    </div>
                    <div className="p-4 bg-gray-50 rounded text-center">
                        <p className="text-sm text-gray-500">Status</p>
                        <p className={`text-2xl font-bold ${statusColor}`}>{statusText}</p>
                    </div>
                    <div className="p-4 bg-gray-50 rounded text-center">
                        <p className="text-sm text-gray-500">Duration</p>
                        <p className="text-xl font-medium">{formatDuration(userResult.durationSeconds)}</p>
                    </div>
                </div>
            </div>

            {/* Questions Review */}
            <div className="space-y-6">
                <h2 className="text-xl font-semibold text-gray-700">Detailed Review</h2>

                {questions.map((q, index) => (
                    <div key={q.questionId} className="bg-white shadow rounded-lg p-6 border border-gray-100">
                        <div className="flex justify-between items-start mb-4">
                            <h3 className="text-lg font-medium text-gray-900">
                                <span className="text-gray-500 mr-2">Q{index + 1}.</span>
                                {q.questionText}
                            </h3>
                            <span className="text-sm text-gray-500 font-medium">
                                {q.points} pts
                            </span>
                        </div>

                        <div className="ml-4 pl-4 border-l-2 border-gray-200">
                            {renderAnswerReview(q)}
                        </div>
                    </div>
                ))}
            </div>

            <div className="flex justify-center pt-8">
                <Link
                    href={`/student/${courseId}`}
                    className="px-6 py-3 bg-blue-600 text-white font-medium rounded-lg hover:bg-blue-700 transition-colors"
                >
                    Back to Course
                </Link>
            </div>
        </div>
    );
}

// Helper Components & Functions

function renderAnswerReview(q: QuestionReviewViewModel) {
    if (q.questionType === QuestionTypeEnum.ShortAnswer) {
        return <ShortAnswerReview question={q} />;
    }
    // Multiple Choice & True/False handle similarly
    return <ChoicesReview question={q} />;
}

function ShortAnswerReview({ question }: { question: QuestionReviewViewModel }) {
    const userAnswer = question.userAnswer?.textAnswer;

    // Note: Since short answers depend on manual grading or exact string match logic on backend, 
    // we show what user typed vs what was expected if available in 'answers' list.
    const correctAnswers = question.answers?.map(a => a.answerText).join(", ");

    return (
        <div className="space-y-3">
            <div>
                <p className="text-xs uppercase text-gray-500 font-semibold mb-1">Your Answer</p>
                <div className="p-3 bg-gray-50 rounded text-gray-800 border border-gray-200">
                    {userAnswer || <span className="text-gray-400 italic">No answer provided</span>}
                </div>
            </div>
            <div>
                <p className="text-xs uppercase text-green-600 font-semibold mb-1">Correct Answer(s)</p>
                <div className="p-3 bg-green-50 rounded text-green-800 border border-green-200">
                    {correctAnswers || "See instructor for correct answer"}
                </div>
            </div>
        </div>
    );
}

function ChoicesReview({ question }: { question: QuestionReviewViewModel }) {
    return (
        <div className="space-y-2">
            {question.choices?.map(choice => {
                const isSelected = question.userAnswer?.selectedChoiceIds?.includes(choice.id); // Frontend property is selectedChoiceIds from API response
                const isCorrect = choice.isCorrect;

                let styles = "border border-gray-200 bg-white";
                let icon = null;

                if (isCorrect) {
                    styles = "border-green-300 bg-green-50 text-green-800 ring-1 ring-green-300";
                    icon = <span className="ml-auto text-green-600 font-bold">✓ Correct</span>;
                }

                if (isSelected && !isCorrect) {
                    styles = "border-red-300 bg-red-50 text-red-800 ring-1 ring-red-300";
                    icon = <span className="ml-auto text-red-600 font-bold">✗ Your Answer</span>;
                }

                if (isSelected && isCorrect) {
                    icon = <span className="ml-auto text-green-700 font-bold">✓ Your Answer</span>;
                }

                return (
                    <div key={choice.id} className={`flex items-center p-3 rounded-lg ${styles}`}>
                        <div className={`w-4 h-4 rounded-full border mr-3 flex items-center justify-center
                            ${isSelected ? (isCorrect ? 'bg-green-600 border-green-600' : 'bg-red-500 border-red-500') : 'border-gray-300'}
                        `}>
                            {isSelected && <div className="w-1.5 h-1.5 bg-white rounded-full"></div>}
                        </div>
                        <span className="flex-1">{choice.choiceText}</span>
                        {icon}
                    </div>
                );
            })}
        </div>
    );
}

function formatDuration(seconds?: number | null) {
    if (!seconds) return "--";
    const m = Math.floor(seconds / 60);
    const s = Math.floor(seconds % 60);
    return `${m}m ${s}s`;
}
