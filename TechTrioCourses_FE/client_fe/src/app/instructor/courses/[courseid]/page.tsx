"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { courseAPI, CourseResponse, CourseStatusEnum } from "@/services/courseAPI";
import Link from "next/link";

export default function CourseDetailPage() {
  const params = useParams();
  const router = useRouter();
  const [course, setCourse] = useState<CourseResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchCourse = async () => {
      try {
        const id = params.courseid as string;
        const data = await courseAPI.getCourseById(id);

        setCourse(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : "An unknown error occurred");
      } finally {
        setLoading(false);
      }
    };

    if (params.courseid) {
      fetchCourse();
    }
  }, [params.courseid]);


  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
        <div className="text-center">
          <div className="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mb-4"></div>
          <div className="text-xl text-gray-700">Loading course details...</div>
        </div>
      </div>
    );
  }

  if (error || !course) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
        <div className="bg-red-50 border-l-4 border-red-500 p-6 rounded-lg shadow-lg">
          <p className="text-red-700 font-semibold">Error: {error || "Course not found"}</p>
          <Link
            href="/courses"
            className="mt-4 inline-block text-indigo-600 hover:text-indigo-800 underline"
          >
            Back to Courses
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50">
      <div className="container mx-auto px-4 py-12">
        <Link
          href="/courses"
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
            <div className="mb-6">
              <h1 className="text-4xl md:text-5xl font-bold mb-4 text-gray-800">
                {course.title}
              </h1>

              <div className="flex flex-wrap gap-3 mb-6">
                <span className="inline-flex items-center px-4 py-2 rounded-full text-sm font-medium bg-indigo-100 text-indigo-700">
                  {course.categoryName || "Uncategorized"}
                </span>
                <span className="inline-flex items-center px-4 py-2 rounded-full text-sm font-medium bg-gray-100 text-gray-700">
                  By {course.creatorName || "Unknown"}
                </span>
                <span className={`inline-flex items-center px-4 py-2 rounded-full text-sm font-medium ${course.status === CourseStatusEnum.Published ? "bg-green-100 text-green-700" : "bg-yellow-100 text-yellow-700"
                  }`}>
                  {course.status === CourseStatusEnum.Published ? "Active" : "Draft"}
                </span>
              </div>

              <div className="flex items-center gap-6 text-gray-600 mb-8">
                <div className="flex items-center">
                  <svg className="w-5 h-5 mr-2 text-indigo-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
                  </svg>
                  <span className="font-semibold">{course.totalLessons}</span>&nbsp;Lessons
                </div>
                <div className="flex items-center">
                  <svg className="w-5 h-5 mr-2 text-indigo-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                  </svg>
                  <span className="font-semibold">{course.totalQuizzes}</span>&nbsp;Quizzes
                </div>
                <div className="flex items-center text-amber-600 font-semibold">
                  <svg className="w-5 h-5 mr-2 fill-current" viewBox="0 0 20 20">
                    <path d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z" />
                  </svg>
                  {course.averageRating.toFixed(1)} Rating
                </div>
              </div>
            </div>

            <div className="border-t border-gray-200 pt-8">
              <h2 className="text-2xl font-bold mb-4 text-gray-800">About This Course</h2>
              <p className="text-gray-600 text-lg leading-relaxed whitespace-pre-wrap">
                {course.description || "No description available for this course."}
              </p>
            </div>

            {course.createdAt && (
              <div className="border-t border-gray-200 mt-8 pt-6 text-sm text-gray-500">
                <p>Created: {new Date(course.createdAt).toLocaleDateString()}</p>
                {course.updatedAt && (
                  <p>Last Updated: {new Date(course.updatedAt).toLocaleDateString()}</p>
                )}
              </div>
            )}

            <div className="mt-8">

              <Link
                href={`/instructor/courses/${course.id}/lessons`}
                className="block w-full text-center bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-semibold py-2 px-4 rounded-lg hover:from-blue-600 hover:to-indigo-700 transition-all duration-300"
              >
                Manage Lessons
              </Link>
              <Link
                href={`/instructor/courses/${course.id}/quizzes`}
                className="block w-full text-center bg-gradient-to-r from-yellow-500 to-pink-600 text-white font-semibold py-2 px-4 rounded-lg hover:from-purple-600 hover:to-pink-700 transition-all duration-300 mt-4"
              >
                Manage Quizzes
              </Link>
              <Link
                href={`/instructor/courses/${course.id}/questions`}
                className="block w-full text-center bg-gradient-to-r from-purple-500 to-pink-600 text-white font-semibold py-2 px-4 rounded-lg hover:from-purple-600 hover:to-pink-700 transition-all duration-300 mt-4"
              >
                Question Bank
              </Link>
              
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
