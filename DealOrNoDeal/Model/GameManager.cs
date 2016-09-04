using System;
using System.Collections.Generic;
using System.Linq;

namespace DealOrNoDeal.Model
{

    /// <summary>
    ///     Handles the management of the actual gameplay.
    /// </summary>
    public class GameManager
    {
        /// <summary>
        ///     Deal or No Deal consists of 3 different game versions: Syndicated, Regular, or Mega, each 
        ///     with varying monetary amounts.
        /// </summary>
        public enum GameVersion
        {
            /// <summary>
            ///     The version of the game in which monetary values are significantly lower.
            /// </summary>
            Syndicated,

            /// <summary>
            ///     The normal version of the game.
            /// </summary>
            Regular,

            /// <summary>
            ///     The version of the game in which monetary values are significantly higher.
            /// </summary>
            Mega
        }

        private readonly List<int> dollarValuesForGame;
        private readonly List<int> briefCaseDollarValues;
        private readonly List<int> dollarValuesInPlay;
        private readonly List<int> allBankOffers;
        private readonly Banker banker;
        private static int maxOffer;
        private static int minOffer;

        /// <summary>
        ///      Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        public GameManager()
        {
            this.dollarValuesForGame = new List<int> { 0, 1, 5, 10, 25, 50, 75, 100, 200, 300, 400, 500, 750, 1000, 5000, 10000, 25000, 50000, 75000, 100000, 200000, 300000, 400000, 500000, 750000, 1000000 };
            this.briefCaseDollarValues = new List<int>();
            this.dollarValuesInPlay = new List<int>(this.dollarValuesForGame);
            this.banker = new Banker();
            this.allBankOffers = new List<int>();

            this.populateBriefCasesWithRandomDollarValuesForStartOfGame();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the current round.
        /// </summary>
        /// <value>
        /// The current round.
        /// </value>
        public int CurrentRound { get; set; } = 1;

        /// <summary>
        /// Gets or sets the cases to be opened in current round.
        /// </summary>
        /// <value>
        /// The cases to be opened in current round.
        /// </value>
        public int CasesRemainingForRound { get; set; } = 6;

        /// <summary>
        /// Gets or sets the cases to be opened in next round.
        /// </summary>
        /// <value>
        /// The cases to be opened in next round.
        /// </value>
        public int CasesToBeOpenedInNextRound { get; set; }

        /// <summary>
        /// Gets or sets the case selected by player.
        /// </summary>
        /// <value>
        /// The case selected by player.
        /// </value>
        public int CaseSelectedByPlayer { get; set; } = 0;

        /// <summary>
        /// Gets or sets the current offer from banker.
        /// </summary>
        /// <value>
        /// The current offer from banker.
        /// </value>
        public int CurrentOfferFromBanker { get; set; }

        /// <summary>
        /// Gets or sets the minimum offer from banker.
        /// </summary>
        /// <value>
        /// The minimum offer from banker.
        /// </value>
        public int MinOfferFromBanker { get; set; } = minOffer;


        /// <summary>
        /// Gets or sets the maximum offer from banker.
        /// </summary>
        /// <value>
        /// The maximum offer from banker.
        /// </value>
        public int MaxOfferFromBanker { get; set; } = maxOffer;

        /// <summary>
        /// Gets or sets the current game version.
        /// </summary>
        /// <value>
        /// The current game version.
        /// </value>
        public GameVersion CurrentGameVersion { get; set; } = GameVersion.Regular;

        #endregion

        #region Methods

        /// <summary>
        ///     Removes the dollar amount from the values that are still in play.
        ///     Precondition: User must select a brief case
        ///     Postcondition: dollarValuesInPlay has less values
        /// </summary>
        /// <param name="value">The value of the dollar amount to remove from play.</param>
        /// <returns>The index of the list element that contains the specifed dollar amount; -1 if value is not found</returns>
        public int RemoveDollarAmountFromPlay(int value)
        {
            if (!this.dollarValuesForGame.Contains(value))
            {
                return -1;
            }

            var dollarIndex = this.dollarValuesForGame.IndexOf(value);
            this.dollarValuesInPlay.Remove(value);
            return dollarIndex;
        }


        /// <summary>
        ///     Gets the offer.
        ///     Precondition: None
        ///     Postcondition: Current offer is stored in a list of offers
        /// </summary>
        /// <returns>The banker's current offer</returns>
        public int GetOffer()
        {
            this.CurrentOfferFromBanker = this.banker.BankOffer(this.dollarValuesInPlay, this.numofCasesToOpenNextRound());
            this.allBankOffers.Add(this.CurrentOfferFromBanker);
            this.offers();
            return this.CurrentOfferFromBanker;
        }

        /// <summary>
        ///     Moves to next round by incrementing Round property and setting
        ///     initial number of cases for that round
        ///     Precondition: None
        ///     Postcondition: Round == Round@prev + 1 AND CasesRemainingForRound == (number of cases to open for a new the round)
        /// </summary>
        public void MoveToNextRound()
        {
            this.CurrentRound = this.CurrentRound + 1;
            this.numOfCasesToOpenCurrentRound();
        }


        /// <summary>
        ///     Gets the brief case value.
        ///     Precondition: User must select a brief case number
        ///     Postcondition: None
        /// </summary>
        /// <param name="briefCaseNumber">The brief case number.</param>
        /// <returns>The value inside the specified briefcase</returns>
        public int GetBriefCaseValue(int briefCaseNumber)
        {
            return this.briefCaseDollarValues[briefCaseNumber];
        }

        private void populateBriefCasesWithRandomDollarValuesForStartOfGame()
        {
            var dollarValuesForGameClone = new List<int>(this.dollarValuesForGame);

            var random = new Random();

            for (var i = 0; i < 26; i++)
            {
                var randomNumber = random.Next(0, dollarValuesForGameClone.Count);
                var dollarAmount = dollarValuesForGameClone[randomNumber];
                this.briefCaseDollarValues.Add(dollarAmount);

                dollarValuesForGameClone.Remove(dollarAmount);
            }
        }

        private int numofCasesToOpenNextRound()
        {
            switch (this.CurrentRound)
            {
                case 1:
                    return this.CasesToBeOpenedInNextRound = 5;
                case 2:
                    return this.CasesToBeOpenedInNextRound = 4;
                case 3:
                    return this.CasesToBeOpenedInNextRound = 4;
                case 4:
                    return this.CasesToBeOpenedInNextRound = 3;
                case 5:
                    return this.CasesToBeOpenedInNextRound = 2;
                default:
                    return this.CasesToBeOpenedInNextRound = 1;
            }
        }

        private void numOfCasesToOpenCurrentRound()
        {
            switch (this.CurrentRound)
            {
                case 1:
                    this.CasesRemainingForRound = 6;
                    return;
                case 2:
                    this.CasesRemainingForRound = 5;
                    return;
                case 3:
                    this.CasesRemainingForRound = 4;
                    return;
                case 4:
                    this.CasesRemainingForRound = 3;
                    return;
                case 5:
                    this.CasesRemainingForRound = 2;
                    return;
                default:
                    this.CasesRemainingForRound = 1;
                    return;
            }
        }

        private void offers()
        {
            minOffer = this.allBankOffers.Min();
            this.MinOfferFromBanker = minOffer;

            maxOffer = this.allBankOffers.Max();
            this.MaxOfferFromBanker = maxOffer;
        }

        #endregion
    }
}