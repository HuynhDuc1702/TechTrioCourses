"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { lessonAPI, LessonResponse, LessonMediaTypeEnum, getMediaTypeLabel, LessonStatusEnum } from "@/services/lessonAPI";
import { userLessonAPI } from "@/services/userAPI";
import { useAuth } from "@/contexts/AuthContext";
import LessonSideNavigation from "@/components/student/LessonSideNavigation";
import Link from "next/link";

export default function StudentLessonPage() {
    const params = useParams();
    const router = useRouter();
    const { user } = useAuth();

    const courseId = params.courseId as string;

    const lessonId = params.lessonId as string;

    const [lesson, setLesson] = useState<LessonResponse | null>(null);
    const [allLessons, setAllLessons] = useState<LessonResponse[]>([]);
    const [isCompleted, setIsCompleted] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [markingComplete, setMarkingComplete] = useState(false);

    useEffect(() => {
        if (lessonId && courseId) {
            loadLessonData();
        }
    }, [lessonId, courseId, user?.userId]);

    const loadLessonData = async () => {
        try {
            setLoading(true);

            // Fetch current lesson
            const lessonData = await lessonAPI.getLessonById(lessonId);
            setLesson(lessonData);

            // Fetch all lessons for the course (for side navigation)
            const courseLessons = await lessonAPI.getLessonsByCourseId(courseId);
            const publishedLessons = courseLessons.filter(lesson => lesson.status === LessonStatusEnum.Published);
            const sortedLessons = publishedLessons.sort((a, b) => (a.orderIndex || 0) - (b.orderIndex || 0));
            setAllLessons(sortedLessons);

            // Check if lesson is completed
            if (user?.userId) {
                try {
                    const completionStatus = await userLessonAPI.checkIsCompleted(lessonId);
                    setIsCompleted(completionStatus.isCompleted);
                } catch (err) {
                    console.log('Could not check completion status:', err);
                    setIsCompleted(false);
                }
            }
        } catch (err) {
            setError(err instanceof Error ? err.message : "Failed to load lesson");
        } finally {
            setLoading(false);
        }
    };

    const handleMarkAsCompleted = async () => {
        if (!user?.userId) {
            alert('Please log in to mark lessons as completed');
            return;
        }

        try {
            setMarkingComplete(true);
            await userLessonAPI.createUserLesson({
                userId: user.userId,
                lessonId: lessonId,
                courseId: courseId,
            });
            setIsCompleted(true);
        } catch (err) {
            alert('Failed to mark lesson as completed');
        } finally {
            setMarkingComplete(false);
        }
    };

    const getYouTubeVideoId = (url: string): string | null => {
        const patterns = [
            /(?:youtube\.com\/watch\?v=|youtu\.be\/)([^&\n?#]+)/,
            /youtube\.com\/embed\/([^&\n?#]+)/
        ];

        for (const pattern of patterns) {
            const match = url.match(pattern);
            if (match && match[1]) return match[1];
        }
        return null;
    };

    const renderMedia = () => {
        if (!lesson?.mediaUrl) {
            return (
                <div className="bg-gray-100 rounded-lg p-8 text-center text-gray-500">
                    No media content available for this lesson
                </div>
            );
        }

        // Handle Video type - YouTube only for now
        if (lesson.mediaType === LessonMediaTypeEnum.Video) {
            const videoId = getYouTubeVideoId(lesson.mediaUrl);

            if (videoId) {
                return (
                    <div className="relative w-full" style={{ paddingBottom: '56.25%' }}>
                        <iframe
                            className="absolute top-0 left-0 w-full h-full rounded-lg"
                            src={`https://www.youtube.com/embed/${videoId}`}
                            title={lesson.title}
                            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                            allowFullScreen
                        />
                    </div>
                );
            }
        }

        // Placeholder for other media types
        return (
            <div className="bg-gray-50 border-2 border-dashed border-gray-300 rounded-lg p-12 text-center">
                <div className="text-gray-600 mb-4">
                    <svg className="w-16 h-16 mx-auto mb-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z" />
                    </svg>
                    <p className="text-lg font-medium text-gray-700 mb-2">
                        Media content will be displayed here
                    </p>
                    <p className="text-sm text-gray-500">
                        Type: {getMediaTypeLabel(lesson.mediaType)}
                    </p>
                </div>
                <a
                    href={lesson.mediaUrl}
                    target="_blank"
                    rel="noopener noreferrer"
                    className="text-blue-600 hover:text-blue-800 text-sm underline break-all"
                >
                    {lesson.mediaUrl}
                </a>
            </div>
        );
    };

    if (loading) {
        return (
            <div className="flex justify-center items-center min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
                <div className="text-center">
                    <div className="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mb-4"></div>
                    <div className="text-xl text-gray-700">Loading lesson...</div>
                </div>
            </div>
        );
    }

    if (error || !lesson) {
        return (
            <div className="flex justify-center items-center min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
                <div className="bg-red-50 border-l-4 border-red-500 p-6 rounded-lg shadow-lg max-w-md">
                    <p className="text-red-700 font-semibold">Error: {error || "Lesson not found"}</p>
                    <Link
                        href={`/student/${courseId}`}
                        className="mt-4 inline-block text-indigo-600 hover:text-indigo-800 underline"
                    >
                        Back to Course
                    </Link>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50">
            <div className="container mx-auto px-4 py-8">
                {/* Breadcrumb */}
                <Link
                    href={`/student/${courseId}`}
                    className="inline-flex items-center text-indigo-600 hover:text-indigo-800 mb-6 transition-colors"
                >
                    <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
                    </svg>
                    Back to Course
                </Link>

                <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
                    {/* Side Navigation - Lesson List */}
                    <div className="lg:col-span-1">
                        <LessonSideNavigation
                            courseId={courseId}
                            currentLessonId={lessonId}
                            lessons={allLessons}
                        />
                    </div>

                    {/* Main Content */}
                    <div className="lg:col-span-3">
                        <div className="bg-white rounded-2xl shadow-xl overflow-hidden">
                            <div className="bg-gradient-to-r from-blue-500 to-indigo-600 h-2"></div>

                            <div className="p-6 md:p-8">
                                {/* Lesson Header */}
                                <div className="mb-6">
                                    <h1 className="text-3xl md:text-4xl font-bold text-gray-900 mb-4">
                                        {lesson.title}
                                    </h1>

                                    {isCompleted && (
                                        <div className="inline-flex items-center gap-2 bg-green-50 px-4 py-2 rounded-full">
                                            <svg className="w-5 h-5 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                                                <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd" />
                                            </svg>
                                            <span className="text-sm font-medium text-green-700">Completed</span>
                                        </div>
                                    )}
                                </div>

                                {/* Media Content */}
                                <div className="mb-8">
                                    {renderMedia()}
                                </div>

                                {/* Lesson Content */}
                                {lesson.content && (
                                    <div className="mb-8">
                                        <h2 className="text-xl font-semibold text-gray-900 mb-4">Lesson Content</h2>
                                        <div className="prose max-w-none text-gray-700 whitespace-pre-wrap">
                                            {lesson.content}
                                        </div>
                                    </div>
                                )}

                                {/* Mark as Completed Button */}
                                {!isCompleted && (
                                    <div className="border-t border-gray-200 pt-6">
                                        <button
                                            onClick={handleMarkAsCompleted}
                                            disabled={markingComplete}
                                            className="w-full md:w-auto px-8 py-4 bg-gradient-to-r from-green-500 to-emerald-600 text-white font-semibold rounded-lg hover:from-green-600 hover:to-emerald-700 transition-all shadow-lg hover:shadow-xl disabled:opacity-50 disabled:cursor-not-allowed"
                                        >
                                            {markingComplete ? (
                                                <span className="flex items-center justify-center gap-2">
                                                    <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></div>
                                                    Marking...
                                                </span>
                                            ) : (
                                                <span className="flex items-center justify-center gap-2">
                                                    <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                                                    </svg>
                                                    Mark as Completed
                                                </span>
                                            )}
                                        </button>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
