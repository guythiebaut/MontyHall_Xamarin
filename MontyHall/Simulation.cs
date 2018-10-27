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
                MessageHelper.Send("Simulation", "started");
                IRound RoundSimulation = new Round(3);
                int displayCount;
                var wins = 0;

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
                        Say("Games played: " + (i + 1).ToString("###,###,###"), "RoundNumber");
                    }

                    RoundSimulation.ClearRounds();
                    RoundSimulation.AddRound(true);

                    if (random)
                    {
                        swap = StaticRandom.Instance.Next(0, 2) == 1;
                    }

                    if (RoundSimulation.WinIfSwap() == swap)
                    {
                        wins++;
                        if (i % displayCount == 0)
                        {
                            Say("Games won: " + (wins).ToString("###,###,###"), "RoundsWon");
                        }
                    }
                }
                Say("Games played: " + string.Format("{0:n0}", rounds), "RoundNumber");
                Say("Games won: " + string.Format("{0:n0}", wins), "RoundsWon");
                MessageHelper.Send("Simulation", "completed");
            });
        }

        private void Say(string message, string sender)
        {
            MessageHelper.Send(sender, message);
        }
    }
}