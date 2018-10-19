﻿using System.Collections.Generic;
using MontyHall.Interfaces;

namespace MontyHall
{
    class GameShowHost
    {
        internal delegate void showDoorDelegate(int a, int b);
        showDoorDelegate showDoor;
        internal delegate void sayDelegate(string a, int b);
        sayDelegate say;
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

        public GameShowHost(MontyHall.Interfaces.IRound round, showDoorDelegate showDoorDel, sayDelegate sayDel)
        {
            ThisRound = round;
            showDoor = showDoorDel;
            say = sayDel;
            say("Pick a door below!", 1);
            say(string.Empty, 2);
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
                showDoor(revealedDoor, 0);
                say("Hold or swap?", 1);
                say("Pick a door again.", 2);
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

                if (prizeDoor == doorNumber)
                {
                    say("You win!", 1);
                    say(string.Empty, 2);
                    showDoor(toReveal, 0);
                }
                else
                {
                    say("You lose!", 1);
                    say(string.Empty, 2);
                    showDoor(toReveal, 1);
                }

                showDoor(prizeDoor, 2);
                CurrentStage = Stage.PrizeDoorRevealed;
            }

        }
    }
}