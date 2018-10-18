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
    public class LoopTest
    {
        // https://arteksoftware.com/backgrounding-with-xamarin-forms/
        public async Task SimulationWorker(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    token.ThrowIfCancellationRequested();
                    //await Task.Delay(10);
                    if (i%1000 == 0)
                    {
                        var message = i.ToString();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MessagingCenter.Send<string>(message, "msg");
                        });
                    }
                }
            }, token);
        }
    }
}
