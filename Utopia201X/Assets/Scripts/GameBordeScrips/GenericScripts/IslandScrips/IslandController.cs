using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.IslandScrips.ConstructionAreaScripts;
using Utopia201X.BordeGameScence.SwitchRoundsExstra;

namespace Utopia201X.BordeGameScence.IslandScrips
{
    public class IslandController : MonoBehaviour
    {
        private ConstructionAreasManeger constructionAreasManeger;
        private BuyableObjectManager buyableObjectManager;
        [SerializeField] private Transform harbor;
        [SerializeField] private string side;

        public Transform Harbor
        {
            get
            {
                return harbor;
            }

            private set
            {
                harbor = value;
            }
        }

        public string Side
        {
            get
            {
                return side;
            }

            private set
            {
                side = value;
            }
        }

        private void Awake()
        {
            constructionAreasManeger = ConstructionAreasManeger.Instance;
            constructionAreasManeger.SetUpConstructionAreasInDictionary(this.Side, gameObject.GetComponentsInChildren<ConstructionArea>());

            buyableObjectManager = BuyableObjectManager.Instance;
        }

        #region Find Legal Transfoms Methods

        //UC5
        public bool IsLegalCoustructionArea(string playerName, string buildingName, ref Transform placement)
        {
            if (GameManager.instance.IsUC4PreConditionRelevent(playerName) && !FortsExplosion(playerName))
            {
                return constructionAreasManeger.FindSelectedConstructionArea(playerName, Side, buildingName, ref placement);
            }
            else
            {
                return false;
            }
        }

        //UC7 && UC13 
        public bool CheckRabelConditionOnisland(string playerName, ref Transform placement, bool activeforUC3)
        {
            int prevBuyable_ID = -1;
            string prevBuyableName = "";
            bool proceedCondition = GameManager.instance.IsUC4PreConditionRelevent(GameManager.instance.GetOppositPlayerName(playerName)) || activeforUC3;            

            if (proceedCondition && !buyableObjectManager.IsThereActiveFort(playerName))
            {
                constructionAreasManeger.ConquerConstructionArea(Side, "RebelSoldiers", ref placement, ref prevBuyable_ID, ref prevBuyableName);

                if (prevBuyable_ID != -1 && prevBuyableName != "")
                {
                    buyableObjectManager.RemoveAndDestroyBuyableObject(playerName, prevBuyableName, prevBuyable_ID);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Add and "Set To Distraction" BuyableObject Methods

        //UC5 & UC6 & UC7 
        public void AskIslnadToAddBuyableObject(string playerName, string buyableObjectName, GameObject buyablePrefab)
        {
            buyableObjectManager.AddBuyableObject(playerName, buyableObjectName, buyablePrefab);

            if (buyableObjectName != "PTBoat " && buyableObjectName != "FishingBoat")
            {
                constructionAreasManeger.SetTakeReleventBuildigId(Side, buyableObjectName, buyableObjectManager.GetBuyableId(playerName, buyableObjectName, buyableObjectManager.GetBuyableListLastIndex(playerName, buyableObjectName)));
            }
        }

        //UC5 & UC6 & UC7 & UC14 
        public void SetBuyableObjectToDistraction(string playerName, string buyableObjectName, int buyable_ID)
        {
            ActivitysManager.Instance.AddActivity(playerName);
            buyableObjectName = buyableObjectManager.ResetGivenBuyableName(buyableObjectName);
            if (buyableObjectName != "PTBoat" && buyableObjectName != "FishingBoat")
            {
                constructionAreasManeger.AddFreedConstructionArea(Side, buyable_ID);
            }
            buyableObjectManager.RemoveAndDestroyBuyableObject(playerName, buyableObjectName, buyable_ID);
            ActivitysManager.Instance.RemoveActivity(playerName);
        }

        #endregion

        //UC5
        private bool FortsExplosion(string playerName)
        {
            List<int> fortsIds = new List<int>();
            ActivitysManager.Instance.AddActivity(playerName);

            if (!buyableObjectManager.FortCountChecker(playerName, ref fortsIds))
            {
                for (int i = 0; i < fortsIds.Count; i++)
                {
                    SetBuyableObjectToDistraction(playerName, "Fort", fortsIds[i]);
                }
                GameManager.instance.ResetPlayersIslands();
                ActivitysManager.Instance.RemoveActivity(playerName);
                return true;
            }
            else
            {
                ActivitysManager.Instance.RemoveActivity(playerName);
                return false;
            }
        }

        public int AskForFactoryCount(string playerName)
        {
            return buyableObjectManager.GetFactoryCount(playerName);
        }

        public void AskToResetFreeConstructionAreas()
        {
            constructionAreasManeger.ResetFreeConstructionAreas(Side);
        }

        #region  UC3

        public PlayerReportDocument MakeAReport(string playerName, PlayerReportDocument reportDocument)
        {            
            if (buyableObjectManager.RabelReport(playerName, ref reportDocument.rabelCount))
            {
                reportDocument.rabelSign = true;
                reportDocument = buyableObjectManager.FortReport(playerName, reportDocument);
            }
            else
            {
                reportDocument.fortSign = buyableObjectManager.IsThereActiveFort(playerName);
            }

            reportDocument = buyableObjectManager.HospitalReport(playerName, reportDocument);

            reportDocument = buyableObjectManager.FactoryReport(playerName, reportDocument);

            reportDocument = buyableObjectManager.FishingBaotReport(playerName, reportDocument);

            buyableObjectManager.AcreOfCropsReport(playerName);

            return reportDocument;
        }

        public void SetRebelDestroyFlags(string playerName, int goldenCounter)
        {
            buyableObjectManager.SetRebelDestroyFlags(playerName, goldenCounter);
        }

        public void ExecutionBuyabelDestraction(string playerName)
        {            
            string[] releventBuyableNames = { "AcreOfCrops", "Factory", "School", "Hospital", "HousingProject", "PTBoat", "FishingBoat", "RebelSoldiers" };

            List<int> buyableIDs = new List<int>();

            for (int i = 0; i < releventBuyableNames.Length; i++)
            {
                buyableIDs = buyableObjectManager.FindAllIdsToDestroyByName(playerName, releventBuyableNames[i]);

                for (int j = 0; buyableIDs != null && j < buyableIDs.Count; j++)
                {
                    SetBuyableObjectToDistraction(playerName, releventBuyableNames[i], buyableIDs[j]);
                }

                buyableIDs.Clear();
            }

        }       

        #endregion

    }
}
