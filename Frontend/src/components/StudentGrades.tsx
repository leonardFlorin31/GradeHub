import { useEffect, useState } from "react";

interface Grade {
  classId: string;
  value: number;
  date: string;
}

const mockLabels = ["Assignment", "Quiz", "Midterm", "Project", "Homework"];

const StudentGrades = ({ studentId }: { studentId: string }) => {
  const [grades, setGrades] = useState<Grade[]>([]);
  const [showAverages, setShowAverages] = useState<Record<string, boolean>>({});

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

  const toggleAverage = (classId: string) => {
    setShowAverages((prev) => ({
      ...prev,
      [classId]: !prev[classId],
    }));
  };

  const calculateAverage = (grades: Grade[]) => {
    if (grades.length === 0) return 0;
    const sum = grades.reduce((acc, g) => acc + g.value, 0);
    return (sum / grades.length).toFixed(2);
  };

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

              <ul className="space-y-2 mb-4">
                {grades.map((grade, index) => {
                  const assignmentType =
                    mockLabels[index % mockLabels.length] + " " + (index + 1);

                  return (
                    <li
                      key={index}
                      className="flex justify-between items-center border-b pb-2"
                    >
                      <div className="flex flex-col">
                        <span className="text-sm text-gray-700 font-medium mb-2">
                          {assignmentType}
                        </span>
                        <span className="text-bice-blue font-bold">
                          {grade.value}
                        </span>
                      </div>
                      <span className="text-sm text-gray-500">
                        {new Date(grade.date).toLocaleDateString("en-GB")}
                      </span>
                    </li>
                  );
                })}
              </ul>

              <button
                onClick={() => toggleAverage(classId)}
                className="text-sm text-white bg-bice-blue px-3 py-1 rounded hover:bg-blue-800 transition"
              >
                {showAverages[classId] ? "Hide Average" : "Show Average"}
              </button>

              {showAverages[classId] && (
                <p className="mt-3 text-gray-700 font-medium">
                  Average:{" "}
                  <span className="text-bice-blue font-bold">
                    {calculateAverage(grades)}
                  </span>
                </p>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default StudentGrades;
