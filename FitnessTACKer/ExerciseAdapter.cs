using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using FitnessTACKer;
using System;
using System.Collections.Generic;

namespace FitnessTACKer.Adapter
{
    public class ExerciseAdapter : RecyclerView.Adapter
    {

        private List<ExerciseItem> data;
        public event EventHandler<int> ItemClick;

        public ExerciseAdapter(List<ExerciseItem> exercises)
        {
            data = exercises;
        }

        public override int ItemCount
        {
            get { return data.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ExerciseViewHolder eHolder = holder as ExerciseViewHolder;
            eHolder.ExerciseName.Text = data[position].exerciseName;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.ListItemExercise, parent, false);

            return new ExerciseViewHolder(itemView, OnClick);
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