using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.SwitchRoundsExstra;
using Utopia201X.BordeGameScence.FactoryScrips;

namespace Utopia201X.BordeGameScence.Managers
{
    public class SwitchBetweenRoundsManager : MonoBehaviour
    {
        private static SwitchBetweenRoundsManager instance = null;
        
        private List<PlayerReportDocument> playerReportDocuments;

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

            //create playerReportDocuments list
            playerReportDocuments = new List<PlayerReportDocument>();
        }

        public static SwitchBetweenRoundsManager Instance
        {
            get
            {                
                return instance;
            }
        }
        
        public void StartSwitchBetweenRounds(int playersCount)
        {            
            StartCoroutine(SwitchBetweenRoundsLifeCicyle(playersCount));
        }

        private IEnumerator SwitchBetweenRoundsLifeCicyle(int playersCount)
        {
            #region MakingPlayerReport phase           
            GameManager.instance.PlaySwitchBetweenRoundsSounds(2);
            for (int index = 0; index < playersCount; index++)
            {                
                playerReportDocuments.Add(new PlayerReportDocument());
                playerReportDocuments[index] = MakingPlayerReport(playerReportDocuments[index], index);               
            }                                    
            #endregion

            #region Transition phase              
            GameManager.instance.MultiScores();            
            for (int i = 0; i < playerReportDocuments.Count; i++)
            {
                GameManager.instance.UpdateRoundTotal(i, CheckAddCitizensEarn(playerReportDocuments[i].CitizensEarnThisRound), playerReportDocuments[i].GoldEarnThisRound, playerReportDocuments[i].PointsEarnThisRound);
            }            
            yield return new WaitForSeconds(1.75f);
            #endregion

            #region ExecutionResults phase
            GameManager.instance.PlaySwitchBetweenRoundsSounds(1);
            GameManager.instance.UpdateScorePanelTimerStatus();
            yield return new WaitForSeconds(2f);

            GameManager.instance.PlaySwitchBetweenRoundsSounds(2);
            for (int index = 0; index < playerReportDocuments.Count; index++)
            {
                //part one                
                GameManager.instance.IslandResultsExecution(index);                                             
                NewRabels(playerReportDocuments[index], index);
                
                //part two                
                PlayerResultsExecution(playerReportDocuments[index], index);                                
            }            
            yield return new WaitForSeconds(1.75f);
            #endregion

            #region Finish Up phase           
            GameManager.instance.PlaySwitchBetweenRoundsSounds(1);
            GameManager.instance.UpdateScorePanelTimerStatus();
            yield return new WaitForSeconds(3f);
            playerReportDocuments.Clear();            
            GameManager.instance.SetNextPhase();                       
            #endregion
        }

        private PlayerReportDocument MakingPlayerReport(PlayerReportDocument playerReportDocument, int playerIndex)
        {                              
            return GameManager.instance.PlayerReport(playerIndex, playerReportDocument);
        }

        private void PlayerResultsExecution(PlayerReportDocument playerReportDocument, int playerIndex)
        {
            GameManager.instance.UpdateGameTotal(playerIndex, 1);
            GameManager.instance.UpdatePlayer(playerReportDocument.PointsEarnThisRound, CheckAddCitizensEarn(playerReportDocument.CitizensEarnThisRound), playerReportDocument.GoldEarnThisRound, playerReportDocument.playerName);
            GameManager.instance.SortOwnershipOfFactorys(playerReportDocument.playerName);
            GameManager.instance.UpdateGameTotal(playerIndex, 2);           
        }

        private int CheckAddCitizensEarn(int addCitizensEarn)
        {
            if (addCitizensEarn < 0)
            {
                addCitizensEarn = 0;
            }

            return addCitizensEarn;
        }

        #region UC13 Methods

        private void NewRabels(PlayerReportDocument playerReportDocument, int playerIndex)
        {            
            Transform placement = transform;
            //To check the conditions for the production of rebels
            if (!playerReportDocument.fortSign && !playerReportDocument.goldenGroupSign)
            {                
                //How much rebels need to be produce
                for (int i = 0; i < playerReportDocument.totalRebelsToProduce; i++)
                {                    
                    if(IsPossibleToProduceRabels(playerReportDocument.rabelCount, playerReportDocument.goldenCount)
                        &&  GameManager.instance.CheckRabelCondition(playerIndex, ref placement)) 
                    {                        
                        BuyableFactory.instance.InstantiateRabel(playerReportDocument.playerName, placement);
                    }
                }                
            }                        
        }

        private bool IsPossibleToProduceRabels(int rabelCount, int goldenCount)
        {
            float randomNumder = Random.Range(0.0f, 100.0f) + rabelCount - goldenCount;

            if (randomNumder >= 100.0f || randomNumder >= 26.0f && randomNumder <= 50.0f)
            {
                return true;
            }
            else
            {                
                return false;
            }
        }

        #endregion       

    }
}