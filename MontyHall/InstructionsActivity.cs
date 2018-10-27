using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Android.Content;
using System.Text;
using System.IO;
using System.Reflection;

namespace MontyHall
{
    [Activity]
    public class InstructionsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_instructions);

            TextView Instructions = FindViewById<TextView>(Resource.Id.Instructions);
            Instructions.Text = GetFromResources("Resources.files.instructions.txt");
        }

        internal string GetFromResources(string resourceName)
        {
            var assem = Assembly.GetExecutingAssembly();
            var name = assem.GetName().Name;
            //note - the following can be used to get a list of resources
            ///var resources = assem.GetManifestResourceNames();
            using (Stream stream = assem.GetManifestResourceStream(assem.GetName().Name + '.' + resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}