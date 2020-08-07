using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utopia201X.Eunms;
using Utopia201X.MainMenusScence.MainMenuManager;
using Utopia201X.BordeGameScence.Managers;
using Utopia201X.BordeGameScence.SwitchRoundsExstra;
using Utopia201X.BordeGameScence.PlayerScrips;


namespace Utopia201X.BordeGameScence
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        private GameType gameType;
        private AIManager ai_Manager;
        private ActivitysManager acvtivitysManager;
        private EnvironmentObjectManager environmentObjectManager;
        private GameBordUIAndMenusManager UIManager;
        private SoundManager soundManager;
        private SwitchBetweenRoundsManager switchBetweenRoundsManager;
        private TimerManager timerManager;
        private List<PlayerController> players;

        #region Setup Game Methods

        private void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //create players list
            players = new List<PlayerController>();
        }

        void Start()
        {
            ai_Manager = AIManager.instance;
            
            acvtivitysManager = ActivitysManager.Instance;

            environmentObjectManager = EnvironmentObjectManager.instance;

            UIManager = GameBordUIAndMenusManager.Instance;

            soundManager = SoundManager.instance;

            switchBetweenRoundsManager = SwitchBetweenRoundsManager.Instance;

            timerManager = TimerManager.instance;

            //set parameters form the Main Menu scene
            MainMenuManager mainMenuManager = MainMenuManager.instance;

            //set TimerManager parameters            
            this.timerManager.SetTimer(mainMenuManager.TempNumOfRounds,mainMenuManager.TempChosenTimePerRound);

            //set game type
            this.gameType = mainMenuManager.TempGameType;

            //move MainMenuManager instance 
            //back to his scene
            Scene menuScene = SceneManager.GetSceneAt(0);
            SceneManager.MoveGameObjectToScene(mainMenuManager.gameObject, menuScene);

            //set the UI tiner
            UpdateUITimer(timerManager.CurrentTimeOfRounnd, timerManager.NumOfRounds);

            //set players controllers
            IntoPlayers();

            //play a dig Suond
            soundManager.PlaySoundByName("begin");

            timerManager.StartTimer();

            //UC15
        }

        //UC2
        private void IntoPlayers()
        {
            //chose a ramdom munber
            float randomNumder = Random.Range(0.0f, 100.0f);

            //chose which will own what island by the random number
            if (randomNumder <= 25.0f || randomNumder > 51.0f && randomNumder <= 75.0f)//ToDo: twick the chosing of the islands to more randomise but still a 50/50 chance
            {
                SetPlayers("left", "right");
            }
            else
            {
                SetPlayers("right", "left");
            }
        }

        private void SetPlayers(string playerOneSide, string playerTwoSide)
        {
            players.Add(new PlayerController(PlayerType.livePlayer, "Player1", playerOneSide));
            UIManager.AskToSetupPlayerPanel(players[0].SumOfHappyPoints, players[0].GetCitizensSumNumber(), players[0].CurrentGoldCount, players[0].Name, playerOneSide, this.gameType.ToString());

            players.Add(new PlayerController(PlayerType.AIPlayer, "Player2", playerTwoSide));
            UIManager.AskToSetupPlayerPanel(players[1].SumOfHappyPoints, players[1].GetCitizensSumNumber(), players[1].CurrentGoldCount, players[1].Name, playerTwoSide, this.gameType.ToString());
        }

        #endregion

        #region TimeManager Methods

        public void UpdateUITimer(int currentTimeOfRounnd, int numOfRounnds)
        {
            UIManager.AskToSetTimerPanel(currentTimeOfRounnd, numOfRounnds);
        }

        public bool IsItTheLastSecond()
        {
            return timerManager.IsTheLastSecond;
        }

        public int GetCurrentRound()
        {
            return timerManager.NumOfRounds;
        }
        
        #endregion

        #region SoundManager Methods 

        public void PlayErrorSound(bool playerError)
        {
            if (playerError)
            {
                soundManager.PlaySoundByName("playerError");
            }
            else
            {
                soundManager.PlaySoundByName("codeError");
            }
        }

        //UC3
        public void PlaySwitchBetweenRoundsSounds(int soundNum)
        {
            switch (soundNum)
            {
                case 1:
                    soundManager.PlaySoundByName("end1");
                    break;
                case 2:
                    soundManager.PlaySoundByName("end2");
                    break;
                case 3:
                    soundManager.PlaySoundByName("begin");
                    break;
                default:
                    Debug.Log("ERROR FORM PlaySwitchBetweenRoundsSounds");
                    PlayErrorSound(false);
                    break;
            }

        }

        #endregion

        #region Player Managment Methods 

        private int GetRelevantPlayerIndex(string playerName)
        {
            switch (playerName)
            {
                case "Player1":
                    return 0;
                case "Player2":
                    return 1;
                default:
                    PlayErrorSound(false);
                    return -1;
            }
        }

        public string GetOppositPlayerName(string playerName)
        {
            if (playerName == players[0].Name)
            {
                return players[1].Name;
            }
            else
            {
                return players[0].Name;
            }
        }

        public int GetPlayerCurrentGold(string playerName)
        {
            return players[GetRelevantPlayerIndex(playerName)].CurrentGoldCount;
        }

        public void UpdatePlayer(int numToUpdate, string whatToUpdate, string playerName)
        {
            int playerIndex = GetRelevantPlayerIndex(playerName);

            switch (whatToUpdate)
            {
                case "points":
                    players[playerIndex].UpdateAttributes(numToUpdate, 0, 0);
                    break;
                case "citizens":
                    players[playerIndex].UpdateAttributes(0, numToUpdate, 0);
                    break;
                case "gold":
                    players[playerIndex].UpdateAttributes(0, 0, numToUpdate);
                    break;
            }
            UIManager.UpdatePlayerPanel(players[playerIndex].SumOfHappyPoints, players[playerIndex].GetCitizensSumNumber(), players[playerIndex].CurrentGoldCount, GetPlayerSide(playerName));
        }
       
        public void UpdatePlayer(int newPoints, int newCitizens, int newGold, string playerName)
        {
            int playerIndex = GetRelevantPlayerIndex(playerName);
            players[playerIndex].UpdateAttributes(newPoints, newCitizens, newGold);
            UIManager.UpdatePlayerPanel(players[playerIndex].SumOfHappyPoints, players[playerIndex].GetCitizensSumNumber(), players[playerIndex].CurrentGoldCount, players[playerIndex].GetIslandSide());
        }

        public bool IsPlayerHasAnActiveGoldenCitizenGroup(string playerName, int index)
        {
            return players[GetRelevantPlayerIndex(playerName)].IsThereAnActiveGoldenCitizenGroup(index);
        }

        public int[] AskForCitizensGroupCatgorysStatus(string playerName, int groupIndex)
        {
            return players[GetRelevantPlayerIndex(playerName)].GetCitizensGroupCatgorysStatus(groupIndex);
        }

        #region Ask Player Methods

        #region Ask Player to Satisfy Methods

        public bool AskPlayerToSortAnSatisfaction(string playerName, string categoryName)
        {
            return players[GetRelevantPlayerIndex(playerName)].CitizenGroupsAnSatisfactionSorter(categoryName);
        }

        public void AskPlayerToSortSatisfaction(string playerName, string categoryName, bool setCategoryTo)
        {
            players[GetRelevantPlayerIndex(playerName)].CitizenGroupsSatisfactionSorter(categoryName, setCategoryTo);
        }

        #endregion

        #region Ask Player to Lock Methods

        public int AskPlayerToLockRandomCategory(string playerName, int citizensGroupIndex, ref string categoryName, ref int CategoryIndex)
        {
            return players[GetRelevantPlayerIndex(playerName)].LockRandomCategory(citizensGroupIndex, ref categoryName, ref CategoryIndex);
        }

        public void AskPlayerToFreeCategory(string playerName, string categoryName, int citizensGroupIndex)
        {
            players[GetRelevantPlayerIndex(playerName)].FreeReleventCategory(categoryName, citizensGroupIndex);
        }

        #endregion

        public void SortOwnershipOfFactorys(string playerName, int factoryCount = 0)
        {
            players[GetRelevantPlayerIndex(playerName)].SortOwnershipOfFactorys(factoryCount);
        }
        
        #endregion

        #endregion

        #region Islands Methods

        public string GetPlayerSide(string playerName)
        {
            return players[GetRelevantPlayerIndex(playerName)].GetIslandSide();
        }

        //UC5
        public bool AskToFindLegalCoustructionArea(string playerName, string buildingName, ref Transform placement)
        {
            if (IsUC4PreConditionRelevent(playerName))
            {
                return players[GetRelevantPlayerIndex(playerName)].FindLegalCoustructionArea(playerName, buildingName, ref placement);
            }
            else
            {
                return false;
            }
        }

        //UC6
        public Transform AskForHarbortPosition(string playerName, ref string islandSide)
        {
            return players[GetRelevantPlayerIndex(playerName)].GetHarborPosition(out islandSide);
        }

        //UC7
        public bool CheckRabelCondition(string playerName, ref Transform placement)
        {
            if (IsUC4PreConditionRelevent(GetOppositPlayerName(playerName)))
            {
                return players[GetRelevantPlayerIndex(playerName)].CheckRabelConditionInPlayer(playerName, ref placement);
            }
            else
            {
                return false;
            }
        }

        //UC13
        public bool CheckRabelCondition(int playerIndex, ref Transform placement)
        {
            return players[playerIndex].CheckRabelConditionInPlayer(players[playerIndex].Name, ref placement, true);
        }

        //UC5 & UC6 & UC7 
        public void AskFromPlayerToAddBuyableObject(string playerName, string buyableObjectName, GameObject buyablePrefab)
        {
            if (buyableObjectName == "RebelSoldiers")
            {
                playerName = GetOppositPlayerName(playerName);
            }
            players[GetRelevantPlayerIndex(playerName)].AskToAddBuyableObject(playerName, buyableObjectName, buyablePrefab);
        }

        //UC5 & UC6 & UC14
        public void AskFromPlaverToSetBuyableToDistraction(string playerName, string buyableObjectName, int buyable_ID)
        {
            players[GetRelevantPlayerIndex(playerName)].AskToSetBuyableToDistraction(playerName, buyableObjectName, buyable_ID);
        }

        public void ResetPlayersIslands()
        {
            for (int index = 0; index < players.Count; index++)
            {
                players[index].ResetIsland();
            }
        }

        #endregion

        #region CitizensGroupPanels Methods

        public void AskToUpdateWorkStatus(string side, int groupPanelIndex, int workStatusNumber)
        {
            UIManager.AskToUpdateWorkStatus(side, groupPanelIndex, workStatusNumber);
        }

        public void AskToUpdateExtraText(string side, string categoryName, int updateTo)
        {
            UIManager.AskToUpdateExtraText(side, categoryName, updateTo);
        }

        public void ActiveCitizensGroupsPanel(string side)
        {
            UIManager.ActiveCitizensGroupsPanel(side);
        }

        #endregion

        #region UC4 And His Sons Methods

        public bool IsBuyableMenuPanelActive(string side)
        {
            return UIManager.AskIsBuyableMenuPanelActive(side);
        }

        public bool IsUC4PreConditionRelevent(string playerName)
        {
            if (!IsItTheLastSecond() && IsBuyableMenuPanelActive(GetPlayerSide(playerName)))
                return true;
            else
                return false;
        }

        //UC5 & UC7
        public void AskToLockRabelInput(string playerName, int stayUnteracrableFor, bool activeGoldenSine)
        {
            UIManager.AskToLockRabelInput(GetPlayerSide(playerName), stayUnteracrableFor, activeGoldenSine);
        }

        public void AskToResetRabelInput(string playerName, int resetWith, bool deactiveGoldenSine = false)
        {
            UIManager.AskToResetRabelInput(GetPlayerSide(playerName), resetWith, deactiveGoldenSine);
        }

        #endregion

        #region UC3 Methods

        public IEnumerator SwitchBetweenRoundsSetUp()
        {            
            // disable UC4 pre-condition
            UIManager.ShutDownBottomPanel();        
            //disable the other UCs pre-condition

            //wait for all the critical activits to end
            while (!acvtivitysManager.IsActiveEmpty())
            {                
                yield return null;
            }

            PlaySwitchBetweenRoundsSounds(1);
            UIManager.ScorePanelToggler();
            yield return new WaitForSeconds(2f);
            switchBetweenRoundsManager.StartSwitchBetweenRounds(players.Count);            
        }

        public PlayerReportDocument PlayerReport(int playerIndex, PlayerReportDocument playerReportDocument)
        {           
            playerReportDocument.playerName = players[playerIndex].Name;          
            playerReportDocument = players[playerIndex].IslandReport(playerReportDocument);                       
            playerReportDocument = players[playerIndex].CitizenGroupsReport(playerReportDocument);            

            return playerReportDocument;
        }

        public void IslandResultsExecution(int playerIndex)
        {
            players[playerIndex].IslandResultsExecution();
        }

        public void SetNextPhase()
        {
            timerManager.ResetTimer();

            if (GetCurrentRound() > 0)
            {
                //activate all UCs pre-condition and all relevant UCs 
                timerManager.StartTimer();
                UIManager.TurnOnBottomPanel();
                //play the "Start Round" Suond        
                PlaySwitchBetweenRoundsSounds(3);
                //next round
            }
            else
            {
                if (players[0].SumOfHappyPoints > players[1].SumOfHappyPoints)
                {
                    UIManager.AskOpenGameEndPanel(players[0].SumOfHappyPoints, players[0].GetIslandSide(), players[1].SumOfHappyPoints, players[1].GetIslandSide());
                }
                else if(players[0].SumOfHappyPoints == players[1].SumOfHappyPoints)
                {
                    UIManager.AskOpenGameEndPanel(players[0].SumOfHappyPoints, players[0].GetIslandSide(), players[1].SumOfHappyPoints, players[1].GetIslandSide(), true);
                }
                else
                {
                    UIManager.AskOpenGameEndPanel(players[1].SumOfHappyPoints, players[1].GetIslandSide(), players[0].SumOfHappyPoints, players[0].GetIslandSide());
                }
                //end game
            }
        }

        #region ScorePanel Methods
        
        public void SetupScorePanelPlayersTitels(string leftTitel, string rightTitel)
        {
            UIManager.SetupScorePanelPlayersTitels(leftTitel, rightTitel);
        }

        #region Score Management Methods

        public void UpdateGroupsScore(string side, bool isEffectiveFactory, Dictionary<string, bool> releventCategoryNames)
        {
            UIManager.UpdateGroupsScore(side, isEffectiveFactory, releventCategoryNames);
        }

        public void UpdateCitizensScore(string playerName, int panelIndex, int updateTo)
        {
            UIManager.UpdateCitizensScore(GetPlayerSide(playerName), panelIndex, updateTo);
        }

        public void UpdateGoldScore(string playerName, int panelIndex, int updateTo)
        {
            UIManager.UpdateGoldScore(GetPlayerSide(playerName), panelIndex, updateTo);
        }

        public void UpdateHappinessScore(string playerName, int panelIndex, int updateTo)
        {
            UIManager.UpdateHappinessScore(GetPlayerSide(playerName), panelIndex, updateTo);
        }

        #endregion

        public void MultiScores()
        {
            UIManager.MultiScores();
        }
       
        #region Total Management Methods

        public void UpdateRoundTotal(int playerIndex, int citizensEarn, int goldEarn, int pointsEarn)
        {
            UIManager.UpdateTotal(players[playerIndex].GetIslandSide(), 0, citizensEarn, goldEarn, pointsEarn);
        }

        public void UpdateGameTotal(int playerIndex, int totalPanelIndex)
        {
            UIManager.UpdateTotal(players[playerIndex].GetIslandSide(), totalPanelIndex, players[playerIndex].GetCitizensSumNumber(), players[playerIndex].CurrentGoldCount, players[playerIndex].SumOfHappyPoints);
        }

        #endregion
        
        public void UpdateScorePanelTimerStatus()
        {
            UIManager.UpdateTimerStatus();
        }
        
        #endregion

        #endregion

        #region Other Methods 

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
           Application.Quit ();
#endif
        }

        #endregion
    }
}
