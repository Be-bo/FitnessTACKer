using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;

namespace FitnessTACKer
{
    public class WorkoutViewHolder : RecyclerView.ViewHolder
    {
        public TextView Title { get; set; }
        public TextView Exercises { get; set; }
        public LinearLayout ExpandedLayout { get; set; }
        public LinearLayout ExerciseItem { get; set; }

        public WorkoutViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.workout_title);
            Exercises = itemView.FindViewById<TextView>(Resource.Id.tv_exercises);
            ExpandedLayout = itemView.FindViewById<LinearLayout>(Resource.Id.expanded_layout);
            ExerciseItem = itemView.FindViewById<LinearLayout>(Resource.Id.root_exercise_item);

            itemView.Click += (sender, e) => listener(base.AdapterPosition);
        }
    }
}