﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(MontyHall.AndroidForceKeyboardDismissalService))]
namespace MontyHall
{
    public class AndroidForceKeyboardDismissalService : IForceKeyboardDismissalService
    {
        public void DismissKeyboard()
        {
            InputMethodManager imm = InputMethodManager.FromContext(CrossCurrentActivity.Current.Activity.ApplicationContext);

            imm.HideSoftInputFromWindow(
                CrossCurrentActivity.Current.Activity.Window.DecorView.WindowToken, HideSoftInputFlags.NotAlways);
        }
    }
}