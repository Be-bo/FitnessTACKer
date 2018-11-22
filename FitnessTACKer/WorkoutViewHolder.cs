using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace FitnessTACKer
{
    public class WorkoutViewHolder : RecyclerView.ViewHolder
    {
        public TextView Title { get; set; }

        public WorkoutViewHolder(View itemView) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.workout_title);
        }
    }
}