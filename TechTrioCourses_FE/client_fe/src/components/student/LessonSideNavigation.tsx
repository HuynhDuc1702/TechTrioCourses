"use client";

import { LessonResponse } from "@/services/lessonAPI";
import { useRouter } from "next/navigation";

interface LessonSideNavigationProps {
    courseId: string;
    currentLessonId: string;
    lessons: LessonResponse[];
}

export default function LessonSideNavigation({ courseId, currentLessonId, lessons }: LessonSideNavigationProps) {
    const router = useRouter();

    const navigateToLesson = (lessonId: string) => {
        router.push(`/student/${courseId}/lessons/${lessonId}`);
    };

    return (
        <div className="bg-white rounded-2xl shadow-xl overflow-hidden sticky top-6">
            <div className="bg-gradient-to-r from-blue-500 to-indigo-600 p-4">
                <h2 className="text-white font-semibold text-lg">Lessons</h2>
            </div>
            <div className="max-h-[calc(100vh-200px)] overflow-y-auto">
                {lessons.map((lesson, index) => (
                    <button
                        key={lesson.id}
                        onClick={() => navigateToLesson(lesson.id)}
                        className={`w-full text-left p-4 border-b border-gray-200 transition-colors ${lesson.id === currentLessonId
                                ? 'bg-indigo-50 border-l-4 border-indigo-600'
                                : 'hover:bg-gray-50'
                            }`}
                    >
                        <div className="flex items-start gap-3">
                            <div className={`flex-shrink-0 w-8 h-8 rounded-full flex items-center justify-center text-sm font-semibold ${lesson.id === currentLessonId
                                    ? 'bg-indigo-600 text-white'
                                    : 'bg-gray-200 text-gray-600'
                                }`}>
                                {index + 1}
                            </div>
                            <div className="flex-1 min-w-0">
                                <p className={`text-sm font-medium truncate ${lesson.id === currentLessonId ? 'text-indigo-900' : 'text-gray-900'
                                    }`}>
                                    {lesson.title}
                                </p>
                            </div>
                        </div>
                    </button>
                ))}
            </div>
        </div>
    );
}
