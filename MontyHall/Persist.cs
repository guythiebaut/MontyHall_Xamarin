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

namespace MontyHall
{
    public class Persist
    {
        ISharedPreferences prefs = Application.Context.GetSharedPreferences("PREF_NAME", FileCreationMode.Private);

        public void AddPreference(string key, string value)
        {
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(key, (string)value);
            editor.Apply();
        }

        public object GetPreference(string key)
        {
            return prefs.GetString(key, null);
        }

        public bool Contains(string key)
        {
            return prefs.Contains(key);
        }
    }
}