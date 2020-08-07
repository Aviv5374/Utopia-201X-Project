using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utopia201X.BordeGameScence.UI.Bottom;
using Utopia201X.BordeGameScence.UI.Bottom.Buyable;

namespace Utopia201X.BordeGameScence.UI
{
    public class BottomPanelManeger : MonoBehaviour
    {
        public static BottomPanelManeger instance = null;

        [SerializeField] private Button[] BottomPanelBtns;        
        [SerializeField] private BuyableMenuPanelController buyabelMenuPanelLeft;
        [SerializeField] private BuyableDataPanelController buyabelDataPanelLeft;
        [SerializeField] private BuyableMenuPanelController buyabelMenuPanelRight;        
        [SerializeField] private BuyableDataPanelController buyabelDataPanelRight;

        private GameMenuPanelManager gameMenuPanelManager;        

        private ScorePanelManager ScorePanel;

        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private TextMeshProUGUI[] leftFinalResultsTexts;
        [SerializeField] private TextMeshProUGUI[] rightFinalResultsTexts;
        private Dictionary<string, TextMeshProUGUI[]> finalResultsTexts;
        [SerializeField] private Color gold;

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

            finalResultsTexts = new Dictionary<string, TextMeshProUGUI[]>();
            finalResultsTexts.Add("left", leftFinalResultsTexts);
            finalResultsTexts.Add("right", rightFinalResultsTexts);
        }

        private void Start()
        {            
            StartCoroutine(SetupBuyableMenuPanels());

            gameMenuPanelManager = GameMenuPanelManager.instance;

            ScorePanel = ScorePanelManager.Instance;

            ClosePanels();
        }

        private void Update()
        {
            if (!GameManager.instance.IsItTheLastSecond())
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    if (!gameMenuPanelManager.gameObject.activeSelf)
                    {
                        OpenPanel("menu");
                    }
                    else
                    {
                        ClosePanels();
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    ClosePanels();
                }
            }
        }

        #region Bottom Panel Normal Activitys Methods

        public void OpenPanel(string panelName)
        {
            ClosePanels();
            if (!GameManager.instance.IsItTheLastSecond())
            {
                switch (panelName)
                {
                    case "buyable":                                                                        
                        if (GameManager.instance.GetPlayerSide("Player1") == "left")
                        {
                            buyabelMenuPanelLeft.gameObject.SetActive(true);
                        }
                        else
                        {
                            buyabelMenuPanelRight.gameObject.SetActive(true);
                        }
                        break;
                    case "menu":                        
                        gameMenuPanelManager.gameObject.SetActive(true);
                        gameMenuPanelManager.Pause();
                        break;
                    case "score":                        
                        ScorePanel.gameObject.SetActive(true);
                        gameMenuPanelManager.Pause();
                        break;
                    default:
                        GameManager.instance.PlayErrorSound(false);
                        break;
                }
            }
            else if (endGamePanel.activeSelf)
            {
                ScorePanel.gameObject.SetActive(true);
            }
            else
            {
                GameManager.instance.PlayErrorSound(true);                
            }
        }

        public void ClosePanels()
        {
            gameMenuPanelManager.Resume();
            gameMenuPanelManager.gameObject.SetActive(false);

            buyabelMenuPanelLeft.gameObject.SetActive(false);
            buyabelDataPanelLeft.Deactivate();

            buyabelMenuPanelRight.gameObject.SetActive(false);
            buyabelDataPanelRight.Deactivate();

            ScorePanel.gameObject.SetActive(false);
        }        

        #endregion

        #region UC3 Methods
        
        public void ButtensToggler()
        {
            for (int i = 0; i < BottomPanelBtns.Length; i++)
            {
                BottomPanelBtns[i].interactable = !BottomPanelBtns[i].interactable;
            }
        }
                        
        public void OpenGameEndPanel(int winnerScore, string winnerSide, int loseScore, string loseSide, bool isTieScore = false)
        {
            endGamePanel.SetActive(true);
            finalResultsTexts[winnerSide][0].text = winnerScore.ToString();
            finalResultsTexts[winnerSide][0].color = gold;
            finalResultsTexts[winnerSide][1].text = "WIN";
            finalResultsTexts[winnerSide][1].color = gold;

            finalResultsTexts[loseSide][0].text = loseScore.ToString();

            if (isTieScore)
            {                
                finalResultsTexts[loseSide][0].color = gold;
                finalResultsTexts[loseSide][1].text = "WIN";
                finalResultsTexts[loseSide][1].color = gold;
            }

            BottomPanelBtns[1].interactable = true;
        }

        #region ScorePanel Methods

        public void SetupScorePanelPlayersTitels(string leftTitel, string rightTitel)
        {
            ScorePanel.SetupPlayersTitels(leftTitel, rightTitel);
        }

        public void ScorePanelToggler()
        {
            ScorePanel.ScorePanelAcvtiveToggler();
        }

        #region Score Management Methods

        public void UpdateGroupsScore(string side, bool isEffectiveFactory, Dictionary<string, bool> releventCategoryNames)
        {
            ScorePanel.UpdateGroupsScore(side, isEffectiveFactory, releventCategoryNames);
        }

        public void UpdateCitizensScore(string side, int panelIndex, int updateTo)
        {
            ScorePanel.UpdateCitizensScore(side, panelIndex, updateTo);
        }

        public void UpdateGoldScore(string side, int panelIndex, int updateTo)
        {
            ScorePanel.UpdateGoldScore(side, panelIndex, updateTo);
        }

        public void UpdateHappinessScore(string side, int panelIndex, int updateTo)
        {
            ScorePanel.UpdateHappinessScore(side, panelIndex, updateTo);
        }

        #endregion

        public void MultiScores()
        {
            ScorePanel.MultiCitizensScore();
            ScorePanel.MultiGoldScore();
            ScorePanel.MultiHappinessScore();
        }

        public void UpdateTotal(string side, int panelIndex, int citizensEarn, int goldEarn, int pointsEarn)
        {
            ScorePanel.UpdateCitizensTotal(side, panelIndex, citizensEarn);
            ScorePanel.UpdateGoldTotal(side, panelIndex, goldEarn);
            ScorePanel.UpdateHappinessTotal(side, panelIndex, pointsEarn);
        }

        public void UpdateTimerStatus()
        {
            ScorePanel.UpdateTimerStatus();
        }

        #endregion

        #endregion

        #region Buyable Menu Panels Methods

        private IEnumerator SetupBuyableMenuPanels()
        {
            yield return null;
                       
            if (GameManager.instance.GetPlayerSide("Player1") == "left")
            {
                buyabelMenuPanelLeft.PanelOwner = "Player1";
                buyabelMenuPanelRight.PanelOwner = "Player2";                
            }
            else
            {
                buyabelMenuPanelLeft.PanelOwner = "Player2";
                buyabelMenuPanelRight.PanelOwner = "Player1";               
            }
        }

        #region UC4 And His Sons Methods
        
        public bool IsBuyableMenuPanelActive(string side)
        {
            if (side == "left")
            {
                return buyabelMenuPanelLeft.gameObject.activeInHierarchy;
            }
            else
            {
                return buyabelMenuPanelRight.gameObject.activeInHierarchy;
            }
            
        }

        public void AskToLockRabelInput(string side, int stayUnteracrableFor, bool activeGoldenSine)
        {           
            if (side == "left")
            {
                buyabelMenuPanelLeft.LockRabelInput(stayUnteracrableFor, activeGoldenSine);
            }
            else
            {
                buyabelMenuPanelRight.LockRabelInput(stayUnteracrableFor, activeGoldenSine);
            }
            
        }

        public void AskToResetRabelInput(string side, int lastFortActiveReset, bool deactiveGoldenSine)
        {            
            if (side == "left")
            {
                buyabelMenuPanelLeft.ResetRabelInput(lastFortActiveReset, deactiveGoldenSine);
            }
            else
            {
                buyabelMenuPanelRight.ResetRabelInput(lastFortActiveReset, deactiveGoldenSine);
            }
                
        }

        #endregion

        #endregion
        

    }
}