using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using FitnessTACKer.Adapter;
using System;


namespace FitnessTACKer
{
    public class CalendarFragment : Fragment
    {
        CalendarView calendar;
        RecyclerView recyclerView;
        private List<WorkoutItem> RecyclerViewData;
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
            AdapterHome = new WorkoutAdapter(RecyclerViewData);
            recyclerView.SetAdapter(AdapterHome);
            recyclerView.SetLayoutManager(new LinearLayoutManager(Context));

            AdapterHome.ItemClick += OnItemClick;
            calendar.DateChange += YourCalendarView_DateChange;
            return view;

        }

        void YourCalendarView_DateChange(object sender, CalendarView.DateChangeEventArgs e)
        {
            System.Console.WriteLine("Date : {0} Month : {1} Year : {2}", e.DayOfMonth, e.Month, e.Year);
            HideWorkout();
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
