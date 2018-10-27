using System;
using System.Collections.Generic;
using MontyHall.Interfaces;

namespace MontyHall
{
    public class Round : IRound
    {
        private int _DoorCount;
        private int DoorCount
        { get { return _DoorCount; } set { _DoorCount = value; } }
        internal List<Doors> Rounds = new List<Doors>();

        internal enum EPrize
        {
            Goat,
            Car
        }

        internal Round(int doorCount)
        {
            DoorCount = doorCount;
        }

        internal int PrizeDoor(int round)
        {
            return Rounds[round].Door.FindIndex(x => x == EPrize.Car);
        }

        internal class Doors
        {
            internal List<EPrize> Door = new List<EPrize>();
            internal int _ChosenDoor;
            internal bool WinIfSwap;

            internal int ChosenDoor
            {
                get { return _ChosenDoor; }
                set
                {
                    _ChosenDoor = value;
                    WinIfSwap = Door[_ChosenDoor] == EPrize.Goat;
                }
            }

            internal int PrizeDoor()
            {
                return Door.FindIndex(x => x == EPrize.Car);
            }

            internal List<int> GoatDoors()
            {
                var goats = new List<int>() { 0, 1, 2 };
                goats.RemoveAt(Door.FindIndex(x => x == EPrize.Car));
                return goats;
            }
        }

        private Doors GetNewRound(int doorCount, bool autoChooseDoor)
        {
            var doors = new Doors();

            for (int i = 0; i < doorCount; i++)
            {
                doors.Door.Add(EPrize.Goat);
            }

            //let's set the prize door
            doors.Door[GetRandomDoor()] = EPrize.Car;

            if (autoChooseDoor)
            {
                doors.ChosenDoor = GetRandomDoor();
            }
            return doors;
        }

        internal static int GetRandomDoor()
        {
            var door = GetRandInt(0, 3);
            return door;
        }

        private static int GetRandInt(int min, int max)
        {
            return StaticRandom.Instance.Next(min, max);
        }

        int IRound.GetPrizeDoor(int round)
        {
            throw new NotImplementedException();
        }

        int IRound.GetGoatDoor(int round, int excludeDoor)
        {
            return GetGoatDoor(round, excludeDoor);
        }

        int IRound.GetGoatDoor(int excludeDoor)
        {
            return GetGoatDoor(0, excludeDoor);
        }

        int IRound.GetPrizeDoor()
        {
            return Rounds[0].PrizeDoor();
        }

        int GetGoatDoor(int round, int excludeDoor)
        {
            var goats = Rounds[0].GoatDoors();

            if (goats.Contains(excludeDoor))
            {
                goats.RemoveAt(goats.IndexOf(excludeDoor));
            }

            var randGoatDoor = GetRandInt(0, goats.Count);
            var count = 0;

            foreach (int door in goats)
            {
                if (door != excludeDoor)
                {
                    if (count == randGoatDoor)
                    {
                        return door;
                    }
                    count++;
                }
            }
            return -1;
        }

        void IRound.ClearRounds()
        {
            Rounds.Clear();
        }

        void IRound.AddRound(bool autoChooseDoor)
        {
            Rounds.Add(GetNewRound(DoorCount, autoChooseDoor));
        }

        bool IRound.WinIfSwap(int round)
        {
            return Rounds[round].WinIfSwap;
        }

        bool IRound.WinIfSwap()
        {
            return Rounds[0].WinIfSwap;
        }
    }
}