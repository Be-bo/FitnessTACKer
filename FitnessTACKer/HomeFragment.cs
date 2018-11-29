using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using FitnessTACKer.Adapter;
using System;
using Android.Content;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace FitnessTACKer
{
    public class HomeFragment : Fragment
    {
        private List<WorkoutItem> RecyclerViewData;
        private WorkoutAdapter AdapterHome;

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

            RecyclerViewData = new List<WorkoutItem>();

            RecyclerView recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerview_home);
            AdapterHome = new WorkoutAdapter(view.Context, RecyclerViewData);
            recyclerView.SetAdapter(AdapterHome);
            recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            AdapterHome.ItemClick += OnItemClick;

            RetrieveWorkouts();

            return view;
        }

        public void RetrieveWorkouts()
        {
            RecyclerViewData.Add(new WorkoutItem() { title = "Friday workout", exercises= "Weighted Pull Ups\nBarbell Full Squat\nSingle-Arm Linear Jammer\nLandmine 180's", expanded=false });
            AdapterHome.NotifyDataSetChanged();
            RecyclerViewData.Add(new WorkoutItem() { title = "wednesday prancercise", exercises = "Bench Press\nDeadlift with Chains\nBox Squat\nKneeling Squat", expanded = false });
            AdapterHome.NotifyDataSetChanged();
        }

        private void OnItemClick(object sender, int position)
        {
            if (RecyclerViewData[position].expanded)
            {
                RecyclerViewData[position].expanded = false;
            } else
            {
                RecyclerViewData[position].expanded = true;
            }
            AdapterHome.NotifyItemChanged(position);
        }
    }
}
