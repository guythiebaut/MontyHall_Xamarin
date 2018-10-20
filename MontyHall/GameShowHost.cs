using System.Collections.Generic;
using MontyHall.Interfaces;

namespace MontyHall
{
    class GameShowHost
    {
        internal IRound ThisRound;
        internal int revealedDoor;
        internal List<int> doorsRevealed = new List<int>();

        enum Stage
        {
            GameStarted,
            FirstDoorChosen,
            DoorRevealed,
            SecondDoorChosen,
            PrizeDoorRevealed
        }

        private Stage CurrentStage = Stage.GameStarted;

        public GameShowHost(MontyHall.Interfaces.IRound round)
        {
            ThisRound = round;
            MessageHelper.Send("HostSaysLine1", "Pick a door below!");
            MessageHelper.Send("HostSaysLine2", string.Empty);
        }

        public void ResetGame()
        {
            ThisRound.ClearRounds();
        }

        public void DoorSelectedByPlayer(int doorNumber)
        {
            if (CurrentStage == Stage.GameStarted)
            {
                doorsRevealed.Clear();
                CurrentStage = Stage.FirstDoorChosen;
                revealedDoor = ThisRound.GetGoatDoor(doorNumber);
                doorsRevealed.Add(revealedDoor + 1);
                MessageHelper.Send("ShowGoatDoor", revealedDoor.ToString());
                MessageHelper.Send("HostSaysLine1", "Hold or swap ?");
                MessageHelper.Send("HostSaysLine2", "Pick a door again.");
                return;
            }

            if (CurrentStage == Stage.FirstDoorChosen && doorNumber != revealedDoor)
            {
                CurrentStage = Stage.SecondDoorChosen;
                int prizeDoor = ThisRound.GetPrizeDoor();
                doorsRevealed.Add(prizeDoor + 1);
                int revealed = 0;

                foreach (int door in doorsRevealed)
                {
                    revealed += door;
                }

                int toReveal = 6 - revealed - 1;
                MessageHelper.Send("GamePlayed", string.Empty);

                if (prizeDoor == doorNumber)
                {
                    MessageHelper.Send("GameWon", string.Empty);
                    MessageHelper.Send("HostSaysLine1", "You win!");
                    MessageHelper.Send("HostSaysLine2", string.Empty);
                    MessageHelper.Send("ShowGoatDoor", toReveal.ToString());
                }
                else
                {
                    MessageHelper.Send("HostSaysLine1", "You lose!");
                    MessageHelper.Send("HostSaysLine2", string.Empty);
                    MessageHelper.Send("ShowGoatDoor", toReveal.ToString());
                }

                MessageHelper.Send("ShowPrizeDoor", prizeDoor.ToString());
                CurrentStage = Stage.PrizeDoorRevealed;
            }

        }
    }
}