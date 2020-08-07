using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.SwitchRoundsExstra;
using Utopia201X.BordeGameScence.Buyable.Buildings;
using Utopia201X.BordeGameScence.Buyable.Boats;
using Utopia201X.BordeGameScence.Buyable.Units;
using Utopia201X.BordeGameScence.FactoryScrips.Handlers;
using Utopia201X.BordeGameScence.UI;

namespace Utopia201X.BordeGameScence.FactoryScrips
{
    public class BuyableFactory : MonoBehaviour
    {
        public static BuyableFactory instance = null;
        [SerializeField] private GameObject[] buyableObjectsPrefabs = new GameObject[11];
        private UC5_Handler UC5_Handler;              
        private int relevantIndex;
        private bool isFactoryActive;

        public int RelevantIndex
        {
            get
            {
                return relevantIndex;
            }

            private set
            {
                relevantIndex = value;
            }
        }

        public bool IsFactoryActive
        {
            get
            {
                return isFactoryActive;
            }

            private set
            {
                isFactoryActive = value;
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
        }

        private void Start()
        {
            UC5_Handler = UC5_Handler.instance;                        
            RelevantIndex = -1;
        }

        #region Get and Construct Methods

        private int GetBuyableCost(int buyableObjectIndex)
        {
            switch (buyableObjectIndex)
            {
                case 0:
                    return buyableObjectsPrefabs[buyableObjectIndex].GetComponent<FortScript>().Cost;
                case 1:
                    return buyableObjectsPrefabs[buyableObjectIndex].GetComponent<FactoryScript>().Cost;
                case 2:
                    return buyableObjectsPrefabs[buyableObjectIndex].GetComponent<AcreOfCropsScript>().Cost;
                case 3:
                    return buyableObjectsPrefabs[buyableObjectIndex].GetComponent<SchoolScript>().Cost;
                case 4:
                    return buyableObjectsPrefabs[buyableObjectIndex].GetComponent<HospitalScript>().Cost;
                case 5:
                    return buyableObjectsPrefabs[buyableObjectIndex].GetComponent<HousingProjectScript>().Cost;
                case 6:
                    return buyableObjectsPrefabs[buyableObjectIndex].GetComponent<RebelSoldiersScript>().Cost;
                case 7:                    
                case 9:                    
                    return buyableObjectsPrefabs[buyableObjectIndex].GetComponent<PTBoatScript>().Cost;
                case 8:
                case 10:                    
                    return buyableObjectsPrefabs[buyableObjectIndex].GetComponent<FishingBoatScript>().Cost;                
                default:                    
                    GameManager.instance.PlayErrorSound(false);
                    Debug.Log("ERROR form GetBuyableCost");
                    return -1;
            }
        }

        private string GetBuyableName(int buyableObjectIndex)
        {
            switch (buyableObjectIndex)
            {
                case 0:
                    return buyableObjectsPrefabs[buyableObjectIndex].name;//"Fort"
                case 1:
                    return buyableObjectsPrefabs[buyableObjectIndex].name;//"Factory"
                case 2:
                    return buyableObjectsPrefabs[buyableObjectIndex].name;//"AceOfCorps"
                case 3:
                    return buyableObjectsPrefabs[buyableObjectIndex].name;//"School"
                case 4:
                    return buyableObjectsPrefabs[buyableObjectIndex].name;//"Hospital"
                case 5:
                    return buyableObjectsPrefabs[buyableObjectIndex].name;//"HousingProject"
                case 6:
                    return buyableObjectsPrefabs[buyableObjectIndex].name;//"RebelSoldiers"
                case 7:
                case 9:                    
                    return "PTBoat";
                case 8:
                case 10:                    
                    return "FishingBoat";                
                default:                    
                    GameManager.instance.PlayErrorSound(false);
                    Debug.Log("ERROR form GetBuyableName");
                    return null;
            }
        }

        private void ConstructBuyable(GameObject tempBuyableInsance, string playerName, int buyableObjectIndex)
        {
            switch (buyableObjectIndex)
            {
                case 0:
                    tempBuyableInsance.GetComponent<FortScript>().Costruct(playerName);
                    break;
                case 1:
                    tempBuyableInsance.GetComponent<FactoryScript>().Costruct(playerName);
                    break;
                case 2:
                    tempBuyableInsance.GetComponent<AcreOfCropsScript>().Costruct(playerName);
                    break;
                case 3:
                    tempBuyableInsance.GetComponent<SchoolScript>().Costruct(playerName);
                    break;
                case 4:
                    tempBuyableInsance.GetComponent<HospitalScript>().Costruct(playerName);
                    break;
                case 5:
                    tempBuyableInsance.GetComponent<HousingProjectScript>().Costruct(playerName);
                    break;
                case 6:
                    tempBuyableInsance.GetComponent<RebelSoldiersScript>().Costruct(playerName);
                    break;
                case 7:
                case 9:                    
                    tempBuyableInsance.GetComponent<PTBoatScript>().Costruct(playerName);
                    break;
                case 8:
                case 10:                    
                    tempBuyableInsance.GetComponent<FishingBoatScript>().Costruct(playerName);
                    break;                
                default:
                    GameManager.instance.PlayErrorSound(false);
                    Debug.Log("ERROR form ConstructBuyable");
                    break;
            }
        }

        #endregion

        #region UC4 MSS Methods
        
        public void PrepareBuilder(int buyableHotkeyNumber, string playerName)
        {
            ActivitysManager.Instance.AddActivity(playerName);
            IsFactoryActive = true;                     
            if (GameManager.instance.IsUC4PreConditionRelevent(playerName) && DoThePlayerHaveEnoughMoney(buyableHotkeyNumber, playerName))
            {
                RelevantIndex = buyableHotkeyNumber - 1;
                StartCoroutine("SendToRleventUC", playerName);
            }
            else
            {                
                EndingFactoryActivity(playerName, true);
            }
        }

        private bool DoThePlayerHaveEnoughMoney(int buyableHotkeyNumber, string playerName)
        {                      
            if (GameManager.instance.IsUC4PreConditionRelevent(playerName) && GameManager.instance.GetPlayerCurrentGold(playerName) >= GetBuyableCost(buyableHotkeyNumber - 1))
            {
                return true;
            }
            else
            {                
                return false;
            }
        }

        private IEnumerator SendToRleventUC(string playerName)
        {            
            Transform placement = transform;                                
            //UC5
            if (GameManager.instance.IsUC4PreConditionRelevent(playerName) && RelevantIndex >= 0 && RelevantIndex < 6 )
            {
                UC5_Handler.StartCoroutine("ManageDemoPrefabsSide", playerName);
                                
                while (!UC5_Handler.CanConfirmerBeActivet) { yield return null; }                         

                if (GameManager.instance.IsUC4PreConditionRelevent(playerName) && UC5_Handler.ConstructionAreaConfirmer(playerName, GetBuyableName(RelevantIndex), ref placement))
                {
                    PrepareToBeInstantiate(playerName, placement);
                }
                else
                {                   
                    UC5_Handler.CanConfirmerBeActivet = false;
                    EndingFactoryActivity(playerName, true);
                }               
            }
            //UC7
            else if (GameManager.instance.IsUC4PreConditionRelevent(playerName) && RelevantIndex == 6 && CheckRebelSoldiersConditions(playerName, ref placement))
            {                
                PrepareToBeInstantiate(playerName, placement);
            }
            //UC6
            else if (GameManager.instance.IsUC4PreConditionRelevent(playerName) && RelevantIndex == 7 || RelevantIndex == 8)
            {               
                string islandSide = "";
                placement = GameManager.instance.AskForHarbortPosition(playerName, ref islandSide);                

                if (islandSide == "right")
                {                   
                    RelevantIndex += 2;
                }

                PrepareToBeInstantiate(playerName, placement);
            }
            else
            {                
                EndingFactoryActivity(playerName, true);
            }
        }

        private void PrepareToBeInstantiate(string playerName, Transform placement)
        {            
            GameObject tempBuyableInsance = Instantiate(buyableObjectsPrefabs[RelevantIndex], placement);
            RepositionTempBuyable(tempBuyableInsance);            
            ConstructBuyable(tempBuyableInsance, playerName, RelevantIndex);             
            GameManager.instance.AskFromPlayerToAddBuyableObject(playerName, GetBuyableName(RelevantIndex), tempBuyableInsance);
            GameManager.instance.UpdatePlayer(-GetBuyableCost(RelevantIndex), "gold", playerName);
            EndingFactoryActivity(playerName, false);
        }

        private void RepositionTempBuyable(GameObject tempBuyableInsance)
        {
            if (RelevantIndex == 7 || RelevantIndex == 9)
            {
                tempBuyableInsance.transform.localPosition = new Vector3(0.33f, 0.31f, 0);
            }
            else if (RelevantIndex == 8 || RelevantIndex == 10)
            {
                tempBuyableInsance.transform.localPosition = new Vector3(0.22f, 0.17f, 0);
            }
            else
            {
                tempBuyableInsance.transform.localPosition = new Vector3(0, 0, 0);
            }
        }

        private void EndingFactoryActivity(string playerName, bool playErrorMessage)
        {
            if (playErrorMessage)
            {
                GameManager.instance.PlayErrorSound(true);
            }
            IsFactoryActive = false;
            RelevantIndex = -1;
            ActivitysManager.Instance.RemoveActivity(playerName);
        }

        #endregion

        #region Rebel Methodes
        //UC7
        private bool CheckRebelSoldiersConditions(string playerName, ref Transform placement)
        {
            string OppositPlayerName = GameManager.instance.GetOppositPlayerName(playerName);            

            if (GameManager.instance.IsUC4PreConditionRelevent(playerName))
            {
                return GameManager.instance.CheckRabelCondition(OppositPlayerName, ref placement);
            }
            else
            {
                return false;
            }
        }

        //UC13
        public void InstantiateRabel(string playerName, Transform placement)
        {
            string oppositPlayerName = GameManager.instance.GetOppositPlayerName(playerName);
            GameObject tempRabelInsance = Instantiate(buyableObjectsPrefabs[6], placement);
            tempRabelInsance.transform.localPosition = new Vector3(0, 0, 0);            
            ConstructBuyable(tempRabelInsance, oppositPlayerName, 6);
            GameManager.instance.AskFromPlayerToAddBuyableObject(oppositPlayerName, GetBuyableName(6), tempRabelInsance);           
        }

        #endregion

    }
}
