using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;

namespace FitnessTACKer
{
    public class ExerciseViewHolder : RecyclerView.ViewHolder
    {
        

        public ExerciseViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
}