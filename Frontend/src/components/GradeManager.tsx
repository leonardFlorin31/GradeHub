import { useEffect, useState } from "react";

type Grade = {
  id: string;
  value: number;
  classId: string;
  date: string;
};

type GradeManagerProps = {
  studentId: string;
  classId: string;
};

const GradeManager = ({ studentId, classId }: GradeManagerProps) => {
  const [grades, setGrades] = useState<Grade[]>([]);
  const [newGrade, setNewGrade] = useState<number>(0);

  const fetchGrades = () => {
    fetch(`https://localhost:64060/api/students/${studentId}/grades`)
      .then((res) => res.json())
      .then((data) => {
        const filtered = data.filter((g: Grade) => g.classId === classId);
        setGrades(filtered);
      });
  };

  useEffect(() => {
    fetchGrades();
  }, [studentId]);

  const handleAddGrade = async () => {
    if (newGrade < 1 || newGrade > 10) {
      alert("Grade must be between 1 and 10");
      return;
    }

    await fetch(`https://localhost:64060/api/students/${studentId}/grades`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ classId, value: newGrade }),
    });

    setNewGrade(0);
    fetchGrades();
  };

  const handleDeleteGrade = async (gradeId: string) => {
    await fetch(
      `https://localhost:64060/api/students/${studentId}/grades/${gradeId}`,
      { method: "DELETE" }
    );
    fetchGrades();
  };

  return (
    <div className="border rounded-lg p-4 bg-white shadow mt-1">
      <h4 className="text-lg font-semibold mb-2 text-bice-blue">
        Manage Grades for Student ID:{" "}
        <span className="text-gray-700">{studentId.slice(0, 5)}</span>
      </h4>

      <ul className="mb-4 space-y-1">
        {grades.length > 0 ? (
          grades.map((g) => (
            <li key={g.id} className="flex justify-between items-center">
              <span className="ml-2">
                <strong>{g.value}</strong> •{" "}
                <span className="text-gray-500 text-sm">
                  {new Date(g.date).toLocaleDateString("en-GB")}
                </span>
              </span>
              <button
                onClick={() => handleDeleteGrade(g.id)}
                className="text-red-600 hover:underline text-sm"
              >
                Delete
              </button>
            </li>
          ))
        ) : (
          <li className="text-gray-500">No grades found.</li>
        )}
      </ul>

      <div className="flex gap-2 items-center">
        <input
          type="number"
          min={1}
          max={10}
          value={newGrade || ""}
          onChange={(e) => setNewGrade(Number(e.target.value))}
          className="border rounded px-3 py-1 w-24"
          placeholder="Grade"
        />
        <button
          onClick={handleAddGrade}
          className="bg-blue-600 text-white px-4 py-1 rounded hover:bg-blue-700"
        >
          Add
        </button>
      </div>
    </div>
  );
};

export default GradeManager;
