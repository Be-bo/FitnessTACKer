using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using FitnessTACKer.Adapter;
using System;
using System.Collections.Generic;

namespace FitnessTACKer
{
    public class HomeFragment : Fragment
    {
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

            View view = inflater.Inflate(Resource.Layout.HomeFragment, null);
            ConfigureRecyclerView(view.FindViewById<RecyclerView>(Resource.Id.recyclerview_home),
               new WorkoutAdapter(RetrieveWorkouts()));

            return view;
        }

        public List<WorkoutItem> RetrieveWorkouts()
        {
            return new List<WorkoutItem>() {
                new WorkoutItem() { title = "leg workout" },
                new WorkoutItem() { title = "some workout" }
            };
        }

        void ConfigureRecyclerView(RecyclerView recyclerView, RecyclerView.Adapter adapter)
        {
            try
            {
                recyclerView.SetAdapter(adapter);
                recyclerView.SetLayoutManager(new LinearLayoutManager(Context));

            }
            catch (NullReferenceException e)
            {

            }

        }
    }
}
