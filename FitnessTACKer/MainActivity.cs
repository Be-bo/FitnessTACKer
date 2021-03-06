﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using System;
using FitnessTACKer.Adapter;
using Android.Views;

namespace FitnessTACKer
{
    [Activity(Label = "FitnessTACKer", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : AppCompatActivity
    {

        Keyboard keyboard;

        BottomNavigationView bottomNavigation;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Window.SetSoftInputMode(SoftInput.AdjustPan);

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            
            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            bottomNavigation.SelectedItemId = Resource.Id.action_home;

            LoadFragment(Resource.Id.action_home);

        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        void LoadFragment(int id)
        {
            //creating, linking and putting the keyboard into a bundle
            keyboard = FindViewById<Keyboard>(Resource.Id.keyboard_weights);
            Bundle mybundle = new Bundle();
            mybundle.PutSerializable("keyboard", keyboard);

            Android.Support.V4.App.Fragment fragment = null;
            //int title = 0;
            switch (id)
            {
                case Resource.Id.action_home:
                    fragment = HomeFragment.NewInstance();
                    fragment.Arguments = mybundle;
                    //title = Resource.String.home;
                    break;
                case Resource.Id.action_calender:
                    fragment = CalendarFragment.NewInstance();
                    //title = Resource.String.calendar;
                    break;
                case Resource.Id.action_workout:
                    fragment = WorkoutFragment.NewInstance();
                    //title = Resource.String.workout;
                    break;
                case Resource.Id.action_settings:
                    fragment = SettingsFragment.NewInstance();
                    //title = Resource.String.settings;
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();

            //if (title!=0) SupportActionBar.SetTitle(title);
        }

    }
}

