using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.Design.Widget;

namespace FitnessTACKer
{
    [Activity(Label = "FitnessTACKer", MainLauncher = true, Icon = "@mipmap/icon",Theme ="@style/Theme.AppCompat.DayNight")]
    public class MainActivity : AppCompatActivity
    {
        BottomNavigationView bottomNavigation;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            LoadFragment(Resource.Id.action_home);

        }
        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.action_home:
                    fragment = HomeFragment.NewInstance();
                    break;
                case Resource.Id.action_calender:
                    fragment = CalendarFragment.NewInstance();
                    break;
                case Resource.Id.action_workout:
                    fragment = WorkoutFragment.NewInstance();
                    break;
                case Resource.Id.action_settings:
                    fragment = SettingsFragment.NewInstance();
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }
    }
}

