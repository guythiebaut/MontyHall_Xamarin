using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_simulation);
            WireUpControls();
        }

        private void WireUpControls()
        {
            SimulationRoundsRun = FindViewById<TextView>(Resource.Id.SimulationRoundsRun);
            SimulationsWon = FindViewById<TextView>(Resource.Id.SimulationsWon);
            MessageHelper.Subscribe(this, "RoundNumber", SimulationRoundsRun);
            MessageHelper.Subscribe(this, "RoundsWon", SimulationsWon);

            StartSimButton = FindViewById<Android.Widget.Button>(Resource.Id.startSim);
            SimulationsToRun = FindViewById<EditText>(Resource.Id.SimulationsToRun);

            StartSimButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                intent.PutExtra("Rounds", SimulationsToRun.Text);
                StartService(intent);
            };
        }
    }
}