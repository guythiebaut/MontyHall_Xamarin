using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MontyHall
{
    [Service]
    public class LongRunningTaskService : Service
    {
        CancellationTokenSource _cts;
        public delegate void sayDelegate(string a, string b);

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                try
                {
                    //INVOKE THE SHARED CODE
                    var simulation = new Simulation();
                    int rounds;
                    int.TryParse(intent.GetStringExtra("Rounds"), out rounds);
                    var swap = intent.GetBooleanExtra("Swap", false);
                    var random = intent.GetBooleanExtra("Random", false);
                    simulation.SimulationWorker(_cts.Token, rounds, swap, random).Wait();
                }
                catch (System.OperationCanceledException)
                {
                }
                finally
                {
                    if (_cts.IsCancellationRequested)
                    {
                        MessageHelper.Send("CancelledMessage", "cancelled");
                    }
                }

            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}