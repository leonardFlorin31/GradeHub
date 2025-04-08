
using GradeHub.MainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeHub.Class
{
    public class GradeEntry
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime Timestamp { get; private set; }
        public Grade Grade { get; private set; }

        public GradeEntry(Guid id, DateTime timestamp, Grade grade)
        {
            Id = id;
            Timestamp = timestamp;
            Grade = grade;
        }

        public GradeEntry(DateTime timestamp, Grade grade)
        {
            Id = Guid.NewGuid(); // new ID for new entries
            Timestamp = timestamp;
            Grade = grade;
        }
    }
}
