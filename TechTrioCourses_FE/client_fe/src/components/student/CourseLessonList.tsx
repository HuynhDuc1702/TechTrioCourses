"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { lessonAPI, LessonResponse } from "@/services/lessonAPI";
import { userLessonAPI } from "@/services/userAPI";
import { useAuth } from "@/contexts/AuthContext";

interface CourseLessonListProps {
    courseId: string;
}

interface LessonWithCompletion extends LessonResponse {
    isCompleted: boolean;
}

export default function CourseLessonList({ courseId }: CourseLessonListProps) {
    const router = useRouter();
    const { user } = useAuth();
    const [lessons, setLessons] = useState<LessonWithCompletion[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchLessonsWithCompletion = async () => {
            try {
                setLoading(true);

                // Fetch all lessons for this course
                const lessonData = await lessonAPI.getLessonsByCourseId(courseId);

                // Check completion status for each lesson
                if (user?.userId) {
                    const lessonsWithCompletion = await Promise.all(
                        lessonData.map(async (lesson) => {
                            try {
                                const completionStatus = await userLessonAPI.checkIsCompleted(
                                    lesson.id
                                );
                                return {
                                    ...lesson,
                                    isCompleted: completionStatus.isCompleted,
                                };
                            } catch (err) {
                                // If check fails, assume not completed
                                return {
                                    ...lesson,
                                    isCompleted: false,
                                };
                            }
                        })
                    );
                    setLessons(lessonsWithCompletion);
                } else {
                    // If no user, mark all as not completed
                    setLessons(lessonData.map(lesson => ({ ...lesson, isCompleted: false })));
                }
            } catch (err) {
                setError(err instanceof Error ? err.message : "Failed to load lessons");
            } finally {
                setLoading(false);
            }
        };

        fetchLessonsWithCompletion();
    }, [courseId, user?.userId]);

    const handleLessonClick = (lessonId: string) => {
        router.push(`/student/${courseId}/lessons/${lessonId}`);
    };

    if (loading) {
        return (
            <div className="flex justify-center items-center py-12">
                <div className="text-center">
                    <div className="inline-block animate-spin rounded-full h-10 w-10 border-b-2 border-indigo-600 mb-3"></div>
                    <p className="text-gray-600">Loading lessons...</p>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="bg-red-50 border-l-4 border-red-500 p-4 rounded-lg">
                <p className="text-red-700 font-semibold">Error: {error}</p>
            </div>
        );
    }

    if (lessons.length === 0) {
        return (
            <div className="text-center py-12 bg-gray-50 rounded-lg">
                <svg
                    className="mx-auto h-12 w-12 text-gray-400"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                >
                    <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"
                    />
                </svg>
                <h3 className="mt-2 text-lg font-medium text-gray-900">No lessons yet</h3>
                <p className="mt-1 text-sm text-gray-500">
                    This course doesn't have any lessons at the moment.
                </p>
            </div>
        );
    }

    return (
        <div className="space-y-3">
            {lessons.map((lesson, index) => (
                <div
                    key={lesson.id}
                    onClick={() => handleLessonClick(lesson.id)}
                    className="group bg-white border border-gray-200 rounded-lg p-4 hover:shadow-md hover:border-indigo-300 transition-all duration-200 cursor-pointer"
                >
                    <div className="flex items-center justify-between">
                        <div className="flex items-center flex-1 min-w-0">
                            {/* Lesson Number */}
                            <div className="flex-shrink-0 w-10 h-10 bg-indigo-100 text-indigo-600 rounded-full flex items-center justify-center font-semibold mr-4">
                                {index + 1}
                            </div>

                            {/* Lesson Info */}
                            <div className="flex-1 min-w-0">
                                <h3 className="text-lg font-semibold text-gray-800 group-hover:text-indigo-600 transition-colors truncate">
                                    {lesson.title}
                                </h3>
                                {lesson.content && (
                                    <p className="text-sm text-gray-500 mt-1 line-clamp-1">
                                        {lesson.content.substring(0, 100)}
                                        {lesson.content.length > 100 ? "..." : ""}
                                    </p>
                                )}
                            </div>
                        </div>

                        {/* Completion Status */}
                        <div className="flex items-center gap-3 ml-4">
                            {lesson.isCompleted ? (
                                <div className="flex items-center gap-2 bg-green-50 px-3 py-1.5 rounded-full">
                                    <svg
                                        className="w-5 h-5 text-green-600"
                                        fill="currentColor"
                                        viewBox="0 0 20 20"
                                    >
                                        <path
                                            fillRule="evenodd"
                                            d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
                                            clipRule="evenodd"
                                        />
                                    </svg>
                                    <span className="text-sm font-medium text-green-700">Completed</span>
                                </div>
                            ) : (
                                <div className="flex items-center gap-2 bg-gray-50 px-3 py-1.5 rounded-full">
                                    <svg
                                        className="w-5 h-5 text-gray-400"
                                        fill="none"
                                        stroke="currentColor"
                                        viewBox="0 0 24 24"
                                    >
                                        <path
                                            strokeLinecap="round"
                                            strokeLinejoin="round"
                                            strokeWidth={2}
                                            d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"
                                        />
                                    </svg>
                                    <span className="text-sm font-medium text-gray-500">Not Started</span>
                                </div>
                            )}

                            {/* Arrow Icon */}
                            <svg
                                className="w-5 h-5 text-gray-400 group-hover:text-indigo-600 transition-colors"
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={2}
                                    d="M9 5l7 7-7 7"
                                />
                            </svg>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    );
}
