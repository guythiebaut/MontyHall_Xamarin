using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using MontyHall.Interfaces;

namespace MontyHall
{
    public class Simulation
    {

        // https://arteksoftware.com/backgrounding-with-xamarin-forms/
        public async Task SimulationWorker(CancellationToken token, int rounds, bool swap, bool random)
        {

            await Task.Run(async () =>
            {
                IRound RoundSimulation = new Round(3);
                int displayCount;
                int wins = 0;

                if (rounds >= 100000)
                {
                    displayCount = rounds / 1001;
                }
                else
                {
                    displayCount = rounds / 3;
                }

                for (int i = 0; i < rounds; i++)
                {
                    //if (bw.CancellationPending)
                    //{

                    //    e.Cancel = true;
                    //    return;

                    //}
                    if (i % displayCount == 0)
                    {
                        Say("Round number: " + (i + 1).ToString("###,###,###"), "RoundNumber");
                    }

                    RoundSimulation.ClearRounds();


                    RoundSimulation.AddRound(true);

                    if (RoundSimulation.WinIfSwap() == swap)
                    {
                        wins++;
                        if (i % displayCount == 0)
                        {
                            Say("Rounds won: " + (wins).ToString("###,###,###"), "RoundsWon");
                        }
                    }
                }
                Say("Round number: " + (rounds).ToString("###,###,###"), "RoundNumber");
                Say("Rounds won: " + (wins).ToString("###,###,###"), "RoundsWon");
            });
        }

        private void Say(string message, string sender)
        {
            MessageHelper.Send(sender, message);
        }
    }
}