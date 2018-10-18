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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_simulation);
            WireUpControls();
        }

        private void WireUpControls()
        {
            StartSimButton = FindViewById<Android.Widget.Button>(Resource.Id.startSim);
            SimulationRoundsRun = FindViewById<TextView>(Resource.Id.SimulationRoundsRun);

            MessagingCenter.Subscribe<string>(this, "msg", (message) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SimulationRoundsRun.Text = message;
                });
            });

            StartSimButton.Click += (sender, e) =>
            {
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    MessagingCenter.Send<string>("msg", "This is a test");
                //});


                var intent = new Intent(this, typeof(LongRunningTaskService));
                StartService(intent);
            };
        }

        //private void StartSimulation()
        //{
        //    //MessagingCenter.Send

        //    CancellationToken cts = new CancellationToken();


        //    Task.Run(async () =>
        //     {
        //         await SimulationWorker(cts);
        //     });

        //}



    }
}