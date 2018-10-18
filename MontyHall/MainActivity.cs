using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Android.Content;

namespace MontyHall
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Button playGameButton = FindViewById<Button>(Resource.Id.PlayGame);
            playGameButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(GameActivity));
                StartActivity(intent);
            };

            Button simulationButton = FindViewById<Button>(Resource.Id.RunSimulation);
            simulationButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(SimulationActivity));
                StartActivity(intent);
            };
        }
    }
}