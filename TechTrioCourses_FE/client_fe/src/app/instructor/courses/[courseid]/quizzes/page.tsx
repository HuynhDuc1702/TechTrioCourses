"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import {
    quizAPI,
    QuizResponse,
    QuizStatusEnum
} from "@/services/quizAPI";
import { useAuth } from "@/contexts/AuthContext";
import Link from "next/link";
import Swal from 'sweetalert2';

export default function QuizListPage() {
    const params = useParams();
    const { user } = useAuth();
    const [quizzes, setQuizzes] = useState<QuizResponse[]>([]);
    const [loading, setLoading] = useState(true);

    const courseId = params.courseid as string;

    useEffect(() => {
        if (user && courseId) {
            loadQuizzes();
        }
    }, [user, courseId]);

    const loadQuizzes = async () => {
        try {
            setLoading(true);
            const data = await quizAPI.getQuizzesByCourseId(courseId);
            setQuizzes(data);
        } catch (error) {
            console.error("Failed to load quizzes", error);
            Swal.fire("Error", "Failed to load quizzes", "error");
        } finally {
            setLoading(false);
        }
    };

    const handleDeleteQuiz = async (id: string) => {
        try {
            const result = await Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            });

            if (result.isConfirmed) {
                await quizAPI.deleteQuiz(id);
                Swal.fire('Deleted!', 'Quiz has been deleted.', 'success');
                loadQuizzes();
            }
        } catch (error) {
            console.error("Failed to delete quiz", error);
            Swal.fire("Error", "Failed to delete quiz", "error");
        }
    };

    const getStatusName = (status: number) => {
        switch (Number(status)) {
            case QuizStatusEnum.Published: return "Published";
            case QuizStatusEnum.Hidden: return "Hidden";
            default: return "Unknown";
        }
    };

    return (
        <div className="min-h-screen bg-gray-50 py-8">
            <div className="container mx-auto px-4">
                <div className="flex items-center mb-6">
                    <Link href={`/instructor/courses/${courseId}`} className="text-indigo-600 hover:text-indigo-800 mr-4">
                        &larr; Back to Course
                    </Link>
                    <h1 className="text-3xl font-bold text-gray-900">Quizzes</h1>
                    <div className="ml-auto">
                        <Link
                            href={`/instructor/courses/${courseId}/quizzes/create`}
                            className="bg-indigo-600 text-white px-4 py-2 rounded-lg hover:bg-indigo-700 transition inline-block"
                        >
                            + Create Quiz
                        </Link>
                    </div>
                </div>

                {loading ? (
                    <div className="text-center py-12">Loading quizzes...</div>
                ) : (
                    <div className="bg-white rounded-xl shadow-md overflow-hidden">
                        <table className="min-w-full divide-y divide-gray-200">
                            <thead className="bg-gray-50">
                                <tr>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Name</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Description</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Duration (min)</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Total Marks</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                                    <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                                </tr>
                            </thead>
                            <tbody className="bg-white divide-y divide-gray-200">
                                {quizzes.length === 0 ? (
                                    <tr>
                                        <td colSpan={6} className="px-6 py-4 text-center text-gray-500">
                                            No quizzes found for this course.
                                        </td>
                                    </tr>
                                ) : (
                                    quizzes.map((quiz) => (
                                        <tr key={quiz.id}>
                                            <td className="px-6 py-4">
                                                <div className="text-sm font-medium text-gray-900 line-clamp-2" title={quiz.name}>
                                                    {quiz.name}
                                                </div>
                                            </td>
                                            <td className="px-6 py-4">
                                                <div className="text-sm text-gray-500 line-clamp-2" title={quiz.description || ""}>
                                                    {quiz.description || "-"}
                                                </div>
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                                {quiz.durationMinutes}
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                                {quiz.totalMarks}
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap">
                                                <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${Number(quiz.status) === QuizStatusEnum.Published ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                                                    }`}>
                                                    {getStatusName(quiz.status)}
                                                </span>
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                                                <Link
                                                    href={`/instructor/courses/${courseId}/quizzes/${quiz.id}/edit`}
                                                    className="text-indigo-600 hover:text-indigo-900 mr-4"
                                                >
                                                    Edit
                                                </Link>

                                                <button
                                                    onClick={() => handleDeleteQuiz(quiz.id)}
                                                    className="text-red-600 hover:text-red-900"
                                                >
                                                    Delete
                                                </button>
                                            </td>
                                        </tr>
                                    ))
                                )}
                            </tbody>
                        </table>
                    </div>
                )}
            </div>
        </div>
    );
}
