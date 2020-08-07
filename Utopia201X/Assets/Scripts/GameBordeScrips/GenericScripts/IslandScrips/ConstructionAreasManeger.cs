using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.IslandScrips.ConstructionAreaScripts
{
    public class ConstructionAreasManeger
    {
        private static ConstructionAreasManeger instance = null;
        //Dictionary<string = "island side",Dictionary<string = "list type", list<ConstructionArea>> = list<Free>,list<Taken>
        private Dictionary<string, Dictionary<string, List<ConstructionArea>>> constructionAreas;

        #region Setup Methods

        private ConstructionAreasManeger()
        {
            this.constructionAreas = new Dictionary<string, Dictionary<string, List<ConstructionArea>>>();

            SetConstructionAreasDictionary("left");

            SetConstructionAreasDictionary("right");            
        }

        public static ConstructionAreasManeger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConstructionAreasManeger();
                }
                return instance;
            }
        }

        private void SetConstructionAreasDictionary(string islandSide)
        {
            this.constructionAreas.Add(islandSide, new Dictionary<string, List<ConstructionArea>>());
            this.constructionAreas[islandSide].Add("Free", new List<ConstructionArea>());
            this.constructionAreas[islandSide].Add("Taken", new List<ConstructionArea>());
        }

        public void SetUpConstructionAreasInDictionary(string islandSide, Component[] constructionAreasArry)
        {
            for (int i = 0; i < constructionAreasArry.Length; i++)
                constructionAreasArry[i].GetComponent<ConstructionArea>().myIslandSide = islandSide;

            for (int i = 0; i < constructionAreasArry.Length; i++)
                this.constructionAreas[islandSide]["Free"].Add(constructionAreasArry[i] as ConstructionArea);            
        }

        #endregion

        #region Get Methods 

        public int GetLastIndexByType(string islandSide, string listType)
        {
            return constructionAreas[islandSide][listType].Count - 1;
        }

        public int GetIndexOfCAByTypeAndId(string islandSide, string listType, int id)
        {
            int releventIndex = -1;
            for (int index = 0; index < constructionAreas[islandSide][listType].Count; index++)
            {
                if (constructionAreas[islandSide][listType][index].Id == id)
                {
                    releventIndex = index;
                }
            }
            return releventIndex;
        }

        #endregion

        #region Take Construction Area Methods

        public void AddTakenConstructionArea(string islandSide, string buildingName, ref Transform placement, int index)
        {            
            constructionAreas[islandSide]["Free"][index].ReleventBuyableName = buildingName;            
            placement = constructionAreas[islandSide]["Free"][index].gameObject.transform;

            MoveConstructionAreaToRelevitList(islandSide, "Free", "Taken", index);
        }

        public void SetConqueredFreeConstructionArea(string islandSide, string rabelName, ref Transform placement, int index)
        {            
            constructionAreas[islandSide]["Free"][index].IsSelected = true;            
            AddTakenConstructionArea(islandSide, rabelName, ref placement, index);
        }

        public void SetTakeReleventBuildigId(string islansSide, string buyableObjectName, int newBuildigId)
        {            
            for (int i = 0; i < constructionAreas[islansSide]["Taken"].Count; i++)
            {
                if (constructionAreas[islansSide]["Taken"][i].ReleventBuyable_ID == -1)
                {
                    constructionAreas[islansSide]["Taken"][i].ReleventBuyable_ID = newBuildigId;                   
                }
            }
        }

        #endregion

        #region Free Construction Area Methods

        public void AddFreedConstructionArea(string islandSide, int buyable_ID)
        {            
            int releventIndex = -1;
            for (int i = 0; i < constructionAreas[islandSide]["Taken"].Count; i++)
            {
                if (constructionAreas[islandSide]["Taken"][i].ReleventBuyable_ID == buyable_ID)
                {
                    constructionAreas[islandSide]["Taken"][i].IsSelected = false;
                    constructionAreas[islandSide]["Taken"][i].ReleventBuyableName = "";
                    constructionAreas[islandSide]["Taken"][i].ReleventBuyable_ID = -1;
                    releventIndex = i;
                    break;
                }
            }
            
            if(releventIndex == -1)
            {
                Debug.Log("Error form AddFreedConstructionArea");
                GameManager.instance.PlayErrorSound(false);
            }

            MoveConstructionAreaToRelevitList(islandSide, "Taken", "Free", releventIndex);           
        }

        #endregion

        public void MoveConstructionAreaToRelevitList(string islandSide, string moveFrom, string moveTo, int index)
        {           
            //if moveTo is emty OR if moveFrom's id is bigger from moveTo's id add constructionArea to the end
            if (constructionAreas[islandSide][moveTo].Count == 0 || constructionAreas[islandSide][moveFrom][index].Id > constructionAreas[islandSide][moveTo][GetLastIndexByType(islandSide, moveTo)].Id)
            {               
                constructionAreas[islandSide][moveTo].Add(constructionAreas[islandSide][moveFrom][index]);
            }
            //moveFrom id is smaller from moveTo id add constructionArea to the beginning
            else if (constructionAreas[islandSide][moveFrom][index].Id < constructionAreas[islandSide][moveTo][0].Id)
            {                
                constructionAreas[islandSide][moveTo].Insert(0, constructionAreas[islandSide][moveFrom][index]);
            }
            //else add constructionArea in the middle
            else
            {                
                for (int inerIndex = 0; inerIndex < constructionAreas[islandSide][moveTo].Count; inerIndex++)
                {
                    if (constructionAreas[islandSide][moveFrom][index].Id > constructionAreas[islandSide][moveTo][inerIndex].Id && constructionAreas[islandSide][moveFrom][index].Id < constructionAreas[islandSide][moveTo][inerIndex + 1].Id)
                    {
                        constructionAreas[islandSide][moveTo].Insert(inerIndex + 1, constructionAreas[islandSide][moveFrom][index]);
                        break;
                    }
                }
            }
            constructionAreas[islandSide][moveFrom].RemoveAt(index);           
        }

        //UC5
        public bool FindSelectedConstructionArea(string playerName, string islandSide, string buildingName, ref Transform placemant)
        {           
            for (int index = 0; index < constructionAreas[islandSide]["Free"].Count; index++)
            {
                if (GameManager.instance.IsUC4PreConditionRelevent(playerName) && constructionAreas[islandSide]["Free"][index].IsSelected)
                {
                    AddTakenConstructionArea(islandSide, buildingName, ref placemant, index);
                    return true;
                }

            }
            GameManager.instance.ResetPlayersIslands();
            return false;
        }

        #region Rabel Conquer Construction Area Methods

        public void ConquerConstructionArea(string islandSide, string rabelName, ref Transform placemant, ref int prevBuyable_ID, ref string prevBuyableName)
        {           
            int randomIndex;            
            List<int> conqueredTakenConstructionAreaIndexs = new List<int>();
            conqueredTakenConstructionAreaIndexs = ConqueredTakenConstructionAreaCounter(islandSide, rabelName, conqueredTakenConstructionAreaIndexs);

            if (conqueredTakenConstructionAreaIndexs.Count >= 5 && IsPossibleToReplaceRabels())
            {                
                randomIndex = Random.Range(0, conqueredTakenConstructionAreaIndexs.Count);
                placemant = SetConqueredTakenConstructionArea(islandSide, rabelName,
                    conqueredTakenConstructionAreaIndexs[randomIndex], ref prevBuyable_ID, ref prevBuyableName);                
            }
            else if (constructionAreas[islandSide]["Free"].Count > 0)
            {               
                randomIndex = Random.Range(0, constructionAreas[islandSide]["Free"].Count);
                SetConqueredFreeConstructionArea(islandSide, rabelName, ref placemant, randomIndex);
            }
            else
            {                
                randomIndex = Random.Range(0, constructionAreas[islandSide]["Taken"].Count);
                placemant = TryToConquerTakenConstructionArea(islandSide, rabelName, randomIndex, ref prevBuyable_ID, ref prevBuyableName);
            }

        }

        private bool IsPossibleToReplaceRabels()
        {
            float randomNumder = Random.Range(0.0f, 100.0f);

            if (randomNumder <= 19.0f || randomNumder >= 40.0f && randomNumder <= 59.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<int> ConqueredTakenConstructionAreaCounter(string islandSide, string rabelName, List<int> conqueredTakenConstructionAreaIndexs)
        {
            for (int i = 0; i < constructionAreas[islandSide]["Taken"].Count; i++)
            {
                if (constructionAreas[islandSide]["Taken"][i].ReleventBuyableName == rabelName)
                {
                    conqueredTakenConstructionAreaIndexs.Add(i);
                }
            }

            return conqueredTakenConstructionAreaIndexs;
        }

        private Transform SetConqueredTakenConstructionArea(string islandSide, string rabelName, int randomIndex, ref int prevBuyable_ID, ref string prevBuyableName)
        {
            prevBuyable_ID = constructionAreas[islandSide]["Taken"][randomIndex].ReleventBuyable_ID;
            prevBuyableName = constructionAreas[islandSide]["Taken"][randomIndex].ReleventBuyableName;

            constructionAreas[islandSide]["Taken"][randomIndex].ReleventBuyable_ID = -1;
            constructionAreas[islandSide]["Taken"][randomIndex].ReleventBuyableName = rabelName;            
            return constructionAreas[islandSide]["Taken"][randomIndex].gameObject.transform;
        }

        private Transform TryToConquerTakenConstructionArea(string islandSide, string rabelName, int randomIndex, ref int prevBuyable_ID, ref string prevBuyableName)
        {            
            if (constructionAreas[islandSide]["Taken"][randomIndex].ReleventBuyableName != "Fort")
            {
                return SetConqueredTakenConstructionArea(islandSide, rabelName, randomIndex, ref prevBuyable_ID, ref prevBuyableName);
            }
            randomIndex = Random.Range(0, constructionAreas[islandSide]["Taken"].Count);
            return TryToConquerTakenConstructionArea(islandSide, rabelName, randomIndex, ref prevBuyable_ID, ref prevBuyableName);
        }

        #endregion

        public void ResetFreeConstructionAreas(string islandSide)
        {
            for (int i = 0; i < constructionAreas[islandSide]["Free"].Count; i++)
            {
                constructionAreas[islandSide]["Free"][i].IsSelected = false;                
            }
        }


    }
}