using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utopia201X.BordeGameScence.UI;
using Utopia201X.BordeGameScence.UI.Top;
using TMPro;

namespace Utopia201X.BordeGameScence.Managers
{
    public class GameBordUIAndMenusManager
    {
        private static GameBordUIAndMenusManager instance = null;

        private TopPanelManager topPanelManager;
        private CitizensGroupPanelsManager citizensGroupPanelsManager;
        private Dictionary<string, Dictionary<string, GameObject>> arrows;
        private BottomPanelManeger bottomPanelManeger;                      

        private GameBordUIAndMenusManager()
        {
            topPanelManager = TopPanelManager.Instance;
            citizensGroupPanelsManager = CitizensGroupPanelsManager.Instance;
            SetupArrows();
            bottomPanelManeger = BottomPanelManeger.instance;                                               
        }

        public static GameBordUIAndMenusManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameBordUIAndMenusManager();
                return instance;
            }
        }       

        #region topPanelManager Methods

        public void AskToSetTimerPanel(int time, int round)
        {
            topPanelManager.SetTimerPanel(time, round);
        }

        public void AskToSetupPlayerPanel(int points, int citizen, int gold, string ownerName, string side, string gameType)
        {
            topPanelManager.SetupPlayerPanel(points, citizen, gold, ownerName, side, gameType);

            if (ownerName == "Player1")
            {
                ArrowsManagement(side);
            }
        }

        public void UpdatePlayerPanel(int points, int citizen, int gold, string side)
        {
            topPanelManager.UpdatePlayerPanel(points, citizen, gold, side);
        }

        #endregion

        #region CitizensGroupPanelsManager Methods

        public void AskToUpdateWorkStatus(string side, int groupPanelIndex, int workStatusNumber)
        {
            citizensGroupPanelsManager.UpdateWorkStatus(side, groupPanelIndex, workStatusNumber);
        }

        public void AskToUpdateExtraText(string side, string categoryName, int updateTo)
        {
            citizensGroupPanelsManager.UpdateExtraPanel(side, categoryName, updateTo);
        }

        public void ActiveCitizensGroupsPanel(string side)
        {
            citizensGroupPanelsManager.ActiveCitizensGroupsPanel(side);
        }

        #endregion

        #region Arrows Method

        private void SetupArrows()
        {
            arrows = new Dictionary<string, Dictionary<string, GameObject>>();

            arrows.Add("left", new Dictionary<string, GameObject>());
            arrows["left"].Add("Up", GameObject.Find("UpArrowLeft"));
            arrows["left"].Add("Down", GameObject.Find("DownArrowLeft"));
            arrows["left"]["Up"].SetActive(false);
            arrows["left"]["Down"].SetActive(false);

            arrows.Add("right", new Dictionary<string, GameObject>());
            arrows["right"].Add("Up", GameObject.Find("UpArrowRight"));
            arrows["right"].Add("Down", GameObject.Find("DownArrowRight"));
            arrows["right"]["Up"].SetActive(false);
            arrows["right"]["Down"].SetActive(false);
        }

        private void ArrowsManagement(string relevantPlayerSide)
        {
            arrows[relevantPlayerSide]["Up"].SetActive(true);
            arrows[relevantPlayerSide]["Down"].SetActive(true);
        }

        #endregion
                
        #region bottomPanelManeger Methods

        #region UC3

        public void ShutDownBottomPanel()
        {
            bottomPanelManeger.ButtensToggler();
            bottomPanelManeger.ClosePanels();
        }

        public void TurnOnBottomPanel()
        {
            bottomPanelManeger.ButtensToggler();           
        }

        public void AskOpenGameEndPanel(int winnerScore, string winnerSide, int loseScore, string loseSide, bool isTieScore = false)
        {
            bottomPanelManeger.ClosePanels();
            bottomPanelManeger.OpenGameEndPanel(winnerScore, winnerSide, loseScore, loseSide, isTieScore);
        }

        #region ScorePanel Methods

        public void SetupScorePanelPlayersTitels(string leftTitel, string rightTitel)
        {
            bottomPanelManeger.SetupScorePanelPlayersTitels(leftTitel, rightTitel);
        }

        public void ScorePanelToggler()
        {
            bottomPanelManeger.ScorePanelToggler();
        }

        #region Score Management Methods

        public void UpdateGroupsScore(string side, bool isEffectiveFactory, Dictionary<string, bool> releventCategoryNames)
        {
            bottomPanelManeger.UpdateGroupsScore(side, isEffectiveFactory, releventCategoryNames);
        }

        public void UpdateCitizensScore(string side, int panelIndex, int updateTo)
        {
            bottomPanelManeger.UpdateCitizensScore(side, panelIndex, updateTo);
        }

        public void UpdateGoldScore(string side, int panelIndex, int updateTo)
        {
            bottomPanelManeger.UpdateGoldScore(side, panelIndex, updateTo);
        }

        public void UpdateHappinessScore(string side, int panelIndex, int updateTo)
        {
            bottomPanelManeger.UpdateHappinessScore(side, panelIndex, updateTo);
        }

        #endregion

        public void MultiScores()
        {
            bottomPanelManeger.MultiScores();            
        }

        public void UpdateTotal(string side, int panelIndex, int citizensEarn, int goldEarn, int pointsEarn)
        {
            bottomPanelManeger.UpdateTotal(side, panelIndex, citizensEarn,goldEarn,pointsEarn);            
        }

        public void UpdateTimerStatus()
        {
            bottomPanelManeger.UpdateTimerStatus();
        }

        #endregion

        #endregion

        #region UC4 And His Sons Methods

        public bool AskIsBuyableMenuPanelActive(string side)
        {
            return bottomPanelManeger.IsBuyableMenuPanelActive(side);
        }
        
        public void AskToLockRabelInput(string side, int stayUnteracrableFor, bool activeGoldenSine)
        {
            bottomPanelManeger.AskToLockRabelInput(side, stayUnteracrableFor, activeGoldenSine);
        }

        public void AskToResetRabelInput(string side, int resetWith, bool deactiveGoldenSine)
        {
            bottomPanelManeger.AskToResetRabelInput(side, resetWith, deactiveGoldenSine);
        }

        #endregion

        #endregion

       
    }
}
