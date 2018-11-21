using Android.OS;
using Android.Support.V4.App;

using Android.Views;



namespace FitnessTACKer
{
    public class SettingsFragment : Fragment {//PreferenceFragment    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Load the preferences from an XML resource
            //AddPreferencesFromResource(Resource.Xml.settings_codec);
            //ISharedPreferences d = PreferenceManager.GetDefaultSharedPreferences(this);


            // Create your fragment here
        }

        public static SettingsFragment NewInstance()
        {
            var frag4 = new SettingsFragment { Arguments = new Bundle() };
            return frag4;
        }


       public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.SettingsFragment, null);
       }
    }
}
