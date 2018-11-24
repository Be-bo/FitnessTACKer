using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Preferences;
using Android.Views;



namespace FitnessTACKer
{
    public class SettingsFragment : PreferenceFragmentCompat {

        public static SettingsFragment NewInstance()
        {
            var frag4 = new SettingsFragment { Arguments = new Bundle() };
            return frag4;
        }

        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            AddPreferencesFromResource(Resource.Xml.settings_codec);
        }

    }
}
