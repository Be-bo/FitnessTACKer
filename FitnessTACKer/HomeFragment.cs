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
        private View root;

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

            root.FindViewById<TextView>(Resource.Id.todays_date).Text = DateTime.UtcNow.Date.ToString("dddd, d");

            RecyclerViewData = new List<WorkoutItem>();

            RecyclerView recyclerView = root.FindViewById<RecyclerView>(Resource.Id.recyclerview_home);
            AdapterHome = new WorkoutAdapter(root.Context, RecyclerViewData);
            recyclerView.SetAdapter(AdapterHome);
            recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            recyclerView.NestedScrollingEnabled = false;
            AdapterHome.ItemClick += OnItemClick;

            RetrieveWorkouts();

            return root;
        }

        public void RetrieveWorkouts()
        {
            RecyclerViewData.Add(new WorkoutItem() { title = "Friday workout", exercises= "Weighted Pull Ups\nBarbell Full Squat\nSingle-Arm Linear Jammer\nLandmine 180's", expanded=false});
            AdapterHome.NotifyDataSetChanged();
            RecyclerViewData.Add(new WorkoutItem() { title = "wednesday prancercise", exercises = "Bench Press\nDeadlift with Chains\nBox Squat\nKneeling Squat", expanded = false});
            AdapterHome.NotifyDataSetChanged();
        }

        private void OnItemClick(object sender, int position)
        {
            int senderId = -1;
            try
            {
                senderId = ((Button)sender).Id;
            } catch (Exception e)
            {

            }
            if (senderId!=-1 && senderId == Resource.Id.add_exercise_btn)
            {
                RecyclerViewData[position].exercises += "\nExercise name";
            } else
            {
                RecyclerViewData[position].expanded = !RecyclerViewData[position].expanded;

                var imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(root.WindowToken, HideSoftInputFlags.NotAlways);
            }
            
            AdapterHome.NotifyItemChanged(position);
        }
    }
}
