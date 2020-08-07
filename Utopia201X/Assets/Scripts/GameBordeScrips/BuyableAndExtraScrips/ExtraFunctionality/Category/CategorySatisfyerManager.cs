using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.SwitchRoundsExstra;

namespace Utopia201X.BordeGameScence.CategoryManagers
{
    public class CategorySatisfyerManager
    {
        private class ExtraCategorySatisfyer
        {
            private string name;
            private int satisfyerID;

            public ExtraCategorySatisfyer(string name, int satisfyerID)
            {
                this.Name = name;
                this.SatisfyerID = satisfyerID;
            }
            
            internal string Name
            {
                get
                {
                    return name;
                }

                set
                {
                    name = value;
                }
            }
            
            internal int SatisfyerID
            {
                get
                {
                    return satisfyerID;
                }

                set
                {
                    satisfyerID = value;
                }
            }
        }

        private static CategorySatisfyerManager instance = null;
        //Dictionary<string playerName, Dictionary<string SatisfyerType,List <ExtraCategorySatisfyer>>>
        private Dictionary<string, Dictionary<string, List<ExtraCategorySatisfyer>>> extraSatisfyers;

        #region SetUp Methods

        private CategorySatisfyerManager()
        {
            extraSatisfyers = new Dictionary<string, Dictionary<string, List<ExtraCategorySatisfyer>>>();

            SetExtraSatisfyers("Player1");           

            SetExtraSatisfyers("Player2");            
        }

        public static CategorySatisfyerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategorySatisfyerManager();
                }
                return instance;
            }

        }

        private void SetExtraSatisfyers(string playerName)
        {
            extraSatisfyers.Add(playerName, new Dictionary<string, List<ExtraCategorySatisfyer>>());
            extraSatisfyers[playerName].Add("food", new List<ExtraCategorySatisfyer>());
            extraSatisfyers[playerName].Add("house", new List<ExtraCategorySatisfyer>());
            extraSatisfyers[playerName].Add("education", new List<ExtraCategorySatisfyer>());
        }

        #endregion

        public void FinedAndSatisfyCategory(string playerName, string categoryName, bool hasHarbor, int satisfyerID)
        {
            ActivitysManager.Instance.AddActivity(playerName);

            //the real deal
            if (GameManager.instance.AskPlayerToSortAnSatisfaction(playerName, categoryName))
            {
                ActivitysManager.Instance.RemoveActivity(playerName);
                return;
            }

            //save extra in the relvent list
            AddExtraSatisfyer(playerName, categoryName, hasHarbor, satisfyerID);

            ActivitysManager.Instance.RemoveActivity(playerName);
        }

        private void AddExtraSatisfyer(string playerName, string categoryName, bool hasHarbor, int satisfyerID)
        {

            switch (categoryName)
            {
                case "food":
                    if (hasHarbor)
                    {
                        extraSatisfyers[playerName][categoryName].Add(new ExtraCategorySatisfyer("FishingBoat", satisfyerID));
                    }
                    else
                    {
                        extraSatisfyers[playerName][categoryName].Add(new ExtraCategorySatisfyer("AcreOfCrops", satisfyerID));
                    }                    
                    break;
                case "house":
                    extraSatisfyers[playerName][categoryName].Add(new ExtraCategorySatisfyer("HousingProject", satisfyerID));
                    break;
                case "education":
                    extraSatisfyers[playerName][categoryName].Add(new ExtraCategorySatisfyer("School", satisfyerID));
                    break;
                default:                    
                    GameManager.instance.PlayErrorSound(false);
                    break;
            }

            GameManager.instance.AskToUpdateExtraText(GameManager.instance.GetPlayerSide(playerName), categoryName, extraSatisfyers[playerName][categoryName].Count);
        }

        public bool ExtraSatisfyerFinderAndRemover(string playerName, string satisfyerName, int satisfyerID, out string typeOfCategorySatisfyer)
        {
            ActivitysManager.Instance.AddActivity(playerName);
            
            //find in which list to run on
            switch (satisfyerName)
            {
                case "AcreOfCrops":
                case "FishingBoat":
                    typeOfCategorySatisfyer = "food";
                    break;
                case "HousingProject":
                    typeOfCategorySatisfyer = "house";
                    break;
                case "School":
                    typeOfCategorySatisfyer = "education";
                    break;
                default:
                    typeOfCategorySatisfyer = null;
                    GameManager.instance.PlayErrorSound(false);
                    break;
            }
                   
            //checking if the relevent BuyableObject is an extra
            for (int i = 0; i < extraSatisfyers[playerName][typeOfCategorySatisfyer].Count; i++)
            {
                if (extraSatisfyers[playerName][typeOfCategorySatisfyer][i].SatisfyerID == satisfyerID)
                {
                    extraSatisfyers[playerName][typeOfCategorySatisfyer].RemoveAt(i);
                    GameManager.instance.AskToUpdateExtraText(GameManager.instance.GetPlayerSide(playerName), typeOfCategorySatisfyer, extraSatisfyers[playerName][typeOfCategorySatisfyer].Count);
                    ActivitysManager.Instance.RemoveActivity(playerName);
                    // it is an extra
                    return true;
                }
            }
            ActivitysManager.Instance.RemoveActivity(playerName);
            // it is not an extra
            return false;
        }

        public bool ExtraSatisfyerGiver(string playerName, string categoryName)
        {
            if (extraSatisfyers[playerName][categoryName].Count > 0)
            {
                extraSatisfyers[playerName][categoryName].RemoveAt(extraSatisfyers[playerName][categoryName].Count - 1);
                GameManager.instance.AskToUpdateExtraText(GameManager.instance.GetPlayerSide(playerName), categoryName, extraSatisfyers[playerName][categoryName].Count);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}