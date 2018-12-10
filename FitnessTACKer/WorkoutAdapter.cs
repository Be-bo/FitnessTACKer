﻿using Android.App;
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
using System.ComponentModel;
using System.Reflection;
using static Java.Util.ResourceBundle;

namespace FitnessTACKer.Adapter
{
    public class WorkoutAdapter : RecyclerView.Adapter
    {

        private List<WorkoutItem> data;
        public event EventHandler<int> ItemClick;
        private Context context;
        private View itemView;
        private View root_view;
        private Keyboard keyboard;

        public WorkoutAdapter(Context c, List<WorkoutItem> workouts, Keyboard key, View v)
        {
            data = workouts;
            context = c;
            keyboard = key;
            root_view = v;
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

            if (data[position].editModeNewWorkout)
            {
                // ResetListeners(workoutHolder, position);
                ToggleEditModeExisting(false, workoutHolder, position);
                ToggleEditModeNewWorkout(true, workoutHolder, position);

            } else if (data[position].editModeExisting)
            {
                // ResetListeners(workoutHolder, position);
                ToggleEditModeNewWorkout(false, workoutHolder, position);
                ToggleEditModeExisting(true, workoutHolder, position);
            }
            else
            {
                ToggleEditModeNewWorkout(false, workoutHolder, position);
                ToggleEditModeExisting(false, workoutHolder, position);

                workoutHolder.Title.Text = data[position].title;
                workoutHolder.Exercises.Text = data[position].exercises != null && data[position].exercises.Length > 0 ? data[position].exercises : "";

                if (!workoutHolder.AddExerciseBtn.HasOnClickListeners)
                {
                    workoutHolder.AddExerciseBtn.Click += delegate (object senderExercise, EventArgs eExercise) {
                        AddExerciseOnClick(senderExercise, eExercise, workoutHolder.AddExerciseBtn, workoutHolder.ExpandedLayout, position, workoutHolder.Root);
                    };
                }

                ConfigureMoreOptionsMenu(workoutHolder, position);

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
                            ll.TopMargin = 8; ll.BottomMargin = 8; ll.LeftMargin = 18; ll.RightMargin = 24;
                            exerciseView.LayoutParameters = ll;
                            workoutHolder.ExpandedLayout.AddView(exerciseView);

                            EditText target = exerciseView.FindViewById<EditText>(Resource.Id.target_weight);
                            EditText final = exerciseView.FindViewById<EditText>(Resource.Id.final_weight);
                            setKeyboard(target);
                            setKeyboard(final);

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
                        ll.TopMargin = 8; ll.BottomMargin = 8; ll.LeftMargin = 24; ll.RightMargin = 24;
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

        public void ResetListeners(WorkoutViewHolder workoutHolder, int position)
        {
            if (workoutHolder.AddExerciseBtn.HasOnClickListeners)
            {
                workoutHolder.AddExerciseBtn.Click -= delegate (object senderExercise, EventArgs eExercise) {
                    AddExerciseOnClick(senderExercise, eExercise, workoutHolder.AddExerciseBtn, workoutHolder.ExpandedLayout, position, workoutHolder.Root);
                };
            }

            if (workoutHolder.MoreOptionsButton.HasOnClickListeners)
            {
                workoutHolder.MoreOptionsButton.Click -= delegate (object sender, EventArgs e)
                {
                    workoutHolder.MoreOptionsButton.Visibility = ViewStates.Invisible;
                    workoutHolder.MoreOptionsMenu.Visibility = ViewStates.Visible;
                };
            }
            if (workoutHolder.SaveWorkoutBtn.HasOnClickListeners)
            {
                workoutHolder.SaveWorkoutBtn.Click -= delegate (object sender2, EventArgs e2)
                {
                    workoutHolder.MoreOptionsButton.Visibility = ViewStates.Visible;
                    workoutHolder.MoreOptionsMenu.Visibility = ViewStates.Gone;
                    Toast.MakeText(context, context.GetString(Resource.String.workout_saved, workoutHolder.Title.Text.ToString()), ToastLength.Long).Show();
                };
            }
            if (workoutHolder.EditWorkoutBtn.HasOnClickListeners)
            {
                workoutHolder.EditWorkoutBtn.Click -= delegate (object sender3, EventArgs e3)
                {
                    data[position].editModeExisting = true;
                    NotifyItemChanged(position);
                };
            }
            String[] exercisesList = new String[0];
            if (data[position].exercises != null)
            {
                exercisesList = data[position].exercises.Split('\n');
                for (int i = 0; i < exercisesList.Length; i++)
                {
                    View exerciseView = workoutHolder.ExpandedLayout.GetChildAt(i);
                    if (exerciseView != null)
                    {
                        LinearLayout rootExerciseItem = exerciseView.FindViewById<LinearLayout>(Resource.Id.root_exercise_item);
                        Button addSetBtn = exerciseView.FindViewById<Button>(Resource.Id.add_set_btn);

                        if (rootExerciseItem != null && rootExerciseItem.HasOnClickListeners)
                        {
                            rootExerciseItem.Click -= delegate (object rootSender, EventArgs rootE) { ExerciseItemOnClick(rootSender, rootE); };
                        }

                        if (addSetBtn != null && addSetBtn.HasOnClickListeners)
                        {
                            addSetBtn.Click -= delegate (object addSetSender, EventArgs addSetE) { ExerciseItemOnClick(addSetSender, addSetE); };
                        }
                    }
                }
            }
        }

        private void ConfigureMoreOptionsMenu(WorkoutViewHolder workoutViewHolder, int position)
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
                    Toast.MakeText(context, context.GetString(Resource.String.workout_saved, workoutViewHolder.Title.Text.ToString()), ToastLength.Long).Show();
                };
            }
            if (!workoutViewHolder.EditWorkoutBtn.HasOnClickListeners)
            {
                workoutViewHolder.EditWorkoutBtn.Click += delegate (object sender3, EventArgs e3)
                {
                    data[position].editModeExisting = true;
                    NotifyItemChanged(position);
                };
            }
        }

        // edit mode for existing workout
        private void ToggleEditModeExisting(bool edit, WorkoutViewHolder workoutHolder, int position)
        {
            if (edit)
            {
                workoutHolder.EditModeRoot.Visibility = ViewStates.Visible;
                workoutHolder.SaveChangesBtn.Visibility = ViewStates.Visible;

                workoutHolder.LayoutTitleAndMenu.Visibility = ViewStates.Gone;
                workoutHolder.Exercises.Visibility = ViewStates.Gone;
                workoutHolder.AddExerciseBtn.Visibility = ViewStates.Gone;

                workoutHolder.ExpandedLayout.Visibility = ViewStates.Visible;
                workoutHolder.ExpandedLayout.RemoveAllViews();

                workoutHolder.NewWorkoutName.Text = data[position].title;
                workoutHolder.NewWorkoutName.RequestFocus();

                workoutHolder.NewWorkoutName.TextChanged += delegate (object newNameTextChanged, TextChangedEventArgs eNewName)
                {
                    WorkoutNameTextChangeEvent(newNameTextChanged, eNewName, workoutHolder.SaveChangesBtn);
                };

                String[] exercises = null;
                if (data[position].exercises != null && data[position].exercises.Length > 0)
                {
                    exercises = data[position].exercises.Split('\n');
                }

                LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
                {
                    TopMargin = 4,
                    BottomMargin = 4
                };

                if (exercises!=null)
                {
                    for (int i = 0; i < exercises.Length; i++)
                    {
                        View editModeExerciseView = LayoutInflater.From(context).Inflate(Resource.Layout.ListItemExerciseEditMode, null);
                        editModeExerciseView.FindViewById<AutoCompleteTextView>(Resource.Id.exercise_name_edittext).Text = exercises[i];
                        ImageButton deleteExerciseBtn = editModeExerciseView.FindViewById<ImageButton>(Resource.Id.delete_exercise_btn);
                        SetSuggestions(editModeExerciseView.FindViewById<AutoCompleteTextView>(Resource.Id.exercise_name_edittext));

                        editModeExerciseView.LayoutParameters = ll;
                        workoutHolder.ExpandedLayout.AddView(editModeExerciseView);

                        SetupTrashcansListenerEditMode(workoutHolder.ExpandedLayout, deleteExerciseBtn, editModeExerciseView);
                    }

                }

                if (!workoutHolder.DeleteWorkoutBtn.HasOnClickListeners)
                {
                    workoutHolder.DeleteWorkoutBtn.Click += delegate (object s, EventArgs e2) { ShowDialogDeleteWorkout(position); };
                }

                workoutHolder.SaveChangesBtn.Click += delegate (object saveChangesSender, EventArgs eSaveChanges) {
                        SaveChangesOnClick(saveChangesSender, eSaveChanges, workoutHolder, position);
                    };
                
            } else
            {
                workoutHolder.LayoutTitleAndMenu.Visibility = ViewStates.Visible;
                workoutHolder.Exercises.Visibility = ViewStates.Visible;
                workoutHolder.AddExerciseBtn.Visibility = ViewStates.Visible;

                workoutHolder.SaveChangesBtn.Visibility = ViewStates.Gone;

                workoutHolder.EditModeRoot.Visibility = ViewStates.Gone;
                workoutHolder.ExpandedLayout.Visibility = ViewStates.Gone;

                // reset listeners
                workoutHolder.NewWorkoutName.TextChanged -= delegate (object newNameTextChanged, TextChangedEventArgs eNewName)
                {
                    WorkoutNameTextChangeEvent(newNameTextChanged, eNewName, workoutHolder.SaveChangesBtn);
                };

                String[] exercises = null;
                if (data[position].exercises != null && data[position].exercises.Length > 0)
                {
                    exercises = data[position].exercises.Split('\n');
                }

                if (exercises != null)
                {
                    for (int i = 0; i < exercises.Length; i++)
                    {
                        View editModeExerciseView = workoutHolder.ExpandedLayout.GetChildAt(i);
                        if (editModeExerciseView != null && editModeExerciseView.FindViewById<ImageButton>(Resource.Id.delete_exercise_btn)!=null)
                        {
                            editModeExerciseView.FindViewById<ImageButton>(Resource.Id.delete_exercise_btn).Click -= delegate (object sender, EventArgs e)
                            {
                                int indexToDelete = FindChildIndexByExerciseName(workoutHolder.ExpandedLayout, editModeExerciseView.FindViewById<AutoCompleteTextView>(Resource.Id.exercise_name_edittext).Text);
                                if (indexToDelete != -1)
                                {
                                    DeleteExerciseEditModeOnClick(sender, e, workoutHolder.ExpandedLayout, indexToDelete);
                                }
                            };
                        }
                    }

                }

                if (workoutHolder.DeleteWorkoutBtn.HasOnClickListeners)
                {
                    workoutHolder.DeleteWorkoutBtn.Click -= delegate (object s, EventArgs e2) { ShowDialogDeleteWorkout(position); };
                }

                if (workoutHolder.SaveChangesBtn.HasOnClickListeners)
                {
                    workoutHolder.SaveChangesBtn.Click -= delegate (object saveChangesSender, EventArgs eSaveChanges) {
                        SaveChangesOnClick(saveChangesSender, eSaveChanges, workoutHolder, position);
                    };
                }
            }
        }

        private void SetupTrashcansListenerEditMode(LinearLayout expandedLayout, ImageButton deleteExerciseBtn, View editModeExerciseView)
        {
            deleteExerciseBtn.Click += delegate (object sender, EventArgs e)
            {
                int indexToDelete = FindChildIndexByExerciseName(expandedLayout, editModeExerciseView.FindViewById<AutoCompleteTextView>(Resource.Id.exercise_name_edittext).Text);
                if (indexToDelete != -1)
                {
                    DeleteExerciseEditModeOnClick(sender, e, expandedLayout, indexToDelete);
                }
            };
        }

        public int FindChildIndexByExerciseName(LinearLayout expandedLayout, string exerciseName)
        {
            for (int i=0; i<expandedLayout.ChildCount; i++)
            {
                if (expandedLayout.GetChildAt(i).FindViewById<AutoCompleteTextView>(Resource.Id.exercise_name_edittext).Text == exerciseName)
                    return i;
            }
            return -1;
        }

        public void DeleteExerciseEditModeOnClick(object sender, EventArgs e, LinearLayout expandedLayout, int i)
        {
            expandedLayout.RemoveViewAt(i);
        }

        public void WorkoutNameTextChangeEvent(object sender, TextChangedEventArgs e, Button saveChangesBtn)
        {
            saveChangesBtn.Enabled = e.Text.ToString().Length > 0;
        }

        public void SaveChangesOnClick(object sender, EventArgs e, WorkoutViewHolder workoutHolder, int position)
        {
            try
            {
                data[position].title = workoutHolder.NewWorkoutName.Text.ToString();
                data[position].exercises = "";
                for (int i = 0; i < workoutHolder.ExpandedLayout.ChildCount; i++)
                {
                    string exercise = workoutHolder.ExpandedLayout.GetChildAt(i).FindViewById<AutoCompleteTextView>(Resource.Id.exercise_name_edittext).Text.ToString();
                    if (exercise.Length > 0)
                    {
                        data[position].exercises += exercise + (i == workoutHolder.ExpandedLayout.ChildCount - 1 ? "" : "\n"); // TODO fix this ?
                    }
                }
                data[position].editModeExisting = false;
                NotifyItemChanged(position);
            } catch (Exception error)
            {
                Log.Error("SHIT", "some shit happened "+error.Message);
            }
        }

        // edit mode for newly added workout
        private void ToggleEditModeNewWorkout(bool edit, WorkoutViewHolder workoutHolder, int position)
        {
            if (edit)
            {
                workoutHolder.RootWorkoutLayout.Visibility = ViewStates.Gone;
                workoutHolder.EditModeRoot.Visibility = ViewStates.Visible;

                ShowKeyboard(workoutHolder.NewWorkoutName);

                workoutHolder.NewWorkoutName.FocusChange += new EventHandler<View.FocusChangeEventArgs>((sender, e) =>
                {
                    string newWorkoutName = workoutHolder.NewWorkoutName.Text.ToString();
                    if (!e.HasFocus && newWorkoutName.Length > 0)
                    {
                        // save new workout name
                        if (position > -1 && data.Count > position)
                        {
                            data[position].title = newWorkoutName;
                            data[position].editModeNewWorkout = false;
                            HideKeyboard(workoutHolder.NewWorkoutName);
                        }
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
                            data[position].editModeNewWorkout = false;
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
                if (!workoutHolder.DeleteWorkoutBtn.HasOnClickListeners)
                {
                    workoutHolder.DeleteWorkoutBtn.Click += delegate (object senderDeleteWorkout, EventArgs eDeleteWorkout)
                    {
                        if (workoutHolder.NewWorkoutName.Text.ToString().Length > 0)
                        {
                            string newWorkoutName = workoutHolder.NewWorkoutName.Text.ToString();
                            ShowConfirmDialog(context.GetString(Resource.String.confirm_save_changes),
                                context.GetString(Resource.String.save), context.GetString(Resource.String.dont_save), position,
                                newWorkoutName, workoutHolder);
                        }
                        else
                        {
                            data.RemoveAt(position);
                            // NotifyItemRemoved(position);
                            NotifyDataSetChanged();
                            HideKeyboard(workoutHolder.NewWorkoutName);
                        }
                    };
                }

            }
            else
            {
                workoutHolder.EditModeRoot.Visibility = ViewStates.Gone;
                workoutHolder.RootWorkoutLayout.Visibility = ViewStates.Visible;

                // reset listeners
                workoutHolder.NewWorkoutName.FocusChange -= new EventHandler<View.FocusChangeEventArgs>((sender, e) =>
                {
                    string newWorkoutName = workoutHolder.NewWorkoutName.Text.ToString();
                    if (!e.HasFocus && newWorkoutName.Length > 0)
                    {
                        // save new workout name
                        if (position > -1 && data.Count > position)
                        {
                            data[position].title = newWorkoutName;
                            data[position].editModeNewWorkout = false;
                            HideKeyboard(workoutHolder.NewWorkoutName);
                        }
                    }
                });
                workoutHolder.NewWorkoutName.EditorAction -= (sender, e) => {
                    if (e.ActionId == ImeAction.Done)
                    {
                        string newWorkoutName = workoutHolder.NewWorkoutName.Text.ToString();
                        if (newWorkoutName.Length > 0)
                        {
                            // save new workout name
                            data[position].title = newWorkoutName;
                            data[position].editModeNewWorkout = false;
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
                if (workoutHolder.DeleteWorkoutBtn.HasOnClickListeners)
                {
                    workoutHolder.DeleteWorkoutBtn.Click -= delegate (object senderDeleteWorkout, EventArgs eDeleteWorkout)
                    {
                        if (workoutHolder.NewWorkoutName.Text.ToString().Length > 0)
                        {
                            string newWorkoutName = workoutHolder.NewWorkoutName.Text.ToString();
                            ShowConfirmDialog(context.GetString(Resource.String.confirm_save_changes),
                                context.GetString(Resource.String.save), context.GetString(Resource.String.dont_save), position,
                                newWorkoutName, workoutHolder);
                        }
                        else
                        {
                            data.RemoveAt(position);
                            // NotifyItemRemoved(position);
                            NotifyDataSetChanged();
                            HideKeyboard(workoutHolder.NewWorkoutName);
                        }
                    };
                }

            }
        }

        private void ShowDialogDeleteWorkout(int position)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            AlertDialog dialog = null;
            builder.SetTitle("Are you sure you want to delete this workout?");
            builder.SetPositiveButton("Delete", delegate (object s, DialogClickEventArgs ev) {
                if (position > -1 && position < data.Count)
                {
                    data.RemoveAt(position);
                    NotifyDataSetChanged();
                }
            });
            builder.SetNegativeButton("Cancel", delegate (object s2, DialogClickEventArgs ev2) {
                if (dialog != null) dialog.Dismiss();
            });

            dialog = builder.Create();
            dialog.Show();
        }

        public void ShowConfirmDialog(string title, string pos, string neg, int adapterPosition, string newWorkoutName, WorkoutViewHolder workoutHolder)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle(title);
            builder.SetPositiveButton(pos, delegate (object s, DialogClickEventArgs ev) {
                // save new workout name
                data[adapterPosition].title = newWorkoutName;
                data[adapterPosition].editModeNewWorkout = false;
                data[adapterPosition].expanded = true;
                HideKeyboard(workoutHolder.NewWorkoutName);
                NotifyItemChanged(adapterPosition);
            });
            builder.SetNegativeButton(neg, delegate (object s2, DialogClickEventArgs ev2) {
                data.RemoveAt(adapterPosition);
                NotifyDataSetChanged();
                // NotifyItemRemoved(adapterPosition);
                HideKeyboard(workoutHolder.NewWorkoutName);
            });

            builder.Create().Show();

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

            AutoCompleteTextView exerciseEdittext = newExerciseView.FindViewById<AutoCompleteTextView>(Resource.Id.exercise_name_edittext);
            SetSuggestions(exerciseEdittext);

            ShowKeyboard(exerciseEdittext);

            ConfigureExerciseTrashcanListener(newExerciseView, viewPosition, currentExpandedLayout, exerciseEdittext);

            // TODO show exercise suggestions

            exerciseEdittext.EditorAction += (senderExerciseEt, eExerciseEt) => {
                if (eExerciseEt.ActionId == ImeAction.Done)
                {
                    string newExerciseName = exerciseEdittext.Text.ToString();
                    if (newExerciseName.Length > 0)
                    {
                        OnAddExerciseEvent(exerciseEdittext, currentPosition, currentExpandedLayout, ll, viewPosition, rootCardView);
                    };
                }
                else
                {
                    eExerciseEt.Handled = false;
                }
            };
            // exerciseEdittext.FocusChange +=

        }

        private void SetSuggestions(AutoCompleteTextView text)
        {
            //Suggestions
            var autoCompleteOptions = new String[]{"Lat Pulldown", "Barbell Bench Press", "Dumbbell Bench Press", "Pull Up", "Chin Up", "Dumbbell Shoulder Press",
            "Dumbbell Lateral Raise", "Lateral Raise Machine", "Dumbbell Front Raise", "Dumbbell Row", "Barbell Row", "Rowing Machine", "Rear Delt Fly Machine", "Chest Fly Machine",
            "Pec Deck", "Deadlift", "Barbell Back Squat", "Barbell Front Squat", "Leg Press Horizontal", "Leg Press Vertical", "Calf Raise", "Glute Machine", "Glute Bridge",
            "Arnold Press", "T Bar Row", "Cable Fly", "Dumbbell Kick Back", "Narrow Bench Press", "Rope Pushdown", "Dip", "Push Up", "Straight Bar Pushdown", "Barbell Curl",
            "Dumbbell Curl", "Hammer Curl", "EZ Bar Curl", "Incline Barbell Bench Press", "Decline Barbell Bench Press", "Incline Dumbbell Press", "Decline Dumbbell Press",
            "Dumbbell Fly", "Farmer Walk", "Over Head Press", "Seated Dumbbell Curl", "Concentration Curl", "Hack Squat", "Smith Machine Lunge", "Smith Machine Press",
            "Smith Machine Over Head Press", "Seated Calf Raise", "Crunch", "Sit Up", "Leg Raise", "Handing Leg Raise", "Rope Crunch", "Rope Tricep Extension",
            "Behind Head Dumbbell Extension", "Read Delt Row", "Dumbbell Shrug", "Hyperextension"};
            ArrayAdapter autoCompleteAdapter = new ArrayAdapter(context, Android.Resource.Layout.SimpleDropDownItem1Line, autoCompleteOptions);
            text.Adapter = autoCompleteAdapter;
        }

        private void ConfigureExerciseTrashcanListener(View newExerciseView, int viewPosition, LinearLayout currentExpandedLayout, EditText exerciseEdittext)
        {
            ImageButton deleteExerciseBtn = newExerciseView.FindViewById<ImageButton>(Resource.Id.delete_exercise_btn);
            if (!deleteExerciseBtn.HasOnClickListeners)
            {
                deleteExerciseBtn.Click += delegate (object sender, EventArgs e)
                {
                    currentExpandedLayout.RemoveViewAt(viewPosition);
                    HideKeyboard(exerciseEdittext);
                };
            };

        }

        public void OnAddExerciseEvent(EditText exerciseEdittext, int currentPosition,
            LinearLayout currentExpandedLayout, LinearLayout.LayoutParams ll, int index, CardView rootCardView)
        {
            HideKeyboard(exerciseEdittext);

            // format exercises list
            if (currentPosition > -1 && currentPosition < data.Count)
                data[currentPosition].exercises += (data[currentPosition].exercises != null ? "\n" : "") + exerciseEdittext.Text.ToString();

            // replace edit view with normal exercise view 
            currentExpandedLayout.RemoveViewAt(index);
            View exerciseView = LayoutInflater.From(context).Inflate(Resource.Layout.ListItemExercise, null);
            exerciseView.FindViewById<TextView>(Resource.Id.exercise_name).Text = exerciseEdittext.Text.ToString();
            ll.LeftMargin = 24; ll.RightMargin = 24;
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
                EditText target = setView.FindViewById<EditText>(Resource.Id.target_weight);
                EditText final = setView.FindViewById<EditText>(Resource.Id.final_weight);
                setKeyboard(target);
                setKeyboard(final);

            }
            else if (senderId == Resource.Id.root_exercise_item)
            {
                if (((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility == ViewStates.Gone)
                {
                    // expand
                    ((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility = ViewStates.Visible;
                    ((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Visible;
                    ((LinearLayout)sender).FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Visible;

                    //set keyboard for both EditTexts where user enters weight
                    EditText target = ((LinearLayout)sender).FindViewById<EditText>(Resource.Id.target_weight);
                    EditText final = ((LinearLayout)sender).FindViewById<EditText>(Resource.Id.final_weight);
                    setKeyboard(target);
                    setKeyboard(final);
                }
                else
                {
                    // collapse
                    LinearLayout setsLayout = ((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set);
                    setsLayout.Visibility = ViewStates.Gone;
                    ((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Gone;
                    ((LinearLayout)sender).FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Gone;
                    var im = ((InputMethodManager)context.GetSystemService(Android.Content.Context.InputMethodService));

                    if (!((LinearLayout)sender).FindViewById<EditText>(Resource.Id.target_weight).HasFocus)
                    {
                        im.HideSoftInputFromWindow(((LinearLayout)sender).FindViewById<EditText>(Resource.Id.target_weight).WindowToken, 0);
                    };

                    CleanUpEmptySets(setsLayout, 1);
                }
            }

        }

        private void CleanUpEmptySets(LinearLayout parent, int maxCount)
        {
            if (maxCount <= parent.ChildCount)
            {
                for (int i = 1; i < parent.ChildCount; i++)
                {
                    View setView = parent.GetChildAt(i);
                    if (setView.FindViewById<EditText>(Resource.Id.target_sets).Text.Length == 0 &&
                        setView.FindViewById<EditText>(Resource.Id.target_weight).Text.Length == 0 &&
                        setView.FindViewById<EditText>(Resource.Id.final_sets).Text.Length == 0 &&
                        setView.FindViewById<EditText>(Resource.Id.final_weight).Text.Length == 0)
                    {
                        parent.RemoveViewAt(i);
                        maxCount--;
                        break;
                    } else if (parent.GetChildAt(0).FindViewById<EditText>(Resource.Id.target_sets).Text.Length == 0 &&
                        parent.GetChildAt(0).FindViewById<EditText>(Resource.Id.target_weight).Text.Length == 0 &&
                        parent.GetChildAt(0).FindViewById<EditText>(Resource.Id.final_sets).Text.Length == 0 &&
                        parent.GetChildAt(0).FindViewById<EditText>(Resource.Id.final_weight).Text.Length == 0)
                    {
                        parent.GetChildAt(0).FindViewById<EditText>(Resource.Id.target_sets).Text = setView.FindViewById<EditText>(Resource.Id.target_sets).Text;
                        parent.GetChildAt(0).FindViewById<EditText>(Resource.Id.target_weight).Text = setView.FindViewById<EditText>(Resource.Id.target_weight).Text;
                        parent.GetChildAt(0).FindViewById<EditText>(Resource.Id.final_sets).Text = setView.FindViewById<EditText>(Resource.Id.final_sets).Text;
                        parent.GetChildAt(0).FindViewById<EditText>(Resource.Id.final_weight).Text = setView.FindViewById<EditText>(Resource.Id.final_weight).Text;
                        parent.RemoveViewAt(i);
                        maxCount--;
                        break;
                    }
                }
                CleanUpEmptySets(parent, maxCount+1);
            }
        }

        private void setKeyboard(EditText target)
        {
            if (target != null)
            {
                //.setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
                target.SetRawInputType(InputTypes.ClassText);
                target.SetTextIsSelectable(true);
                //InputConnection ic = (InputConnection) target.OnCreateInputConnection(new EditorInfo());
                //weight_keyboard.setInputConnection(ic);
                target.Touch += delegate
                {
                    keyboard.setCurrentEditText(target);
                    InputMethodManager imm = (InputMethodManager)context.GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(root_view.WindowToken, 0);
                    keyboard.Visibility = ViewStates.Visible;
                };
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