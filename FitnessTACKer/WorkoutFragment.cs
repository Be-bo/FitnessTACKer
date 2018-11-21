using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace FitnessTACKer
{
    public class WorkoutFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static WorkoutFragment NewInstance()
        {
            var frag3 = new WorkoutFragment { Arguments = new Bundle() };
            return frag3;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.WorkoutFragment, null);
        }
    }
}
