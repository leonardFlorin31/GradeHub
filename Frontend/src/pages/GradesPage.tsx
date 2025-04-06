import { useAuthContext } from "@/AuthContext";
import StudentGrades from "@/components/StudentGrades";
import TeacherClassGrades from "@/components/TeacherClassGrades";

const GradesPage = () => {
  const { user } = useAuthContext();

  if (user.role === "student") {
    return <StudentGrades studentId={user.userId} />;
  }

  if (user.role === "teacher") {
    return <TeacherClassGrades teacherId={user.userId} />;
  }

  return <p>Loading or unknown role</p>;
};

export default GradesPage;
