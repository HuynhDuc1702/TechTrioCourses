"use client";

import { useState, useEffect } from 'react';
import { courseAPI, Course } from '@/services/courseAPI';
import { useAuth } from '@/contexts/AuthContext';
import { useRouter } from 'next/navigation';
import { UserRoleEnum } from '@/services/userAPI';

export default function InstructorCoursesPage() {
  const [courses, setCourses] = useState<Course[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [modalMode, setModalMode] = useState<'create' | 'edit'>('create');
  const [selectedCourse, setSelectedCourse] = useState<Course | null>(null);
  const { user } = useAuth();
  const router = useRouter();

  // Form state
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    categoryId: '',
  });

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

  const openEditModal = (course: Course) => {
    setModalMode('edit');
    setFormData({
      title: course.title,
      description: course.description || '',
      categoryId: course.categoryId || '',
    });
    setSelectedCourse(course);
    setShowModal(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSubmitting(true);

    console.log('Form submitted with data:', formData);
    console.log('User ID:', user?.userId);

    try {
      if (modalMode === 'create') {
        console.log('Creating course...');
        const courseData = {
          title: formData.title,
          description: formData.description || null,
          categoryId: formData.categoryId || null,
          creatorId: user?.userId || null,
          status: 0, // Draft status
        };
        console.log('Sending course data:', courseData);
        const result = await courseAPI.createCourse(courseData);
        console.log('Course created successfully:', result);
      } else if (selectedCourse) {
        console.log('Updating course:', selectedCourse.id);
        const updateData = {
          title: formData.title,
          description: formData.description || null,
          categoryId: formData.categoryId || null,
        };
        const result = await courseAPI.updateCourse(selectedCourse.id, updateData);
        console.log('Course updated successfully:', result);
      }
      
      setShowModal(false);
      await loadCourses();
    } catch (err: any) {
      console.error('Error submitting form:', err);
      console.error('Error response:', err.response?.data);
      console.error('Validation errors:', err.response?.data?.errors);
      
      let errorMessage = 'Operation failed';
      
      // Handle validation errors from ASP.NET Core
      if (err.response?.data?.errors) {
        const errors = err.response.data.errors;
        const errorMessages = Object.entries(errors).map(([field, messages]: [string, any]) => {
          return `${field}: ${Array.isArray(messages) ? messages.join(', ') : messages}`;
        });
        errorMessage = errorMessages.join('\n');
      } else if (err.response?.data?.title) {
        errorMessage = err.response.data.title;
      } else if (err.response?.data?.message) {
        errorMessage = err.response.data.message;
      } else if (err.message) {
        errorMessage = err.message;
      }
      
      setError(errorMessage);
    } finally {
      setSubmitting(false);
    }
  };

  const handleDisable = async (id: string) => {
    if (!confirm('Are you sure you want to disable this course? Students will no longer be able to access it.')) {
      return;
    }

    try {
      await courseAPI.disableCourse(id);
      await loadCourses();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to disable course');
    }
  };

  const getStatusBadge = (status: number) => {
    const statusMap: { [key: number]: { label: string; color: string } } = {
      0: { label: 'Draft', color: 'bg-gray-100 text-gray-700' },
      1: { label: 'Published', color: 'bg-green-100 text-green-700' },
      2: { label: 'Disabled', color: 'bg-red-100 text-red-700' },
    };
    const statusInfo = statusMap[status] || { label: 'Unknown', color: 'bg-gray-100 text-gray-700' };
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
                  {course.status !== 2 && (
                    <button
                      onClick={() => handleDisable(course.id)}
                      className="flex-1 px-4 py-2 bg-red-50 text-red-600 rounded-lg hover:bg-red-100 transition-colors font-medium"
                    >
                      Disable
                    </button>
                  )}
                </div>
              </div>
            ))}
          </div>
        )}

        {/* Modal */}
        {showModal && (
          <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
            <div className="bg-white rounded-2xl shadow-xl max-w-2xl w-full p-8">
              <h2 className="text-2xl font-bold text-gray-900 mb-6">
                {modalMode === 'create' ? 'Create New Course' : 'Edit Course'}
              </h2>

              {error && (
                <div className="mb-4 bg-red-50 border-l-4 border-red-500 p-4 rounded">
                  <p className="text-red-700 text-sm">{error}</p>
                </div>
              )}

              <form onSubmit={handleSubmit} className="space-y-6">
                <div>
                  <label htmlFor="title" className="block text-sm font-medium text-gray-700 mb-2">
                    Course Title *
                  </label>
                  <input
                    id="title"
                    type="text"
                    value={formData.title}
                    onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                    required
                    className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent text-gray-900"
                    placeholder="Enter course title"
                  />
                </div>

                <div>
                  <label htmlFor="description" className="block text-sm font-medium text-gray-700 mb-2">
                    Description
                  </label>
                  <textarea
                    id="description"
                    value={formData.description}
                    onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                    rows={4}
                    className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent text-gray-900"
                    placeholder="Enter course description"
                  />
                </div>

                <div>
                  <label htmlFor="categoryId" className="block text-sm font-medium text-gray-700 mb-2">
                    Category ID (Optional)
                  </label>
                  <input
                    id="categoryId"
                    type="text"
                    value={formData.categoryId}
                    onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
                    className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent text-gray-900"
                    placeholder="Enter category ID"
                  />
                </div>

                <div className="flex gap-4">
                  <button
                    type="button"
                    onClick={() => setShowModal(false)}
                    disabled={submitting}
                    className="flex-1 px-6 py-3 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition-colors font-medium disabled:opacity-50 disabled:cursor-not-allowed"
                  >
                    Cancel
                  </button>
                  <button
                    type="submit"
                    disabled={submitting}
                    className="flex-1 px-6 py-3 bg-gradient-to-r from-blue-500 to-indigo-600 text-white rounded-lg hover:from-blue-600 hover:to-indigo-700 transition-all font-medium disabled:opacity-50 disabled:cursor-not-allowed"
                  >
                    {submitting ? 'Saving...' : (modalMode === 'create' ? 'Create Course' : 'Update Course')}
                  </button>
                </div>
              </form>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
