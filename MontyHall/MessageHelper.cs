﻿using Android.Widget;
using System;
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
                    if (subscribingObject is TextView) { ((TextView)subscribingObject).Text = message; }
                });
            });
        }

        public static void Subscribe(object subscriber, string sender, Action<string, string> subscribingAction)
        {
            MessagingCenter.Subscribe<string>(subscriber, sender, (message) =>
            {
                Device.BeginInvokeOnMainThread(() => { subscribingAction.Invoke(sender, message); });
            });
        }
    }
}