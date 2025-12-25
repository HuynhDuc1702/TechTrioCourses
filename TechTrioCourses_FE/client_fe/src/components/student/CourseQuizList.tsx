"use client";

import Link from "next/link";

interface CourseQuizListProps {
    courseId: string;
}

export default function CourseQuizList({ courseId }: CourseQuizListProps) {
    return (
        <div className="text-center py-12 bg-gradient-to-br from-indigo-50 to-purple-50 rounded-lg border-2 border-dashed border-indigo-200">
            <svg
                className="mx-auto h-16 w-16 text-indigo-400"
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
            <h3 className="mt-4 text-xl font-semibold text-gray-900">Quizzes Coming Soon</h3>
            <p className="mt-2 text-sm text-gray-600 max-w-md mx-auto">
                Quiz functionality will be available soon. Focus on completing the lessons for now!
            </p>
            <div className="mt-6">
                <Link
                    href={`/student/${courseId}`}
                    className="inline-flex items-center px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors"
                >
                    <svg
                        className="w-5 h-5 mr-2"
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth={2}
                            d="M15 19l-7-7 7-7"
                        />
                    </svg>
                    Back to Course
                </Link>
            </div>
        </div>
    );
}
