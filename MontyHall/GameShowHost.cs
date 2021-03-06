﻿using System.Collections.Generic;
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
        private int FirstDoorChosen;

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
                FirstDoorChosen = doorNumber;
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
                if (doorNumber!=FirstDoorChosen)
                {
                    MessageHelper.Send("Swapped", string.Empty);
                }
                CurrentStage = Stage.SecondDoorChosen;
                var prizeDoor = ThisRound.GetPrizeDoor();
                doorsRevealed.Add(prizeDoor + 1);
                var revealed = 0;

                foreach (var door in doorsRevealed)
                {
                    revealed += door;
                }

                var toReveal = 6 - revealed - 1;
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