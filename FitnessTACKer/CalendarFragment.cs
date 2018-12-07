using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System;

namespace FitnessTACKer
{
    public class CalendarFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            

        }

        public static CalendarFragment NewInstance()
        {
            var frag2 = new CalendarFragment { Arguments = new Bundle() };
            return frag2;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.CalendarFragment, null);

            view.FindViewById<CalendarView>(Resource.Id.calendarView1).DateChange += CalendarOnDateChange;

            return view;
        }

        private void CalendarOnDateChange(object sender, CalendarView.DateChangeEventArgs args)
        {
            string dateSelected = new DateTime(args.Year, args.Month, args.DayOfMonth).ToString("yyyy/MM/dd");
            Toast.MakeText(Context, dateSelected, ToastLength.Long).Show();
        }
    }
}
