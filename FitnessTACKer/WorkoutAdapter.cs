using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using FitnessTACKer;
using System.Collections.Generic;

namespace FitnessTACKer.Adapter
{
    public class WorkoutAdapter : RecyclerView.Adapter
    {

        private List<WorkoutItem> data;

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
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.ListItemWorkout, parent, false);

            return new WorkoutViewHolder(itemView);
        }
    }
}