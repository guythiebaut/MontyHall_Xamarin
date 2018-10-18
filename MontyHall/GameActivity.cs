using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using MontyHall.Interfaces;
using System;

namespace MontyHall
{
    [Activity]
    public class GameActivity : AppCompatActivity
    {
        Button DoorLeft;
        Button DoorCentre;
        Button DoorRight;
        Button ResetGameButton;
        TextView HostText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_game);
            WireUpControls();
            SetUpGame();
        }

        private void WireUpControls()
        {
            DoorLeft = FindViewById<Button>(Resource.Id.doorLeft);
            DoorCentre = FindViewById<Button>(Resource.Id.doorCentre);
            DoorRight = FindViewById<Button>(Resource.Id.doorRight);
            ResetGameButton = FindViewById<Button>(Resource.Id.ResetGame);
            HostText = FindViewById<TextView>(Resource.Id.HostText);
        }

        private void SetUpGame()
        {
            IRound RoundGame = new Round(3);
            RoundGame.AddRound(false);
            var host = new GameShowHost(RoundGame, RevealDoor, Speak);

            DoorLeft.Click += (sender, e) =>
            {
                host.DoorSelectedByPlayer(0);
            };

            DoorCentre.Click += (sender, e) =>
            {
                host.DoorSelectedByPlayer(1);
            };

            DoorRight.Click += (sender, e) =>
            {
                host.DoorSelectedByPlayer(2);
            };

            ResetGameButton.Click += (sender, e) =>
            {
                IRound ResetRoundGame = new Round(3);
                ResetRoundGame.AddRound(false);
                host = new GameShowHost(ResetRoundGame, RevealDoor, Speak);
                ResetDoors();
            };
        }

        private void Speak(string sayText)
        {
            HostText.Text = sayText;
        }

        private void ResetDoors()
        {
            DoorLeft.Text = "Door 1";
            DoorCentre.Text = "Door 2";
            DoorRight.Text = "Door 3";
        }

        private void RevealDoor(int door, int doorType)
        {
            string doorText = doorType == 2 ? "Prize" : "Goat";

            switch (door)
            {
                case 0:
                    DoorLeft.Text = doorText;
                    break;
                case 1:
                    DoorCentre.Text = doorText;
                    break;
                case 2:
                    DoorRight.Text = doorText;
                    break;
                default:
                    break;
            }
        }

    }
}