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
        Persist persist = new Persist();
        Android.Widget.Button StartSimButton;
        TextView SimulationRoundsRun;
        TextView SimulationsWon;
        EditText SimulationsToRun;
        RadioGroup radioGroupStrategy;
        RadioButton Swap;
        RadioButton Hold;
        RadioButton Random;
        bool SimulationRunning = false;
        int maxSims = 10000000;
        int lastSimulationRuns;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_simulation);
            WireUpControls();
            ApplySavedSettings();
        }

        private void WireUpControls()
        {
            SimulationRoundsRun = FindViewById<TextView>(Resource.Id.SimRoundsRun);
            SimulationsWon = FindViewById<TextView>(Resource.Id.SimsWon);
            StartSimButton = FindViewById<Android.Widget.Button>(Resource.Id.startSim);
            SimulationsToRun = FindViewById<EditText>(Resource.Id.SimsToRun);
            radioGroupStrategy = FindViewById<RadioGroup>(Resource.Id.radioGroupStrategy);
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
                     value = lastSimulationRuns;
                     SimulationsToRun.Text = value.ToString();
                 }

                 if (value <= 0)
                 {
                     value = 1;
                     SimulationsToRun.Text = value.ToString();
                 }

                 lastSimulationRuns = value;
                 persist.AddPreference("SimulationRuns", lastSimulationRuns.ToString());
             };

            radioGroupStrategy.CheckedChange += (sender, e) =>
            {
                persist.AddPreference("Strategy", string.Empty);
                persist.AddPreference("Strategy_Swap", Swap.Checked.ToString());
                persist.AddPreference("Strategy_Hold", Hold.Checked.ToString());
                persist.AddPreference("Strategy_Random", Random.Checked.ToString());
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

        private void ApplySavedSettings()
        {
            if (persist.Contains("SimulationRuns"))
            {
                int.TryParse((string)persist.GetPreference("SimulationRuns"), out lastSimulationRuns);
                SimulationsToRun.Text = lastSimulationRuns.ToString();
            }

            if (persist.Contains("Strategy"))
            {
                Swap.Checked = Convert.ToBoolean(persist.GetPreference("Strategy_Swap"));
                Hold.Checked = Convert.ToBoolean(persist.GetPreference("Strategy_Hold"));
                Random.Checked = Convert.ToBoolean(persist.GetPreference("Strategy_Random"));
            }
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