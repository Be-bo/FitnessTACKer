using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace FitnessTACKer
{
    public class CalendarFragment : Fragment
    {
        

        public static CalendarFragment NewInstance()
        {
            var frag2 = new CalendarFragment { Arguments = new Bundle() };
            return frag2;
        }
        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState); 
            View view = inflater.Inflate(Resource.Layout.CalendarFragment, null);
            CalendarView calendar = view.FindViewById<CalendarView>(Resource.Id.calendarView1);

            return view;
        }
    }
}
