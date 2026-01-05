"use client";
import { useState, useEffect } from "react";
import { useParams, useRouter } from "next/navigation";
import { quizAPI, QuizCreateRequest, QuizStatusEnum, QuizResponse } from "@/services/quizAPI";
import { courseAPI, CourseResponse } from "@/services/courseAPI";
import { useAuth } from "@/contexts/AuthContext";
import { UserRoleEnum } from "@/services/userAPI";
import Link from "next/link";

export default function EditQuizPage() {
  const params = useParams();
  const router = useRouter();
  const { user } = useAuth();
  const [course, setCourse] = useState<CourseResponse | null>(null);
  const [quiz, setQuiz] = useState<QuizResponse | null>(null);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null); 

 // Form state
  const [formData, setFormData] = useState<{
        name: string;
        description?: string;
        totalMarks?: number;
        durationMinutes?: number;
        status: QuizStatusEnum;
    }>({
        name: "",
        description: "",
        totalMarks : 0,
        durationMinutes: 0,
        status: QuizStatusEnum.Hidden,
    }
  );

  useEffect(() => {
    // Check if user is instructor or admin
    if (user && user.role !== UserRoleEnum.Instructor && user.role !== UserRoleEnum.Admin) {
      router.push('/');
      return;
    }
      loadData();
  }, [user, params]);
  const loadData = async () => {
    try {
      const courseData = await courseAPI.getCourseById(params.courseid as string);
        setCourse(courseData);

        const quizData = await quizAPI.getQuizById(params.quizzId as string);
        setQuiz(quizData);

        // Populate form data
        setFormData({
            name: quizData.name,
            description: quizData.description || "",
            totalMarks: quizData.totalMarks || 0,
            durationMinutes: quizData.durationMinutes || 0,
            status: quizData.QuizStatus,
        });
    } catch (err: any) {
        setError(err.message || 'Failed to load course');
    }
  };
const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.name.trim()) {
      alert('Quiz name is required');
      return;
    }

    try {
      setSubmitting(true);
      setError(null);

      const createData: QuizCreateRequest = {
        courseId: params.courseid as string,
        name: formData.name,
        description: formData.description || null,
        totalMarks: formData.totalMarks || undefined,
        durationMinutes: formData.durationMinutes || undefined,
        QuizStatus: formData.status,
      };

      await quizAPI.createQuiz(createData);
      router.push(`/instructor/courses/${params.courseid}/quizzes`);
    } catch (err: any) {
      setError(err.message || 'Failed to create quiz');
    } finally {
      setSubmitting(false);
    }
  };

  const handleCancel = () => {
    router.push(`/instructor/courses/${params.courseid}/lessons`);
  };
   if (!quiz) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-gray-600">Quiz not found</div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="container mx-auto px-4 max-w-3xl">
        {/* Breadcrumb */}
        <div className="mb-4">
          <Link
            href={`/instructor/courses/${params.courseid}`}
            className="text-blue-600 hover:underline"
          >
            ← Back to course's details
          </Link>
        </div>

        {/* Header */}
        <div className="bg-white rounded-lg shadow-sm p-6 mb-6">
          <h1 className="text-3xl font-bold text-gray-900">Create New Quiz</h1>
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
            {/* Name */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Quiz Name <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
                placeholder="Enter quiz name"
                required
              />
            </div>

            {/* Description */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Description
              </label>
              <textarea
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
                placeholder="Enter quiz description (optional)"
                rows={8}
              />
              <p className="text-xs text-gray-500 mt-1">
                Provide detailed description for the quiz
              </p>
            </div>

            {/* Total Marks */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Total Marks
              </label>
              <input
                type="number"
                value={formData.totalMarks}
                onChange={(e) => setFormData({ ...formData, totalMarks: parseInt(e.target.value) || 0 })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
                placeholder="Enter total marks"
                min="0"
              />
              <p className="text-xs text-gray-500 mt-1">
                Total marks for the quiz
              </p>
            </div>

            {/* Duration */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Duration (Minutes)
              </label>
               <input
                type="number"
                value={formData.durationMinutes}
                onChange={(e) => setFormData({ ...formData, durationMinutes: parseInt(e.target.value) || 0 })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
                placeholder="Enter duration in minutes"
                min="0"
              />
              <p className="text-xs text-gray-500 mt-1">
                Duration of the quiz in minutes
              </p>
             
            </div>

          

            {/* Status */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Status
              </label>
              <select
                value={formData.status}
                onChange={(e) => setFormData({ ...formData, status: parseInt(e.target.value) as QuizStatusEnum })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-gray-900"
              >
                <option value={QuizStatusEnum.Hidden}>Hidden</option>
                <option value={QuizStatusEnum.Published}>Published</option>
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
                {submitting ? 'Creating...' : 'Create Quiz'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}