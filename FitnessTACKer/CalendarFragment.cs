using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using FitnessTACKer.Adapter;
using System;

//Days that have exercises scheduled: 12, 15, 23 day of any month
//Schedule a workout on 10 of any month, name it "Added Workout" and name one exercise "Added Exercise".
//Once this workout is scheduled, it will be listed every time you click on 10.

namespace FitnessTACKer
{
    public class CalendarFragment : Fragment
    {
        CalendarView calendar;
        RecyclerView recyclerView;
        private List<WorkoutItem> RecyclerViewData;
        
        int day = 0;
        int tempDay = 0;
        bool addWorkout = false;
        private WorkoutAdapter AdapterHome;

        public static CalendarFragment NewInstance()
        {
            var frag2 = new CalendarFragment { Arguments = new Bundle() };
            return frag2;
        }
        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState); 
            View view = inflater.Inflate(Resource.Layout.CalendarFragment, null);

            calendar = view.FindViewById<CalendarView>(Resource.Id.calendarView1);
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            RecyclerViewData = new List<WorkoutItem>();
          

            AdapterHome = new WorkoutAdapter(view.Context, RecyclerViewData);
            

            recyclerView.SetAdapter(AdapterHome);
            recyclerView.SetLayoutManager(new LinearLayoutManager(Context));

            AdapterHome.ItemClick += OnItemClick;
            calendar.DateChange += YourCalendarView_DateChange;
            SetupClickListeners(view);

            return view;
        }

        private void SetupClickListeners(View root)
        {
            root.FindViewById<Button>(Resource.Id.add_workout_btn).Click += delegate
            {
                // collapse previously expanded item
                for (int i = 0; i < RecyclerViewData.Count; i++)
                {
                    if (RecyclerViewData[i].expanded)
                    {
                        RecyclerViewData[i].expanded = false;
                        AdapterHome.NotifyItemChanged(i);
                    }
                }
                // add workout button on click
                addWorkout = true;
                RecyclerViewData.Add(new WorkoutItem() { editMode = true });
                AdapterHome.NotifyDataSetChanged();

            };
   
    }

        void YourCalendarView_DateChange(object sender, CalendarView.DateChangeEventArgs e)
        {
            System.Console.WriteLine("Date : {0} Month : {1} Year : {2}", e.DayOfMonth, e.Month, e.Year);
            HideWorkout();

            if (addWorkout && e.DayOfMonth == 10)
            {
                ShowWorkout0();
            }
            if (e.DayOfMonth == 12)
            {
                ShowWorkout1();
            }
            else if (e.DayOfMonth == 15)
            {
                ShowWorkout2();
            }
            else if (e.DayOfMonth == 23)
            {
                ShowWorkout1();
                ShowWorkout2();
            }

        }

        public void ShowWorkout0()
        {
            RecyclerViewData.Add(new WorkoutItem() { title = "Added Workout", exercises = "Added Exercise", expanded = false });
            AdapterHome.NotifyDataSetChanged();
        }

        public void ShowWorkout1()
        {
            RecyclerViewData.Add(new WorkoutItem() { title = "Jazzercise", exercises = "Jazz and cise", expanded = false });
            AdapterHome.NotifyDataSetChanged();
        }

        public void ShowWorkout2()
        {
            RecyclerViewData.Add(new WorkoutItem() { title = "Aquacise", exercises = "Move them bones", expanded = false });
            AdapterHome.NotifyDataSetChanged();
        }


        public void HideWorkout()
        {
            RecyclerViewData.Clear();
            AdapterHome.NotifyDataSetChanged();
        }

        private void OnItemClick(object sender, int position)
        {
            if (RecyclerViewData[position].expanded)
            {
                RecyclerViewData[position].expanded = false;
            }
            else
            {
                RecyclerViewData[position].expanded = true;
            }
            AdapterHome.NotifyItemChanged(position);
        }
    }
}
