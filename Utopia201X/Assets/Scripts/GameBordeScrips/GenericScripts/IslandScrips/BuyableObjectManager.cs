using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.Buyable;
using Utopia201X.BordeGameScence.Buyable.Units;
using Utopia201X.BordeGameScence.Buyable.Buildings;
using Utopia201X.BordeGameScence.Buyable.Boats;
using Utopia201X.BordeGameScence.SwitchRoundsExstra;

namespace Utopia201X.BordeGameScence.IslandScrips
{
    public class BuyableObjectManager
    {
        private static BuyableObjectManager instance = null;
        //Dictionary<srting = "player name", Dictionary<string= "buyable name" , List<BuyableObject>>> = List<Fort>,List<Factory>,List<AcreOfCrops>,List<School>,List<Hospital>,List<HousingProject>,List<RebelSoldiers>,List<PTBoat>,List<FishingBoat>
        private Dictionary<string, Dictionary<string, List<GameObject>>> buyableObjects;

        #region Setup Methods

        private BuyableObjectManager()
        {
            this.buyableObjects = new Dictionary<string, Dictionary<string, List<GameObject>>>();

            SetUpDictionarys("Player1");

            SetUpDictionarys("Player2");
        }

        public static BuyableObjectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BuyableObjectManager();
                }
                return instance;
            }

        }

        private void SetUpDictionarys(string playerName)
        {
            this.buyableObjects.Add(playerName, new Dictionary<string, List<GameObject>>());

            this.buyableObjects[playerName].Add("Fort", new List<GameObject>());
            this.buyableObjects[playerName].Add("Factory", new List<GameObject>());
            this.buyableObjects[playerName].Add("AcreOfCrops", new List<GameObject>());
            this.buyableObjects[playerName].Add("School", new List<GameObject>());
            this.buyableObjects[playerName].Add("Hospital", new List<GameObject>());
            this.buyableObjects[playerName].Add("HousingProject", new List<GameObject>());
            this.buyableObjects[playerName].Add("RebelSoldiers", new List<GameObject>());
            this.buyableObjects[playerName].Add("PTBoat", new List<GameObject>());
            this.buyableObjects[playerName].Add("FishingBoat", new List<GameObject>());
        }

        #endregion

        #region Get & Ask Methods

        public int GetBuyableListLastIndex(string playerName, string buyableObjectName)
        {
            return buyableObjects[playerName][buyableObjectName].Count - 1;
        }

        private BuyableObjectScript GetBuyableScriptComponent(GameObject buyableObject, string buyableObjectName)
        {
            switch (buyableObjectName)
            {
                case "Fort":
                    return buyableObject.GetComponent<FortScript>();
                case "Factory":
                    return buyableObject.GetComponent<FactoryScript>();
                case "AcreOfCrops":
                    return buyableObject.GetComponent<AcreOfCropsScript>();
                case "School":
                    return buyableObject.GetComponent<SchoolScript>();
                case "Hospital":
                    return buyableObject.GetComponent<HospitalScript>();
                case "HousingProject":
                    return buyableObject.GetComponent<HousingProjectScript>();
                case "RebelSoldiers":
                    return buyableObject.GetComponent<RebelSoldiersScript>();
                case "PTBoat":
                    return buyableObject.GetComponent<PTBoatScript>();
                case "FishingBoat":
                    return buyableObject.GetComponent<FishingBoatScript>();
                default:
                    Debug.Log("ERROR FROM GetBuyableScriptComponent");
                    GameManager.instance.PlayErrorSound(false);
                    return null;
            }
        }

        public int GetBuyableId(string playerName, string buyableObjectName, int index)
        {
            BuyableObjectScript buyableScriptComponent = GetBuyableScriptComponent(buyableObjects[playerName][buyableObjectName][index], buyableObjectName);
            return buyableScriptComponent.BuyableObjectId;
        }

        private int GetBuyableRounOfCreation(string playerName, string buyableObjectName, int index)
        {
            BuyableObjectScript buyableScriptComponent = GetBuyableScriptComponent(buyableObjects[playerName][buyableObjectName][index], buyableObjectName);
            return buyableScriptComponent.RoundOfCreation;
        }

        public string ResetGivenBuyableName(string buyableObjectName)
        {
            switch (buyableObjectName)
            {
                case "Fort(Clone)":
                    buyableObjectName = "Fort";
                    break;
                case "Factory(Clone)":
                    buyableObjectName = "Factory";
                    break;
                case "AcreOfCrops(Clone)":
                    buyableObjectName = "AcreOfCrops";
                    break;
                case "School(Clone)":
                    buyableObjectName = "School";
                    break;
                case "Hospital(Clone)":
                    buyableObjectName = "Hospital";
                    break;
                case "HousingProject(Clone)":
                    buyableObjectName = "HousingProject";
                    break;
                case "RebelSoldiers(Clone)":
                    buyableObjectName = "RebelSoldiers";
                    break;
                case "PTBoatLeft(Clone)":
                case "PTBoatRight(Clone)":
                    buyableObjectName = "PTBoat";
                    break;
                case "FishingBoatLeft(Clone)":
                case "FishingBoatRight(Clone)":
                    buyableObjectName = "FishingBoat";
                    break;
                default:
                    break;
            }

            return buyableObjectName;
        }

        private void ActiveBuyableDestraction(string buyableObjectName, GameObject tempBuyableObject)
        {
            switch (buyableObjectName)
            {
                case "Fort":
                    tempBuyableObject.GetComponent<FortScript>().PrepareToBeDestroy();
                    break;
                case "Factory":
                    tempBuyableObject.GetComponent<FactoryScript>().PrepareToBeDestroy();
                    break;
                case "AcreOfCrops":
                    tempBuyableObject.GetComponent<AcreOfCropsScript>().PrepareToBeDestroy();
                    break;
                case "School":
                    tempBuyableObject.GetComponent<SchoolScript>().PrepareToBeDestroy();
                    break;
                case "Hospital":
                    tempBuyableObject.GetComponent<HospitalScript>().PrepareToBeDestroy();
                    break;
                case "HousingProject":
                    tempBuyableObject.GetComponent<HousingProjectScript>().PrepareToBeDestroy();
                    break;
                case "RebelSoldiers":
                    tempBuyableObject.GetComponent<RebelSoldiersScript>().PrepareToBeDestroy();
                    break;
                case "PTBoat":
                    tempBuyableObject.GetComponent<PTBoatScript>().PrepareToBeDestroy();
                    break;
                case "FishingBoat":
                    tempBuyableObject.GetComponent<FishingBoatScript>().PrepareToBeDestroy();
                    break;
                default:
                    tempBuyableObject.SetActive(false);
                    GameManager.instance.PlayErrorSound(false);
                    break;
            }
        }

        public int GetFactoryCount(string playerName)//OR GetBuyableCount(string playerName, string buyableObjectName)?????
        {
            return buyableObjects[playerName]["Factory"].Count;//OR buyableObjects[playerName][buyableObjectName].Count;??????
        }

        #endregion

        #region Add and Remove Methods

        public void AddBuyableObject(string playerName, string buyableObjectName, GameObject buyableObject)
        {
            buyableObjects[playerName][buyableObjectName].Add(buyableObject);
        }

        public void RemoveAndDestroyBuyableObject(string playerName, string buyableObjectName, int buyable_ID)
        {
            for (int index = 0; index < buyableObjects[playerName][buyableObjectName].Count; index++)
            {
                if (GetBuyableId(playerName, buyableObjectName, index) == buyable_ID)
                {
                    if (buyableObjectName == "Fort" && index == buyableObjects[playerName][buyableObjectName].Count - 1)
                    {
                        GameManager.instance.AskToResetRabelInput(GameManager.instance.GetOppositPlayerName(playerName), LastFortActiveReset(playerName));
                    }
                    GameObject tempBuyableObject = buyableObjects[playerName][buyableObjectName][index];
                    buyableObjects[playerName][buyableObjectName].RemoveAt(index);
                    ActiveBuyableDestraction(buyableObjectName, tempBuyableObject);
                    return;
                }
            }
        }

        #endregion

        #region Fort Methods

        public bool IsThereActiveFort(string playerName)
        {
            for (int index = 0; index < buyableObjects[playerName]["Fort"].Count; index++)
            {
                if (buyableObjects[playerName]["Fort"][index].GetComponent<FortScript>().IsActive())
                {
                    return true;
                }
            }

            return false;
        }

        private int LastFortActiveReset(string playerName)
        {
            if (buyableObjects[playerName]["Fort"].Count >= 2)
                return GetBuyableRounOfCreation(playerName, "Fort", GetBuyableListLastIndex(playerName, "Fort") - 1) - GetBuyableRounOfCreation(playerName, "Fort", GetBuyableListLastIndex(playerName, "Fort"));
            else
                return 5;
        }

        public bool FortCountChecker(string playerName, ref List<int> fortsIds)
        {
            if (buyableObjects[playerName]["Fort"].Count >= 21)
            {
                for (int index = 0; index < buyableObjects[playerName]["Fort"].Count; index++)
                {
                    fortsIds.Add(GetBuyableId(playerName, "Fort", index));
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region UC3 And Reports Methods

        #region Rabel Report Methods

        public bool RabelReport(string playerName, ref int rabelCount)
        {
            rabelCount = buyableObjects[playerName]["RebelSoldiers"].Count;

            if (rabelCount == 0)
            {
                return false;
            }
            else
            {
                RebelSoldiersScript relevanteRebelSoldiers;

                for (int i = 0; i < rabelCount; i++)
                {
                    relevanteRebelSoldiers = buyableObjects[playerName]["RebelSoldiers"][i].GetComponent<RebelSoldiersScript>();

                    if (relevanteRebelSoldiers.IsTimeToBeDestroy())
                    {
                        relevanteRebelSoldiers.NeedToBeDestroy = true;
                        //TODO: replace the line below with suitable visual effect
                        relevanteRebelSoldiers.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                    }

                    FlagBuyableInRabelRadios(relevanteRebelSoldiers);
                }

                return true;
            }

        }

        private void FlagBuyableInRabelRadios(RebelSoldiersScript relevanteRebelSoldiers)
        {
            Collider2D[] colliderInRabelRadios;
            colliderInRabelRadios = Physics2D.OverlapBoxAll(relevanteRebelSoldiers.transform.position, new Vector2(3, 3), 0.0f);
            for (int i = 0; i < colliderInRabelRadios.Length; i++)
            {                
                if (colliderInRabelRadios[i].gameObject.CompareTag("BuyableObject") && ResetGivenBuyableName(colliderInRabelRadios[i].gameObject.name) != "Fort"
                    && ResetGivenBuyableName(colliderInRabelRadios[i].gameObject.name) != "RebelSoldiers")
                {
                    BuyableObjectScript buyableComponentScript = GetBuyableScriptComponent(colliderInRabelRadios[i].gameObject, ResetGivenBuyableName(colliderInRabelRadios[i].gameObject.name));                                       
                    buyableComponentScript.damageFormRebelSoldier++;
                    //TODO: write here code for damage visual effect 
                    buyableComponentScript.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;//???????
                    if (buyableComponentScript.damageFormRebelSoldier >= 2)
                    {
                        buyableComponentScript.NeedToBeDestroy = true;
                        //TODO: replace the line below with suitable visual effect
                        buyableComponentScript.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                    }

                }
            }
            
        }

        public void SetRebelDestroyFlags(string playerName, int goldenCounter)
        {
            BuyableObjectScript buyableScriptComponent;

            for (int i = 0; i < goldenCounter && i < buyableObjects[playerName]["RebelSoldiers"].Count ; i++)
            {
                buyableScriptComponent = GetBuyableScriptComponent(buyableObjects[playerName]["RebelSoldiers"][i], "RebelSoldiers");
                buyableScriptComponent.NeedToBeDestroy = true;
                //TODO: replace the lines below with suitable visual effect
                buyableScriptComponent.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }

        #endregion

        #region Fort Report Methods

        public PlayerReportDocument FortReport(string playerName, PlayerReportDocument reportDocument)
        {            
            if (IsThereActiveFort(playerName))
            {
                reportDocument.fortSign = true;
                SetBuyablesDestoryFlag(playerName, "Factory", false);
                SetBuyablesDestoryFlag(playerName, "AcreOfCrops", false);
                SetBuyablesDestoryFlag(playerName, "School", false);
                SetBuyablesDestoryFlag(playerName, "Hospital", false);
                SetBuyablesDestoryFlag(playerName, "HousingProject", false);
                SetBuyablesDestoryFlag(playerName, "PTBoat", false);
                SetBuyablesDestoryFlag(playerName, "FishingBoat", false);

                SetBuyablesDestoryFlag(playerName, "RebelSoldiers", true);
            }

            return reportDocument;
        }

        private void SetBuyablesDestoryFlag(string playerName, string buyableObjectName, bool setTo)
        {
            BuyableObjectScript buyableScriptComponent;

            for (int index = 0; index < buyableObjects[playerName][buyableObjectName].Count; index++)
            {
                buyableScriptComponent = GetBuyableScriptComponent(buyableObjects[playerName][buyableObjectName][index], buyableObjectName);
                buyableScriptComponent.NeedToBeDestroy = setTo;
                //TODO: replace the lines below with suitable visual effect
                if (!setTo)
                {
                    buyableScriptComponent.damageFormRebelSoldier = 0;
                    buyableScriptComponent.gameObject.GetComponent<SpriteRenderer>().color = buyableScriptComponent.OriginalColor;
                }
                else
                {
                    buyableScriptComponent.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                
            }
        }

        #endregion

        public PlayerReportDocument HospitalReport(string playerName, PlayerReportDocument reportDocument)
        {            
            GameManager.instance.UpdateCitizensScore(playerName, 0, buyableObjects[playerName]["Hospital"].Count);

            reportDocument.AddCitizens(3 * buyableObjects[playerName]["Hospital"].Count);

            return reportDocument;
        }

        public PlayerReportDocument FactoryReport(string playerName, PlayerReportDocument reportDocument)
        {            
            GameManager.instance.UpdateCitizensScore(playerName, 1, buyableObjects[playerName]["Factory"].Count);
            GameManager.instance.UpdateGoldScore(playerName, 0, buyableObjects[playerName]["Factory"].Count);

            reportDocument.AddCitizens(-1 * buyableObjects[playerName]["Factory"].Count);
            reportDocument.AddGold(4 * buyableObjects[playerName]["Factory"].Count);

            return reportDocument;
        }

        public PlayerReportDocument FishingBaotReport(string playerName, PlayerReportDocument reportDocument)
        {            
            GameManager.instance.UpdateGoldScore(playerName, 1, buyableObjects[playerName]["FishingBoat"].Count);

            reportDocument.AddGold(buyableObjects[playerName]["FishingBoat"].Count);

            return reportDocument;
        }

        public void AcreOfCropsReport(string playerName)
        {                    
            AcreOfCropsScript relevantAcreOfCrops;

            for (int i = 0; i < buyableObjects[playerName]["AcreOfCrops"].Count; i++)
            {
                relevantAcreOfCrops = buyableObjects[playerName]["AcreOfCrops"][i].GetComponent<AcreOfCropsScript>();

                if (relevantAcreOfCrops.IsTimeToBeDestroy())
                {
                    relevantAcreOfCrops.NeedToBeDestroy = true;
                    //TODO: replace the lines below with suitable visual effect
                    relevantAcreOfCrops.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }

        public List<int> FindAllIdsToDestroyByName(string playerName, string buyableName)
        {
            List<int> ids = new List<int>();
            BuyableObjectScript buyableScript;
            for (int i = 0; i < buyableObjects[playerName][buyableName].Count; i++)
            {
                buyableScript = GetBuyableScriptComponent(buyableObjects[playerName][buyableName][i], buyableName);
                if (buyableScript.NeedToBeDestroy)
                {
                    ids.Add(buyableScript.BuyableObjectId);
                }
            }
            return ids;
        }

        #endregion

    }
}