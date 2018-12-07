using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;

namespace FitnessTACKer
{
    public class ExerciseViewHolder : RecyclerView.ViewHolder
    {
        public TextView ExerciseName { get; set; }

        public ExerciseViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            ExerciseName = ItemView.FindViewById<TextView>(Resource.Id.exercise_name);
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
}