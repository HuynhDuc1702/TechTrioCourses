"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import {
    questionAPI,
    quizQuestionAPI,
    QuestionResponse,
    QuestionCreateRequest,
    QuestionTypeEnum,
    QuestionStatusEnum,
    questionChoiceAPI,
    QuestionChoiceCreateRequest,
    questionAnswerAPI,
    QuestionAnswerCreateRequest
} from "@/services/quizAPI";
import { useAuth } from "@/contexts/AuthContext";
import Link from "next/link";
import Swal from 'sweetalert2';

export default function QuestionBankPage() {
    const params = useParams();
    const router = useRouter();
    const { user } = useAuth();
    const [questions, setQuestions] = useState<QuestionResponse[]>([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);

    const [editingQuestionId, setEditingQuestionId] = useState<string | null>(null);

    // Form state
    const [formData, setFormData] = useState<{
        questionText: string;
        questionType: QuestionTypeEnum;
        points: number;
        status: QuestionStatusEnum;
        choices: { id?: string; text: string; isCorrect: boolean }[];
        answers: { id?: string; text: string }[];
    }>({
        questionText: "",
        questionType: QuestionTypeEnum.MultipleChoice,
        points: 1,
        status: QuestionStatusEnum.Published,
        choices: [{ text: "", isCorrect: false }, { text: "", isCorrect: false }],
        answers: [{ text: "" }]
    });

    const courseId = params.courseid as string;

    useEffect(() => {
        if (user && courseId) {
            loadQuestions();
        }
    }, [user, courseId]);

    const loadQuestions = async () => {
        try {
            setLoading(true);
            const allQuestions = await questionAPI.getAllQuestions();

            // Filter by courseId
            const courseQuestions = allQuestions.filter(q => q.courseId === courseId);
            setQuestions(courseQuestions);
        } catch (error) {
            console.error("Failed to load questions", error);
            Swal.fire("Error", "Failed to load questions", "error");
        } finally {
            setLoading(false);
        }
    };

    const resetForm = () => {
        setFormData({
            questionText: "",
            questionType: QuestionTypeEnum.MultipleChoice,
            points: 1,
            status: QuestionStatusEnum.Published,
            choices: [{ text: "", isCorrect: false }, { text: "", isCorrect: false }],
            answers: [{ text: "" }]
        });
        setEditingQuestionId(null);
    };

    const handleEditQuestion = async (question: QuestionResponse) => {
        try {
            setEditingQuestionId(question.id);

            // 1. Fetch choices
            let choices: { id?: string; text: string; isCorrect: boolean }[] = [];
            let answers: { id?: string; text: string }[] = [];

            if (question.questionType === QuestionTypeEnum.MultipleChoice || question.questionType === QuestionTypeEnum.TrueFalse) {
                const fetchedChoices = await questionChoiceAPI.getQuestionChoicesByQuestionId(question.id);
                choices = fetchedChoices.map(c => ({
                    id: c.id,
                    text: c.choiceText,
                    isCorrect: c.isCorrect
                }));
            } else if (question.questionType === QuestionTypeEnum.ShortAnswer) {
                const fetchedAnswers = await questionAnswerAPI.getQuestionAnswersByQuestionId(question.id);
                answers = fetchedAnswers.map(a => ({
                    id: a.id,
                    text: a.answerText
                }));
            }

            // Defaults if empty
            if (question.questionType === QuestionTypeEnum.MultipleChoice && choices.length === 0) {
                choices = [{ text: "", isCorrect: false }, { text: "", isCorrect: false }];
            }
            if (question.questionType === QuestionTypeEnum.TrueFalse && choices.length === 0) {
                choices = [{ text: "True", isCorrect: true }, { text: "False", isCorrect: false }];
            }
            if (question.questionType === QuestionTypeEnum.ShortAnswer && answers.length === 0) {
                answers = [{ text: "" }];
            }

            setFormData({
                questionText: question.questionText,
                questionType: question.questionType,
                points: question.points,
                status: question.status,
                choices: choices,
                answers: answers
            });

            setShowModal(true);
        } catch (error) {
            console.error("Failed to load question details", error);
            Swal.fire("Error", "Failed to load question details", "error");
        }
    };

    const handleTypeChange = (newType: number) => {
        let newChoices = formData.choices;
        let newAnswers = formData.answers;

        if (newType === QuestionTypeEnum.TrueFalse) {
            // Auto-populate T/F
            newChoices = [
                { text: "True", isCorrect: true },
                { text: "False", isCorrect: false }
            ];
        } else if (newType === QuestionTypeEnum.MultipleChoice) {
            // Reset to empty if coming from T/F or ensure valid state
            if (newChoices.length === 0 || (newChoices.length === 2 && newChoices[0].text === "True" && newChoices[1].text === "False")) {
                newChoices = [{ text: "", isCorrect: false }, { text: "", isCorrect: false }];
            }
        }

        if (newType === QuestionTypeEnum.ShortAnswer && newAnswers.length === 0) {
            newAnswers = [{ text: "" }];
        }

        setFormData({
            ...formData,
            questionType: newType,
            choices: newChoices,
            answers: newAnswers
        });
    };

    const handleSubmit = async () => {
        if (!formData.questionText) {
            Swal.fire("Error", "Question text is required", "error");
            return;
        }

        try {
            if (editingQuestionId) {
                // UPDATE MODE
                // 1. Update Question details
                await questionAPI.updateQuestion(editingQuestionId, {
                    questionText: formData.questionText,
                    questionType: formData.questionType,
                    points: formData.points,
                    status: formData.status
                });

                // 2. Handle Choices (for MC and TF)
                if (formData.questionType === QuestionTypeEnum.MultipleChoice || formData.questionType === QuestionTypeEnum.TrueFalse) {
                    // Fetch existing to compare
                    const existingChoices = await questionChoiceAPI.getQuestionChoicesByQuestionId(editingQuestionId);

                    // A. Delete removed
                    const choicesToDelete = existingChoices.filter(ec => !formData.choices.find(fc => fc.id === ec.id));
                    for (const choice of choicesToDelete) {
                        await questionChoiceAPI.deleteQuestionChoice(choice.id);
                    }

                    // B. Update or Create
                    for (const choice of formData.choices) {
                        if (choice.text) {
                            if (choice.id) {
                                await questionChoiceAPI.updateQuestionChoice(choice.id, {
                                    choiceText: choice.text,
                                    isCorrect: choice.isCorrect
                                });
                            } else {
                                await questionChoiceAPI.createQuestionChoice({
                                    questionId: editingQuestionId,
                                    choiceText: choice.text,
                                    isCorrect: choice.isCorrect
                                });
                            }
                        }
                    }
                }
                // 3. Handle Answers (for ShortAnswer)
                else if (formData.questionType === QuestionTypeEnum.ShortAnswer) {
                    const existingAnswers = await questionAnswerAPI.getQuestionAnswersByQuestionId(editingQuestionId);

                    // A. Delete removed
                    const answersToDelete = existingAnswers.filter(ea => !formData.answers.find(fa => fa.id === ea.id));
                    for (const ans of answersToDelete) {
                        await questionAnswerAPI.deleteQuestionAnswer(ans.id);
                    }

                    // B. Update or Create
                    for (const ans of formData.answers) {
                        if (ans.text) {
                            if (ans.id) {
                                await questionAnswerAPI.updateQuestionAnswer(ans.id, { answerText: ans.text });
                            } else {
                                await questionAnswerAPI.createQuestionAnswer({
                                    questionId: editingQuestionId,
                                    answerText: ans.text
                                });
                            }
                        }
                    }
                }

                Swal.fire("Success", "Question updated successfully", "success");

            } else {
                // CREATE MODE
                // 1. Create Question
                const questionRequest: QuestionCreateRequest = {
                    userId: user?.userId || "",
                    courseId: courseId,
                    content: formData.questionText,
                    questionText: formData.questionText,
                    questionType: Number(formData.questionType),
                    points: formData.points,
                    status: Number(formData.status)
                };

                const createdQuestion = await questionAPI.createQuestion(questionRequest);

                // 2. Create Choices if MC or TF
                if (formData.questionType === QuestionTypeEnum.MultipleChoice || formData.questionType === QuestionTypeEnum.TrueFalse) {
                    for (const choice of formData.choices) {
                        if (choice.text) {
                            await questionChoiceAPI.createQuestionChoice({
                                questionId: createdQuestion.id,
                                choiceText: choice.text,
                                isCorrect: choice.isCorrect
                            });
                        }
                    }
                }
                // 3. Create Answers if ShortAnswer
                else if (formData.questionType === QuestionTypeEnum.ShortAnswer) {
                    for (const ans of formData.answers) {
                        if (ans.text) {
                            await questionAnswerAPI.createQuestionAnswer({
                                questionId: createdQuestion.id,
                                answerText: ans.text
                            });
                        }
                    }
                }

                Swal.fire("Success", "Question created successfully", "success");
            }

            setShowModal(false);
            loadQuestions();
            resetForm();

        } catch (error) {
            console.error("Failed to save question", error);
            Swal.fire("Error", "Failed to save question", "error");
        }
    };

    const handleDeleteQuestion = async (id: string) => {
        try {
            // Check usage
            const usages = await quizQuestionAPI.getQuizQuestionsByQuestionId(id);
            if (usages && usages.length > 0) {
                Swal.fire("Cannot Delete", "This question is used in one or more quizzes. Please remove it from the quizzes first.", "warning");
                return;
            }

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
                await questionAPI.deleteQuestion(id);
                Swal.fire('Deleted!', 'Question has been deleted.', 'success');
                loadQuestions();
            }
        } catch (error) {
            console.error("Failed to delete question", error);
            Swal.fire("Error", "Failed to delete question", "error");
        }
    };



    const getTypeName = (type: number) => {
        switch (type) {
            case QuestionTypeEnum.MultipleChoice: return "Multiple Choice";
            case QuestionTypeEnum.TrueFalse: return "True/False";
            case QuestionTypeEnum.ShortAnswer: return "Short Answer";
            default: return "Unknown";
        }
    };

    const getStatusName = (status: number) => {
        switch (status) {
            case QuestionStatusEnum.Published: return "Published";
            case QuestionStatusEnum.Hidden: return "Hidden"; // Or Disabled
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
                    <h1 className="text-3xl font-bold text-gray-900">Question Bank</h1>
                    <div className="ml-auto">
                        <button
                            onClick={() => { resetForm(); setShowModal(true); }}
                            className="bg-indigo-600 text-white px-4 py-2 rounded-lg hover:bg-indigo-700 transition"
                        >
                            + Create Question
                        </button>
                    </div>
                </div>

                {loading ? (
                    <div className="text-center py-12">Loading questions...</div>
                ) : (
                    <div className="bg-white rounded-xl shadow-md overflow-hidden">
                        <table className="min-w-full divide-y divide-gray-200">
                            <thead className="bg-gray-50">
                                <tr>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Question</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Type</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Points</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                                    <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                                </tr>
                            </thead>
                            <tbody className="bg-white divide-y divide-gray-200">
                                {questions.length === 0 ? (
                                    <tr>
                                        <td colSpan={5} className="px-6 py-4 text-center text-gray-500">
                                            No questions found for this course. Start by creating one.
                                        </td>
                                    </tr>
                                ) : (
                                    questions.map((q) => (
                                        <tr key={q.id}>
                                            <td className="px-6 py-4">
                                                <div className="text-sm font-medium text-gray-900 line-clamp-2" title={q.questionText}>
                                                    {q.questionText}
                                                </div>
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                                {getTypeName(q.questionType)}
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                                {q.points}
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap">
                                                <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${q.status === QuestionStatusEnum.Published ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                                                    }`}>
                                                    {getStatusName(q.status)}
                                                </span>
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                                                <button
                                                    onClick={() => handleEditQuestion(q)}
                                                    className="text-indigo-600 hover:text-indigo-900 mr-4"
                                                >
                                                    Edit
                                                </button>

                                                <button
                                                    onClick={() => handleDeleteQuestion(q.id)}
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

            {/* Create Modal */}
            {/* Simple Create Modal */}
            {showModal && (
                <div className="fixed inset-0 z-[100] flex items-center justify-center bg-black bg-opacity-50 p-4">
                    <div className="bg-white rounded-xl shadow-2xl w-full max-w-2xl max-h-[90vh] overflow-y-auto">
                        <div className="p-6">
                            <h2 className="text-2xl font-bold mb-6 text-gray-800 border-b pb-2">
                                {editingQuestionId ? "Edit Question" : "Create New Question"}
                            </h2>

                            <div className="space-y-6">
                                <div>
                                    <label className="block text-sm font-semibold text-gray-700 mb-2">Question Text</label>
                                    <textarea
                                        value={formData.questionText}
                                        onChange={(e) => setFormData({ ...formData, questionText: e.target.value })}
                                        className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition shadow-sm text-gray-900"
                                        rows={3}
                                        placeholder="Enter your question here..."
                                    />
                                </div>

                                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Question Type</label>
                                        <select
                                            value={formData.questionType}
                                            onChange={(e) => handleTypeChange(parseInt(e.target.value))}
                                            className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 bg-white text-gray-900"
                                        >
                                            <option value={QuestionTypeEnum.MultipleChoice}>Multiple Choice</option>
                                            <option value={QuestionTypeEnum.TrueFalse}>True / False</option>
                                            <option value={QuestionTypeEnum.ShortAnswer}>Short Answer</option>
                                        </select>
                                    </div>
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Points</label>
                                        <input
                                            type="number"
                                            value={formData.points}
                                            onChange={(e) => setFormData({ ...formData, points: parseInt(e.target.value) })}
                                            className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 text-gray-900"
                                            min={1}
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Status</label>
                                        <select
                                            value={formData.status}
                                            onChange={(e) => setFormData({ ...formData, status: parseInt(e.target.value) })}
                                            className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 bg-white text-gray-900"
                                        >
                                            <option value={QuestionStatusEnum.Published}>Published</option>
                                            <option value={QuestionStatusEnum.Hidden}>Hidden</option>
                                        </select>
                                    </div>
                                </div>

                                {/* Choices Section for MC and TF */}
                                {(formData.questionType === QuestionTypeEnum.MultipleChoice || formData.questionType === QuestionTypeEnum.TrueFalse) && (
                                    <div className="bg-gray-50 p-4 rounded-lg border border-gray-200">
                                        <label className="block text-sm font-semibold text-gray-700 mb-3 block">
                                            {formData.questionType === QuestionTypeEnum.TrueFalse ? "Correct Answer" : "Answer Choices"}
                                        </label>
                                        <div className="space-y-3">
                                            {formData.choices.map((choice, index) => (
                                                <div key={index} className="flex items-center gap-3">
                                                    <input
                                                        type="checkbox" // Or radio for T/F specifically, but checkbox allows flexibility if multiple true allowed (though logic might restrict)
                                                        checked={choice.isCorrect}
                                                        onChange={(e) => {
                                                            const newChoices = [...formData.choices];
                                                            // For T/F, ensure mutual exclusivity
                                                            if (formData.questionType === QuestionTypeEnum.TrueFalse) {
                                                                newChoices.forEach(c => c.isCorrect = false);
                                                                newChoices[index].isCorrect = true;
                                                            } else {
                                                                newChoices[index].isCorrect = e.target.checked;
                                                            }
                                                            setFormData({ ...formData, choices: newChoices });
                                                        }}
                                                        className="w-5 h-5 text-indigo-600 rounded focus:ring-indigo-500 border-gray-300 cursor-pointer"
                                                        title="Mark as correct answer"
                                                    />
                                                    <input
                                                        type="text"
                                                        value={choice.text}
                                                        disabled={formData.questionType === QuestionTypeEnum.TrueFalse} // Read-only for T/F? User requested "Auto input"
                                                        placeholder={`Option ${index + 1}`}
                                                        onChange={(e) => {
                                                            const newChoices = [...formData.choices];
                                                            newChoices[index].text = e.target.value;
                                                            setFormData({ ...formData, choices: newChoices });
                                                        }}
                                                        className={`flex-1 p-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500 text-gray-900 ${formData.questionType === QuestionTypeEnum.TrueFalse ? "bg-gray-100" : ""}`}
                                                    />
                                                    {formData.questionType !== QuestionTypeEnum.TrueFalse && (
                                                        <button
                                                            type="button"
                                                            onClick={() => {
                                                                const newChoices = formData.choices.filter((_, i) => i !== index);
                                                                setFormData({ ...formData, choices: newChoices });
                                                            }}
                                                            className="text-red-500 hover:text-red-700 p-1 hover:bg-red-50 rounded"
                                                            title="Remove choice"
                                                        >
                                                            ✕
                                                        </button>
                                                    )}
                                                </div>
                                            ))}
                                        </div>
                                        {formData.questionType !== QuestionTypeEnum.TrueFalse && (
                                            <button
                                                type="button"
                                                onClick={() => setFormData({ ...formData, choices: [...formData.choices, { text: "", isCorrect: false }] })}
                                                className="mt-3 text-sm font-medium text-indigo-600 hover:text-indigo-800 flex items-center gap-1"
                                            >
                                                + Add Another Choice
                                            </button>
                                        )}
                                    </div>
                                )}

                                {/* Short Answer Section */}
                                {formData.questionType === QuestionTypeEnum.ShortAnswer && (
                                    <div className="bg-gray-50 p-4 rounded-lg border border-gray-200">
                                        <label className="block text-sm font-semibold text-gray-700 mb-3">Accepted Answers (Exact Match)</label>
                                        <div className="space-y-3">
                                            {formData.answers.map((ans, index) => (
                                                <div key={index} className="flex items-center gap-3">
                                                    <input
                                                        type="text"
                                                        value={ans.text}
                                                        placeholder={`Acceptable Answer ${index + 1}`}
                                                        onChange={(e) => {
                                                            const newAnswers = [...formData.answers];
                                                            newAnswers[index].text = e.target.value;
                                                            setFormData({ ...formData, answers: newAnswers });
                                                        }}
                                                        className="flex-1 p-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                                                    />
                                                    <button
                                                        type="button"
                                                        onClick={() => {
                                                            const newAnswers = formData.answers.filter((_, i) => i !== index);
                                                            if (newAnswers.length === 0) newAnswers.push({ text: "" }); // Keep at least one
                                                            setFormData({ ...formData, answers: newAnswers });
                                                        }}
                                                        className="text-red-500 hover:text-red-700 p-1 hover:bg-red-50 rounded"
                                                        title="Remove answer"
                                                    >
                                                        ✕
                                                    </button>
                                                </div>
                                            ))}
                                        </div>
                                        <button
                                            type="button"
                                            onClick={() => setFormData({ ...formData, answers: [...formData.answers, { text: "" }] })}
                                            className="mt-3 text-sm font-medium text-indigo-600 hover:text-indigo-800 flex items-center gap-1"
                                        >
                                            + Add Another Acceptable Answer
                                        </button>
                                    </div>
                                )}
                            </div>

                            <div className="mt-8 flex justify-end gap-3 pt-4 border-t">
                                <button
                                    type="button"
                                    onClick={() => setShowModal(false)}
                                    className="px-6 py-2.5 bg-white border border-gray-300 text-gray-700 font-medium rounded-lg hover:bg-gray-50 transition shadow-sm"
                                >
                                    Cancel
                                </button>
                                <button
                                    type="button"
                                    onClick={handleSubmit}
                                    className="px-6 py-2.5 bg-indigo-600 text-white font-medium rounded-lg hover:bg-indigo-700 transition shadow-sm"
                                >
                                    {editingQuestionId ? "Update Question" : "Create Question"}
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
