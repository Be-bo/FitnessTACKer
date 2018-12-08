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
                ToggleEditModeExisting(false, workoutHolder, position);
                ToggleEditModeNewWorkout(true, workoutHolder, position);
            }
            else if (data[position].editModeExisting)
            {
                // ToggleEditModeNewWorkout(false, workoutHolder, position);
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
                //workoutHolder.SaveChangesBtn.Visibility = ViewStates.Visible;

                workoutHolder.LayoutTitleAndMenu.Visibility = ViewStates.Gone;
                workoutHolder.Exercises.Visibility = ViewStates.Gone;
                workoutHolder.AddExerciseBtn.Visibility = ViewStates.Gone;

                workoutHolder.ExpandedLayout.Visibility = ViewStates.Visible;
                workoutHolder.ExpandedLayout.RemoveAllViews();

                workoutHolder.NewWorkoutName.Text = data[position].title;
                workoutHolder.NewWorkoutName.RequestFocus();

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

                bool itemDeleted = false;
                if (exercises != null)
                {
                    for (int i = 0; i < exercises.Length; i++)
                    {
                        View editModeExerciseView = LayoutInflater.From(context).Inflate(Resource.Layout.ListItemExerciseEditMode, null);
                        editModeExerciseView.FindViewById<EditText>(Resource.Id.exercise_name_edittext).Text = exercises[i];
                        ImageButton deleteExerciseBtn = editModeExerciseView.FindViewById<ImageButton>(Resource.Id.delete_exercise_btn);
                        if (!deleteExerciseBtn.HasOnClickListeners)
                        {
                            deleteExerciseBtn.Click += delegate (object sender, EventArgs e)
                            {
                                if (i < workoutHolder.ExpandedLayout.ChildCount && !itemDeleted)
                                {
                                    workoutHolder.ExpandedLayout.RemoveViewAt(i);
                                }
                                else if (i < workoutHolder.ExpandedLayout.ChildCount && itemDeleted && (i - 1) > -1)
                                {
                                    workoutHolder.ExpandedLayout.RemoveViewAt(i - 1);
                                }
                                itemDeleted = true;
                            };
                        }

                        editModeExerciseView.LayoutParameters = ll;
                        workoutHolder.ExpandedLayout.AddView(editModeExerciseView);
                    }

                }

                if (!workoutHolder.DeleteWorkoutBtn.HasOnClickListeners)
                {
                    workoutHolder.DeleteWorkoutBtn.Click += delegate (object s, EventArgs e2) { ShowDialogDeleteWorkout(position); };
                }

                //if (!workoutHolder.SaveChangesBtn.HasOnClickListeners)
                //{
                //    workoutHolder.SaveChangesBtn.Click += delegate (object saveChangesSender, EventArgs eSaveChanges) {
                //        SaveChangesOnClick(saveChangesSender, eSaveChanges);
                //    };
                //}

            }
            else
            {
                workoutHolder.LayoutTitleAndMenu.Visibility = ViewStates.Visible;
                workoutHolder.Exercises.Visibility = ViewStates.Visible;
                workoutHolder.AddExerciseBtn.Visibility = ViewStates.Visible;

                //workoutHolder.SaveChangesBtn.Visibility = ViewStates.Gone;
                workoutHolder.EditModeRoot.Visibility = ViewStates.Gone;
                workoutHolder.ExpandedLayout.Visibility = ViewStates.Gone;
            }
        }

        public void SaveChangesOnClick(object sender, EventArgs e)
        {

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
                            NotifyItemRemoved(position);
                            HideKeyboard(workoutHolder.NewWorkoutName);
                        }
                    };
                }

            }
            else
            {
                workoutHolder.EditModeRoot.Visibility = ViewStates.Gone;
                workoutHolder.RootWorkoutLayout.Visibility = ViewStates.Visible;
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
                NotifyItemRemoved(adapterPosition);
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

            EditText exerciseEdittext = newExerciseView.FindViewById<EditText>(Resource.Id.exercise_name_edittext);
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
                    ((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set).Visibility = ViewStates.Gone;
                    ((LinearLayout)sender).FindViewById<LinearLayout>(Resource.Id.layout_set_title).Visibility = ViewStates.Gone;
                    ((LinearLayout)sender).FindViewById<Button>(Resource.Id.add_set_btn).Visibility = ViewStates.Gone;
                    var im = ((InputMethodManager)context.GetSystemService(Android.Content.Context.InputMethodService));

                    if (!((LinearLayout)sender).FindViewById<EditText>(Resource.Id.target_weight).HasFocus)
                    {
                        im.HideSoftInputFromWindow(((LinearLayout)sender).FindViewById<EditText>(Resource.Id.target_weight).WindowToken, 0);
                    };
                }
            }

        }

        private void setKeyboard(EditText target)
        {

            //.setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
            target.SetRawInputType(InputTypes.ClassText);
            target.SetTextIsSelectable(true);
            //InputConnection ic = (InputConnection) target.OnCreateInputConnection(new EditorInfo());
            //weight_keyboard.setInputConnection(ic);
            target.Touch += delegate
            {
                target.SetBackgroundResource(Resource.Drawable.weight_outline);
                keyboard.setCurrentEditText(target);
                InputMethodManager imm = (InputMethodManager)context.GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(root_view.WindowToken, 0);
                keyboard.Visibility = ViewStates.Visible;
            };
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