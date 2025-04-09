import { useState } from "react";

type GradeInput = {
  studentId: string;
  value: number;
};

type BulkGradeUploaderProps = {
  classId: string;
  onUploadSuccess: () => void;
};

const BulkGradeUploader = ({
  classId,
  onUploadSuccess,
}: BulkGradeUploaderProps) => {
  const [entries, setEntries] = useState<GradeInput[]>([
    { studentId: "", value: 0 },
  ]);
  const [message, setMessage] = useState("");

  const handleEntryChange = (
    index: number,
    field: keyof GradeInput,
    value: string | number
  ) => {
    const updated = [...entries];
    if (field === "value") {
      updated[index].value = Number(value);
    } else if (field === "studentId") {
      updated[index].studentId = value as string;
    }
    setEntries(updated);
  };

  const addEntry = () => {
    setEntries([...entries, { studentId: "", value: 0 }]);
  };

  const removeEntry = (index: number) => {
    setEntries(entries.filter((_, i) => i !== index));
  };

  const submitGrades = async () => {
    const valid = entries.every(
      (e) => e.studentId && e.value >= 1 && e.value <= 10
    );
    if (!valid) {
      setMessage("Please enter valid student IDs and grades between 1-10.");
      return;
    }

    const res = await fetch(
      `https://localhost:64060/api/classes/${classId}/bulk-grades`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ grades: entries }),
      }
    );

    if (res.ok) {
      if (res.ok) {
        setMessage("Grades uploaded successfully.");
        setEntries([{ studentId: "", value: 0 }]);

        onUploadSuccess();
      }
    } else {
      const error = await res.text();
      setMessage("Failed to upload grades: " + error);
    }
  };

  return (
    <div className="bg-white p-6 rounded shadow mt-6 w-[320px] max-w-md">
      <h4 className="text-lg font-bold mb-4 text-bice-blue">
        Bulk Grade Upload
      </h4>

      {entries.map((entry, index) => (
        <div key={index} className="flex gap-2 mb-2 items-center">
          <input
            type="text"
            placeholder="Student ID"
            value={entry.studentId}
            onChange={(e) =>
              handleEntryChange(index, "studentId", e.target.value)
            }
            className="border px-2 py-1 rounded w-[140px]"
          />
          <input
            type="number"
            min={1}
            max={10}
            placeholder="Grade"
            value={entry.value || ""}
            onChange={(e) => handleEntryChange(index, "value", e.target.value)}
            className="border px-2 py-1 rounded w-[80px]"
          />
          <button
            onClick={() => removeEntry(index)}
            className="text-red-500 hover:underline text-sm"
          >
            Remove
          </button>
        </div>
      ))}

      <button
        onClick={addEntry}
        className="text-blue-600 hover:underline text-sm mb-4"
      >
        + Add Another
      </button>

      <button
        onClick={submitGrades}
        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 ml-10"
      >
        Submit Grades
      </button>

      {message && (
        <p className="text-sm mt-3 text-center text-gray-700">{message}</p>
      )}
    </div>
  );
};

export default BulkGradeUploader;
