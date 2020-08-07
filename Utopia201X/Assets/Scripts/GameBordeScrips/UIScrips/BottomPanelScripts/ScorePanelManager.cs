using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Utopia201X.BordeGameScence.UI.Bottom
{
    public class ScorePanelManager : MonoBehaviour
    {
        private static ScorePanelManager instance = null;
        
        [SerializeField] private TextMeshProUGUI[] playersTitels = new TextMeshProUGUI[2];

        [SerializeField] private GameObject citizensSummeryPanel;
        //Dictionary<srting = "side", Dictionary<string= "textType" , TextMeshProUGUI[]>> = citizensPanelTexts[left/right][score/multi/total][index]
        private Dictionary<string, Dictionary<string, TextMeshProUGUI[]>> citizensPanelTexts;
        #region citizensPanelTexts arrays
        [SerializeField] private TextMeshProUGUI[] leftCitizensScoreTexts = new TextMeshProUGUI[6];
        [SerializeField] private TextMeshProUGUI[] leftCitizensMultiTexts = new TextMeshProUGUI[6];
        [SerializeField] private TextMeshProUGUI[] leftCitizensTotalTexts = new TextMeshProUGUI[3];
        [SerializeField] private TextMeshProUGUI[] rightCitizensScoreTexts = new TextMeshProUGUI[6];
        [SerializeField] private TextMeshProUGUI[] rightCitizensMultiTexts = new TextMeshProUGUI[6];
        [SerializeField] private TextMeshProUGUI[] rightCitizensTotalTexts = new TextMeshProUGUI[3];
        #endregion
        private readonly int[] citizensMultiNumbers = { 3, -1, 2, 1, 1, 1 };

        [SerializeField] private GameObject goldSummeryPanel;
        //Dictionary<srting = "side", Dictionary<string= "textType" , TextMeshProUGUI[]>> = goldPanelTexts[left/right][score/multi/total][index]
        private Dictionary<string, Dictionary<string, TextMeshProUGUI[]>> goldPanelTexts;
        #region goldPanelTexts arrays
        [SerializeField] private TextMeshProUGUI[] leftGoldScoreTexts = new TextMeshProUGUI[7];
        [SerializeField] private TextMeshProUGUI[] leftGoldMultiTexts = new TextMeshProUGUI[7];
        [SerializeField] private TextMeshProUGUI[] leftGoldTotalTexts = new TextMeshProUGUI[3];
        [SerializeField] private TextMeshProUGUI[] rightGoldScoreTexts = new TextMeshProUGUI[7];
        [SerializeField] private TextMeshProUGUI[] rightGoldMultiTexts = new TextMeshProUGUI[7];
        [SerializeField] private TextMeshProUGUI[] rightGoldTotalTexts = new TextMeshProUGUI[3];
        #endregion
        private readonly int[] goldMultiNumbers = { 4, 1, 15, 2, 1, 1, 2 };

        [SerializeField] private GameObject happinessSummeryPanel;
        //Dictionary<srting = "side", Dictionary<string= "textType" , TextMeshProUGUI[]>> = happinessPanelTexts[left/right][score/multi/total][index]
        private Dictionary<string, Dictionary<string, TextMeshProUGUI[]>> happinessPanelTexts;
        #region happinessPanelTexts arrays
        [SerializeField] private TextMeshProUGUI[] leftHappinessScoreTexts = new TextMeshProUGUI[3];
        [SerializeField] private TextMeshProUGUI[] leftHappinessMultiTexts = new TextMeshProUGUI[3];
        [SerializeField] private TextMeshProUGUI[] leftHappinessTotalTexts = new TextMeshProUGUI[3];
        [SerializeField] private TextMeshProUGUI[] rightHappinesssScoreTexts = new TextMeshProUGUI[3];
        [SerializeField] private TextMeshProUGUI[] rightHappinesssMultiTexts = new TextMeshProUGUI[3];
        [SerializeField] private TextMeshProUGUI[] rightHappinessTotalTexts = new TextMeshProUGUI[3];
        #endregion
        private readonly int[] happinessMultiNumbers = { 8, 8, 8 };

        private Dictionary<string, int> groupCounts;
        private Dictionary<string, int[]> sumCategoryCount;
        private Dictionary<string, int> ownersCounts;

        [SerializeField] private TextMeshProUGUI scoreTimer;
        private int scoreTimerCounter;

        [SerializeField] private Button closeButton;

        internal static ScorePanelManager Instance
        {
            get
            {
                return instance;
            }
            
        }
        
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
                            
            # region citizensPanelTexts setup
            citizensPanelTexts = new Dictionary<string, Dictionary<string, TextMeshProUGUI[]>>();

            citizensPanelTexts.Add("left", new Dictionary<string, TextMeshProUGUI[]>());
            citizensPanelTexts["left"].Add("score", leftCitizensScoreTexts);
            citizensPanelTexts["left"].Add("multi", leftCitizensMultiTexts);
            citizensPanelTexts["left"].Add("total", leftCitizensTotalTexts);

            citizensPanelTexts.Add("right", new Dictionary<string, TextMeshProUGUI[]>());
            citizensPanelTexts["right"].Add("score", rightCitizensScoreTexts);
            citizensPanelTexts["right"].Add("multi", rightCitizensMultiTexts);
            citizensPanelTexts["right"].Add("total", rightCitizensTotalTexts);
            #endregion
            
            # region goldPanelTexts setup
            goldPanelTexts = new Dictionary<string, Dictionary<string, TextMeshProUGUI[]>>();

            goldPanelTexts.Add("left", new Dictionary<string, TextMeshProUGUI[]>());
            goldPanelTexts["left"].Add("score", leftGoldScoreTexts);
            goldPanelTexts["left"].Add("multi", leftGoldMultiTexts);
            goldPanelTexts["left"].Add("total", leftGoldTotalTexts);

            goldPanelTexts.Add("right", new Dictionary<string, TextMeshProUGUI[]>());
            goldPanelTexts["right"].Add("score", rightGoldScoreTexts);
            goldPanelTexts["right"].Add("multi", rightGoldMultiTexts);
            goldPanelTexts["right"].Add("total", rightGoldTotalTexts);
            #endregion
            
            # region happinessPanelTexts setup
            happinessPanelTexts = new Dictionary<string, Dictionary<string, TextMeshProUGUI[]>>();

            happinessPanelTexts.Add("left", new Dictionary<string, TextMeshProUGUI[]>());
            happinessPanelTexts["left"].Add("score", leftHappinessScoreTexts);
            happinessPanelTexts["left"].Add("multi", leftHappinessMultiTexts);
            happinessPanelTexts["left"].Add("total", leftHappinessTotalTexts);

            happinessPanelTexts.Add("right", new Dictionary<string, TextMeshProUGUI[]>());
            happinessPanelTexts["right"].Add("score", rightHappinesssScoreTexts);
            happinessPanelTexts["right"].Add("multi", rightHappinesssMultiTexts);
            happinessPanelTexts["right"].Add("total", rightHappinessTotalTexts);
            #endregion

            groupCounts = new Dictionary<string, int>();
            groupCounts.Add("left", 0);
            groupCounts.Add("right", 0);

            sumCategoryCount = new Dictionary<string, int[]>();
            sumCategoryCount.Add("left", new int[3]);
            sumCategoryCount.Add("right", new int[3]);

            ownersCounts = new Dictionary<string, int>();
            ownersCounts.Add("left", 0);
            ownersCounts.Add("right", 0);

            scoreTimer.gameObject.SetActive(false);
            scoreTimerCounter = 3;                                    
        }
        
        public void SetupPlayersTitels(string leftTitel, string rightTitel)
        {
            playersTitels[0].text = leftTitel;
            playersTitels[1].text = rightTitel;
        }

        #region Useal Management Methods

        public void ScorePanelAcvtiveToggler()
        {
            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                ResetDictionarys(citizensPanelTexts);
                ResetDictionarys(goldPanelTexts);
                ResetDictionarys(happinessPanelTexts);
                ZeroingCounts("left");
                ZeroingCounts("right");
                scoreTimerCounter = 3;
                scoreTimer.gameObject.SetActive(true);
                scoreTimer.text = "Checking Players Status";
                closeButton.interactable = false;
                closeButton.gameObject.SetActive(false);
                SummeryPanelAcvtiveToggler();
            }
            else
            {
                scoreTimer.gameObject.SetActive(false);                
                closeButton.gameObject.SetActive(true);
                closeButton.interactable = true;
                SummeryPanelAcvtiveToggler();
            }            
        }

        private void SummeryPanelAcvtiveToggler()
        {
            citizensSummeryPanel.SetActive(!citizensSummeryPanel.activeSelf);
            goldSummeryPanel.SetActive(!goldSummeryPanel.activeSelf);
            happinessSummeryPanel.SetActive(!happinessSummeryPanel.activeSelf);
        }

        private void ZeroingCounts(string side)
        {
            groupCounts[side] = 0;
            for (int i = 0; i < sumCategoryCount[side].Length; i++)
            {
                sumCategoryCount[side][i] = 0;
            }            
            ownersCounts[side] = 0;
        }

        private void ResetDictionarys(Dictionary<string, Dictionary<string, TextMeshProUGUI[]>> dictionary)
        {
            for (int i = 0; i < dictionary["right"]["score"].Length; i++)
            {
                dictionary["left"]["score"][i].text = "0";
                dictionary["right"]["score"][i].text = "0";
                dictionary["left"]["multi"][i].text = "0";
                dictionary["right"]["multi"][i].text = "0";
            }

            for (int i = 0; i < dictionary["right"]["total"].Length; i++)
            {
                dictionary["left"]["total"][i].text = "0";
                dictionary["right"]["total"][i].text = "0";
            }

        }

        #endregion

        #region Score Management Methods

        public void UpdateGroupsScore(string side, bool isEffectiveFactory, Dictionary<string, bool> releventCategoryNames)
        {
            groupCounts[side]++;
            UpdateCitizensScore(side, 2, groupCounts[side]);
            UpdateGoldScore(side, 2, groupCounts[side]);            

            foreach (KeyValuePair<string,bool> categoryName in releventCategoryNames)
            {
                if (categoryName.Value)
                {
                    switch (categoryName.Key)
                    {
                        case "food":
                            UpdateCategoryScore(side, isEffectiveFactory, 0, 3, 4, 0);                            
                            break;
                        case "house":
                            UpdateCategoryScore(side, isEffectiveFactory, 1, 4, 5, 1);                            
                            break;
                        case "education":
                            UpdateCategoryScore(side, isEffectiveFactory, 2, 5, 6, 2);                            
                            break;
                        default:
                            GameManager.instance.PlayErrorSound(false);
                            Debug.Log("ERROR from UpdateGroupsScore");
                            break;
                    }
                }
            }

            if (isEffectiveFactory)
            {
                ownersCounts[side]++;
                UpdateGoldScore(side, 3, ownersCounts[side]);
            }
        }

        private void UpdateCategoryScore(string side, bool isEffectiveFactory, int categoryIndex, int citizensIndex, int goldIndex, int happinessIndex)
        {
            sumCategoryCount[side][categoryIndex]++;
            UpdateCitizensScore(side, citizensIndex, sumCategoryCount[side][categoryIndex]);
            if (isEffectiveFactory)
            {
                UpdateGoldScore(side, goldIndex, sumCategoryCount[side][categoryIndex]);
            }
            UpdateHappinessScore(side, happinessIndex, sumCategoryCount[side][categoryIndex]);
        }

        public void UpdateCitizensScore(string side, int panelIndex, int updateTo)
        {
            UpdateScore(side, panelIndex, updateTo, citizensPanelTexts);
        }

        public void UpdateGoldScore(string side, int panelIndex, int updateTo)
        {
            UpdateScore(side, panelIndex, updateTo, goldPanelTexts);
        }

        public void UpdateHappinessScore(string side, int panelIndex, int updateTo)
        {
            UpdateScore(side, panelIndex, updateTo, happinessPanelTexts);
        }

        private void UpdateScore(string side, int panelIndex, int updateTo, Dictionary<string, Dictionary<string, TextMeshProUGUI[]>> dictionary)
        {
            dictionary[side]["score"][panelIndex].text = updateTo.ToString();
        }

        #endregion

        #region Multi Management Methods

        public void MultiCitizensScore()
        {
            MultiScore("left", citizensPanelTexts, citizensMultiNumbers);
            MultiScore("right", citizensPanelTexts, citizensMultiNumbers);
        }

        public void MultiGoldScore()
        {
            MultiScore("left", goldPanelTexts, goldMultiNumbers);
            MultiScore("right", goldPanelTexts, goldMultiNumbers);
        }

        public void MultiHappinessScore()
        {
            MultiScore("left", happinessPanelTexts, happinessMultiNumbers);
            MultiScore("right", happinessPanelTexts, happinessMultiNumbers);            
        }

        private void MultiScore(string side, Dictionary<string, Dictionary<string, TextMeshProUGUI[]>> dictionary, int[] multiNumbers)
        {
            int multiNum;

            for (int i = 0; i < dictionary[side]["multi"].Length; i++)
            {
                multiNum = int.Parse(dictionary[side]["score"][i].text) * multiNumbers[i];
                dictionary[side]["multi"][i].text = multiNum.ToString();
            }
        }

        #endregion

        #region Total Management Methods

        public void UpdateCitizensTotal(string side, int panelIndex, int updateTo)
        {
            UpdateTotalPanelTexts(side, panelIndex, updateTo, citizensPanelTexts);
        }

        public void UpdateGoldTotal(string side, int panelIndex, int updateTo)
        {
            UpdateTotalPanelTexts(side, panelIndex, updateTo, goldPanelTexts);
        }

        public void UpdateHappinessTotal(string side, int panelIndex, int updateTo)
        {
            UpdateTotalPanelTexts(side, panelIndex, updateTo, happinessPanelTexts);
        }

        private void UpdateTotalPanelTexts(string side, int panelIndex, int updateTo, Dictionary<string, Dictionary<string, TextMeshProUGUI[]>> dictionary)
        {
            dictionary[side]["total"][panelIndex].text = updateTo.ToString();
        }
            
        #endregion

        #region Timer Management Methods

        public void UpdateTimerStatus()
        {
            if (scoreTimer.text == "Checking Players Status")
            {
                scoreTimer.text = "Realizing Results";
            }
            else
            {
                scoreTimer.text = scoreTimerCounter.ToString();
                StartTimer();
            }
        }
                
        private void StartTimer()
        {            
            InvokeRepeating("TimerLifeCycle", 0.5f, 1);
        }

        private void TimerLifeCycle()
        {
            scoreTimerCounter--;
            scoreTimer.text = scoreTimerCounter.ToString();            
            CheckTimer();
        }

        private void CheckTimer()
        {
            if (scoreTimerCounter <= 0)
            {
                StopTimer();
                ScorePanelAcvtiveToggler();                
            }
        }

        private void StopTimer()
        {
            CancelInvoke("TimerLifeCycle");
        }
               
        #endregion

    }
}
