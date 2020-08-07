using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.SwitchRoundsExstra;
using Utopia201X.BordeGameScence.CategoryManagers;


namespace Utopia201X.BordeGameScence.PlayerScrips
{
    public class CitizenGroup
    {
        private class Category
        {
            private bool satisfaction;
            private bool locked;

            public Category(bool satisfation, bool locked)
            {
                this.satisfaction = satisfation;
                this.locked = locked;
            }
           
            internal bool Satisfaction
            {
                get
                {
                    return satisfaction;
                }

                set
                {
                    satisfaction = value;
                }
            }
            
            internal bool Locked
            {
                get
                {
                    return locked;
                }

                set
                {
                    locked = value;
                }
            }
        }

        private int numOfCitizenInGroup;
        //Dictionary<String name, Category>
        private Dictionary<string, Category> categorys;        
        private bool onwerOfFactory;
        private bool golden;
        private int becomeGoldenInTurnNumber;

        public CitizenGroup(string ownerName, int numOfCitizenInGroup)
        {
            this.categorys = new Dictionary<string, Category>();

            this.categorys.Add("food", new Category(CategoryManager.instance.GetExtraSatisfyer(ownerName, "food"), false));
            this.categorys.Add("house", new Category(CategoryManager.instance.GetExtraSatisfyer(ownerName, "house"), false));
            this.categorys.Add("education", new Category(CategoryManager.instance.GetExtraSatisfyer(ownerName, "education"), false));

            this.numOfCitizenInGroup = numOfCitizenInGroup;
            OnwerOfFactory = false;
            Golden = IsGolden();
        }
           
        internal int NumOfCitizenInGroup
        {
            get
            {
                return numOfCitizenInGroup;
            }

            private set
            {
                numOfCitizenInGroup = value;
            }
        }
                
        internal bool OnwerOfFactory
        {
            get
            {
                return onwerOfFactory;
            }

            set
            {
                onwerOfFactory = value;
            }
        }
        
        internal bool Golden
        {
            get
            {
                return golden;
            }

            set
            {
                golden = value;
            }
        }
        
        internal int BecomeGoldenInTurnNumber
        {
            get
            {
                return becomeGoldenInTurnNumber;
            }

            set
            {
                becomeGoldenInTurnNumber = value;
            }
        }
        
        #region Catgory Management Methods

        public int GetCatgoryIndex(string categoryName)
        {
            switch (categoryName)
            {

                case "food":
                    return 0;
                case "house":
                    return 1;
                case "education":
                    return 2;
                default:
                    GameManager.instance.PlayErrorSound(false);
                    return -1;
            }
        }

        public int[] GetCatgorysStatus()
        {
            int[] CatgorysStatus = {3, 3, 3};
            foreach (KeyValuePair<string, Category> category in categorys)
            {
                if (category.Value.Satisfaction)
                {
                    CatgorysStatus[GetCatgoryIndex(category.Key)] = 1;
                }

                if (category.Value.Locked)
                {
                    CatgorysStatus[GetCatgoryIndex(category.Key)] = 2;
                }                
            }

            return CatgorysStatus;
        }

        #region Category Satisfy Methodes

        public bool IsCategorySatisfy(string categoryName)
        {
            return categorys[categoryName].Satisfaction;
        }

        public void CatgorySatisfactionSorter(string categoryName, bool setCategoryTo)
        {
            categorys[categoryName].Satisfaction = setCategoryTo;
            this.Golden = IsGolden();
        }

        public bool IsGolden()
        {
            if (!categorys["food"].Satisfaction || !categorys["house"].Satisfaction || !categorys["education"].Satisfaction)
            {
                becomeGoldenInTurnNumber = -1;
                return false;
            }

            if (categorys["food"].Satisfaction && categorys["house"].Satisfaction && categorys["education"].Satisfaction && becomeGoldenInTurnNumber == -1)
            {
                becomeGoldenInTurnNumber = GameManager.instance.GetCurrentRound();
                return true;
            }

            return golden;
        }
            
        public bool IsActiveGolden()
        {
            if (IsGolden() && becomeGoldenInTurnNumber - GameManager.instance.GetCurrentRound() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Category Lock Methodes

        public bool IsCategoryLocked(string categoryName)
        {
            return categorys[categoryName].Locked;
        }

        public int SelectCatergoryToLock(int triesCounter, ref string categoryName)
        {
            int radomCategoryNumber = Random.Range(0, 3) + 1;

            switch (radomCategoryNumber)
            {
                case 1:
                    categoryName = "food";
                    break;
                case 2:
                    categoryName = "house";
                    break;
                case 3:
                    categoryName = "education";
                    break;
                default:
                    categoryName = "";
                    return SelectCatergoryToLock(++triesCounter, ref categoryName);
            }

            return LockCatergoryOpthions(triesCounter, ref categoryName);
        }

        private int LockCatergoryOpthions(int triesCounter, ref string categoryName)
        {
            if (!categorys[categoryName].Locked)
            {
                categorys[categoryName].Locked = true;
            }
            //triesCounter = 16 mean to represent 4 possible results of 4 Lock attempts multi 4 cycle of tries
            else if (triesCounter >= 16)
            {
                categoryName = "";
            }
            else
            {
                return SelectCatergoryToLock(++triesCounter, ref categoryName);
            }

            return triesCounter;
        }

        public void FreeCategory(string categoryName)
        {
            categorys[categoryName].Locked = false;
        }

        public int LockCategoryCounter(int sumLockCategorys)
        {
            foreach (KeyValuePair<string, Category> category in categorys)
            {
                if (category.Value.Locked)
                {
                    sumLockCategorys++;
                }
            }

            return sumLockCategorys;
        }

        #endregion

        #endregion

        #region UC3 Methods

        public int ReleventCategoryCounter(bool rabelSign, bool fortSign, ref Dictionary<string, bool> releventCategoryNames)
        {                        
            int sumOfReleventCategory = 0;           
            bool rabelCheck = rabelSign && !fortSign && !IsActiveGolden();                      

            if (IsGolden() && !rabelCheck)
            {                               
                sumOfReleventCategory = 3;
                releventCategoryNames["food"] = true;
                releventCategoryNames["house"] = true;
                releventCategoryNames["education"] = true;
            }
            else
            {                
                foreach (KeyValuePair<string, Category> category in categorys)
                {
                    if (category.Value.Satisfaction && (rabelCheck ? !category.Value.Locked : true))
                    {
                        sumOfReleventCategory++;
                        releventCategoryNames[category.Key] = true;
                    }
                }
            }
            
            return sumOfReleventCategory;        
        }
        
        public int EffectivenessCheck(int sumOfReleventCategory, Dictionary<string, bool> releventCategoryNames)
        {            
            int sumEffectiveness = 0;
            if (OnwerOfFactory)
            {               
                sumEffectiveness += 2;

                if (sumOfReleventCategory == 3)
                {                    
                    sumEffectiveness += 4;
                }
                else
                {
                    foreach (KeyValuePair<string,bool> releventCategory in releventCategoryNames)
                    {
                        if (releventCategory.Value)
                        {
                            switch (releventCategory.Key)
                            {
                                case "food":
                                case "house":
                                    sumEffectiveness++;
                                    break;
                                case "education":
                                    sumEffectiveness += 2;
                                    break;
                                default:
                                    GameManager.instance.PlayErrorSound(false);
                                    Debug.Log("ERROR from EffectivenessCheck");
                                    break;
                            }
                        }
                    }                   
                }
            }

            return sumEffectiveness;
        }

        public int AddCitizenToGruop(int newCitizen)
        {
            int returnValue = 0;
            NumOfCitizenInGroup += newCitizen;
            if (NumOfCitizenInGroup > 500)
            {
                returnValue = NumOfCitizenInGroup - 500;
                NumOfCitizenInGroup -= returnValue;
            }
            return returnValue;
        }

        #endregion

    }
}