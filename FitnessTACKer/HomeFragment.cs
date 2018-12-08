using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using FitnessTACKer.Adapter;
using System;
using Android.Content;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using Android.Support.V4.Widget;
using Android.Widget;
using System.Text.RegularExpressions;
using Android.Views.InputMethods;

namespace FitnessTACKer
{
    public class HomeFragment : Fragment
    {
        private List<WorkoutItem> RecyclerViewData;
        private WorkoutAdapter AdapterHome;
        private RecyclerView RecyclerViewWorkouts;
        private View root;
        private int Today;
        private TextView ToolBarDate;
        private int IncrementDays;
        private TextView NoScheduledWorkoutsTv;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static HomeFragment NewInstance()
        {
            var frag1 = new HomeFragment { Arguments = new Bundle() };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            root = inflater.Inflate(Resource.Layout.HomeFragment, null);
            NoScheduledWorkoutsTv = root.FindViewById<TextView>(Resource.Id.no_scheduled_workouts);

            IncrementDays = 0;

            string currentDate = DateTime.UtcNow.Date.ToString("dddd, d");
            ToolBarDate = root.FindViewById<TextView>(Resource.Id.todays_date);
            Today = Int32.Parse(Regex.Match(currentDate, @"\d+").Value);
            ToolBarDate.Text = currentDate + GetDaySuffix(Today);

            RecyclerViewData = new List<WorkoutItem>();

            RecyclerViewWorkouts = root.FindViewById<RecyclerView>(Resource.Id.recyclerview_home);
            AdapterHome = new WorkoutAdapter(root.Context, RecyclerViewData);
            RecyclerViewWorkouts.SetAdapter(AdapterHome);
            RecyclerViewWorkouts.SetLayoutManager(new LinearLayoutManager(Context));
            RecyclerViewWorkouts.NestedScrollingEnabled = false;
            AdapterHome.ItemClick += OnItemClick;

            RetrieveWorkouts();
            
            SetupClickListeners(root);

            return root;
        }

        private string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        private void SetupClickListeners(View root)
        {
            // add workout button on click
            root.FindViewById<Button>(Resource.Id.add_workout_btn).Click += delegate
            {
                NoScheduledWorkoutsTv.Visibility = ViewStates.Gone;

                // collapse previously expanded item
                for (int i = 0; i < RecyclerViewData.Count; i++)
                {
                    if (RecyclerViewData[i].expanded)
                    {
                        RecyclerViewData[i].expanded = false;
                        AdapterHome.NotifyItemChanged(i);
                    }
                }
                RecyclerViewData.Add(new WorkoutItem() { editModeNewWorkout = true });
                AdapterHome.NotifyDataSetChanged();
            };

            // left/right arrows in actionbar
            root.FindViewById<ImageButton>(Resource.Id.left_arrow_btn).Click += delegate (object sender, EventArgs e){ ToolBarArrowsClickListener(sender, e, 0);};
            root.FindViewById<ImageButton>(Resource.Id.right_arrow_btn).Click += delegate (object sender, EventArgs e) { ToolBarArrowsClickListener(sender, e, 1); };

        }

        private void ToolBarArrowsClickListener(object sender, EventArgs e, int pos)
        {
            IncrementDays += (pos == 0) ? -1 : 1;
            DateTime selectedDate = DateTime.UtcNow.AddDays(IncrementDays);
            string selectedDateStr = selectedDate.ToString("dddd, d");
            ToolBarDate.Text = selectedDateStr + GetDaySuffix(Int32.Parse(Regex.Match(selectedDateStr, @"\d+").Value)); ;

            if (IncrementDays == 0) // current date
            {
                NoScheduledWorkoutsTv.Visibility = ViewStates.Gone;
                RetrieveWorkouts();
            } else
            {
                RecyclerViewData.Clear();
                AdapterHome.NotifyDataSetChanged();
                NoScheduledWorkoutsTv.Visibility = ViewStates.Visible; 
            }
        }

        public void RetrieveWorkouts()
        {
            RecyclerViewData.Clear();
            AdapterHome.NotifyDataSetChanged();
            RecyclerViewData.Add(new WorkoutItem() { title = "Friday workout", exercises= "Weighted Pull Ups\nBarbell Full Squat\nSingle-Arm Linear Jammer\nLandmine 180's", expanded=false, editModeNewWorkout=false});
            AdapterHome.NotifyDataSetChanged();
            RecyclerViewData.Add(new WorkoutItem() { title = "wednesday prancercise", exercises = "Bench Press\nDeadlift with Chains\nBox Squat\nKneeling Squat", expanded = false, editModeNewWorkout = false});
            AdapterHome.NotifyDataSetChanged();
        }

        private void OnItemClick(object sender, int position)
        {
            // collapse previously expanded item
            for (int i= 0; i < RecyclerViewData.Count; i++)
            {
                if (RecyclerViewData[i].expanded && i != position)
                {
                    RecyclerViewData[i].expanded = false;
                    AdapterHome.NotifyItemChanged(i);
                }
            }

            // expand selected item
            RecyclerViewData[position].expanded = !RecyclerViewData[position].expanded;
            HideKeyboard();
            AdapterHome.NotifyItemChanged(position);
            RecyclerViewWorkouts.SmoothScrollToPosition(position);
        }

        public void HideKeyboard()
        {
            var imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(root.WindowToken, HideSoftInputFlags.NotAlways);
        }
    }
}
