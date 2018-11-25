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
        public TextView Temp { get; set; }

        public WorkoutViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.workout_title);
            Temp = itemView.FindViewById<TextView>(Resource.Id.temp_temp);
            Exercises = itemView.FindViewById<TextView>(Resource.Id.tv_exercises);
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
}