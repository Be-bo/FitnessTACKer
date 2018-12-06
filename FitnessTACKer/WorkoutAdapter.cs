using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using FitnessTACKer;
using System;
using System.Collections.Generic;

namespace FitnessTACKer.Adapter
{
    public class WorkoutAdapter : RecyclerView.Adapter
    {

        private List<WorkoutItem> data;
        public event EventHandler<int> ItemClick;
        private Context context;
        private View itemView;

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

            workoutHolder.MoreOptionsButton.Visibility = ViewStates.Visible;
            workoutHolder.MoreOptionsMenu.Visibility = ViewStates.Gone;

            if (data[position].editMode)
            {
                ToggleEditMode(true, workoutHolder, position);
            } else
            {
                ToggleEditMode(false, workoutHolder, position);

                workoutHolder.Title.Text = data[position].title;
                workoutHolder.Exercises.Text = data[position].exercises != null && data[position].exercises.Length > 0 ? data[position].exercises : "";

                if (!workoutHolder.AddExerciseBtn.HasOnClickListeners)
                {
                    workoutHolder.AddExerciseBtn.Click += delegate (object senderExercise, EventArgs eExercise) {
                        AddExerciseOnClick(senderExercise, eExercise, workoutHolder.AddExerciseBtn, workoutHolder.ExpandedLayout, position, workoutHolder.Root);
                    };
                }

                ConfigureMoreOptionsMenu(workoutHolder);

                if (data[position].expanded)
                {
                    // expand
                    workoutHolder.ExpandedLayout.RemoveAllViews();
                    workoutHolder.ExpandedLayout.Visibility = ViewStates.Visible;
                    workoutHolder.Exercises.Visibility = ViewStates.Gone;
                    workoutHolder.AddExerciseBtn.Visibility = ViewStates.Visible;

                    // setup widgets for expanded view
                    String[] exercisesList = new String[0];
                    if (data[position].exercises != null)
                    {
                        exercisesList = data[position].exercises.Split('\n');
                        for (int i = 0; i < exercisesList.Length; i++)
                        {
                            View exerciseView = LayoutInflater.From(context).Inflate(Resource.Layout.ListItemExercise, null);
                            exerciseView.FindViewById<TextView>(Resource.Id.exercise_name).Text = exercisesList[i];
                            LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                            ll.TopMargin = 8; ll.BottomMargin = 8;
                            exerciseView.LayoutParameters = ll;
                            workoutHolder.ExpandedLayout.AddView(exerciseView);

                            if (i == 0)
                            {
                                // expand first item
                                exerciseView.FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility = ViewStates.Visible;
                                exerciseView.FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Visible;
                                exerciseView.FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Visible;
                            }
                            else
                            {
                                exerciseView.FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility = ViewStates.Gone;
                                exerciseView.FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Gone;
                                exerciseView.FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Gone;
                            }

                            LinearLayout rootExerciseItem = exerciseView.FindViewById<LinearLayout>(Resource.Id.root_exercise_item);
                            Button addSetBtn = exerciseView.FindViewById<Button>(Resource.Id.add_set_btn);

                            if (!rootExerciseItem.HasOnClickListeners)
                            {
                                rootExerciseItem.Click += delegate (object rootSender, EventArgs rootE) { ExerciseItemOnClick(rootSender, rootE); };
                            }

                            if (!addSetBtn.HasOnClickListeners)
                            {
                                addSetBtn.Click += delegate (object addSetSender, EventArgs addSetE) { ExerciseItemOnClick(addSetSender, addSetE); };
                            }
                        }
                    }
                   
                    if (workoutHolder.Exercises.Text.Length > 0 && workoutHolder.Exercises.Text.Split('\n').Length < exercisesList.Length)
                    {
                        View exerciseView = LayoutInflater.From(context).Inflate(Resource.Layout.ListItemExercise, null);
                        exerciseView.FindViewById<TextView>(Resource.Id.exercise_name).Text = exercisesList[exercisesList.Length - 1];
                        LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        ll.TopMargin = 8; ll.BottomMargin = 8;
                        exerciseView.LayoutParameters = ll;
                        workoutHolder.ExpandedLayout.AddView(exerciseView);
                    }

                }
                else
                {
                    // collapse
                    workoutHolder.ExpandedLayout.Visibility = ViewStates.Gone;
                    workoutHolder.Exercises.Visibility = ViewStates.Visible;
                    workoutHolder.AddExerciseBtn.Visibility = ViewStates.Gone;
                }
            }
        }

        private void ConfigureMoreOptionsMenu(WorkoutViewHolder workoutViewHolder)
        {
            if (!workoutViewHolder.MoreOptionsButton.HasOnClickListeners)
            {
                workoutViewHolder.MoreOptionsButton.Click += delegate (object sender, EventArgs e)
                {
                    workoutViewHolder.MoreOptionsButton.Visibility = ViewStates.Invisible;
                    workoutViewHolder.MoreOptionsMenu.Visibility = ViewStates.Visible;
                };
            }
            if (!workoutViewHolder.SaveWorkoutBtn.HasOnClickListeners)
            {
                workoutViewHolder.SaveWorkoutBtn.Click += delegate (object sender2, EventArgs e2)
                {
                    workoutViewHolder.MoreOptionsButton.Visibility = ViewStates.Visible;
                    workoutViewHolder.MoreOptionsMenu.Visibility = ViewStates.Gone;
                };
            }
            if (!workoutViewHolder.EditWorkoutBtn.HasOnClickListeners)
            {
                workoutViewHolder.EditWorkoutBtn.Click += delegate (object sender3, EventArgs e3)
                {
                    workoutViewHolder.MoreOptionsButton.Visibility = ViewStates.Visible;
                    workoutViewHolder.MoreOptionsMenu.Visibility = ViewStates.Gone;
                };
            }
        }

        private void ToggleEditMode(bool edit, WorkoutViewHolder workoutHolder, int position)
        {
            if (edit)
            {
                workoutHolder.RootWorkoutLayout.Visibility = ViewStates.Gone;
                workoutHolder.EditModeRoot.Visibility = ViewStates.Visible;

                ShowKeyboard(workoutHolder.NewWorkoutName);
                
                workoutHolder.NewWorkoutName.FocusChange += new EventHandler<View.FocusChangeEventArgs>((sender, e) =>
                {
                    string newWorkoutName = workoutHolder.NewWorkoutName.Text.ToString();
                    if (!e.HasFocus && newWorkoutName.Length>0)
                    {
                        // save new workout name
                        data[position].title = newWorkoutName;
                        data[position].editMode = false;
                        HideKeyboard(workoutHolder.NewWorkoutName);
                    }
                });
                workoutHolder.NewWorkoutName.EditorAction += (sender, e) => {
                    if (e.ActionId == ImeAction.Done)
                    {
                        string newWorkoutName = workoutHolder.NewWorkoutName.Text.ToString();
                        if (newWorkoutName.Length > 0)
                        {
                            // save new workout name
                            data[position].title = newWorkoutName;
                            data[position].editMode = false;
                            data[position].expanded = true;
                            HideKeyboard(workoutHolder.NewWorkoutName);
                            NotifyItemChanged(position);
                        }
                    }
                    else
                    {
                        e.Handled = false;
                    }
                };

            } else
            {
                workoutHolder.EditModeRoot.Visibility = ViewStates.Gone;
                workoutHolder.RootWorkoutLayout.Visibility = ViewStates.Visible;
            }
        }

        public void AddExerciseOnClick(object sender, EventArgs e, Button addExerciseBtn, LinearLayout currentExpandedLayout, int currentPosition, CardView rootCardView)
        {
            View newExerciseView = LayoutInflater.From(context).Inflate(Resource.Layout.ListItemExerciseEditMode, null);
            LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            ll.TopMargin = 8; ll.BottomMargin = 8;
            newExerciseView.LayoutParameters = ll;
            currentExpandedLayout.AddView(newExerciseView);
            // save view's current position for later use
            int viewPosition = currentExpandedLayout.ChildCount - 1;

            // TODO show exercise suggestions
            EditText exerciseEdittext = newExerciseView.FindViewById<EditText>(Resource.Id.exercise_name_edittext);
            ImageButton currentCheckBtn = newExerciseView.FindViewById<ImageButton>(Resource.Id.check_btn);
            currentCheckBtn.Enabled = false;
            exerciseEdittext.RequestFocus();
            exerciseEdittext.TextChanged -= delegate (object sender2, TextChangedEventArgs e2) { ExerciseNameTextChange(sender2, e2, currentCheckBtn); };
            exerciseEdittext.TextChanged += delegate (object sender2, TextChangedEventArgs e2) { ExerciseNameTextChange(sender2, e2, currentCheckBtn); };
            if (!currentCheckBtn.HasOnClickListeners)
            {
                currentCheckBtn.Click += delegate (object senderCheckBtn, EventArgs eCheckBtn) {
                    ExerciseCheckBtnClick(senderCheckBtn, eCheckBtn, exerciseEdittext, currentPosition, currentExpandedLayout, currentCheckBtn, ll, viewPosition, rootCardView);
                };
            }
        }

        public void ExerciseCheckBtnClick(object sender, EventArgs e, EditText exerciseEdittext, int currentPosition, 
            LinearLayout currentExpandedLayout, ImageButton currentCheckBtn, LinearLayout.LayoutParams ll, int index, CardView rootCardView)
        {
            if (exerciseEdittext.Text.ToString().Length > 0)
            {
                HideKeyboard(exerciseEdittext);

                // format exercises list
                data[currentPosition].exercises += (data[currentPosition].exercises!=null ? "\n" : "") + exerciseEdittext.Text.ToString();
                
                // replace edit view with normal exercise view 
                currentExpandedLayout.RemoveViewAt(index);
                View exerciseView = LayoutInflater.From(context).Inflate(Resource.Layout.ListItemExercise, null);
                exerciseView.FindViewById<TextView>(Resource.Id.exercise_name).Text = exerciseEdittext.Text.ToString();
                exerciseView.LayoutParameters = ll;
                currentExpandedLayout.AddView(exerciseView, index);

                LinearLayout rootExerciseItem = exerciseView.FindViewById<LinearLayout>(Resource.Id.root_exercise_item);
                Button addSetBtn = exerciseView.FindViewById<Button>(Resource.Id.add_set_btn);

                if (!rootExerciseItem.HasOnClickListeners)
                {
                    rootExerciseItem.Click += delegate (object rootSender, EventArgs rootE) { ExerciseItemOnClick(rootSender, rootE); };
                }

                if (!addSetBtn.HasOnClickListeners)
                {
                    addSetBtn.Click += delegate (object addSetSender, EventArgs addSetE) { ExerciseItemOnClick(addSetSender, addSetE); };
                }
            }
        }

        private void ShowKeyboard(View pView)
        {
            pView.RequestFocus();

            InputMethodManager inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(pView, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }

        private void HideKeyboard(EditText et)
        {
            if (et.HasFocus)
                et.ClearFocus();
            var imm = (InputMethodManager)context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(et.WindowToken, 0);
        }

        private void ExerciseNameTextChange(object sender, TextChangedEventArgs e, ImageButton currentCheckBtn)
        {
            if (e.Text.ToString().Length > 0)
            {
                currentCheckBtn.Enabled = true;
            } else
            {
                currentCheckBtn.Enabled = false;
            }
        }
        
        public void ExerciseItemOnClick(object sender, EventArgs e)
        {
            int senderId = ((View)sender).Id;
            if (senderId == Resource.Id.add_set_btn)
            {
                View setView = LayoutInflater.From(context).Inflate(Resource.Layout.ExerciseSetItem, null);
                LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                ll.TopMargin = 8; ll.BottomMargin = 8;
                setView.LayoutParameters = ll;
                ((((Button)sender).Parent as LinearLayout).GetChildAt(2) as LinearLayout).AddView(setView);
            }
            else if (senderId == Resource.Id.root_exercise_item)
            {
                if (((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility == ViewStates.Gone)
                {
                    // expand
                    ((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility = ViewStates.Visible;
                    ((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Visible;
                    ((LinearLayout)sender).FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Visible;
                }
                else
                {
                    // collapse
                    ((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility = ViewStates.Gone;
                    ((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Gone;
                    ((LinearLayout)sender).FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Gone;
                    var im = ((InputMethodManager)context.GetSystemService(Android.Content.Context.InputMethodService));

                    if (!((LinearLayout)sender).FindViewById<EditText>(Resource.Id.target1).HasFocus)
                    {
                        im.HideSoftInputFromWindow(((LinearLayout)sender).FindViewById<EditText>(Resource.Id.target1).WindowToken, 0);
                    };
                }
            }
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            itemView = LayoutInflater.From(parent.Context).
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