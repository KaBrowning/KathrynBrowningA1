using System;
using System.Collections.Generic;

namespace DealOrNoDeal.Model
{
    /// <summary>
    ///     Handles the calculation of the banker's offer
    /// </summary>
    internal class Banker
    {

        /// <summary>
        ///     Calculates an offer to give the player based on how much money is in play and how many cases are to be opened in the next round.
        ///     Precondition: The user must have completed at least 1 round of the game.
        ///     Postcondition: None
        /// </summary>
        /// <param name="dollarAmonutsInPlay">The dollar amonuts in play.</param>
        /// <param name="numOfCasesToOpenInNextRound">The number of cases to open in next round.</param>
        /// <returns>The calculated offer.</returns>
        public int BankOffer(List<int> dollarAmonutsInPlay, int numOfCasesToOpenInNextRound)
        {
            var totalAmountInPlay = 0.0;

            foreach (var briefCaseDollars in dollarAmonutsInPlay)
            {
                totalAmountInPlay += briefCaseDollars;
            }

            return (int)Math.Round(totalAmountInPlay / numOfCasesToOpenInNextRound / dollarAmonutsInPlay.Count);

        }
    }
}
