using System.Collections.Generic;

namespace FitnessTACKer
{
    public class ExerciseItem
    {
        public string exerciseName { get; set; }
        public bool expanded { get; set; }
        public List<string> sets { get; set; }
    }
}