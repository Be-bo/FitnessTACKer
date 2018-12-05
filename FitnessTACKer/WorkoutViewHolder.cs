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
        public Button AddExerciseBtn { get; set; }
        public CardView Root { get; set; }
        public ImageButton MoreOptionsButton { get; set; }

        public WorkoutViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.workout_title);
            Exercises = itemView.FindViewById<TextView>(Resource.Id.tv_exercises);
            ExpandedLayout = itemView.FindViewById<LinearLayout>(Resource.Id.expanded_layout);
            ExerciseItem = itemView.FindViewById<LinearLayout>(Resource.Id.root_exercise_item);
            AddExerciseBtn = itemView.FindViewById<Button>(Resource.Id.add_exercise_btn);
            Root = itemView.FindViewById<CardView>(Resource.Id.root_list_item_workout);
            MoreOptionsButton = itemView.FindViewById<ImageButton>(Resource.Id.more_options_btn);

            // AddExerciseBtn.Click += (sender, e) => listener(base.AdapterPosition);
            itemView.Click += (sender, e) => listener(base.AdapterPosition);
            Title.Click += (sender, e) => listener(base.AdapterPosition);
            // MoreOptionsButton.Click += (sender, e) => listener(base.AdapterPosition);
        }
    }
}