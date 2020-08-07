using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.Eunms;
using Utopia201X.BordeGameScence.IslandScrips;
using Utopia201X.BordeGameScence.SwitchRoundsExstra;


namespace Utopia201X.BordeGameScence.PlayerScrips
{
    public class PlayerController
    {
        private PlayerType playerType;
        private string name;
        private IslandController playerIsland;
        private List<CitizenGroup> citizenGroups;
        private int currentGoldCount;
        private int sumOfHappyPoints;

        public PlayerType PlayerType
        {
            get
            {
                return playerType;
            }

            private set
            {
                playerType = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            private set
            {
                name = value;
            }
        }

        public int CurrentGoldCount
        {
            get
            {
                return currentGoldCount;
            }

            private set
            {
                currentGoldCount = value;
            }
        }

        public int SumOfHappyPoints
        {
            get
            {
                return sumOfHappyPoints;
            }

            private set
            {
                sumOfHappyPoints = value;
            }
        }

        public PlayerController(PlayerType type, string name, string islandSide)
        {
            this.PlayerType = type;
            this.Name = name;

            SetupIsland(islandSide);

            citizenGroups = new List<CitizenGroup>();
            //2 full groups
            citizenGroups.Add(new CitizenGroup(Name, 500));
            citizenGroups.Add(new CitizenGroup(Name, 500));

            CurrentGoldCount = 100;
            SumOfHappyPoints = 0;
        }

        #region Island Ask Methods

        private void SetupIsland(string islansSide)
        {
            switch (islansSide)
            {
                case "left":
                    playerIsland = GameObject.Find("LeftIsland").GetComponent<IslandController>();
                    break;
                case "right":
                    playerIsland = GameObject.Find("RightIsland").GetComponent<IslandController>();
                    break;
                default:
                    GameManager.instance.PlayErrorSound(false);
                    break;
            }
        }

        public string GetIslandSide()
        {
            return playerIsland.Side;
        }

        //UC5 
        public bool FindLegalCoustructionArea(string playerName, string buildingName, ref Transform placement)
        {
            if (GameManager.instance.IsUC4PreConditionRelevent(playerName))
            {
                return playerIsland.IsLegalCoustructionArea(playerName, buildingName, ref placement);
            }
            else
            {
                return false;
            }
        }

        //UC6
        public Transform GetHarborPosition(out string islandSide)
        {
            islandSide = GetIslandSide();
            return playerIsland.Harbor;
        }

        //UC7 && UC 13
        public bool CheckRabelConditionInPlayer(string playerName, ref Transform placement, bool activeForUC3 = false)
        {
            bool proceedCondition = GameManager.instance.IsUC4PreConditionRelevent(GameManager.instance.GetOppositPlayerName(playerName)) || activeForUC3;            

            if (proceedCondition && !IsThereAnActiveGoldenCitizenGroup() && IsThereEnoughtFreeCategory())
            {                
                return playerIsland.CheckRabelConditionOnisland(playerName, ref placement, activeForUC3);
            }
            else
            {
                return false;
            }
        }

        //UC5 & UC6 & UC7 
        public void AskToAddBuyableObject(string playerName, string buyableObjectName, GameObject buyablePrefab)
        {
            playerIsland.AskIslnadToAddBuyableObject(playerName, buyableObjectName, buyablePrefab);
        }

        //UC5 & UC6 & UC7 & UC14
        public void AskToSetBuyableToDistraction(string playerName, string buyableObjectName, int buyable_ID)
        {
            playerIsland.SetBuyableObjectToDistraction(playerName, buyableObjectName, buyable_ID);
        }

        //UC4
        public void ResetIsland()
        {
            playerIsland.AskToResetFreeConstructionAreas();
        }

        #endregion

        #region Citizens Groups Methods      

        #region Manage Citizens Groups Methods

        public int GetCitizensSumNumber()
        {
            int citizensSumNum = 0;
            //all the citizenGroups apart from the last one has NumOfCitizenInGroup for 500
            //so we mult them with 500 to get their citizensSumNum
            citizensSumNum += (citizenGroups.Count - 1) * 500;
            //add to local citizensSumNum the NumOfCitizenInGroup of the last citizenGroups
            citizensSumNum += citizenGroups[citizenGroups.Count - 1].NumOfCitizenInGroup;
            return citizensSumNum;
        }

        public int[] GetCitizensGroupCatgorysStatus(int groupIndex)
        {
            return citizenGroups[groupIndex].GetCatgorysStatus();
        }

        private void SortCitisensInGoups(int newCitizensToAdd)
        {            
            int remaindCitizens = -1;

            if (newCitizensToAdd <= 0 || citizenGroups[citizenGroups.Count - 1].NumOfCitizenInGroup < 500)
            {
                remaindCitizens = citizenGroups[citizenGroups.Count - 1].AddCitizenToGruop(newCitizensToAdd);
            }

            if (remaindCitizens != 0)
            {
                citizenGroups.Add(new CitizenGroup(Name, 0));
                GameManager.instance.ActiveCitizensGroupsPanel(GetIslandSide());
                if (remaindCitizens > 0)
                {
                    SortCitisensInGoups(remaindCitizens);
                }
                else
                {
                    SortCitisensInGoups(newCitizensToAdd);
                }
            }

            //ToCheck: 
            //I realy want this code/option to be relevent or balence it to not be possible?
            //what happen went a Citizens Group removed?
            //what happen went player don't have any Citizens Group?            
            /*else if (citizenGroups[citizenGroups.Count - 1].NumOfCitizenInGroup <= 0)
            {
                remaindCitizens = citizenGroups[citizenGroups.Count - 1].NumOfCitizenInGroup;
                citizenGroups.RemoveAt(citizenGroups.Count - 1);
                SortCitisensInGoups(remaindCitizens);
            }
            */
        }

        //UC4 and maybe UC3        
        public void SortOwnershipOfFactorys(int factoryCount)
        {
            factoryCount += playerIsland.AskForFactoryCount(Name);

            if (factoryCount >= 1)
            {
                for (int i = 0; i < factoryCount && i < citizenGroups.Count; i++)
                {
                    citizenGroups[i].OnwerOfFactory = true;
                    GameManager.instance.AskToUpdateWorkStatus(GetIslandSide(), i, 1);
                }

                if (factoryCount < citizenGroups.Count)
                {
                    for (int i = factoryCount - 1; i < citizenGroups.Count; i++)
                    {
                        citizenGroups[i].OnwerOfFactory = false;
                        GameManager.instance.AskToUpdateWorkStatus(GetIslandSide(), i, 2);
                    }
                }
            }
            else
            {
                for (int i = 0; i < citizenGroups.Count; i++)
                {
                    citizenGroups[i].OnwerOfFactory = false;
                    GameManager.instance.AskToUpdateWorkStatus(GetIslandSide(), i, 3);
                }
            }
        }

        #endregion

        #region Satisfy Citizens Groups Methods

        public bool CitizenGroupsAnSatisfactionSorter(string categoryName)
        {
            for (int index = 0; index < citizenGroups.Count; index++)
            {
                if (!citizenGroups[index].IsCategorySatisfy(categoryName))
                {
                    citizenGroups[index].CatgorySatisfactionSorter(categoryName, true);

                    if (IsThereAnActiveGoldenCitizenGroup())
                    {
                        GameManager.instance.AskToLockRabelInput(GameManager.instance.GetOppositPlayerName(Name), 1, true);
                    }
                    else
                    {
                        GameManager.instance.AskToResetRabelInput(GameManager.instance.GetOppositPlayerName(Name), 1, true);
                    }

                    return citizenGroups[index].IsCategorySatisfy(categoryName);
                }
            }
            return false;
        }

        public void CitizenGroupsSatisfactionSorter(string categoryName, bool setCategoryTo)
        {
            for (int index = citizenGroups.Count - 1; index > -1; index--)
            {
                if (citizenGroups[index].IsCategorySatisfy(categoryName))
                {
                    citizenGroups[index].CatgorySatisfactionSorter(categoryName, setCategoryTo);

                    if (IsThereAnActiveGoldenCitizenGroup())
                    {
                        GameManager.instance.AskToLockRabelInput(GameManager.instance.GetOppositPlayerName(Name), 1, true);
                    }
                    else
                    {
                        GameManager.instance.AskToResetRabelInput(GameManager.instance.GetOppositPlayerName(Name), 1, true);
                    }

                    return;
                }
            }
        }

        public bool IsThereAnActiveGoldenCitizenGroup()
        {
            for (int i = 0; i < citizenGroups.Count; i++)
            {
                if (citizenGroups[i].IsActiveGolden())
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsThereAnActiveGoldenCitizenGroup(int index)
        {
            if (citizenGroups[index].IsActiveGolden())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Lock Citizens Groups Methods

        public int LockRandomCategory(int citizensGroupIndex, ref string categoryName, ref int CategoryIndex)
        {
            int lockTriesCounter = 1;
            citizensGroupIndex = Random.Range(0, citizenGroups.Count);
            lockTriesCounter = citizenGroups[citizensGroupIndex].SelectCatergoryToLock(lockTriesCounter, ref categoryName);
            //lockTriesCounter = 16 mean to represent 4 possible results of 4 Lock attempts multi 4 cycle of tries 
            if (lockTriesCounter >= 16)
            {
                return LockRandomCategory(citizensGroupIndex, ref categoryName, ref CategoryIndex);
            }

            if (CategoryIndex == -1)
            {
                CategoryIndex = citizenGroups[citizensGroupIndex].GetCatgoryIndex(categoryName);
            }


            return citizensGroupIndex;
        }

        public void FreeReleventCategory(string categoryName, int citizensGroupIndex)
        {
            citizenGroups[citizensGroupIndex].FreeCategory(categoryName);
        }

        private bool IsThereEnoughtFreeCategory()
        {
            int sumLockCategorys = 0;

            for (int i = 0; i < citizenGroups.Count; i++)
            {
                sumLockCategorys = citizenGroups[i].LockCategoryCounter(sumLockCategorys);
            }

            if ((citizenGroups.Count * 3) - sumLockCategorys > 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region UC3

        public PlayerReportDocument IslandReport(PlayerReportDocument reportDocument)
        {           
            return playerIsland.MakeAReport(Name, reportDocument);
        }

        #region Citizen Groups Reports Methods

        public PlayerReportDocument CitizenGroupsReport(PlayerReportDocument reportDocument)
        {           
            reportDocument.totalRebelsToProduce = GroupsCategorysStatusReport();
            reportDocument.goldenGroupSign = GoldenGroupsReport(ref reportDocument.goldenCount);
            reportDocument = GroupsCategorysCounterReport(reportDocument);

            return reportDocument;
        }

        private int GroupsCategorysStatusReport()
        {
            int[] groupCategorysStatus;
            int unsatisfactionCategorys = 0;
            int lockedCategorys = 0;

            //Gather all Group's Catgorys Status
            for (int i = 0; i < citizenGroups.Count; i++)
            {
                groupCategorysStatus = GetCitizensGroupCatgorysStatus(i);

                //Check how much unsatisfactionCategorys and lockedCategorys there are
                for (int j = 0; j < groupCategorysStatus.Length; j++)
                {
                    switch (groupCategorysStatus[j])
                    {
                        case 1:
                            break;
                        case 2:
                            lockedCategorys++;
                            break;
                        case 3:
                            unsatisfactionCategorys++;
                            break;
                        default:
                            GameManager.instance.PlayErrorSound(false);
                            Debug.Log("ERROR from GroupsCategorysStatusReport");
                            break;
                    }
                }
            }

            //Divide by 3             
            unsatisfactionCategorys /= 3;
            lockedCategorys /= 3;

            //Send the bigger num
            if (lockedCategorys > unsatisfactionCategorys)
            {
                return lockedCategorys;
            }
            else
            {
                return unsatisfactionCategorys;
            }

        }

        private bool GoldenGroupsReport(ref int goldenCounter)
        {
            int activeGoldenCounter = 0;

            for (int i = 0; i < citizenGroups.Count; i++)
            {
                if (citizenGroups[i].IsGolden())
                {
                    goldenCounter++;
                }

                if (citizenGroups[i].IsActiveGolden())
                {
                    activeGoldenCounter++;
                }
            }

            playerIsland.SetRebelDestroyFlags(Name, activeGoldenCounter);

            return IsThereAnActiveGoldenCitizenGroup();
        }

        private PlayerReportDocument GroupsCategorysCounterReport(PlayerReportDocument reportDocument)
        {
            int sumOfReleventCategory;
            //Key = CatgoryNames, Value = relevent
            Dictionary<string, bool> releventCategoryNames = new Dictionary<string, bool>();
            releventCategoryNames.Add("food", false);
            releventCategoryNames.Add("house", false);
            releventCategoryNames.Add("education", false);

            for (int i = 0; i < citizenGroups.Count; i++)
            {
                if (citizenGroups[i].NumOfCitizenInGroup == 500)
                {
                    //CategoryCounter
                    sumOfReleventCategory = citizenGroups[i].ReleventCategoryCounter(reportDocument.rabelSign, reportDocument.fortSign, ref releventCategoryNames);
                    reportDocument.AddCitizens(1 * sumOfReleventCategory);
                    reportDocument.AddPoins(8 * sumOfReleventCategory);
                    reportDocument.AddCitizens(2);

                    //EffectivenessCheck
                    reportDocument.AddGold(citizenGroups[i].EffectivenessCheck(sumOfReleventCategory, releventCategoryNames));
                    reportDocument.AddGold(15);

                    GameManager.instance.UpdateGroupsScore(GetIslandSide(), citizenGroups[i].OnwerOfFactory, releventCategoryNames);

                    //Reset
                    sumOfReleventCategory = 0;
                    releventCategoryNames["food"] = false;
                    releventCategoryNames["house"] = false;
                    releventCategoryNames["education"] = false;
                }
            }

            return reportDocument;
        }

        #endregion

        public void IslandResultsExecution()
        {            
            playerIsland.ExecutionBuyabelDestraction(Name);
        }

        #endregion

        #region UpdateAttributes Methods 

        public void UpdateAttributes(int newPionts, int newCitizens, int newGold)
        {
            AddPoints(newPionts);

            SortCitisensInGoups(newCitizens);

            AddGold(newGold);
        }

        private void AddPoints(int newPoints)
        {            
            if (newPoints >= 0)
            {
                SumOfHappyPoints += newPoints;
            }
            else
            {
                GameManager.instance.PlayErrorSound(false);
                Debug.Log("ERROR form AddPoints");
            }
        }

        private void AddGold(int newGold)
        {            
            if ((CurrentGoldCount + newGold) > -1)
            {
                CurrentGoldCount += newGold;
            }
            else
            {
                GameManager.instance.PlayErrorSound(false);
                Debug.Log("ERROR form AddGold");
            }
        }

        #endregion

    }
}
