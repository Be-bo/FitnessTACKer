namespace FitnessTACKer
{
    public class WorkoutItem : Java.Lang.Object
    {
        public string title { get; set; }
        public bool expanded { get; set; }
        public string exercises { get; set; }
        public bool editMode { get; set; }
    }
}
