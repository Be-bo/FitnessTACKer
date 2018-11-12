using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.Design.Widget;

namespace FitnessTACKer
{
    [Activity(Label = "FitnessTACKer", MainLauncher = true, Icon = "@mipmap/icon",Theme ="@style/Theme.AppCompat")]
    public class MainActivity : AppCompatActivity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { button.Text = $"{count++} lifts!"; };

            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += (s, e) => {

                switch (e.Item.ItemId) {
                    case Resource.Id.action_calender:
                        Toast.MakeText(this, "Calender Clicked", ToastLength.Short).Show();
                        break;
                    case Resource.Id.action_home:
                        Toast.MakeText(this, "Home Clicked", ToastLength.Short).Show();
                        break;
                    case Resource.Id.action_workout:
                        Toast.MakeText(this, "Workout Clicked", ToastLength.Short).Show();
                        break;
                    case Resource.Id.action_settings:
                        Toast.MakeText(this, "Settings Clicked", ToastLength.Short).Show();
                        break;
                }
            };
        }
    }
}

