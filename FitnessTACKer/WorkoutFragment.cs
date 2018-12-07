using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using System;
using Android.Content;
using Android.Support.V7.Widget;
using SearchView = Android.Support.V7.Widget.SearchView;
using Android.Widget;
using Android.Runtime;
using Android.Views.InputMethods;


namespace FitnessTACKer
{
    public class WorkoutFragment : Fragment
    {

        RecyclerView rv;
        SearchView sv;
        View rootView;
        private MyAdapter adapter;

        private JavaList<WorkoutItem> MyWorkoutList;

       // public override void OnCreate(Bundle savedInstanceState)
      //  {
      //      base.OnCreate(savedInstanceState);

            // Create your fragment here
    //    }

        public static WorkoutFragment NewInstance()
        {
            var frag3 = new WorkoutFragment { Arguments = new Bundle() };
            return frag3;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            rootView = inflater.Inflate(Resource.Layout.WorkoutFragment, container, false);
            MyWorkoutList = new JavaList<WorkoutItem>();
            //init 
            sv = rootView.FindViewById<SearchView>(Resource.Id.searchView1);
            rv = rootView.FindViewById<RecyclerView>(Resource.Id.mRecyclerID);
            adapter = new MyAdapter(MyWorkoutList);
            rv.SetAdapter(adapter);
            rv.SetLayoutManager(new LinearLayoutManager(Context));
            rv.SetItemAnimator(new DefaultItemAnimator());

            adapter.ItemClick += OnItemClick;
            RetrieveWorkouts();

            sv.QueryTextChange += sv_QueryTextChange;
            return rootView;
        }

        private void OnItemClick(object sender, int position)
        {
            MyWorkoutList[position].expanded = !MyWorkoutList[position].expanded;

            //var imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            //imm.HideSoftInputFromWindow(rootView.WindowToken, HideSoftInputFlags.NotAlways);

            adapter.NotifyItemChanged(position);
        }


        void sv_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            adapter.Filter.InvokeFilter(e.NewText);
        }

        public void RetrieveWorkouts()
        {
            MyWorkoutList.Add(new WorkoutItem() { title = "Friday workout", exercises = "Weighted Pull Ups\nBarbell Full Squat\nSingle-Arm Linear Jammer\nLandmine 180's", expanded = false });
            adapter.NotifyDataSetChanged();
            MyWorkoutList.Add(new WorkoutItem() { title = "wednesday prancercise", exercises = "Bench Press\nDeadlift with Chains\nBox Squat\nKneeling Squat", expanded = false });
            adapter.NotifyDataSetChanged();
        }

    }
}
