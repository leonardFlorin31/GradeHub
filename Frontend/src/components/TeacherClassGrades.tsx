import { useEffect, useState } from "react";

const TeacherClassGrades = ({ teacherId }: { teacherId: string }) => {
  const [classes, setClasses] = useState<any[]>([]);
  const [studentGrades, setStudentGrades] = useState<Record<string, any[]>>({});

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
    <div className="p-6 max-w-6xl mx-auto">
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

            <div className="overflow-x-auto rounded-lg shadow border border-gray-200">
              <table className="min-w-full divide-y divide-gray-200 bg-white dark:bg-dm-dark">
                <thead className="bg-gray-100 text-left">
                  <tr>
                    <th className="px-4 py-3 text-sm font-semibold text-gray-700">
                      Student Name
                    </th>
                    <th className="px-4 py-3 text-sm font-semibold text-gray-700">
                      Student ID
                    </th>
                    <th className="px-4 py-3 text-sm font-semibold text-gray-700">
                      Grades
                    </th>
                    <th className="px-4 py-3 text-sm font-semibold text-gray-700">
                      Dates
                    </th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-100">
                  {cls.students.map((student: any) => {
                    const grades = studentGrades[student.id] || [];
                    const values = grades.map((g: any) => g.value).join(", ");
                    const dates = grades
                      .map((g: any) =>
                        new Date(g.date).toLocaleDateString("en-GB")
                      )
                      .join(", ");

                    return (
                      <tr key={student.id} className="hover:bg-gray-50">
                        <td className="px-4 py-2 font-medium">
                          {student.name}
                        </td>
                        <td className="px-4 py-2 text-sm text-gray-600">
                          {student.id}
                        </td>
                        <td className="px-4 py-2 text-bice-blue font-bold">
                          {values || "-"}
                        </td>
                        <td className="px-4 py-2 text-sm text-gray-600">
                          {dates || "-"}
                        </td>
                      </tr>
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
