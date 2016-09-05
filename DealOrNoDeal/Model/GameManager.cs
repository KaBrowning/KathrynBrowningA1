using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Popups;

namespace DealOrNoDeal.Model
{
    /// <summary>
    ///     Handles the management of the actual gameplay.
    /// </summary>
    public class GameManager
    {
        #region Types and Delegates

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

        #endregion

        #region Data members

        private static int maxOffer;
        private static int minOffer;

        private List<int> dollarValuesForGame;
        private readonly List<int> briefCaseDollarValues;
        private readonly List<int> dollarValuesInPlay;
        private readonly List<int> allBankOffers;
        private readonly Banker banker;
        private Dictionary<int, int> casesForNineRoundGame;
        private Dictionary<int, int> casesForTenRoundGame;
        private Dictionary<int, int> casesForFourteenRoundGame;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the current round.
        /// </summary>
        /// <value>
        ///     The current round.
        /// </value>
        public int CurrentRound { get; set; } = 1;

        /// <summary>
        ///     Gets or sets the cases to be opened in current round.
        /// </summary>
        /// <value>
        ///     The cases to be opened in current round.
        /// </value>
        public int CasesRemainingForRound { get; set; } = 6;

        /// <summary>
        ///     Gets or sets the cases to be opened in next round.
        /// </summary>
        /// <value>
        ///     The cases to be opened in next round.
        /// </value>
        public int CasesToBeOpenedInNextRound { get; set; }

        /// <summary>
        ///     Gets or sets the case selected by player.
        /// </summary>
        /// <value>
        ///     The case selected by player.
        /// </value>
        public int CaseSelectedByPlayer { get; set; } = 0;

        /// <summary>
        ///     Gets or sets the current offer from banker.
        /// </summary>
        /// <value>
        ///     The current offer from banker.
        /// </value>
        public int CurrentOfferFromBanker { get; set; }

        /// <summary>
        ///     Gets or sets the minimum offer from banker.
        /// </summary>
        /// <value>
        ///     The minimum offer from banker.
        /// </value>
        public int MinOfferFromBanker { get; set; } = minOffer;

        /// <summary>
        ///     Gets or sets the maximum offer from banker.
        /// </summary>
        /// <value>
        ///     The maximum offer from banker.
        /// </value>
        public int MaxOfferFromBanker { get; set; } = maxOffer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        public GameManager()
        {
            this.dollarValuesForGame = new List<int> {
                0,
                1,
                5,
                10,
                25,
                50,
                75,
                100,
                200,
                300,
                400,
                500,
                750,
                1000,
                5000,
                10000,
                25000,
                50000,
                75000,
                100000,
                200000,
                300000,
                400000,
                500000,
                750000,
                1000000
            };
            this.casesForNineRoundGame = new Dictionary<int, int>() {
                {1, 8 },
                {2, 6 },
                {3, 4 },
                {4, 2 },
                {5, 1 },
                {6, 1 },
                {7, 1 },
                {8, 1 },
                {9, 1 }

            };
            this.casesForTenRoundGame = new Dictionary<int, int>() {
                {1, 6 },
                {2, 5 },
                {3, 4 },
                {4, 3 },
                {5, 2 },
                {6, 1 },
                {7, 1 },
                {8, 1 },
                {9, 1 },
                {10, 1 }
            };
            this.casesForFourteenRoundGame = new Dictionary<int, int>() {
                {1, 5 },
                {2, 4 },
                {3, 3 },
                {4, 2 },
                {5, 1 },
                {6, 1 },
                {7, 1 },
                {8, 1 },
                {9, 1 },
                {10, 1 },
                {11, 1 },
                {12, 1 },
                {13, 1 },
                {14, 1 }
            };

            this.briefCaseDollarValues = new List<int>();
            this.changeGameType(GameVersion.Regular, 9);
            this.dollarValuesInPlay = new List<int>(this.dollarValuesForGame);
            this.banker = new Banker();
            this.allBankOffers = new List<int>();
            this.populateBriefCasesWithRandomDollarValuesForStartOfGame();
        }

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
            this.CurrentOfferFromBanker = this.banker.BankOffer(this.dollarValuesInPlay,
                this.numofCasesToOpenNextRound());
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
            //this.NumOfCasesToOpenPerRound();
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

        private void changeGameType(GameVersion gameVersion, int numofRounds)
        {
            this.changeDollarValuesForGame(gameVersion);
            this.NumOfCasesToOpenPerRound(numofRounds);
        }

        private void changeDollarValuesForGame(GameVersion gameVersion)
        {
            switch (gameVersion)
            {
                case GameVersion.Syndicated:
                    this.dollarValuesForGame = new List<int> {
                        0,
                        1,
                        5,
                        10,
                        25,
                        50,
                        75,
                        100,
                        200,
                        300,
                        400,
                        500,
                        750,
                        1000,
                        2500,
                        5000,
                        10000,
                        25000,
                        50000,
                        75000,
                        100000,
                        150000,
                        200000,
                        250000,
                        350000,
                        500000
                    };
                    break;
                case GameVersion.Regular:
                    this.dollarValuesForGame = new List<int> {
                        0,
                        1,
                        5,
                        10,
                        25,
                        50,
                        75,
                        100,
                        200,
                        300,
                        400,
                        500,
                        750,
                        1000,
                        5000,
                        10000,
                        25000,
                        50000,
                        75000,
                        100000,
                        200000,
                        300000,
                        400000,
                        500000,
                        750000,
                        1000000
                    };
                    break;
                case GameVersion.Mega:
                    this.dollarValuesForGame = new List<int> {
                        0,
                        100,
                        500,
                        1000,
                        2500,
                        5000,
                        7500,
                        10000,
                        20000,
                        30000,
                        40000,
                        50000,
                        75000,
                        100000,
                        225000,
                        400000,
                        500000,
                        750000,
                        1000000,
                        2000000,
                        3000000,
                        4000000,
                        5000000,
                        6000000,
                        8500000,
                        10000000
                    };
                    break;
            }
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

        /// <summary>
        /// Numbers the of cases to open per round.
        /// </summary>
        /// <returns></returns>
        public void NumOfCasesToOpenPerRound(int numOfRounds)
        {
            if (numOfRounds != 9 || numOfRounds != 10 || numOfRounds != 14)
            {
                throw new ArgumentOutOfRangeException(); //unhandled in user code
            }

            try
            {
                if (isTenRounds(numOfRounds))
                {
                    this.decrementTenRounds();
                }
                else if (isFourteenRounds(numOfRounds))
                {
                    this.decrementFourteenRounds();
                }
                else
                {
                    this.decrementNineRounds();
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                var whatHappened = new MessageDialog("error has occured" + e);
            }     
        }

        private static bool isTenRounds(int numOfRounds)
        {
            return numOfRounds == 10;
        }

        private static bool isFourteenRounds(int numOfRounds)
        {
            return numOfRounds == 14;
        }

        private void decrementNineRounds()
        {
            foreach (var round in this.casesForNineRoundGame)
            {
                this.CasesRemainingForRound = this.casesForNineRoundGame[round.Value]--;
            }
        }

        private void decrementTenRounds()
        {
            foreach (var round in this.casesForTenRoundGame)
            {
                this.CasesRemainingForRound = this.casesForTenRoundGame[round.Value]--;
            }
        }

        private void decrementFourteenRounds()
        {
            foreach (var round in this.casesForFourteenRoundGame)
            {
                this.CasesRemainingForRound = this.casesForFourteenRoundGame[round.Value]--; //may not decrement the per round cases, may decrement the entire number of cases to be opened.
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