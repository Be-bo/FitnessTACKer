using Android.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using FitnessTACKer;
using System;
using System.Collections.Generic;

namespace FitnessTACKer.Adapter
{
    public class WorkoutAdapter : RecyclerView.Adapter, View.IOnClickListener
    {

        private List<WorkoutItem> data;
        public event EventHandler<int> ItemClick;
        private Context context;

        public WorkoutAdapter(Context c, List<WorkoutItem> workouts)
        {
            data = workouts;
            context = c;
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
                workoutHolder.ExpandedLayout.RemoveAllViews();
                workoutHolder.ExpandedLayout.Visibility = ViewStates.Visible;
                workoutHolder.Exercises.Visibility = ViewStates.Gone;
                workoutHolder.AddExerciseBtn.Visibility = ViewStates.Visible;

                String[] exercisesList = data[position].exercises.Split('\n');
                for (int i=0; i< exercisesList.Length; i++)
                {
                    View exerciseView = LayoutInflater.From(context).Inflate(Resource.Layout.ListItemExercise, null);
                    exerciseView.FindViewById<TextView>(Resource.Id.exercise_name).Text = exercisesList[i];
                    LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    ll.TopMargin = 8; ll.BottomMargin = 8;
                    exerciseView.LayoutParameters = ll;
                    workoutHolder.ExpandedLayout.AddView(exerciseView);

                    if (i==0)
                    {
                        // expand first item
                        exerciseView.FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility = ViewStates.Visible;
                        exerciseView.FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Visible;
                        exerciseView.FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Visible;
                    } else
                    {
                        exerciseView.FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility = ViewStates.Gone;
                        exerciseView.FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Gone;
                        exerciseView.FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Gone;
                    }

                    LinearLayout rootExerciseItem = exerciseView.FindViewById<LinearLayout>(Resource.Id.root_exercise_item);
                    Button addSetBtn = exerciseView.FindViewById<Button>(Resource.Id.add_set_btn);

                    if (!rootExerciseItem.HasOnClickListeners)
                        rootExerciseItem.SetOnClickListener(this);

                    if (!addSetBtn.HasOnClickListeners)
                        addSetBtn.SetOnClickListener(this);
                }

                if (workoutHolder.Exercises.Text.Split('\n').Length < exercisesList.Length)
                {
                    View exerciseView = LayoutInflater.From(context).Inflate(Resource.Layout.ListItemExercise, null);
                    exerciseView.FindViewById<TextView>(Resource.Id.exercise_name).Text = exercisesList[exercisesList.Length-1];
                    LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    ll.TopMargin = 8; ll.BottomMargin = 8;
                    exerciseView.LayoutParameters = ll;
                    workoutHolder.ExpandedLayout.AddView(exerciseView);
                }

            } else
            {
                // collapse
                workoutHolder.ExpandedLayout.Visibility = ViewStates.Gone;
                workoutHolder.Exercises.Visibility = ViewStates.Visible;
                workoutHolder.AddExerciseBtn.Visibility = ViewStates.Gone;
            }
        }

        // exercise item onClick
        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.root_exercise_item)
            {
                if (v.FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility == ViewStates.Gone)
                {
                    v.FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility = ViewStates.Visible;
                    v.FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Visible;
                    v.FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Visible;
                }
                else
                {
                    v.FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility = ViewStates.Gone;
                    v.FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Gone;
                    v.FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Gone;
                }
            }
            else if (v.Id == Resource.Id.add_set_btn)
            {
                View setView = LayoutInflater.From(context).Inflate(Resource.Layout.ExerciseSetItem, null);
                LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                ll.TopMargin = 8; ll.BottomMargin = 8;
                setView.LayoutParameters = ll;
                ((v.Parent as LinearLayout).GetChildAt(2) as LinearLayout).AddView(setView);
            }
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.ListItemWorkout, parent, false);

            return new WorkoutViewHolder(itemView, OnClick);
        }

        // workout item onClick
        private void OnClick(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, position);
            }
        }
    }
}