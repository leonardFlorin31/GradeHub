import { useEffect, useState } from "react";

const ClassManager = ({
  teacherClass,
  onChange,
}: {
  teacherClass: any;
  onChange: () => void;
}) => {
  const [allStudents, setAllStudents] = useState<any[]>([]);
  const [assignedIds, setAssignedIds] = useState<string[]>([]);
  const encodedClassName = teacherClass
    ? encodeURIComponent(teacherClass.className)
    : "";

  useEffect(() => {
    fetch("https://localhost:64060/api/students")
      .then((res) => res.json())
      .then((data) => setAllStudents(data));

    if (teacherClass) {
      const ids = teacherClass.students.map((s: any) => s.id);
      setAssignedIds(ids);
    }
  }, [teacherClass]);

  const handleAddStudent = async (studentId: string) => {
    const res = await fetch(
      `https://localhost:64060/api/classes/${encodedClassName}/students/${studentId}`,
      { method: "POST" }
    );

    if (res.ok) {
      setAssignedIds((prev) => [...prev, studentId]);
      onChange(); // ⬅️ Trigger parent refetch
    } else {
      const error = await res.text();
      console.error("Add error:", error);
      alert("Could not add student to class.");
    }
  };

  const handleRemoveStudent = async (studentId: string) => {
    const res = await fetch(
      `https://localhost:64060/api/classes/${encodedClassName}/students/${studentId}`,
      { method: "DELETE" }
    );

    if (res.ok) {
      setAssignedIds((prev) => prev.filter((id) => id !== studentId));
      onChange(); // ⬅️ Trigger parent refetch
    } else {
      const error = await res.text();
      console.error("Remove error:", error);
      alert("Failed to remove student from class.");
    }
  };

  return (
    <div className="bg-white rounded-lg p-6 shadow-md border border-gray-300 w-[300px]">
      <h3 className="text-lg font-bold mb-4 text-bice-blue text-center">
        Manage Class Students
      </h3>

      <ul className="space-y-2 max-h-[60vh] overflow-y-auto">
        {[...allStudents]
          .sort((a, b) => a.name.localeCompare(b.name))
          .map((student) => (
            <li
              key={student.id}
              className="flex justify-between items-center border-b pb-1"
            >
              <span>
                {student.name} ({student.id.slice(0, 5)})
              </span>
              {assignedIds.includes(student.id) ? (
                <button
                  onClick={() => handleRemoveStudent(student.id)}
                  className="text-red-600 hover:underline text-sm"
                >
                  Remove
                </button>
              ) : (
                <button
                  onClick={() => handleAddStudent(student.id)}
                  className="text-blue-600 hover:underline text-sm"
                >
                  Add
                </button>
              )}
            </li>
          ))}
      </ul>
    </div>
  );
};

export default ClassManager;
