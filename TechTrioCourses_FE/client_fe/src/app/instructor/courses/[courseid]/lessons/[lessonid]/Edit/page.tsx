"use client";
import { useState, useEffect } from "react";
import { useParams, useRouter } from "next/navigation";
import { lessonAPI, LessonStatusEnum, LessonUpdateRequest, LessonResponse } from "@/services/lessonAPI";
import { courseAPI, CourseResponse } from "@/services/courseAPI";
import { useAuth } from "@/contexts/AuthContext";
import { UserRoleEnum } from "@/services/userAPI";
import Link from "next/link";

export default function EditLessonPage() {
  const params = useParams();
  const router = useRouter();
  const { user } = useAuth();
  const [course, setCourse] = useState<CourseResponse | null>(null);
  const [lesson, setLesson] = useState<LessonResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [formData, setFormData] = useState<{
    title: string;
    content?: string;
    mediaUrl?: string;
    mediaType?: string;
    orderIndex?: number;
    status: LessonStatusEnum;
  }>({
    title: '',
    content: '',
    mediaUrl: '',
    mediaType: '',
    orderIndex: 0,
    status: LessonStatusEnum.Hidden,
  });

  useEffect(() => {
    // Check if user is instructor or admin
    if (user && user.role !== UserRoleEnum.Instructor && user.role !== UserRoleEnum.Admin) {
      router.push('/');
      return;
    }
    loadData();
  }, [user, params.courseid, params.lessonid]);

  const loadData = async () => {
    try {
      setLoading(true);
      
      // Load course
      const courseData = await courseAPI.getCourseById(params.courseid as string);
      setCourse(courseData);
      
      // Load lesson - params.id is the dynamic route parameter for lesson ID
      // We need to get the lesson ID from the URL params
      const lessonId = Array.isArray(params.lessonid) ? params.lessonid[params.lessonid.length - 1] : params.lessonid;
      
      // Actually, the route structure is [courseId]/lessons/[lessonId]
      // So we need to access the lesson ID from a different param
      // Let me check the route structure - it should be lessons/[id]/page.tsx
      // where [id] is the lesson ID, not course ID
      
      const lessonData = await lessonAPI.getLessonById(lessonId as string);
      setLesson(lessonData);
      
      // Populate form
      setFormData({
        title: lessonData.title,
        content: lessonData.content || '',
        mediaUrl: lessonData.mediaUrl || '',
        mediaType: lessonData.mediaType || '',
        orderIndex: lessonData.orderIndex || 0,
        status: lessonData.status,
      });
    } catch (err: any) {
      setError(err.message || 'Failed to load lesson');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.title.trim()) {
      alert('Lesson title is required');
      return;
    }

    if (!lesson) return;

    try {
      setSubmitting(true);
      setError(null);
      
      const updateData: LessonUpdateRequest = {
        courseId: lesson.courseId,
        title: formData.title,
        content: formData.content || null,
        mediaUrl: formData.mediaUrl || null,
        mediaType: formData.mediaType || null,
        orderIndex: formData.orderIndex || null,
        status: formData.status,
      };
      
      await lessonAPI.updateLesson(lesson.id, updateData);
      router.push(`/instructor/courses/${lesson.courseId}/lessons`);
    } catch (err: any) {
      setError(err.message || 'Failed to update lesson');
    } finally {
      setSubmitting(false);
    }
  };

  const handleCancel = () => {
    if (lesson) {
      router.push(`/instructor/courses/${lesson.courseId}/lessons`);
    } else {
      router.back();
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-gray-600">Loading lesson...</div>
      </div>
    );
  }

  if (!lesson) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-gray-600">Lesson not found</div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="container mx-auto px-4 max-w-3xl">
        {/* Breadcrumb */}
        <div className="mb-4">
          <Link 
            href={`/instructor/courses/${lesson.courseId}/lessons`}
            className="text-blue-600 hover:underline"
          >
            ← Back to Lessons
          </Link>
        </div>

        {/* Header */}
        <div className="bg-white rounded-lg shadow-sm p-6 mb-6">
          <h1 className="text-3xl font-bold text-gray-900">Edit Lesson</h1>
          {course && (
            <p className="text-gray-600 mt-2">
              Course: <span className="font-medium">{course.title}</span>
            </p>
          )}
        </div>

        {/* Error Message */}
        {error && (
          <div className="mb-6 bg-red-50 border-l-4 border-red-500 p-4 rounded">
            <p className="text-red-700 text-sm">{error}</p>
          </div>
        )}

        {/* Form */}
        <div className="bg-white rounded-lg shadow-sm p-6">
          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Title */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Lesson Title <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                value={formData.title}
                onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
                placeholder="Enter lesson title"
                required
              />
            </div>

            {/* Content */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Content
              </label>
              <textarea
                value={formData.content}
                onChange={(e) => setFormData({ ...formData, content: e.target.value })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
                placeholder="Enter lesson content (optional)"
                rows={8}
              />
              <p className="text-xs text-gray-500 mt-1">
                Provide detailed content for the lesson
              </p>
            </div>

            {/* Media URL */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Media URL
              </label>
              <input
                type="url"
                value={formData.mediaUrl}
                onChange={(e) => setFormData({ ...formData, mediaUrl: e.target.value })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
                placeholder="https://example.com/video.mp4"
              />
              <p className="text-xs text-gray-500 mt-1">
                URL to video, audio, or other media content
              </p>
            </div>

            {/* Media Type */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Media Type
              </label>
              <select
                value={formData.mediaType}
                onChange={(e) => setFormData({ ...formData, mediaType: e.target.value })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
              >
                <option value="">Select media type</option>
                <option value="video">Video</option>
                <option value="audio">Audio</option>
                <option value="document">Document</option>
                <option value="image">Image</option>
              </select>
            </div>

            {/* Order Index */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Order Index
              </label>
              <input
                type="number"
                value={formData.orderIndex}
                onChange={(e) => setFormData({ ...formData, orderIndex: parseInt(e.target.value) || 0 })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
                placeholder="Order index"
                min="0"
              />
              <p className="text-xs text-gray-500 mt-1">
                Determines the order of lessons (lower numbers appear first)
              </p>
            </div>

            {/* Status */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Status
              </label>
              <select
                value={formData.status}
                onChange={(e) => setFormData({ ...formData, status: parseInt(e.target.value) as LessonStatusEnum })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
              >
                <option value={LessonStatusEnum.Hidden}>Hidden</option>
                <option value={LessonStatusEnum.Published}>Published</option>
              </select>
              <p className="text-xs text-gray-500 mt-1">
                Hidden lessons are not visible to students
              </p>
            </div>

            {/* Action Buttons */}
            <div className="flex gap-3 pt-4">
              <button
                type="button"
                onClick={handleCancel}
                className="flex-1 px-6 py-3 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors font-medium"
                disabled={submitting}
              >
                Cancel
              </button>
              <button
                type="submit"
                className="flex-1 px-6 py-3 bg-gradient-to-r from-blue-500 to-indigo-600 text-white rounded-lg hover:from-blue-600 hover:to-indigo-700 transition-all font-medium disabled:opacity-50 disabled:cursor-not-allowed"
                disabled={submitting}
              >
                {submitting ? 'Updating...' : 'Update Lesson'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
