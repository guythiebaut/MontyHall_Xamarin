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
        Button ResetStatsButton;
        TextView HostText1;
        TextView HostText2;
        TextView PlayedText;
        TextView WonText;
        int GamesPlayed;
        int GamesWon;
        int Swapped;

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
            ResetStatsButton = FindViewById<Button>(Resource.Id.StatsReset);
            HostText1 = FindViewById<TextView>(Resource.Id.HostText1);
            HostText2 = FindViewById<TextView>(Resource.Id.HostText2);
            PlayedText = FindViewById<TextView>(Resource.Id.PlayedText);
            WonText = FindViewById<TextView>(Resource.Id.WonText);

            MessageHelper.Subscribe(this, "GameWon", ActionDelegate);
            MessageHelper.Subscribe(this, "GamePlayed", ActionDelegate);
            MessageHelper.Subscribe(this, "HostSaysLine1", ActionDelegate);
            MessageHelper.Subscribe(this, "HostSaysLine2", ActionDelegate);
            MessageHelper.Subscribe(this, "ShowPrizeDoor", ActionDelegate);
            MessageHelper.Subscribe(this, "ShowGoatDoor", ActionDelegate);
        }

        private void ActionDelegate(string sender, string message)
        {
            if (sender == "GameWon") Won();
            if (sender == "GamePlayed") Played();
            if (sender == "HostSaysLine1") HostText1.Text = message;
            if (sender == "HostSaysLine2") HostText2.Text = message;
            if (sender == "ShowPrizeDoor") ShowDoor(true, message);
            if (sender == "ShowGoatDoor") ShowDoor(false, message);
        }

        private void ShowDoor(bool prize, string door)
        {
            int doorToShow;
            int.TryParse(door, out doorToShow);
            string doorText = prize ? "Prize" : "Goat";

            switch (doorToShow)
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

        private void SetUpGame()
        {
            IRound RoundGame = new Round(3);
            RoundGame.AddRound(false);
            var host = new GameShowHost(RoundGame);

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
                host = new GameShowHost(ResetRoundGame);
                ResetDoors();
            };

            ResetStatsButton.Click += (sender, e) =>
            {
                ResetStats();
            };
        }

        private void Speak(string sayText, int lineNumber)
        {
            switch (lineNumber)
            {
                case 1:
                    HostText1.Text = sayText;
                    break;
                case 2:
                    HostText2.Text = sayText;
                    break;
                default:
                    break;
            }
        }

        private void Swap()
        {
            Swapped++;
        }

        private void Played()
        {
            GamesPlayed++;
            PlayedText.Text = string.Format("Games played: {0}", GamesPlayed);
        }

        private void Won()
        {
            GamesWon++;
            WonText.Text = string.Format("Games Won: {0}", GamesWon);
        }

        private void ResetStats()
        {
            GamesWon = 0;
            Swapped = 0;
            GamesPlayed = 0;
            PlayedText.Text = string.Format("Games played: {0}", GamesPlayed);
            WonText.Text = string.Format("Games Won: {0}", GamesWon);
        }

        private void ResetDoors()
        {
            DoorLeft.Text = "Door 1";
            DoorCentre.Text = "Door 2";
            DoorRight.Text = "Door 3";
        }
    }
}