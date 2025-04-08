import { useEffect, useState } from "react";
import GradeManager from "./GradeManager";

const TeacherClassGrades = ({ teacherId }: { teacherId: string }) => {
  const [classes, setClasses] = useState<any[]>([]);
  const [studentGrades, setStudentGrades] = useState<Record<string, any[]>>({});
  const [newGrades, setNewGrades] = useState<Record<string, number>>({});

  const handleGradeChange = (studentId: string, value: string) => {
    setNewGrades((prev) => ({
      ...prev,
      [studentId]: Number(value),
    }));
  };

  const handleAddGrade = async (studentId: string, classId: string) => {
    const value = newGrades[studentId];
    if (!value || value < 1 || value > 10) {
      alert("Please enter a grade between 1 and 10");
      return;
    }

    const res = await fetch(
      `https://localhost:64060/api/students/${studentId}/grades`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ classId, value }),
      }
    );

    if (res.ok) {
      const updatedGrade = await res.json();
      setStudentGrades((prev) => ({
        ...prev,
        [studentId]: [...(prev[studentId] || []), updatedGrade],
      }));
      setNewGrades((prev) => ({ ...prev, [studentId]: 0 }));
    } else {
      alert("Failed to add grade.");
    }
  };

  useEffect(() => {
    fetch("https://localhost:64060/api/classes")
      .then((res) => res.json())
      .then((data) => {
        const teacherClasses = data.filter(
          (c: any) => c.teacher.classId === teacherId
        );
        setClasses(teacherClasses);

        // Fetch grades for each student
        teacherClasses.forEach((cls: any) => {
          cls.students.forEach((student: any) => {
            fetch(`https://localhost:64060/api/students/${student.id}/grades`)
              .then((res) => res.json())
              .then((grades) => {
                setStudentGrades((prev) => ({
                  ...prev,
                  [student.id]: grades.filter(
                    (g: any) => g.classId === teacherId
                  ),
                }));
              });
          });
        });
      });
  }, [teacherId]);

  return (
    <div className="p-6 w-[35%] mx-auto">
      <h2 className="text-2xl font-bold mb-6 text-center text-white">
        Class Grades Overview
      </h2>

      {classes.length === 0 ? (
        <p className="text-gray-500 text-center">No classes assigned yet.</p>
      ) : (
        classes.map((cls: any) => (
          <div key={cls.className} className="mb-12">
            <h3 className="text-xl font-bold mb-4 text-bice-blue text-center">
              {cls.className}
            </h3>

            <div className="overflow-y-auto max-h-[80vh] rounded-lg shadow border border-gray-200">
              <table className="min-w-full divide-y divide-gray-200 bg-white dark:bg-dm-dark">
                <thead className="bg-gray-100 text-left">
                  <tr>
                    <th className="px-4 py-3 text-sm font-semibold text-gray-700">
                      Student Name
                    </th>
                    <th className="px-4 py-3 text-sm font-semibold text-gray-700">
                      Student ID
                    </th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-100">
                  {cls.students.map((student: any) => {
                    return (
                      <>
                        <tr key={student.id} className="hover:bg-gray-50">
                          <td className="px-4 py-2 font-medium">
                            {student.name}
                          </td>
                          <td className="px-4 py-2 text-sm text-gray-600">
                            {student.id}
                          </td>
                        </tr>

                        <tr>
                          <td colSpan={4} className="p-2 bg-gray-50">
                            <GradeManager
                              studentId={student.id}
                              classId={teacherId}
                            />
                          </td>
                        </tr>
                      </>
                    );
                  })}
                </tbody>
              </table>
            </div>
          </div>
        ))
      )}
    </div>
  );
};

export default TeacherClassGrades;
