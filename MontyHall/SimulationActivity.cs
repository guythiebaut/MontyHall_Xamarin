using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;
using Xamarin.Forms;

namespace MontyHall
{
    [Activity]
    public class SimulationActivity : AppCompatActivity
    {
        Android.Widget.Button StartSimButton;
        TextView SimulationRoundsRun;
        TextView SimulationsWon;
        EditText SimulationsToRun;
        RadioButton Swap;
        RadioButton Hold;
        RadioButton Random;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_simulation);
            WireUpControls();
        }

        private void WireUpControls()
        {
            SimulationRoundsRun = FindViewById<TextView>(Resource.Id.SimRoundsRun);
            SimulationsWon = FindViewById<TextView>(Resource.Id.SimsWon);
            StartSimButton = FindViewById<Android.Widget.Button>(Resource.Id.startSim);
            SimulationsToRun = FindViewById<EditText>(Resource.Id.SimsToRun);
            Swap = FindViewById<RadioButton>(Resource.Id.radioSwap);
            Hold = FindViewById<RadioButton>(Resource.Id.radioHold);
            Random = FindViewById<RadioButton>(Resource.Id.radioRandom);

            MessageHelper.Subscribe(this, "RoundNumber", SimulationRoundsRun);
            MessageHelper.Subscribe(this, "RoundsWon", SimulationsWon);

            StartSimButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                intent.PutExtra("Rounds", SimulationsToRun.Text);
                intent.PutExtra("Swap", Swap.Checked);
                intent.PutExtra("Hold", Hold.Checked);
                intent.PutExtra("Random", Random.Checked);
                StartService(intent);
            };
        }
    }
}