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
        bool SimulationRunning = false;
        int maxSims = 10000000;
        string lastVal;

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
            MessageHelper.Subscribe(this, "Simulation", SimulationUpdate);

            SimulationsToRun.AfterTextChanged += (sender, e) =>
             {
                 int value;
                 int.TryParse(SimulationsToRun.Text, out value);

                 if (value > maxSims)
                 {
                     value = maxSims;
                     SimulationsToRun.Text = value.ToString();
                 }

                 if (value <= 0)
                 {
                     value = 1;
                     SimulationsToRun.Text = value.ToString();
                 }

                 lastVal = SimulationsToRun.Text;
             };

            StartSimButton.Click += (sender, e) =>
            {
                if (SimulationRunning) return;
                DependencyService.Get<IForceKeyboardDismissalService>().DismissKeyboard();
                var intent = new Intent(this, typeof(LongRunningTaskService));
                intent.PutExtra("Rounds", SimulationsToRun.Text);
                intent.PutExtra("Swap", Swap.Checked);
                intent.PutExtra("Hold", Hold.Checked);
                intent.PutExtra("Random", Random.Checked);
                StartService(intent);
            };
        }

        private void SimulationUpdate(string sender, string message)
        {
            switch (message)
            {
                case "started":
                    SimulationRunning = true;
                    break;
                case "completed":
                    SimulationRunning = false;
                    break;
                default:
                    break;
            }
        }
    }
}