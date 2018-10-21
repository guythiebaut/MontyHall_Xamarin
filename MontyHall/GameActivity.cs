using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using MontyHall.Interfaces;

namespace MontyHall
{
    [Activity]
    public class GameActivity : AppCompatActivity
    {
        ImageButton DoorLeft;
        ImageButton DoorCentre;
        ImageButton DoorRight;
        Button ResetGameButton;
        Button ResetStatsButton;
        TextView HostText1;
        TextView HostText2;
        TextView PlayedText;
        TextView WonText;
        TextView SwappedText;
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
            DoorLeft = FindViewById<ImageButton>(Resource.Id.doorLeft);
            DoorCentre = FindViewById<ImageButton>(Resource.Id.doorCentre);
            DoorRight = FindViewById<ImageButton>(Resource.Id.doorRight);
            ResetGameButton = FindViewById<Button>(Resource.Id.ResetGame);
            ResetStatsButton = FindViewById<Button>(Resource.Id.StatsReset);
            HostText1 = FindViewById<TextView>(Resource.Id.HostText1);
            HostText2 = FindViewById<TextView>(Resource.Id.HostText2);
            PlayedText = FindViewById<TextView>(Resource.Id.PlayedText);
            WonText = FindViewById<TextView>(Resource.Id.WonText);
            SwappedText = FindViewById<TextView>(Resource.Id.SwappedText);

            MessageHelper.Subscribe(this, "GameWon", MessageAction);
            MessageHelper.Subscribe(this, "Swapped", MessageAction);
            MessageHelper.Subscribe(this, "GamePlayed", MessageAction);
            MessageHelper.Subscribe(this, "HostSaysLine1", MessageAction);
            MessageHelper.Subscribe(this, "HostSaysLine2", MessageAction);
            MessageHelper.Subscribe(this, "ShowPrizeDoor", MessageAction);
            MessageHelper.Subscribe(this, "ShowGoatDoor", MessageAction);
        }

        private void MessageAction(string sender, string message)
        {
            if (sender == "GameWon") Won();
            if (sender == "Swapped") Swap();
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
            int doorImage = prize ? Resource.Drawable.gold : Resource.Drawable.goat;

            switch (doorToShow)
            {
                case 0:
                    DoorLeft.SetImageResource(doorImage);
                    break;
                case 1:
                    DoorCentre.SetImageResource(doorImage);
                    break;
                case 2:
                    DoorRight.SetImageResource(doorImage);
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
            SwappedText.Text = string.Format("Times swapped: {0}", Swapped);
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
            SwappedText.Text = string.Format("Times swapped: {0}", Swapped);
        }

        private void ResetDoors()
        {
            DoorLeft.SetImageResource(Resource.Drawable.door_transparent);
            DoorCentre.SetImageResource(Resource.Drawable.door_transparent);
            DoorRight.SetImageResource(Resource.Drawable.door_transparent);
        }
    }
}