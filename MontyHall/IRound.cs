
namespace MontyHall.Interfaces
{
    interface IRound
    {
        int GetPrizeDoor(int round);
        int GetPrizeDoor();
        int GetGoatDoor(int round, int excludeDoor);
        int GetGoatDoor(int excludeDoor);
        void ClearRounds();
        void AddRound(bool chosenDoor);
        bool WinIfSwap(int round);
        bool WinIfSwap();
    }
}