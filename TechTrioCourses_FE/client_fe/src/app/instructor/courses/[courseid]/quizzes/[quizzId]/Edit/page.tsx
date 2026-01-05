"use client";
import { useState, useEffect } from "react";
import { useParams, useRouter } from "next/navigation";
import {
  quizAPI,
  QuizUpdateRequest,
  QuizStatusEnum,
  questionAPI,
  quizQuestionAPI,
  QuestionResponse,
  QuestionTypeEnum,
  QuestionStatusEnum,
  QuizQuestionCreateRequest,
  QuizResponse
} from "@/services/quizAPI";
import { courseAPI, CourseResponse } from "@/services/courseAPI";
import { useAuth } from "@/contexts/AuthContext";
import { UserRoleEnum } from "@/services/userAPI";
import Link from "next/link";

type StepBasicInfoProps = {
  formData: {
    name: string;
    description?: string;
    durationMinutes?: number;
  };
  setFormData: React.Dispatch<React.SetStateAction<any>>;
};

type StepConfigurationProps = {
  formData: any;
  setFormData: React.Dispatch<React.SetStateAction<any>>;
  questions: QuestionResponse[];
  toggleQuestion: (id: string) => void;
};


type StepReviewProps = {
  formData: any;
  questions: QuestionResponse[];
  setFormData: React.Dispatch<React.SetStateAction<any>>;
};

const StepBasicInfo = ({ formData, setFormData }: StepBasicInfoProps) => (
  <>

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
  </>
);


const StepConfiguration = ({
  formData,
  setFormData,
  questions,
  toggleQuestion,
}: StepConfigurationProps) => (
  <>
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
    {/*Questions Assignment*/}
    <div>
      <label className="block text-sm font-medium text-gray-700 mb-2">
        Questions
      </label>
      {questions.length === 0 ? (
        <p className="text-gray-500">No published questions available for this course.</p>
      ) : (
        <div className="space-y-3 max-h-64 overflow-y-auto border rounded-lg p-3">
          {questions.map((question) => (
            <div key={question.id} className="flex items-center">
              <input
                type="checkbox"
                checked={formData.questionIds.includes(question.id)}
                onChange={() => toggleQuestion(question.id)}
                className="h-4 w-4 text-blue-600 border-gray-300 rounded"
              />
              <span className="ml-2 text-gray-900">{question.questionText}</span>
              <span className="ml-2 text-gray-900">Type: {QuestionTypeEnum[question.questionType]}</span>
            </div>
          ))}
        </div>
      )
      }
    </div>
  </>
);

const StepReview = ({ formData, questions, setFormData }: StepReviewProps) => (
  <>
    <h3 className="text-lg text-gray-600 font-semibold mb-4">Review Quiz</h3>

    <ul className="space-y-2 text-gray-600">
      <li><b>Name</b>: {formData.name}</li>
      <li><b>Description</b>: {formData.description || 'N/A'}</li>
      <li><b>Total Marks</b>: {formData.totalMarks || 'N/A'}</li>
      <li><b>Duration (Minutes)</b>: {formData.durationMinutes || 'N/A'}</li>
      <ul className="list-disc ml-5 text-sm">
        {questions
          .filter((q: QuestionResponse) =>
            formData.questionIds.includes(q.id)
          )
          .map((q: QuestionResponse) => (
            <li key={q.id}>{q.questionText}</li>
          ))}
      </ul>

    </ul>
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
        Hidden quizzes are not visible to students
      </p>
    </div>
  </>
);

export default function EditQuizPage() {
  type QuizStep = 1 | 2 | 3;

  const [step, setStep] = useState<QuizStep>(1);
  const params = useParams();
  const router = useRouter();
  const { user } = useAuth();
  const [course, setCourse] = useState<CourseResponse | null>(null);
  const [quiz, setQuiz] = useState<QuizResponse | null>(null);
  const [questions, setQuestions] = useState<QuestionResponse[]>([]);
  const [submitting, setSubmitting] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Form state
  const [formData, setFormData] = useState<{
    name: string;
    description?: string;
    totalMarks?: number;
    durationMinutes?: number;
    status: QuizStatusEnum;
    questionIds: string[];
  }>({
    name: "",
    description: "",
    totalMarks: 0,
    durationMinutes: 0,
    status: QuizStatusEnum.Hidden,
    questionIds: [],
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
      setLoading(true);

      // Load course
      const courseData = await courseAPI.getCourseById(params.courseid as string);
      setCourse(courseData);

      // Load quiz
      const quizData = await quizAPI.getQuizById(params.quizzId as string);
      setQuiz(quizData);

      // Load questions
      const allQuestions = await questionAPI.getQuestionsByCourseId(params.courseid as string);
      const publishedQuestions = allQuestions.filter(
        (q) => q.status === QuestionStatusEnum.Published
      );
      setQuestions(publishedQuestions);

      // Load assigned questions
      const assignedQuestions = await quizQuestionAPI.getQuizQuestionsByQuizId(params.quizzId as string);
      const assignedQuestionIds = assignedQuestions.map(qq => qq.questionId);

      // Populate form data
      setFormData({
        name: quizData.name,
        description: quizData.description || "",
        totalMarks: quizData.totalMarks || 0,
        durationMinutes: quizData.durationMinutes || 0,
        status: quizData.status,
        questionIds: assignedQuestionIds,
      });
    } catch (err: any) {
      setError(err.message || 'Failed to load data');
    } finally {
      setLoading(false);
    }
  };

  const toggleQuestion = (questionId: string) => {
    setFormData(prev => {
      const exists = prev.questionIds.includes(questionId);

      return {
        ...prev,
        questionIds: exists
          ? prev.questionIds.filter(id => id !== questionId)
          : [...prev.questionIds, questionId],
      };
    });
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

      // Update quiz basic info
      const updateData: QuizUpdateRequest = {
        name: formData.name,
        description: formData.description || null,
        totalMarks: formData.totalMarks || null,
        durationMinutes: formData.durationMinutes || null,
        status: formData.status,
      };

      await quizAPI.updateQuiz(params.quizzId as string, updateData);

      // Get current quiz questions and remove all
      const currentQuizQuestions = await quizQuestionAPI.getQuizQuestionsByQuizId(params.quizzId as string);
      for (const qq of currentQuizQuestions) {
        await quizQuestionAPI.deleteQuizQuestion(params.quizzId as string, qq.questionId);
      }

      // Add new quiz questions
      const questionPoint = formData.totalMarks && formData.questionIds.length > 0
        ? Math.floor(formData.totalMarks / formData.questionIds.length)
        : 0;

      for (let i = 0; i < formData.questionIds.length; i++) {
        const questionId = formData.questionIds[i];
        const quizQuestionData: QuizQuestionCreateRequest = {
          quizId: params.quizzId as string,
          questionId: questionId,
          questionOrder: i + 1,
          overridePoints: questionPoint,
        };
        await quizQuestionAPI.createQuizQuestion(quizQuestionData);
      }

      router.push(`/instructor/courses/${params.courseid}/quizzes`);
    } catch (err: any) {
      setError(err.message || 'Failed to update quiz');
    } finally {
      setSubmitting(false);
    }
  };

  const handleCancel = () => {
    router.push(`/instructor/courses/${params.courseid}/quizzes`);
  };

  const handleNext = () => {
    if (!validateStep()) return;

    if (step < 3) {
      setStep((prev) => (prev + 1) as QuizStep);
    }
  };

  const handleBack = () => {
    if (step > 1) setStep((step - 1) as QuizStep);
  };

  const renderStep = () => {
    switch (step) {
      case 1:
        return (
          <StepBasicInfo
            formData={formData}
            setFormData={setFormData}
          />
        );

      case 2:
        return (
          <StepConfiguration
            formData={formData}
            setFormData={setFormData}
            questions={questions}
            toggleQuestion={toggleQuestion}
          />
        );

      case 3:
        return (
          <StepReview
            formData={formData}
            questions={questions}
            setFormData={setFormData}
          />
        );

      default:
        return null;
    }
  };

  const renderActions = () => (
    <div className="flex gap-3 pt-4">
      {step > 1 && (
        <button
          type="button"
          onClick={handleBack}
          className="flex-1 px-6 py-3 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors"
        >
          Back
        </button>
      )}

      <button
        type="button"
        onClick={handleCancel}
        className="flex-1 px-6 py-3 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors"
      >
        Cancel
      </button>

      {step < 3 ? (
        <button
          type="button"
          onClick={handleNext}
          className="flex-1 px-6 py-3 bg-gradient-to-r from-blue-500 to-indigo-600 text-white rounded-lg hover:from-blue-600 hover:to-indigo-700 transition-all"
        >
          Next
        </button>
      ) : (
        <button
          type="submit"
          disabled={submitting}
          className="flex-1 px-6 py-3 bg-gradient-to-r from-green-500 to-emerald-600 text-white rounded-lg hover:from-green-600 hover:to-emerald-700 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {submitting ? "Updating..." : "Update Quiz"}
        </button>
      )}
    </div>
  );

  const validateStep = (): boolean => {
    if (step === 1) {
      if (!formData.name.trim()) {
        alert("Quiz name is required");
        return false;
      }

      if (!formData.durationMinutes || formData.durationMinutes < 10) {
        alert("Quiz duration must be at least 10 minutes");
        return false;
      }
    }

    if (step === 2) {
      if (!formData.totalMarks || formData.totalMarks < 10) {
        alert("Total marks must be at least 10");
        return false;
      }

      if (formData.questionIds.length === 0) {
        alert("Please select at least one question");
        return false;
      }
    }

    return true;
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="text-center">
          <div className="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mb-4"></div>
          <div className="text-xl text-gray-700">Loading quiz...</div>
        </div>
      </div>
    );
  }

  if (!quiz) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="bg-red-50 border-l-4 border-red-500 p-6 rounded-lg shadow-lg">
          <p className="text-red-700 font-semibold">Quiz not found</p>
          <Link
            href={`/instructor/courses/${params.courseid}/quizzes`}
            className="mt-4 inline-block text-indigo-600 hover:text-indigo-800 underline"
          >
            Back to Quizzes
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="container mx-auto px-4 max-w-3xl">
        {/* Breadcrumb */}
        <div className="mb-4">
          <Link
            href={`/instructor/courses/${params.courseid}/quizzes`}
            className="text-blue-600 hover:underline"
          >
            ← Back to quizzes
          </Link>
        </div>

        {/* Header */}
        <div className="bg-white rounded-lg shadow-sm p-6 mb-6">
          <h1 className="text-3xl font-bold text-gray-900">Edit Quiz</h1>
          {course && (
            <p className="text-gray-600 mt-2">
              Course: <span className="font-medium">{course.title}</span>
            </p>
          )}
        </div>

        {/* Progress Steps */}
        <div className="bg-white rounded-lg shadow-sm p-6 mb-6">
          <div className="flex items-center justify-between">
            <div className={`flex-1 text-center ${step >= 1 ? 'text-blue-600 font-semibold' : 'text-gray-400'}`}>
              <div className={`w-10 h-10 mx-auto rounded-full flex items-center justify-center border-2 ${step >= 1 ? 'border-blue-600 bg-blue-50' : 'border-gray-300'}`}>
                1
              </div>
              <p className="mt-2 text-sm">Basic Info</p>
            </div>
            <div className={`flex-1 h-1 ${step >= 2 ? 'bg-blue-600' : 'bg-gray-300'}`}></div>
            <div className={`flex-1 text-center ${step >= 2 ? 'text-blue-600 font-semibold' : 'text-gray-400'}`}>
              <div className={`w-10 h-10 mx-auto rounded-full flex items-center justify-center border-2 ${step >= 2 ? 'border-blue-600 bg-blue-50' : 'border-gray-300'}`}>
                2
              </div>
              <p className="mt-2 text-sm">Configuration</p>
            </div>
            <div className={`flex-1 h-1 ${step >= 3 ? 'bg-blue-600' : 'bg-gray-300'}`}></div>
            <div className={`flex-1 text-center ${step >= 3 ? 'text-blue-600 font-semibold' : 'text-gray-400'}`}>
              <div className={`w-10 h-10 mx-auto rounded-full flex items-center justify-center border-2 ${step >= 3 ? 'border-blue-600 bg-blue-50' : 'border-gray-300'}`}>
                3
              </div>
              <p className="mt-2 text-sm">Review</p>
            </div>
          </div>
        </div>

        {/* Error Message */}
        {error && (
          <div className="mb-6 bg-red-50 border-l-4 border-red-500 p-4 rounded">
            <p className="text-red-700 text-sm">{error}</p>
          </div>
        )}

        {/* Form */}
        <div className="bg-white rounded-lg shadow-sm p-6">
          {step === 3 ? (
            <form onSubmit={handleSubmit}>
              {renderStep()}
              {renderActions()}
            </form>
          ) : (
            <>
              {renderStep()}
              {renderActions()}
            </>
          )}
        </div>
      </div>
    </div>
  );
}
