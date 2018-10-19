using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Xamarin.Forms;

namespace MontyHall
{
    public static class MessageHelper
    {
        public static void Send(string sender, string message)
        {
            Device.BeginInvokeOnMainThread(
                () => MessagingCenter.Send(message, sender)
            );
        }

        public static void Subscribe(object subscriber, string sender, object subscribingObject)
        {
            MessagingCenter.Subscribe<string>(subscriber, sender, (message) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (subscribingObject is TextView)
                    {
                        ((TextView)subscribingObject).Text = message;
                    }
                });
            });
        }
    }
}