import { useEffect, useState } from "react";

interface Grade {
  classId: string;
  value: number;
  date: string;
}

const StudentGrades = ({ studentId }: { studentId: string }) => {
  const [grades, setGrades] = useState<Grade[]>([]);

  useEffect(() => {
    fetch(`https://localhost:64060/api/students/${studentId}/grades`)
      .then((res) => res.json())
      .then(setGrades)
      .catch((err) => console.error("Failed to fetch grades", err));
  }, [studentId]);

  // Group grades by classId
  const groupedGrades = grades.reduce((acc: Record<string, Grade[]>, grade) => {
    if (!acc[grade.classId]) acc[grade.classId] = [];
    acc[grade.classId].push(grade);
    return acc;
  }, {});

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <h2 className="text-2xl font-semibold mb-6 text-white">Your Grades</h2>

      {grades.length === 0 ? (
        <p className="text-gray-500">No grades available.</p>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {Object.entries(groupedGrades).map(([classId, grades]) => (
            <div
              key={classId}
              className="bg-white shadow-md rounded-lg p-4 border border-gray-100"
            >
              <h3 className="text-lg font-semibold mb-3 text-bice-blue">
                {classId}
              </h3>
              <ul className="space-y-2">
                {grades.map((grade, index) => (
                  <li
                    key={index}
                    className="flex justify-between items-center border-b pb-1"
                  >
                    <span className="text-gray-700 font-medium mr-5">
                      {grade.value}
                    </span>
                    <span className="text-sm text-gray-500">
                      {new Date(grade.date).toLocaleDateString("en-GB")}
                    </span>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default StudentGrades;
