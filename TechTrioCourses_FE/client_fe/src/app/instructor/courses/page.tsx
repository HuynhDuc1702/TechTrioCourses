"use client";

import { useState, useEffect } from 'react';
import { courseAPI, CourseResponse } from '@/services/courseAPI';
import { categoryAPI, CategoryResponse } from '@/services/categoryAPI';
import { useAuth } from '@/contexts/AuthContext';
import { useRouter } from 'next/navigation';
import { UserRoleEnum } from '@/services/userAPI';
import { CourseCreateRequest } from '@/services/courseAPI';
import { CourseUpdateRequest } from '@/services/courseAPI';
import CourseModal from '@/components/course/CourseModal';
import { CourseStatusEnum } from '@/services/courseAPI';
import Link from 'next/link';

export default function InstructorCoursesPage() {
  const [courses, setCourses] = useState<CourseResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [modalMode, setModalMode] = useState<'create' | 'edit'>('create');
  const [selectedCourse, setSelectedCourse] = useState<CourseResponse | null>(null);
  const [categories, setCategories] = useState<CategoryResponse[]>([]);
  const [loadingCategories, setLoadingCategories] = useState(false);
  const { user } = useAuth();
  const router = useRouter();

  // Form state
  type CourseFormData = {
    title: string;
    description?: string;
    categoryId?: string; 
  };

  const [formData, setFormData] = useState<CourseFormData>({
    title: '',
    description: undefined,
    categoryId: undefined, 
  });
  //Load categories for selection
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        setLoadingCategories(true);
        const data = await categoryAPI.getAllCategories();
        setCategories(data);
      } catch (error) {
        console.error('Failed to load categories', error);
      } finally {
        setLoadingCategories(false);
      }
    };

    fetchCategories();
  }, []);

  useEffect(() => {
    // Check if user is instructor or admin
    if (user && user.role !== UserRoleEnum.Instructor && user.role !== UserRoleEnum.Admin) {
      router.push('/');
      return;
    }
    loadCourses();
  }, [user]);

  const loadCourses = async () => {
    try {
      setLoading(true);
      const data = await courseAPI.getAllCourses();
      // Filter courses created by current user
      const myCourses = data.filter(course => course.creatorId === user?.userId);
      setCourses(myCourses);
    } catch (err: any) {
      setError(err.message || 'Failed to load courses');
    } finally {
      setLoading(false);
    }
  };

  const openCreateModal = () => {
    setModalMode('create');
    setFormData({ title: '', description: '', categoryId: '' });
    setSelectedCourse(null);
    setShowModal(true);
  };

  const openEditModal = (course: CourseResponse) => {
    setModalMode('edit');
    setFormData({
      title: course.title,
      description: course.description || '',
      categoryId: course.categoryId || '',
    });
    setSelectedCourse(course);
    setShowModal(true);
  };

 
 

  const getStatusBadge = (status: CourseStatusEnum) => {
    const statusMap: Record<
      CourseStatusEnum,
      { label: string; color: string }
    > = {
      [CourseStatusEnum.Hidden]: {
        label: 'Hidden',
        color: 'bg-gray-100 text-gray-700',
      },
      [CourseStatusEnum.Published]: {
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
        <div className="text-gray-600">Loading courses...</div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="container mx-auto px-4">
        <div className="flex justify-between items-center mb-8">
          <div>
            <h1 className="text-3xl font-bold text-gray-900">My Courses</h1>
            <p className="text-gray-600 mt-1">Manage your courses</p>
          </div>
          <button
            onClick={openCreateModal}
            className="px-6 py-3 bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-semibold rounded-lg hover:from-blue-600 hover:to-indigo-700 transition-all"
          >
            + Create New Course
          </button>
        </div>

        {error && (
          <div className="mb-6 bg-red-50 border-l-4 border-red-500 p-4 rounded">
            <p className="text-red-700 text-sm">{error}</p>
          </div>
        )}

        {courses.length === 0 ? (
          <div className="bg-white rounded-lg shadow-sm p-12 text-center">
            <p className="text-gray-500 text-lg">No courses yet. Create your first course to get started!</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {courses.map((course) => (
              <div key={course.id} className="bg-white rounded-lg shadow-sm hover:shadow-md transition-shadow p-6">
                <div className="flex justify-between items-start mb-4">
                  <h3 className="text-xl font-semibold text-gray-900 flex-1">{course.title}</h3>
                  {getStatusBadge(course.status)}
                </div>

                <p className="text-gray-600 text-sm mb-4 line-clamp-3">
                  {course.description || 'No description'}
                </p>

                <div className="flex items-center gap-4 text-sm text-gray-500 mb-4">
                  <span>📚 {course.totalLessons} lessons</span>
                  <span>📝 {course.totalQuizzes} quizzes</span>
                </div>

                {course.categoryName && (
                  <div className="text-xs text-gray-500 mb-4">
                    Category: {course.categoryName}
                  </div>
                )}

                <div className="flex gap-2">
                  <button
                    onClick={() => openEditModal(course)}
                    className="flex-1 px-4 py-2 bg-blue-50 text-blue-600 rounded-lg hover:bg-blue-100 transition-colors font-medium"
                  >
                    Edit
                  </button>
                  <Link 
                  href={`/instructor/courses/${course.id}`}
                  className="block w-full text-center bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-semibold py-2 px-4 rounded-lg hover:from-blue-600 hover:to-indigo-700 transition-all duration-300"
                >
                  View Details
                </Link>
                </div>
              </div>
            ))}
          </div>
        )}

      <CourseModal
          open={showModal}
          mode={modalMode}
          course={selectedCourse}
          courseStatus={selectedCourse ? selectedCourse.status : CourseStatusEnum.Hidden}
          categories={categories}
          loadingCategories={loadingCategories}
          submitting={submitting}
          onClose={() => setShowModal(false)}
          onSubmit={async (data) => {
            if (modalMode === 'create') {
              await courseAPI.createCourse({
                title: data.title!,
                description: data.description || null,
                categoryId: data.categoryId || null,
                creatorId: user?.userId || null,
                status: data.status!,
              });
            } else if (selectedCourse) {
              await courseAPI.updateCourse(selectedCourse.id, {
                title: data.title,
                description: data.description || null,
                categoryId: data.categoryId || null,
                status: data.status!,
              });
            }

            setShowModal(false);
            await loadCourses();
          }}
        />
      </div>
    </div>
  );
}