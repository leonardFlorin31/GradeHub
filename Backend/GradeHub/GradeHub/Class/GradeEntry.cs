
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
        public DateTime Timestamp { get; private set; }
        public Grade Grade { get; private set; }

        public GradeEntry(DateTime timestamp, Grade grade)
        {
            Timestamp = timestamp;
            Grade = grade;
        }
    }
}
