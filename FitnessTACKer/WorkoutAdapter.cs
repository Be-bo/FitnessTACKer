using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using FitnessTACKer;
using System;
using System.Collections.Generic;

namespace FitnessTACKer.Adapter
{
    public class WorkoutAdapter : RecyclerView.Adapter
    {

        private List<WorkoutItem> data;
        public event EventHandler<int> ItemClick;

        public WorkoutAdapter(List<WorkoutItem> workouts)
        {
            data = workouts;
        }

        public override int ItemCount
        {
            get { return data.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            WorkoutViewHolder workoutHolder = holder as WorkoutViewHolder;
            workoutHolder.Title.Text = data[position].title;
            workoutHolder.Exercises.Text = data[position].exercises;

            if (data[position].expanded)
            {
                // expand
                workoutHolder.Temp.Visibility = ViewStates.Visible;
            } else
            {
                // collapse
                workoutHolder.Temp.Visibility = ViewStates.Gone;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.ListItemWorkout, parent, false);

            return new WorkoutViewHolder(itemView, OnClick);
        }

        private void OnClick(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, position);
            }
        }
    }
}