using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DealOrNoDeal.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DealOrNoDeal
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DealOrNoDeal
    {
        #region Data members

        /// <summary>
        ///     The application height
        /// </summary>
        public const int ApplicationHeight = 500;

        /// <summary>
        /// The application width
        /// </summary>
        public const int ApplicationWidth = 500;

        private readonly List<Button> briefCaseButtons;
        private readonly List<Border> dollarAmountLabels;
        private readonly List<int> briefCasesClicked;
        private readonly GameManager gameManager;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DealOrNoDeal" /> class.
        /// </summary>
        public DealOrNoDeal()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size { Width = ApplicationWidth, Height = ApplicationHeight };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));

            this.briefCaseButtons = new List<Button>();
            this.dollarAmountLabels = new List<Border>();
            this.briefCasesClicked = new List<int>();
            this.gameManager = new GameManager();
            this.buildBriefCaseButtonCollection();
            this.buildDollarAmountLabelCollection();

            this.dealButton.IsEnabled = false;
            this.noDealButton.IsEnabled = false;
        }

        #endregion

        #region Methods

        private void buildDollarAmountLabelCollection()
        {
            this.dollarAmountLabels.Clear();

            this.dollarAmountLabels.Add(this.label0Border);
            this.dollarAmountLabels.Add(this.label1Border);
            this.dollarAmountLabels.Add(this.label2Border);
            this.dollarAmountLabels.Add(this.label3Border);
            this.dollarAmountLabels.Add(this.label4Border);
            this.dollarAmountLabels.Add(this.label5Border);
            this.dollarAmountLabels.Add(this.label6Border);
            this.dollarAmountLabels.Add(this.label7Border);
            this.dollarAmountLabels.Add(this.label8Border);
            this.dollarAmountLabels.Add(this.label9Border);
            this.dollarAmountLabels.Add(this.label10Border);
            this.dollarAmountLabels.Add(this.label11Border);
            this.dollarAmountLabels.Add(this.label12Border);
            this.dollarAmountLabels.Add(this.label13Border);
            this.dollarAmountLabels.Add(this.label14Border);
            this.dollarAmountLabels.Add(this.label15Border);
            this.dollarAmountLabels.Add(this.label16Border);
            this.dollarAmountLabels.Add(this.label17Border);
            this.dollarAmountLabels.Add(this.label18Border);
            this.dollarAmountLabels.Add(this.label19Border);
            this.dollarAmountLabels.Add(this.label20Border);
            this.dollarAmountLabels.Add(this.label21Border);
            this.dollarAmountLabels.Add(this.label22Border);
            this.dollarAmountLabels.Add(this.label23Border);
            this.dollarAmountLabels.Add(this.label24Border);
            this.dollarAmountLabels.Add(this.label25Border);
        }

        private void buildBriefCaseButtonCollection()
        {
            this.briefCaseButtons.Clear();

            this.briefCaseButtons.Add(this.case1);
            this.briefCaseButtons.Add(this.case2);
            this.briefCaseButtons.Add(this.case3);
            this.briefCaseButtons.Add(this.case4);
            this.briefCaseButtons.Add(this.case5);
            this.briefCaseButtons.Add(this.case6);
            this.briefCaseButtons.Add(this.case7);
            this.briefCaseButtons.Add(this.case8);
            this.briefCaseButtons.Add(this.case9);
            this.briefCaseButtons.Add(this.case10);
            this.briefCaseButtons.Add(this.case11);
            this.briefCaseButtons.Add(this.case12);
            this.briefCaseButtons.Add(this.case13);
            this.briefCaseButtons.Add(this.case14);
            this.briefCaseButtons.Add(this.case15);
            this.briefCaseButtons.Add(this.case16);
            this.briefCaseButtons.Add(this.case17);
            this.briefCaseButtons.Add(this.case18);
            this.briefCaseButtons.Add(this.case19);
            this.briefCaseButtons.Add(this.case20);
            this.briefCaseButtons.Add(this.case21);
            this.briefCaseButtons.Add(this.case22);
            this.briefCaseButtons.Add(this.case23);
            this.briefCaseButtons.Add(this.case24);
            this.briefCaseButtons.Add(this.case25);
            this.briefCaseButtons.Add(this.case26);
        }

        private void briefcase_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                button.IsEnabled = true;
                button.Visibility = Visibility.Collapsed;

                var briefCaseClicked = this.getBriefcaseIndex(button);

                this.setPlayerBriefCase(briefCaseClicked);

                var selectedDollarAmount =
                    this.gameManager.RemoveDollarAmountFromPlay(this.gameManager.GetBriefCaseValue(briefCaseClicked));

                this.greyOutLabel(selectedDollarAmount);

                this.briefCaseButtons.Remove(button);
            }
            this.updateCurrentRoundInformation();
        }

        private void greyOutLabel(int index)
        {
            var associatedLabel = this.dollarAmountLabels[index];
            associatedLabel.Background = new SolidColorBrush(Colors.Gray);
        }

        private int getBriefcaseIndex(Button selectedBriefCase)
        {
            if (!this.briefCaseButtons.Contains(selectedBriefCase))
            {
                return -1;
            }

            var briefCaseIndex = this.briefCaseButtons.IndexOf(selectedBriefCase);
            return briefCaseIndex;
        }

        private void updateCurrentRoundInformation()
        {
            this.roundLabel.Text = "Round " + this.gameManager.CurrentRound + ": " + this.gameManager.CasesRemainingForRound + " cases to open";
            this.updateLabelsAsCasesAreSelected();
        }

        private void updateLabelsAsCasesAreSelected()
        {
           // var casesRemaining = this.gameManager.CasesRemainingForRound;
           // this.gameManager.CasesRemainingForRound--;
            this.casesToOpenLabel.Text = this.gameManager.CasesRemainingForRound + " more cases to open";
            this.summaryOutput.Text = "Your case: " + this.gameManager.CaseSelectedByPlayer;

            if (this.gameManager.CasesRemainingForRound == 0)
            {
                this.summaryOutput.Text = "Offers: Min: " + this.gameManager.MinOfferFromBanker + "; Max: " + this.gameManager.MaxOfferFromBanker
                   + "\nCurrent offer: $" + this.gameManager.GetOffer() + "\nDeal or No Deal?";

                this.hideBriefCaseButtons();
                this.showDealAndNoDealButtons();
            }
        }

        private void dealButton_Click(object sender, RoutedEventArgs e)
        {
            this.summaryOutput.Text = "Your case contained: $" + this.gameManager.GetBriefCaseValue(this.gameManager.CaseSelectedByPlayer)
                + "\nAccepted offer: $" + this.gameManager.GetOffer() + "\nGAME OVER";

            this.hideBriefCaseButtons();
            this.hideDealAndNoDealButtons();
        }

        private void noDealButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.gameManager.CurrentRound == 9)
            {
                this.roundLabel.Text = "This is the final round.\nSelect a case.";
                //this.showBriefCaseButtons();
            }
            else
            {
                this.showBriefCaseButtons();

                this.hideDealAndNoDealButtons();

                this.summaryOutput.Text = "Offers: Min: $" + this.gameManager.MinOfferFromBanker + "; Max: $" +
                                          this.gameManager.MaxOfferFromBanker
                                          + "\nLast Offer: $" + this.gameManager.CurrentOfferFromBanker;

                this.gameManager.MoveToNextRound();
                this.updateCurrentRoundInformation();
            }
        }

        private void showBriefCaseButtons()
        {
            foreach (var button in this.briefCaseButtons)
            {
                button.IsEnabled = true;
                button.Visibility = Visibility.Visible;
            }
        }

        private void hideBriefCaseButtons()
        {
            foreach (var button in this.briefCaseButtons)
            {
                button.IsEnabled = false;
                button.Visibility = Visibility.Collapsed;
            }
        }

        private void hideDealAndNoDealButtons()
        {
            this.dealButton.IsEnabled = false;
            this.dealButton.Visibility = Visibility.Collapsed;

            this.noDealButton.IsEnabled = false;
            this.noDealButton.Visibility = Visibility.Collapsed;
        }

        private void showDealAndNoDealButtons()
        {
            this.dealButton.IsEnabled = true;
            this.dealButton.Visibility = Visibility.Visible;

            this.noDealButton.IsEnabled = true;
            this.noDealButton.Visibility = Visibility.Visible;
        }

        private void setPlayerBriefCase(int briefCase)
        {
            this.briefCasesClicked.Add(briefCase);
            this.gameManager.CaseSelectedByPlayer = this.briefCasesClicked[0] + 1;
        }

        #endregion
    }
}