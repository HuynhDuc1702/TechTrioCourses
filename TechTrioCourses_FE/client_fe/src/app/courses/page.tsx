"use client";

import { useEffect, useState } from "react";
import { courseAPI, CourseResponse, CourseStatusEnum } from "@/services/courseAPI";
import Link from "next/link";

export default function CoursesPage() {
  const [courses, setCourses] = useState<CourseResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const data = await courseAPI.getAllCourses();
          const myCourses = data.filter(course => course.status === CourseStatusEnum.Published);
        setCourses(myCourses);
      } catch (err) {
        setError(err instanceof Error ? err.message : "An unknown error occurred");
      } finally {
        setLoading(false);
      }
    };

    fetchCourses();
  }, []);

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
        <div className="text-center">
          <div className="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mb-4"></div>
          <div className="text-xl text-gray-700">Loading courses...</div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
        <div className="bg-red-50 border-l-4 border-red-500 p-6 rounded-lg shadow-lg">
          <p className="text-red-700 font-semibold">Error: {error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50">
      <div className="container mx-auto px-4 py-12">
        <div className="text-center mb-12">
          <h1 className="text-5xl font-bold mb-4 bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent">
            Available Courses
          </h1>
          <p className="text-gray-600 text-lg">Explore our collection of learning opportunities</p>
        </div>
        
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {courses.map((course) => (
            <div
              key={course.id}
              className="group bg-white rounded-xl shadow-lg hover:shadow-2xl transition-all duration-300 overflow-hidden border border-gray-100 hover:border-indigo-200 hover:-translate-y-1"
            >
              <div className="bg-gradient-to-r from-blue-500 to-indigo-600 h-2"></div>
              
              <div className="p-6">
                <h2 className="text-2xl font-bold mb-3 text-gray-800 group-hover:text-indigo-600 transition-colors">
                  {course.title}
                </h2>
                
                <p className="text-gray-600 mb-6 line-clamp-3 leading-relaxed">
                  {course.description || "No description available."}
                </p>
                
                <div className="flex flex-wrap gap-2 mb-6">
                  <span className="inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-indigo-100 text-indigo-700">
                    {course.categoryName || "Uncategorized"}
                  </span>
                  <span className="inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-gray-100 text-gray-700">
                    By {course.creatorName || "Unknown"}
                  </span>
                </div>
                
                <div className="border-t border-gray-200 pt-4 mb-4 flex justify-between items-center">
                  <div className="flex items-center text-sm text-gray-600">
                    <svg className="w-4 h-4 mr-1 text-indigo-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
                    </svg>
                    {course.totalLessons} Lessons
                  </div>
                  <div className="flex items-center text-sm font-semibold text-amber-600">
                    <svg className="w-4 h-4 mr-1 fill-current" viewBox="0 0 20 20">
                      <path d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/>
                    </svg>
                    {course.averageRating.toFixed(1)}
                  </div>
                </div>

                <Link 
                  href={`/courses/${course.id}`}
                  className="block w-full text-center bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-semibold py-2 px-4 rounded-lg hover:from-blue-600 hover:to-indigo-700 transition-all duration-300"
                >
                  View Details
                </Link>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
