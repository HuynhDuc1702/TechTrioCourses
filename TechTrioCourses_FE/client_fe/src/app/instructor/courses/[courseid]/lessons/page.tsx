"use client";
import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { courseAPI, CourseResponse } from "@/services/courseAPI";
import { lessonAPI, LessonResponse, LessonStatusEnum } from "@/services/lessonAPI";
import Link from "next/link";
import { UserRoleEnum } from "@/services/userAPI";
import { useAuth } from "@/contexts/AuthContext";
import { lessonAxios } from '@/middleware/axiosMiddleware';

export default function InstructorCourseLessonsPage() {
  const params = useParams();
  const router = useRouter();
  const [lessons, setLessons] = useState<LessonResponse[]>([]);
  const [course, setCourse] = useState<CourseResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  const { user } = useAuth();

  useEffect(() => {
    // Check if user is instructor or admin
    if (user && user.role !== UserRoleEnum.Instructor && user.role !== UserRoleEnum.Admin) {
      router.push('/');
      return;
    }
    loadCourse();
    loadLessons();
  }, [user, params.id]);

  const loadCourse = async () => {
    try {
      const courseData = await courseAPI.getCourseById(params.courseid as string);
      setCourse(courseData);
    } catch (err: any) {
      setError(err.message || 'Failed to load course');
    }
  };
  const navigateToCreate = () => {
    router.push(`/instructor/courses/${params.courseid}/lessons/Create`);
  };

  const navigateToEdit = (lessonId: string) => {
    router.push(`/instructor/courses/${params.courseid}/lessons/${lessonId}/Edit`);
  };    

  const loadLessons = async () => {
    try {
      setLoading(true);
      const data = await lessonAPI.getLessonsByCourseId(params.courseid as string);
      // Sort by orderIndex
      const sortedLessons = data.sort((a, b) => (a.orderIndex || 0) - (b.orderIndex || 0));
      setLessons(sortedLessons);
    } catch (err: any) {
      setError(err.message || 'Failed to load lessons');
    } finally {
      setLoading(false);
    }
  };

  

  const handleToggleStatus = async (lesson: LessonResponse) => {
    try {
      const newStatus = lesson.status === LessonStatusEnum.Published 
        ? LessonStatusEnum.Hidden 
        : LessonStatusEnum.Published;
      
      await lessonAPI.updateLesson(lesson.id, {
        courseId: lesson.courseId,
        title: lesson.title,
        content: lesson.content,
        mediaUrl: lesson.mediaUrl,
        mediaType: lesson.mediaType,
        orderIndex: lesson.orderIndex,
        status: newStatus,
      });
      await loadLessons();
    } catch (err: any) {
      alert(err.message || 'Failed to update lesson status');
    }
  };

  const getStatusBadge = (status: LessonStatusEnum) => {
    const statusMap: Record<LessonStatusEnum, { label: string; color: string }> = {
      [LessonStatusEnum.Hidden]: {
        label: 'Hidden',
        color: 'bg-gray-100 text-gray-700',
      },
      [LessonStatusEnum.Published]: {
        label: 'Published',
        color: 'bg-green-100 text-green-700',
      },
    };

    const statusInfo = statusMap[status];
    return (
      <span className={`px-3 py-1 rounded-full text-xs font-medium ${statusInfo.color}`}>
        {statusInfo.label}
      </span>
    );
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-gray-600">Loading lessons...</div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="container mx-auto px-4">
        {/* Breadcrumb */}
        <div className="mb-4">
          <Link href="/instructor/courses" className="text-blue-600 hover:underline">
            ← Back to Courses
          </Link>
        </div>

        {/* Header */}
        <div className="bg-white rounded-lg shadow-sm p-6 mb-6">
          <div className="flex justify-between items-start">
            <div>
              <h1 className="text-3xl font-bold text-gray-900">{course?.title}</h1>
              <p className="text-gray-600 mt-2">{course?.description}</p>
              <div className="mt-3 flex items-center gap-4 text-sm text-gray-500">
                <span>📚 {lessons.length} lessons</span>
                <span>📝 {course?.totalQuizzes || 0} quizzes</span>
              </div>
            </div>
            <button
              onClick={navigateToCreate}
              className="px-6 py-3 bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-semibold rounded-lg hover:from-blue-600 hover:to-indigo-700 transition-all"
            >
              + Create New Lesson
            </button>
          </div>
        </div>

        {error && (
          <div className="mb-6 bg-red-50 border-l-4 border-red-500 p-4 rounded">
            <p className="text-red-700 text-sm">{error}</p>
          </div>
        )}

        {/* Lessons List */}
        {lessons.length === 0 ? (
          <div className="bg-white rounded-lg shadow-sm p-12 text-center">
    

      <p className="text-gray-500 text-lg">No lessons yet. Create your first lesson to get started!</p>
          </div>
        ) : (
          <div className="space-y-4">
            {lessons.map((lesson, index) => (
              <div key={lesson.id} className="bg-white rounded-lg shadow-sm hover:shadow-md transition-shadow p-6">
                <div className="flex items-start gap-4">
                  {/* Order Number */}
                  <div className="flex-shrink-0 w-12 h-12 bg-gradient-to-br from-blue-500 to-indigo-600 text-white rounded-lg flex items-center justify-center font-bold text-lg">
                    {lesson.orderIndex || index + 1}
                  </div>

                  {/* Lesson Content */}
                  <div className="flex-1">
                    <div className="flex justify-between items-start mb-2">
                      <h3 className="text-xl font-semibold text-gray-900">{lesson.title}</h3>
                      {getStatusBadge(lesson.status)}
                    </div>

                    {lesson.content && (
                      <p className="text-gray-600 text-sm mb-3 line-clamp-2">{lesson.content}</p>
                    )}

                    {lesson.mediaUrl && (
                      <div className="text-xs text-gray-500 mb-3">
                        🎥 Media: {lesson.mediaType || 'Unknown'} - {lesson.mediaUrl}
                      </div>
                    )}

                    <div className="text-xs text-gray-400">
                      Updated: {new Date(lesson.updatedAt).toLocaleDateString()}
                    </div>
                  </div>

                  {/* Action Buttons */}
                  <div className="flex gap-2">
                    <button
                      onClick={() => handleToggleStatus(lesson)}
                      className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                        lesson.status === LessonStatusEnum.Published
                          ? 'bg-gray-50 text-gray-600 hover:bg-gray-100'
                          : 'bg-green-50 text-green-600 hover:bg-green-100'
                      }`}
                      title={lesson.status === LessonStatusEnum.Published ? 'Hide lesson' : 'Publish lesson'}
                    >
                      {lesson.status === LessonStatusEnum.Published ? '👁️ Hide' : '🚀 Publish'}
                    </button>
                    <button
                      onClick={() => navigateToEdit(lesson.id)}
                      className="px-4 py-2 bg-blue-50 text-blue-600 rounded-lg hover:bg-blue-100 transition-colors font-medium"
                    >
                      ✏️ Edit
                    </button>
                   
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}