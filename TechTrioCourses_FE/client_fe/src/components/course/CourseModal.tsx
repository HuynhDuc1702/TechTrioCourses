'use client';

import { useState, useEffect } from 'react';
import { CategoryResponse } from '@/services/categoryAPI';
import {
  CourseCreateRequest,
  CourseUpdateRequest,
  CourseResponse,
  CourseStatusEnum,
} from '@/services/courseAPI';

type Props = {
  mode: 'create' | 'edit';
  open: boolean;
  loadingCategories: boolean;
  categories: CategoryResponse[];
  course?: CourseResponse | null;
  courseStatus: CourseStatusEnum;
  submitting: boolean;
  onClose: () => void;
  onSubmit: (
    data: Partial<CourseCreateRequest | CourseUpdateRequest>
  ) => Promise<void>;
};

type FormData = {
  title: string;
  description?: string;
  categoryId?: string;
  courseStatus: CourseStatusEnum;
};

export default function CourseModal({
  mode,
  open,
  categories,
  loadingCategories,
  course,
  courseStatus,
  submitting,
  onClose,
  onSubmit,
}: Props) {
  const [formData, setFormData] = useState<FormData>({
    title: '',
    description: '',
    categoryId: undefined,
    courseStatus: CourseStatusEnum.Hidden,
  });

  // Update form data when course changes or modal opens
  useEffect(() => {
    if (open && mode === 'edit' && course) {
      setFormData({
        title: course.title ?? '',
        description: course.description ?? '',
        categoryId: course.categoryId ?? undefined,
        courseStatus: course.status,
      });
    } else if (open && mode === 'create') {
      setFormData({
        title: '',
        description: '',
        categoryId: undefined,
        courseStatus: courseStatus,
      });
    }
  }, [open, mode, course, courseStatus]);

  if (!open) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    await onSubmit({
      title: formData.title,
      description: formData.description || null,
      categoryId: formData.categoryId || null,
      status: formData.courseStatus,
    });
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-2xl shadow-xl max-w-2xl w-full p-8">
        <h2 className="text-2xl font-bold mb-6">
          {mode === 'create' ? 'Create Course' : 'Edit Course'}
        </h2>

        <form onSubmit={handleSubmit} className="space-y-6">
          {/* Title */}
          <div>
            <label className="block text-sm font-medium text-black mb-2">
              Course Title *
            </label>
            <input
              value={formData.title}
              onChange={(e) =>
                setFormData({ ...formData, title: e.target.value })
              }
              required
              className="w-full px-4 py-3 border rounded-lg text-black"
            />
          </div>

          {/* Description */}
          <div>
            <label className="block text-sm font-medium text-black mb-2">
              Description
            </label>
            <textarea
              rows={4}
              value={formData.description}
              onChange={(e) =>
                setFormData({ ...formData, description: e.target.value })
              }
              className="w-full px-4 py-3 border rounded-lg text-black"
            />
          </div>

          {/* Category */}
          <div>
            <label className="block text-sm font-medium text-black mb-2">
              Category
            </label>
            <select
              value={formData.categoryId ?? ''}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  categoryId: e.target.value || undefined,
                })
              }
              disabled={loadingCategories}
              className="w-full px-4 py-3 border rounded-lg text-black"
            >
              <option value="">-- Select category --</option>
              {categories.map((cat) => (
                <option key={cat.id} value={cat.id}>
                  {cat.name}
                </option>
              ))}
            </select>
          </div>
          <div>
            <label
              htmlFor="status"
              className="block text-sm font-medium text-black mb-2"
            >
              Status
            </label>

            <select
              id="status"
              value={formData.courseStatus}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  courseStatus: parseInt(e.target.value) as CourseStatusEnum,
                })
              }
              className="w-full px-4 py-3 border border-gray-300 rounded-lg text-black
               focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
            >
              <option value={CourseStatusEnum.Hidden}>Hidden</option>
              <option value={CourseStatusEnum.Published}>Published</option>
            </select>
          </div>


          {/* Actions */}
          <div className="flex gap-4">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 border border-red-500 text-red-600 px-6 py-3 rounded-lg
             hover:bg-red-50 transition-colors"
            >
              Cancel
            </button>

            <button
              type="submit"
              disabled={submitting}
              className="flex-1 bg-indigo-600 text-white px-6 py-3 rounded-lg"
            >
              {submitting
                ? 'Saving...'
                : mode === 'create'
                  ? 'Create'
                  : 'Update'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
